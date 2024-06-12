using System.ComponentModel.DataAnnotations;

namespace HelpComing.Models
{
    public class CreateReplyViewModel
    {
        public Guid ReplyID { get; set; }
        public Guid RequestID { get; set; }
        public Guid PostUser { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public string ReplyMessage { get; set; }

        [Required(ErrorMessage = "Обязательное поле")]
        public DateTime ReplyDateTime { get; set; }
    }
}
