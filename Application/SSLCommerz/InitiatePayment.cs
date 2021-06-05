using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Net;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using Application.Interfaces;
using Domain.DTOs;
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
            public string Amount { get; set; }
            public string NumberOfItems { get; set; }
            public string TransactionId { get; set; }
        }
        public class CommandValidator : AbstractValidator<Command>
        {
            public CommandValidator()
            {
                RuleFor(x => x.Amount).NotEmpty();
                 RuleFor(x => x.NumberOfItems).NotEmpty();
                
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
  
                var postData = InitializeParams(user,request.Amount,request.NumberOfItems,request.TransactionId);
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

            private NameValueCollection InitializeParams(AppUser user,string total,string numberOfItems,string transactionId)
            {

                NameValueCollection PostData = new NameValueCollection();
                PostData.Add("store_id", "homel60b93200bec30");
                PostData.Add("store_passwd", "homel60b93200bec30@ssl");
                PostData.Add("total_amount",total);
                PostData.Add("currency", "BDT");
                PostData.Add("tran_id",transactionId);
                PostData.Add("product_category", "Real Estate");
                PostData.Add("success_url", "http://betahomeland.aveneur.com//#/cart");
                PostData.Add("fail_url", "http://betahomeland.aveneur.com//#/failedPayment"); // "Fail.aspx" page needs to be created
                PostData.Add("cancel_url", "http://betahomeland.aveneur.com//#/cart"); // "Cancel.aspx" page needs to be created
                PostData.Add("version", "3.00");
                PostData.Add("cus_name", user.FirstName+" "+user.LastName);
                PostData.Add("cus_email", "ragibibnehossain@mail.com");
                PostData.Add("cus_add1", user.Address ?? "Not declared");
                PostData.Add("cus_city", "City Name");
                PostData.Add("cus_postcode", "Post Code");
                PostData.Add("cus_country", "Bangladesh");
                PostData.Add("cus_phone", user.PhoneNumber);
                PostData.Add("shipping_method", "NO");
                PostData.Add("value_a", "ref00");
                PostData.Add("value_b", "ref00");
                PostData.Add("value_c", "ref00");
                PostData.Add("value_d", "ref00");
                PostData.Add("num_of_item", numberOfItems);
                PostData.Add("product_name", "Unit");
                PostData.Add("product_profile", "general");
                PostData.Add("product_category", "Real Estate");
                return PostData;
            }
        }
    }
}