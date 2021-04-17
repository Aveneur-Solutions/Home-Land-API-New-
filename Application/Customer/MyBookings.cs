using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.UnitBooking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Customer
{
    public class MyBookings
    {
        public class Query : IRequest<List<BookingDTO>>
        {
            public string PhoneNo { get; set; }
        }
        public class Handler : IRequestHandler<Query, List<BookingDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;

            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<BookingDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var bookings = await _context.Bookings
                .Include(x => x.Flat)
                .Include(x => x.User)
                .Where(x => x.User.PhoneNumber == request.PhoneNo)
                .ToListAsync();

              //  if (allotments.Capacity == 0) throw new RestException(HttpStatusCode.NoContent, new { error = "No Allotments available right now" });

                var mappedBookings = _mapper.Map<List<Booking>, List<BookingDTO>>(bookings);

                return mappedBookings;
            }
        }
    }
}