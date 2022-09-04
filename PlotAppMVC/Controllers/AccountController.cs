using Microsoft.AspNet.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Models.Dtos;
using Models.Entities;
using Services;

namespace PlotAppMVC.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IRoleService _roleService;

        public AccountController(IAccountService accountService, IRoleService roleService)
        {
            _accountService = accountService;
            _roleService = roleService;
        }

        [HttpGet("register")]
        public ActionResult Register()
        {
            return View();  
        }

        [HttpPost("register")]
        public async Task<ActionResult> Register([FromForm] RegisterUserDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _accountService.Register(dto);

            if (result)
            {
                TempData["Success"] = "User Created";
                return Redirect("/");
            }
            TempData["Error"] = "Registration failed";
            return View(dto);
        }

        [HttpGet("login")]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<ActionResult> Login(LoginUserDto dto)
        {
            
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _accountService.Login(dto);

            if (result)
            {
                TempData["Success"] = "Logged successfully";
                return Redirect("/");
            }
            TempData["Error"] = "Email or password incorrect";
            return View(dto);
        }

        [HttpGet("logout")]
        [Authorize]
        public async Task<ActionResult> Logout()
        {
            await _accountService.Logout();
            return Redirect("/login");
        }

        [HttpGet("role")]
        [Authorize(Roles = "Admin,Owner")]
        public ActionResult Role()
        {
            var result = _roleService.GetAllRoles();

            ViewData["roles"] = result;

            return View();
        }

        [HttpPost("role")]
        [Authorize(Roles = "Admin,Owner")]
        public async Task<ActionResult> Role(RoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _roleService.CreateRole(dto);

            var roles = _roleService.GetAllRoles();

            ViewData["roles"] = roles;

            if (result)
            {
                TempData["Success"] = "Role Created";
                return View();
            }
            TempData["Error"] = "Creating role failed";
            return View(dto);
        }

        [HttpPost("/role/{roleId}")]
        public async Task<ActionResult> DeleteRole([FromRoute] string roleId)
        {
            var result = await _roleService.DeleteRoleById(roleId);

            var roles = _roleService.GetAllRoles();

            ViewData["roles"] = roles;

            if (result)
            {
                TempData["Success"] = "Role Deleted";
                return View("Role");
            }
            TempData["Error"] = "Deleting role failed";
            return View("Role");
        }

        [HttpGet("/profile/{userId}")]
        [Authorize]
        public async Task<ActionResult> Profile([FromRoute] string userId)
        {
            if (userId != User?.Identity?.GetUserId() && User?.IsInRole("Owner") == false)
            {
                return Redirect("/");
            }

            if (userId is null) return View("/Views/NotFound.cshtml");

            var user = await _accountService.GetUserById(userId);

            if (user is null) return View("/Views/NotFound.cshtml");

            ViewData["roles"] = _accountService.GetRoles();

            return View(user);
        }

        [HttpPost("/profile/{userId}/update")]
        [Authorize]
        public async Task<ActionResult> Profile([FromForm] UserModel userModel, [FromRoute] string userId)
        {
            if (userId != User?.Identity?.GetUserId() && User?.IsInRole("Owner") == false)
            {
                return Redirect("/");
            }

            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var userUpdated = await _accountService.UpdateUser(userModel, userId);

            if(userUpdated is not null)
            {
                TempData["Success"] = "User Updated Successfully";
            }
            else
            {
                TempData["Error"] = "User Update Failed";
            }

            ViewData["roles"] = _accountService.GetRoles();

            return View(userUpdated);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Owner")]
        public ActionResult Users([FromQuery] UsersQuery query)
        {
            var users = _accountService
                .GetAllUsers(query);

            var roles = _accountService.GetRoles();

            if(users is not null && roles is not null)
            {
                ViewData["roles"] = roles;
                ViewData["users"] = users;
            }
            return View();
        }

        [HttpGet("users/{userId}/delete")]
        [Authorize(Roles = "Owner")]
        public ActionResult ConfirmDeleteUser([FromRoute] string userId)
        {
            var user = _accountService.GetUserById(userId);

            if (user is not null)
            {
                ViewData["user"] = user;
                return View();
            }

            TempData["Error"] = "User not found";
            return Redirect("/users");
        }

        [HttpPost("users/{userId}/delete/confirmed")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> DeleteUser([FromRoute] string userId)
        {
            var result = await _accountService.DeleteUser(userId);

            if (result)
            {
                TempData["Success"] = "User Deleted";
            }
            else
            {
                TempData["Error"] = "User delete failed";
            }

            return Redirect("/users");
        }
    }
}
