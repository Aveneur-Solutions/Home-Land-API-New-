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
            var accountSid = "AC78bac3ee9dc04e78505c4b98ba63f839";
            // Your Auth Token from twilio.com/console
            // var authToken = configuration["SMSAccountPassword"];
            var authToken = "c80056b3aeb5ae2de65717729e32c5ab";
            // var fromNumber = configuration["SMSAccountFrom"];
            var fromNumber = "+13479835360";

            TwilioClient.Init(accountSid, authToken);

            return MessageResource.CreateAsync(
              to: new PhoneNumber(number),
              from: new PhoneNumber(fromNumber),
              body: message);
        }
    }
}