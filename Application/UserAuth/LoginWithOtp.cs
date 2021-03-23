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
using Microsoft.Extensions.Configuration;
using Persistence;

namespace Application.UserAuth
{
    public class LoginWithOtp
    {
          public class Query : IRequest<UserDTO>
        {
            public string PhoneNumber { get; set; }
            public string Otp { get; set; }
        }
        public class QueryValidator : AbstractValidator<Query>
        {
            public QueryValidator()
            {
                RuleFor(x => x.PhoneNumber).NotEmpty();
                RuleFor(x => x.Otp).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Query, UserDTO>
        {

            private readonly UserManager<AppUser> _userManager;
            private readonly Interfaces.IJwtGenerator _jwtGenerator;
            private readonly IConfiguration _configuration;
            private readonly HomelandContext _context;
            public Handler(HomelandContext context, UserManager<AppUser> userManager, IJwtGenerator jwtGenerator, IConfiguration configuration)
            {
                _context = context;
                _configuration = configuration;
                _jwtGenerator = jwtGenerator;
                _userManager = userManager;
            
            }

            public async Task<UserDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync( x=> x.PhoneNumber == request.PhoneNumber);
                if (user == null)
                    throw new RestException(HttpStatusCode.Unauthorized, new { error = "bhung bhang credentials dile dhukte parben na" });


                if(!String.IsNullOrEmpty(user.OTP) && user.OTP == request.Otp)
                {
                    user.OTP = null;
                    await _userManager.UpdateAsync(user);
                    return new UserDTO
                    {
                        Fullname = user.FirstName+" "+user.LastName,
                        PhoneNumber = user.PhoneNumber,
                        Token = _jwtGenerator.CreateToken(user)
                    };
                }
                else throw new RestException(HttpStatusCode.Unauthorized, new { error = "Faizlami Koren mia" });
                
               // throw new RestException(HttpStatusCode.Unauthorized, new { error = "bhung bhang credentials dile dhukte parben na" });
            }
        }
    }
}