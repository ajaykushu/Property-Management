using API.Authentication.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Models.RequestModels;
using Models.ResponseModels;
using System;
using System.Threading.Tasks;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class LoginController : ControllerBase
    {
        public IConfiguration Configuration;
        private readonly IUserManager _user;

        public LoginController(ILogger<LoginController> logger, IUserManager user)
        {
            _user = user;
        }
        /// <summary>
        /// This Controller Action is Post method for Login.
        /// </summary>
        /// <param name="login">Login</param>
        /// <returns>IActionResult</returns>
        // POST: api/Logins
        [HttpPost]

        [Route("userlogin")]
        public async Task<ActionResult<TokenResponse>> UserLogin([FromBody] LoginReq login)
        {

            var tokenResponse = await _user.DoLogin(login);
            if (tokenResponse == null)
            {
                throw new Exception("Internal Server Error");
            }
            return tokenResponse;

        }

        /// <summary>
        /// This Controller Action Post Method for Email based token generation for Password Change.
        /// </summary>
        /// <param name="email">string</param>
        /// <returns>IActionResult</returns>
        [HttpGet]
        [Route("sendforgotpwdemail")]
        public async Task<ActionResult<bool>> SendForgotPasswordEmail(string email, string verificationPath)
        {
            if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(verificationPath))
            {
                var status = await _user.GetPasswordChangeTokenAsync(email, verificationPath);
                return Ok(status);
            }
            else
            {
                throw new ArgumentNullException("Some values are missing");
            }
        }
        /// <summary>
        /// This Controller Action updates the new Password of User.
        /// </summary>
        /// <param name="user">RequestUser</param>
        /// <returns>IActionResult</returns>
        [HttpPost]
        [Route("changepassword")]
        public async Task<ActionResult<bool>> ChangePassword([FromBody] PasswordChange user)
        {
            var status = await _user.ChangePassowrd(user.Email, user.Token, user.NewPassword);
            return Ok(status);
        }

    }
}
