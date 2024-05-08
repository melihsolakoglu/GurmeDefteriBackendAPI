using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;

namespace GurmeDefteriWebUI.Services.Interfaces
{

    public interface IFoodModelStatePropCheck
    {
        Task<ModelStateFeedback> GetModelStateFeedbacAddkFood(Food model);
        Task<ModelStateFeedback> GetModelStateFeedbackUpdateFoodAsync(Food model, bool IsNameChanged);
    }
}
