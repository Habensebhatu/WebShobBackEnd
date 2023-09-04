using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;

namespace business_logic_layer
{
	public class customerBLL
	{
        private readonly CustomerDAL _customerDAL;
        public customerBLL()
		{
			_customerDAL = new CustomerDAL();
		}
        public async Task<CustomerModel> AddCustomer(CustomerModel customer)
        {

            CustomerEntityModel FormatOrder = new CustomerEntityModel()
            {
                CustomerId = customer.CustomerId,
                recipientName = customer.recipientName,
                CustomerEmail = customer.CustomerEmail,
                city = customer.city,
                line1 = customer.line1,
                phoneNumber = "0618823849",
                postalCode = customer.postalCode
                
            };
            await _customerDAL.AddCustomer(FormatOrder);
            return customer;

        }

        public async Task<CustomerModel> GetCustomerByEmail(string customerEmail)
        {
            var customer = await _customerDAL.getCustomerBYEmail(customerEmail);
            if (customer == null)
            {
                return null;
            }
            return new CustomerModel {
              CustomerId = customer.CustomerId,
              recipientName = customer.recipientName,
              city = customer.city,
              phoneNumber = customer.phoneNumber,
              CustomerEmail= customer.CustomerEmail,
              postalCode = customer.postalCode,
              line1 = customer.line1

            };
        }
    }
}

