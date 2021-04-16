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
    public class CreateAllotment
    {
        public class Command : IRequest
        {
            public string flatId { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.flatId).NotEmpty();
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

                var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.FlatId == request.flatId);
                if (booking == null) throw new RestException(HttpStatusCode.NotFound, new { error = "This flat is not booked ." });

                
                // will optimize this code later 
                var allotment = await _context.AllotMents.FirstOrDefaultAsync(x => x.FlatId == request.flatId) ;

                if(allotment != null) throw new RestException(HttpStatusCode.Conflict,new {error = "This flat is already alloted"});

                var newAllotment = new AllotMent
                {
                    FlatId = request.flatId,
                    UserId = booking.UserId,
                    DateAlloted = DateTime.Now
                };

                await _context.AllotMents.AddAsync(newAllotment);

                var result = await _context.SaveChangesAsync() > 0;

                if (result) return Unit.Value;

                else throw new RestException(HttpStatusCode.BadRequest, new { error = "Problem Creating an allotment" });


            }
        }
    }
}