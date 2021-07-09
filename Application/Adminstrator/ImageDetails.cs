using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Common;
using Domain.Errors;
using MediatR;
using Persistence;

namespace Application.Adminstrator
{
    public class ImageDetails
    {
        public class Query : IRequest<Image>
        {
            public Guid Id { get; set; }
        }

        public class Handler : IRequestHandler<Query, Image>
        {
            private readonly HomelandContext _context;
            public Handler(HomelandContext context)
            {
                _context = context;
            }

            public async Task<Image> Handle(Query request, CancellationToken cancellationToken)
            {
                var image = await _context.Images.FindAsync(request.Id);

                if (image == null)
                {
                    throw new RestException(HttpStatusCode.NotFound, new { error = "No image found" });
                }

                return image;
            }
        }
    }
}