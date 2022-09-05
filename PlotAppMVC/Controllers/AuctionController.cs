using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Entities;
using Services;

namespace PlotAppMVC.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionService _auctionService;

        public AuctionController(IAuctionService auctionService)
        {
            _auctionService = auctionService;
        }

        [HttpGet("/auction")]
        public ActionResult Index()
        {
            var auctions = _auctionService.GetAllAuctions();

            ViewData["auctions"] = auctions;
            return View();
        }

        [HttpGet("/auction/user")]
        public ActionResult UserAuctions()
        {
            var userId = User?.Identity?.GetUserId();

            var auctions = _auctionService.GetAllAuctions().Where(a => a.AuthorId == userId).ToList();

            ViewData["auctions"] = auctions;
            return View("UserAuctions");
        }

        // GET: AuctionController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        [HttpGet("/auction/create")]
        public ActionResult Create()
        {
            var types = _auctionService.GetAllTypes();
            ViewData["types"] = types;
            return View();
        }

        // POST: AuctionController/Create
        [HttpPost("/auction/create")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Create(ItemDto dto)
        {
            var types = _auctionService.GetAllTypes();
            ViewData["types"] = types;

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            var userId = User?.Identity?.GetUserId();

            var auction = await _auctionService.CreateAuction(dto, userId);

            if(auction)
            {
                TempData["Success"] = "Auction added";
                return Redirect("/auction");
            }

            TempData["Error"] = "Auction creating failed";
            return View(dto);
        }

        // GET: AuctionController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: AuctionController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [HttpGet("/auction/{auctionId}/delete")]
        public ActionResult DeleteConfirm([FromRoute]string auctionId)
        {
            var auction = _auctionService.GetAuction(auctionId);
            ViewData["auction"] = auction;

            return View("ConfirmDeleteAuction");
        }

        [HttpPost("/auction/{auctionId}/delete/confirmed")]
        public async Task<ActionResult> Delete([FromRoute] string auctionId)
        {
            var userId = User?.Identity?.GetUserId();
            var deletedAuction = await _auctionService.DeleteAuction(auctionId, userId);

            if (deletedAuction)
            {
                TempData["Success"] = "Auction Deleted";
            }
            else
            {
                TempData["Error"] = "Auction delete failed";
            }

            return Redirect("/auction/user");
        }

        [HttpGet("/auction/type")]
        public ActionResult Types()
        {
            var result = _auctionService.GetAllTypes();
            ViewData["types"] = result;

            return View("Types");
        }

        [HttpPost("/auction/type/create")]
        public async Task<ActionResult> CreateType([FromForm] TypeModel type)
        {
            var result = _auctionService.GetAllTypes();
            ViewData["types"] = result;

            if (!ModelState.IsValid)
            {
                return View("Types", type);
            }
            var newType = await _auctionService.CreateType(type);

            if (newType)
            {
                TempData["Success"] = "Type created";
                return Redirect("/auction/type");
            }

            TempData["Error"] = "Type creating failed";
            return View("Types");
        }

        [HttpPost("/auction/type/{typeId}")]
        public async Task<ActionResult> DeleteType([FromRoute]string typeId)
        {
            var deletedType = await _auctionService.DeleteType(typeId);

            var result = _auctionService.GetAllTypes();
            ViewData["types"] = result;

            if (deletedType)
            {
                TempData["Success"] = "Type deleted";
                return Redirect("/auction/type");
            }

            TempData["Error"] = "Type deleting failed";
            return View("Types");
        }
    }
}
