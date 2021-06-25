using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Helper;
using Application.Interfaces;
using Domain.Errors;
using Domain.UserAuth;
using FluentValidation;
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
            public IFormFile ProfileImage { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.Email).NotEmpty();
                RuleFor(x => x.Address).NotEmpty();
                RuleFor(x => x.NID).NotEmpty();
            }
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
                user.FirstName = request.FirstName;
                user.LastName = request.LastName;
                user.Email = request.Email;
                user.Address = request.Address;
                user.NID = request.NID;
                if (request.ProfileImage != null)
                {
                    user.ProfileImage = FileUpload.UploadSingleImage(request.ProfileImage, _environment, "User");
                }

                var result = await _userManager.UpdateAsync(user);
                if (result.Succeeded) return Unit.Value;

                throw new RestException(HttpStatusCode.ExpectationFailed, new { error = "Couldn't Update profile" });
            }
        }
    }
}