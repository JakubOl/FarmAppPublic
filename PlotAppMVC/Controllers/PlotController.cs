using Microsoft.AspNetCore.Mvc;
using Services;
using Microsoft.JSInterop;
using Microsoft.AspNet.Identity;
using Models.Entities;
using Services.ApiFiles;
using Microsoft.AspNetCore.Authorization;
using MongoDB.Bson;

namespace PlotAppMVC.Controllers
{
    [Authorize]
    public class PlotController : Controller
    {
        private readonly IPlotService _plotService;
        private readonly IJSRuntime _js;
        private readonly IAccountService _accountService;

        public PlotController(IPlotService plotService, IJSRuntime js, IAccountService accountService)
        {
            _plotService = plotService;
            _js = js;
            _accountService = accountService;
        }

        [AllowAnonymous]
        [HttpGet]
        public async Task<ActionResult> Index()
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
        public async Task<ActionResult> AddPlot([FromForm]PlotModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var plotCoordinates = await PlotProcessor.LoadPlot(model.City, model.PlotNumber);

            if(plotCoordinates is null) {
                ViewData["message"] = "Plot not found";
                return View("Index", model);
            }
            model.Latitude = plotCoordinates[0];
            model.Longitude = plotCoordinates[1];

            var userId = User.Identity.GetUserId();

            await _plotService.CreatePlot(model, userId);

            return Redirect("/");
        }

        [HttpPost("/plot/{id}")]
        public async Task<ActionResult> DeletePlot([FromRoute]string id)
        {
            var userId = User.Identity.GetUserId();

            await _plotService.DeletePlot(id, userId);
            return Redirect("/");
        }
    }
}
