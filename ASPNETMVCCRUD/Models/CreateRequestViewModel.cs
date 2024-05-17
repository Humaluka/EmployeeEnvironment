using System.ComponentModel.DataAnnotations;

namespace HelpComing.Models
{
    public class CreateRequestViewModel
    {
        public Guid RequestID { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string PersonName { get; set; }

        public IFormFile PhotoFile { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public byte[] Photo
        {
            get
            {
                using (var memoryStream = new MemoryStream())
                {
                    PhotoFile.CopyTo(memoryStream);
                    return memoryStream.ToArray();
                }
            }
        }

        [Required(ErrorMessage = "Обязательное поле")]
        public string LastSeenLocation { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime LastSeenDateTime { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string Description { get; set; }

        public Guid? CreateUser { get; set; }
    }
}
