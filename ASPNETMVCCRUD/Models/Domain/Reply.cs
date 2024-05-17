namespace HelpComing.Models.Domain
{
    public class Reply
    {
        public Guid ReplyID { get; set; }
        public Guid RequestID { get; set; }
        public Guid PostUser { get; set; }
        public string ReplyMessage { get; set; }
        public DateTime ReplyDateTime { get; set; }
    }
}
