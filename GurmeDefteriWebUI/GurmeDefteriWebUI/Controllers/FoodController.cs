using GurmeDefteriWebUI.Data;
using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;
using GurmeDefteriWebUI.Services;
using GurmeDefteriWebUI.Services.Interfaces;
using GurmeDefteriWebUI.Utils;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Numerics;
using System.Xml.Linq;

namespace GurmeDefteriWebUI.Controllers
{
    [Authorize(Roles = "Admin")]
    public class FoodController : Controller
    {
        private readonly StringPropertyTrim stringPropertyTrim;
        private readonly FoodService _foodService;
        private readonly IFoodModelStatePropCheck _foodModelStatePropCheck;
        private readonly int pageSize;
        public FoodController(IFoodModelStatePropCheck foodModelStatePropCheck)
        {
            stringPropertyTrim = new();
            _foodModelStatePropCheck = foodModelStatePropCheck;
            _foodService = new();
            pageSize = 6;
        }
        [HttpGet]
        public async Task<IActionResult> Index(int page = 1, string searchKey="")
        {
            if (String.IsNullOrEmpty(searchKey))
            {
                int pageCount = Convert.ToInt32(await _foodService.GetPageCountFoodAsync(pageSize));
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                List<Food> foodItems = await _foodService.GetPagedFoodAsync(page, pageSize);
                ViewData["FoodItems"] = foodItems;
                ViewBag.PageNumber = page;
                ViewData["PageCount"] = pageCount;
                return View();
            }
            else
            {
                searchKey = searchKey.Trim();
                int pageCount = Convert.ToInt32(await _foodService.GetPageCountFoodByNameAsync(pageSize,searchKey));
                var foodItems = await _foodService.GetPagedFoodWithKeyAsync(page, pageSize, searchKey);
                page = (page > pageCount) ? pageCount : page;
                page = (page < 1) ? 1 : page;
                ViewBag.FoodItems = foodItems;
                ViewBag.PageNumber = page;
                ViewBag.SearchKey = searchKey;
                ViewData["PageCount"] = pageCount;
                return View();
            }
        }
        [HttpPost]
        public async Task<IActionResult> Index( string searchKey)
        {
                return RedirectToAction(nameof(Index), new { searchKey });
        }
        [HttpGet]
        public async Task<IActionResult> AddFood()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> AddFood(Food model)
        {
            stringPropertyTrim.TrimAllStringProperties(model);
            if (ModelState.IsValid)
            {
                ModelStateFeedback foodModelState = await  _foodModelStatePropCheck.GetModelStateFeedbacAddkFood(model);
                if (!foodModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, foodModelState.Message);
                    return View(model);
                }
               var respond= await _foodService.AddFood(model.Name,model.Country,model.ImageBytes,model.Category);
                if(respond=="Error")
                {
                    ModelState.AddModelError(string.Empty, "Yemek Eklenemedi, Resmi değitirmeyi deniyebilirsiniz");
                    return View(model);
                }
                TempData["PopUpTittle"] = "İşlem Başarılı";
                TempData["PopUpMessage"] = $"{model.Name} Başarıyla Eklendi :)";
                TempData["isPopupVisible"] = true;

              return RedirectToAction();
            }
            else
            {
                ModelState.AddModelError(string.Empty,"");
                return View(model);
            }
        }
        [HttpGet]
        public async Task<IActionResult> UpdateFood(string name)
        {
            var food = await _foodService.GetFoodByNameAsync(name.Trim());
            return View(food);
        }
        [HttpPost]
        public async Task<IActionResult> UpdateFood(Food model,string prevName)
        {
            stringPropertyTrim.TrimAllStringProperties(model);
            ViewBag.FoodItem = model;
            bool IsNameChanged = model.Name != prevName;

            if (ModelState.IsValid)
            {
                ModelStateFeedback foodModelState = await _foodModelStatePropCheck.GetModelStateFeedbackUpdateFoodAsync(model, IsNameChanged);
                if (!foodModelState.IsValid)
                {
                    ModelState.AddModelError(string.Empty, foodModelState.Message);
                    return View(model);
                }
                var respond = await _foodService.UpdateFood(model);          
                if (respond == "Error")
                {
                    ModelState.AddModelError(string.Empty, "Yemek Eklenemedi, Resmi değitirmeyi deniyebilirsiniz");
                    return View(model);
                }
                TempData["PopUpTittle"] = "İşlem Başarılı";
                TempData["PopUpMessage"] = $"{model.Name} Başarıyla Güncellendi :)";
                TempData["isPopupVisible"] = true;

                return await UpdateFood(model.Name);
            }
            else
            {
                ModelState.AddModelError(string.Empty, "");
                return View(model);
            }
        }

        [HttpPost]
        public async Task<IActionResult> DeleteFood(string Id)
        {
            await _foodService.DeleteFoodAsync(Id);
            return RedirectToAction(nameof(Index));
        }
    }
}
