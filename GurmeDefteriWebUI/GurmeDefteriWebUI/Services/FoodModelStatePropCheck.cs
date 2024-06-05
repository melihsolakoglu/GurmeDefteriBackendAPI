using GurmeDefteriWebUI.Data;
using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;
using GurmeDefteriWebUI.Services.Interfaces;
using System.Reflection;

namespace GurmeDefteriWebUI.Services
{
    public class FoodModelStatePropCheck : IFoodModelStatePropCheck
    {
        private ModelStateFeedback _modelStateFeedback;
        private ModelStateFeedback _modelStateFeedbackTemp;
        public FoodModelStatePropCheck()
        {
            _modelStateFeedback = new(true," ");
        }
       
        public  async Task<ModelStateFeedback> GetModelStateFeedbacAddkFood(Food modelAddFood)
        {
            CheckName(modelAddFood.Name);
            CheckCountry(modelAddFood.Country);
            await CheckIsThereSameFoodAsync(modelAddFood.Name);
                _modelStateFeedbackTemp = _modelStateFeedback;
                _modelStateFeedback = new(true, " ");
                return _modelStateFeedbackTemp;
        }

        public async Task<ModelStateFeedback> GetModelStateFeedbackUpdateFoodAsync(Food modelFood, bool IsNameChanged)
        {
            if (IsNameChanged)
            {
                CheckName(modelFood.Name);
                await CheckIsThereSameFoodAsync(modelFood.Name);
            }
            CheckCountry(modelFood.Country);
            _modelStateFeedbackTemp = _modelStateFeedback;
            _modelStateFeedback = new(true, " "); 
            return _modelStateFeedbackTemp;
        }

        private void CheckName(string name)
        {
            if(name.Length > 100) {
                _modelStateFeedback.Message += "Yemek ismi çok uzun.\n";
                _modelStateFeedback.IsValid = false;
            }
        }
        private void CheckCountry(string country)
        {
            if (!CountryData.countries.Contains(country))
            {
                _modelStateFeedback.Message += "Geçersiz ülke. Lütfen listelenmis ülkelerden birini seçin.\n";
                _modelStateFeedback.IsValid = false;
            }
        }
        private async Task CheckIsThereSameFoodAsync(string name)
        {
            FoodService foodSeri = new();
            var foodSameName = await foodSeri.GetFoodByNameAsync(name);
            if (foodSameName!=null)
            {
                _modelStateFeedback.Message += "Zaten bu isimde bir yemek bulunmakta.\n";
                _modelStateFeedback.IsValid = false;
            }
        }

    }
}
