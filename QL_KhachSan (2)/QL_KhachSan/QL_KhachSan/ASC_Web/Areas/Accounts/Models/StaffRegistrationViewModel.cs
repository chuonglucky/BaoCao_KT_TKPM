using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.Accounts.Models
{
    public class StaffRegistrationViewModel : IValidatableObject
    {
        public string? UserId { get; set; }

        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Tên đăng nhập là bắt buộc.")]
        [Display(Name = "Tên đăng nhập")]
        public string UserName { get; set; }

        [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} và tối đa {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }

        [Required(ErrorMessage = "Vui lòng chọn vai trò.")]
        [Display(Name = "Vai trò")]
        public string RoleToAssign { get; set; }

        public bool IsEdit { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; } = true;

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsEdit)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    yield return new ValidationResult("Mật khẩu là bắt buộc.", new[] { nameof(Password) });
                }
                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    yield return new ValidationResult("Xác nhận mật khẩu là bắt buộc.", new[] { nameof(ConfirmPassword) });
                }
            }
        }
    }
}
