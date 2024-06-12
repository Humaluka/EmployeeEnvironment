namespace HelpComing.Models.Domain
{
    public class Request
    {
        public Guid RequestID { get; set; }
        public string PersonName { get; set; }

        public byte[]? Photo { get; set; }

        public string LastSeenLocation { get; set; }

        public DateTime LastSeenDateTime { get; set; }

        public string Description { get; set; }

        public Guid? CreateUser { get; set; }
        public DateTime RequestDate { get; set; }
    }
}
