using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities.Mapping
{
    class BedMap : EntityTypeConfiguration<Bed>
    {
        public BedMap(){
            // Primary Key
            this.HasKey(t => t.Bed_ID);

            // Properties

   
            // Table & Column Mappings
            this.ToTable("Staff_Bed");
            this.Property(t => t.Bed_ID).HasColumnName("Bed_ID");
            this.Property(t => t.GHI_CHU).HasColumnName("GHI_CHU");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.Name_Bed).HasColumnName("Name_Bed");
            this.Property(t => t.Room_Id).HasColumnName("Room_Id");
            this.Property(t => t.Trang_Thai).HasColumnName("Trang_Thai");
        
        }
    }
}
