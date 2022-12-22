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
using NuGet.Protocol.Plugins;
using HogeschoolPXL.Data.DefaultData;

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
		public async Task<IActionResult> LoginAsync(LoginViewModel login)
        {
            if (login.Email == null || login.Password == null)
                return View("Login");

            var signInResult = await _signInManager.PasswordSignInAsync(
			login.Email, login.Password, false, lockoutOnFailure: false);

			if (signInResult.Succeeded)
			{
                TempData["Login"] = "LoginSucceeded";
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
            ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
            return View();
		}
		[HttpPost]
		public async Task<IActionResult> RegisterAsync(RegisterViewModel register)
		{
            // Create User
			var identityUser = new IdentityUser { Email = register.Email, UserName = register.Email };
			var identityResult = await _userManager.CreateAsync(identityUser, register.Password);

            var user = await _userManager.FindByEmailAsync(identityUser.ToString());
            var role = await _roleManager.FindByIdAsync(register.Roles);
            //await _userManager.AddToRoleAsync(user, register.Roles.ToString());

			if (identityResult.Succeeded)
            {
                // Add User and Role to the RoleRequest Page
                _context.RoleRequestsViewModel.Add(new RoleRequestsViewModel { User = user.ToString(), Role = role.ToString() });
                _context.SaveChanges();

                TempData["Login"] = "RegisterSucceeded";
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in identityResult.Errors)
			{
				ModelState.AddModelError("", error.Description);
			}

            ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
            return View();
		}
        #endregion

        #region logout
        [HttpGet]
		public async Task<IActionResult> LogOut()
		{
            TempData["Login"] = "LogOut";

            await _signInManager.SignOutAsync();
			return RedirectToAction("Index", "Home");
		}
        #endregion

        #region role request
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult RoleRequest()
        {
            return View(_context.RoleRequestsViewModel);
        }
        [HttpGet]
        [Authorize(Roles = "Admin")]
		public async Task<IActionResult> RoleRequestChooseAsync(int Id, string choice)
        {
            if (choice == "Accept")
            {
                // Get users requested role and their email
                var getUser = _context.RoleRequestsViewModel.Where(x => x.Id == Id).FirstOrDefault();
				var user = await _userManager.FindByEmailAsync(getUser.User);
                if (user == null || getUser == null)
                {
                    return BadRequest();
                }

                // Add the role to the User
				var addRole = await _userManager.AddToRoleAsync(user, getUser.Role);
                if (!addRole.Succeeded)
                {
					return BadRequest();
				}

                // Remove Request
				var getId = _context.RoleRequestsViewModel.Find(Id);
				_context.RoleRequestsViewModel.Remove(getId);
				_context.SaveChanges();

				TempData["Login"] = "RoleAccepted";
            }
            else if (choice == "Decline")
            {
				// Remove Request
				var getId = _context.RoleRequestsViewModel.Find(Id);
                _context.RoleRequestsViewModel.Remove(getId);
                _context.SaveChanges();

                TempData["Login"] = "RoleDenied";
            }
            return RedirectToAction("Identity", "Account");
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
            if (role.RoleName == null)
            {
				ViewBag.Roles = _roleManager.Roles;
				ModelState.AddModelError("", $"Rolename can't be empty");
				return View();
			}

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
        public async Task<IActionResult> DeleteRolesAsync(string? id)
        {
            var role = await _roleManager.FindByIdAsync(id);

			// Check if any users are in the role
			var usersInRole = await _userManager.GetUsersInRoleAsync(id);
			if (usersInRole.Any())
			{
				// Return an error if there are users in the role
				ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Name));
				TempData["Alert"] = "UsersInRole";
				return RedirectToAction("Identity");
			}

			var result = _context.Roles.Where(x => x.Name == role.ToString()).FirstOrDefault();
			if (result != null)
            {
                _context.Roles.Remove(result);
                _context.SaveChanges();

                TempData["Alert"] = "RoleDeleted";
                return RedirectToAction("Identity");
            }
            ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Name));
            return RedirectToAction("Identity");
		}
        #endregion

        #region delete user
        [HttpGet]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(string? Id)
        {
            var user = await _userManager.FindByIdAsync(Id);

            if (user == null)
            {
                return BadRequest();
            }
            else
            {
                // Deletes the user from identity and database
                var resultIdentity = _context.Users.Where(x => x.Id == Id).FirstOrDefault();
                var resultData = _context.Gebruiker.Where(x => x.Email == resultIdentity.Email).FirstOrDefault();
                if (resultIdentity != null)
                {
                    // If a user deletes the logged in user, they will be logged out
                    string loggedInUser = User.Identity.Name;
                    var deletedUser = await _userManager.FindByIdAsync(Id);
                    if (loggedInUser == deletedUser.Email) { LogOut(); }
                    else { TempData["Alert"] = "UserDeleted"; }

                    // Remove user from database
                    _context.Users.Remove(resultIdentity);
                    if (resultData != null) { _context.Gebruiker.Remove(resultData); }
                    _context.SaveChanges();

                    return RedirectToAction("Identity");
                }
            }
            return View();
        }
		#endregion

		#region manageUserRoles
		[Authorize(Roles = "Admin")]
		public IActionResult ManageUserRolesAsync()
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
            if (submitButton == "Add Role To User")
            {
                return AddUserRoleAsync(roles, users);
            }
            else if (submitButton == "Remove Role From User")
            {
                return RemoveUserRoleAsync(roles, users);
            }
            ModelState.AddModelError("", "Something went wrong");
            return Task.FromResult((IActionResult)View("ManageUserRoles"));
        }
        #endregion

        #region add role to user
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
                return View("ManageUserRoles");
            }

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            // Check if the role was successfully assigned
            if (result.Succeeded)
            {
                ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
                ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));
				TempData["Alert"] = "RoleAssignedToUser";
				return RedirectToAction("Identity");
            }
            else
            {
                return BadRequest();
            }
        }
        #endregion

        #region remove role from user
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

            // Gets all users in selected role
            var usersInRole = await _userManager.GetUsersInRoleAsync(role.Name);

			// Check if user has the role before deleting
			if (!usersInRole.Any(u => u.Id == users))
			{
				ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
				ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));
				ModelState.AddModelError("", "User doesn't have this role");
				return View("ManageUserRoles");
			}

			// Remove role from the user
			var result = await _userManager.RemoveFromRoleAsync(user, role.ToString());

            // Check if the role was successfully removed
            if (result.Succeeded)
            {
                ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
                ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));
				TempData["Alert"] = "RoleRemovedFromUser";
                return RedirectToAction("Identity");
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
