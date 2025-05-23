using Microsoft.Extensions.Options;
/*using System.Net.Mail;*/
using MimeKit;
using MailKit;
using MailKit.Net.Smtp;
using ASC_Web.Configuration;
using ASC_Web.services;
using System.Security.Authentication;
using MicrosoftIE = Microsoft.AspNetCore.Identity.UI.Services;

namespace ASC_Web.services
{
    public class AuthMessageSender : IEmailSender, MicrosoftIE.IEmailSender
    {
        private readonly IOptions<ApplicationSettings> _settings;

        public AuthMessageSender(IOptions<ApplicationSettings> settings)
        {
            _settings = settings ?? throw new ArgumentNullException(nameof(settings));

            // Kiểm tra sớm các cài đặt SMTP quan trọng khi khởi tạo
            if (_settings.Value == null)
            {
                throw new InvalidOperationException("ApplicationSettings is not configured.");
            }
            if (string.IsNullOrEmpty(_settings.Value.SMTPServer))
            {
                throw new InvalidOperationException("SMTPServer is not configured in ApplicationSettings.");
            }
            if (_settings.Value.SMTPPort <= 0)
            {
                throw new InvalidOperationException("SMTPPort is not configured or invalid in ApplicationSettings.");
            }
            // Bạn có thể thêm kiểm tra cho SMTPAccount và SMTPPassword ở đây nếu muốn bắt lỗi sớm hơn,
            // tuy nhiên, việc để trống có thể là một trường hợp sử dụng (ví dụ: relay không cần xác thực),
            // nên kiểm tra trong SendEmailAsync vẫn hợp lý.
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            if (string.IsNullOrEmpty(email)) throw new ArgumentNullException(nameof(email), "Email address cannot be null or empty.");
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject), "Email subject cannot be null or empty.");
            if (string.IsNullOrEmpty(message)) throw new ArgumentNullException(nameof(message), "Email message cannot be null or empty.");

            // Kiểm tra lại thông tin SMTP credentials ở đây, vì chúng có thể không được cung cấp
            // cho các kịch bản không yêu cầu xác thực SMTP.
            if (string.IsNullOrEmpty(_settings.Value.SMTPAccount))
            {
                // Có thể ghi log cảnh báo thay vì throw exception nếu SMTP relay không cần tài khoản
                Console.WriteLine("Warning: SMTPAccount is not configured. Attempting to send without authentication if server allows.");
                // throw new InvalidOperationException("SMTPAccount is not configured properly in ApplicationSettings.");
            }
            if (string.IsNullOrEmpty(_settings.Value.SMTPPassword) && !string.IsNullOrEmpty(_settings.Value.SMTPAccount))
            {
                // Chỉ throw lỗi nếu có tài khoản nhưng không có mật khẩu
                Console.WriteLine("Warning: SMTPPassword is not configured for the provided SMTPAccount. Attempting to send without authentication if server allows, or if account does not require password.");
                // throw new InvalidOperationException("SMTPPassword is not configured properly for the provided SMTPAccount in ApplicationSettings.");
            }


            Console.WriteLine($"Attempting to send email to: {email} with subject: '{subject}'");
            Console.WriteLine($"Using SMTP Server: {_settings.Value.SMTPServer}, Port: {_settings.Value.SMTPPort}");
            Console.WriteLine($"Sender Account: {_settings.Value.SMTPAccount}"); // Cẩn thận khi log thông tin nhạy cảm

            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress(
                string.IsNullOrEmpty(_settings.Value.HotelName) ? "Application Admin" : _settings.Value.HotelName, // Sử dụng tên khách sạn từ config nếu có
                _settings.Value.SMTPAccount // Tài khoản gửi phải là tài khoản đã cấu hình
            ));
            emailMessage.To.Add(new MailboxAddress("", email)); // Tên người nhận có thể để trống
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html) { Text = message }; // Ưu tiên HTML

            using (var client = new SmtpClient())
            {
                try
                {
                    Console.WriteLine("Connecting to SMTP server...");
                    // Sử dụng SecureSocketOptions.Auto để MailKit tự động chọn phương thức bảo mật tốt nhất
                    // hoặc bạn có thể cấu hình cụ thể hơn nếu biết rõ server yêu cầu (StartTls, SslOnConnect)
                    await client.ConnectAsync(_settings.Value.SMTPServer, _settings.Value.SMTPPort, MailKit.Security.SecureSocketOptions.StartTlsWhenAvailable);
                    Console.WriteLine("Connected to SMTP server.");

                    // Chỉ xác thực nếu có cả tên người dùng và mật khẩu
                    if (!string.IsNullOrEmpty(_settings.Value.SMTPAccount) && !string.IsNullOrEmpty(_settings.Value.SMTPPassword))
                    {
                        Console.WriteLine("Authenticating with SMTP server...");
                        await client.AuthenticateAsync(_settings.Value.SMTPAccount, _settings.Value.SMTPPassword);
                        Console.WriteLine("Authenticated successfully.");
                    }
                    else
                    {
                        Console.WriteLine("Proceeding without SMTP authentication (either account or password not provided).");
                    }

                    Console.WriteLine("Sending email...");
                    await client.SendAsync(emailMessage);
                    Console.WriteLine("Email sent successfully.");
                }
                catch (AuthenticationException ex)
                {
                    Console.WriteLine($"SMTP Authentication Error: {ex.Message}. Check your SMTPAccount and SMTPPassword.");
                    // Log chi tiết lỗi hơn cho nhà phát triển
                    // _logger.LogError(ex, "SMTP Authentication failed for account {SmtpAccount}", _settings.Value.SMTPAccount);
                    throw new InvalidOperationException($"SMTP authentication failed. Please check credentials. Server response: {ex.Message}", ex);
                }
                catch (SmtpCommandException ex)
                {
                    Console.WriteLine($"SMTP Command Error (Status Code: {ex.StatusCode}): {ex.Message}");
                    // _logger.LogError(ex, "SMTP Command Exception for account {SmtpAccount}. Status Code: {StatusCode}", _settings.Value.SMTPAccount, ex.StatusCode);
                    throw new InvalidOperationException($"An SMTP command failed. Server response ({ex.StatusCode}): {ex.Message}", ex);
                }
                catch (MailKit.Security.SslHandshakeException ex)
                {
                    Console.WriteLine($"SSL/TLS Handshake Error: {ex.Message}. Ensure your SMTP server's SSL/TLS certificate is valid and that the correct SecureSocketOptions are used.");
                    throw new InvalidOperationException($"SSL/TLS handshake with the SMTP server failed: {ex.Message}", ex);
                }
                catch (System.Net.Sockets.SocketException ex)
                {
                    Console.WriteLine($"Socket Connection Error: {ex.Message}. Check SMTP server address, port, and network connectivity.");
                    throw new InvalidOperationException($"Could not connect to SMTP server {_settings.Value.SMTPServer}:{_settings.Value.SMTPPort}. Error: {ex.Message}", ex);
                }
                catch (Exception ex) // Bắt các lỗi chung khác
                {
                    Console.WriteLine($"General Error sending email: {ex.ToString()}"); // Log toàn bộ exception để debug
                    // _logger.LogError(ex, "An unexpected error occurred while sending email to {EmailAddress}", email);
                    throw new InvalidOperationException($"Failed to send email due to an unexpected error: {ex.Message}", ex);
                }
                finally
                {
                    if (client.IsConnected)
                    {
                        await client.DisconnectAsync(true);
                        Console.WriteLine("Disconnected from SMTP server.");
                    }
                }
            }
        }
    }
}
