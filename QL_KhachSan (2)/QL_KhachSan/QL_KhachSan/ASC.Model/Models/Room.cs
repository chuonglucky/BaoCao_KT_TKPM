using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class Room : BaseEntity
    {
        public Room() { }

        public Room(string roomNumber)
        {
            // this.Id = Guid.NewGuid(); // Đã được BaseEntity xử lý
            this.RoomNumber = roomNumber;
        }

        public string RoomNumber { get; set; }
        public Guid RoomTypeId { get; set; }
        public virtual MasterDataValue RoomType { get; set; } // 'virtual' để hỗ trợ lazy loading
        public string Status { get; set; }
        public decimal PricePerNight { get; set; }

        // Các thuộc tính mới dựa trên câu truy vấn SQL
        public DateTime? CheckInDate { get; set; } // Nullable DateTime, tương ứng datetime2(7), NULL
        public DateTime? CheckOutDate { get; set; } // Nullable DateTime, tương ứng datetime2(7), NULL
        public decimal? TotalPrice { get; set; }
    }
}
