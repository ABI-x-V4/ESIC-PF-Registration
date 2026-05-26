using DataModels;
using Microsoft.AspNetCore.Mvc;
using Repository.User;

namespace ESIC_PF_Registration.Controllers
{
    public class LoginController : Controller
    {
        private readonly IUser _iuser;

        public LoginController(IUser iuser)
        {
            _iuser = iuser;
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserDTO model)
        {
            var result = await _iuser.LoginAsync(model.Username, model.Password);

            if (result == null)
                return Unauthorized("Invalid username or password");

            return Ok(result);
        }
    }
}
