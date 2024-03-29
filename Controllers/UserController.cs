using FirstWebApplication.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace FirstWebApplication.Controllers
{
    public class UserController : Controller
    {
        private readonly UserManager<UserModel> _userManager;
        private readonly AppDbContext _appDbContext;

        public UserController(UserManager<UserModel> userManager, AppDbContext appDbContext)
        {
            _userManager = userManager;
            _appDbContext = appDbContext;
        } 

        [HttpGet]
        public IActionResult SignUp()
        { 
            return View(); 
        }

        [HttpPost]
        public async Task<IActionResult> SignUp(UserModel registeruser)
        {
            if (ModelState.IsValid)
            {
                var user = new UserModel
                {
                    UserName = registeruser.UserName,
                    Email = registeruser.Email
                };

                var result = await _userManager.CreateAsync(user,registeruser.Password);

                if (result.Succeeded)
                {
                    _appDbContext.Users.Add(user);
                    await _appDbContext.SaveChangesAsync();
                    
                    return RedirectToAction("Index","Home");
                }

                // If registration failed, add errors to ModelState
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }

            // If ModelState is invalid or registration failed, redisplay the registration form with errors
            return View(registeruser);
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        
        [HttpPost]
        public IActionResult Login(UserModel user)
        {
            return RedirectToAction("Index", "Home");

        }
    }
}
