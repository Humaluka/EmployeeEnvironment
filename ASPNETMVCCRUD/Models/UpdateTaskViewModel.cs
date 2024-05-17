namespace HelpComing.Models
{
    public class UpdateTaskViewModel
    {
        public Guid TaskID { get; set; }
        public string Description { get; set; }

        public DateTime Spring { get; set; }

        public DateTime CompleteTime { get; set; }

        public string Stage { get; set; }

        public string AssigneeID { get; set; }

        public string AuthorID { get; set; }

        public string SubtaskID { get; set; }
    }
}
