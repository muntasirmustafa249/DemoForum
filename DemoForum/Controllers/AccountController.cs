using DemoForum.Services;
using DemoForum.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DemoForum.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly AccountService _accountService;
        public AccountController(AccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet(Name = "DemoTokenGeneration")]
        public async Task<ActionResult> DemoTokenGeneration()
        {
            var token = await _accountService.GenerateLoginToken("DemoUser", "demo@forum.me");

            LoginResponseModel response = new LoginResponseModel()
            {
                Token = token,
                Message = "Logged in successfully."
            };

            return Ok(response);
        }

        [HttpPost(Name = "DemoTokenProcessing")]
        public async Task<ActionResult> DemoTokenProcessing([FromBody] string token)
        {
            var tokenData = await _accountService.GetTokenData(token);

            if (tokenData == null) return BadRequest("Invalid token");

            if (!tokenData.IsTokenValid) return ValidationProblem("Token is no longer valid");

            return Ok(tokenData);
        }

        [HttpGet(Name = "DemoTokenFromRequestProcessing")]
        public async Task<ActionResult> DemoTokenFromRequestProcessing()
        {
            var auth = Request.Headers.Authorization;

            if (string.IsNullOrEmpty(auth.ToString()) || string.IsNullOrEmpty(auth[0])) return Ok("You are not logged in");
            var bearerToken = auth[0] ?? "";

            string token = bearerToken.Replace("Bearer ", "");

            var tokenData = await _accountService.GetTokenData(token);

            if (tokenData == null) return BadRequest("Invalid token");

            if (!tokenData.IsTokenValid) return ValidationProblem("Token is no longer valid");

            return Ok(tokenData);

            //var requestUser = Request.HttpContext.User;
            //if (requestUser == null || requestUser.Claims == null) return Ok("You are not logged in");
            //return Ok(requestUser.Claims.ToList());
        }

        [HttpPost(Name = "Login")]
        public async Task<ActionResult> Login([FromBody] LoginModel loginData)
        {
            var loginUser = await _accountService.CheckUserForLogin(loginData);
            if (loginUser == null || loginUser.Id == 0) return NotFound("Incorrect username or password");
            string loginToken = await _accountService.GenerateLoginToken(loginUser.UserName, loginUser.Email);
            LoginResponseModel response = new LoginResponseModel()
            {
                Token = loginToken,
                Message = "Logged in successfully."
            };
            return Ok(response);
        }

        [HttpPost(Name = "Register")]
        public async Task<ActionResult> Register([FromBody] RegistrationModel registrationData)
        {
            bool userExists = await _accountService.CheckUserForRegistration(registrationData);

            if (userExists) return Conflict("Provided username or email address is already used");

            await _accountService.CreateUser(registrationData);

            return Created();
        }
    }
}
