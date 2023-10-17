using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactUsController : ControllerBase
    {
        private readonly IEmailContactUs emailService;
        public ContactUsController(IEmailContactUs service)
        {
            this.emailService = service;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitQuote([FromBody] MailContactUS contactUsRequest)
        {
            MailContactUS mailrequest = new MailContactUS();
            mailrequest.Email = contactUsRequest.Email;
            mailrequest.Name = contactUsRequest.Name;
            mailrequest.Body = contactUsRequest.Body;
            mailrequest.Telephone = contactUsRequest.Telephone;
            await emailService.SendEmailAsync(mailrequest);

            return Ok();
        }
    }
}
