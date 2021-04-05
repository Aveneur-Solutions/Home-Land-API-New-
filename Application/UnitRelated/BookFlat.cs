using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Errors;
using Domain.UnitBooking;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class BookFlat
    {
        public class Command : IRequest
        {
            public string PhoneNumber { get; set; }
            public string FlatId { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.PhoneNumber).NotEmpty();
            }
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
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.PhoneNumber);
                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { error = "You don't have any account here with this number . sorry" });

                var flat = await _context.Flats.FindAsync(request.FlatId);
                if (flat == null) throw new RestException(HttpStatusCode.NotFound, new { error = "The Flat you want to book does not exist" });

                if (!flat.IsBooked)
                {
                    flat.IsBooked = true;
                    Booking booking = new Booking
                    {
                        User = user,
                        Flat = flat,
                        DateBooked = DateTime.Now
                    };

                   await _context.Bookings.AddAsync(booking);
                    _context.Flats.Update(flat);

                    var result = await _context.SaveChangesAsync() > 0 ;
                    if(result) return Unit.Value;
                    else throw new RestException(HttpStatusCode.Forbidden,new {error ="Couldn't Complete the booking"});
                }
                else throw new RestException(HttpStatusCode.BadRequest, new { error = "Sorry This flat has already been booked" });
           
            }
        }
    }
}