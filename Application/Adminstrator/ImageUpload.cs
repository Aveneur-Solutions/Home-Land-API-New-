using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Helper;
using Domain.Common;
using Domain.Errors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Adminstrator
{
    public class ImageUpload
    {
        public class Command : IRequest
        {
            public List<IFormFile> File { get; set; }
            public string Section { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.File).NotEmpty();
            }

        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly HomelandContext _context;
            private readonly IWebHostEnvironment _environment;
            public Handler(HomelandContext context, IWebHostEnvironment environment)
            {
                _environment = environment;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                var images = FileUpload.UploadImage(request.File, _environment, string.IsNullOrEmpty(request.Section) ? null : request.Section);
                var imageList = new List<Image> { };
                if (images.Count > 0)
                {
                    foreach (var image in images)
                    {
                        Image imageToBeAdded = new Image
                        {
                            ImageLocation = image,
                            Section = request.Section
                        };
                        imageList.Add(imageToBeAdded);
                    }
                    await _context.Images.AddRangeAsync(imageList);
                    var result = await _context.SaveChangesAsync() > 0;

                    if (result) return Unit.Value;

                }
                throw new RestException(HttpStatusCode.NoContent, new { error = "Couldn't Upload the Images" });
            }
        }
    }
}