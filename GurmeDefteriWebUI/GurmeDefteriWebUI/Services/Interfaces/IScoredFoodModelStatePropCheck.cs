using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;

namespace GurmeDefteriWebUI.Services.Interfaces
{
    public interface IScoredFoodModelStatePropCheck
    {
        Task<ModelStateFeedback> GetModelStateFeedbacAddScoredFood(ScoredFood model);
        Task<ModelStateFeedback> GetModelStateFeedbacUpdateScoredFood(int score);
    }
}
