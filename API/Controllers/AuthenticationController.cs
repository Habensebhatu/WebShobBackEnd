using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using business_logic_layer;
using business_logic_layer.ViewModel;
using Data_layer.Migrations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly LoginBLL _loginBLL;
        public AuthenticationController()
        {
            _loginBLL = new LoginBLL();
        }

        [HttpPost("login")]
        public async Task<ActionResult<LoginModel>> Login([FromBody] LoginModel model)
        {

            LoginModel user = await _loginBLL.Authenticate(model.username, model.password);

            if (user == null)
            {
                return BadRequest(new { message = "Invalid email or password" });
            }

            return user;
        }

    }
}
