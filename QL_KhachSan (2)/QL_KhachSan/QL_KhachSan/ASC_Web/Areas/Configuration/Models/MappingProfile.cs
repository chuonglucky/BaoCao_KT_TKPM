using ASC.Model.Models;
using ASC_Web.Areas.ServiceRequests.Models;
using AutoMapper;


namespace ASC_Web.Areas.Configuration.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile() {
            // Mappings cho MasterData
            CreateMap<MasterDataKey, MasterDataKeyViewModel>().ReverseMap();
            CreateMap<MasterDataValue, MasterDataValueViewModel>().ReverseMap();

            // Mappings cho ServiceRequest
            CreateMap<CreateServiceRequestViewModel, ServiceRequest>()
                // Các thuộc tính sẽ được gán thủ công trong Controller hoặc có tên khác nhau
                .ForMember(dest => dest.GuestEmail, opt => opt.Ignore()) // Sẽ gán từ User.Identity
                .ForMember(dest => dest.ServiceKeyName, opt => opt.Ignore()) // Sẽ gán từ selectedKey.Name
                .ForMember(dest => dest.ServiceValueName, opt => opt.Ignore()) // Sẽ gán từ selectedValue.Name
                .ForMember(dest => dest.Status, opt => opt.Ignore()) // Sẽ có giá trị mặc định hoặc gán thủ công
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore()) // Sẽ gán từ User.Identity
                .ForMember(dest => dest.Id, opt => opt.Ignore()) // Id thường do DB hoặc BaseEntity tạo
                .ForMember(dest => dest.CreatedDate, opt => opt.Ignore()) // Thường do BaseEntity xử lý
                .ForMember(dest => dest.UpdatedDate, opt => opt.Ignore()) // Thường do BaseEntity xử lý
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())   // Sẽ được gán khi cập nhật
                .ForMember(dest => dest.IsDeleted, opt => opt.Ignore()) // Mặc định là false
                .ForMember(dest => dest.CompletedDate, opt => opt.Ignore())
                .ForMember(dest => dest.AssignedStaffEmail, opt => opt.Ignore())
                .ForMember(dest => dest.StaffRemarks, opt => opt.Ignore())
                // Các thuộc tính còn lại có tên giống nhau sẽ được map tự động:
                // RequestedDate, RequestedEndDate, RequestedServicesDetails
                ;

            CreateMap<ServiceRequest, ServiceRequestDetailsViewModel>();
        }
    }
}
