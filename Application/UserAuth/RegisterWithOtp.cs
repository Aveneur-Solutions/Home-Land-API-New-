using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Errors;
using Domain.UserAuth;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserAuth
{
    public class RegisterWithOtp
    {
        public class Command : IRequest<UserDTO>
        {
            public string PhoneNumber { get; set; }
            public string Otp { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.Otp).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, UserDTO>
        {
            private readonly HomelandContext _context;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly UserManager<AppUser> _userManager;
            public Handler(HomelandContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _context = context;

            }

            public async Task<UserDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);

                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No user found with this number" });
                if (user.OTP == request.Otp)
                {
                    user.PhoneNumberConfirmed = true;
                    user.OTP = null;
                    await _userManager.UpdateAsync(user);
                    string roleName = "";
                    if (await _userManager.IsInRoleAsync(user, "Super Admin")) roleName = "Super Admin";
                    else if (await _userManager.IsInRoleAsync(user, "Admin")) roleName = "Admin";
                    else roleName = "User";
                    return new UserDTO
                    {
                        Token = _jwtGenerator.CreateToken(user, roleName),
                        Fullname = user.FirstName + " " + user.LastName,
                        PhoneNumber = user.PhoneNumber
                    };
                }
                else throw new RestException(HttpStatusCode.Unauthorized, new { error = "bhung bhang credentials dile dhukte parben na" });
            }
        }
    }
}