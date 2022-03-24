using Erp.Domain.Crm.Entities;
using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Entities.Mapping
{
     public class vwCRM_TK_TUONGTACMap :  EntityTypeConfiguration<vwCRM_TK_TUONGTAC>
    {
        public vwCRM_TK_TUONGTACMap()
        {
            this.HasKey(t => t.Id);

            this.ToTable("vwCRM_TK_TUONGTAC");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.NGAYLAP).HasColumnName("NGAYLAP");
            this.Property(t => t.NGUOILAP_ID).HasColumnName("NGUOILAP_ID");
            this.Property(t => t.NGUOI_LAP).HasColumnName("NGUOI_LAP");
            this.Property(t => t.TONG_PLAN).HasColumnName("TONG_PLAN");
            this.Property(t => t.TONG_QL).HasColumnName("TONG_QL");
            this.Property(t => t.SOTUONGTAC).HasColumnName("SOTUONGTAC");
            this.Property(t => t.SO_QUAHAN).HasColumnName("SO_QUAHAN");
            this.Property(t => t.CHUA_PLAN).HasColumnName("CHUA_PLAN");
            this.Property(t => t.CHUA_PLAN_NEXT).HasColumnName("CHUA_PLAN_NEXT");

        }
    }
    
}
