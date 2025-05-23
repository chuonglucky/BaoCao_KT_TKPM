using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.Configuration.Models
{
    public class MasterDataValueViewModel
    {
        public Guid Id { get; set; }

        [Display(Name = "Thuộc Key Chính (ID)")]
        public Guid MasterDataKeyId { get; set; }

        [Required(ErrorMessage = "Tên Giá trị không được để trống.")]
        [Display(Name = "Tên Giá trị")]
        public string Name { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; } = true; // Mặc định là active
    }
}
