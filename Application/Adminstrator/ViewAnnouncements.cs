using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Announcements;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Adminstrator
{
    public class ViewAnnouncements
    {
        public class Query : IRequest<List<Announcement>> { }

        public class Handler : IRequestHandler<Query, List<Announcement>>
        {
            private readonly HomelandContext _context;
            public Handler(HomelandContext context)
            {
                _context = context;
            }

            public async Task<List<Announcement>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Announcements.ToListAsync();
            }
        } 
    }
}