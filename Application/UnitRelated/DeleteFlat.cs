using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Errors;
using MediatR;
using Persistence;

namespace Application.UnitRelated
{
    public class DeleteFlat
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

                if (flat == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No flat found" });

                try
                {
                    _context.Flats.Remove(flat);

                    var result = await _context.SaveChangesAsync() > 0;
                    if (result) return Unit.Value;
                }
                catch (Exception)
                {
                    throw;
                }



                throw new System.NotImplementedException();
            }
        }
    }
}