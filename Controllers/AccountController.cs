﻿using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HogeschoolPXL.Data;
using Microsoft.AspNetCore.Authorization;
using System.Data;

namespace HogeschoolPXL.Controllers
{
    public class AccountController : Controller
	{
		#region dependency injection
		AppDbContext _context;
        UserManager<IdentityUser> _userManager;
		SignInManager<IdentityUser> _signInManager;
		RoleManager<IdentityRole> _roleManager;
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager,
            RoleManager<IdentityRole> roleManager, AppDbContext context)
		{
			_userManager = userManager;
			_signInManager = signInManager;
			_roleManager = roleManager;
            _context = context;
		}
		#endregion

		#region login
		[HttpGet]
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		// returnUrl works but has a problem with the Popup Toast after login that appears on the Home Page only
		// Uncomment the commented lines below to test returnUrl
		public async Task<IActionResult> LoginAsync(LoginViewModel login/*, string returnUrl*/)
        {
            if (login.Email == null || login.Password == null)
                return View("Login");

            var signInResult = await _signInManager.PasswordSignInAsync(
			login.Email, login.Password, false, lockoutOnFailure: false);

			if (signInResult.Succeeded)
			{
                TempData["Login"] = "Succeeded";
                //if (!string.IsNullOrEmpty(returnUrl))
                //    return Redirect(returnUrl);
                //else
					return RedirectToAction("Index", "Home");
            }
			else
			{
                ModelState.AddModelError("", "Something went wrong");
                return View("Login");
            }
		}
		#endregion

        #region register
        [HttpGet]
		public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> RegisterAsync(RegisterViewModel register)
		{
			var identityUser = new IdentityUser
			{
				Email = register.Email,
				UserName = register.Email
			};
			var identityResult = await _userManager.CreateAsync(identityUser, register.Password);

			if (identityResult.Succeeded)
				return View("RegistrationCompleted");
			foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

			return View();
		}
		#endregion

		#region logout
		[HttpGet]
		public async Task<IActionResult> LogOut()
		{
            TempData["Login"] = "LogOut";

            await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home"); // RedirectToAction Belangrijk!!
		}
        #endregion

        #region create roles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult CreateRoles()
        {
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> CreateRolesAsync(RoleViewModel role)
        {
            if (!await _roleManager.RoleExistsAsync(role.RoleName))
            {
                var identityRole = new IdentityRole(role.RoleName);
                var result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    TempData["Alert"] = "RoleCreated";
                    return RedirectToAction("Identity");
                }
            }
            ViewBag.Roles = _roleManager.Roles;
            ModelState.AddModelError("", $"Role ({role.RoleName}) already exists");
            return View();
        }
        #endregion

        #region delete roles
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult DeleteRoles()
        {
            ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Name));
            return View();
        }
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteRolesAsync(RoleViewModel role)
        {
            if (await _roleManager.RoleExistsAsync(role.RoleName))
            {
                var result = _context.Roles.Where(x => x.Name == role.RoleName).FirstOrDefault();
                if (result != null)
                {
                    _context.Roles.Remove(result);
                    _context.SaveChanges();

                    TempData["Alert"] = "RoleDeleted";
                    return RedirectToAction("Identity");
                }
            }
            ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Name));
            ModelState.AddModelError("", "Problem with deleting role");
            return View();
        }
        #endregion

        #region manage user roles
        public IActionResult ManageUserRoles()
        {
            ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
            ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));

            return View();
        }
        #endregion

        #region form handler
        /* Because our form has 2 submit buttons, it goes through this method to check which button was pressed */
        [Authorize(Roles = "Admin")]
        public Task<IActionResult> FormHandler(string submitButton, string roles, string users)
        {
            if (submitButton == "Add Role")
            {
                return AddUserRoleAsync(roles, users);
            }
            else if (submitButton == "Remove Role")
            {
                return RemoveUserRoleAsync(roles, users);
            }
            ModelState.AddModelError("", "Something went wrong");
            return Task.FromResult((IActionResult)View("ManageUserRoles"));
        }
        #endregion

        #region assign role
		[HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddUserRoleAsync(string roles, string users)
		{
            // Get the user and role by their respective ids
            var user = await _userManager.FindByIdAsync(users.ToString());
            var role = await _roleManager.FindByIdAsync(roles.ToString());

            if (user == null || role == null)
            {
                return BadRequest();
            }

            // Gets all users in selected role, needed to check if the user already has role
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            // Check if user already has role
            if (usersInRole.Any(u => u.Id == users))
            {
                ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
                ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));
                ModelState.AddModelError("", "User already has role");
                return View("ManageUserRole");
            }

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            // Check if the role was successfully assigned
            if (result.Succeeded)
            {
                ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
                ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));
				ViewBag.Alert = "RoleAssignedToUser";
				return View("ManageUserRole");
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region remove role
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> RemoveUserRoleAsync(string roles, string users)
        {
            // Get the user and role by their respective ids
            var user = await _userManager.FindByIdAsync(users.ToString());
            var role = await _roleManager.FindByIdAsync(roles.ToString());

            if (user == null || role == null)
            {
                return BadRequest();
            }

            // Gets all users in selected role, needed to check if the user already has role
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

            // Check if user doesn't have the role
            if (!usersInRole.Any(u => u.Id == users))
            {
                ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
                ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));
                ModelState.AddModelError("", "User doesn't have this role");
                return View("ManageUserRoles");
            }

            // Remove role from the user
            var result = await _userManager.RemoveFromRoleAsync(user, role.Name);

            // Check if the role was successfully assigned
            if (result.Succeeded)
            {
                ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
                ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));
                ViewBag.Alert = "RoleRemovedFromUser";
                return View("ManageUserRoles");
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region identity
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult Identity()
        {
            if (TempData["Alert"] != null)
            {
                ViewBag.Alert = TempData["Alert"];
                TempData.Remove("Alert");
            }
            var identityViewModel = new IdentityViewModel
            {
                Roles = _roleManager.Roles,
                Users = _userManager.Users
            };
            return View(identityViewModel);
        }
        #endregion

        #region acces denied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
        #endregion
    }

}
