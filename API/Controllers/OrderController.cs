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
    public class OrderController : ControllerBase
    {
        private readonly OrderBLL _orderBLL;

        public OrderController()
        {
            _orderBLL = new OrderBLL();
        }

        [HttpPost]
        public async Task<ActionResult<OrderModel>> addProduct([FromBody] OrderModel orderModel)
        {
            if (orderModel == null)
            {
                return BadRequest();
            }

            OrderModel  result = await _orderBLL.AddOrder(orderModel);

            return result;
        }

        [HttpGet]
        public async Task<ActionResult<List<OrderModel>>> GetOrders()
        {
            List<OrderModel> orderModels = await _orderBLL.GetOrders();

            if (orderModels == null || !orderModels.Any())
                return NotFound();

            return orderModels;
        }


        [HttpGet("{id}")]
        public async Task<ActionResult<GetOrderModel>> GetOrderById(Guid id)
        {
            GetOrderModel orderModel = await _orderBLL.GetOrderById(id);

            if (orderModel == null)
                return NotFound();

            return orderModel;
        }


    }
}
