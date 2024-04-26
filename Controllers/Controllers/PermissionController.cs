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
        public async Task<ActionResult> Index(string Id)
        {
            //A new instance of PermissionViewModel is created to hold data for the view.
            var model = new PermissionViewModel();
            //A new empty list of RoleClaimsViewModel objects is created to hold all permissions.
            var allPermissions = new List<RoleClaimsViewModel>();

            // Permissions for the "Products" module are retrieved using the GeneratePermissionsForModule method from
            // the Permissions class. Each permission is converted into a RoleClaimsViewModel object and
            // added to the allPermissions list.
            var productsPermissions = Permissions<Products>.GeneratePermissionsForModule("Products")
                               .Select(permission => new RoleClaimsViewModel { Value = permission })
                               .ToList();
            allPermissions.AddRange(productsPermissions);

            // Retrieve permissions for the "Order" module

          // var orderPermissions = Permissions<Orders>.GeneratePermissionsForModule("Orders")
           //                        .Select(permission => new RoleClaimsViewModel { Value = permission })
           //                        .ToList();
          // allPermissions.AddRange(orderPermissions);

            // Retrieve the role based on the provided role ID
            var role = await _roleManager.FindByIdAsync(Id);
            if (role == null)
            {
                // Handle the case where the role is not found
                ModelState.AddModelError("", $"Role with ID {Id} not found.");
                return View(model); // Or handle the error appropriately
            }
            //The RoleId property of the model is set
            model.RoleId = Id;

            // Claims associated with the retrieved role are retrieved 
            var claims = await _roleManager.GetClaimsAsync(role);
            //The values of these claims are extracted into a list.
            var roleClaimValues = claims.Select(a => a.Value).ToList();

            // The intersection of all permission values and role claim values is calculated to find
            // which permissions are authorized for the role.
            var authorizedClaims = allPermissions.Select(a => a.Value)
                                                 .Intersect(roleClaimValues)
                                                 .ToList();

            // For each permission in allPermissions, if the permission's value is found in the authorizedClaims list,
            // the Selected property of that permission in the model is set to true.
            foreach (var permission in allPermissions)
            {
                if (authorizedClaims.Any(aC => aC == permission.Value))
                {
                    permission.Selected = true;
                }
            }
            //Finally, the RoleClaims property of the model is set to the list of all permissions,
            //including their selection status.
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
            //return RedirectToAction("Index", new { roleId = model.RoleId });
            return RedirectToAction("Index", "Role");
        }
    }
}
//The index method returns a list of DTO models that contains the details on which Permissions
//are active for the corresponding UserRole.
//public async Task<ActionResult> Index(string Id)
//{
//A new instance of PermissionViewModel is created to hold data for the view.
//    var model = new PermissionViewModel();
//A new empty list of RoleClaimsViewModel objects is created to hold all permissions.
//    var allPermissions = new List<RoleClaimsViewModel>();
//    allPermissions.GetPermissions(typeof(Permissions.Products), Id);//all available permissions for policy=Permissions.Products
//    var role = await _roleManager.FindByIdAsync(Id);
//    model.RoleId = Id;

//    var claims = await _roleManager.GetClaimsAsync(role);//Claims associated with the retrieved role
//    var allClaimValues = allPermissions.Select(a => a.Value).ToList(); //'list' of all claims available
//    var roleClaimValues = claims.Select(a => a.Value).ToList(); //'list' of claims associated with the role

//    //The intersection of all permission values and role claim values is calculated to find which permissions are authorized for the role.
//    var authorizedClaims = allClaimValues.Intersect(roleClaimValues).ToList();

//    //For each permission in allPermissions, if the permission's value is found in the authorizedClaims list,
//    //the Selected property of that permission in the model is set to true.
//    foreach (var permission in allPermissions)
//    {
//        if (authorizedClaims.Any(a => a == permission.Value))
//        {
//            permission.Selected = true;
//        }
//    }
//    //Finally, the RoleClaims property of the model is set to the list of all permissions, including their selection status.
//    model.RoleClaims = allPermissions;
//    return View(model);
//}
