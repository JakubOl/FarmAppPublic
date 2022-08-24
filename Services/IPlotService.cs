
namespace Services
{
    public interface IPlotService
    {
        Task CreatePlot(PlotModel plot, ObjectId userId);
        Task DeletePlot(string plotId, string userId);
        Task<List<PlotModel>> GetAllPlots();
        Task<PlotModel> GetPlot(string id);
        Task<List<PlotModel>> GetUsersPlots(string userId);
        Task UpdateSuggestion(PlotModel plot);
    }
}