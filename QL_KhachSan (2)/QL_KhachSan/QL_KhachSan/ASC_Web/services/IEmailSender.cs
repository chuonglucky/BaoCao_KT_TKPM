
namespace ASC_Web.services
{public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
