using Models.Entities;
using MongoDB.Driver;

namespace DataAccess
{
	public interface IDbConnection
    {
        MongoClient Client { get; }
        string DbName { get; }
        IMongoCollection<PlotModel> PlotCollection { get; }
        string PlotCollectionName { get; }
        IMongoCollection<RoleModel> RoleCollection { get; }
        string RoleCollectionName { get; }
        IMongoCollection<UserModel> UserCollection { get; }
        string UserCollectionName { get; }
        IMongoCollection<ItemModel> AuctionCollection { get; }
        string AuctionCollectionName { get; }
        IMongoCollection<CategoryModel> TypeCollection { get; }
        string TypeCollectionName { get; }
        IMongoCollection<CommentModel> CommentCollection { get; }
        string CommentCollectionName { get; }
    }
}