using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSMGP.Model
{
    class SupplierModel
    {
        public int supplierID { get; set; }
        public String supplierName { get; set; }
        public String supplierLocation { get; set; }
        public int modifiedBy { get; set; }
        public String dateModified { get; set; }
        public String timeModified { get; set; }
        public int isPriority { get; set; }
    }
}
