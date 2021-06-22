using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Helper;
using Application.Interfaces;
using Domain.Errors;
using Domain.UserAuth;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserAuth
{
    public class UpdateProfile
    {
        public class Command : IRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string Email { get; set; }
            public string Address { get; set; }
            public string NID { get; set; }
            public IFormFile Image { get; set; }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly IWebHostEnvironment _environment;
            private readonly IUserAccessor _userAccessor;

            private readonly UserManager<AppUser> _userManager;
            private readonly HomelandContext _context;
            public Handler(HomelandContext context, UserManager<AppUser> userManager, IUserAccessor userAccessor, IWebHostEnvironment environment)
            {
                _userAccessor = userAccessor;
                _environment = environment;
                _context = context;
                _userManager = userManager;

            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                if (user == null) throw new RestException(HttpStatusCode.Unauthorized, new { error = "Log in again" });
                user.FirstName = request.FirstName ?? user.FirstName;
                user.LastName = request.LastName ?? user.LastName;
                user.Email = request.Email ?? user.Email;
                user.Address = request.Address ?? user.Address;
                user.NID = request.NID ?? user.NID;
                if (request.Image != null)
                {
                    user.ProfileImage = FileUpload.UploadSingleImage(request.Image, _environment, "User");
                }
                
                var result =  await _userManager.UpdateAsync(user);
                if(result.Succeeded) return Unit.Value;

                throw new RestException(HttpStatusCode.ExpectationFailed,new {error ="Couldn't Update profile"});
               

            }
        }
    }
}