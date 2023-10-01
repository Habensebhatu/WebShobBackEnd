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
            confirmationBuilder.HtmlBody = $"Geachte {mailRequest.Name},.<br><br>Hartelijk dank voor " +
                $"uw interesse in onze diensten" +
                $" voor het bouwen van webshops. Wij waarderen het vertrouwen dat u in ons " +
                $"stelt en stellen het op prijs " +
                $"dat u de tijd heeft genomen om ons offerteformulier in te vullen." +
                $".<br><br>Met deze e-mail willen wij u graag bevestigen dat wij uw " +
                $"offerteaanvraag hebben ontvangen" +
                $" en deze momenteel in behandeling is. Ons team van experts is druk bezig met het beoordelen " +
                $"van uw vereisten en zal spoedig een gedetailleerde offerte voor u opstellen." +
                $"<br><br> Met vriendelijke groet,<br><br>" +
                $"WebSheba";
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
            quoteBuilder.HtmlBody = $"Quote Request from {mailRequest.Name}<br><br>Email: {mailRequest.Email}<br><br>Telephone: {mailRequest.Telephone}<br><br>Message: {mailRequest.Body}";
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

