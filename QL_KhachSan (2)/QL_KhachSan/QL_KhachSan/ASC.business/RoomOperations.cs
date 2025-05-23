using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.business.interfaces;
using ASC.DataAccess.Interfaces;
using ASC.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace ASC.business
{
    public class RoomOperations : IRoomOperations
    {
        private readonly IUnitOfWork _uow;

        public RoomOperations(IUnitOfWork uow)
        {
            _uow = uow ?? throw new ArgumentNullException(nameof(uow));
        }

        public async Task<IEnumerable<Room>> GetAllRoomsAsync(bool includeRoomType = true)
        {
            Func<IQueryable<Room>, IQueryable<Room>> includeFunc = null;
            if (includeRoomType)
            {
                includeFunc = query => query.Include(r => r.RoomType);
            }

            return await _uow.Repository<Room>().FindAllByQuery(
                filter: r => !r.IsDeleted,
                orderBy: o => o.OrderBy(r => r.RoomNumber), // Sắp xếp theo số phòng
                include: includeFunc
            );
        }

        public async Task<Room> GetRoomByIdAsync(Guid id, bool includeRoomType = true)
        {
            if (id == Guid.Empty) return null;

            Func<IQueryable<Room>, IQueryable<Room>> includeFunc = null;
            if (includeRoomType)
            {
                includeFunc = query => query.Include(r => r.RoomType);
            }

            var rooms = await _uow.Repository<Room>().FindAllByQuery(
                filter: r => r.Id == id && !r.IsDeleted,
                include: includeFunc
            );
            return rooms.FirstOrDefault();
        }

        public async Task<(bool Success, string ErrorMessage)> SetRoomStatusAsync(Guid roomId, string newStatus, string updatedBy)
        {
            if (roomId == Guid.Empty || string.IsNullOrWhiteSpace(newStatus))
            {
                return (false, "Thông tin không hợp lệ.");
            }

            var room = await _uow.Repository<Room>().FindAsync(roomId);
            if (room == null || room.IsDeleted)
            {
                return (false, "Không tìm thấy phòng hoặc phòng đã bị xóa.");
            }

            if (newStatus.Equals("Bảo trì", StringComparison.OrdinalIgnoreCase) &&
                (room.Status.Equals("Đang sử dụng", StringComparison.OrdinalIgnoreCase) || room.Status.Equals("Booked", StringComparison.OrdinalIgnoreCase)))
            {
                var activeOrConfirmedBooking = await _uow.Repository<Booking>().FindAllByQuery(
                    b => b.RoomId == roomId && !b.IsDeleted &&
                         (b.Status == "CheckedIn" || b.Status == "Confirmed") &&
                         DateTime.Now < b.CheckOutDate // Booking vẫn còn hiệu lực
                );
                if (activeOrConfirmedBooking.Any())
                {
                    return (false, $"Phòng {room.RoomNumber} đang có khách hoặc đã được đặt. Không thể chuyển sang bảo trì.");
                }
            }

            room.Status = newStatus;
            room.UpdatedBy = updatedBy; // Ghi nhận người cập nhật

            if (newStatus.Equals("Trống", StringComparison.OrdinalIgnoreCase) || newStatus.Equals("Available", StringComparison.OrdinalIgnoreCase))
            {
              
                if (room.Status.Equals("Bảo trì", StringComparison.OrdinalIgnoreCase)) // Chỉ clear nếu trước đó là bảo trì và giờ chuyển về trống
                {
                    room.CheckInDate = null;
                    room.CheckOutDate = null;
                }
            }


            _uow.Repository<Room>().Update(room);
            await _uow.SaveChangesAsync();
            return (true, $"Cập nhật trạng thái phòng {room.RoomNumber} thành '{newStatus}' thành công.");
        }
    }
}
