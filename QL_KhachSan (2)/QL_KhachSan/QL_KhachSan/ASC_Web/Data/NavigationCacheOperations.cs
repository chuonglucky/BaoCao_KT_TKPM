using ASC_Web.Models;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging; // Đảm bảo bạn có using này nếu dùng ILogger
using Microsoft.AspNetCore.Hosting; // Đảm bảo bạn có using này nếu dùng IWebHostEnvironment
using System;
using System.IO;
using System.Linq;
using System.Text.Json; // Chỉ giữ lại System.Text.Json
using System.Threading.Tasks;
namespace ASC_Web.Data
{
    public class NavigationCacheOperations : INavigationCacheOperations
    {
        private readonly IDistributedCache _cache;
        private readonly ILogger<NavigationCacheOperations> _logger;
        private readonly string _navigationCacheName = "NavigationCache";
        private readonly string _navigationJsonFilePath;
        private static readonly JsonSerializerOptions _jsonSerializerOptions = new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true // Cho phép không phân biệt hoa thường với tên thuộc tính JSON
        };

        public NavigationCacheOperations(IDistributedCache cache, ILogger<NavigationCacheOperations> logger, Microsoft.AspNetCore.Hosting.IWebHostEnvironment hostingEnvironment)
        {
            _cache = cache;
            _logger = logger;
            _navigationJsonFilePath = Path.Combine(hostingEnvironment.ContentRootPath, "Navigation", "Navigation.json");
        }

        public async Task CreateNavigationCacheAsync()
        {
            NavigationMenu menu = new NavigationMenu();
            try
            {
                if (!File.Exists(_navigationJsonFilePath))
                {
                    _logger?.LogError($"File Navigation.json không tìm thấy tại: {_navigationJsonFilePath}");
                    await _cache.SetStringAsync(_navigationCacheName, JsonSerializer.Serialize(menu, _jsonSerializerOptions), new DistributedCacheEntryOptions
                    {
                        AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                    });
                    return;
                }

                var menuJsonString = await File.ReadAllTextAsync(_navigationJsonFilePath);

                if (!string.IsNullOrEmpty(menuJsonString))
                {
                    menu = JsonSerializer.Deserialize<NavigationMenu>(menuJsonString, _jsonSerializerOptions);

                    if (menu?.MenuItems != null)
                    {
                        menu.MenuItems = menu.MenuItems.OrderBy(p => p.Sequence).ToList();
                        foreach (var menuItem in menu.MenuItems)
                        {
                            if (menuItem.NestedItems != null && menuItem.NestedItems.Any())
                            {
                                menuItem.NestedItems = menuItem.NestedItems.OrderBy(p => p.Sequence).ToList();
                            }
                        }
                    }
                }
            }
            catch (FileNotFoundException)
            {
                _logger?.LogError($"File Navigation.json không tìm thấy tại: {_navigationJsonFilePath}");
                menu = new NavigationMenu();
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "Lỗi khi deserialize Navigation.json");
                menu = new NavigationMenu();
            }
            catch (Exception ex)
            {
                _logger?.LogError(ex, "Lỗi không mong muốn xảy ra trong CreateNavigationCacheAsync");
                menu = new NavigationMenu();
            }

            var options = new DistributedCacheEntryOptions
            {
                AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1),
                SlidingExpiration = TimeSpan.FromMinutes(30)
            };
            await _cache.SetStringAsync(_navigationCacheName, JsonSerializer.Serialize(menu ?? new NavigationMenu(), _jsonSerializerOptions), options);
        }

        public async Task<NavigationMenu> GetNavigationCacheAsync()
        {
            var cachedString = await _cache.GetStringAsync(_navigationCacheName);
            if (string.IsNullOrEmpty(cachedString))
            {
                _logger?.LogInformation("Cache menu điều hướng trống hoặc không tìm thấy. Đang tạo mới cache.");
                await CreateNavigationCacheAsync();
                cachedString = await _cache.GetStringAsync(_navigationCacheName);

                if (string.IsNullOrEmpty(cachedString))
                {
                    _logger?.LogWarning("Cache menu điều hướng vẫn trống sau khi thử tạo mới.");
                    return new NavigationMenu();
                }
            }

            try
            {
                return JsonSerializer.Deserialize<NavigationMenu>(cachedString, _jsonSerializerOptions) ?? new NavigationMenu();
            }
            catch (JsonException ex)
            {
                _logger?.LogError(ex, "Lỗi khi deserialize dữ liệu menu từ cache. Đang thử tạo mới.");
                await CreateNavigationCacheAsync();
                cachedString = await _cache.GetStringAsync(_navigationCacheName);
                if (!string.IsNullOrEmpty(cachedString))
                {
                    try
                    {
                        return JsonSerializer.Deserialize<NavigationMenu>(cachedString, _jsonSerializerOptions) ?? new NavigationMenu();
                    }
                    catch (JsonException innerEx)
                    {
                        _logger?.LogError(innerEx, "Vẫn lỗi khi deserialize dữ liệu menu sau khi tạo mới cache.");
                    }
                }
                return new NavigationMenu();
            }
        }
    }
}
