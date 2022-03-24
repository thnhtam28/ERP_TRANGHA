using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity.ModelConfiguration;

namespace Erp.Domain.Entities.Mapping
{
    public class vwUserType_kdMap : EntityTypeConfiguration<vwUserType_kd>
    {
        public vwUserType_kdMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.Name)
                .HasMaxLength(50);

            this.Property(t => t.Note)
                .HasMaxLength(500);

            // Table & Column Mappings
            this.ToTable("vwSystem_UserType_kd");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.Name).HasColumnName("Name");
            this.Property(t => t.OrderNo).HasColumnName("OrderNo");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.Scope).HasColumnName("Scope");
            this.Property(t => t.IsSystem).HasColumnName("IsSystem");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.ChiefUserID).HasColumnName("ChiefUserID");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.idcu).HasColumnName("idcu");

        }
    }
}
