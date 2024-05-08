using GurmeDefteriWebUI.Helpers;
using GurmeDefteriWebUI.Models.ViewModel;
using GurmeDefteriWebUI.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.Threading.Tasks;

namespace GurmeDefteriWebUI.Controllers
{
    public class AccountController : Controller
    {
        private readonly AuthService _authService;
        public AccountController()
        {
            _authService ??= new ();
        }

        [HttpGet]
        public IActionResult Login()
        {
            if(User.IsInRole("Admin"))
                return RedirectToAction("Index", "Home");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginUser loginUser)
        {
           string loginUserMail = loginUser.Mail.Trim();
            string loginUserPassword = loginUser.Password.Trim();
            if (ModelState.IsValid)
            {
                var result = await _authService.Authenticate(loginUserMail, loginUserPassword);

                if (result )
                {
                    var claims = new List<Claim>
                    {
                        new Claim(ClaimTypes.Name,loginUserMail),
                        new Claim(ClaimTypes.Role,"Admin")
                    };
                    var claimsIdentity= new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);
                    var authProperties =new AuthenticationProperties();
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                    return RedirectToAction("Index", "Home");
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı adı veya şifre hatalı");
                    return View(loginUser);
                }
            }
            else
            {
                
                return View(loginUser);
            }
        }

        [HttpPost]
        public async Task<IActionResult> LogoutAsync()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login", "Account");
        }

    }
}
