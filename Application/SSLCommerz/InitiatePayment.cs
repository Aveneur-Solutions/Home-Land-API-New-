using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.DTOs;
using Domain.Errors;
using Domain.UnitBooking;
using Domain.UserAuth;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Persistence;
using static Domain.Common.SSLCommerz;

namespace Application.SSLCommerz
{
    public class InitiatePayment
    {

        public class Command : IRequest<PaymentResponseDTO>
        {
            public string OrderId { get; set; }
            public string SuccessUrl { get; set; }
            public string FailedUrl { get; set; }

        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.OrderId).NotEmpty();
                RuleFor(x => x.SuccessUrl).NotEmpty();
                RuleFor(x => x.FailedUrl).NotEmpty();

            }
        }

        public class Handler : IRequestHandler<Command, PaymentResponseDTO>
        {
            private readonly HomelandContext _context;
            private readonly IUserAccessor _userAccessor;
            public Handler(HomelandContext context, IUserAccessor userAccessor)
            {
                _userAccessor = userAccessor;
                _context = context;
            }

            public async Task<PaymentResponseDTO> Handle(Command request, CancellationToken cancellationToken)
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(x => x.PhoneNumber == _userAccessor.GetUserPhoneNo());
                if (user == null) throw new RestException(HttpStatusCode.NotFound, new { error = "Maybe your are not logged in" });

                var order = await _context.Orders.Include(x => x.OrderDetails).FirstOrDefaultAsync(x => x.Id == request.OrderId);

                if (order == null) throw new RestException(HttpStatusCode.NotFound, new { error = "Order doesn't exist" });
                if (!order.PaymentConfirmed)
                {
                    var postData = InitializeParams(user, request, order);
                    byte[] response = null;
                    using (WebClient client = new WebClient())
                    {
                        response = client.UploadValues("https://sandbox.sslcommerz.com/gwprocess/v4/api.php", postData);
                    }
                    var resp = System.Text.Encoding.UTF8.GetString(response);
                    var jsonResp = JsonSerializer.Deserialize<SSLCommerzInitResponse>(resp);
                    // return jsonResp;
                    return new PaymentResponseDTO
                    {
                        Status = jsonResp.status,
                        GatewayPageURL = jsonResp.GatewayPageURL,
                        FailedReason = jsonResp.failedreason
                    };
                }
                else throw new RestException(HttpStatusCode.BadRequest,new {error="This order has already been paid"});
            }

            private NameValueCollection InitializeParams(AppUser user, Command request, Order order)
            {

                NameValueCollection PostData = new NameValueCollection();
                PostData.Add("store_id", "homel60b93200bec30");
                PostData.Add("store_passwd", "homel60b93200bec30@ssl");
                PostData.Add("total_amount", order.Amount.ToString());
                PostData.Add("currency", "BDT");
                PostData.Add("tran_id", order.TransactionId);
                PostData.Add("product_category", "Real Estate");
                PostData.Add("success_url", "https://homeland.aveneur.com/api/Payment/success");
                PostData.Add("fail_url", "https://homeland.aveneur.com/api/Payment/failed"); 
                PostData.Add("cancel_url", "http://betahomeland.aveneur.com//#/cancelled"); 
                PostData.Add("version", "3.00");
                PostData.Add("cus_name", user.FirstName + " " + user.LastName);
                PostData.Add("cus_email", "ragibibnehossain@mail.com");
                PostData.Add("cus_add1", user.Address ?? "Not declared");
                PostData.Add("cus_city", "City Name");
                PostData.Add("cus_postcode", "Post Code");
                PostData.Add("cus_country", "Bangladesh");
                PostData.Add("cus_phone", user.PhoneNumber);
                PostData.Add("shipping_method", "NO");
                PostData.Add("value_a", request.OrderId);
                PostData.Add("value_b", user.PhoneNumber);
                PostData.Add("value_c", request.SuccessUrl);
                PostData.Add("value_d", request.FailedUrl);
                PostData.Add("num_of_item", order.OrderDetails.Count.ToString());
                PostData.Add("product_name", "Unit");
                PostData.Add("product_profile", "general");
                PostData.Add("product_category", "Real Estate");
                return PostData;
            }
        }
    }
}