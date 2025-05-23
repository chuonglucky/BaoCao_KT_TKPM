namespace ASC_Web.Areas.Configuration.Models
{
    public class MasterKeysViewModel
    {
        public List<MasterDataKeyViewModel> MasterKeys { get; set; }
        public MasterDataKeyViewModel MasterKeyInContext { get; set; } // Dùng cho form thêm/sửa
        public bool IsEdit { get; set; }

        public MasterKeysViewModel()
        {
            MasterKeys = new List<MasterDataKeyViewModel>();
            MasterKeyInContext = new MasterDataKeyViewModel();
        }
    }
}
