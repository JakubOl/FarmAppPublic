using Microsoft.Extensions.Configuration;
using Models.Entities;
using MongoDB.Driver;

namespace DataAccess;
public class DbConnection : IDbConnection
{
    private readonly IConfiguration _config;
    private readonly IMongoDatabase _db;
    private string _connectionId = "MongoDB";
    public string DbName { get; private set; }

    public string PlotCollectionName { get; private set; } = "plots";
    public string RoleCollectionName { get; private set; } = "roleModels";
    public string UserCollectionName { get; private set; } = "userModels";
    public string TillageCollectionName { get; private set; } = "tillages";
    public string WorkCollectionName { get; private set; } = "works";

    public MongoClient Client { get; private set; }
    public IMongoCollection<PlotModel> PlotCollection { get; private set; }
    public IMongoCollection<RoleModel> RoleCollection { get; private set; }
    public IMongoCollection<UserModel> UserCollection { get; private set; }
    public IMongoCollection<TillageModel> TillageCollection { get; private set; }
    public IMongoCollection<WorkModel> WorkCollection { get; private set; }


    public DbConnection(IConfiguration config)
    {
        _config = config;
        Client = new MongoClient(_config.GetConnectionString(_connectionId));
        DbName = _config["DatabaseName"];
        _db = Client.GetDatabase(DbName);



        PlotCollection = _db.GetCollection<PlotModel>(PlotCollectionName);
        RoleCollection = _db.GetCollection<RoleModel>(RoleCollectionName);
        UserCollection = _db.GetCollection<UserModel>(UserCollectionName);
        TillageCollection = _db.GetCollection<TillageModel>(TillageCollectionName);
        WorkCollection = _db.GetCollection<WorkModel>(WorkCollectionName);

    }
}