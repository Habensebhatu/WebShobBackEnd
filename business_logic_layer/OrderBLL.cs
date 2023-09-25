using System;
using business_logic_layer.ViewModel;
using Data_layer;
using Data_layer.Context;

namespace business_logic_layer
{
    public class OrderBLL
    {
        private readonly OrderDAL _orderDAL;
        public OrderBLL()
        {
            _orderDAL = new OrderDAL();
        }

        public async Task<OrderModel> AddOrder(OrderModel order)
        {

            Order FormatOrder = new Order()
            {
                OrderId = new Guid(),
                OrderDate = DateTime.Now,
                CustomerId = order.CustomerId,
                OrderNumber = order.OrderNumber,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetail
                {
                    OrderDetailId = new Guid(),
                    ProductId = od.ProductId,
                    Quantity = od.Quantity,
                    AmountTotal = od.AmountTotal / 100,

                }).ToList()
            };
            await _orderDAL.AddOrder(FormatOrder);
            return order;

        }

        public async Task<List<OrderModel>> GetOrders()
        {
            List<Order> ordersFromDb = await _orderDAL.GetOrders();

            return ordersFromDb.Select(order => new OrderModel
            {
                OrderId = order.OrderId,
                OrderDate = order.OrderDate,
                CustomerId = order.CustomerId,
                recipientName = order.Customer.recipientName,
                OrderNumber = order.OrderNumber,
                OrderDetails = order.OrderDetails.Select(od => new OrderDetailModel
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    Title = od.Product.Title,
                    Quantity = od.Quantity,
                    AmountTotal = od.AmountTotal,
                    ImageUrl = od.Product.ProductImages?.FirstOrDefault()?.ImageUrl,
                    Price = od.Product.Price


                }).ToList()
            }).ToList();
        }
        public async Task<GetOrderModel> GetOrderById(Guid id)
        {
            Order orderFromDb = await _orderDAL.GetOrderById(id);

            if (orderFromDb == null)
                return null;

            return new GetOrderModel
            {
                OrderId = orderFromDb.OrderId,
                CustomerId = orderFromDb.CustomerId,
                CustomerEmail = orderFromDb.Customer.CustomerEmail, // Assuming your Order's Customer object has an Email property
                recipientName = orderFromDb.Customer.recipientName,
                line1 = orderFromDb.Customer.line1,  // Assuming line1, city, postalCode, and phoneNumber properties exist in your Order's Customer object
                city = orderFromDb.Customer.city,
                postalCode = orderFromDb.Customer.postalCode,
                phoneNumber = orderFromDb.Customer.phoneNumber,
                OrderDate = orderFromDb.OrderDate,
                OrderNumber = orderFromDb.OrderNumber,
                OrderDetails = orderFromDb.OrderDetails.Select(od => new OrderDetailModel
                {
                    OrderDetailId = od.OrderDetailId,
                    ProductId = od.ProductId,
                    Title = od.Product.Title,
                    Quantity = od.Quantity,
                    AmountTotal = od.AmountTotal,
                    ImageUrl = od.Product.ProductImages?.FirstOrDefault()?.ImageUrl,
                    Price = od.Product.Price
                }).ToList()
            };
        }


    }
}

