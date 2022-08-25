

using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class PlotService : IPlotService
    {
        private readonly IDbConnection _db;
        private readonly IMemoryCache _cache;
        private readonly UserManager<UserModel> _user;
        private readonly IMongoCollection<PlotModel> _plots;
        private const string CacheName = "PlotData";

        public PlotService(IDbConnection db, IMemoryCache cache, UserManager<UserModel> user)
        {
            _db = db;
            _cache = cache;
            _user = user;
            _plots = db.PlotCollection;
        }

        public async Task<List<PlotModel>> GetAllPlots()
        {
            var output = _cache.Get<List<PlotModel>>(CacheName);
            if (output is null)
            {
                var result = await _plots.FindAsync(_ => true);
                output = result.ToList();
                _cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
            }
            return output;
        }

        public async Task<List<PlotModel>> GetUsersPlots(string userId)
        {
            var output = _cache.Get<List<PlotModel>>(userId);
            if (output is null)
            {
                var user = await _user.FindByIdAsync(userId);
                var userPlotsId = user.PlotsIds;
                if (userPlotsId is null) return null;
                var plots = await GetAllPlots();
                var userPlots = plots.Where(p => userPlotsId.Contains(p.Id.ToString()));
                output = userPlots.ToList();

                _cache.Set(userId, output, TimeSpan.FromMinutes(1));
            }

            return output;
        }

        public async Task<PlotModel> GetPlot(string id)
        {
            //var results = await _plots.FindAsync(s => s.Id == id);
            //return results.FirstOrDefault();
            throw new NotImplementedException();
        }

        public async Task UpdateSuggestion(PlotModel plot)
        {
            await _plots.ReplaceOneAsync(s => s.Id == plot.Id, plot);
            _cache.Remove(CacheName);
        }

        public async Task CreatePlot(PlotModel plot, string userId)
        {
            var client = _db.Client;

            _cache.Remove(userId);

            plot.Area = plot.Area.Replace(",", ".");

            using var session = await client.StartSessionAsync();

            session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var plotInTransaction = db.GetCollection<PlotModel>(_db.PlotCollectionName);
                await plotInTransaction.InsertOneAsync(plot);

                var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _user.FindByIdAsync(userId);

                if (user.PlotsIds is null)
                {
                    user.PlotsIds = new List<string>()
                    {
                        plot.Id
                    };
                }
                else
                {
                    user.PlotsIds.Add(plot.Id);
                }

                await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);


                await session.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task DeletePlot(string plotId, string userId)
        {
            var client = _db.Client;

            _cache.Remove(userId);

            using var session = await client.StartSessionAsync();

            session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var plotInTransaction = db.GetCollection<PlotModel>(_db.PlotCollectionName);
                await plotInTransaction.DeleteOneAsync(plot => plot.Id == plotId);

                var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _user.FindByIdAsync(userId);
                user.PlotsIds.Remove(plotId);
                await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

                await session.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}
