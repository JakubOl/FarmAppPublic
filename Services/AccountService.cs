using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Models.Entities;

namespace Services
{
    public class AccountService : IAccountService
    {
        private UserManager<UserModel> _userManager;
        private readonly RoleManager<RoleModel> _roles;
        private SignInManager<UserModel> _signInManager;

        public AccountService(UserManager<UserModel> userManager, SignInManager<UserModel> signInManager, RoleManager<RoleModel> roles)
        {
            _userManager = userManager;
            _roles = roles;
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
                EmailAddress = dto.EmailAddress,
                DisplayName = dto.FirstName + " " + dto.LastName,
                PlotsIds = new List<string>()
            };


            IdentityResult result = await _userManager.CreateAsync(newUser, dto.Password);

            await _userManager.AddToRoleAsync(newUser, "User");

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
            //if (dto.Email == "jakub@jakub.com" && appUser.Roles.Count == 1)
            //{
            //    await _userManager.AddToRoleAsync(appUser, "Owner");
            //}
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
            var users = _userManager.Users.ToList();
            return users;
        }

        public async Task<UserModel> GetUserById(string id)
        {
            return await _userManager.FindByIdAsync(id);
        }

        public async Task<UserModel> UpdateUser(UserModel userDto, string id)
        {
            var user = await GetUserById(id);

            user.Address = userDto.Address;
            user.Country = userDto.Country;
            user.FirstName = userDto.FirstName;
            user.LastName = userDto.LastName;
            user.PhoneNumber = userDto.PhoneNumber;
            user.MobileNumber = userDto.MobileNumber;
            user.StateRegion = userDto.StateRegion;
            user.Email = user.Email;

            if(userDto.Role is not null)
            {
                var roleGuid = new Guid(userDto.Role);
                user.Roles[0] = roleGuid;
            }

            await _userManager.UpdateAsync(user);

            return user;
        }

        public async Task<bool> DeleteUser(string userId)
        {
            var user = await GetUserById(userId);

            if(user == null) return false;

            try
            {
                var result = await _userManager.DeleteAsync(user);
                return result.Succeeded;
            }
            catch (Exception ex)
            {
                return false;
            }
        }

        public async Task<List<RoleModel>> GetRoles()
        {
            var results = _roles.Roles.ToList();
            return results;
        }
    }
}
