using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.Errors;
using Domain.UnitBooking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class BookingList
    {
        public class Query : IRequest<List<BookingDTO>>
        {

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
                .Include(x => x.User)
                .Include(x => x.Flat)
                .ToListAsync();

                if (bookings.Capacity == 0) throw new RestException(HttpStatusCode.OK, new { error = "No booking available" });
                var mappedBookings = _mapper.Map<List<Booking>, List<BookingDTO>>(bookings);
                return mappedBookings;
            }
        }
    }
}