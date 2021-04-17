using System.Collections.Generic;
using System.Threading.Tasks;
using Application.UnitRelated;
using Domain.DTOs;
using Domain.UnitBooking;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    public class FlatController : BaseController
    {
        [HttpGet]
        [AllowAnonymous]
        public async Task<ActionResult<List<FlatDTO>>> FlatList()
        {
            return await Mediator.Send(new ViewFlats.Query());
        }
        [HttpGet("featured")]
        [AllowAnonymous]
        public async Task<ActionResult<List<FlatDTO>>> FeaturedFlatList()
        {
            return await Mediator.Send(new ViewFeaturedFlats.Query());
        }
        [HttpGet("booked")]
        public async Task<ActionResult<List<FlatDTO>>> BookedFlatList()
        {
            return await Mediator.Send(new ViewBookedFlats.Query());
        }

        [HttpGet("bookings")]
        public async Task<ActionResult<List<BookingDTO>>> BookingList()
        {
            return await Mediator.Send(new BookingList.Query());
        }
        [HttpGet("transfers")]
        public async Task<ActionResult<List<TransferDTO>>> TransferList()
        {
            return await Mediator.Send(new TransferList.Query());
        }
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<ActionResult<FlatDTO>> FlatList(string id)
        {
            return await Mediator.Send(new FlatDetails.Query { ID = id });
        }
        [HttpPost]
        public async Task<ActionResult<Unit>> CreateFlat(CreateFlat.Command command)
        {
            return await Mediator.Send(command);
        }
        [HttpPut("{id}")]
        public async Task<ActionResult<Unit>> EditFlat(string id, EditFlat.Command command)
        {
            command.Id = id;
            return await Mediator.Send(command);
        }
        [HttpPut("setFeatured/{id}")]
        public async Task<ActionResult<Unit>> SetFeaturedFlatStatus(string id)
        {

            return await Mediator.Send(new SetFeaturedFlatStatus.Command { Id = id });
        }
        [HttpDelete("{id}")]
        public async Task<ActionResult<Unit>> DeleteFlat(string id)
        {

            return await Mediator.Send(new DeleteFlat.Command { Id = id });
        }
        [HttpPost("booking")]
        public async Task<ActionResult<Unit>> BookFlat(BookFlat.Command command)
        {
            return await Mediator.Send(command);
        }

        [HttpPost("transfer")]
        public async Task<ActionResult<Unit>> TransferFlat(TransferFlat.Command command)
        {
            return await Mediator.Send(command);
        }

    }
}