namespace ASC_Web.Configuration
{
    public class ApplicationSettings
    {
        public string ApplicationTitle { get; set; }
        public string HotelName { get; set; }
        public string DefaultCurrency { get; set; }
        public int MaxBookingDaysInAdvance { get; set; }
        public int DefaultCancellationPolicyDays { get; set; }
        public string AdminEmail { get; set; }
        public string AdminName { get; set; }
        public string AdminPassword { get; set; }
        public string ManagerEmail { get; set; }
        public string ManagerName { get; set; }
        public string ManagerPassword { get; set; }
        public string Roles { get; set; }
        public string SMTPServer { get; set; }
        public int SMTPPort { get; set; }
        public string SMTPAccount { get; set; }
        public string SMTPPassword { get; set; }
    }
}
