using System.ComponentModel.DataAnnotations;
 
namespace Clinic.ViewModels
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Не указан телефон")]
        public string Email { get; set; }
         
        [Required(ErrorMessage = "Не указан пароль")]
        [DataType(DataType.Password)]
        public string Password { get; set; }
    }
}
