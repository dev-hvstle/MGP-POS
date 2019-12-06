using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSMGP.Model
{
    class ProductsModel
    {
        public int productID { get; set; }
        public String productName { get; set; }
        public Double productPrice { get; set; }
        public int supplierID { get; set; }
        public int modifiedBy { get; set; }
        public String dateModified { get; set; }
        public String timeModified { get; set; }
        public int isPriority { get; set; }
    }
}
