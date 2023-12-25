using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly customerBLL _customer;

        public CustomerController()
        {
            _customer = new customerBLL();
        }

        [HttpPost("AddCustomer")]
        public async Task<ActionResult<CustomerModel>> AddCustomer([FromBody] CustomerModel customer)
        {
            if (customer == null)
            {
                return BadRequest();
            }

            CustomerModel result = await _customer.AddCustomer(customer);
            return result;
        }

        [HttpGet("GetCustomerByEmail/{email}")]
        public async Task<ActionResult<CustomerModel>> GetCustomerByEmail(string email)
        {
            return await _customer.GetCustomerByEmail(email);
        }

        [HttpGet("GetCustomerByEmail")]
        public async Task<ActionResult<IEnumerable<CustomerModel>>> GetCustomers()
        {
            return await _customer.GetCustomers();
        }
    }
}
