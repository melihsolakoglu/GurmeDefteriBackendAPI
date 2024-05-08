using GurmeDefteriWebUI.Data;
using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;
using GurmeDefteriWebUI.Services;
using GurmeDefteriWebUI.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System.Drawing.Printing;

namespace GurmeDefteriWebUI.Controllers
{
    public class UserController : Controller
    {

        private readonly UserService _userService;
        private readonly IUserModelStatePropCheck _userModelStateProp;
        private readonly int pageSize;
        public UserController(IUserModelStatePropCheck userModelState)
        {
            _userModelStateProp = userModelState;
                _userService = new ();
                pageSize = 8;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searcKey = "")
        {
            if (String.IsNullOrEmpty(searcKey))
            {
                int pageCount = Convert.ToInt32(await _userService.GetPageCountUserAsync(pageSize));
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                List<User> userItems = await _userService.GetPagedUserAsync(page, pageSize);
                ViewData["UserItems"] = userItems;
                ViewBag.PageNumber = page;
                ViewData["PageCount"] = pageCount;
                return View();
            }
            else
            {
                searcKey = searcKey.Trim();
                int pageCount = Convert.ToInt32(await _userService.GetPageCountUseryNameAsync(pageSize, searcKey));
                var userItems = await _userService.GetPagedUserWithKeyAsync(page, pageSize, searcKey);
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                ViewData["UserItems"] = userItems;
                ViewBag.PageNumber = page;
                ViewBag.SearchKey = searcKey;
                ViewData["PageCount"] = pageCount;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index(string searcKey)
        {
            return RedirectToAction(nameof(Index), new { searcKey });
        }


        [HttpGet]
        public async Task<IActionResult> AddUser()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> AddUser(User model)
        {



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
        public async Task<IActionResult> DeleteUser(string mail)
        {
            var userToDelete = await _userService.GetUserByMailAsync(mail);
            await _userService.DeleteUserAsync(userToDelete.Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
