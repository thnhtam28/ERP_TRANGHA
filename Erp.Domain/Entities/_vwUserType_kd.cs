using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Entities
{
    public partial class vwUserType_kd
    {
        public vwUserType_kd()
        {
            //this.Users = new List<User>();
            //this.UserTypePages = new List<UserTypePage>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public Nullable<int> OrderNo { get; set; }
        public string Note { get; set; }
        //public virtual ICollection<User> Users { get; set; }
        //public virtual ICollection<UserTypePage> UserTypePages { get; set; }
        public string Code { get; set; }
        public Nullable<bool> Scope { get; set; }
        public Nullable<bool> IsSystem { get; set; }
        public Nullable<int> ChiefUserID { get; set; }
        public string FullName { get; set; }
        public int BranchId { get; set; }
        public string idcu { get; set; }


    }
}