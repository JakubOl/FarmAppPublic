
namespace Services
{
    public interface IAuctionService
    {
        Task<bool> CreateAuction(ItemDto dto, string userId);
        Task<bool> DeleteAuction(string auctionId, string userId);
        PagedResult<ItemModel> GetAuctions(Query query, bool admin = false, string userId = "");
        ItemModel GetAuction(string auctionId);
        Task<bool> UpdateAuction(string auctionId, ItemDto dto, string userId);
        List<CategoryModel> GetAllCategories();
        Task<bool> CreateCategory(CategoryModel category);
        Task<bool> DeleteCategory(string categoryId);
    }
}