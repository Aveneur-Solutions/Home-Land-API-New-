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
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class CreateFlat
    {
        public class Command : IRequest
        {
            public string Id { get; set; }
            public string Size { get; set; }
            public string Price { get; set; }
            public string Level { get; set; }
            public string BuildingNumber { get; set; }
            public string NoOfBedrooms { get; set; }
            public string NoOfBaths { get; set; }
            public string NoOfBalconies { get; set; }
            public string BookingPrice { get; set; }
            public string DownPaymentDays { get; set; }
            public string NetArea { get; set; }
            public string CommonArea { get; set; }
            public IFormFile Images { get; set; }
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
                RuleFor(x => x.NetArea).NotEmpty();
                RuleFor(x => x.CommonArea).NotEmpty();
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
                if(flat != null) throw new RestException(HttpStatusCode.Conflict,new {error ="A flat with the given Id already exists"});
                var building = await _context.Buildings.FirstOrDefaultAsync(x => x.BuildingNumber == request.BuildingNumber);
                if(building == null) throw new RestException(HttpStatusCode.NotFound,new {error="No building found"});
                 flat = new Flat
                {
                    Id = request.Id,
                    Size = int.Parse(request.Size),
                    Price = double.Parse(request.Price),
                    Level = int.Parse(request.Level),
                    Building = building,
                    NoOfBalconies = int.Parse(request.NoOfBalconies),
                    NoOfBaths = int.Parse(request.NoOfBaths),
                    NoOfBedrooms = int.Parse(request.NoOfBedrooms),
                    DownPaymentDays = int.Parse(request.DownPaymentDays),
                    BookingPrice = double.Parse(request.BookingPrice),
                    NetArea = int.Parse(request.NetArea),
                    CommonArea = int.Parse(request.CommonArea),
                    IsBooked = false,
                    IsSold = false
                };

                var imageList = new List<IFormFile>();
                if(request.Images != null) imageList.Add(request.Images);

                var images = FileUpload.UploadImage(imageList, _env, "Flat");
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

                await _context.Flats.AddAsync(flat);
                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Unit.Value;

                throw new RestException(HttpStatusCode.Forbidden, new { error = "Couldn't Upload the flat Information" });
            }
        }
    }
}