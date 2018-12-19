using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp1.Model
{
    class MailEntity
    {
        public string From { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string TransactionType { get; set; }
        public string EntityName { get; set; }
        public DateTime DOT { get; set; }
    }
}
