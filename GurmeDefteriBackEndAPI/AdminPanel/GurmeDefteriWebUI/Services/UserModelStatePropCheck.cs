using GurmeDefteriWebUI.Models.Dto;
using GurmeDefteriWebUI.Models.ViewModel;
using GurmeDefteriWebUI.Services.Interfaces;
using Microsoft.Extensions.FileSystemGlobbing.Internal;
using System.Text.RegularExpressions;
using System.Xml.Linq;

namespace GurmeDefteriWebUI.Services
{
    public class UserModelStatePropCheck : IUserModelStatePropCheck
    {
        private readonly UserService _userService;
        private ModelStateFeedback _modelStateFeedback;
        private ModelStateFeedback _modelStateFeedbackTemp;
        public UserModelStatePropCheck()
        {
            _userService = new();
            _modelStateFeedback = new(true, " ");
        }
        public async Task<ModelStateFeedback> GetModelStateFeedbacAddkUser(User model)
        {
           await CheckIsThereSameMailAsync(model.Email);
            await CheckMailFormat(model.Email);
            await CheckNameFormat(model.Name);
            await CheckAge(model.Age);
            await CheckRole(model.Role);
            _modelStateFeedbackTemp = _modelStateFeedback;
            _modelStateFeedback = new(true, " ");
            return _modelStateFeedbackTemp;
        }

        public async Task<ModelStateFeedback> GetModelStateFeedbackUpdateUserAsync(User model, bool IsMailChanged)
        {
            if(IsMailChanged) 
            await CheckIsThereSameMailAsync(model.Email);
            await CheckMailFormat(model.Email);
            await CheckNameFormat(model.Name);
            await CheckAge(model.Age);
            await CheckRole(model.Role);
            _modelStateFeedbackTemp = _modelStateFeedback;
            _modelStateFeedback = new(true, " ");
            return _modelStateFeedbackTemp;
        }

        private async Task CheckIsThereSameMailAsync(string mail)
        {

            var UserSameMail = await _userService.GetUserByMailAsync(mail);
            if (UserSameMail != null)
            {
                _modelStateFeedback.Message += "Zaten bu maili kullanan bir kullanıcı bulunmakta.\n";
                _modelStateFeedback.IsValid = false;
            }
            else { 
            }
           
        }
        private async Task CheckMailFormat(string mail)
        {

            if (!Regex.IsMatch(mail, @"^[a-zA-Z0-9._%+-]+@[a-zA-Z0-9.-]+\.[a-zA-Z]{2,}$" ))
            {
                _modelStateFeedback.Message += "Mail Uygun Değil.\n";
                _modelStateFeedback.IsValid = false;
            }
        }

        private async Task CheckNameFormat(string name)
        {

           if(!Regex.IsMatch(name, @"^[A-Za-z\s]{2,50}$"))
            {
                _modelStateFeedback.Message += "Kullanıcı Adı Uygun Değil.\n";
                _modelStateFeedback.IsValid = false;
            }
        }
        private async Task CheckAge(int age)
        {

            if (age<5&&age>150)
            {
                _modelStateFeedback.Message += "Kullanıcı Yaşı Uygun Değil.\n";
                _modelStateFeedback.IsValid = false;
            }
        }

        private async Task CheckRole(string role)
        {

            if (role!="User"&& role != "Admin")
            {
                _modelStateFeedback.Message += "Rol Geçerli Değil.\n";
                _modelStateFeedback.IsValid = false;
            }
        }

    }
}
