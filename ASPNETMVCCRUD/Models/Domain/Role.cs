namespace HelpComing.Models.Domain
{
    public class Role
    {
        public int RoleID  { get; set; }
        public string RoleName { get; set; }

        public enum RoleTypes
        {
            Admin = 1,
            Moderator = 2,
            User = 3
        }
    }
}
