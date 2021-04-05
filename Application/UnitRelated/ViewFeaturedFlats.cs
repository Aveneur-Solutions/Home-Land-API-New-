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
    public class ViewFeaturedFlats
    {
        public class Query : IRequest<List<FlatDTO>>
        {

        }
        public class Handler : IRequestHandler<Query, List<FlatDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;

            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<FlatDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var flats = await _context.Flats.Where(x => x.IsFeatured).ToListAsync();
                //flats = 
                if (flats.Capacity == 0) throw new RestException(HttpStatusCode.NoContent, new { error = "No flats are set as featured" });

                var mappedFlats = _mapper.Map<List<Flat>,List<FlatDTO>>(flats);
                
                return mappedFlats;
            }
        }

    }
}