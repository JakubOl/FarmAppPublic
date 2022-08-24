using Models.Dtos;

namespace Services
{
    public interface IAccountService
    {
        Task<bool> Login(LoginUserDto dto);
        Task Logout();
        Task<bool> Register(RegisterUserDto dto);
        string GetCurrentUserId();
    }
}