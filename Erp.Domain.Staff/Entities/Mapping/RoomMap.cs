using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities.Mapping
{
    public class RoomMap : EntityTypeConfiguration<Room>
    {
        public RoomMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name).HasMaxLength(150);
                        this.Property(t => t.Code).HasMaxLength(50);
            this.Property(t => t.Floor).HasMaxLength(10);


            // Table & Column Mappings
            this.ToTable("Staff_Room");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.Floor).HasColumnName("Floor");

        }
    }
}
