using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities
{
    public class Bed
    {
        public Bed()
        {

        }
        public int Bed_ID { get; set; }
        public int Room_Id { get; set; }
        public bool IsDeleted { get; set; }
        public string Name_Bed { get; set; }
        public bool Trang_Thai { get; set; }
        public string GHI_CHU { get; set; }
   
    }

}
