
namespace Services
{
    public interface IPlotService
    {
        Task CreatePlot(PlotModel plot, string userId);
        Task DeletePlot(string plotId, string userId);
        Task<List<PlotModel>> GetAllPlots();
        Task<PlotModel> GetPlot(string id);
        Task<List<PlotModel>> GetUsersPlots(string userId, string searchText);
        Task<bool> UpdatePlot(string plotId, PlotModel plot, string userId);
    }
}