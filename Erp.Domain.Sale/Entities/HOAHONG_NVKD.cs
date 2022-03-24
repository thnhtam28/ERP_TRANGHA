using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities
{
   public class HOAHONG_NVKD
    {
        public HOAHONG_NVKD() { }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
       
        public int STT { get; set; }
        public string TYLE_TARGET { get; set; }

        public decimal? MIN_TARGET { get; set; }
        public decimal? MAX_TARGET { get; set; }
        public decimal? TYLE_HOAHONG { get; set; }
    }
}
