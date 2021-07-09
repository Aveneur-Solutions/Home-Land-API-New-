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
    public class ResetPassword
    {
        public class Command : IRequest
        {
            public string NewPassword { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.NewPassword).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly UserManager<AppUser> _userManager;
            private readonly HomelandContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(HomelandContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = "No user Exists with this number" });
                var result = await _userManager.RemovePasswordAsync(user);

                if (result.Succeeded)
                {
                    var outcome = await _userManager.AddPasswordAsync(user, request.NewPassword);

                    if (outcome.Succeeded) return Unit.Value;
                }
                throw new RestException(HttpStatusCode.Unauthorized, new { error = "Couldn't remove the old password" });
            }
        }
    }
}