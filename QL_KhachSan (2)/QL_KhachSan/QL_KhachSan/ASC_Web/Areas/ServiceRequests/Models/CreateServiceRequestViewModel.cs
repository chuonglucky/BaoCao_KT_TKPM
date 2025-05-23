using Microsoft.AspNetCore.Mvc.Rendering;
using System.ComponentModel.DataAnnotations;

namespace ASC_Web.Areas.ServiceRequests.Models
{
    public class CreateServiceRequestViewModel
    {
        // SelectedMasterDataKeyId và MasterKeys đã được loại bỏ

        [Display(Name = "Dịch vụ")] // Thay đổi label
        [Required(ErrorMessage = "Vui lòng chọn một dịch vụ.")] // Thay đổi thông báo lỗi
        public Guid? SelectedMasterDataValueId { get; set; }

        // Danh sách tất cả các dịch vụ (chi tiết) có sẵn để chọn
        public SelectList? AllAvailableServices { get; set; }

        [Required(ErrorMessage = "Ngày mong muốn/check-in không được để trống.")]
        [Display(Name = "Ngày Mong muốn/Check-in")]
        [DataType(DataType.Date)]
        public DateTime RequestedDate { get; set; } = DateTime.Today;

        [Display(Name = "Ngày Kết thúc/Check-out (Nếu có)")]
        [DataType(DataType.Date)]
        public DateTime? RequestedEndDate { get; set; }

        [Display(Name = "Ghi chú/Yêu cầu cụ thể")]
        [DataType(DataType.MultilineText)]
        [StringLength(500, ErrorMessage = "Ghi chú không được vượt quá 500 ký tự.")]
        public string? RequestedServicesDetails { get; set; }
    }

    // ServiceRequestDetailsViewModel giữ nguyên như trước
    public class ServiceRequestDetailsViewModel
    {
        public Guid Id { get; set; }
        [Display(Name = "Email Khách hàng")]
        public string GuestEmail { get; set; } = string.Empty;
        [Display(Name = "Loại Dịch vụ Chính")]
        public string ServiceKeyName { get; set; } = string.Empty;
        [Display(Name = "Chi tiết Dịch vụ")]
        public string ServiceValueName { get; set; } = string.Empty;
        [Display(Name = "Trạng thái")]
        public string Status { get; set; } = string.Empty;
        [Display(Name = "Ghi chú/Yêu cầu cụ thể")]
        public string? RequestedServicesDetails { get; set; }
        [Display(Name = "Ngày Mong muốn/Check-in")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime RequestedDate { get; set; }
        [Display(Name = "Ngày Kết thúc/Check-out")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? RequestedEndDate { get; set; }
        [Display(Name = "Ngày Yêu cầu (Tạo)")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime CreatedDate { get; set; }
        [Display(Name = "Ngày Hoàn thành")]
        [DisplayFormat(DataFormatString = "{0:dd/MM/yyyy HH:mm}", ApplyFormatInEditMode = true)]
        public DateTime? CompletedDate { get; set; }
        [Display(Name = "Nhân viên Xử lý")]
        public string? AssignedStaffEmail { get; set; }
        [Display(Name = "Người tạo Yêu cầu")]
        public string CreatedBy { get; set; } = string.Empty;
        [Display(Name = "Cập nhật lần cuối bởi")]
        public string? UpdatedBy { get; set; }
        [Display(Name = "Phản hồi từ Nhân viên")]
        [DataType(DataType.MultilineText)]
        public string? StaffRemarks { get; set; }
    }
}
