using ASC.business.interfaces;
using ASC.Model.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ASC_Web.Areas.ServiceRequests.Models;

namespace ASC_Web.Areas.ServiceRequests.Controllers
{
    [Area("ServiceRequests")]
    [Authorize]
    public class ServiceRequestController : Controller
    {
        private readonly IServiceRequestOperations _serviceRequestOps;
        private readonly IMasterDataOperations _masterDataOps;
        private readonly IMapper _mapper;

        public ServiceRequestController(
            IServiceRequestOperations serviceRequestOps,
            IMasterDataOperations masterDataOps,
            IMapper mapper)
        {
            _serviceRequestOps = serviceRequestOps;
            _masterDataOps = masterDataOps;
            _mapper = mapper;
        }

        private async Task PopulateAllAvailableServicesAsync(CreateServiceRequestViewModel model, Guid? selectedServiceValueId)
        {
            var serviceItems = new List<SelectListItem>();
            var serviceCategories = (await _masterDataOps.GetMasterDataKeysAsync(includeInactive: false, includeSoftDeleted: false))
                                      .Where(k => k.Name != "Phòng") // LƯU Ý: "Phòng" là tên cứng
                                      .ToList();

            foreach (var category in serviceCategories)
            {
                var values = await _masterDataOps.GetMasterDataValuesAsync(category.Id, includeInactive: false, includeSoftDeleted: false);
                foreach (var value in values)
                {
                    serviceItems.Add(new SelectListItem
                    {
                        Value = value.Id.ToString(),
                        Text = $"{category.Name} - {value.Name}"
                    });
                }
            }
            model.AllAvailableServices = new SelectList(serviceItems.OrderBy(s => s.Text), "Value", "Text", selectedServiceValueId);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var model = new CreateServiceRequestViewModel
            {
                RequestedDate = DateTime.Today
            };
            await PopulateAllAvailableServicesAsync(model, null);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateServiceRequestViewModel model)
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                ModelState.AddModelError("", "Không thể xác định người dùng hiện tại. Vui lòng đăng nhập lại.");
                await PopulateAllAvailableServicesAsync(model, model.SelectedMasterDataValueId);
                return View(model);
            }

            MasterDataValue? selectedServiceValue = null;
            MasterDataKey? parentKey = null;

            if (model.SelectedMasterDataValueId.HasValue && model.SelectedMasterDataValueId.Value != Guid.Empty)
            {
                selectedServiceValue = await _masterDataOps.GetMasterDataValueByIdAsync(model.SelectedMasterDataValueId.Value);
                if (selectedServiceValue == null || !selectedServiceValue.IsActive || selectedServiceValue.IsDeleted)
                {
                    ModelState.AddModelError(nameof(model.SelectedMasterDataValueId), "Dịch vụ được chọn không hợp lệ hoặc không hoạt động.");
                    selectedServiceValue = null;
                }
                else
                {
                    parentKey = await _masterDataOps.GetMasterDataKeyByIdAsync(selectedServiceValue.MasterDataKeyId);
                    if (parentKey == null || !parentKey.IsActive || parentKey.IsDeleted || parentKey.Name == "Phòng")
                    {
                        ModelState.AddModelError(nameof(model.SelectedMasterDataValueId), "Loại dịch vụ của dịch vụ được chọn không hợp lệ.");
                        selectedServiceValue = null;
                        parentKey = null;
                    }
                }
            }

            if (model.RequestedDate.Date < DateTime.Today)
            {
                ModelState.AddModelError(nameof(model.RequestedDate), "Ngày mong muốn/check-in không được là ngày trong quá khứ.");
            }

            if (model.RequestedEndDate.HasValue && model.RequestedEndDate.Value.Date < model.RequestedDate.Date)
            {
                ModelState.AddModelError(nameof(model.RequestedEndDate), "Ngày kết thúc/check-out phải lớn hơn hoặc bằng ngày mong muốn/check-in.");
            }

            if (ModelState.IsValid)
            {
                var serviceRequest = _mapper.Map<ServiceRequest>(model); // Đảm bảo ServiceRequest model có các thuộc tính tương ứng từ ViewModel (RequestedDate, RequestedServicesDetails, etc.)
                serviceRequest.GuestEmail = currentUserEmail;
                serviceRequest.CreatedBy = currentUserEmail;

                // Gán tên cho ServiceRequest (ID được tạm thời bình luận)
                serviceRequest.ServiceValueName = selectedServiceValue?.Name;   // Giả định ServiceRequest có thuộc tính này
                serviceRequest.ServiceKeyName = parentKey?.Name;            // Giả định ServiceRequest có thuộc tính này

                // !! CÁC DÒNG DƯỚI ĐÂY ĐƯỢC TẠM THỜI BÌNH LUẬN CHO ĐẾN KHI CÓ CẤU TRÚC MODEL ServiceRequest !!
                // serviceRequest.MasterDataValueId = selectedServiceValue?.Id; 
                // serviceRequest.MasterDataKeyId = parentKey?.Id;          

                try
                {
                    var createdRequest = await _serviceRequestOps.CreateServiceRequestAsync(serviceRequest);
                    TempData["SuccessMessage"] = $"Yêu cầu '{createdRequest.ServiceValueName ?? createdRequest.ServiceKeyName}' của bạn đã được gửi thành công và đang chờ xác nhận.";
                    return RedirectToAction(nameof(MyRequests));
                }
                catch (Exception ex)
                {
                    // Log lỗi (ex) chi tiết hơn ở đây
                    ModelState.AddModelError("", "Đã xảy ra lỗi khi gửi yêu cầu của bạn: " + ex.Message);
                }
            }

            await PopulateAllAvailableServicesAsync(model, model.SelectedMasterDataValueId);
            return View(model);
        }

        // Các actions khác (MyRequests, PendingList, Details, etc.) giữ nguyên như lần hoàn thiện trước
        [HttpGet]
        public async Task<IActionResult> MyRequests()
        {
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(currentUserEmail))
            {
                return Unauthorized("Vui lòng đăng nhập để xem yêu cầu.");
            }
            var requests = await _serviceRequestOps.GetServiceRequestsByGuestEmailAsync(currentUserEmail);
            var model = _mapper.Map<IEnumerable<ServiceRequestDetailsViewModel>>(requests);
            return View(model.OrderByDescending(r => r.CreatedDate));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Receptionist,Manager")]
        public async Task<IActionResult> PendingList()
        {
            var requests = await _serviceRequestOps.GetServiceRequestsByStatusAsync(ServiceRequest.Statuses.PendingConfirmation);
            var model = _mapper.Map<IEnumerable<ServiceRequestDetailsViewModel>>(requests);
            return View(model.OrderByDescending(r => r.CreatedDate));
        }

        [HttpGet]
        [Authorize(Roles = "Admin,Manager")]
        public async Task<IActionResult> AllRequests()
        {
            var requests = await _serviceRequestOps.GetAllServiceRequestsAsync();
            var model = _mapper.Map<IEnumerable<ServiceRequestDetailsViewModel>>(requests);
            return View(model.OrderByDescending(r => r.CreatedDate));
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var request = await _serviceRequestOps.GetServiceRequestByIdAsync(id);
            if (request == null) return NotFound("Không tìm thấy yêu cầu.");

            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (!User.IsInRole("Admin") && !User.IsInRole("Manager") && !User.IsInRole("Receptionist") &&
                (string.IsNullOrEmpty(currentUserEmail) || request.GuestEmail != currentUserEmail))
            {
                return Forbid("Bạn không có quyền xem yêu cầu này.");
            }

            var model = _mapper.Map<ServiceRequestDetailsViewModel>(request);
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Receptionist,Manager")]
        public async Task<IActionResult> Approve(Guid id, string? staffRemarks)
        {
            if (id == Guid.Empty) return NotFound();
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(currentUserEmail)) return Unauthorized();

            var success = await _serviceRequestOps.UpdateServiceRequestStatusAsync(id, ServiceRequest.Statuses.Confirmed, currentUserEmail, staffRemarks);
            if (success)
            {
                TempData["SuccessMessage"] = "Yêu cầu đã được chấp nhận.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể chấp nhận yêu cầu. Yêu cầu có thể đã được xử lý hoặc không tồn tại.";
            }
            return RedirectToAction(nameof(PendingList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Receptionist,Manager")]
        public async Task<IActionResult> Reject(Guid id, string staffRemarks)
        {
            if (id == Guid.Empty) return NotFound();

            if (string.IsNullOrWhiteSpace(staffRemarks))
            {
                TempData["ErrorMessage"] = "Vui lòng cung cấp lý do từ chối.";
                TempData["RejectErrorForId_" + id] = "Vui lòng cung cấp lý do từ chối."; // Dùng để hiển thị lỗi trên modal nếu có
                return RedirectToAction(nameof(PendingList));
            }

            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(currentUserEmail)) return Unauthorized();

            var success = await _serviceRequestOps.UpdateServiceRequestStatusAsync(id, ServiceRequest.Statuses.Rejected, currentUserEmail, staffRemarks);
            if (success)
            {
                TempData["SuccessMessage"] = "Yêu cầu đã được từ chối.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể từ chối yêu cầu. Yêu cầu có thể đã được xử lý hoặc không tồn tại.";
            }
            return RedirectToAction(nameof(PendingList));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin,Receptionist,Manager,Housekeeping")]
        public async Task<IActionResult> Complete(Guid id, string? staffRemarks)
        {
            if (id == Guid.Empty) return NotFound();
            var currentUserEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(currentUserEmail)) return Unauthorized();

            var success = await _serviceRequestOps.UpdateServiceRequestStatusAsync(id, ServiceRequest.Statuses.Completed, currentUserEmail, staffRemarks);
            if (success)
            {
                TempData["SuccessMessage"] = "Yêu cầu đã được đánh dấu hoàn thành.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không thể đánh dấu hoàn thành yêu cầu.";
            }

            if (User.IsInRole("Housekeeping"))
            {
                return RedirectToAction(nameof(PendingList));
            }
            return RedirectToAction(nameof(AllRequests));
        }
    }
}
