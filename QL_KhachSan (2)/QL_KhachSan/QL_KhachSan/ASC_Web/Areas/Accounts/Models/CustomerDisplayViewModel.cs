namespace ASC_Web.Areas.Accounts.Models
{
    public class CustomerDisplayViewModel
    {
        public string UserId { get; set; } // To identify user for actions like delete
        public string UserName { get; set; }
        public string Email { get; set; }
        public bool IsActive { get; set; }
    }
}
