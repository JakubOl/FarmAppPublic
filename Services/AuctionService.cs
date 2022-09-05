using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IDbConnection _db;
        private IMongoCollection<TypeModel> _types;
        private IMongoCollection<ItemModel> _auctions;
        private readonly UserManager<UserModel> _user;


        public AuctionService(IDbConnection db, UserManager<UserModel> user)
        {
            _db = db;
            _user = user;
            _auctions = db.AuctionCollection;
            _types = db.TypeCollection;

        }
        public List<ItemModel> GetAllAuctions()
        {
            var results = _auctions.Find(_ => true);
            return results.ToList();
        }

        public ItemModel GetAuction(string auctionId)
        {
            var result = _auctions.Find(a => a.Id == auctionId).FirstOrDefault();
            return result;
        }

        public async Task<bool> CreateAuction(ItemDto dto, string userId)
        {
            var client = _db.Client;

            var auction = new ItemModel()
            {
                AuthorId = userId,
                Description = dto.Description,
                IsActive = dto.IsActive,
                Price = dto.Price,
                ProductionDate = dto.ProductionDate,
                Title = dto.Title,
                TypeId = dto.Type,
            };

            auction.Type = (await _types.FindAsync(t => t.Id == auction.TypeId)).FirstOrDefault();

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var auctionInTransaction = db.GetCollection<ItemModel>(_db.AuctionCollectionName);
                await auctionInTransaction.InsertOneAsync(auction);

                var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _user.FindByIdAsync(userId);

                user.Auctions.Add(auction.Id);

                await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

                //await session.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                return false;
            }
        }

        public async Task<bool> DeleteAuction(string auctionId, string userId)
        {
            var client = _db.Client;

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var auctionInTransaction = db.GetCollection<ItemModel>(_db.AuctionCollectionName);
                await auctionInTransaction.DeleteOneAsync(a => a.Id == auctionId);

                var usersInTransaction = db.GetCollection<UserModel>(_db.UserCollectionName);
                var user = await _user.FindByIdAsync(userId);
                user.Auctions.Remove(auctionId);
                await usersInTransaction.ReplaceOneAsync(u => u.Id == user.Id, user);

                //await session.CommitTransactionAsync();
                return true;
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                return false;
            }
        }

        public List<TypeModel> GetAllTypes()
        {
            var results = _types.Find(_ => true);
            return results.ToList();
        }

        public async Task<bool> CreateType(TypeModel type)
        {
            var client = _db.Client;

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var typeInTransaction = db.GetCollection<TypeModel>(_db.TypeCollectionName);
                await typeInTransaction.InsertOneAsync(type);

                //await session.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                return false;
            }
        }

        public async Task<bool> DeleteType(string typeId)
        {
            var client = _db.Client;

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var typeInTransaction = db.GetCollection<TypeModel>(_db.TypeCollectionName);
                await typeInTransaction.DeleteOneAsync(t => t.Id == typeId);

                //await session.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                return false;
            }
        }
    }
}
