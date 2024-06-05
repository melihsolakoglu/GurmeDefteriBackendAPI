using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;
using GurmeDefteriWebUI.Services.Interfaces;

namespace GurmeDefteriWebUI.Services
{
    public class ScoredFoodModelStatePropCheck : IScoredFoodModelStatePropCheck
    {
        private readonly ScoredFoodService scoredFoodService;
        private ModelStateFeedback _modelStateFeedback;
        private ModelStateFeedback _modelStateFeedbackTemp;
        public ScoredFoodModelStatePropCheck()
        {
            scoredFoodService = new();
            _modelStateFeedback = new(true, " ");
        }
        public async Task<ModelStateFeedback> GetModelStateFeedbacAddScoredFood(ScoredFood model)
        {
           await CheckScore(model.Score);
            await CheckMail(model.Email);
            await CheckFoodName(model.Foodname);
            await CheckIsAlreadyAdded(model.Email,model.Foodname);
            _modelStateFeedbackTemp = _modelStateFeedback;
            _modelStateFeedback = new(true, " ");
            return _modelStateFeedbackTemp;
        }

        public async Task<ModelStateFeedback> GetModelStateFeedbacUpdateScoredFood(int score)
        {
            await CheckScore(score);
            _modelStateFeedbackTemp = _modelStateFeedback;
            _modelStateFeedback = new(true, " ");
            return _modelStateFeedbackTemp;
        }
        public async Task CheckScore(int score)
        {
            if (score > 10 || score < 1)
            {
                _modelStateFeedback.Message += "Skor 1-10 arasıdna olmalıdır.\n";
                _modelStateFeedback.IsValid = false;
            }
        }
        public async Task CheckMail(string mail)
        {
            List<string> userMails = await scoredFoodService.GetAllUserMails();
            if (!userMails.Contains(mail))
            {
                _modelStateFeedback.Message += "Bu maile ait bir hesap bulunammakta !!!\n";
                _modelStateFeedback.IsValid = false;
            }
        }

        public async Task CheckFoodName(string foodName)
        {
            List<string> foodNames = await scoredFoodService.GetAllFoodsNames();
            if (!foodNames.Contains(foodName))
            {
                _modelStateFeedback.Message += "Bu isimde bir yemek bulunmamakta!!!\n";
                _modelStateFeedback.IsValid = false;
            }
        }
        public async Task CheckIsAlreadyAdded(string mail,string foodName)
        {
            bool foodNames = await scoredFoodService.CheckScoredFood(mail, foodName);
            if (foodNames)
            {
                _modelStateFeedback.Message += "Bu Skor Zaten Eklenmiş!!!\n";
                _modelStateFeedback.IsValid = false;
            }
        }

        //daha önceden bu mail ve bu adrese ait bir puanalama yapımış mı
    }
}
