namespace Services
{
    public interface IRoleService
    {
        List<RoleModel> GetAllRoles();
        Task<bool> CreateRole(RoleDto dto);
        Task<bool> DeleteRoleById(string roleId);
    }
}