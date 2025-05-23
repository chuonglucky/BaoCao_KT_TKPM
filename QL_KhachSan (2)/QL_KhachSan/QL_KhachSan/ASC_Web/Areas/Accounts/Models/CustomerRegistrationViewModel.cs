using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.Accounts.Models
{
    public class CustomerRegistrationViewModel : IValidatableObject // Implement IValidatableObject
    {
        [Required(ErrorMessage = "Email là bắt buộc.")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ.")]
        [Display(Name = "Email")]
        public string Email { get; set; }

        // UserName property is removed

        [StringLength(100, ErrorMessage = "{0} phải dài ít nhất {2} và tối đa {1} ký tự.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        public string? Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Xác nhận mật khẩu")]
        [Compare("Password", ErrorMessage = "Mật khẩu và mật khẩu xác nhận không khớp.")]
        public string? ConfirmPassword { get; set; }

        public bool IsEdit { get; set; }

        [Display(Name = "Trạng thái")]
        public bool IsActive { get; set; }

        public string? CurrentEditUserId { get; set; } // Used by JavaScript to store UserID in edit mode

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (!IsEdit)
            {
                if (string.IsNullOrWhiteSpace(Password))
                {
                    yield return new ValidationResult("Mật khẩu là bắt buộc.", new[] { nameof(Password) });
                }
                else if (Password.Length < 6)
                {
                    yield return new ValidationResult("Mật khẩu phải dài ít nhất 6 ký tự.", new[] { nameof(Password) });
                }

                if (string.IsNullOrWhiteSpace(ConfirmPassword))
                {
                    yield return new ValidationResult("Xác nhận mật khẩu là bắt buộc.", new[] { nameof(ConfirmPassword) });
                }
                // Compare attribute handles this, but can be explicit if needed and Password is not null
                else if (!string.IsNullOrWhiteSpace(Password) && Password != ConfirmPassword)
                {
                    yield return new ValidationResult("Mật khẩu và mật khẩu xác nhận không khớp.", new[] { nameof(ConfirmPassword) });
                }
            }
        }
    }
}
