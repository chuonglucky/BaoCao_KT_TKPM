using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class MasterDataValue : BaseEntity
    {
        public MasterDataValue() { }

        public MasterDataValue(Guid masterDataKeyId, string valueName)
        {
            // this.Id = Guid.NewGuid(); // Đã được BaseEntity xử lý
            this.MasterDataKeyId = masterDataKeyId;
            this.Name = valueName;
        }

        public bool IsActive { get; set; }
        public string Name { get; set; }
        public Guid MasterDataKeyId { get; set; }
        public virtual MasterDataKey MasterDataKey { get; set; } // Thêm 'virtual'
    }
}
