using ConsoleApp1.Model;
using Google.Apis.Gmail.v1;
using Google.Apis.Gmail.v1.Data;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Business
{
    class ExpenseCalculator
    {
        MailEntity entity;
        List<MailEntity> lstentity = new List<MailEntity>();
        public IEnumerable<MailEntity> GetMailDetails(ListMessagesResponse listMessages, GmailService service)
        {
            foreach (var email in listMessages.Messages)
            {
                entity = new MailEntity();
                var emailInfoRequest = service.Users.Messages.Get("me", email.Id);
                var emailInfoResponse = emailInfoRequest.Execute();

                if (emailInfoResponse != null)
                {
                    //loop through the headers to get from,date,subject, body 
                    foreach (var mParts in emailInfoResponse.Payload.Headers)
                    {
                        if (mParts.Name == "Date")
                        {
                            //int index = mParts.Value.LastIndexOf(':') + 3;

                            //string datepulled = mParts.Value.Remove(index, mParts.Value.Length - index);
                            //DateTime dt;
                            //while (!DateTime.TryParse(datepulled, out dt))
                            //{
                            //    datepulled = datepulled.Remove(0, 1);
                            //}
                            //entity.DOT = Convert.ToDateTime(datepulled);

                            ////datepulled = datepulled.Remove(index, datepulled.Length - index).Trim();
                            //entity.DOT = Convert.ToDateTime(datepulled);

                            //int index = mParts.Value.IndexOf('+');

                            //string date = mParts.Value.Remove(index, mParts.Value.Length - index).Trim();

                            entity.DOT = Convert.ToDateTime(mParts.Value.Substring(0,16).Trim());



                           
                        }
                        else if (mParts.Name == "Time")
                        {
                            entity.From = mParts.Value;
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
                            if (emailInfoResponse.Payload.Parts != null)
                            {
                                foreach (var item in emailInfoResponse.Payload.Parts)
                                {
                                    if (item.MimeType == "text/html")
                                    {
                                        byte[] data = FromBase64ForUrlString(item.Body.Data);
                                        entity.Body = Encoding.UTF8.GetString(data);
                                    }
                                }
                            }
                            else
                            {
                                byte[] data = FromBase64ForUrlString(emailInfoResponse.Payload.Body.Data);
                                entity.Body = Encoding.UTF8.GetString(data);
                            }
                        }
                    }
                }
                lstentity.Add(entity);
            }
            GetTransactionDetails(lstentity);
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

        public void GetTransactionDetails(List<MailEntity> Mails)
        {
            try
            {
                string[] sub = { "info@icicibank.com", "alert@icicibank.com", "citialert.india@citicorp.com" };

                

                var v= Mails.Where(p => p.From.ToLower().Contains("citialert.india@citicorp.com")).ToList();

                //var v = lstentity.Distinct().Where(i => sub.Any(s => i.From.ToLower().Contains(s.ToLower()))).ToList();
            }
            catch (CustomException ex)
            {
                throw;
            }
        }
    }
}
