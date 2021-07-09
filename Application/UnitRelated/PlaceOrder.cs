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
            public decimal Amount{ get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.flatIds).NotEmpty();
            }
        }

        public class Handler : IRequestHandler<Command, OrderConfirmedDto>
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
                var order = await _context.Orders.FirstOrDefaultAsync(x=> x.Id == request.OrderId);
                if(order != null) throw new RestException(HttpStatusCode.Conflict, new { error = "Duplicate Order Id found. Either you've already ordered or some technical problem" });                var transactionId = "Trx" + DateTime.Now.ToString("ddmmyyhhmmss");
                var newOrder = new Order
                {
                    Id = request.OrderId,
                    OrderDate = DateTime.Now,
                    User = user,
                    TransactionId = transactionId,
                    Amount = request.Amount
                };
                //check if flats exist
                var outcome = checkIfAlreadyOrdered(request.flatIds);
                var orderedFlatIds = outcome.Result;
                if (!String.IsNullOrEmpty(orderedFlatIds)) throw new RestException(HttpStatusCode.Conflict, new { error = "Oops!!Someone has already placed an order for " + orderedFlatIds });

                var orderDetailsList = createOrderDetails(newOrder, request.flatIds);
                await _context.Orders.AddAsync(newOrder);
                await _context.OrderDetails.AddRangeAsync(orderDetailsList);
                var result = await _context.SaveChangesAsync() > 0;

                if (result) return new OrderConfirmedDto
                {
                    TransactionID = transactionId
                };
                throw new RestException(HttpStatusCode.ExpectationFailed, new { error = "Couldn't place the order" });
            }

            private async Task<string> checkIfAlreadyOrdered(string[] flatIds)
            {
                string orderedIds = "";
                foreach (var flatId in flatIds)
                {
                    var flat = await _context.OrderDetails.FirstOrDefaultAsync(x => x.FlatId == flatId);

                    if (flat != null)
                    {
                        orderedIds =  orderedIds + (flatId + ",");
                    }

                }
                return orderedIds;
            }
            private List<OrderDetails> createOrderDetails(Order order, string[] flatIds)
            {
                List<OrderDetails> orderDetailsList = new List<OrderDetails> { };

                foreach (var flatId in flatIds)
                {
                    var orderDetails = new OrderDetails
                    {
                        FlatId = flatId,
                        Order = order
                    };
                    orderDetailsList.Add(orderDetails);
                }

                return orderDetailsList;

            }
        }
    }
}