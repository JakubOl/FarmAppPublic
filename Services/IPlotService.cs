
namespace Services
{
    public interface IPlotService
    {
        Task<bool> CreatePlot(PlotDto model, string userId);
        Task<bool> DeletePlot(string plotId, string userId);
        Task<PlotModel> GetPlot(string id);
        Task<List<PlotModel>> GetUsersPlots(string userId, string searchText);
        Task<bool> UpdatePlot(string plotId, PlotModel plot, string userId);
    }
}