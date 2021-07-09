using System.Threading.Tasks;
using Application.SSLCommerz;
using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class PaymentController : BaseController
    {
        [HttpPost("Payment")]
        [Authorize(Roles = "User")]
        public async Task<ActionResult<PaymentResponseDTO>> InitiatePayment(InitiatePayment.Command command)
        {
            return await Mediator.Send(command);
        }
        [HttpPost("success")]
        [AllowAnonymous]
        public async Task<ActionResult> SuccessPayment([FromForm]SuccessPayment.Command command)
        {
          await Mediator.Send(command);
          if(command.status == "VALID")  return Redirect(command.value_c);
          else return Redirect(command.value_d);
        }
        [HttpPost("failed")]
        [AllowAnonymous]
        public async Task<ActionResult> FailedPayment([FromForm]FailedPayment.Command command)
        {
          await Mediator.Send(command);
            return Redirect(command.value_d);
    
        }
        [HttpPost("ipn")]
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> IpnListener(IPNListener.Command command)
        {
            return await Mediator.Send(command);
        }
    }
}