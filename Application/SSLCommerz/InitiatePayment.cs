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
  
                var postData = InitializeParams(user,request.Amount);
                byte[] response = null;
                using (WebClient client = new WebClient())
                {
                    response = client.UploadValues("https://sandbox.sslcommerz.com/gwprocess/v4/api.php", postData);
                }
                var resp = System.Text.Encoding.UTF8.GetString(response);
                var jsonResp = JsonSerializer.Deserialize<SSLCommerzInitResponse>(resp);

                return new PaymentResponseDTO
                {
                    Status = jsonResp.status,
                    GatewayPageURL = jsonResp.GatewayPageURL
                };

            }

            private NameValueCollection InitializeParams(AppUser user,string total)
            {

                var transactionId = "Trx"+DateTime.Now.ToString("ddmmyyhhmmss");
                NameValueCollection PostData = new NameValueCollection();
                PostData.Add("store_id", "homel60b93200bec30");
                PostData.Add("store_passwd", "homel60b93200bec30@ssl");
                PostData.Add("currency", "BDT");
                PostData.Add("tran_id", transactionId);
                PostData.Add("product_category", "Real Estate");
                PostData.Add("success_url", "http://betahomeland.aveneur.com/#/cart");
                PostData.Add("fail_url", "https://betahomeland.aveneur.com/#/my-bookings"); // "Fail.aspx" page needs to be created
                PostData.Add("cancel_url", "https://betahomeland.aveneur.com/#/my-bookings"); // "Cancel.aspx" page needs to be created
                PostData.Add("version", "3.00");
                PostData.Add("cus_name", user.FirstName+" "+user.LastName);
                PostData.Add("cus_email", "abc.xyz@mail.co");
                PostData.Add("cus_add1", user.Address);
                PostData.Add("cus_city", "City Nam");
                PostData.Add("cus_postcode", "Post Cod");
                PostData.Add("cus_country", "Bangladesh");
                PostData.Add("cus_phone", user.PhoneNumber);
                PostData.Add("shipping_method", "NO");
                PostData.Add("value_a", "ref00");
                PostData.Add("value_b", "ref00");
                PostData.Add("value_c", "ref00");
                PostData.Add("value_d", "ref00");
                PostData.Add("num_of_item", "1");
                PostData.Add("product_name", "Unit");
                PostData.Add("product_profile", "general");
                PostData.Add("product_category", "Real Estate");
                return PostData;
            }
        }
    }
}