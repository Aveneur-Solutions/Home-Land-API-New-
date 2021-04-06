using System;
using System.Collections.Generic;
using System.Linq;
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
    public class TransferFlat
    {
        public class Command : IRequest
        {
            public string TransmitterPhoneNumber { get; set; }
            public string RecieverPhoneNumber { get; set; }
            public List<string> FlatIds { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.TransmitterPhoneNumber).NotEmpty();
                RuleFor(x => x.RecieverPhoneNumber).NotEmpty();
                RuleFor(x => x.FlatIds).NotEmpty();
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
                var reciever = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.RecieverPhoneNumber);
                if (reciever == null) throw new RestException(HttpStatusCode.NotFound, new { error = "Kaare pathaite chaan bujhlam na . Account nai to ei number e kono" });
                var sender = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.TransmitterPhoneNumber);
                if (sender == null) throw new RestException(HttpStatusCode.NotFound, new { error = "Nijer e account nai abar transfer korte chai . WOW " });
                if(sender.PhoneNumber == reciever.PhoneNumber) throw new RestException(HttpStatusCode.BadRequest,new {error = "Nijeke nije transfer korte paarben na . Eto self love bhalo na."});
               
                var transferList = new List<TransferredFlat>();
                var bookingsToBeRemoved = new List<Booking>();
                var bookingsToBeAdded = new List<Booking>();

                foreach (var flatId in request.FlatIds)
                {
                    var flat = await _context.Flats.FindAsync(flatId);
                    if (flat == null) continue;
                    var bookingToBeDeleted = await _context.Bookings.FirstOrDefaultAsync(x => (x.FlatId == flatId && x.UserId == sender.Id));
                    if (bookingToBeDeleted == null) continue; // This condition will hit every time if the flats that are requested to be transfered doesn't belong to the user

                    var transferredFlat = new TransferredFlat
                    {
                        Flat = flat,
                        Transmitter = sender,
                        Reciever = reciever,
                        TransferDate = DateTime.Now
                    };
                    var bookingToBeAdded = new Booking
                    {
                        Flat = flat,
                        User = reciever,
                        DateBooked = transferredFlat.TransferDate
                    };
                    bookingsToBeRemoved.Add(bookingToBeDeleted);
                    bookingsToBeAdded.Add(bookingToBeAdded);
                    transferList.Add(transferredFlat);
                }

                if (bookingsToBeAdded.Count == 0
                 && bookingsToBeRemoved.Count == 0
                 && transferList.Count == 0)
                    throw new RestException(HttpStatusCode.BadRequest, new { error = "Can't transfer because the flats doesn't belong to you" });

                _context.Bookings.RemoveRange(bookingsToBeRemoved);
                await _context.Bookings.AddRangeAsync(bookingsToBeAdded);
                await _context.TransferredFlats.AddRangeAsync(transferList);

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Unit.Value;

                throw new RestException(HttpStatusCode.Forbidden, new { error = "Couldn't Complete the transfer" });
            }
        }
    }
}