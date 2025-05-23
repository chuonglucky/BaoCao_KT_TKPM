namespace ASC_Web.services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
