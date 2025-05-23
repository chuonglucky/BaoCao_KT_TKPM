using Microsoft.AspNetCore.Identity;

namespace ASC_Web.Areas.Accounts.Models
{
    public class CustomerViewModel
    {
        public List<CustomerDisplayViewModel>? DisplayCustomers { get; set; } // For displaying customer list with active status
        public CustomerRegistrationViewModel Registration { get; set; }
    }
}
