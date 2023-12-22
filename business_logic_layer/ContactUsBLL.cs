using business_logic_layer.ViewModel;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;



namespace business_logic_layer
{
    public class ContactUsBLL : IEmailContactUs
    {
		
        private readonly emailSettingsModel _emailSettings;

        public ContactUsBLL(IOptions<emailSettingsModel> options)
        {
            _emailSettings = options.Value;
        }

        public async Task SendEmailAsync(MailContactUS mailRequest)
        {
            // Send confirmation text to customers
            var confirmationEmail = new MimeMessage();
            confirmationEmail.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
            confirmationEmail.To.Add(MailboxAddress.Parse(mailRequest.Email));
            confirmationEmail.Subject = "Bevestiging van uw offerteaanvraag";
            var confirmationBuilder = new BodyBuilder();
            confirmationBuilder.HtmlBody = $"Geachte {mailRequest.Name},<br><br>" +
                 $"Hartelijk dank voor uw bericht via ons 'contact ons' formulier." +
                 $" Wij waarderen het dat u contact met ons heeft opgenomen en we zullen uw bericht zo spoedig mogelijk in behandeling nemen." +
                 $"<br><br>Met deze e-mail willen wij u graag bevestigen dat wij uw bericht hebben ontvangen" +
                 $" en ons team zal hier zo spoedig mogelijk op reageren." +
                 $"<br><br> Met vriendelijke groet,<br><br>" +
                 $"Sofani market";
            confirmationEmail.Body = confirmationBuilder.ToMessageBody();

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await client.AuthenticateAsync(_emailSettings.SenderName, _emailSettings.SmtpPassword);
                await client.SendAsync(confirmationEmail);
                await client.DisconnectAsync(true);
            }

            // Send request quote information to you
            var quoteEmail = new MimeMessage();
            quoteEmail.Sender = MailboxAddress.Parse(_emailSettings.SenderEmail);
            quoteEmail.To.Add(MailboxAddress.Parse(_emailSettings.SenderEmail)); // Replace with your email address
            quoteEmail.Subject = mailRequest.Name;
            var quoteBuilder = new BodyBuilder();
            quoteBuilder.HtmlBody = $"Bericht van {mailRequest.Name}<br><br>" +
                 $"Email: {mailRequest.Email}<br><br>" +
                 $"Telefoon: {mailRequest.Telephone}<br><br>" +
                 $"Berichtinhoud: {mailRequest.Body}";
            quoteEmail.Body = quoteBuilder.ToMessageBody();

            using (var quoteClient = new SmtpClient())
            {
                await quoteClient.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.StartTls);
                await quoteClient.AuthenticateAsync(_emailSettings.SenderName, _emailSettings.SmtpPassword);
                await quoteClient.SendAsync(quoteEmail);
                await quoteClient.DisconnectAsync(true);
            }
        }
    

}
}

