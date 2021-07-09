using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Helper;
using Domain.Announcements;
using Domain.Errors;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.Adminstrator
{
    public class CreateAnnouncement
    {
          public class Command : IRequest
        {
            public string Name { get; set; }
            public IFormFile FileLocation { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Name).NotEmpty();
                RuleFor(x => x.FileLocation).NotEmpty();

            }

        }
        public class Handler : IRequestHandler<Command>
        {
            private readonly HomelandContext _context;
            private readonly IWebHostEnvironment _env;
            public Handler(HomelandContext context, IWebHostEnvironment env)
            {
                _env = env;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var announcement = new Announcement{
                  Name = request.Name,
                  TimeUploaded = DateTime.Now,
                   FileLocation = FileUpload.UploadSingleFile(request.FileLocation,_env,"Announcements") 
                };
                await _context.Announcements.AddAsync(announcement);
                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Unit.Value;

                throw new RestException(HttpStatusCode.Forbidden, new { error = "Couldn't Upload the flat Information" });
            }
        }
    }
}