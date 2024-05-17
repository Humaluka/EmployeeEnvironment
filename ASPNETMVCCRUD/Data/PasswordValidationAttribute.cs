using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;

namespace HelpComing.Data
{
    public class PasswordValidationAttribute : ValidationAttribute
    {
        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var password = value as string;

            if (string.IsNullOrEmpty(password))
            {
                return new ValidationResult("Обязательно поле");
            }

            if (password.Length < 8 || password.Length > 24)
            {
                return new ValidationResult("Пароль должен содержать от 8 до 24 символов");
            }

            if (!Regex.IsMatch(password, @"[A-Z]"))
            {
                return new ValidationResult("Пароль должен содержать как минимум 1 заглавную букву");
            }

            if (!Regex.IsMatch(password, @"[a-z]"))
            {
                return new ValidationResult("Пароль должен содержать как минимум 1 прописную букву");
            }

            if (!Regex.IsMatch(password, @"[0-9]"))
            {
                return new ValidationResult("Пароль должен содержать как минимум 1 цифру");
            }

            if (!Regex.IsMatch(password, @"[\W_]"))
            {
                return new ValidationResult("Пароль должен содержать как минимум 1 специальный символ");
            }

            if (!Regex.IsMatch(password, @"^[a-zA-Z0-9\W_]+$"))
            {
                return new ValidationResult("Пароль содержит недопустимые символы");
            }

            return ValidationResult.Success;
        }
    }
}
