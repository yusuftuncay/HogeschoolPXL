using HogeschoolPXL.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.CodeAnalysis.CSharp.Syntax;
using Microsoft.EntityFrameworkCore;
using HogeschoolPXL.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using HogeschoolPXL.Data;
using Microsoft.AspNetCore.Authorization;

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

        #region create role
        [HttpGet]
        public IActionResult CreateRole()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoleAsync(RoleViewModel role)
        {
            if (!await _roleManager.RoleExistsAsync(role.RoleName))
            {
                var identityRole = new IdentityRole(role.RoleName);
                var result = await _roleManager.CreateAsync(identityRole);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            ModelState.AddModelError("", "Probleem met aanmaken van rol");
            return View();
        }
		#endregion

		#region assign role
		[HttpGet]
        [Authorize(Roles = "Admin")]
        public IActionResult AssignRole()
		{
			ViewBag.Roles = _context.Roles.Select(x => new SelectListItem(x.Name, x.Id));
			ViewBag.Users = _context.Users.Select(x => new SelectListItem(x.UserName, x.Id));

			return View();
		}
		[HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AssignRoleAsync(string roles, string users)
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
                return View();
            }

            // Assign the role to the user
            var result = await _userManager.AddToRoleAsync(user, role.Name);

            // Check if the role was successfully assigned
            if (result.Succeeded)
            {
                return Ok();
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
