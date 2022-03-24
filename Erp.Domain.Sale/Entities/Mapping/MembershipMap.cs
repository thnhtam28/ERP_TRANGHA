using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class MembershipMap : EntityTypeConfiguration<Membership>
    {
        public MembershipMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);
                        this.Property(t => t.Status).HasMaxLength(50);
            this.Property(t => t.TargetModule).HasMaxLength(100);
            this.Property(t => t.Code).HasMaxLength(100);
            this.Property(t => t.TargetCode).HasMaxLength(100);


            // Table & Column Mappings
            this.ToTable("Sale_Membership");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");
            //this.Property(t => t.Name).HasColumnName("Name");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.TargetId).HasColumnName("TargetId");
            this.Property(t => t.TargetModule).HasColumnName("TargetModule");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.TargetCode).HasColumnName("TargetCode");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.ExpiryDateOld).HasColumnName("ExpiryDateOld");
            this.Property(t => t.Type).HasColumnName("Type");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.Is_extend).HasColumnName("Is_extend");
            this.Property(t => t.IdParent).HasColumnName("IdParent");

            this.Property(t => t.solandadung).HasColumnName("solandadung");
            this.Property(t => t.solantratra).HasColumnName("solantratra");
            this.Property(t => t.TongLanCSD).HasColumnName("TongLanCSD");
        }
    }
}
