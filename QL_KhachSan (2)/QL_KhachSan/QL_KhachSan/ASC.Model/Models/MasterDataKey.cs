using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class MasterDataKey : BaseEntity
    {
        public MasterDataKey() { }
        public MasterDataKey(string keyName)
        {
            // this.Id = Guid.NewGuid(); // Đã được BaseEntity xử lý
            this.Name = keyName;
        }

        public bool IsActive { get; set; }
        public string Name { get; set; }
        public virtual ICollection<MasterDataValue> MasterDataValues { get; set; } = new List<MasterDataValue>(); // Thêm navigation property
    }
}
