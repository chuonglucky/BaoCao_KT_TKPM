using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.business.interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Models;

namespace ASC.business
{
    public class ServiceRequestOperations : IServiceRequestOperations
    {
        private readonly IUnitOfWork _uow;
        private readonly IRepository<ServiceRequest> _serviceRequestRepo;

        public ServiceRequestOperations(IUnitOfWork uow)
        {
            _uow = uow;
            _serviceRequestRepo = _uow.Repository<ServiceRequest>();
        }

        public async Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request)
        {
            if (request == null)
                throw new ArgumentNullException(nameof(request));

            // Validate dữ liệu cơ bản (có thể thêm nhiều validation ở đây hoặc dùng FluentValidation)
            if (string.IsNullOrWhiteSpace(request.GuestEmail))
                throw new ArgumentException("GuestEmail is required.", nameof(request.GuestEmail));
            if (string.IsNullOrWhiteSpace(request.ServiceKeyName))
                throw new ArgumentException("ServiceKeyName is required.", nameof(request.ServiceKeyName));
            if (string.IsNullOrWhiteSpace(request.ServiceValueName))
                throw new ArgumentException("ServiceValueName is required.", nameof(request.ServiceValueName));

            request.Status = ServiceRequest.Statuses.PendingConfirmation; // Trạng thái ban đầu
                                                                          // CreatedDate, UpdatedDate, CreatedBy, UpdatedBy sẽ được xử lý bởi BaseEntity và Repository

            var addedRequest = await _serviceRequestRepo.AddAsync(request);
            await _uow.SaveChangesAsync();
            return addedRequest;
        }

        public async Task<ServiceRequest> GetServiceRequestByIdAsync(Guid id)
        {
            if (id == Guid.Empty) return null;
            // Nếu có navigation properties (MasterDataKey, MasterDataValue), có thể dùng Include:
            // return await _serviceRequestRepo.GetQueryable().Include(sr => sr.MasterDataKey).FirstOrDefaultAsync(sr => sr.Id == id && !sr.IsDeleted);
            return await _serviceRequestRepo.FindAsync(id);
        }

        public async Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync()
        {
            // Sắp xếp theo ngày yêu cầu mới nhất
            return (await _serviceRequestRepo.FindAllAsync()).OrderByDescending(sr => sr.RequestedDate);
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByStatusAsync(string status)
        {
            if (string.IsNullOrWhiteSpace(status))
                return Enumerable.Empty<ServiceRequest>();

            return (await _serviceRequestRepo.FindAllByQuery(sr => sr.Status == status && !sr.IsDeleted))
                   .OrderByDescending(sr => sr.RequestedDate);
        }

        public async Task<IEnumerable<ServiceRequest>> GetServiceRequestsByGuestEmailAsync(string guestEmail)
        {
            if (string.IsNullOrWhiteSpace(guestEmail))
                return Enumerable.Empty<ServiceRequest>();

            return (await _serviceRequestRepo.FindAllByQuery(sr => sr.GuestEmail.ToLower() == guestEmail.ToLower() && !sr.IsDeleted))
                   .OrderByDescending(sr => sr.RequestedDate);
        }

        public async Task<bool> UpdateServiceRequestStatusAsync(Guid id, string newStatus, string updatedByEmail, string staffRemarks = null)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(newStatus) || string.IsNullOrWhiteSpace(updatedByEmail))
                return false;

            var request = await _serviceRequestRepo.FindAsync(id);
            if (request == null || request.IsDeleted)
                return false;

            // Kiểm tra logic chuyển đổi trạng thái hợp lệ (ví dụ: không thể từ Completed về Pending)
            // (Logic này có thể phức tạp hơn tùy yêu cầu)
            // if (request.Status == ServiceRequest.Statuses.Completed && newStatus != ServiceRequest.Statuses.Completed)
            //    throw new InvalidOperationException("Cannot change status of a completed request.");

            request.Status = newStatus;
            request.UpdatedBy = updatedByEmail;
            request.UpdatedDate = DateTime.UtcNow; // Repository.Update sẽ tự làm điều này
            if (staffRemarks != null)
            {
                request.StaffRemarks = staffRemarks;
            }

            if (newStatus == ServiceRequest.Statuses.Completed)
            {
                request.CompletedDate = DateTime.UtcNow;
            }
            if (newStatus == ServiceRequest.Statuses.Confirmed && string.IsNullOrEmpty(request.AssignedStaffEmail))
            {
                request.AssignedStaffEmail = updatedByEmail; // Tự gán người xác nhận nếu chưa có ai được gán
            }


            _serviceRequestRepo.Update(request);
            return await _uow.SaveChangesAsync() > 0;
        }

        public async Task<bool> AssignStaffToRequestAsync(Guid id, string staffEmail, string updatedByEmail)
        {
            if (id == Guid.Empty || string.IsNullOrWhiteSpace(staffEmail) || string.IsNullOrWhiteSpace(updatedByEmail))
                return false;

            var request = await _serviceRequestRepo.FindAsync(id);
            if (request == null || request.IsDeleted)
                return false;

            request.AssignedStaffEmail = staffEmail;
            request.UpdatedBy = updatedByEmail;
            _serviceRequestRepo.Update(request);
            return await _uow.SaveChangesAsync() > 0;
        }
    }
}
