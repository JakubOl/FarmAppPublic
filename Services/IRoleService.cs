namespace Services
{
    public interface IRoleService
    {
        Task<bool> CreateRole(RoleDto dto);
    }
}