using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;
using Microsoft.AspNetCore.Identity;

namespace ASC.Model.Models
{
    // IAuditTracker đã được implement bởi BaseEntity
    public class Booking : BaseEntity
    {
        public Booking()
        {
            Status = "PendingApproval"; // Trạng thái mặc định khi khách gửi yêu cầu
        }

        [Required]
        [StringLength(200)]
        public string CustomerName { get; set; }

        [Required]
        public Guid RoomId { get; set; }
        public virtual Room Room { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CheckInDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? CheckOutDate { get; set; }

        [StringLength(50)]
        public string Status { get; set; }

        public string UserId { get; set; }
        public virtual IdentityUser User { get; set; }
    }
}
