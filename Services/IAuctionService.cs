
namespace Services
{
    public interface IAuctionService
    {
        Task<bool> CreateAuction(ItemDto dto, string userId);
        Task<bool> DeleteAuction(string auctionId, string userId);
        List<ItemModel> GetAllAuctions();
        ItemModel GetAuction(string auctionId);

        List<TypeModel> GetAllTypes();
        Task<bool> CreateType(TypeModel type);
        Task<bool> DeleteType(string typeId);
    }
}