using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Domain.DTOs;
using Domain.UnitBooking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class BuildingList
    {
        public class Query : IRequest<List<BuildingDTO>>
        {

        }
        public class Handler : IRequestHandler<Query, List<BuildingDTO>>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;

            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<List<BuildingDTO>> Handle(Query request, CancellationToken cancellationToken)
            {
                var buildings = await _context.Buildings.Include(x => x.Flats).AsNoTracking().ToListAsync();

                var mappedBuildings = _mapper.Map<List<Building>,List<BuildingDTO>>(buildings);
                
                return mappedBuildings;
            }
        }
    }
}