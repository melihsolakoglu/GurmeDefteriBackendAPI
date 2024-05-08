using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

namespace GurmeDefteriWebUI.Services
{
    public class RoleService
    {
        private readonly RoleManager<IdentityRole> _roleManager;

        public RoleService(RoleManager<IdentityRole> roleManager)
        {
            _roleManager = roleManager;
        }

    }
}
