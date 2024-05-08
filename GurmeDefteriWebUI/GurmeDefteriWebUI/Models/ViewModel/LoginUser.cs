using Microsoft.AspNetCore.Identity;

namespace GurmeDefteriWebUI.Models.ViewModel
{
    public class LoginUser : IdentityUser
    {
        public string Mail { get; set; }
        public string Password { get; set; }
    }
}
