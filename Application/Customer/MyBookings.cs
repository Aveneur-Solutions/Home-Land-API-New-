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
    public class MyBookings
    {
        public class Query : IRequest<List<BookingDTO>> { }
        public class Handler : IRequestHandler<Query, List<BookingDTO>>
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

            public async Task<List<BookingDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var role = _userAccessor.GetUserRole();
                if (role == "User")
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                    var bookings = await _context.Bookings
                    .Include(x => x.Flat)
                    .Include(x => x.User)
                    .Where(x => x.User.PhoneNumber == user.PhoneNumber)
                    .ToListAsync();
                    //  if (allotments.Capacity == 0) throw new RestException(HttpStatusCode.NoContent, new { error = "No Allotments available right now" });
                    var mappedBookings = _mapper.Map<List<Booking>, List<BookingDTO>>(bookings);
                    return mappedBookings;

                }
                else throw new RestException(HttpStatusCode.Forbidden, new { error = "Power er misuse kora uchit na . ei Api shudhu Customer er" });
                // var role = _userAccessor.GetUserRole();
                // if (role == "User")
                // {
                // }
                // else throw new RestException(HttpStatusCode.Forbidden, new { error = "Power er misuse kora uchit na . ei Api shudhu Customer er" });
            }
        }
    }
}