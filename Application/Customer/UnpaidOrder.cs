using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using AutoMapper;
using Domain.DTOs;
using Domain.Errors;
using Domain.UnitBooking;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.Customer
{
    public class UnpaidOrder
    {
        public class Query : IRequest<OrderDetailsDTO> { }
        public class Handler : IRequestHandler<Query, OrderDetailsDTO>
        {
            private readonly HomelandContext _context;
            private readonly IMapper _mapper;
            private readonly IUserAccessor _userAccessor;

            public Handler(HomelandContext context, IMapper mapper, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _mapper = mapper;
                _context = context;
            }

            public async Task<OrderDetailsDTO> Handle(Query request, CancellationToken cancellationToken)
            {
               
                    var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                    
                    var order = await _context.Orders
                    .Include(x => x.User)
                    .Include(x => x.OrderDetails)
                      .ThenInclude(x => x.Flat)
                    .Where(x => !x.PaymentConfirmed)
                    .FirstOrDefaultAsync(x => x.UserId == user.Id);
                    
                    if(order == null) throw new RestException(HttpStatusCode.OK,new {error = "No order"});

                    var orderDetails = new OrderDetailsDTO {
                      Flats = new List<Flat>{},
                      Amount = order.Amount,
                      TotalUnits = order.OrderDetails.Count,
                      OrderId = order.Id
                    };

                    foreach(var orderDetails1 in order.OrderDetails) 
                    {
                        orderDetails.Flats.Add(orderDetails1.Flat);
                    }
                    return orderDetails;               
            }
        }
    }
}