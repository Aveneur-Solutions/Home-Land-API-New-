using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Errors;
using FluentValidation;
using MediatR;
using Persistence;

namespace Application.UnitRelated
{
    public class SetFeaturedFlatStatus
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly HomelandContext _context;
            public Handler(HomelandContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var flat = await _context.Flats.FindAsync(request.Id);

                if(flat == null) throw new RestException(HttpStatusCode.NotFound,new {flat = "What do you want ?? emon to kono flat e nai"});
                
                flat.IsFeatured = !flat.IsFeatured;

                var result =await _context.SaveChangesAsync() > 0;
            
                if(result ) return Unit.Value;


                throw new RestException(HttpStatusCode.ResetContent,new {error = "Couldn't set the featured status try again"});
            }
        }

    }
}