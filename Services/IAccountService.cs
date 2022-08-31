using Models.Dtos;

namespace Services
{
    public interface IAccountService
    {
        Task<bool> Login(LoginUserDto dto);
        Task Logout();
        Task<bool> Register(RegisterUserDto dto);
        Task<UserModel> GetUserById(string id);
        Task<bool> UpdateUser(UserModel model, string id);
        List<UserModel> GetAllUsers();
        Task<bool> DeleteUser(string userId);

    }
}