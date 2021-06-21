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
    public class FlatDetails
    {
        public class Query : IRequest<FlatDTO>
        {
            public string ID { get; set; }
        }
        public class Handler : IRequestHandler<Query, FlatDTO>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;

            public Handler(HomelandContext context, IMapper mapper)
            {
                _mapper = mapper;
                _context = context;
            }

            public async Task<FlatDTO> Handle(Query request, CancellationToken cancellationToken)
            {
                var flat = await _context.Flats.Include(x => x.Images).Include(x => x.Building).FirstOrDefaultAsync(x => x.Id == request.ID);

                if (flat == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No flatt found with the given id" });


                return _mapper.Map<Flat,FlatDTO>(flat);
            }
        }
    }
}
