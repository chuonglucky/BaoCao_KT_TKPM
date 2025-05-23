using ASC.business.interfaces;
using ASC_Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ASC_Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin,Staff,Receptionist,Manager")]
    public class AdminBookingController : Controller
    {
        private readonly IBookingOperations _bookingOperations; // Thay thế _context và _userManager

        public AdminBookingController(IBookingOperations bookingOperations) // Inject IBookingOperations
        {
            _bookingOperations = bookingOperations;
        }

        public async Task<IActionResult> PendingRequests()
        {
            var pendingBookings = await _bookingOperations.GetPendingBookingRequestsAsync();
            return View(pendingBookings);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmRequest(Guid bookingId)
        {
           

            var result = await _bookingOperations.ConfirmBookingRequestAsync(bookingId, User.Identity.Name /* Hoặc một định danh admin khác */);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.ErrorMessage; // Message đã bao gồm thông tin phòng
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            return RedirectToAction(nameof(PendingRequests));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> RejectRequest(Guid bookingId)
        {
            var result = await _bookingOperations.RejectBookingRequestAsync(bookingId, User.Identity.Name);
            if (result.Success)
            {
                TempData["SuccessMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            return RedirectToAction(nameof(PendingRequests));
        }

        public async Task<IActionResult> AllBookings()
        {
            var allBookings = await _bookingOperations.GetAllBookingsAsync();
            return View(allBookings); // Cần tạo view AllBookings.cshtml (bạn đã có)
        }
    }
}
