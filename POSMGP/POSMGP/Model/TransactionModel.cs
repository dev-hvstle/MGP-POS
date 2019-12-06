using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSMGP.Model
{
    class TransactionModel
    {
        public int transactionID { get; set; }
        public String customerName { get; set; }
        public int userID { get; set; }
        public int totalItems { get; set; }
        public Double totalAmount { get; set; }
        public Double paymentAmount { get; set; }
        public Double paymentChange { get; set; }
        public String transactionTime { get; set; }
        public String transactionDate { get; set; }
        public int isPriority { get; set; }
    }
}
