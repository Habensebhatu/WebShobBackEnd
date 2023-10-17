using business_logic_layer;
using business_logic_layer.ViewModel;
using Microsoft.AspNetCore.Mvc;


namespace API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RegistrationController : ControllerBase
    {
        private readonly UserRegistrationBLL _userRegistrationBLL;
        private readonly IConfiguration _configuration;

        public RegistrationController(IConfiguration configuration)
        {
            _configuration = configuration;
            _userRegistrationBLL = new UserRegistrationBLL(_configuration);

        }

        [HttpPost("register")]
        public async Task<ActionResult<string>> Register(UserRegistrationModel userModel)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = await _userRegistrationBLL.RegisterUser(userModel);
            return Ok(new { token = token });
        }

        [HttpPost("login")]
        public async Task<ActionResult<string>> Login(Login loginModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            string token = await _userRegistrationBLL.LoginUser(loginModel);

            if (string.IsNullOrEmpty(token))
            {
                return Unauthorized();
            }

            return Ok(new { token = token });
        }

        [HttpGet("get-all-users")]
        public async Task<ActionResult<List<UserRegistrationModel>>> GetAllUsers()
        {
            var users = await _userRegistrationBLL.GetAllUsers();
            return Ok(users);
        }


    }
}
