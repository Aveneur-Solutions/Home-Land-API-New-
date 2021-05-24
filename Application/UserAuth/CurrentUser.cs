using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Errors;
using Domain.UserAuth;
using MediatR;
using Microsoft.AspNetCore.Identity;
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
            private readonly UserManager<AppUser> _userManager;

            public Handler(HomelandContext context, IUserAccessor userAccessor, IJwtGenerator jwtGenerator, UserManager<AppUser> userManager)
            {
                _userManager = userManager;
                _jwtGenerator = jwtGenerator;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<UserDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());

                if (user == null) throw new RestException(HttpStatusCode.Unauthorized, new { user = "Apni ke?" });

                string roleName = "";

                if (await _userManager.IsInRoleAsync(user, "Super Admin")) roleName = "Super Admin";
                else if (await _userManager.IsInRoleAsync(user, "Admin")) roleName = "Admin";
                else roleName = "User";

                return new UserDTO
                {
                    PhoneNumber = user.PhoneNumber,
                    Fullname = user.FirstName + " " + user.LastName,
                    Address = user.Address,
                    NID = user.NID,
                    Token = _jwtGenerator.CreateToken(user, roleName)
                };
            }
        }
    }
}