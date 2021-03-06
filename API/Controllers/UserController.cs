using System.Threading.Tasks;
using Application.UserAuth;
using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class UserController : BaseController
    {

        [HttpPost("login")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> Login(Login.Command command)
        {
            return await Mediator.Send(command);
        }
        [HttpPost("loginWithOtp")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> LoginWithOtp(LoginWithOtp.Query query)
        {
            return await Mediator.Send(query);
        }
        [HttpPost("register")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> Register(Register.Command command)
        {
            return await Mediator.Send(command);
        }
        [HttpPost("registerWithOtp")]
        [AllowAnonymous]
        public async Task<ActionResult<UserDTO>> RegisterWithOtp(RegisterWithOtp.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet]
        public async Task<UserDTO> GetCurrentUser()
        {
            return await Mediator.Send(new CurrentUser.Query());
        }
        
        [HttpPost("changePassword")]
        public async Task<Unit> ChangePassword(ChangePassword.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpGet("{phonenumber}")]
        public async Task<ActionResult<CustomerDTO>> GetUser(string phonenumber)
        {
            return await Mediator.Send(new GetUser.Query { PhoneNumber = phonenumber });
        }
        [HttpPost("ResendOtp")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> ResendOTP(ResendOTP.Command command)
        {
           return await Mediator.Send(command);
        }
        [HttpPost("ResetPassword")]
        public async Task<ActionResult<Unit>> ResetPassword(ResetPassword.Command command)
        {
           return await Mediator.Send(command);
        }
        [HttpPut("UpdateProfile")]
        public async Task<ActionResult<Unit>> UpdateProfile([FromForm]UpdateProfile.Command command)
        {
           return await Mediator.Send(command);
        }
    }
}
