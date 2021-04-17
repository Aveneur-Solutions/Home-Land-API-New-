using System.Collections.Generic;
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
    public class TransferList
    {
           public class Query : IRequest<List<TransferDTO>>
        {

        }
        public class Handler : IRequestHandler<Query, List<TransferDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;

            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<TransferDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var transfers = await _context.TransferredFlats
                .Include(x => x.Transmitter)
                .Include(x => x.Reciever)
                .Include(x => x.Flat)
                .ToListAsync();

                var mappedTransfers = _mapper.Map<List<TransferredFlat>, List<TransferDTO>>(transfers);
                return mappedTransfers;
            }
        }
    }
}