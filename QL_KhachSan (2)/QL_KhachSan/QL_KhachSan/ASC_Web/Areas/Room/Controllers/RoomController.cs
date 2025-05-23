using ASC.business.interfaces;
using ASC_Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASC_Web.Areas.Room.Controllers
{
    [Area("Room")]
    [Authorize]
    public class RoomsController : Controller
    {
        private readonly IRoomOperations _roomOperations; // Thay thế ApplicationDbContext

        public RoomsController(IRoomOperations roomOperations) // Inject IRoomOperations
        {
            _roomOperations = roomOperations;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SetMaintenanceStatus(Guid roomId, string newStatus)
        {
            var updatedBy = User.Identity.Name; // Hoặc một cách lấy định danh người dùng khác

            var result = await _roomOperations.SetRoomStatusAsync(roomId, newStatus, updatedBy);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.ErrorMessage; // Message đã bao gồm thông tin phòng
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            return RedirectToAction(nameof(Details), new { id = roomId });
  
        }

        public async Task<IActionResult> Index()
        {
            var rooms = await _roomOperations.GetAllRoomsAsync(includeRoomType: true);
            return View(rooms);
        }

        public async Task<IActionResult> Details(Guid? id)
        {
            if (id == null || id == Guid.Empty)
            {
                return NotFound();
            }
            var room = await _roomOperations.GetRoomByIdAsync(id.Value, includeRoomType: true);
            if (room == null)
            {
                return NotFound();
            }
            return View(room);
        }
    }
}
