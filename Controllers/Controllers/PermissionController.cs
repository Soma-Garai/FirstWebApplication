using FirstWebApplication.Constants;
using FirstWebApplication.Helpers;
using FirstWebApplication.Models;
using FirstWebApplication.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FirstWebApplication.Controllers
{
    public class PermissionController : Controller
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public PermissionController(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

        //The index method returns a list of DTO models that contains the details on which Permissions
        //are active for the corresponding UserRole.
        public async Task<ActionResult> Index(string Id)
        {
            var model = new PermissionViewModel();
            var allPermissions = new List<RoleClaimsViewModel>();
            allPermissions.GetPermissions(typeof(Permissions.Products), Id); //policy=Permissions.Products
            var role = await _roleManager.FindByIdAsync(Id);
            model.RoleId = Id;
            var claims = await _roleManager.GetClaimsAsync(role); //this role is null.fix it
            var allClaimValues = allPermissions.Select(a => a.Value).ToList();
            var roleClaimValues = claims.Select(a => a.Value).ToList();
            var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(a => a == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            model.RoleClaims = allPermissions;
            return View(model);
        }
        //Once the Admin Maps new Permission to a selected user and click the Save Button,
        //the enabled permissions are added to the Role.
        public async Task<IActionResult> Update(PermissionViewModel model)
        {
            string Id = model.RoleId;
            var role = await _roleManager.FindByIdAsync(Id);
            var claims = await _roleManager.GetClaimsAsync(role); 
            foreach (var claim in claims)
            {
                await _roleManager.RemoveClaimAsync(role, claim);
            }
            var selectedClaims = model.RoleClaims.Where(a => a.Selected).ToList();
            foreach (var claim in selectedClaims)
            {
                await _roleManager.AddPermissionClaim(role, claim.Value);
            }
            return RedirectToAction("Index", new { roleId = model.RoleId });
        }
    }
}
