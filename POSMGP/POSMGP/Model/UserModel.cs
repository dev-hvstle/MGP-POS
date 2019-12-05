using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace POSMGP.Model
{
    class UserModel
    {
        public int userID { get; set; }
        public String userFName { get; set; }
        public String userMName { get; set; }
        public String userLName { get; set; }
        public String userName { get; set; }
        public String userPass { get; set; }
        public String userType { get; set; }
        public String dateModified { get; set; }
        public String timeModified { get; set; }
        public int isPriority { get; set; }
    }
}
