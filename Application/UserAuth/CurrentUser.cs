using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UserAuth
{
    public class CurrentUser
    {
        public class Query : IRequest<UserDTO> { }
        public class Handler : IRequestHandler<Query, UserDTO>
        {
            private readonly HomelandContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly IJwtGenerator _jwtGenerator;

            public Handler(HomelandContext context, IUserAccessor userAccessor, IJwtGenerator jwtGenerator)
            {
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<UserDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());

                if (user == null) throw new RestException(HttpStatusCode.Unauthorized, new { user = "Apni ke?" });

                return new UserDTO
                {
                    PhoneNumber = user.PhoneNumber,
                    Fullname = user.FirstName + " " + user.LastName,
                    Token = _jwtGenerator.CreateToken(user)
                };
            }
        }
    }
}