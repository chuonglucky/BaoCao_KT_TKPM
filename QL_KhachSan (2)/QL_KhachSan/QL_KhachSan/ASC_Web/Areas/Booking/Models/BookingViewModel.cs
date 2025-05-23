using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.Booking.Models
{
    public class BookingViewModel
    {
        [Required]
        public Guid RoomId { get; set; }

        public string RoomNumber { get; set; }

        [Display(Name = "Tên khách hàng")]
        [Required(ErrorMessage = "Tên khách hàng là bắt buộc.")]
        [StringLength(200)]
        public string CustomerName { get; set; }

        [Display(Name = "Ngày nhận phòng")]
        [Required(ErrorMessage = "Ngày nhận phòng là bắt buộc.")]
        [DataType(DataType.DateTime)]
        public DateTime CheckInDate { get; set; }

        [Display(Name = "Ngày trả phòng")]
        [Required(ErrorMessage = "Ngày trả phòng là bắt buộc.")]
        [DataType(DataType.DateTime)]
        public DateTime CheckOutDate { get; set; }

        public string RoomType { get; set; }
    }
}
