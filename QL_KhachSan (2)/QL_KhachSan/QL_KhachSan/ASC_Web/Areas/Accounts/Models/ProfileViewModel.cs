using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.Accounts.Models
{
    public class ProfileViewModel
    {
        public string UserId { get; set; }

        [Display(Name = "Tên đăng nhập")]
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        public string UserName { get; set; } // Sẽ dùng để hiển thị và cho phép chỉnh sửa

        [Display(Name = "Email")]
        [EmailAddress]
        public string Email { get; set; }

        [Display(Name = "Số điện thoại")]
        [Phone(ErrorMessage = "Số điện thoại không hợp lệ.")]
        public string PhoneNumber { get; set; }

        [Display(Name = "Vai trò")]
        public IList<string> Roles { get; set; }

        [Display(Name = "Ảnh đại diện")]
        public string AvatarUrl { get; set; }

        // Thuộc tính để nhận file ảnh tải lên từ form
        [Display(Name = "Chọn ảnh đại diện mới")]
        public IFormFile AvatarFile { get; set; }

        [Display(Name = "Email đã xác nhận")]
        public bool EmailConfirmed { get; set; }

        [Display(Name = "Khóa tài khoản đến")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm:ss}")]
        public DateTimeOffset? LockoutEnd { get; set; }

        public ProfileViewModel()
        {
            Roles = new List<string>();
            AvatarUrl = "/images/default-user-avatar.png"; // Ảnh mặc định
        }
    }
}
