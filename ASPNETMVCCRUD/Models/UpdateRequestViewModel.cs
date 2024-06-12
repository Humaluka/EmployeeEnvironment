using HelpComing.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Collections;
using System.ComponentModel.DataAnnotations;
using System.IO;

namespace HelpComing.Models
{
    public class UpdateRequestViewModel
    {
        public Guid RequestID { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string PersonName { get; set; }

        public IFormFile PhotoFile { get; set; }

        public byte[]? Photo
        {
            get
            {
                if (PhotoFile != null)
                {
                    using (MemoryStream memoryStream = new MemoryStream())
                    {
                        PhotoFile.CopyTo(memoryStream);
                        return memoryStream.ToArray();
                    }
                }
                return null;
            }
            set
            {
                MemoryStream memoryStream = new MemoryStream(value);
                PhotoFile = new FormFile(memoryStream, 0, value.Length, "name", "photo");
            }
        }

        [Required(ErrorMessage = "Обязательное поле")]
        public string LastSeenLocation { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime LastSeenDateTime { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string Description { get; set; }

        public Guid? CreateUser { get; set; }

        public DateTime RequestDate { get; set; }
    }
}
