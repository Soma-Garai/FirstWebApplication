using FirstWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using FirstWebApplication.ViewModels;
using System.Net.Mail;
using Microsoft.AspNetCore.Authorization;

namespace FirstWebApplication.Controllers
{
    [AllowAnonymous]
    public class UserController : Controller
    {
        private readonly SignInManager<UserModel> _signInManager;
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _appDbContext;
        private readonly IHttpContextAccessor _contextAccessor;

        public UserController(SignInManager<UserModel> signInManager, UserManager<UserModel> userManager, AppDbContext appDbContext, IHttpContextAccessor contextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _appDbContext = appDbContext;
            _contextAccessor = contextAccessor;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid)
            {
                MailAddress address = new MailAddress(model.Email);
                string userName = address.User;
                var user = new UserModel
                {
                    UserName = userName,
                    Email = model.Email,
                    FirstName = model.FirstName,
                    LastName = model.LastName
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    await _signInManager.SignInAsync(user, isPersistent: false);
                    return RedirectToAction("Index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            return View(model);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        public bool IsValidEmail(string emailaddress)
        {
            try
            {
                MailAddress m = new MailAddress(emailaddress);
                return true;
            }
            catch (FormatException)
            {
                return false;
            }
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                var userName = model.Email;
                if (IsValidEmail(model.Email))  //if user has entered email to login,store it as 'username' and if direct username is entered then that will be stored in result.
                {
                    var user = await _userManager.FindByEmailAsync(model.Email);
                    if (user != null)
                    {
                        userName = user.UserName;
                    }
                }
                var result = await _signInManager.PasswordSignInAsync(userName, model.Password, model.RememberMe, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }

                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
            }

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            // Get the user identifier (e.g., user ID or username)
            string userId = User.Identity.Name;

            // Clear the cart data associated with the user's identifier from session
            HttpContext.Session.Remove(userId);

            // Perform logout operation
            await _signInManager.SignOutAsync();

            // Redirect the user to the home page or login page
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        [AllowAnonymous]
        public IActionResult AccessDenied()
        {

            return View();
        }
    }
}
