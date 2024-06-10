using GurmeDefteriWebUI.Data;
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
    public class UserController : Controller
    {
        private readonly StringPropertyTrim stringPropertyTrim;
        private readonly UserService _userService;
        private readonly IUserModelStatePropCheck _userModelStateProp;
        private readonly int pageSize;
        public UserController(IUserModelStatePropCheck userModelState)
        {
            stringPropertyTrim = new();
            _userModelStateProp = userModelState;
                _userService = new ();
                pageSize = 8;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searchKey = "")
        {
            if (String.IsNullOrEmpty(searchKey))
            {
                int pageCount = Convert.ToInt32(await _userService.GetPageCountUserAsync(pageSize));
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                List<User> userItems = await _userService.GetPagedUserAsync(page, pageSize);
                userItems.RemoveAll(u=>u.Email== Request.Cookies["Mail"]);
                ViewData["UserItems"] = userItems;
                ViewBag.PageNumber = page;
                ViewData["PageCount"] = pageCount;
                return View();
            }
            else
            {
                searchKey = searchKey.Trim();
                int pageCount = Convert.ToInt32(await _userService.GetPageCountUseryNameAsync(pageSize, searchKey));
                var userItems = await _userService.GetPagedUserWithKeyAsync(page, pageSize, searchKey);
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                userItems.RemoveAll(u => u.Email == Request.Cookies["Mail"]);
                ViewData["UserItems"] = userItems;
                ViewBag.PageNumber = page;
                ViewBag.SearchKey = searchKey;
                ViewData["PageCount"] = pageCount;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index(string searchKey)
        {
            return RedirectToAction(nameof(Index), new { searchKey });
        }


        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(User model)
        {
            stringPropertyTrim.TrimAllStringProperties(model);
            if (ModelState.IsValid)
            {
                ModelStateFeedback foodModelState = await _userModelStateProp.GetModelStateFeedbacAddkUser(model);
                if (!foodModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, foodModelState.Message);
                    return View(model);
                }
                var respond = await _userService.AddUser(model);
                if (respond == "Error")
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı Eklenemedi,");
                    return View(model);
                }
                TempData["PopUpTittle"] = "İşlem Başarılı";
                TempData["PopUpMessage"] = $"{model.Name} Başarıyla Eklendi :)";
                TempData["isPopupVisible"] = true;

                return RedirectToAction();
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateUser(string mail)
        {
            var userToUpdate = await _userService.GetUserByMailAsync(mail);
            return View(userToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateUser(User model, string prevMail)
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
                    ModelState.AddModelError(string.Empty, "Kullanıcı Güncellenemedi, ");
                    return View(model);
                }
                TempData["PopUpTittle"] = "İşlem Başarılı";
                TempData["PopUpMessage"] = $"{model.Name} Başarıyla Güncellendi :)";
                TempData["isPopupVisible"] = true;

                return await UpdateUser(model.Email);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteUser(string Id)
        {
            await _userService.DeleteUserAsync(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
