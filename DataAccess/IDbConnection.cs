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
    }
}