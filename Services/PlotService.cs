

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
            //var output = _cache.Get<List<PlotModel>>(CacheName);
            //if (output is null)
            //{
                var result = await _plots.FindAsync(_ => true);
                var output = result.ToList();
                //_cache.Set(CacheName, output, TimeSpan.FromMinutes(1));
            //}
            return output;
        }

        public async Task<List<PlotModel>> GetUsersPlots(string userId, string searchText)
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
            if (!string.IsNullOrWhiteSpace(searchText))
            {
                output = output.Where(p => p.City.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || (p.Tillage is not null && p.Tillage.Contains(searchText, StringComparison.OrdinalIgnoreCase)) 
                || p.PlotNumber.Contains(searchText, StringComparison.OrdinalIgnoreCase)
                || (p.Area is not null && p.Area.Contains(searchText, StringComparison.OrdinalIgnoreCase)))
                .ToList();
            }

            return output;
        }

        public async Task<PlotModel> GetPlot(string id)
        {
            var result = await _plots.FindAsync(s => s.Id == id);
            return result.FirstOrDefault();
        }

        public async Task<bool> UpdatePlot(string plotId, PlotModel plot, string userId)
        {
            var plotEntity = await GetPlot(plotId);

            plotEntity.City = plot.City;
            plotEntity.Tillage = plot.Tillage;
            plotEntity.Area = plot.Area;

            var result = await _plots.ReplaceOneAsync(p => p.Id == plotId, plotEntity);

            _cache.Remove(userId);

            if(result.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }

        //public async Task CreatePlot(PlotModel plot, string userId)
        //{
        //    var client = _db.Client;

        //    _cache.Remove(userId);

        //    plot.Area = plot.Area.Replace(",", ".");

        //    using var session = await client.StartSessionAsync();

        //    session.StartTransaction();

        //    try
        //    {
        //        var db = client.GetDatabase(_db.DbName);
        //        var plotInTransaction = db.GetCollection<PlotModel>(_db.PlotCollectionName);
        //        await plotInTransaction.InsertOneAsync(plot);

        //        var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
        //        var user = await _user.FindByIdAsync(userId);

        //        if (user.PlotsIds is null)
        //        {
        //            user.PlotsIds = new List<string>()
        //            {
        //                plot.Id
        //            };
        //        }
        //        else
        //        {
        //            user.PlotsIds.Add(plot.Id);
        //        }

        //        await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

        //        await session.CommitTransactionAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        await session.AbortTransactionAsync();
        //        throw;
        //    }
        //}
   
        //public async Task DeletePlot(string plotId, string userId)
        //{
        //    var client = _db.Client;

        //    _cache.Remove(userId);

        //    using var session = await client.StartSessionAsync();

        //    session.StartTransaction();

        //    try
        //    {
        //        var db = client.GetDatabase(_db.DbName);
        //        var plotInTransaction = db.GetCollection<PlotModel>(_db.PlotCollectionName);
        //        await plotInTransaction.DeleteOneAsync(plot => plot.Id == plotId);

        //        var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
        //        var user = await _user.FindByIdAsync(userId);
        //        user.PlotsIds.Remove(plotId);
        //        await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

        //        await session.CommitTransactionAsync();
        //    }
        //    catch (Exception ex)
        //    {
        //        await session.AbortTransactionAsync();
        //        throw;
        //    }
        //}

        public async Task CreatePlot(PlotModel plot, string userId)
        {
            var client = _db.Client;

            _cache.Remove(userId);

            plot.Area = plot.Area.Replace(",", ".");

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

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

                _cache.Remove(userId);
                //await session.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                throw;
            }
        }

        public async Task DeletePlot(string plotId, string userId)
        {
            var client = _db.Client;

            _cache.Remove(userId);

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var plotInTransaction = db.GetCollection<PlotModel>(_db.PlotCollectionName);
                await plotInTransaction.DeleteOneAsync(plot => plot.Id == plotId);

                var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _user.FindByIdAsync(userId);
                user.PlotsIds.Remove(plotId);
                await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

                //await session.CommitTransactionAsync();
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                throw;
            }
        }
    }
}
