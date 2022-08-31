﻿using Microsoft.AspNet.Identity;
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
                ViewData["Message"] = "User Created";
                return Redirect("/");
            }
            ViewData["Message"] = "Registration failed";
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
                ViewData["Message"] = "Logged successfully";
                return Redirect("/");
            }
            ViewData["Message"] = "Email or password incorrect";
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
        [Authorize(Roles = "Admin")]
        public ActionResult Role()
        {
            return View();
        }

        [HttpPost("role")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> Role(RoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _roleService.CreateRole(dto);

            if (result)
            {
                ViewData["Message"] = "Role Created";
                return Redirect("/");
            }
            ViewData["Message"] = "Creating role failed";
            return View(dto);
        }

        [HttpGet("/profile")]
        [Authorize]
        public async Task<ActionResult> Profile()
        {
            var userId = User?.Identity?.GetUserId();

            if(userId is null) return View("/Views/NotFound.cshtml");
            var user = await _accountService.GetUserById(userId);

            if (user is null) return View("/Views/NotFound.cshtml");

            return View(user);
        }

        [HttpPost("/profile/{userId}/update")]
        [Authorize]
        public async Task<ActionResult> Profile([FromForm] UserModel userModel, [FromRoute] string userId)
        {
            if (!ModelState.IsValid)
            {
                return View(userModel);
            }

            var userUpdated = await _accountService.UpdateUser(userModel, userId);

            if(userUpdated)
            {
                ViewData["message"] = "User Updated Successfully";
            }
            else
            {
                ViewData["message"] = "User Update Failed";
            }
            return View(userModel);
        }

        [HttpGet("users")]
        [Authorize(Roles = "Owner")]
        public ActionResult Users()
        {
            var users = _accountService.GetAllUsers();

            ViewData["users"] = users;
            return View();
        }

        [HttpPost("users/{userId}")]
        [Authorize(Roles = "Owner")]
        public async Task<ActionResult> Users(RoleDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View(dto);
            }
            var result = await _roleService.CreateRole(dto);

            if (result)
            {
                ViewData["Message"] = "Role Created";
                return Redirect("/");
            }
            ViewData["Message"] = "Creating role failed";
            return View(dto);
        }
    }
}
