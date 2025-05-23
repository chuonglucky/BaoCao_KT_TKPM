using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASC_Web.Areas.Accounts.Models
{
    public class StaffManagementViewModel
    {
        public List<StaffDisplayViewModel> DisplayStaff { get; set; }
        public StaffRegistrationViewModel Registration { get; set; }
        public List<SelectListItem> AvailableRoles { get; set; }

        public StaffManagementViewModel()
        {
            DisplayStaff = new List<StaffDisplayViewModel>();
            Registration = new StaffRegistrationViewModel();
            AvailableRoles = new List<SelectListItem>();
        }
    }
}
