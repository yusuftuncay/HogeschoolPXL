using HogeschoolPXL.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace HogeschoolPXL.Controllers
{
	public class AccountController : Controller
	{
		UserManager<IdentityUser> _userManager;
		SignInManager<IdentityUser> _signInManager;
		public AccountController(UserManager<IdentityUser> userManager, SignInManager<IdentityUser> signInManager)
		{
			_userManager = userManager;
			_signInManager = signInManager;
		}

		public IActionResult Index()
		{
			return View();
		}

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
            {
                return View("Login");
            }

            var signInResult = await _signInManager.PasswordSignInAsync(
			login.Email, login.Password, false, lockoutOnFailure: false);

			if (signInResult.Succeeded)
			{
                TempData["Login"] = "Succeeded";
                return RedirectToAction("Index", "Home");
			}
			else
			{
                ModelState.AddModelError("", "Email of wachtwoord is verkeerd");
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
			{
				return View("RegistrationCompleted");
			}
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
	}

}
