using FirstWebApplication.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApplication.Controllers
{
    public class RoleController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        public RoleController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }
        public IActionResult Index()
        {
            var role = _roleManager.Roles.ToList();
            return View(role);
        }

        [HttpGet]
        public IActionResult CreateRole() 
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            if (!string.IsNullOrWhiteSpace(roleName))
            {
                var role = new IdentityRole(roleName);
                var result = await _roleManager.CreateAsync(role);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index");
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }
            }
            return View(roleName);
        }

        [HttpGet]
        public async Task<IActionResult> EditRole(string id)
        {
            // Retrieve the role from the database using the provided id
            var role = await _roleManager.FindByIdAsync(id);

            // Check if the role exists
            if (role == null)
            {
                // Role not found, handle the error (e.g., display error message)
                return NotFound();
            }

            // Create a view model to pass data to the view
            var model = new IdentityRole
            {
                Id = role.Id,
                Name = role.Name
            };

            // Pass the view model to the view
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateRole(string id, string Name)
        {
            // Retrieve the role from the database
            var role = await _roleManager.FindByIdAsync(id);
            if (role == null)
            {
                // Role not found, handle the error
                return NotFound();
            }

            // Update the role properties
            role.Name = Name;

            // Update the role in the database
            var result = await _roleManager.UpdateAsync(role);
            if (!result.Succeeded)
            {
                // Failed to update role, handle the error
                // You can access the errors using result.Errors
                return BadRequest("Failed to update role");
            }

            // Role updated successfully, redirect to a success page or return a success message
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> DeleteRole(string Id)
        {
            // Retrieve the role from the database using the provided id
            var role = await _roleManager.FindByIdAsync(Id);

            // Check if the role exists
            if (role == null)
            {
                // Role not found, handle the error (e.g., display error message)
                return NotFound();
            }

            // Create a view model to pass data to the view
            var model = new IdentityRole
            {
                Id = role.Id,
                Name = role.Name
            };

            // Pass the view model to the view
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> DeleteRole(string Id, string Name)
        {
            // Retrieve the role from the database
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                // Role not found, handle the error
                return NotFound();
            }

            // Delete the role from the database
            var result = await _roleManager.DeleteAsync(role);
            if (!result.Succeeded)
            {
                // Failed to delete role, handle the error
                // You can access the errors using result.Errors
                return BadRequest("Failed to delete role");
            }

            // Role deleted successfully, redirect to a success page or return a success message
            return RedirectToAction("Index");
        }


    }
}
