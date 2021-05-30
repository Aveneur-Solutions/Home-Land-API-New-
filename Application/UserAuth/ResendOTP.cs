using System.Net;
using System.Threading;
using System.Threading.Tasks;
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
    public class ResendOTP
    {
          public class Command : IRequest
        {
            public string PhoneNumber { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.PhoneNumber).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {

            private readonly UserManager<AppUser> _userManager;
            private readonly IConfiguration _configuration;
            private readonly HomelandContext _context;
            public Handler(HomelandContext context, SignInManager<AppUser> signInManager, UserManager<AppUser> userManager, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
                _userManager = userManager;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = "No account exists with this phonenumber" });
                        
                    // Commented THis line temporarily
                    // string sixDigitNumber = RandomDigitGenerator.SixDigitNumber(); // implemented in helper folder 
                    // await AuthMessageSender.SendSmsAsync(request.PhoneNumber, sixDigitNumber, _configuration);
                    
                    string sixDigitNumber = "000000";
                    user.OTP = sixDigitNumber;
                   var result =  await _userManager.UpdateAsync(user);
                   if(result.Succeeded) return Unit.Value;
                
                throw new RestException(HttpStatusCode.Unauthorized, new { error = "Problem saving otp" });
            }
        }
    }
}