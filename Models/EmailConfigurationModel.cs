namespace Models
{
    public class EmailConfigurationModel
    {
        public string SMTPServerAddress { get; set; }
        public int Port { get; set; }
        public string From { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}