using DataAccess;
using Models.Entities;
using Services;

namespace PlotAppMVC
{
    public static class RegisterServices
    {

        public static void ConfigureServices(this WebApplicationBuilder builder)
        {

            builder.Services.AddControllersWithViews();

            builder.Services.AddMemoryCache();

            builder.Services.AddAuthenticationCore();

            builder.Services.AddSingleton<IDbConnection, DbConnection>();
            var connectionString = builder.Configuration.GetConnectionString("MongoDB");
            var databaseName = builder.Configuration.GetSection("myplotapp").Key;
            builder.Services.AddIdentity<UserModel, RoleModel>().AddMongoDbStores<UserModel, RoleModel, Guid>(connectionString, databaseName);

            builder.Services.AddTransient<IAccountService, AccountService>();
            builder.Services.AddTransient<IPlotService, PlotService>();
            builder.Services.AddTransient<IRoleService, RoleService>();

        }
    }
}
