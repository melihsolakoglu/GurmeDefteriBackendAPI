using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;

namespace GurmeDefteriWebUI.Services.Interfaces
{
    public interface IUserModelStatePropCheck
    {
        Task<ModelStateFeedback> GetModelStateFeedbacAddkUser(User model);
        Task<ModelStateFeedback> GetModelStateFeedbackUpdateUserAsync(User model, bool IsMailChanged);
    }
}
