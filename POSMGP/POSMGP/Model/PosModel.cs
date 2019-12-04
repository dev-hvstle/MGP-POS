using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSMGP.Model
{
    class PosModel
    {
        public int productID { get; set; }
        public String productName { get; set; }
        public Double productPrice { get; set; }
        public int productQuantity { get; set; }
        public Double productSubtotal { get; set; }
        public int userID { get; set; }
        public DateTime dateCreated { get; set; }


    }
}
