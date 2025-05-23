using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.Configuration.Models
{
    public class MasterValuesViewModel
    {
        [Display(Name = "Chọn Key Chính")]
        public Guid? SelectedMasterDataKeyId { get; set; } // PHẢI LÀ Guid? (nullable)

        public string SelectedMasterDataKeyName { get; set; } // Dùng để hiển thị, không có [Required]

        public List<MasterDataValueViewModel> MasterValues { get; set; }

        // MasterValueInContext nên được khởi tạo trong constructor
        // và các thuộc tính của nó (như Name) sẽ được validate riêng
        public MasterDataValueViewModel MasterValueInContext { get; set; }

        public bool IsEdit { get; set; }

        public MasterValuesViewModel()
        {
            MasterValues = new List<MasterDataValueViewModel>();
            MasterValueInContext = new MasterDataValueViewModel(); // IsActive = true được đặt trong MasterDataValueViewModel
            // SelectedMasterDataKeyId sẽ là null theo mặc định cho Guid?
        }
    }
}
