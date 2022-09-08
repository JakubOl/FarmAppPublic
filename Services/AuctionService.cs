using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class AuctionService : IAuctionService
    {
        private readonly IDbConnection _db;
        private IMongoCollection<CategoryModel> _categories;
        private IMongoCollection<ItemModel> _auctions;
        private readonly UserManager<UserModel> _user;


        public AuctionService(IDbConnection db, UserManager<UserModel> user)
        {
            _db = db;
            _user = user;
            _auctions = db.AuctionCollection;
            _categories = db.CategoryCollection;
        }
        public PagedResult<ItemModel> GetAuctions(Query query, bool admin = false, string userId = "")
        {
            
            var results = _auctions.Find(_ => true).ToList();

            if (!admin)
            {
                if (!string.IsNullOrWhiteSpace(userId))
                {
                    results = results.Where(a => a.AuthorId == userId).ToList();
                }
                else
                {
                    results = results.Where(a => a.IsActive == true).ToList();
                }
            }

            if (!string.IsNullOrWhiteSpace(query.SearchPhrase))
            {
                results = results.Where(r => r.Description.Contains(query.SearchPhrase, StringComparison.OrdinalIgnoreCase)
                || r.Title.Contains(query.SearchPhrase, StringComparison.OrdinalIgnoreCase)
                || r.Category.Name.Contains(query.SearchPhrase, StringComparison.OrdinalIgnoreCase))
               .ToList();
            }

            if (query.PageNumber < 1)
            {
                query.PageNumber = (int)Math.Ceiling(results.Count / (double)query.PageSize);
            }

            if (query.PageNumber > (int)Math.Ceiling(results.Count / (double)query.PageSize))
            {
                query.PageNumber = 1;
            }


            var pagedResult = results.Skip((query.PageNumber - 1) * query.PageSize).Take(query.PageSize).ToList();

            var totalItems = results.Count;

            var result = new PagedResult<ItemModel>(pagedResult, totalItems, query.PageSize, query.PageNumber, query.SearchPhrase);

            return result;
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
                Title = dto.Title,
                CategoryId = dto.CategoryId,
                ImageName = dto.ImageName
            };

            auction.Category = (await _categories.FindAsync(t => t.Id == auction.CategoryId)).FirstOrDefault();

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
                var deletedAuction = await auctionInTransaction.DeleteOneAsync(a => a.Id == auctionId);

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

        public async Task<bool> UpdateAuction(string auctionId, ItemDto dto, string userId)
        {
            var auctionEntity = GetAuction(auctionId);

            auctionEntity.Title = dto.Title;
            auctionEntity.Price = dto.Price;
            auctionEntity.Description = dto.Description;
            auctionEntity.IsActive = dto.IsActive;
            auctionEntity.CategoryId = dto.CategoryId;
            auctionEntity.Category = (await _categories.FindAsync(t => t.Id == dto.CategoryId)).FirstOrDefault();

            var result = await _auctions.ReplaceOneAsync(a => a.Id == auctionId, auctionEntity);

            if (result.ModifiedCount > 0)
            {
                return true;
            }
            return false;
        }

        public List<CategoryModel> GetAllCategories()
        {
            var results = _categories.Find(_ => true);
            return results.ToList();
        }

        public async Task<bool> CreateCategory(CategoryModel category)
        {
            var client = _db.Client;

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var categoryInTransaction = db.GetCollection<CategoryModel>(_db.CategoryCollectionName);
                await categoryInTransaction.InsertOneAsync(category);

                //await session.CommitTransactionAsync();

                return true;
            }
            catch (Exception ex)
            {
                //await session.AbortTransactionAsync();
                return false;
            }
        }

        public async Task<bool> DeleteCategory(string categoryId)
        {
            var client = _db.Client;

            //using var session = await client.StartSessionAsync();

            //session.StartTransaction();

            try
            {
                var db = client.GetDatabase(_db.DbName);
                var categoryInTransaction = db.GetCollection<CategoryModel>(_db.CategoryCollectionName);
                await categoryInTransaction.DeleteOneAsync(t => t.Id == categoryId);

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
