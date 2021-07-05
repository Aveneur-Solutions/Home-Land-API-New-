using System;
using System.IO;
using System.Net;
using System.Text;
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

        public static void SendSms(string number, string message, IConfiguration configuration)
        {
                
                String userid = "Ragib007"; //Your Login ID
                String password = "75PBMHD8"; //Your Password
                //Recipient Phone Number multiple number must be separated by comma
                String messageToBeSent = System.Uri.EscapeUriString(message);
                // Create a request using a URL that can receive a post.   
                WebRequest request = WebRequest.Create("http://66.45.237.70/api.php");
                // Set the Method property of the request to POST.  
                request.Method = "POST";
                // Create POST data and convert it to a byte array.  
                string postData = "username=" + userid + "&password=" + password + "&number=" + number + "&message=" + message;
                byte[] byteArray = Encoding.UTF8.GetBytes(postData);
                // Set the ContentType property of the WebRequest.  
                request.ContentType = "application/x-www-form-urlencoded";
                // Set the ContentLength property of the WebRequest.  
                request.ContentLength = byteArray.Length;
                // Get the request stream.  
                Stream dataStream = request.GetRequestStream();
                // Write the data to the request stream.  
                dataStream.Write(byteArray, 0, byteArray.Length);
                // Close the Stream object.  
                dataStream.Close();
                // Get the response.  
                WebResponse response = request.GetResponse();
                // Display the status.  
                Console.WriteLine(((HttpWebResponse)response).StatusDescription);
                // Get the stream containing content returned by the server.  
                dataStream = response.GetResponseStream();
                // Open the stream using a StreamReader for easy access.  
                StreamReader reader = new StreamReader(dataStream);
                // Read the content.  
                string responseFromServer = reader.ReadToEnd();
                // Display the content.  
                Console.WriteLine(responseFromServer);
                // Clean up the streams.  
                reader.Close();
                dataStream.Close();
                response.Close();
        }
    }
}