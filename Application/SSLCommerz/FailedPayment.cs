using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Http;
using Persistence;

namespace Application.SSLCommerz
{
    public class FailedPayment
    {
           public class Command : IRequest
        {
            public string status { get; set; }
            public DateTime tran_date { get; set; }
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

                if (request.status == "FAILED" || request.status == "CANCELLED" || request.status == "EXPIRED")
                {
                    var order = await _context.Orders.FindAsync(request.value_a);
                    if (order != null)
                    {
                        _context.Orders.Remove(order);
                        var result = await _context.SaveChangesAsync() > 0;
                        if (result)
                        {
                            return Unit.Value;
                        }
                    }
                }

                return Unit.Value;

            }

          
        }
    }
}