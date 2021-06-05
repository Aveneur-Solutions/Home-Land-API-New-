using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Errors;
using Domain.UnitBooking;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class PlaceOrder
    {
        public class Command : IRequest<OrderConfirmedDto>
        {
            public string OrderId { get; set; }
            public string[] flatIds { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.flatIds).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command,OrderConfirmedDto>
        {
            private readonly HomelandContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(HomelandContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<OrderConfirmedDto> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { error = "You don't have any account here with this number . sorry" });
                var transactionId = "Trx" + DateTime.Now.ToString("ddmmyyhhmmss");
                var order = new Order
                {
                    Id = request.OrderId,
                    OrderDate = DateTime.Now,
                    User = user,
                    TransactionId = transactionId
                };
                string listOfOrderedFlats = "";

                // foreach (var flatId in request.flatIds)
                // {
                //     var flat = await _context.Flats.FindAsync(flatId);

                //     if (flat == null) continue;

                //     if (string.IsNullOrEmpty(flat.OrderId))
                //     {
                //         flat.Order = order;
                //     }
                //     else listOfOrderedFlats = listOfOrderedFlats + flatId + ",";
                // }

                // if (listOfOrderedFlats.Length > 1) throw new RestException(HttpStatusCode.Conflict, new { error = "Couldn't place the order because " + listOfOrderedFlats + " someone already placed order for them" });

                await _context.Orders.AddAsync(order);
                await _context.Orders.ToListAsync();
                var result = await _context.SaveChangesAsync() > 0;

                if(result) return new OrderConfirmedDto{
                    TransactionID = transactionId
                };
                throw new RestException(HttpStatusCode.ExpectationFailed,new {error="Couldn't place the order"});
            }
        }
    }
}