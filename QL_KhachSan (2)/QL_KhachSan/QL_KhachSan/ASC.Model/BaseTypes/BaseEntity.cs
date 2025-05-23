using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ASC.Model.BaseTypes
{
    public class BaseEntity
    {
        [Key]
        public Guid Id { get; set; }

        // Các thuộc tính từ IAuditTracker
        public DateTime CreatedDate { get; set; }
        public DateTime UpdatedDate { get; set; }
        public string? CreatedBy { get; set; }
        public string? UpdatedBy { get; set; } // Thêm thuộc tính UpdatedBy

        public bool IsDeleted { get; set; }

        protected BaseEntity()
        {
            Id = Guid.NewGuid();
            CreatedDate = DateTime.UtcNow;
            UpdatedDate = DateTime.UtcNow;
            IsDeleted = false;
        }
    }
}
