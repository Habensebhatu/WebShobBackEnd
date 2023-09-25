using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Stripe;
using Stripe.Checkout;
using business_logic_layer;  // Added this line to use your business layer
using shortid;
using IdGen;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly OrderBLL _orderBLL;
        private readonly ProductBLL _productBLL;
        private readonly customerBLL _customer;
        private readonly IEmailService _emailService;
        public StripeController(IEmailService emailService)
        {
            StripeConfiguration.ApiKey = "sk_test_51NTNZBD7MblCQnUpNgCct1zsd7QMxOPgbKvgmZNKSOODW7xAk6VJm8trHx9ledkEj4nZ5CqzZuDoZslvYLcAmuuw00isSEHR10";
            _orderBLL = new OrderBLL();
            _productBLL = new ProductBLL();
            _customer = new customerBLL();
            _emailService = emailService;
        }

        [HttpPost("checkout")]
        public ActionResult Create([FromBody] CheckoutRequestModel request)
        {

            var lineItems = new List<SessionLineItemOptions>();

            foreach (var item in request.Items)
            {
                lineItems.Add(new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        UnitAmount = (long?)(item.Price * 100),
                        Currency = "eur",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = item.Title,
                            Images = new List<string> { item.ImageUrl }
                        },

                    },
                    Quantity = item.Quantity,


                });
            }



            var options = new SessionCreateOptions
            {
                BillingAddressCollection = "required",

                PaymentMethodTypes = new List<string>

                {
                     "card",
                     "ideal"
                },

                LineItems = lineItems,
                Mode = "payment",
                ShippingAddressCollection = new SessionShippingAddressCollectionOptions
                {
                    AllowedCountries = new List<string> { "NL" },
                },

                CustomText = new SessionCustomTextOptions
                {
                    ShippingAddress = new SessionCustomTextShippingAddressOptions
                    {
                        Message = "Please note that we can't guarantee 2-day delivery for PO boxes at this time.",
                    },
                    Submit = new SessionCustomTextSubmitOptions
                    {
                        Message = "We'll email you instructions on how to get started.",
                    },
                },
                SuccessUrl = "http://localhost:4200/payment-success",
                CancelUrl = "http://localhost:4200/payment-cancelled",

            };


            var service = new SessionService();
            Session session;
            try
            {
                session = service.Create(options);
            }
            catch (StripeException e)
            {
                return BadRequest(e.StripeError.Message);
            }

            return Ok(new { id = session.Id });
        }



        const string endpointSecret = "whsec_e0b277601cf11f1a539bf0be090a11455fed7feb64d40bef2817f12e9856f208";


        [HttpPost("webhook")]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeSignatureHeader = Request.Headers["Stripe-Signature"];

            if (string.IsNullOrEmpty(stripeSignatureHeader))
            {
                return BadRequest("Stripe-Signature header is missing.");
            }

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(json, stripeSignatureHeader, endpointSecret);

                if (stripeEvent.Type == "checkout.session.completed")
                {
                    var session = stripeEvent.Data.Object as Stripe.Checkout.Session;
                    if (session == null)
                    {
                        return BadRequest("Unexpected event type.");
                    }

                    var service = new Stripe.Checkout.SessionService();
                    var sessionWithLineItems = service.Get(session.Id, new Stripe.Checkout.SessionGetOptions
                    {
                        Expand = new List<string> { "line_items" }
                    });
                    Console.WriteLine($"session session session session: {session}");
                    string customerEmail = session.CustomerEmail;
                    string paymentMethodType = null;

                    // If customerEmail is empty, fetch associated PaymentIntent to get the email
                    if (string.IsNullOrEmpty(customerEmail) && !string.IsNullOrEmpty(session.PaymentIntentId))
                    {
                        var paymentIntentService = new PaymentIntentService();
                        var paymentIntent = paymentIntentService.Get(session.PaymentIntentId);
                        customerEmail = paymentIntent.ReceiptEmail;

                        var paymentMethodService = new PaymentMethodService();
                        var paymentMethod = paymentMethodService.Get(paymentIntent.PaymentMethodId);

                        // Get the Type
                        paymentMethodType = paymentMethod.Type;
                        Console.WriteLine($"Payment method type used: {paymentMethodType}");

                    }
                    Console.WriteLine($"session.ShippingDetails: {session.ShippingDetails}");

                    var shippingDetails = session.ShippingDetails;
                    var shippingAddress = shippingDetails.Address;
                    var existingCustomer = await _customer.GetCustomerByEmail(customerEmail);
                    if (existingCustomer == null && shippingDetails != null)
                    {

                        // If not, create new customer
                        var newCustomer = new CustomerModel
                        {
                            CustomerId = Guid.NewGuid(),
                            CustomerEmail = customerEmail,
                            recipientName = shippingDetails.Name,
                            city = shippingAddress.City,
                            phoneNumber = "04672939939",
                            line1 = shippingAddress.Line1,
                            postalCode = shippingAddress.PostalCode

                        };
                        existingCustomer = await _customer.AddCustomer(newCustomer);
                    }

                    if (existingCustomer == null)
                    {
                        // Handle this error appropriately. Maybe log it and/or return a relevant response.
                        return BadRequest("Unable to create or fetch the customer.");
                    }
                    var generator = new IdGenerator(0);
                    long uniqueId = generator.CreateId() % 100000000;

                    OrderModel orderModel = new OrderModel
                    {
                        CustomerId = existingCustomer.CustomerId,
                        OrderNumber = uniqueId,
                        OrderDetails = new List<OrderDetailModel>()
                    };

                    Console.WriteLine($"shippingDetails.Name: {shippingDetails.Name}");
                    mailRequestModel mailRequest = new mailRequestModel();
                    mailRequest.CustomerName = customerEmail;
                    mailRequest.recipientName = shippingDetails.Name;
                    mailRequest.city = shippingAddress.City;
                    mailRequest.line1 = shippingAddress.Line1;
                    mailRequest.postalCode = shippingAddress.PostalCode;
                    mailRequest.OrderDate = DateTime.Now;
                    mailRequest.OrderNummer = uniqueId;
                    mailRequest.paymentMethodType = paymentMethodType;
                    foreach (var lineItem in sessionWithLineItems.LineItems)
                    {
                        Console.WriteLine($"ineItem.Description: {lineItem.Description}");

                        StripeImage product = await _productBLL.GetProductsByProductName(lineItem.Description);


                        orderModel.OrderDetails.Add(new OrderDetailModel
                        {
                            ProductId = product.productId,
                            Quantity = (int)lineItem.Quantity,
                            AmountTotal = (decimal)lineItem.AmountTotal

                        });
                        Console.WriteLine($"lineItem.AmountTotal: {lineItem.AmountTotal}");
                        mailRequest.OrderItems.Add(new OrderItemModel
                        {
                            ProductName = lineItem.Description,
                            Quantity = (int)lineItem.Quantity,
                            Price = (decimal)lineItem.AmountTotal / (decimal)lineItem.Quantity / 100,
                            Total = (decimal)lineItem.AmountTotal / 100,
                            ImageUrl = product.ImageUrls

                        });
                    }
                    Console.WriteLine($"Ordermodel: {orderModel}");


                    // Now save using your business logic layer


                    await _orderBLL.AddOrder(orderModel);
                    await _emailService.SendEmailAsync(mailRequest);
                }
                else
                {
                    Console.WriteLine("Unhandled event type: {0}", stripeEvent.Type);
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }


    }
}

