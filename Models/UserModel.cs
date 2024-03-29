using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace FirstWebApplication.Models
{
    public class UserModel : IdentityUser
    {
        public int Id { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }
}
