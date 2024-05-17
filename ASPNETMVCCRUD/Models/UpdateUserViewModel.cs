using HelpComing.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace HelpComing.Models
{
    public class UpdateUserViewModel
    {
        public Guid UserID { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string Username { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [DataType(DataType.Password)]
        [PasswordValidation]
        public string Password { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        [EmailAddress(ErrorMessage = "Неправильный адрес почты")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int? RoleID { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public int? CountryID { get; set; }

        public List<SelectListItem> Roles { get; set; }
        public List<SelectListItem> Countries { get; set; }
    }
}
