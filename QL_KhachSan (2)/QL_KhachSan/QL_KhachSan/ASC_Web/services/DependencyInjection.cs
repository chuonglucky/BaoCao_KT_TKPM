using ASC.DataAccess.Interfaces;
using ASC.DataAccess;
using ASC_Web.Configuration;
using ASC_Web.Data;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using OfficeOpenXml;
using ASC.business.interfaces;
using ASC.business;
namespace ASC_Web.services
{
    public static class DependencyInjection
    {
        //Config services
        public static IServiceCollection AddConfig(this IServiceCollection services, IConfiguration config)
        {
          
            // Add AddDbContext with connectionString to mirage database
            var connectionString = config.GetConnectionString("DefaultConnection") ??
                throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(connectionString));
            //Add Options and get data from appsettings.json with "AppSettings"
            services.AddOptions(); // IOption
            services.Configure<ApplicationSettings>(config.GetSection("AppSettings"));

            services.AddAuthentication().AddGoogle(options =>
            {
                IConfigurationSection googleAuthNSection = config.GetSection("Authentication:Google");
                options.ClientId = config["Google:Identity:ClientId"];
                options.ClientSecret = config["Google:Identity:ClientSecret"];
            });

            return services;
        }

        //Add service
        public static IServiceCollection AddMyDependencyGroup(this IServiceCollection services)
        {
           
            //Add ApplicationDbContext
            services.AddScoped<DbContext, ApplicationDbContext>();

            //Add IdentityUser IdentityUser
            services.AddIdentity<IdentityUser, IdentityRole>((options) =>
            {
                options.User.RequireUniqueEmail = true;
            }).AddEntityFrameworkStores<ApplicationDbContext>().AddDefaultTokenProviders();

            // Add Cache, Session
            services.AddSession();
            services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
            services.AddDistributedMemoryCache();
            services.AddSingleton<INavigationCacheOperations, NavigationCacheOperations>();
            // ĐĂNG KÝ DỊCH VỤ CHO MASTER DATA OPERATIONS
            services.AddScoped<IMasterDataOperations, MasterDataOperations>();
            // Đảm bảo MappingProfile nằm trong namespace ASC_Web.Areas.Configuration.Models
            services.AddAutoMapper(typeof(ASC_Web.Areas.Configuration.Models.MappingProfile).Assembly);


            //Add services
            services.AddTransient<IEmailSender, AuthMessageSender>(); // Dòng này bạn đã có

            // THÊM DÒNG NÀY: Đăng ký AuthMessageSender cho interface IEmailSender của Microsoft Identity UI
            services.AddTransient<Microsoft.AspNetCore.Identity.UI.Services.IEmailSender, AuthMessageSender>();
            services.AddSingleton<IIdentitySeed, IdentitySeed>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            services.AddScoped<IServiceRequestOperations, ServiceRequestOperations>();
            services.AddScoped<IBookingOperations, BookingOperations>();
            services.AddScoped<IRoomOperations, RoomOperations>();
            //...

            //Add RazorPages, MVC
            services.AddRazorPages();
            services.AddDatabaseDeveloperPageExceptionFilter();
            services.AddControllersWithViews();
            return services;
        }
    }
}
