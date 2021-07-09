using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.DTOs;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Adminstrator
{
    public class Stat
    {
        public class Query : IRequest<StatDTO>
        {
        }
        public class Handler : IRequestHandler<Query, StatDTO>
        {
            private readonly HomelandContext _context;

            public Handler(HomelandContext context)
            {
                _context = context;
            }

            public async Task<StatDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var users = await _context.Users.CountAsync();
                var flats = await _context.Flats.CountAsync();
                var bookings = await _context.Bookings.CountAsync();
                var transfers = await _context.TransferredFlats.CountAsync();
                var allotments = await _context.AllotMents.CountAsync();

                return new StatDTO
                {
                    TotalUsers = users,
                    TotalUnits = flats,
                    TotalBookedUnits = bookings,
                    TotalAllottedUnits = allotments,
                    TotalTransfers = transfers
                };
            }
        }
    }
}