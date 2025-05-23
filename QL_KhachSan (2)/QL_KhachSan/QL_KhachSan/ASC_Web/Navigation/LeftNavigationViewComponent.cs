using System.Text.Json;
using ASC_Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace ASC_Web.Navigation
{
    [ViewComponent(Name = "ASC_Web.Navigation.LeftNavigation")]
    public class LeftNavigationViewComponent : ViewComponent
    {
        public LeftNavigationViewComponent() { }

        public async Task<IViewComponentResult> InvokeAsync(NavigationMenu menuData)
        {
            await Task.CompletedTask;

            var processedMenu = menuData ?? new NavigationMenu();

            if (processedMenu.MenuItems != null)
            {
                processedMenu.MenuItems = processedMenu.MenuItems.OrderBy(p => p.Sequence).ToList();
                foreach (var menuItem in processedMenu.MenuItems)
                {
                    if (menuItem.NestedItems != null && menuItem.NestedItems.Any())
                    {
                        menuItem.NestedItems = menuItem.NestedItems.OrderBy(p => p.Sequence).ToList();
                    }
                }
            }

            return View("Default", processedMenu); // Chỉ định rõ tên View là "Default"
        }
    }
}