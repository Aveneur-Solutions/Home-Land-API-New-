using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Helper;
using Domain.Errors;
using Domain.UnitBooking;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.UnitRelated
{
    public class EditFlat
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
            public int Size { get; set; }
            public double Price { get; set; }
            public int Level { get; set; }
            public int BuildingNumber { get; set; }
            public int NoOfBedrooms { get; set; }
            public int NoOfBaths { get; set; }
            public int NoOfBalconies { get; set; }
            public double BookingPrice { get; set; }
            public int DownPaymentDays { get; set; }
            public List<IFormFile> Images { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Id).NotEmpty();
                RuleFor(x => x.Size).NotEmpty();
                RuleFor(x => x.Price).NotEmpty();
                RuleFor(x => x.Level).NotEmpty();
                RuleFor(x => x.BuildingNumber).NotEmpty();
                RuleFor(x => x.NoOfBalconies).NotEmpty();
                RuleFor(x => x.NoOfBaths).NotEmpty();
                RuleFor(x => x.NoOfBedrooms).NotEmpty();
                RuleFor(x => x.DownPaymentDays).NotEmpty();
                RuleFor(x => x.BookingPrice).NotEmpty();
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
                var flat = await _context.Flats.FindAsync(request.Id);

                if (flat == null) throw new RestException(HttpStatusCode.NotFound, new { error = " No Flat found with the given id" });

                flat.Id = request.Id;
                flat.Level = request.Level;
                flat.NoOfBalconies = request.NoOfBalconies;
                flat.NoOfBaths = request.NoOfBaths;
                flat.NoOfBedrooms = request.NoOfBedrooms;
                flat.Price = request.Price;
                flat.Size = request.Size;
                flat.BookingPrice = request.BookingPrice;
                flat.BuildingNumber = request.BuildingNumber;
                flat.DownPaymentDays = request.DownPaymentDays;

                var images = FileUpload.UploadImage(request.Images, _env, "Flat");
                var imageListToBeAdded = new List<FlatImage> { };

                foreach (var image in images)
                {
                    var imageToBeAdded = new FlatImage
                    {
                        Flat = flat,
                        ImageLocation = image
                    };
                    imageListToBeAdded.Add(imageToBeAdded);
                }

                if (imageListToBeAdded.Count > 0)
                {
                    await _context.UnitImages.AddRangeAsync(imageListToBeAdded);
                }

                _context.Flats.Update(flat);
                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Unit.Value;

                throw new System.NotImplementedException();
            }
        }
    }
}