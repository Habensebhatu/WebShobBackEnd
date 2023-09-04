using System;
namespace business_logic_layer.ViewModel
{
	public class emailSettingsModel
	{
        public string SmtpServer { get; set; }
        public int SmtpPort { get; set; }
        public string SmtpPassword { get; set; }
        public string SenderEmail { get; set; }
        public string SenderName { get; set; }
    }
}

