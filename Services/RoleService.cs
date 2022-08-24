using Microsoft.AspNetCore.Identity;

namespace Services
{
    public class RoleService : IRoleService
    {
        private RoleManager<RoleModel> _roleManager;

        public RoleService(RoleManager<RoleModel> roleManager)
        {
            _roleManager = roleManager;
        }

        public async Task<bool> CreateRole(RoleDto dto)
        {
            IdentityResult result = await _roleManager.CreateAsync(new RoleModel()
            {
                Name = dto.RoleName
            });
            if (result.Succeeded)
            {
                return true;
            }
            return false;
        }
    }
}
