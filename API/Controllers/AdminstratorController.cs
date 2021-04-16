using System.Collections.Generic;
using System.Threading.Tasks;
using Application.Adminstrator;
using Domain.DTOs;
using Microsoft.AspNetCore.Authorization;
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
        
    }
}