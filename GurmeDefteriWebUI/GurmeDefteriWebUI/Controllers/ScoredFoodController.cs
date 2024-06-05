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
    public class ScoredFoodController : Controller
    {
        private readonly StringPropertyTrim stringPropertyTrim;
        private readonly ScoredFoodService _scoredFoodervice;
        private readonly int pageSize;
        private readonly IScoredFoodModelStatePropCheck _scoredFoodModelStatePropCheck;
        public ScoredFoodController(IScoredFoodModelStatePropCheck scoredFoodModelStatePropCheck)
        {
            stringPropertyTrim = new();
            _scoredFoodModelStatePropCheck = scoredFoodModelStatePropCheck;
            _scoredFoodervice = new();
            pageSize = 8;

        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searchKey = "")
        {
            if (String.IsNullOrEmpty(searchKey))
            {
                int pageCount = Convert.ToInt32(await _scoredFoodervice.GetPageCountScoredFoodAsync(pageSize));
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                List<ScoredFood> scoredFoodItems = await _scoredFoodervice.GetPagedScoredFoodAsync(page, pageSize);
                ViewData["ScoredFoodItems"] = scoredFoodItems;
                ViewBag.PageNumber = page;
                ViewData["PageCount"] = pageCount;
                return View();
            }
            else
            {
                searchKey = searchKey.Trim();
                int pageCount = Convert.ToInt32(await _scoredFoodervice.GetPageCountScoredFoodWithKeyAsync(pageSize, searchKey));
                var scoredFoodItems = await _scoredFoodervice.GetPagedScoredFoodWithKeyAsync(page, pageSize, searchKey);
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                ViewData["ScoredFoodItems"] = scoredFoodItems;
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
        public async Task<IActionResult> AddScoredFood()
        {
            List<string> userMails =await _scoredFoodervice.GetAllUserMails();
            ViewData["UsersMails"]= userMails;
            List<string> foodNames = await _scoredFoodervice.GetAllFoodsNames();
            ViewData["FoodNames"] = foodNames;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddScoredFood(ScoredFood model)
        {
            stringPropertyTrim.TrimAllStringProperties(model);
            if (ModelState.IsValid)
            {
                ModelStateFeedback foodModelState = await _scoredFoodModelStatePropCheck.GetModelStateFeedbacAddScoredFood(model);
                if (!foodModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, foodModelState.Message);
                    return View(model);
                }
                var respond = await _scoredFoodervice.AddScoredFood(model.Email,model.Foodname,model.Score);
                if (respond == "Error")
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı Eklenemedi,");
                    return View(model);
                }
                TempData["PopUpTittle"] = "İşlem Başarılı";
                TempData["PopUpMessage"] = $"{model.Email+" " +model.Foodname+"  "+model.Score} Başarıyla Eklendi :)";
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
        public async Task<IActionResult> UpdateScoredFood(string scoredFoodId)
        {
            var userToUpdate = await _scoredFoodervice.GetScoredFoodUByIdAsync(scoredFoodId);
            return View(userToUpdate);
        }

        [HttpPost]
        public async Task<IActionResult> UpdateScoredFood(ScoredFood model)
        {
            stringPropertyTrim.TrimAllStringProperties(model);
            ViewBag.FoodItem = model;

            if (ModelState.IsValid)
            {
                ModelStateFeedback userModelState = await _scoredFoodModelStatePropCheck.GetModelStateFeedbacUpdateScoredFood(model.Score);
                if (!userModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, userModelState.Message);
                    return View(model);
                }
                var respond = await _scoredFoodervice.UpdateScoredFood(model.ScoredFoodID,model.Score);
                if (respond == "Error")
                {
                    ModelState.AddModelError(string.Empty, "Kullanıcı Güncellenemedi, ");
                    return View(model);
                }
                TempData["PopUpTittle"] = "İşlem Başarılı";
                TempData["PopUpMessage"] = $"{model.Email + " " + model.Foodname + "  " + model.Score} Başarıyla Güncellendi :)";
                TempData["isPopupVisible"] = true;

                return await UpdateScoredFood(model.ScoredFoodID);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
                return View(model);
            }
        }


        [HttpPost]
        public async Task<IActionResult> DeleteScoredFood(string scoredFoodId)
        {
            await _scoredFoodervice.DeleteScoredFoodAsync(scoredFoodId);
            return RedirectToAction(nameof(Index));
        }
    }
}
