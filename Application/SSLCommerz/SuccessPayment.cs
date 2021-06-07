using System;
using System.Collections.Generic;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Errors;
using Domain.UnitBooking;
using MediatR;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.SSLCommerz
{
    public class SuccessPayment
    {
        public class Command : IRequest
        {
            public string status { get; set; }
            public DateTime Password { get; set; }
            public string tran_id { get; set; }
            public string val_id { get; set; }
            public decimal amount { get; set; }
            public decimal store_amount { get; set; }
            public string card_type { get; set; }
            public string card_no { get; set; }
            public string currency { get; set; }
            public string bank_tran_id { get; set; }
            public string card_issuer { get; set; }
            public string card_issuer_country { get; set; }
            public string card_issuer_country_code { get; set; }
            public string currency_type { get; set; }
            public string currency_amount { get; set; }
            public string value_a { get; set; }
            public string value_b { get; set; }
            public string value_c { get; set; }
            public string value_d { get; set; }
            public string verify_sign { get; set; }
            public string verify_key { get; set; }
            public string risk_level { get; set; }
            public string risk_title { get; set; }

        }


        public class Handler : IRequestHandler<Command>
        {
            private readonly IHttpContextAccessor _httpContextAccessor;

            private readonly HomelandContext _context;
            public Handler(HomelandContext context, IHttpContextAccessor httpContextAccessor)
            {
                _httpContextAccessor = httpContextAccessor;
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                if (request.status == "VALID")
                {
                    var order = await _context.Orders.FindAsync(request.value_a);
                    if (order != null)
                    {
                        order.PaymentConfirmed = true;
                        var bookings = await BookFlat(request);
                        await _context.Bookings.AddRangeAsync(bookings);
                        var result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            return Unit.Value;
                        }
                    }

                }

                return Unit.Value;

            }

            private async Task<List<Booking>> BookFlat(Command request)
            {
                var user = await _context.Users.FirstOrDefaultAsync(x => x.PhoneNumber == request.value_b);
                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { error = "You don't have any account here with this number . sorry" });
                var order = await _context.Orders.
                Include(x => x.OrderDetails).
                  ThenInclude(x => x.Flat).
                FirstOrDefaultAsync(x => x.Id == request.value_a);
                var bookings = new List<Booking>();
                foreach (var orderDetails in order.OrderDetails)
                {
                    var flat = await _context.Flats.FindAsync(orderDetails.FlatId);

                    if (flat == null) continue;
                    flat.IsBooked = true;
                    Booking booking = new Booking
                    {
                        User = user,
                        Flat = flat,
                        DateBooked = DateTime.Now
                    };
                    bookings.Add(booking);
                }
                return bookings;
            }
        }
    }
}