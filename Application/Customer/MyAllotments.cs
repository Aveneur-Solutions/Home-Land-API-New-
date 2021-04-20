using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Errors;
using Domain.UnitBooking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Customer
{
    public class MyAllotments
    {
        public class Query : IRequest<List<AllotmentDTO>>
        {
        }
        public class Handler : IRequestHandler<Query, List<AllotmentDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(HomelandContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<AllotmentDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = _userAccessor.GetUserRole();
                if (role == "User")
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());

                    var allotments = await _context.AllotMents
                    .Include(x => x.Flat)
                    .Include(x => x.User)
                    .Where(x => x.User.PhoneNumber == user.PhoneNumber)
                    .ToListAsync();
                    //  if (allotments.Capacity == 0) throw new RestException(HttpStatusCode.NoContent, new { error = "No Allotments available right now" });
                    var mappedAllotments = _mapper.Map<List<AllotMent>, List<AllotmentDTO>>(allotments);
                    return mappedAllotments;
                }
                else throw new RestException(HttpStatusCode.Forbidden, new { error = "Power er misuse kora uchit na . ei Api shudhu Customer er" });

            }
        }
    }
}