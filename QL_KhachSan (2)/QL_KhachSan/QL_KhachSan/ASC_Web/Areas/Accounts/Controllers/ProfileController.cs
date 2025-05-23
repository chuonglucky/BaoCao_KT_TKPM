using System.Security.Claims;
using ASC_Web.Areas.Accounts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ASC_Web.Areas.Accounts.Controllers
{

    [Area("Accounts")]
    [Authorize]
    public class ProfileController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager; // Thêm SignInManager
        private readonly IWebHostEnvironment _webHostEnvironment;   // Để lấy đường dẫn wwwroot

        public ProfileController(
            UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager, // Inject SignInManager
            IWebHostEnvironment webHostEnvironment)
        {
            _userManager = userManager;
            _signInManager = signInManager; // Gán SignInManager
            _webHostEnvironment = webHostEnvironment;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không thể tìm thấy thông tin người dùng. Vui lòng đăng nhập lại.";
                return RedirectToPage("/Account/Login", new { area = "Identity" });
            }

            var userRoles = await _userManager.GetRolesAsync(user);
            var claims = await _userManager.GetClaimsAsync(user);
            var avatarClaim = claims.FirstOrDefault(c => c.Type == "AvatarUrl");

            var model = new ProfileViewModel
            {
                UserId = user.Id,
                UserName = user.UserName,
                Email = user.Email,
                PhoneNumber = user.PhoneNumber,
                Roles = userRoles.ToList(),
                EmailConfirmed = user.EmailConfirmed,
                LockoutEnd = user.LockoutEnd,
                AvatarUrl = avatarClaim?.Value ?? "/images/default-user-avatar.png"
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditProfile(ProfileViewModel model)
        {
            // Loại bỏ các lỗi validation không liên quan đến việc edit profile này
            ModelState.Remove("Roles");
            ModelState.Remove("Email"); // Email thường không cho sửa trực tiếp ở đây

            if (!ModelState.IsValid)
            {
                // Nếu có lỗi, cần tải lại thông tin cho view (đặc biệt là AvatarUrl và Roles)
                var userForError = await _userManager.GetUserAsync(User);
                var userRolesForError = await _userManager.GetRolesAsync(userForError);
                var claimsForError = await _userManager.GetClaimsAsync(userForError);
                model.AvatarUrl = claimsForError.FirstOrDefault(c => c.Type == "AvatarUrl")?.Value ?? "/images/default-user-avatar.png";
                model.Roles = userRolesForError;
                model.Email = userForError.Email; // Gán lại email để hiển thị đúng
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ. Vui lòng kiểm tra lại.";
                return View("Index", model); // Trả về view Index với model đã có lỗi
            }

            var user = await _userManager.GetUserAsync(User);
            if (user == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy người dùng.";
                return RedirectToAction(nameof(Index));
            }

            bool profileUpdated = false;

            // Cập nhật UserName nếu có thay đổi
            if (!string.IsNullOrWhiteSpace(model.UserName) && user.UserName != model.UserName)
            {
                var setUserNameResult = await _userManager.SetUserNameAsync(user, model.UserName);
                if (!setUserNameResult.Succeeded)
                {
                    foreach (var error in setUserNameResult.Errors) ModelState.AddModelError("UserName", error.Description);
                }
                else
                {
                    await _userManager.UpdateNormalizedUserNameAsync(user); // Quan trọng
                    profileUpdated = true;
                }
            }

            // Cập nhật PhoneNumber nếu có thay đổi
            if (user.PhoneNumber != model.PhoneNumber) // Cho phép cả null và chuỗi rỗng
            {
                var setPhoneResult = await _userManager.SetPhoneNumberAsync(user, model.PhoneNumber);
                if (!setPhoneResult.Succeeded)
                {
                    foreach (var error in setPhoneResult.Errors) ModelState.AddModelError("PhoneNumber", error.Description);
                }
                else
                {
                    profileUpdated = true;
                }
            }


            // Xử lý upload ảnh đại diện
            if (model.AvatarFile != null && model.AvatarFile.Length > 0)
            {
                // Kiểm tra kiểu file và kích thước (ví dụ)
                var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
                var extension = Path.GetExtension(model.AvatarFile.FileName).ToLowerInvariant();
                if (!allowedExtensions.Contains(extension))
                {
                    ModelState.AddModelError("AvatarFile", "Chỉ chấp nhận file ảnh JPG, JPEG, PNG, GIF.");
                }
                else if (model.AvatarFile.Length > 2 * 1024 * 1024) // Giới hạn 2MB
                {
                    ModelState.AddModelError("AvatarFile", "Kích thước file ảnh không được vượt quá 2MB.");
                }
                else
                {
                    // Tạo thư mục nếu chưa có
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads", "avatars");
                    if (!Directory.Exists(uploadsFolder))
                    {
                        Directory.CreateDirectory(uploadsFolder);
                    }

                    // Tạo tên file duy nhất để tránh ghi đè
                    string uniqueFileName = user.Id + "_" + Guid.NewGuid().ToString().Substring(0, 8) + extension;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    try
                    {
                        using (var fileStream = new FileStream(filePath, FileMode.Create))
                        {
                            await model.AvatarFile.CopyToAsync(fileStream);
                        }
                        string newAvatarUrl = "/uploads/avatars/" + uniqueFileName;

                        // Xóa claim AvatarUrl cũ (nếu có)
                        var oldAvatarClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "AvatarUrl");
                        if (oldAvatarClaim != null)
                        {
                            // Tùy chọn: Xóa file avatar cũ trên server
                            if (!string.IsNullOrEmpty(oldAvatarClaim.Value) && oldAvatarClaim.Value != "/images/default-user-avatar.png")
                            {
                                var oldFilePath = Path.Combine(_webHostEnvironment.WebRootPath, oldAvatarClaim.Value.TrimStart('/'));
                                if (System.IO.File.Exists(oldFilePath))
                                {
                                    System.IO.File.Delete(oldFilePath);
                                }
                            }
                            await _userManager.RemoveClaimAsync(user, oldAvatarClaim);
                        }
                        // Thêm claim AvatarUrl mới
                        var addClaimResult = await _userManager.AddClaimAsync(user, new Claim("AvatarUrl", newAvatarUrl));
                        if (!addClaimResult.Succeeded)
                        {
                            ModelState.AddModelError("AvatarFile", "Không thể lưu đường dẫn ảnh đại diện.");
                        }
                        else
                        {
                            profileUpdated = true;
                        }
                    }
                    catch (Exception ex)
                    {
                        // Log lỗi ex
                        ModelState.AddModelError("AvatarFile", "Lỗi khi tải lên ảnh đại diện.");
                    }
                }
            }

            // Nếu có lỗi sau khi xử lý UserName, PhoneNumber hoặc Avatar
            if (!ModelState.IsValid)
            {
                var userForError = await _userManager.GetUserAsync(User);
                var userRolesForError = await _userManager.GetRolesAsync(userForError);
                var claimsForError = await _userManager.GetClaimsAsync(userForError);
                model.AvatarUrl = claimsForError.FirstOrDefault(c => c.Type == "AvatarUrl")?.Value ?? "/images/default-user-avatar.png";
                model.Roles = userRolesForError;
                model.Email = userForError.Email;
                // Giữ lại giá trị UserName và PhoneNumber người dùng đã nhập nếu có lỗi
                // model.UserName và model.PhoneNumber đã được bind từ form
                return View("Index", model);
            }


            // Cập nhật thông tin người dùng trong database nếu có thay đổi
            if (profileUpdated) // Chỉ update nếu có thay đổi thực sự
            {
                var updateResult = await _userManager.UpdateAsync(user);
                if (!updateResult.Succeeded)
                {
                    foreach (var error in updateResult.Errors)
                    {
                        ModelState.AddModelError(string.Empty, error.Description);
                    }
                    // Tải lại thông tin cho view nếu có lỗi
                    var userForError = await _userManager.GetUserAsync(User);
                    var userRolesForError = await _userManager.GetRolesAsync(userForError);
                    var claimsForError = await _userManager.GetClaimsAsync(userForError);
                    model.AvatarUrl = claimsForError.FirstOrDefault(c => c.Type == "AvatarUrl")?.Value ?? "/images/default-user-avatar.png";
                    model.Roles = userRolesForError;
                    model.Email = userForError.Email;
                    return View("Index", model);
                }

                // Nếu UserName thay đổi, cần đăng xuất và yêu cầu đăng nhập lại hoặc refresh cookie
                // Để đơn giản, chúng ta có thể refresh cookie đăng nhập
                await _signInManager.RefreshSignInAsync(user);
                TempData["SuccessMessage"] = "Hồ sơ của bạn đã được cập nhật thành công.";
            }
            else if (!ModelState.Any())
            { // Không có thay đổi nào và không có lỗi
                TempData["InfoMessage"] = "Không có thông tin nào được thay đổi.";
            }


            return RedirectToAction(nameof(Index));
        }
    }
}
