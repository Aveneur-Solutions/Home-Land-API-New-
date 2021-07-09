using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Errors;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class UnbookFlat
    {
        public class Command : IRequest
        {
            public string FlatId { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.FlatId).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command>
        {
            private readonly HomelandContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(HomelandContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {
                var flat = await _context.Flats.FindAsync(request.FlatId);
                if (flat == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No flat found the given Id" });
                flat.IsBooked = false;
                var booking = await _context.Bookings.FirstOrDefaultAsync(x => x.FlatId == request.FlatId);
                if (booking == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No booking information found about this flat" });
                _context.Bookings.Remove(booking);
                var result = await _context.SaveChangesAsync() > 0;
                if (result) return Unit.Value;
                else throw new Exception();
            }
        }
    }
}