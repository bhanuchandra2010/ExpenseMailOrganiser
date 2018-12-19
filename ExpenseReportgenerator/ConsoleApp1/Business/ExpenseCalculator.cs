using ConsoleApp1.Model;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Business
{
    class ExpenseCalculator
    {
        MailEntity entity;
        List<MailEntity> lstentity= new List<MailEntity>();
        public IEnumerable<MailEntity> GetTransactionDetails(ListMessagesResponse listMessages,GmailService service)
        {
           
            
            foreach (var email in listMessages.Messages)
            {
                entity = new MailEntity();
                var emailInfoRequest = service.Users.Messages.Get("me", email.Id);
                var emailInfoResponse = emailInfoRequest.Execute();

                if (emailInfoResponse != null)
                {
                    String from = "";
                    String date = "";
                    String subject = "";

                    //loop through the headers to get from,date,subject, body 
                    foreach (var mParts in emailInfoResponse.Payload.Headers)
                    {
                        if (mParts.Name == "Date")
                        {
                            int index = mParts.Value.LastIndexOf(':')+3;

                            string datepulled = mParts.Value.Remove(index, mParts.Value.Length-index);
                            DateTime dt;
                            while (!DateTime.TryParse(datepulled,out dt))
                            {
                                datepulled = datepulled.Remove(0, 1);
                            }
                            entity.DOT = Convert.ToDateTime(datepulled);

                            //datepulled = datepulled.Remove(index, datepulled.Length - index).Trim();
                            entity.DOT =Convert.ToDateTime(datepulled);
                        }
                        else if (mParts.Name == "From")
                        {
                            entity.From = mParts.Value;
                        }
                        else if (mParts.Name == "Subject")
                        {
                            entity.Subject = mParts.Value;
                        }

                        if (entity.DOT != null && !string.IsNullOrWhiteSpace(entity.From) && emailInfoResponse.Payload.Body != null && !string.IsNullOrWhiteSpace(entity.Subject))
                        {

                          
                                if (emailInfoResponse.Payload.MimeType == "text/html")
                                {
                                    byte[] data = FromBase64ForUrlString(emailInfoResponse.Payload.Body.Data);
                                    entity.Body = Encoding.UTF8.GetString(data);
                                lstentity.Add(entity);
                            }
                        

                            
                        }
                    }
                }
            }
            return lstentity;
        }

        private byte[] FromBase64ForUrlString(string base64ForUrlInput)
        {
            int padChars = (base64ForUrlInput.Length % 4) == 0 ? 0 : (4 - (base64ForUrlInput.Length % 4));
            StringBuilder result = new StringBuilder(base64ForUrlInput, base64ForUrlInput.Length + padChars);
            result.Append(String.Empty.PadRight(padChars, '='));
            result.Replace('-', '+');
            result.Replace('_', '/');
            return Convert.FromBase64String(result.ToString());
        }
    }
}
