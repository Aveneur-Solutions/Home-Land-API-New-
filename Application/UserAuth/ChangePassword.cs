using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Errors;
using Domain.UserAuth;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserAuth
{
    public class ChangePassword
    {
        public class Command : IRequest
        {
            public string OldPassword { get; set; }
            public string NewPassword { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.OldPassword).NotEmpty();
                RuleFor(x => x.NewPassword).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly HomelandContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(HomelandContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _userAccessor = userAccessor;
                _context = context;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = "No user found" });
                var result = await _userManager.ChangePasswordAsync(user,request.OldPassword,request.NewPassword);


                if (result.Succeeded)
                {
                    await _userManager.UpdateAsync(user);
                    return Unit.Value;
                }
                throw new RestException(HttpStatusCode.Unauthorized, new { error = "Couldn't change the password" });
            }
        }
    }
}