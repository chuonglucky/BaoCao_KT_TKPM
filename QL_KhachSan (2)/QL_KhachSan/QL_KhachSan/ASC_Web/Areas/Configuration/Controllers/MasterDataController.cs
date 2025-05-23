using ASC.business.interfaces;
using ASC.Model.Models;
using ASC.Utilities;
using ASC_Web.Areas.Configuration.Models;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace ASC_Web.Areas.Configuration.Controllers
{
    [Area("Configuration")]
    [Authorize(Roles = "Admin")]
    public class MasterDataController : Controller
    {
        private readonly IMasterDataOperations _masterData;
        private readonly IMapper _mapper;

        public MasterDataController(IMasterDataOperations masterData, IMapper mapper)
        {
            _masterData = masterData;
            _mapper = mapper;
        }

        // GET: MasterKeys
        [HttpGet]
        public async Task<IActionResult> MasterKeys()
        {
            var masterKeysEntities = await _masterData.GetMasterDataKeysAsync(includeInactive: false, includeSoftDeleted: false);
            var masterKeysViewModel = _mapper.Map<List<MasterDataKeyViewModel>>(masterKeysEntities);

            var model = new MasterKeysViewModel
            {
                MasterKeys = masterKeysViewModel.ToList(),
                MasterKeyInContext = new MasterDataKeyViewModel { IsActive = true },
                IsEdit = false
            };
            return View(model);
        }

        // POST: MasterKeys/CreateOrEditMasterKey
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEditMasterKey(MasterKeysViewModel model)
        {
            if (model.MasterKeyInContext == null)
            {
                TempData["ErrorMessage"] = "Dữ liệu không hợp lệ.";
                var masterKeysEntitiesForError = await _masterData.GetMasterDataKeysAsync(false, false);
                model.MasterKeys = _mapper.Map<List<MasterDataKeyViewModel>>(masterKeysEntitiesForError);
                model.MasterKeyInContext = new MasterDataKeyViewModel { IsActive = true };
                return View("MasterKeys", model);
            }

            if (string.IsNullOrWhiteSpace(model.MasterKeyInContext.Name))
            {
                ModelState.AddModelError("MasterKeyInContext.Name", "Tên Key Chính không được để trống.");
            }

            if (!ModelState.IsValid)
            {
                var masterKeysEntities = await _masterData.GetMasterDataKeysAsync(false, false);
                model.MasterKeys = _mapper.Map<List<MasterDataKeyViewModel>>(masterKeysEntities);
                return View("MasterKeys", model);
            }

            var masterKeyEntity = _mapper.Map<MasterDataKey>(model.MasterKeyInContext);

            try
            {
                if (model.IsEdit)
                {
                    if (masterKeyEntity.Id == Guid.Empty)
                    {
                        ModelState.AddModelError("", "Không tìm thấy Master Key để cập nhật.");
                        var keysEntities = await _masterData.GetMasterDataKeysAsync(false, false);
                        model.MasterKeys = _mapper.Map<List<MasterDataKeyViewModel>>(keysEntities);
                        return View("MasterKeys", model);
                    }
                    masterKeyEntity.UpdatedBy = HttpContext.User.GetCurrentUserDetails()?.Name;
                    await _masterData.UpdateMasterDataKeyAsync(masterKeyEntity);
                    TempData["SuccessMessage"] = "Cập nhật Master Key thành công!";
                }
                else
                {
                    masterKeyEntity.CreatedBy = HttpContext.User.GetCurrentUserDetails()?.Name;
                    await _masterData.AddMasterDataKeyAsync(masterKeyEntity);
                    TempData["SuccessMessage"] = "Thêm mới Master Key thành công!";
                }
                return RedirectToAction(nameof(MasterKeys));
            }
            catch (InvalidOperationException ex)
            {
                ModelState.AddModelError("MasterKeyInContext.Name", ex.Message);
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", $"Đã xảy ra lỗi: {ex.Message}");
            }

            var currentMasterKeys = await _masterData.GetMasterDataKeysAsync(false, false);
            model.MasterKeys = _mapper.Map<List<MasterDataKeyViewModel>>(currentMasterKeys);
            return View("MasterKeys", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditMasterKey(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            var masterKeyEntity = await _masterData.GetMasterDataKeyByIdAsync(id, includeSoftDeleted: true);
            if (masterKeyEntity == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy Master Key.";
                return RedirectToAction(nameof(MasterKeys));
            }
            if (masterKeyEntity.IsDeleted)
            {
                TempData["InfoMessage"] = "Master Key này đã bị xóa mềm. Cân nhắc khôi phục nếu muốn chỉnh sửa.";
            }

            var allMasterKeys = await _masterData.GetMasterDataKeysAsync(false, false);
            var model = new MasterKeysViewModel
            {
                MasterKeys = _mapper.Map<List<MasterDataKeyViewModel>>(allMasterKeys),
                MasterKeyInContext = _mapper.Map<MasterDataKeyViewModel>(masterKeyEntity),
                IsEdit = true
            };
            return View("MasterKeys", model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMasterKey(Guid id)
        {
            if (id == Guid.Empty) return NotFound();
            try
            {
                await _masterData.DeleteMasterDataKeyAsync(id);
                TempData["SuccessMessage"] = "Xóa Master Key thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa Master Key: {ex.Message}";
            }
            return RedirectToAction(nameof(MasterKeys));
        }

        [HttpGet]
        public async Task<IActionResult> MasterValues(Guid? SelectedMasterDataKeyId)
        {
            var viewModel = new MasterValuesViewModel
            {
                SelectedMasterDataKeyId = SelectedMasterDataKeyId, // Gán trực tiếp Guid?
                MasterValueInContext = new MasterDataValueViewModel
                {
                    IsActive = true,
                    // MasterDataKeyId trong MasterDataValueViewModel là Guid (non-nullable)
                    // Gán giá trị từ SelectedMasterDataKeyId.Value nếu nó có giá trị, ngược lại là Guid.Empty
                    MasterDataKeyId = SelectedMasterDataKeyId.HasValue ? SelectedMasterDataKeyId.Value : Guid.Empty
                }
            };

            var allMasterKeys = await _masterData.GetMasterDataKeysAsync(includeInactive: false, includeSoftDeleted: false);
            ViewBag.MasterKeysSelectList = new SelectList(allMasterKeys, "Id", "Name", SelectedMasterDataKeyId);

            if (SelectedMasterDataKeyId.HasValue && SelectedMasterDataKeyId.Value != Guid.Empty)
            {
                var selectedKeyEntity = allMasterKeys.FirstOrDefault(k => k.Id == SelectedMasterDataKeyId.Value);
                if (selectedKeyEntity != null)
                {
                    viewModel.SelectedMasterDataKeyName = selectedKeyEntity.Name;
                    var valuesEntities = await _masterData.GetMasterDataValuesAsync(selectedKeyEntity.Id, includeInactive: false, includeSoftDeleted: false);
                    viewModel.MasterValues = _mapper.Map<List<MasterDataValueViewModel>>(valuesEntities);
                    viewModel.MasterValueInContext.MasterDataKeyId = selectedKeyEntity.Id; // Đảm bảo gán lại ID của key cha đã chọn
                }
                else
                {
                    TempData["ErrorMessage"] = "Master Key đã chọn không hợp lệ, không hoạt động, hoặc đã bị xóa.";
                    viewModel.SelectedMasterDataKeyId = null;
                }
            }
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateOrEditMasterValue(MasterValuesViewModel model)
        {
            ModelState.Remove("SelectedMasterDataKeyName");
            ModelState.Remove("MasterValues");

            if (model.MasterValueInContext == null)
            {
                TempData["ErrorMessage"] = "Dữ liệu gửi lên không hợp lệ (MasterValueInContext is null).";
                return RedirectToAction(nameof(MasterValues), new { SelectedMasterDataKeyId = model.SelectedMasterDataKeyId });
            }

            if (string.IsNullOrWhiteSpace(model.MasterValueInContext.Name))
            {
                ModelState.AddModelError("MasterValueInContext.Name", "Tên Giá trị không được để trống.");
            }

            Guid parentKeyIdForLogic;

            if (!model.IsEdit) // Thêm mới
            {
                if (!model.SelectedMasterDataKeyId.HasValue || model.SelectedMasterDataKeyId.Value == Guid.Empty)
                {
                    ModelState.AddModelError("SelectedMasterDataKeyId", "Vui lòng chọn một Master Key cha.");
                }
                parentKeyIdForLogic = model.SelectedMasterDataKeyId ?? Guid.Empty; // Gán Guid.Empty nếu null
                model.MasterValueInContext.MasterDataKeyId = parentKeyIdForLogic;
            }
            else // Chỉnh sửa
            {
                // Khi chỉnh sửa, MasterDataKeyId của value đã có trong model.MasterValueInContext.MasterDataKeyId (từ hidden field)
                parentKeyIdForLogic = model.MasterValueInContext.MasterDataKeyId; // Đây là Guid non-nullable
                if (parentKeyIdForLogic == Guid.Empty)
                {
                    ModelState.AddModelError("MasterValueInContext.MasterDataKeyId", "Master Key cha của giá trị này không hợp lệ khi chỉnh sửa.");
                }
            }

            if (!ModelState.IsValid)
            {
                var allMasterKeys = await _masterData.GetMasterDataKeysAsync(false, false);
                ViewBag.MasterKeysSelectList = new SelectList(allMasterKeys, "Id", "Name", model.SelectedMasterDataKeyId);

                if (model.SelectedMasterDataKeyId.HasValue && model.SelectedMasterDataKeyId.Value != Guid.Empty)
                {
                    var selectedKeyEntity = allMasterKeys.FirstOrDefault(k => k.Id == model.SelectedMasterDataKeyId.Value);
                    if (selectedKeyEntity != null)
                    {
                        model.SelectedMasterDataKeyName = selectedKeyEntity.Name;
                        var valuesEntities = await _masterData.GetMasterDataValuesAsync(model.SelectedMasterDataKeyId.Value, false, false);
                        model.MasterValues = _mapper.Map<List<MasterDataValueViewModel>>(valuesEntities);
                    }
                }
                if (model.MasterValueInContext == null) model.MasterValueInContext = new MasterDataValueViewModel { IsActive = true };
                if (!model.IsEdit) model.MasterValueInContext.MasterDataKeyId = model.SelectedMasterDataKeyId ?? Guid.Empty;

                return View("MasterValues", model);
            }

            var masterValueEntity = _mapper.Map<MasterDataValue>(model.MasterValueInContext);
            masterValueEntity.MasterDataKeyId = parentKeyIdForLogic;

            try
            {
                if (model.IsEdit)
                {
                    if (masterValueEntity.Id == Guid.Empty || masterValueEntity.MasterDataKeyId == Guid.Empty)
                    {
                        TempData["ErrorMessage"] = "Dữ liệu không hợp lệ để cập nhật Master Value.";
                        return RedirectToAction(nameof(MasterValues), new { SelectedMasterDataKeyId = model.SelectedMasterDataKeyId });
                    }
                    masterValueEntity.UpdatedBy = HttpContext.User.GetCurrentUserDetails()?.Name;
                    await _masterData.UpdateMasterDataValueAsync(masterValueEntity);
                    TempData["SuccessMessage"] = "Cập nhật Master Value thành công!";
                }
                else
                {
                    if (masterValueEntity.MasterDataKeyId == Guid.Empty)
                    {
                        TempData["ErrorMessage"] = "Không thể thêm Master Value mà không có Master Key cha hợp lệ.";
                        return RedirectToAction(nameof(MasterValues), new { SelectedMasterDataKeyId = model.SelectedMasterDataKeyId });
                    }
                    masterValueEntity.CreatedBy = HttpContext.User.GetCurrentUserDetails()?.Name;
                    await _masterData.AddMasterDataValueAsync(masterValueEntity);
                    TempData["SuccessMessage"] = "Thêm mới Master Value thành công!";
                }
                return RedirectToAction(nameof(MasterValues), new { SelectedMasterDataKeyId = masterValueEntity.MasterDataKeyId });
            }
            catch (InvalidOperationException ex)
            {
                TempData["ErrorMessage"] = ex.Message;
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Đã xảy ra lỗi khi lưu Master Value: {ex.Message}";
            }

            var repop_allMasterKeysOnError = await _masterData.GetMasterDataKeysAsync(false, false);
            ViewBag.MasterKeysSelectList = new SelectList(repop_allMasterKeysOnError, "Id", "Name", parentKeyIdForLogic);
            if (parentKeyIdForLogic != Guid.Empty)
            {
                var repop_selectedKeyEntityOnError = repop_allMasterKeysOnError.FirstOrDefault(k => k.Id == parentKeyIdForLogic);
                if (repop_selectedKeyEntityOnError != null)
                {
                    model.SelectedMasterDataKeyName = repop_selectedKeyEntityOnError.Name;
                    var repop_valuesEntitiesOnError = await _masterData.GetMasterDataValuesAsync(parentKeyIdForLogic, false, false);
                    model.MasterValues = _mapper.Map<List<MasterDataValueViewModel>>(repop_valuesEntitiesOnError);
                }
            }
            return View("MasterValues", model);
        }

        [HttpGet]
        public async Task<IActionResult> EditMasterValue(Guid id, Guid masterKeyId)
        {
            if (id == Guid.Empty || masterKeyId == Guid.Empty) return NotFound();

            var valueEntity = await _masterData.GetMasterDataValueByIdAsync(id, includeSoftDeleted: true);
            if (valueEntity == null || valueEntity.MasterDataKeyId != masterKeyId)
            {
                TempData["ErrorMessage"] = "Không tìm thấy Master Value hoặc không thuộc Master Key đã chọn.";
                return RedirectToAction(nameof(MasterValues), new { SelectedMasterDataKeyId = masterKeyId });
            }
            if (valueEntity.IsDeleted)
            {
                TempData["InfoMessage"] = "Master Value này đã bị xóa mềm. Cân nhắc khôi phục nếu muốn chỉnh sửa.";
            }

            var viewModel = await PrepareMasterValuesViewModelForEdit(masterKeyId, valueEntity);
            return View("MasterValues", viewModel);
        }

        private async Task<MasterValuesViewModel> PrepareMasterValuesViewModelForEdit(Guid masterKeyId, MasterDataValue valueToEditEntity)
        {
            var viewModel = new MasterValuesViewModel();
            var allMasterKeys = await _masterData.GetMasterDataKeysAsync(false, false);
            ViewBag.MasterKeysSelectList = new SelectList(allMasterKeys, "Id", "Name", masterKeyId);

            var selectedKey = allMasterKeys.FirstOrDefault(k => k.Id == masterKeyId);
            if (selectedKey != null)
            {
                viewModel.SelectedMasterDataKeyId = selectedKey.Id; // Gán Guid?
                viewModel.SelectedMasterDataKeyName = selectedKey.Name;
                var valuesEntities = await _masterData.GetMasterDataValuesAsync(selectedKey.Id, false, false);
                viewModel.MasterValues = _mapper.Map<List<MasterDataValueViewModel>>(valuesEntities);
            }
            viewModel.MasterValueInContext = _mapper.Map<MasterDataValueViewModel>(valueToEditEntity);
            viewModel.IsEdit = true;
            return viewModel;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteMasterValue(Guid id, Guid masterKeyIdRedirect)
        {
            if (id == Guid.Empty) return NotFound();
            try
            {
                await _masterData.DeleteMasterDataValueAsync(id);
                TempData["SuccessMessage"] = "Xóa Master Value thành công!";
            }
            catch (Exception ex)
            {
                TempData["ErrorMessage"] = $"Lỗi khi xóa Master Value: {ex.Message}";
            }
            return RedirectToAction(nameof(MasterValues), new { SelectedMasterDataKeyId = masterKeyIdRedirect });
        }

        // Sửa tham số thành Guid? cho nhất quán nếu cần, hoặc giữ Guid nếu endpoint này luôn yêu cầu ID
        [HttpGet]
        public async Task<IActionResult> GetValuesForMasterKeyId(Guid? SelectedMasterDataKeyId)
        {
            if (!SelectedMasterDataKeyId.HasValue || SelectedMasterDataKeyId.Value == Guid.Empty)
            {
                return Json(new { data = new List<MasterDataValueViewModel>() });
            }
            var values = await _masterData.GetMasterDataValuesAsync(SelectedMasterDataKeyId.Value, includeInactive: false, includeSoftDeleted: false);
            return Json(new { data = _mapper.Map<IEnumerable<MasterDataValueViewModel>>(values) });
        }

        public IActionResult Index()
        {
            return RedirectToAction(nameof(MasterKeys));
        }
    }
}
