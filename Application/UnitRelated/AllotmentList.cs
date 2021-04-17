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
    public class AllotmentList
    {
        public class Query : IRequest<List<AllotmentDTO>>
        {

        }
        public class Handler : IRequestHandler<Query, List<AllotmentDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;

            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<AllotmentDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var allotments = await _context.AllotMents
                .Include(x => x.Flat)
                .Include(x => x.User)
                .ToListAsync();

                if (allotments.Capacity == 0) throw new RestException(HttpStatusCode.NoContent, new { error = "No Allotments available right now" });

                var mappedAllotments = _mapper.Map<List<AllotMent>,List<AllotmentDTO>>(allotments);
                
                return mappedAllotments;
            }
        }
    }
}