using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Adminstrator;
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
        [AllowAnonymous]
        public async Task<ActionResult<List<CustomerDTO>>> AllUsers()
        {
            return await Mediator.Send(new GetAllUsers.Query());
        }
        [HttpPost("Gallery")]
        public async Task<ActionResult<Unit>> UploadPhotoToGallery([FromForm]ImageUpload.Command command)
        {
            // command.File = file;
            return await Mediator.Send(command);
        }
    }
}