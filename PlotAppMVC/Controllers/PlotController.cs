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
        public async Task<List<PlotModel>> GetUserPlots()
        {
            var plots = await _plotService.GetAllPlots();

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

            model.Latitude = plotCoordinates[0];
            model.Longitude = plotCoordinates[1];

            var userId = User.Identity.GetUserId();

            await _plotService.CreatePlot(model, userId);

            return Redirect("/");
        }

    }
}
