using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace Application.SMSService
{
    public static class AuthMessageSender
    {
                public static Task SendEmailAsync(string email, string subject, string message)
        {
            // Plug in your email service here to send an email.
            return Task.FromResult(0);
        }

        public static Task SendSmsAsync(string number, string message, IConfiguration configuration)
        {
            // Plug in your SMS service here to send a text message.
            // Your Account SID from twilio.com/console
            // configuration["SMSAccountIdentification"];
            var accountSid = "ACb3d16d42b137c5c7d8ccf5c7e83ac0d5";
            // Your Auth Token from twilio.com/console
            // var authToken = configuration["SMSAccountPassword"];
            var authToken = "47274eacbc054cda27e02f551fb85b4f";
            // var fromNumber = configuration["SMSAccountFrom"];
            var fromNumber = "+12054311218";

            TwilioClient.Init(accountSid, authToken);

            return MessageResource.CreateAsync(
              to: new PhoneNumber(number),
              from: new PhoneNumber(fromNumber),
              body: message);
        }
    }
}