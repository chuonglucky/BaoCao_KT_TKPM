using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.BaseTypes;

namespace ASC.Model.Models
{
    public class ServiceRequest : BaseEntity
    {
        public static class Statuses
        {
            public const string PendingConfirmation = "Chờ xác nhận";
            public const string Confirmed = "Đã xác nhận";
            public const string Processing = "Đang xử lý"; // Có thể thêm trạng thái này
            public const string Rejected = "Đã từ chối";
            public const string Completed = "Đã hoàn thành";
            public const string CancelledByGuest = "Khách hủy";
            public const string CancelledByStaff = "Nhân viên hủy";
        }

        public ServiceRequest()
        {
            Status = Statuses.PendingConfirmation;
            RequestedDate = DateTime.UtcNow; // Ngày mong muốn sẽ được người dùng nhập
            // CreatedDate sẽ được BaseEntity xử lý
        }

        public ServiceRequest(string guestEmail) : this()
        {
            GuestEmail = guestEmail;
        }

        [Required(ErrorMessage = "Email khách hàng không được để trống")]
        [EmailAddress(ErrorMessage = "Địa chỉ email không hợp lệ")]
        [Display(Name = "Email Khách hàng")]
        public string GuestEmail { get; set; }

        [Required(ErrorMessage = "Loại dịch vụ chính không được để trống")]
        [Display(Name = "Loại Dịch vụ Chính")]
        public string ServiceKeyName { get; set; } // Ví dụ: "Phòng", "Dịch vụ ăn uống"

        [Required(ErrorMessage = "Chi tiết dịch vụ không được để trống")]
        [Display(Name = "Chi tiết Dịch vụ/Phòng")]
        public string ServiceValueName { get; set; } // Ví dụ: "Phòng VIP", "Gọi món phở"

        // Các ID này là tùy chọn, nhưng rất hữu ích cho việc truy vấn và đảm bảo tính toàn vẹn dữ liệu
        // Nếu bạn thêm chúng, nhớ cập nhật DbContext và tạo migration
        // public Guid? MasterDataKeyId { get; set; }
        // public virtual MasterDataKey MasterDataKey { get; set; }
        // public Guid? MasterDataValueId { get; set; }
        // public virtual MasterDataValue MasterDataValue { get; set; }

        [Required(ErrorMessage = "Trạng thái không được để trống")]
        [Display(Name = "Trạng thái")]
        public string Status { get; set; }

        [Display(Name = "Ghi chú/Yêu cầu cụ thể")]
        [DataType(DataType.MultilineText)]
        public string? RequestedServicesDetails { get; set; }

        [Required(ErrorMessage = "Ngày mong muốn/check-in không được để trống")]
        [Display(Name = "Ngày Mong muốn/Check-in")]
        [DataType(DataType.DateTime)] // Sử dụng DateTime để có cả ngày và giờ nếu cần
        public DateTime RequestedDate { get; set; }

        [Display(Name = "Ngày Kết thúc/Check-out (nếu có)")]
        [DataType(DataType.DateTime)]
        public DateTime? RequestedEndDate { get; set; }

        [Display(Name = "Ngày Hoàn thành")]
        [DataType(DataType.DateTime)]
        public DateTime? CompletedDate { get; set; }

        [Display(Name = "Nhân viên Xử lý")]
        public string? AssignedStaffEmail { get; set; }

        [Display(Name = "Phản hồi từ Nhân viên/Lý do")]
        [DataType(DataType.MultilineText)]
        public string? StaffRemarks { get; set; }

        // Nếu là yêu cầu đặt phòng và đã được xác nhận, có thể liên kết đến BookingId
        // public Guid? BookingId { get; set; }
        // public virtual Booking Booking { get; set; }
    }
}
