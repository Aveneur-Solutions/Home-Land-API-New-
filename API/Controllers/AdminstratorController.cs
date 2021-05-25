using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Adminstrator;
using Application.UnitRelated;
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
        [AllowAnonymous]
        public async Task<ActionResult<Unit>> UnbookFlat(string id)
        {
            return await Mediator.Send(new UnbookFlat.Command{FlatId = id});
        }
    }
}
