using Google.Apis.Auth.OAuth2;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using Google.Apis.Services;
using Google.Apis.Util.Store;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleApp1.Business;
using ConsoleApp1.Model;

namespace ConsoleApp1
{
    class Program
    {
        // If modifying these scopes, delete your previously saved credentials
        // at ~/.credentials/gmail-dotnet-quickstart.json
        static string[] Scopes = { GmailService.Scope.GmailReadonly };
        static string ApplicationName = "Gmail API .NET Quickstart";

        static void Main(string[] args)
        {

            UserCredential credential;
            ExpenseCalculator calculator = new ExpenseCalculator();

            using (var stream =
              new FileStream("client_secret.json", FileMode.Open, FileAccess.Read))
            {
                string credPath = System.Environment.GetFolderPath(
                  System.Environment.SpecialFolder.Personal);
                credPath = Path.Combine(credPath, ".credentials/gmail-dotnet-quickstart.json");

                credential = GoogleWebAuthorizationBroker.AuthorizeAsync(
                  GoogleClientSecrets.Load(stream).Secrets,
                  Scopes,
                  "user",
                  CancellationToken.None,
                  new FileDataStore(credPath, true)).Result;
                Console.WriteLine("Credential file saved to: " + credPath);
            }

            // Create Gmail API service.
            var service = new GmailService(new BaseClientService.Initializer()
            {
                HttpClientInitializer = credential,
                ApplicationName = ApplicationName,
            });


            var inboxlistRequest = service.Users.Messages.List("me");
            inboxlistRequest.LabelIds = "INBOX";
            inboxlistRequest.IncludeSpamTrash = false;
            inboxlistRequest.Q = "after:"+DateTime.Now.Year+"/" + DateTime.Now.Month + "/01" ;
            //get our emails
            var emailListResponse = inboxlistRequest.Execute();

            if (emailListResponse != null && emailListResponse.Messages != null)
            {
                //loop through each email and get what fields you want...
                IEnumerable<MailEntity> lstentity = calculator.GetMailDetails(emailListResponse, service);

            }
            Console.ReadLine();

        }

    }
}
