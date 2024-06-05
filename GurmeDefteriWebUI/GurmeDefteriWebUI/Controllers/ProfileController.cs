using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;
using GurmeDefteriWebUI.Services;
using GurmeDefteriWebUI.Services.Interfaces;
using GurmeDefteriWebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace GurmeDefteriWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProfileController : Controller
    {
        private readonly StringPropertyTrim stringPropertyTrim;
        private readonly UserService _userService;
        private readonly IUserModelStatePropCheck _userModelStateProp;
        public ProfileController(IUserModelStatePropCheck userModelState)
        {
            stringPropertyTrim = new();
            _userModelStateProp = userModelState;
            _userService = new();
        }
        public async Task<IActionResult> IndexAsync()
        {
            string mail = Request.Cookies["Mail"];
             User user= await _userService.GetUserByMailAsync(mail);  
            return View(user);
        }

        [HttpGet]
        public async Task<IActionResult> UpdateProfile(string mail)
        {
            var userToUpdate = await _userService.GetUserByMailAsync(mail.Trim());
            return View(userToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateProfile(User model, string prevMail)
        {
            stringPropertyTrim.TrimAllStringProperties(model);
            ViewBag.FoodItem = model;
            bool IsMailChanged = model.Email != prevMail;

            if (ModelState.IsValid)
            {
                ModelStateFeedback userModelState = await _userModelStateProp.GetModelStateFeedbackUpdateUserAsync(model, IsMailChanged);
                if (!userModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, userModelState.Message);
                    return View(model);
                }
                var respond = await _userService.UpdateUser(model);
                if (respond == "Error")
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı Eklenemedi, ");
                    return View(model);
                }
                else { }
                if (model.Role != "Admin")
                return RedirectToAction("Logout", "Account");

                TempData["PopUpTittle"] = "İşlem Başarılı";
                TempData["PopUpMessage"] = $"{model.Name} Başarıyla Güncellendi :)";
                TempData["isPopupVisible"] = true;
                if(IsMailChanged)
                    Response.Cookies.Append("Mail", model.Email);
                return await UpdateProfile(model.Email);
           
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteProfile(string mail)
        {
            var userToDelete = await _userService.GetUserByMailAsync(mail.Trim());
            await _userService.DeleteUserAsync(userToDelete.Id);
            return RedirectToAction("Logout", "Account");
        }
    }
}
