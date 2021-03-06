using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Errors;
using Domain.UnitBooking;
using Domain.UserAuth;
using FluentValidation;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class TransferFlat
    {
        public class Command : IRequest
        {
            public string Password { get; set; }
            public string RecieverPhoneNumber { get; set; }
            public List<string> FlatIds { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.RecieverPhoneNumber).NotEmpty();
                RuleFor(x => x.FlatIds).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly HomelandContext _context;
            private readonly IUserAccessor _userAccessor;
            private readonly UserManager<AppUser> _userManager;
            public Handler(HomelandContext context, IUserAccessor userAccessor, UserManager<AppUser> userManager)
            {
                _userManager = userManager;
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var sender = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                if (sender == null) throw new RestException(HttpStatusCode.NotFound, new { error = "You don't have any account or you are not logged in " });

                var check = _userManager.CheckPasswordAsync(sender,request.Password);

                if(!check.Result) throw new RestException(HttpStatusCode.Unauthorized,new {error="Incorrect Password"});

                var reciever = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.RecieverPhoneNumber);
                if (reciever == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No account found with this number" });
                if (sender.PhoneNumber == reciever.PhoneNumber) throw new RestException(HttpStatusCode.BadRequest, new { error = "You can't transfer unit to your own account" });
                if(await _userManager.IsInRoleAsync(reciever,"Super Admin")) throw new RestException(HttpStatusCode.BadRequest, new { error = "You can't transfer unit to this account" });
                
               var alreadyTransferred = checkIfAlreadyTransferred(request.FlatIds);
               var alreadyTransferredFlatIds = alreadyTransferred.Result;
               if(!String.IsNullOrEmpty(alreadyTransferredFlatIds) ) throw new RestException(HttpStatusCode.MethodNotAllowed,new {error = "You can't transfer the following units"+alreadyTransferredFlatIds});

                var transferList = new List<Transfer>();
                var bookingsToBeRemoved = new List<Booking>();
                var bookingsToBeAdded = new List<Booking>();

                foreach (var flatId in request.FlatIds)
                {
                    var flat = await _context.Flats.FindAsync(flatId);
                    if (flat == null) continue;
                    var bookingToBeDeleted = await _context.Bookings.FirstOrDefaultAsync(x => (x.FlatId == flatId && x.UserId == sender.Id));
                    if (bookingToBeDeleted == null) continue; // This condition will hit every time if the flats that are requested to be transfered doesn't belong to the user

                    var transferredFlat = new Transfer
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
                    flat.IsAlreadyTransferred = true;
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
            private async Task<string> checkIfAlreadyTransferred(List<string> flatIds)
            {
                Transfer flat=null;
                var transferredFlats = "";
                foreach(var flatId in flatIds)
                {
                  flat = await _context.TransferredFlats.FirstOrDefaultAsync(x => x.FlatId == flatId);
                  if(flat != null) {
                      transferredFlats+=","+flatId;
                      }
                }
                return transferredFlats;
            }
        }
    }
}