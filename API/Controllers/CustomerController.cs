using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Customer;
using Domain.DTOs;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class CustomerController : BaseController
    {
        [HttpGet("myAllotments/{phoneNumber}")]
        public async Task<ActionResult<List<AllotmentDTO>>> MyAllotments(string phoneNumber)
        {
             return await Mediator.Send(new MyAllotments.Query{ PhoneNo = phoneNumber});
        }
         [HttpGet("myBookings/{phoneNumber}")]
        public async Task<ActionResult<List<BookingDTO>>> MyBookings(string phoneNumber)
        {
             return await Mediator.Send(new MyBookings.Query{PhoneNo = phoneNumber});
        }
        
    }
}