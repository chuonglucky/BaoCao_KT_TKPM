using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.Models;

namespace ASC.business.interfaces
{
    public interface IServiceRequestOperations
    {
        Task<ServiceRequest> CreateServiceRequestAsync(ServiceRequest request);
        Task<ServiceRequest> GetServiceRequestByIdAsync(Guid id);
        Task<IEnumerable<ServiceRequest>> GetAllServiceRequestsAsync(); // Lấy tất cả (có thể phân trang sau)
        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByStatusAsync(string status);
        Task<IEnumerable<ServiceRequest>> GetServiceRequestsByGuestEmailAsync(string guestEmail);
        Task<bool> UpdateServiceRequestStatusAsync(Guid id, string newStatus, string updatedByEmail, string staffRemarks = null);
        Task<bool> AssignStaffToRequestAsync(Guid id, string staffEmail, string updatedByEmail);

        // Có thể thêm các phương thức khác như:
        // Task<IEnumerable<ServiceRequest>> GetRequestsAssignedToStaffAsync(string staffEmail);
        // Task<bool> CancelServiceRequestAsync(Guid id, string cancelledByEmail);
    }
}
