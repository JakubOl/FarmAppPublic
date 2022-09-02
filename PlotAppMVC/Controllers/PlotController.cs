using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.JSInterop;
using Microsoft.AspNet.Identity;
using Models.Entities;
using Services.ApiFiles;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;
using Models.Dtos;
using System.Diagnostics;

namespace PlotAppMVC.Controllers
{
    [Authorize]
    public class PlotController : Controller
    {
        private readonly IPlotService _plotService;
        private readonly IPlotProcessor _plotProcessor;

        public PlotController(IPlotService plotService, IPlotProcessor plotProcessor)
        {
            _plotService = plotService;
            _plotProcessor = plotProcessor;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index()
        {
            return View("Index");
        }

        [HttpGet("/plots")]
        public async Task<List<PlotModel>> GetUserPlots([FromQuery] string searchText)
        {
            var userId = User.Identity.GetUserId();
            var plots = await _plotService.GetUsersPlots(userId, searchText);

            if (plots is null) return new List<PlotModel>();
            return plots;
        }

        [HttpPost("/plot")]
        public async Task<ActionResult> AddPlot([FromForm]PlotDto model)
        {
            if (!ModelState.IsValid)
            {
                TempData["Error"] = "Invalid input";
                return View("Index", model);
            }

            var plotCoordinates = await _plotProcessor.LoadPlot(model.City, model.PlotNumber);

            if(plotCoordinates is null) {
                TempData["Error"] = "Plot not found";
                return View("Index", model);
            }

            var plot = new PlotModel()
            {
                Area = model.Area,
                PlotNumber = model.PlotNumber,
                City = model.City,
                Tillage = model.Tillage,
                Latitude = plotCoordinates[0],
                Longitude = plotCoordinates[1]
            };

            

            var userId = User.Identity.GetUserId();

            await _plotService.CreatePlot(plot, userId);

            TempData["Success"] = "Plot Added";
            return Redirect("/");
        }

        [HttpPost("/plot/{id}")]
        public async Task<ActionResult> DeletePlot([FromRoute]string id)
        {
            var userId = User.Identity.GetUserId();

            await _plotService.DeletePlot(id, userId);

            TempData["Success"] = "Plot Deleted Successfully";

            return Redirect("/");
        }

        [HttpGet("/plot/{id}/edit")]
        public async Task<ActionResult> EditPlot(string id)
        {
            var plot = await _plotService.GetPlot(id);

            return View(plot);
        }

        [HttpPost("/plot/{id}/edit")]
        public async Task<ActionResult> EditPlot([FromRoute] string id, [FromForm] PlotModel model)
        {
            model.Id = id;
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var userId = User.Identity.GetUserId();

            var plotUpdated = await _plotService.UpdatePlot(id, model, userId);

            if (plotUpdated)
            {
                TempData["Success"] = "Plot Updated Successfully";
                return View("Index");
            }

            TempData["Error"] = "Plot Update Failed";
            return View(model);
        }

        [Route("/Error")]
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
