using System;
namespace business_logic_layer.ViewModel
{
    public interface IEmailService
    {
        Task SendEmailAsync(mailRequestModel mailrequest);

    }

}

