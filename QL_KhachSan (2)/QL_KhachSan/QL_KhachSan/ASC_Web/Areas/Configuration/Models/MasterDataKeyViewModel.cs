using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.Configuration.Models
{
    public class MasterDataKeyViewModel
    {
        public Guid Id { get; set; } // Sẽ tự động được gán khi tạo mới, hoặc dùng để nhận diện khi cập nhật/xóa

        [Required(ErrorMessage = "Tên Key không được để trống")]
        [Display(Name = "Tên Key Chính")]
        public string Name { get; set; }

        [Display(Name = "Đang hoạt động")]
        public bool IsActive { get; set; } = true;
    }
}
