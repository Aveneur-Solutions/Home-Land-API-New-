using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Adminstrator
{
    public class ListImages
    {
        public class Query : IRequest<List<Image>> { }

        public class Handler : IRequestHandler<Query, List<Image>>
        {
            private readonly HomelandContext _context;
            public Handler(HomelandContext context)
            {
                _context = context;
            }

            public async Task<List<Image>> Handle(Query request, CancellationToken cancellationToken)
            {
                return await _context.Images.ToListAsync();
            }
        }
    }
}