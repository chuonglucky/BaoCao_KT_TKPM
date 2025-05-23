using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using ASC.business.interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Models;
using Microsoft.EntityFrameworkCore;


namespace ASC.business
{
    public class BookingOperations : IBookingOperations
    {
        private readonly IUnitOfWork _uow;
   
        public BookingOperations(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<Room> GetRoomDetailsForBookingAsync(Guid roomId)
        {
            if (roomId == Guid.Empty) return null;
            var rooms = await _uow.Repository<Room>().FindAllByQuery(
                filter: r => r.Id == roomId && !r.IsDeleted,
                include: query => query.Include(r => r.RoomType)
            );
            return rooms.FirstOrDefault();
        }

        public async Task<Booking> CreateBookingRequestAsync(Booking bookingRequest)
        {
            if (bookingRequest == null) throw new ArgumentNullException(nameof(bookingRequest));
            if (bookingRequest.RoomId == Guid.Empty) throw new ArgumentException("RoomId is required.", nameof(bookingRequest.RoomId));
            if (bookingRequest.CheckInDate == null || bookingRequest.CheckOutDate == null) throw new ArgumentException("CheckInDate and CheckOutDate are required.");
            if (bookingRequest.CheckInDate >= bookingRequest.CheckOutDate) throw new ArgumentException("CheckOutDate must be after CheckInDate.");
            if (bookingRequest.CheckInDate < DateTime.Today) throw new ArgumentException("Cannot request booking for a past date.");

            var room = await _uow.Repository<Room>().FindAsync(bookingRequest.RoomId);
            if (room == null || room.IsDeleted)
            {
                throw new InvalidOperationException("Room not found or has been deleted.");
            }

            bookingRequest.Status = "PendingApproval";
         
            await _uow.Repository<Booking>().AddAsync(bookingRequest);
            await _uow.SaveChangesAsync();
            return bookingRequest;
        }

        public async Task<Booking> GetBookingByIdAsync(Guid id, bool includeRoomAndType = false, bool includeUser = false)
        {
            if (id == Guid.Empty) return null;

            Func<IQueryable<Booking>, IQueryable<Booking>> includeFunction = null;

            if (includeRoomAndType && includeUser)
            {
                includeFunction = query => query.Include(b => b.Room).ThenInclude(r => r.RoomType).Include(b => b.User);
            }
            else if (includeRoomAndType)
            {
                includeFunction = query => query.Include(b => b.Room).ThenInclude(r => r.RoomType);
            }
            else if (includeUser)
            {
                includeFunction = query => query.Include(b => b.User);
            }

            var bookings = await _uow.Repository<Booking>().FindAllByQuery(
                filter: b => b.Id == id && !b.IsDeleted,
                include: includeFunction
            );
            return bookings.FirstOrDefault();
        }

        public async Task<IEnumerable<Booking>> GetPendingBookingRequestsAsync()
        {
            return await _uow.Repository<Booking>().FindAllByQuery(
                filter: b => b.Status == "PendingApproval" && !b.IsDeleted,
                orderBy: o => o.OrderBy(b => b.CreatedDate),
                include: query => query.Include(b => b.Room).ThenInclude(r => r.RoomType).Include(b => b.User)
            );
        }

        public async Task<IEnumerable<Booking>> GetUserBookingsAsync(string userId)
        {
            if (string.IsNullOrWhiteSpace(userId)) return Enumerable.Empty<Booking>();
            return await _uow.Repository<Booking>().FindAllByQuery(
               filter: b => b.UserId == userId && !b.IsDeleted,
               orderBy: o => o.OrderByDescending(b => b.CreatedDate),
               include: query => query.Include(b => b.Room).ThenInclude(r => r.RoomType)
           );
        }

        public async Task<(bool Success, string ErrorMessage, Booking ConfirmedBooking)> ConfirmBookingRequestAsync(Guid bookingId, string adminUserId)
        {
            if (bookingId == Guid.Empty) return (false, "BookingId không hợp lệ.", null);

            var bookingRequest = await GetBookingByIdAsync(bookingId, includeRoomAndType: true); // Lấy kèm Room

            if (bookingRequest == null || bookingRequest.Status != "PendingApproval")
            {
                return (false, "Yêu cầu đặt phòng không hợp lệ hoặc đã được xử lý.", null);
            }

            var room = bookingRequest.Room; // Room đã được eager load bởi GetBookingByIdAsync
            if (room == null)
            {
                bookingRequest.Status = "Rejected";
                bookingRequest.UpdatedBy = adminUserId;
                _uow.Repository<Booking>().Update(bookingRequest);
                await _uow.SaveChangesAsync();
                return (false, "Phòng liên quan đến yêu cầu này không tồn tại và yêu cầu đã bị từ chối.", null);
            }

            if (!(room.Status.Equals("Trống", StringComparison.OrdinalIgnoreCase) ||
                  room.Status.Equals("Available", StringComparison.OrdinalIgnoreCase)))
            {
                return (false, $"Phòng {room.RoomNumber} hiện không trống ({room.Status}). Không thể xác nhận yêu cầu này.", null);
            }

            var conflictingBookings = await _uow.Repository<Booking>().FindAllByQuery(
                b => b.RoomId == room.Id &&
                     b.Id != bookingRequest.Id &&
                     b.Status == "Confirmed" && !b.IsDeleted &&
                     bookingRequest.CheckInDate < b.CheckOutDate &&
                     bookingRequest.CheckOutDate > b.CheckInDate
            );

            if (conflictingBookings.Any())
            {
                return (false, $"Phòng {room.RoomNumber} đã có lịch đặt được xác nhận trùng với thời gian của yêu cầu này.", null);
            }

            bookingRequest.Status = "Confirmed";
            bookingRequest.UpdatedBy = adminUserId; // Ghi nhận người cập nhật

            room.Status = "Booked";
            room.CheckInDate = bookingRequest.CheckInDate;
            room.CheckOutDate = bookingRequest.CheckOutDate;
            room.UpdatedBy = adminUserId; // Ghi nhận người cập nhật

            _uow.Repository<Booking>().Update(bookingRequest);
            _uow.Repository<Room>().Update(room);

            await _uow.SaveChangesAsync();
            return (true, $"Đã xác nhận yêu cầu đặt phòng cho phòng {room.RoomNumber}.", bookingRequest);
        }

        public async Task<(bool Success, string ErrorMessage)> RejectBookingRequestAsync(Guid bookingId, string adminUserId)
        {
            if (bookingId == Guid.Empty) return (false, "BookingId không hợp lệ.");

            var bookingRequest = await _uow.Repository<Booking>().FindAsync(bookingId); // Không cần include ở đây
            if (bookingRequest == null || bookingRequest.IsDeleted || bookingRequest.Status != "PendingApproval")
            {
                return (false, "Yêu cầu đặt phòng không hợp lệ hoặc đã được xử lý.");
            }

            bookingRequest.Status = "Rejected";
            bookingRequest.UpdatedBy = adminUserId;
            _uow.Repository<Booking>().Update(bookingRequest);
            await _uow.SaveChangesAsync();
            return (true, "Đã từ chối yêu cầu đặt phòng.");
        }
        public async Task<(bool Success, string ErrorMessage)> CancelBookingAsync(Guid bookingId, string currentUserId, bool isAdmin)
        {
            var booking = await GetBookingByIdAsync(bookingId, includeRoomAndType: true); // Lấy kèm Room

            if (booking == null)
            {
                return (false, "Không tìm thấy yêu cầu đặt phòng.");
            }

            if (!isAdmin && booking.UserId != currentUserId)
            {
                return (false, "Bạn không có quyền hủy yêu cầu này.");
            }

            if (booking.Status == "PendingApproval" || (booking.Status == "Confirmed" && booking.CheckInDate.HasValue && booking.CheckInDate.Value > DateTime.Now))
            {
                string originalStatus = booking.Status;
                booking.Status = "Cancelled";
                booking.UpdatedBy = currentUserId;

                if (originalStatus == "Confirmed" && booking.Room != null && !booking.Room.IsDeleted && booking.Room.Status != "Bảo trì")
                {
                    if (booking.Room.CheckInDate == booking.CheckInDate && booking.Room.CheckOutDate == booking.CheckOutDate)
                    {
                        booking.Room.Status = "Trống";
                        booking.Room.CheckInDate = null;
                        booking.Room.CheckOutDate = null;
                        booking.Room.UpdatedBy = currentUserId;
                        _uow.Repository<Room>().Update(booking.Room);
                    }
                }
                _uow.Repository<Booking>().Update(booking);
                await _uow.SaveChangesAsync();
                return (true, "Hủy yêu cầu/đặt phòng thành công.");
            }
            else
            {
                return (false, "Không thể hủy yêu cầu/đặt phòng này ở trạng thái hiện tại (có thể đã quá hạn check-in hoặc trạng thái không phù hợp).");
            }
        }

        public async Task<(bool Success, string ErrorMessage)> CheckOutBookingAsync(Guid bookingId, string adminUserId)
        {
            var booking = await GetBookingByIdAsync(bookingId, includeRoomAndType: true); // Lấy kèm Room

            if (booking == null)
            {
                return (false, "Không tìm thấy đặt phòng.");
            }

            if (booking.Status == "Confirmed" || booking.Status == "CheckedIn")
            {
                booking.Status = "CheckedOut";
                booking.UpdatedBy = adminUserId;

                if (booking.Room != null && !booking.Room.IsDeleted && booking.Room.Status != "Bảo trì")
                {
                    if (booking.Room.CheckInDate == booking.CheckInDate && booking.Room.CheckOutDate == booking.CheckOutDate)
                    {
                        booking.Room.Status = "Trống"; // Hoặc "Cleaning" nếu có quy trình dọn dẹp
                        booking.Room.CheckInDate = null;
                        booking.Room.CheckOutDate = null;
                        booking.Room.UpdatedBy = adminUserId;
                        _uow.Repository<Room>().Update(booking.Room);
                    }
                }
                _uow.Repository<Booking>().Update(booking);
                await _uow.SaveChangesAsync();
                return (true, $"Khách hàng {booking.CustomerName} đã trả phòng {booking.Room?.RoomNumber}.");
            }
            else
            {
                return (false, $"Đặt phòng này không ở trạng thái có thể trả phòng (Trạng thái hiện tại: {booking.Status}).");
            }
        }
        public async Task<IEnumerable<Booking>> GetAllBookingsAsync()
        {
            return await _uow.Repository<Booking>().FindAllByQuery(
               filter: b => !b.IsDeleted,
               orderBy: o => o.OrderByDescending(b => b.CreatedDate),
               include: query => query.Include(b => b.Room).ThenInclude(r => r.RoomType).Include(b => b.User)
           );
        }
    }
}
