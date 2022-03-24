using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities
{
    public class CALLog
    {
        public CALLog()
        {

        }
        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        //public Nullable<int> AssignedUserId { get; set; }

        public int stt { get; set; }
        public string keylog { get; set; }
        public string ngaygoi { get; set; }
        public string sogoidien { get; set; }
        public string dauso { get; set; }
        public string sonhan { get; set; }
        public string trangthai { get; set; }
        public string tongthoigiangoi { get; set; }
        public string thoigianthucgoi { get; set; }
        public string linkfile { get; set; }
        public string CallDate { get; set; }


        
    }

}
