using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApplication.Models
{
    public class UserModel : IdentityUser
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }

        public string UserId { get; set; }
        //public string UserName { get; set; }
        //public string Email { get; set; }
        //public string Password { get; set; }
    }
}
