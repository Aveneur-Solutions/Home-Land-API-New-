using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Adminstrator;
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
        public async Task<ActionResult<List<UserDTO>>> AllUsers()
        {
            return await Mediator.Send(new GetAllUsers.Query());
        }
        [HttpPost("Gallery")]
        public async Task<ActionResult<Unit>> UploadPhotoToGallery([FromForm]ImageUpload.Command command)
        {
            // command.File = file;
            return await Mediator.Send(command);
        }
        [HttpGet("Images")]
        public async Task<ActionResult<List<Image>>> GetAllImages()
        {
            return await Mediator.Send(new ListImages.Query());
        }
    }
}