using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Helper;
using Application.Interfaces;
using Application.SMSService;
using Domain.Errors;
using Domain.UserAuth;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application.UserAuth
{
    public class Register
    {
        public class Command : IRequest
        {
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string PhoneNumber { get; set; }
            public string Password { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FirstName).NotEmpty();
                RuleFor(x => x.LastName).NotEmpty();
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.Password).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly SignInManager<AppUser> _signInManager;
            private readonly UserManager<AppUser> _userManager;
            private readonly IJwtGenerator _jwtGenerator;
            private readonly IConfiguration _configuration;
            private readonly HomelandContext _context;
            public Handler(HomelandContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
                _signInManager = signInManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);


                if (user == null)
                {
                    user = new AppUser
                    {
                        FirstName = request.FirstName,
                        LastName = request.LastName,
                        PhoneNumber = request.PhoneNumber,
                        UserName = request.FirstName
                    };
                    // string sixDigitNumber = RandomDigitGenerator.SixDigitNumber();
                    // user.OTP = sixDigitNumber;
                    try
                    {
                        string sixDigitNumber = "000000";
                        user.OTP = sixDigitNumber;
                        await _userManager.CreateAsync(user, request.Password);
                        //  await AuthMessageSender.SendSmsAsync(request.PhoneNumber, sixDigitNumber, _configuration);
                        return Unit.Value;
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Problem creating account", ex);
                    }
                }
                else throw new RestException(HttpStatusCode.Conflict, new { error = "a user already exists with this number" });


            }
        }
    }
}
