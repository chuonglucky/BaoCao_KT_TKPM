using ASC.Model.BaseTypes;
using ASC_Web.Configuration;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace ASC_Web.Data
{
    public class IdentitySeed : IIdentitySeed
    {
        private readonly ILogger<IdentitySeed> _logger;

        public IdentitySeed(ILogger<IdentitySeed> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        public async Task Seed(UserManager<IdentityUser> userManager,
                               RoleManager<IdentityRole> roleManager,
                               IOptions<ApplicationSettings> options)
        {
            _logger.LogInformation("Bắt đầu quá trình seed Roles và Users.");

            var appSettings = options.Value;
            if (appSettings == null)
            {
                _logger.LogCritical("ApplicationSettings không được cấu hình hoặc null. Không thể seed data.");
                throw new InvalidOperationException("ApplicationSettings is not configured.");
            }

            // 1. Seed Roles
            _logger.LogInformation("Đang seed Roles từ chuỗi: '{RolesString}'", appSettings.Roles);
            if (string.IsNullOrWhiteSpace(appSettings.Roles))
            {
                _logger.LogWarning("Chuỗi Roles trong ApplicationSettings bị trống. Sẽ không có role nào được tạo.");
            }
            else
            {
                var roleNames = appSettings.Roles.Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                foreach (var roleName in roleNames)
                {
                    if (string.IsNullOrWhiteSpace(roleName)) continue;

                    _logger.LogInformation("Kiểm tra sự tồn tại của role: {RoleName}", roleName);
                    if (!await roleManager.RoleExistsAsync(roleName))
                    {
                        var role = new IdentityRole { Name = roleName, NormalizedName = roleName.ToUpperInvariant() };
                        var result = await roleManager.CreateAsync(role);
                        if (result.Succeeded)
                        {
                            _logger.LogInformation("Role '{RoleName}' đã được tạo thành công.", roleName);
                        }
                        else
                        {
                            _logger.LogError("Tạo role '{RoleName}' thất bại. Lỗi: {Errors}", roleName, string.Join(", ", result.Errors.Select(e => e.Description)));
                        }
                    }
                    else
                    {
                        _logger.LogInformation("Role '{RoleName}' đã tồn tại.", roleName);
                    }
                }
            }
            _logger.LogInformation("Hoàn tất seed Roles.");

            // 2. Seed Admin User
            _logger.LogInformation("Đang seed Admin User...");
            if (string.IsNullOrWhiteSpace(appSettings.AdminEmail) || string.IsNullOrWhiteSpace(appSettings.AdminPassword) || string.IsNullOrWhiteSpace(appSettings.AdminName))
            {
                _logger.LogWarning("Thông tin Admin (Email, Password, Name) trong ApplicationSettings không đầy đủ. Admin user sẽ không được tạo.");
            }
            else
            {
                var adminUser = await userManager.FindByEmailAsync(appSettings.AdminEmail);
                if (adminUser == null)
                {
                    adminUser = new IdentityUser
                    {
                        UserName = appSettings.AdminName,
                        Email = appSettings.AdminEmail,
                        EmailConfirmed = true,
                    };

                    _logger.LogInformation("Đang tạo Admin User với Email: {AdminEmail} và UserName: {AdminUserName}", appSettings.AdminEmail, adminUser.UserName);
                    var result = await userManager.CreateAsync(adminUser, appSettings.AdminPassword);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Admin User '{AdminEmail}' đã được tạo thành công.", appSettings.AdminEmail);
                        await userManager.AddClaimAsync(adminUser, new Claim(ClaimTypes.Email, appSettings.AdminEmail));
                        await userManager.AddClaimAsync(adminUser, new Claim("IsActive", "True"));

                        var adminRoleName = Roles.Admin.ToString();
                        _logger.LogInformation("Kiểm tra role '{AdminRoleName}' để gán cho Admin User.", adminRoleName);
                        if (await roleManager.RoleExistsAsync(adminRoleName))
                        {
                            if (!(await userManager.IsInRoleAsync(adminUser, adminRoleName)))
                            {
                                _logger.LogInformation("Đang thêm Admin User '{AdminEmail}' vào role '{AdminRoleName}'.", appSettings.AdminEmail, adminRoleName);
                                var addToRoleResult = await userManager.AddToRoleAsync(adminUser, adminRoleName);
                                if (addToRoleResult.Succeeded)
                                {
                                    _logger.LogInformation("Đã thêm Admin User '{AdminEmail}' vào role '{AdminRoleName}' thành công.", appSettings.AdminEmail, adminRoleName);
                                }
                                else
                                {
                                    _logger.LogError("Thêm Admin User '{AdminEmail}' vào role '{AdminRoleName}' thất bại. Lỗi: {Errors}", appSettings.AdminEmail, adminRoleName, string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                                }
                            }
                            else
                            {
                                _logger.LogInformation("Admin User '{AdminEmail}' đã ở trong role '{AdminRoleName}'.", appSettings.AdminEmail, adminRoleName);
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Role '{AdminRoleName}' không tồn tại. Không thể thêm Admin User vào role này.", adminRoleName);
                        }
                    }
                    else
                    {
                        _logger.LogError("Tạo Admin User '{AdminEmail}' thất bại. Lỗi: {Errors}", appSettings.AdminEmail, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    _logger.LogInformation("Admin User với Email '{AdminEmail}' đã tồn tại.", appSettings.AdminEmail);
                }
            }
            _logger.LogInformation("Hoàn tất seed Admin User.");

            // 3. Seed Manager User
            _logger.LogInformation("Đang seed Manager User...");
            if (string.IsNullOrWhiteSpace(appSettings.ManagerEmail) || string.IsNullOrWhiteSpace(appSettings.ManagerPassword) || string.IsNullOrWhiteSpace(appSettings.ManagerName))
            {
                _logger.LogWarning("Thông tin Manager (Email, Password, Name) trong ApplicationSettings không đầy đủ. Manager user sẽ không được tạo.");
            }
            else
            {
                var managerUser = await userManager.FindByEmailAsync(appSettings.ManagerEmail);
                if (managerUser == null)
                {
                    managerUser = new IdentityUser
                    {
                        UserName = appSettings.ManagerName,
                        Email = appSettings.ManagerEmail,
                        EmailConfirmed = true,
                    };
                    _logger.LogInformation("Đang tạo Manager User với Email: {ManagerEmail} và UserName: {ManagerUserName}", appSettings.ManagerEmail, managerUser.UserName);
                    var result = await userManager.CreateAsync(managerUser, appSettings.ManagerPassword);
                    if (result.Succeeded)
                    {
                        _logger.LogInformation("Manager User '{ManagerEmail}' đã được tạo thành công.", appSettings.ManagerEmail);
                        await userManager.AddClaimAsync(managerUser, new Claim(ClaimTypes.Email, appSettings.ManagerEmail));
                        await userManager.AddClaimAsync(managerUser, new Claim("IsActive", "True"));

                        var managerRoleName = Roles.Manager.ToString();
                        _logger.LogInformation("Kiểm tra role '{ManagerRoleName}' để gán cho Manager User.", managerRoleName);
                        if (await roleManager.RoleExistsAsync(managerRoleName))
                        {
                            if (!(await userManager.IsInRoleAsync(managerUser, managerRoleName)))
                            {
                                _logger.LogInformation("Đang thêm Manager User '{ManagerEmail}' vào role '{ManagerRoleName}'.", appSettings.ManagerEmail, managerRoleName);
                                var addToRoleResult = await userManager.AddToRoleAsync(managerUser, managerRoleName);
                                if (addToRoleResult.Succeeded)
                                {
                                    _logger.LogInformation("Đã thêm Manager User '{ManagerEmail}' vào role '{ManagerRoleName}' thành công.", appSettings.ManagerEmail, managerRoleName);
                                }
                                else
                                {
                                    _logger.LogError("Thêm Manager User '{ManagerEmail}' vào role '{ManagerRoleName}' thất bại. Lỗi: {Errors}", appSettings.ManagerEmail, managerRoleName, string.Join(", ", addToRoleResult.Errors.Select(e => e.Description)));
                                }
                            }
                            else
                            {
                                _logger.LogInformation("Manager User '{ManagerEmail}' đã ở trong role '{ManagerRoleName}'.", appSettings.ManagerEmail, managerRoleName);
                            }
                        }
                        else
                        {
                            _logger.LogWarning("Role '{ManagerRoleName}' không tồn tại. Không thể thêm Manager User vào role này.", managerRoleName);
                        }
                    }
                    else
                    {
                        _logger.LogError("Tạo Manager User '{ManagerEmail}' thất bại. Lỗi: {Errors}", appSettings.ManagerEmail, string.Join(", ", result.Errors.Select(e => e.Description)));
                    }
                }
                else
                {
                    _logger.LogInformation("Manager User với Email '{ManagerEmail}' đã tồn tại.", appSettings.ManagerEmail);
                }
            }
            _logger.LogInformation("Hoàn tất seed Manager User.");
            _logger.LogInformation("Hoàn tất toàn bộ quá trình seed Roles và Users.");
        }
    }
}
