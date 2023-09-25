using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Http;
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
        [HttpPost]
        public async Task<ActionResult<CustomerModel>> addCustomer(CustomerModel customer)
        {

            if (customer == null)
            {
                return BadRequest();
            }

            CustomerModel result = await _customer.AddCustomer(customer);

            return customer;

        }

        [HttpGet("{customer}")]
        public async Task<ActionResult<CustomerModel>> getCategoryByCategory(string customer)
        {
            return await _customer.GetCustomerByEmail(customer);
        }

    }
}
