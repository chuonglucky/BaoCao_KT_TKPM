using ASC_Web.Areas.Booking.Models;
using ASC_Web.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ASC.Model.Models;
using ASC.business.interfaces;

namespace ASC_Web.Areas.Booking.Controllers
{
    [Area("Booking")]
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IBookingOperations _bookingOperations; // Thay thế _context
        private readonly UserManager<IdentityUser> _userManager; // Có thể vẫn giữ nếu cần lấy thông tin User cho ViewModel hoặc kiểm tra Role

        public BookingController(IBookingOperations bookingOperations, UserManager<IdentityUser> userManager)
        {
            _bookingOperations = bookingOperations;
            _userManager = userManager;
        }

        public async Task<IActionResult> Create(Guid roomId)
        {
            // Sử dụng helper trong IBookingOperations để lấy thông tin phòng
            var room = await _bookingOperations.GetRoomDetailsForBookingAsync(roomId);

            if (room == null || !(room.Status.Equals("Trống", StringComparison.OrdinalIgnoreCase) || room.Status.Equals("Available", StringComparison.OrdinalIgnoreCase)))
            {
                TempData["ErrorMessage"] = "Phòng không tồn tại hoặc không sẵn sàng để đặt.";
                return RedirectToAction("Index", "Rooms", new { area = "Room" }); // Đảm bảo RoomsController cũng được refactor hoặc link đúng
            }

            var viewModel = new BookingViewModel
            {
                RoomId = room.Id,
                RoomNumber = room.RoomNumber,
                RoomType = room.RoomType?.Name, // RoomType đã được load bởi GetRoomDetailsForBookingAsync
                CheckInDate = DateTime.Today.AddHours(14), // Giờ check-in mặc định
                CheckOutDate = DateTime.Today.AddDays(1).AddHours(12) // Giờ check-out mặc định
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(BookingViewModel model)
        {
            if (ModelState.IsValid) // Các validation cơ bản của ViewModel (Required, StringLength,...)
            {
                // Validate logic ngày tháng trong BLL hoặc ở đây trước khi gọi BLL
                if (model.CheckInDate >= model.CheckOutDate)
                {
                    ModelState.AddModelError("CheckOutDate", "Ngày trả phòng phải sau ngày nhận phòng.");
                }
                if (model.CheckInDate < DateTime.Today) // Chỉ cho phép đặt từ ngày hiện tại trở đi
                {
                    ModelState.AddModelError("CheckInDate", "Không thể gửi yêu cầu đặt phòng cho ngày trong quá khứ.");
                }

                if (ModelState.IsValid) // Kiểm tra lại ModelState sau khi thêm custom validation
                {
                    var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
                    var user = await _userManager.GetUserAsync(User); // Lấy thông tin user (ví dụ: email)

                    var bookingRequest = new ASC.Model.Models.Booking
                    {
                        CustomerName = model.CustomerName, // Hoặc lấy từ thông tin user nếu muốn
                        RoomId = model.RoomId,
                        CheckInDate = model.CheckInDate,
                        CheckOutDate = model.CheckOutDate,
                        UserId = userId,
                        CreatedBy = user?.Email ?? User.Identity.Name, // Ghi lại người tạo
                        UpdatedBy = user?.Email ?? User.Identity.Name,
                        // Status sẽ được BLL set là "PendingApproval"
                    };

                    try
                    {
                        await _bookingOperations.CreateBookingRequestAsync(bookingRequest);
                        TempData["SuccessMessage"] = $"Yêu cầu đặt phòng {model.RoomNumber} của bạn đã được gửi và đang chờ duyệt!";
                        return RedirectToAction(nameof(MyBookings));
                    }
                    catch (Exception ex) // Bắt lỗi từ BLL (ví dụ phòng không tồn tại,...)
                    {
                        ModelState.AddModelError("", $"Lỗi khi gửi yêu cầu: {ex.Message}");
                    }
                }
            }

            // Nếu ModelState không hợp lệ, hoặc có lỗi từ BLL, cần load lại thông tin phòng cho view
            if (model.RoomId != Guid.Empty && (string.IsNullOrEmpty(model.RoomNumber) || string.IsNullOrEmpty(model.RoomType)))
            {
                var roomDetails = await _bookingOperations.GetRoomDetailsForBookingAsync(model.RoomId);
                if (roomDetails != null)
                {
                    model.RoomNumber = roomDetails.RoomNumber;
                    model.RoomType = roomDetails.RoomType?.Name;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> MyBookings()
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            var userBookings = await _bookingOperations.GetUserBookingsAsync(userId);
            return View(userBookings); // Cần View MyBookings.cshtml
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Cancel(Guid id)
        {
            var currentUserId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            bool isAdmin = User.IsInRole("Admin"); // Hoặc các role quản lý khác

            var result = await _bookingOperations.CancelBookingAsync(id, currentUserId, isAdmin);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            return RedirectToAction(nameof(MyBookings));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Staff,Receptionist,Manager")] // Đảm bảo chỉ Staff/Admin mới Checkout được
        public async Task<IActionResult> CheckOut(Guid id)
        {
            // var adminUserId = _userManager.GetUserId(User); // Tương tự, nếu cần User ID
            var result = await _bookingOperations.CheckOutBookingAsync(id, User.Identity.Name);

            if (result.Success)
            {
                TempData["SuccessMessage"] = result.ErrorMessage;
            }
            else
            {
                TempData["ErrorMessage"] = result.ErrorMessage;
            }
            // Admin nên được chuyển hướng về trang quản lý booking của admin
            if (User.IsInRole("Admin") || User.IsInRole("Staff") || User.IsInRole("Receptionist") || User.IsInRole("Manager"))
            {
                return RedirectToAction("AllBookings", "AdminBooking", new { area = "Admin" }); // Hoặc PendingRequests
            }
            return RedirectToAction(nameof(MyBookings)); // User bình thường (nếu có quyền này)
        }
    }
}
