
using System.ComponentModel.DataAnnotations;

namespace GurmeDefteriWebUI.Models.ViewModel
{
    public class FoodTemp
    {
        [StringLength(70, MinimumLength = 2, ErrorMessage = "İsim en az 2, en fazla 70 karakter olmalıdır.")]
        [Required(ErrorMessage = "İsim alanı girilmelidir.\n")]
        public string Name { get; set; }
        [Required(ErrorMessage = "Ülke alanı girilmelidir.\n")]
        public string Country { get; set; }
        [Required(ErrorMessage = "Resim alanı girilmelidir.\n")]
        public IFormFile Image { get; set; }
    }
}
