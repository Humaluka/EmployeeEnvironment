namespace HelpComing.Models.Domain
{
    public class User
    {
        public Guid UserID  { get; set; }
        public string Username { get; set; }

        public string Password { get; set; }

        public string Email { get; set; }

        public int? RoleID { get; set; }

        public int? CountryID { get; set; }
    }
}
