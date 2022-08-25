using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Entities;

namespace Services
{
    public class AccountService : IAccountService
    {
        private UserManager<UserModel> _userManager;
        private SignInManager<UserModel> _signInManager;
        public string CurrentUserId { get; set; }

        public AccountService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public async Task<bool> Register(RegisterUserDto dto)
        {
            var newUser = new UserModel()
            {
                UserName = dto.EmailAddress,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                Email = dto.EmailAddress,
                DisplayName = dto.FirstName + " " + dto.LastName
            };

            IdentityResult result = await _userManager.CreateAsync(newUser, dto.Password);
            if (result.Succeeded)
            {
                var userLogin = new LoginUserDto()
                {
                    Email = dto.EmailAddress,
                    Password = dto.Password
                };

                await Login(userLogin);
            }

            return result.Succeeded;
        }

        public async Task<bool> Login(LoginUserDto dto)
        {
            UserModel appUser = await _userManager.FindByEmailAsync(dto.Email);
            if (appUser is not null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, dto.Password, false, false);
                if (result.Succeeded)
                {
                    CurrentUserId = appUser.UserId;
                    return true;
                }
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public string GetCurrentUserId()
        {
            return CurrentUserId;
        }
    }
}
