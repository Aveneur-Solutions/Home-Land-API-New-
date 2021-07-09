using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.UnitRelated
{
    public class CancelOrder
    {
        public class Command : IRequest
        {
            public string OrderId { get; set; }
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
                var order = await _context.Orders.FindAsync(request.OrderId);

                if (order == null) throw new RestException(HttpStatusCode.NotFound, new { error = "No Order found" });
                if (!order.PaymentConfirmed)
                {
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                    if (user.Id == order.UserId)
                    {
                        _context.Orders.Remove(order);
                        var result = await _context.SaveChangesAsync() > 0;
                        if (result) return Unit.Value;
                    }
                    else throw new RestException(HttpStatusCode.Unauthorized,new {error="You can't cancel this order"});

                }
                throw new RestException(HttpStatusCode.BadRequest, new { error = "Payment has been done can't cancel now" });


            }
        }
    }
}