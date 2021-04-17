using System;
using System.Collections.Generic;
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
            public List<string> FlatIds { get; set; }

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


                var bookings = new List<Booking>();
                var bookedFlatIds = new List<string>();
                foreach (var flatId in request.FlatIds)
                {
                    var flat = await _context.Flats.FindAsync(flatId);

                    if (flat == null) continue;

                    if (!flat.IsBooked)
                    {
                        flat.IsBooked = true;
                        Booking booking = new Booking
                        {
                            User = user,
                            Flat = flat,
                            DateBooked = DateTime.Now
                        };
                        bookings.Add(booking);


                    }
                    else bookedFlatIds.Add(flat.Id);
                }


                if (bookedFlatIds.Count == 0 && bookings.Count >= 1)
                {
                    await _context.Bookings.AddRangeAsync(bookings);
                    var result = await _context.SaveChangesAsync() > 0;
                    if (result) return Unit.Value;
                    else throw new RestException(HttpStatusCode.BadRequest, new { error = "Couldn't Complete the booking" });
                }

                else if (bookedFlatIds.Count >= 1)
                {
                    var errorMessage = "";
                    foreach (var flatId in bookedFlatIds)
                    {
                        errorMessage += flatId + "/";
                    }
                    errorMessage += " numbered flat(s) are booked";
                    throw new RestException(HttpStatusCode.BadRequest, new { error = errorMessage });
                }

                else
                {
                    throw new RestException(HttpStatusCode.NotFound, new { error = "No flat found with the ids given" });
                }



            }
        }
    }
}