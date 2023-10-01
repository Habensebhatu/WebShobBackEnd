using System;
using business_logic_layer.ViewModel;

namespace business_logic_layer
{
	public interface IEmailContactUs
	{
        Task SendEmailAsync(MailContactUS mailRequest);
    }
}

