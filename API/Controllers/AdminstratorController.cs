using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Adminstrator;
using Application.Customer;
using Application.SSLCommerz;
using Application.UnitRelated;
using Domain.Announcements;
using Domain.Common;
using Domain.DTOs;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace API.Controllers
{
    [Authorize(Roles = "Super Admin")]
    public class AdminstratorController : BaseController
    {
        [HttpGet("UserList")]
        public async Task<ActionResult<List<CustomerDTO>>> AllUsers()
        {
            return await Mediator.Send(new GetAllUsers.Query());
        }
        [HttpPost("Gallery")]
        public async Task<ActionResult<Unit>> UploadPhotoToGallery([FromForm] ImageUpload.Command command)
        {
            // command.File = file;
            return await Mediator.Send(command);
        }
        [HttpGet("Images")]
        [AllowAnonymous]
        public async Task<ActionResult<List<Image>>> GetAllImages()
        {
            return await Mediator.Send(new ListImages.Query());
        }
        [HttpGet("Images/{id}")]
        public async Task<ActionResult<Image>> GetImage(Guid id)
        {
            return await Mediator.Send(new ImageDetails.Query { Id = id });
        }
        [HttpDelete("Images/{id}")]
        public async Task<ActionResult<Unit>> DeleteImage(Guid id)
        {
            return await Mediator.Send(new DeleteImage.Command { Id = id });
        }
        [HttpDelete("Unbook/{id}")]
        public async Task<ActionResult<Unit>> UnbookFlat(string id)
        {
            return await Mediator.Send(new UnbookFlat.Command { FlatId = id });
        }
        [HttpGet("customerDetails/{phoneNumber}")]
        public async Task<ActionResult<CustomerDetailsDTO>> CustomerDetails(string phoneNumber)
        {
            return await Mediator.Send(new CustomerDetails.Query { PhoneNumber = phoneNumber });
        }
        [HttpGet("stats")]
        public async Task<ActionResult<StatDTO>> GetStats()
        {
            return await Mediator.Send(new Stat.Query());
        }
         [HttpGet("announcements")]
         [AllowAnonymous]
        public async Task<ActionResult<List<Announcement>>> GetAnnouncements()
        {
            return await Mediator.Send(new ViewAnnouncements.Query());
        }
        [HttpPost("createAnnouncement")]
        public async Task<ActionResult<Unit>> CreateAnnouncement([FromForm] CreateAnnouncement.Command command)
        {
            return await Mediator.Send(command);
        }
        [HttpDelete("deleteAnnouncement/{id}")]
        public async Task<ActionResult<Unit>> DeleteAnnouncement(Guid id)
        {
            return await Mediator.Send(new DeleteAnnouncement.Command{Id = id});
        }

    }
}
