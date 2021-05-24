using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Customer;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
     [Authorize(Roles = "User")]
    public class CustomerController : BaseController
    {
        [HttpGet("myAllotments")]
        public async Task<ActionResult<List<FlatDTO>>> MyAllotments()
        {
            return await Mediator.Send(new MyAllotments.Query { });
        }
        [HttpGet("myBookings")]
        public async Task<ActionResult<List<FlatDTO>>> MyBookings()
        {
            return await Mediator.Send(new MyBookings.Query { });
        }
        [HttpGet("myTransfers")]
        public async Task<ActionResult<List<TransferDTO>>> MyTransfers()
        {
            return await Mediator.Send(new MyTransfers.Query { });
        }
    }
}