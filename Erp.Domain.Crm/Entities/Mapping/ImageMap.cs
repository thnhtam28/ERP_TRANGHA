using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    public class ImageMap : EntityTypeConfiguration<Image>
    {
        public ImageMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);
            this.Property(t => t.Images).HasMaxLength(250);
            this.Property(t => t.Description).HasMaxLength(500);


            // Table & Column Mappings
            this.ToTable("Crm_Image");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.Images).HasColumnName("Images");
            this.Property(t => t.Description).HasColumnName("Description");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.TargetModule).HasColumnName("TargetModule");

        }
    }
}
