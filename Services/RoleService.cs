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

        public List<RoleModel> GetAllRoles()
        {
            var result = _roleManager.Roles.ToList();
            return result;
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

        public async Task<bool> DeleteRoleById(string roleId)
        {
            var role = await _roleManager.FindByIdAsync(roleId);

            if (role == null || role.Name == "Owner")
            {
                return false;
            }

            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                return false;
            }
            return true;
        }
    }
}
