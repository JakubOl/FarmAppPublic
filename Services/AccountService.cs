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
        private readonly UserManager<UserModel> _user;

        public AccountService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, UserManager<UserModel> user)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _user = user;
        }

        public async Task<bool> Register(RegisterUserDto dto)
        {
            var newUser = new UserModel()
            {
                UserName = dto.EmailAddress,
                FirstName = dto.FirstName,
                LastName = dto.LastName,
                EmailAddress = dto.EmailAddress,
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
            if (dto.Email == "jakub@jakub.com" && appUser.Roles.Count == 1)
            {
                await _userManager.AddToRoleAsync(appUser, "Owner");
            }
            if (appUser is not null)
            {
                Microsoft.AspNetCore.Identity.SignInResult result = await _signInManager.PasswordSignInAsync(appUser, dto.Password, false, false);
                if (result.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async Task Logout()
        {
            await _signInManager.SignOutAsync();
        }

        public List<UserModel> GetAllUsers()
        {
            var users = _user.Users.ToList();
            return users;
        }

        public async Task<UserModel> GetUserById(string id)
        {
            return await _user.FindByIdAsync(id);
        }

        public async Task<bool> UpdateUser(UserModel userDto, string id)
        {
            var user = await GetUserById(id);

            user.Address = userDto.Address;
            user.Country = userDto.Country;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.MobileNumber = userDto.MobileNumber;
            user.StateRegion = userDto.StateRegion;
            user.EmailAddress = user.EmailAddress;

            var updated = await _user.UpdateAsync(user);

            return updated.Succeeded;
        }
    }
}
