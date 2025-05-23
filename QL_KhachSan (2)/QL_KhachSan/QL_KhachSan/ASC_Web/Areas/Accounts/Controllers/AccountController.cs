using ASC.Model.BaseTypes;
using ASC.Utilities;
using ASC_Web.Areas.Accounts.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using ASC_Web.services;
using Microsoft.AspNetCore.Mvc.Rendering;
using OfficeOpenXml;

namespace ASC_Web.Areas.Accounts.Controllers
{

    [Authorize]
    [Area("Accounts")]
    public class AccountController : Controller
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IEmailSender _emailSender;
        private readonly SignInManager<IdentityUser> _signInManager;

        public AccountController(UserManager<IdentityUser> userManager,
                                 IEmailSender emailSender,
                                 SignInManager<IdentityUser> signInManager)
        {
            _userManager = userManager;
            _emailSender = emailSender;
            _signInManager = signInManager;
        }

        public IActionResult Index()
        {
            return RedirectToAction("Customers");
        }

        [Authorize(Roles = "Admin,Manager,Receptionist")]
        [HttpGet]
        public async Task<IActionResult> Customers()
        {
            var customersInRole = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
            var customerViewModels = new List<CustomerDisplayViewModel>();
            foreach (var user in customersInRole)
            {
                var isActiveClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "IsActive");
                customerViewModels.Add(new CustomerDisplayViewModel
                {
                    UserId = user.Id,
                    UserName = user.UserName, // UserName here is from existing user, will be email
                    Email = user.Email,
                    IsActive = isActiveClaim != null && bool.TryParse(isActiveClaim.Value, out var isActive) && isActive
                });
            }

            var viewModel = new CustomerViewModel
            {
                DisplayCustomers = customerViewModels.OrderBy(c => c.UserName).ToList(),
                Registration = new CustomerRegistrationViewModel { IsEdit = false, IsActive = true }
            };
            return View(viewModel);
        }

        [Authorize(Roles = "Admin,Manager,Receptionist")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Customers(CustomerViewModel customerViewModel)
        {
            // Check if IsEdit is true, then UserName validation might not be necessary from model state
            // if it's not supposed to be editable.
            // However, we removed UserName from CustomerRegistrationViewModel, so it won't be in ModelState directly.

            if (!ModelState.IsValid)
            {
                var customersInRoleOnError = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
                var customerDisplayModelsOnError = new List<CustomerDisplayViewModel>();
                foreach (var u in customersInRoleOnError)
                {
                    var isActiveClaimError = (await _userManager.GetClaimsAsync(u)).FirstOrDefault(c => c.Type == "IsActive");
                    customerDisplayModelsOnError.Add(new CustomerDisplayViewModel
                    {
                        UserId = u.Id,
                        UserName = u.UserName,
                        Email = u.Email,
                        IsActive = isActiveClaimError != null && bool.TryParse(isActiveClaimError.Value, out var isActive) && isActive
                    });
                }
                customerViewModel.DisplayCustomers = customerDisplayModelsOnError.OrderBy(c => c.UserName).ToList();
                return View(customerViewModel);
            }

            if (customerViewModel.Registration.IsEdit)
            {
                // UserName is not submitted for edit directly from a field. We use Email (which is readonly) to find the user.
                // The CurrentEditUserId can be used if preferred, but Email is also a key.
                var userToUpdate = await _userManager.FindByEmailAsync(customerViewModel.Registration.Email);
                // Or using CurrentEditUserId:
                // var userToUpdate = await _userManager.FindByIdAsync(customerViewModel.Registration.CurrentEditUserId);


                if (userToUpdate == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy khách hàng.");
                    var customersInRoleNotFound = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
                    var customerDisplayModelsNotFound = new List<CustomerDisplayViewModel>();
                    foreach (var u in customersInRoleNotFound)
                    {
                        var loopUserIsActiveClaim = (await _userManager.GetClaimsAsync(u)).FirstOrDefault(c => c.Type == "IsActive");
                        customerDisplayModelsNotFound.Add(new CustomerDisplayViewModel
                        {
                            UserId = u.Id,
                            UserName = u.UserName,
                            Email = u.Email,
                            IsActive = loopUserIsActiveClaim != null && bool.TryParse(loopUserIsActiveClaim.Value, out var ia) && ia
                        });
                    }
                    customerViewModel.DisplayCustomers = customerDisplayModelsNotFound.OrderBy(c => c.UserName).ToList();
                    return View(customerViewModel);
                }

                // Username will be the email, and email is readonly in edit mode for customers, so no change to username.
                // If email were changeable, we would do: userToUpdate.UserName = userToUpdate.Email;

                var claims = await _userManager.GetClaimsAsync(userToUpdate);
                var isActiveClaim = claims.SingleOrDefault(p => p.Type == "IsActive");
                string newIsActiveValue = customerViewModel.Registration.IsActive.ToString();
                bool claimValueChanged = isActiveClaim == null || !string.Equals(isActiveClaim.Value, newIsActiveValue, StringComparison.OrdinalIgnoreCase);

                if (claimValueChanged)
                {
                    if (isActiveClaim != null)
                    {
                        var removeClaimResult = await _userManager.RemoveClaimAsync(userToUpdate, isActiveClaim);
                        if (!removeClaimResult.Succeeded)
                        {
                            ModelState.AddModelError("", "Lỗi khi xóa trạng thái cũ của khách hàng.");
                            // Repopulate list and return
                            var cErr = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
                            var cdmErr = new List<CustomerDisplayViewModel>();
                            foreach (var u in cErr) { var iac = (await _userManager.GetClaimsAsync(u)).FirstOrDefault(c_ => c_.Type == "IsActive"); cdmErr.Add(new CustomerDisplayViewModel { UserId = u.Id, UserName = u.UserName, Email = u.Email, IsActive = iac != null && bool.TryParse(iac.Value, out var ia) && ia }); }
                            customerViewModel.DisplayCustomers = cdmErr.OrderBy(c => c.UserName).ToList();
                            return View(customerViewModel);
                        }
                    }
                    var addClaimResult = await _userManager.AddClaimAsync(userToUpdate, new System.Security.Claims.Claim("IsActive", newIsActiveValue));
                    if (!addClaimResult.Succeeded)
                    {
                        ModelState.AddModelError("", "Lỗi khi cập nhật trạng thái khách hàng.");
                        // Repopulate list and return
                        var cErr = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
                        var cdmErr = new List<CustomerDisplayViewModel>();
                        foreach (var u in cErr) { var iac = (await _userManager.GetClaimsAsync(u)).FirstOrDefault(c_ => c_.Type == "IsActive"); cdmErr.Add(new CustomerDisplayViewModel { UserId = u.Id, UserName = u.UserName, Email = u.Email, IsActive = iac != null && bool.TryParse(iac.Value, out var ia) && ia }); }
                        customerViewModel.DisplayCustomers = cdmErr.OrderBy(c => c.UserName).ToList();
                        return View(customerViewModel);
                    }
                }

                if (customerViewModel.Registration.IsActive)
                {
                    await _emailSender.SendEmailAsync(customerViewModel.Registration.Email, "Tài khoản được Kích hoạt", $"Tài khoản của bạn ({customerViewModel.Registration.Email}) đã được kích hoạt.");
                }
                else
                {
                    await _emailSender.SendEmailAsync(customerViewModel.Registration.Email, "Tài khoản bị Vô hiệu hóa", "Tài khoản của bạn đã bị vô hiệu hóa.");
                }
                TempData["Message"] = "Cập nhật trạng thái khách hàng thành công!";
            }
            else // Add new Customer
            {
                var existingUserByEmail = await _userManager.FindByEmailAsync(customerViewModel.Registration.Email);
                if (existingUserByEmail != null)
                {
                    // This also implies username is taken if username is email
                    ModelState.AddModelError("Registration.Email", "Email đã được sử dụng.");
                }
                // No need to check UserName separately if UserName is derived from Email.
                // CreateAsync will fail if the UserName (which will be the email) already exists in the UserName column.

                if (ModelState.ErrorCount > 0)
                {
                    var cErr = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
                    var cdmErr = new List<CustomerDisplayViewModel>();
                    foreach (var u in cErr) { var iac = (await _userManager.GetClaimsAsync(u)).FirstOrDefault(c_ => c_.Type == "IsActive"); cdmErr.Add(new CustomerDisplayViewModel { UserId = u.Id, UserName = u.UserName, Email = u.Email, IsActive = iac != null && bool.TryParse(iac.Value, out var ia) && ia }); }
                    customerViewModel.DisplayCustomers = cdmErr.OrderBy(c => c.UserName).ToList();
                    return View(customerViewModel);
                }

                IdentityUser newUser = new IdentityUser
                {
                    UserName = customerViewModel.Registration.Email, // Set UserName as Email
                    Email = customerViewModel.Registration.Email,
                    EmailConfirmed = true // Or false and implement email confirmation flow
                };

                IdentityResult result = await _userManager.CreateAsync(newUser, customerViewModel.Registration.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newUser, Roles.Customer.ToString());
                    if (!roleResult.Succeeded)
                    {
                        roleResult.Errors.ToList().ForEach(p => ModelState.AddModelError("", p.Description));
                        await _userManager.DeleteAsync(newUser); // Rollback
                        var cErr = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
                        var cdmErr = new List<CustomerDisplayViewModel>();
                        foreach (var u in cErr) { var iac = (await _userManager.GetClaimsAsync(u)).FirstOrDefault(c_ => c_.Type == "IsActive"); cdmErr.Add(new CustomerDisplayViewModel { UserId = u.Id, UserName = u.UserName, Email = u.Email, IsActive = iac != null && bool.TryParse(iac.Value, out var ia) && ia }); }
                        customerViewModel.DisplayCustomers = cdmErr.OrderBy(c => c.UserName).ToList();
                        return View(customerViewModel);
                    }

                    await _userManager.AddClaimsAsync(newUser, new System.Security.Claims.Claim[]
                    {
                        new System.Security.Claims.Claim(ClaimTypes.Email, customerViewModel.Registration.Email),
                        new System.Security.Claims.Claim("IsActive", customerViewModel.Registration.IsActive.ToString())
                    });

                    if (customerViewModel.Registration.IsActive)
                    {
                        await _emailSender.SendEmailAsync(
                            customerViewModel.Registration.Email,
                            "Chào mừng đến với Dịch vụ của chúng tôi!",
                            $"Tài khoản của bạn đã được tạo thành công.<br/>Tên đăng nhập/Email: {customerViewModel.Registration.Email}<br/>(Vui lòng sử dụng mật khẩu bạn đã đăng ký để đăng nhập)."
                        );
                    }
                    TempData["Message"] = "Thêm mới khách hàng thành công!";
                }
                else
                {
                    result.Errors.ToList().ForEach(p => ModelState.AddModelError("", p.Description));
                    var cErr = await _userManager.GetUsersInRoleAsync(Roles.Customer.ToString());
                    var cdmErr = new List<CustomerDisplayViewModel>();
                    foreach (var u in cErr) { var iac = (await _userManager.GetClaimsAsync(u)).FirstOrDefault(c_ => c_.Type == "IsActive"); cdmErr.Add(new CustomerDisplayViewModel { UserId = u.Id, UserName = u.UserName, Email = u.Email, IsActive = iac != null && bool.TryParse(iac.Value, out var ia) && ia }); }
                    customerViewModel.DisplayCustomers = cdmErr.OrderBy(c => c.UserName).ToList();
                    return View(customerViewModel);
                }
            }
            return RedirectToAction("Customers");
        }

        // POST: /Accounts/Account/DeleteCustomer
        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteCustomer(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Không có thông tin khách hàng để xóa.";
                return RedirectToAction("Customers");
            }

            var userToDelete = await _userManager.FindByIdAsync(userId);
            if (userToDelete == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy khách hàng để xóa.";
                return RedirectToAction("Customers");
            }

            var result = await _userManager.DeleteAsync(userToDelete);
            if (result.Succeeded)
            {
                TempData["Message"] = $"Khách hàng {userToDelete.UserName} đã được xóa thành công.";
            }
            else
            {
                TempData["ErrorMessage"] = "Lỗi khi xóa khách hàng: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction("Customers");
        }

        // GET: /Accounts/Account/Profile
        [HttpGet]
        public async Task<IActionResult> Profile()
        {
            var userEmail = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmail))
            {
                return Challenge();
            }

            var user = await _userManager.FindByEmailAsync(userEmail);
            if (user == null)
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Login", "Account", new { area = "Identity" }); // Make sure this route is correct
            }
            // For customers, UserName is Email. ProfileModel.UserName should reflect this.
            // If user.UserName could differ from user.Email (e.g. for staff), this is fine.
            // For customers managed by this controller, it should be the same.
            return View(new ProfileModel() { UserName = user.UserName });
        }

        // POST: /Accounts/Account/Profile
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Profile(ProfileModel profile)
        {
            if (!ModelState.IsValid)
            {
                return View(profile);
            }

            var userEmailClaim = User.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(userEmailClaim))
            {
                return Challenge();
            }

            var user = await _userManager.FindByEmailAsync(userEmailClaim);
            if (user == null)
            {
                ModelState.AddModelError("", "Không tìm thấy người dùng.");
                return View(profile);
            }

            // If the user is a "Customer" and UserName must be Email:
            bool isCustomer = await _userManager.IsInRoleAsync(user, Roles.Customer.ToString());

            if (user.UserName != profile.UserName)
            {
                if (isCustomer)
                {
                    // For customers, UserName should track Email.
                    // If profile.UserName is intended to change the Email:
                    // 1. Validate new Email format.
                    // 2. Check if new Email is already taken by another user.
                    // 3. Update user.Email and user.UserName.
                    // This part is complex and outside the direct scope of "remove UserName input from Customer Management".
                    // For now, if a customer changes UserName via profile, it might become different from their Email.
                    // To enforce UserName == Email for customers on profile edit:
                    // - Disallow UserName change on profile for customers, or
                    // - Make UserName field on profile readonly for customers, or
                    // - If UserName changes, enforce it's a valid email and also change user.Email.
                    // The current request is specific to the admin's customer management page.
                    // We'll leave this as is, but note the potential inconsistency.
                    // A simple fix here might be to ignore UserName changes for customers or ensure it's an email.
                }

                var existingUserWithNewName = await _userManager.FindByNameAsync(profile.UserName);
                if (existingUserWithNewName != null && existingUserWithNewName.Id != user.Id)
                {
                    ModelState.AddModelError("UserName", "Tên người dùng này đã được sử dụng.");
                    return View(profile);
                }
                user.UserName = profile.UserName;
                if (isCustomer)
                { // If customer, and username changes, ensure email also changes (if that's the policy)
                    // Potentially: user.Email = profile.UserName; (after validation that profile.UserName is a valid, unique email)
                }
            }


            IdentityResult result = await _userManager.UpdateAsync(user);

            if (!result.Succeeded)
            {
                result.Errors.ToList().ForEach(p => ModelState.AddModelError("", p.Description));
                return View(profile);
            }

            await _signInManager.RefreshSignInAsync(user);
            TempData["Message"] = "Cập nhật hồ sơ thành công!";
            return RedirectToAction("Profile");
        }


        // --- Staff Management Methods (largely unchanged as requested scope is customer username) ---
        private List<string> GetStaffRolesList()
        {
            return Enum.GetNames(typeof(Roles))
                        .Where(roleName => roleName != Roles.Customer.ToString())
                        .ToList();
        }

        private async Task<List<StaffDisplayViewModel>> GetStaffUsersWithRolesAsync()
        {
            var staffUsersList = new List<StaffDisplayViewModel>();
            var staffRoleNames = GetStaffRolesList();
            var allUsers = _userManager.Users.ToList(); // Cân nhắc nếu lượng user lớn

            foreach (var user in allUsers)
            {
                var userRoles = await _userManager.GetRolesAsync(user);
                var primaryStaffRole = userRoles.FirstOrDefault(r => staffRoleNames.Contains(r));

                if (!string.IsNullOrEmpty(primaryStaffRole))
                {
                    if (staffUsersList.Any(s => s.UserId == user.Id)) continue;

                    var isActiveClaim = (await _userManager.GetClaimsAsync(user)).FirstOrDefault(c => c.Type == "IsActive");
                    staffUsersList.Add(new StaffDisplayViewModel
                    {
                        UserId = user.Id,
                        UserName = user.UserName,
                        Email = user.Email,
                        IsActive = isActiveClaim != null && bool.TryParse(isActiveClaim.Value, out var isActive) && isActive,
                        Role = primaryStaffRole
                    });
                }
            }
            return staffUsersList.OrderBy(s => s.Role).ThenBy(s => s.UserName).ToList();
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> StaffAccounts()
        {
            var viewModel = new StaffManagementViewModel
            {
                DisplayStaff = await GetStaffUsersWithRolesAsync(),
                Registration = new StaffRegistrationViewModel { IsActive = true }
            };

            var assignableRoles = GetStaffRolesList();
            if (!User.IsInRole(Roles.Admin.ToString()))
            {
                assignableRoles.Remove(Roles.Admin.ToString());
            }

            viewModel.AvailableRoles = assignableRoles
                                        .Select(r => new SelectListItem { Text = r, Value = r })
                                        .OrderBy(r => r.Text)
                                        .ToList();
            return View("StaffAccounts", viewModel);
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> StaffAccounts(StaffManagementViewModel viewModel)
        {
            var registrationModel = viewModel.Registration;
            var assignableRoles = GetStaffRolesList();

            if (!User.IsInRole(Roles.Admin.ToString()))
            {
                assignableRoles.Remove(Roles.Admin.ToString());
            }
            viewModel.AvailableRoles = assignableRoles.Select(r => new SelectListItem { Text = r, Value = r }).OrderBy(r => r.Text).ToList();

            if (!GetStaffRolesList().Contains(registrationModel.RoleToAssign))
            {
                ModelState.AddModelError("Registration.RoleToAssign", "Vai trò được chọn không hợp lệ.");
            }
            if (registrationModel.RoleToAssign == Roles.Admin.ToString() && !User.IsInRole(Roles.Admin.ToString()))
            {
                ModelState.AddModelError("Registration.RoleToAssign", "Bạn không có quyền gán vai trò Quản trị viên.");
            }

            if (!ModelState.IsValid)
            {
                viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                return View("StaffAccounts", viewModel);
            }

            if (registrationModel.IsEdit)
            {
                var userToUpdate = await _userManager.FindByIdAsync(registrationModel.UserId);
                if (userToUpdate == null)
                {
                    ModelState.AddModelError("", "Không tìm thấy nhân viên.");
                    viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                    return View("StaffAccounts", viewModel);
                }

                if (!userToUpdate.Email.Equals(registrationModel.Email, StringComparison.OrdinalIgnoreCase))
                {
                    var existingEmailUser = await _userManager.FindByEmailAsync(registrationModel.Email);
                    if (existingEmailUser != null && existingEmailUser.Id != userToUpdate.Id)
                    {
                        ModelState.AddModelError("Registration.Email", "Email này đã được sử dụng bởi một tài khoản khác.");
                        viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                        return View("StaffAccounts", viewModel);
                    }
                }
                userToUpdate.Email = registrationModel.Email;


                if (!userToUpdate.UserName.Equals(registrationModel.UserName, StringComparison.OrdinalIgnoreCase))
                {
                    var existingUserNameUser = await _userManager.FindByNameAsync(registrationModel.UserName);
                    if (existingUserNameUser != null && existingUserNameUser.Id != userToUpdate.Id)
                    {
                        ModelState.AddModelError("Registration.UserName", "Tên đăng nhập này đã được sử dụng bởi một tài khoản khác.");
                        viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                        return View("StaffAccounts", viewModel);
                    }
                }
                userToUpdate.UserName = registrationModel.UserName;


                var updateResult = await _userManager.UpdateAsync(userToUpdate);
                if (!updateResult.Succeeded)
                {
                    updateResult.Errors.ToList().ForEach(e => ModelState.AddModelError("", e.Description));
                    viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                    return View("StaffAccounts", viewModel);
                }

                var currentRoles = await _userManager.GetRolesAsync(userToUpdate);
                var currentStaffRoles = currentRoles.Where(r => GetStaffRolesList().Contains(r)).ToList();
                var newRole = registrationModel.RoleToAssign;

                bool roleChanged = !currentStaffRoles.Contains(newRole) || currentStaffRoles.Count > 1 || (currentStaffRoles.Count == 1 && currentStaffRoles.First() != newRole);

                if (roleChanged)
                {
                    var removeRolesResult = await _userManager.RemoveFromRolesAsync(userToUpdate, currentStaffRoles);
                    if (!removeRolesResult.Succeeded)
                    {
                        removeRolesResult.Errors.ToList().ForEach(e => ModelState.AddModelError("", $"Lỗi xóa vai trò cũ: {e.Description}"));
                        viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                        return View("StaffAccounts", viewModel);
                    }
                    var addRoleResult = await _userManager.AddToRoleAsync(userToUpdate, newRole);
                    if (!addRoleResult.Succeeded)
                    {
                        addRoleResult.Errors.ToList().ForEach(e => ModelState.AddModelError("", $"Lỗi thêm vai trò mới: {e.Description}"));
                        foreach (var oldRole in currentStaffRoles) await _userManager.AddToRoleAsync(userToUpdate, oldRole); // Rollback roles
                        viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                        return View("StaffAccounts", viewModel);
                    }
                }

                var claims = await _userManager.GetClaimsAsync(userToUpdate);
                var isActiveClaim = claims.SingleOrDefault(p => p.Type == "IsActive");
                string newIsActiveValue = registrationModel.IsActive.ToString();
                bool claimValueChanged = isActiveClaim == null || !string.Equals(isActiveClaim.Value, newIsActiveValue, StringComparison.OrdinalIgnoreCase);

                if (claimValueChanged)
                {
                    if (isActiveClaim != null)
                    {
                        await _userManager.RemoveClaimAsync(userToUpdate, isActiveClaim);
                    }
                    await _userManager.AddClaimAsync(userToUpdate, new Claim("IsActive", newIsActiveValue));
                }

                TempData["Message"] = $"Cập nhật thông tin cho {userToUpdate.UserName} thành công.";
                if (claimValueChanged || roleChanged)
                {
                    string subject = registrationModel.IsActive ? "Tài khoản Nhân viên được Cập nhật và Kích hoạt" : "Tài khoản Nhân viên được Cập nhật và Vô hiệu hóa";
                    string body = $"Thông tin tài khoản nhân viên của bạn ({userToUpdate.Email}) đã được cập nhật.<br/>" +
                                  $"Vai trò hiện tại: {newRole}.<br/>" +
                                  $"Trạng thái: {(registrationModel.IsActive ? "Hoạt động" : "Vô hiệu hóa")}.";
                    await _emailSender.SendEmailAsync(userToUpdate.Email, subject, body);
                }
            }
            else // Add new staff
            {
                var existingUserByEmail = await _userManager.FindByEmailAsync(registrationModel.Email);
                if (existingUserByEmail != null) ModelState.AddModelError("Registration.Email", "Email đã được sử dụng.");

                var existingUserByUserName = await _userManager.FindByNameAsync(registrationModel.UserName);
                if (existingUserByUserName != null) ModelState.AddModelError("Registration.UserName", "Tên đăng nhập đã được sử dụng.");

                if (!ModelState.IsValid)
                {
                    viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                    return View("StaffAccounts", viewModel);
                }

                var newUser = new IdentityUser { UserName = registrationModel.UserName, Email = registrationModel.Email, EmailConfirmed = true };
                var result = await _userManager.CreateAsync(newUser, registrationModel.Password);

                if (result.Succeeded)
                {
                    var roleResult = await _userManager.AddToRoleAsync(newUser, registrationModel.RoleToAssign);
                    if (!roleResult.Succeeded)
                    {
                        await _userManager.DeleteAsync(newUser);
                        roleResult.Errors.ToList().ForEach(e => ModelState.AddModelError("", e.Description));
                        viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                        return View("StaffAccounts", viewModel);
                    }

                    await _userManager.AddClaimsAsync(newUser, new[] {
                        new Claim(ClaimTypes.Email, newUser.Email),
                        new Claim("IsActive", registrationModel.IsActive.ToString())
                    });

                    TempData["Message"] = $"Nhân viên {newUser.UserName} (vai trò: {registrationModel.RoleToAssign}) đã được thêm thành công.";
                    string subject = "Tài khoản Nhân viên của bạn đã được tạo";
                    string body = $"Chào {newUser.UserName},<br/>Tài khoản nhân viên của bạn với vai trò {registrationModel.RoleToAssign} đã được tạo thành công.<br/>" +
                                  $"Bạn có thể đăng nhập bằng email ({newUser.Email}) và mật khẩu đã được cung cấp/đăng ký." +
                                  (registrationModel.IsActive ? "" : "<br/>Lưu ý: Tài khoản của bạn hiện đang bị vô hiệu hóa bởi quản trị viên khi tạo.");
                    await _emailSender.SendEmailAsync(newUser.Email, subject, body);
                }
                else
                {
                    result.Errors.ToList().ForEach(e => ModelState.AddModelError("", e.Description));
                    viewModel.DisplayStaff = await GetStaffUsersWithRolesAsync();
                    return View("StaffAccounts", viewModel);
                }
            }
            return RedirectToAction("StaffAccounts");
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ImportStaffAccounts(IFormFile excelFile)
        {
            if (excelFile == null || excelFile.Length == 0)
            {
                TempData["ErrorMessage"] = "Vui lòng chọn một file Excel.";
                return RedirectToAction("StaffAccounts");
            }

            if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
            {
                TempData["ErrorMessage"] = "Chỉ chấp nhận file Excel định dạng .xlsx.";
                return RedirectToAction("StaffAccounts");
            }

            var successCount = 0;
            var errorMessages = new List<string>();
            var staffRolesList = GetStaffRolesList();
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial; // Or Commercial


            using (var memoryStream = new MemoryStream())
            {
                await excelFile.CopyToAsync(memoryStream);
                memoryStream.Position = 0;

                try
                {
                    using (var package = new ExcelPackage(memoryStream))
                    {
                        ExcelWorksheet worksheet = package.Workbook.Worksheets.FirstOrDefault();
                        if (worksheet == null)
                        {
                            TempData["ErrorMessage"] = "File Excel không hợp lệ hoặc không có sheet nào.";
                            return RedirectToAction("StaffAccounts");
                        }

                        int rowCount = worksheet.Dimension.Rows;
                        for (int row = 2; row <= rowCount; row++)
                        {
                            try
                            {
                                string userName = worksheet.Cells[row, 1].Value?.ToString()?.Trim();
                                string email = worksheet.Cells[row, 2].Value?.ToString()?.Trim();
                                string password = worksheet.Cells[row, 3].Value?.ToString();
                                string roleToAssign = worksheet.Cells[row, 4].Value?.ToString()?.Trim();
                                string isActiveStr = worksheet.Cells[row, 5].Value?.ToString()?.Trim().ToUpperInvariant();

                                if (string.IsNullOrWhiteSpace(userName)) { errorMessages.Add($"Dòng {row}: UserName không được để trống."); continue; }
                                if (string.IsNullOrWhiteSpace(email) || !IsValidEmail(email)) { errorMessages.Add($"Dòng {row}: Email '{worksheet.Cells[row, 2].Value?.ToString()}' không hợp lệ."); continue; }
                                if (string.IsNullOrWhiteSpace(password)) { errorMessages.Add($"Dòng {row}: Password cho '{userName}' không được để trống."); continue; }
                                if (password.Length < 6) { errorMessages.Add($"Dòng {row}: Password cho '{userName}' phải ít nhất 6 ký tự."); continue; }
                                if (string.IsNullOrWhiteSpace(roleToAssign)) { errorMessages.Add($"Dòng {row}: RoleToAssign cho '{userName}' không được để trống."); continue; }
                                if (!staffRolesList.Contains(roleToAssign, StringComparer.OrdinalIgnoreCase)) { errorMessages.Add($"Dòng {row}: Vai trò '{roleToAssign}' cho '{userName}' không hợp lệ. Các vai trò: {string.Join(", ", staffRolesList)}"); continue; }
                                if (roleToAssign.Equals(Roles.Admin.ToString(), StringComparison.OrdinalIgnoreCase) && !User.IsInRole(Roles.Admin.ToString())) { errorMessages.Add($"Dòng {row}: Bạn không có quyền gán vai trò Admin cho '{userName}'."); continue; }

                                bool isActive = true;
                                if (!string.IsNullOrWhiteSpace(isActiveStr))
                                {
                                    if (isActiveStr == "TRUE") isActive = true;
                                    else if (isActiveStr == "FALSE") isActive = false;
                                    else { errorMessages.Add($"Dòng {row}: IsActive '{worksheet.Cells[row, 5].Value?.ToString()}' cho '{userName}' không hợp lệ. Dùng TRUE/FALSE."); continue; }
                                }

                                if (await _userManager.FindByEmailAsync(email) != null) { errorMessages.Add($"Dòng {row}: Email '{email}' đã tồn tại."); continue; }
                                if (await _userManager.FindByNameAsync(userName) != null) { errorMessages.Add($"Dòng {row}: UserName '{userName}' đã tồn tại."); continue; }

                                var newUser = new IdentityUser { UserName = userName, Email = email, EmailConfirmed = true };
                                var createUserResult = await _userManager.CreateAsync(newUser, password);

                                if (createUserResult.Succeeded)
                                {
                                    var addToRoleResult = await _userManager.AddToRoleAsync(newUser, roleToAssign);
                                    if (!addToRoleResult.Succeeded)
                                    {
                                        await _userManager.DeleteAsync(newUser);
                                        errorMessages.Add($"Dòng {row}: Lỗi gán vai trò cho '{userName}': {string.Join(", ", addToRoleResult.Errors.Select(e => e.Description))}");
                                        continue;
                                    }
                                    await _userManager.AddClaimsAsync(newUser, new[] { new Claim("IsActive", isActive.ToString()) });
                                    successCount++;
                                }
                                else
                                {
                                    errorMessages.Add($"Dòng {row}: Lỗi tạo tài khoản cho '{userName}': {string.Join(", ", createUserResult.Errors.Select(e => e.Description))}");
                                }
                            }
                            catch (Exception exRow)
                            {
                                errorMessages.Add($"Dòng {row}: Lỗi xử lý: {exRow.Message}");
                            }
                        }
                    }
                }
                catch (Exception exFile)
                {
                    TempData["ErrorMessage"] = $"Có lỗi xảy ra khi đọc file Excel: {exFile.Message}";
                    return RedirectToAction("StaffAccounts");
                }
            }

            if (errorMessages.Any())
            {
                TempData["ErrorMessage"] = $"<b>Import hoàn tất với {successCount} thành công và {errorMessages.Count} lỗi:</b><br/>" + string.Join("<br/>", errorMessages.Select(e => $"- {e}"));
            }
            else if (successCount > 0)
            {
                TempData["Message"] = $"Import thành công {successCount} nhân viên.";
            }
            else
            {
                TempData["ErrorMessage"] = "Không có dữ liệu hợp lệ được tìm thấy trong file Excel để import.";
            }
            return RedirectToAction("StaffAccounts");
        }

        [Authorize(Roles = "Admin,Manager")]
        [HttpGet]
        public async Task<IActionResult> ExportStaffToExcel()
        {
            ExcelPackage.LicenseContext = LicenseContext.NonCommercial;

            var staffList = await GetStaffUsersWithRolesAsync();

            using (var package = new ExcelPackage())
            {
                var worksheet = package.Workbook.Worksheets.Add("Danh sách Nhân viên");

                worksheet.Cells[1, 1].Value = "Tên đăng nhập";
                worksheet.Cells[1, 2].Value = "Email";
                worksheet.Cells[1, 3].Value = "Vai trò";
                worksheet.Cells[1, 4].Value = "Trạng thái";

                using (var range = worksheet.Cells[1, 1, 1, 4])
                {
                    range.Style.Font.Bold = true;
                }

                if (staffList.Any())
                {
                    for (int i = 0; i < staffList.Count; i++)
                    {
                        var staff = staffList[i];
                        int row = i + 2;
                        worksheet.Cells[row, 1].Value = staff.UserName;
                        worksheet.Cells[row, 2].Value = staff.Email;
                        worksheet.Cells[row, 3].Value = staff.Role;
                        worksheet.Cells[row, 4].Value = staff.IsActive ? "Hoạt động" : "Vô hiệu hóa";
                    }
                }

                worksheet.Cells[worksheet.Dimension.Address].AutoFitColumns();

                var stream = new MemoryStream();
                await package.SaveAsAsync(stream);
                stream.Position = 0;

                string excelName = $"Staff_Accounts_{DateTime.Now:yyyyMMddHHmmss}.xlsx";
                return File(stream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", excelName);
            }
        }


        [HttpPost]
        [Authorize(Roles = "Admin")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteStaffAccount(string userId)
        {
            if (string.IsNullOrEmpty(userId))
            {
                TempData["ErrorMessage"] = "Không có thông tin nhân viên để xóa.";
                return RedirectToAction("StaffAccounts");
            }
            var userToDelete = await _userManager.FindByIdAsync(userId);
            if (userToDelete == null)
            {
                TempData["ErrorMessage"] = "Không tìm thấy nhân viên.";
                return RedirectToAction("StaffAccounts");
            }

            // Prevent deleting currently logged-in user or critical admin accounts if necessary
            var currentUser = await _userManager.GetUserAsync(User);
            if (currentUser.Id == userId)
            {
                TempData["ErrorMessage"] = "Bạn không thể xóa tài khoản của chính mình.";
                return RedirectToAction("StaffAccounts");
            }
            // Add more checks if there are specific usernames or roles that shouldn't be deleted.

            var result = await _userManager.DeleteAsync(userToDelete);
            if (result.Succeeded)
            {
                TempData["Message"] = $"Nhân viên {userToDelete.UserName} đã được xóa.";
            }
            else
            {
                TempData["ErrorMessage"] = "Lỗi xóa nhân viên: " + string.Join(", ", result.Errors.Select(e => e.Description));
            }
            return RedirectToAction("StaffAccounts");
        }

        private bool IsValidEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email)) return false;
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch { return false; }
        }
    }
}
