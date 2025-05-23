using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ASC.Model.Models;

namespace ASC.business.interfaces
{
    public interface IRoomOperations
    {
        Task<IEnumerable<Room>> GetAllRoomsAsync(bool includeRoomType = true);
        Task<Room> GetRoomByIdAsync(Guid id, bool includeRoomType = true);
        Task<(bool Success, string ErrorMessage)> SetRoomStatusAsync(Guid roomId, string newStatus, string updatedBy);
    }
}
