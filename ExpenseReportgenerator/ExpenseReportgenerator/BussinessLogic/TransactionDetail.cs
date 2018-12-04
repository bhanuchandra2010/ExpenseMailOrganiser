using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ExpenseReportgenerator.BussinessLogic
{
    class TransactionDetail
    {
        public TType Transactiontype { get; set; }
        public float Amount { get; set; }
        public string BodyName { get; set; }


    }

    enum TType
    {
        Debit,
        Credit,
    }
}
