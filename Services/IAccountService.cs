using Models.Dtos;

namespace Services
{
    public interface IAccountService
    {
        Task<bool> Login(LoginUserDto dto);
        Task Logout();
        Task<bool> Register(RegisterUserDto dto);
        Task<UserModel> GetUserById(string id);
        Task<UserModel> UpdateUser(UserModel model, string id);
        PagedResult<UserModel> GetAllUsers(Query query);
        Task<bool> DeleteUser(string userId);
        List<RoleModel> GetRoles();
    }
}