using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class HOAHONG_NVKDMap : EntityTypeConfiguration<HOAHONG_NVKD>
    {
        public HOAHONG_NVKDMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("Sale_HOAHONG_NVKD");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
           // this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.STT).HasColumnName("STT");
            this.Property(t => t.TYLE_TARGET).HasColumnName("TYLE_TARGET");

            this.Property(t => t.MIN_TARGET).HasColumnName("MIN_TARGET");
            this.Property(t => t.MAX_TARGET).HasColumnName("MAX_TARGET");
            this.Property(t => t.TYLE_HOAHONG).HasColumnName("TYLE_HOAHONG");
        }

    }
}
