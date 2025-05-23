using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.Models;

namespace ASC.business.interfaces
{
    public interface IBookingOperations
    {
        Task<Booking> CreateBookingRequestAsync(Booking bookingRequest);
        Task<Booking> GetBookingByIdAsync(Guid id, bool includeRoomAndType = false, bool includeUser = false);
        Task<IEnumerable<Booking>> GetPendingBookingRequestsAsync();
        Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId);
        Task<(bool Success, string ErrorMessage, Booking ConfirmedBooking)> ConfirmBookingRequestAsync(Guid bookingId, string adminUserId);
        Task<(bool Success, string ErrorMessage)> RejectBookingRequestAsync(Guid bookingId, string adminUserId);
        Task<(bool Success, string ErrorMessage)> CancelBookingAsync(Guid bookingId, string currentUserId, bool isAdmin);
        Task<(bool Success, string ErrorMessage)> CheckOutBookingAsync(Guid bookingId, string adminUserId);
        Task<IEnumerable<Booking>> GetAllBookingsAsync();
        Task<Room> GetRoomDetailsForBookingAsync(Guid roomId); // Helper để lấy thông tin phòng cho form đặt
    }
}
