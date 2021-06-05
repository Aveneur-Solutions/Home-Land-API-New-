using System;
using System.Net;
using System.Threading;
using System.Threading.Tasks;
using Domain.Errors;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;

namespace Application.SSLCommerz
{
    public class IPNListener
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

            private readonly HomelandContext _context;
            public Handler(HomelandContext context)
            {
                _context = context;
            }

            public async Task<Unit> Handle(Command request, CancellationToken cancellationToken)
            {

                if (request.status == "VALID")
                {
                    var order = await _context.Orders.FirstOrDefaultAsync(x => x.TrasnsactionId == request.tran_id);

                    if (order != null)
                    {
                        order.PaymentConfirmed = true;
                    }
                    var result = await _context.SaveChangesAsync() > 0;
                    if(result) return Unit.Value;
                }


                throw new RestException(HttpStatusCode.Unauthorized, new { error = "Failed Transaction" });
            }
        }
    }
}