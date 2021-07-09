using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Errors;
using MediatR;
using Persistence;

namespace Application.Adminstrator
{
    public class DeleteAnnouncement
    {
        public class Command : IRequest
        {
            public Guid Id { get; set; }
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
                var announcement = await _context.Announcements.FindAsync(request.Id);

                if (announcement == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { error = "No Announcement found" });
                }

                _context.Announcements.Remove(announcement);
                var success = await _context.SaveChangesAsync() > 0;

                if (success) return Unit.Value;

                throw new Exception("Problem deleting announcement");
            }
        }
    }
}