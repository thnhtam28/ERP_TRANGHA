using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    class CRM_KH_GIOITHIEUMap : EntityTypeConfiguration<CRM_KH_GIOITHIEU>
    {
        public CRM_KH_GIOITHIEUMap()
        {
            // Primary Key
            this.HasKey(t => t.KH_GIOITHIEU_ID);

            // Properties

            // Table & Column Mappings
            this.ToTable("CRM_KH_GIOITHIEU");
            this.Property(t => t.KH_GIOITHIEU_ID).HasColumnName("KH_GIOITHIEU_ID");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.KHACHHANG_ID).HasColumnName("KHACHHANG_ID");
            this.Property(t => t.LOAI_GIOITHIEU).HasColumnName("LOAI_GIOITHIEU");
            this.Property(t => t.TRANGTHAI_GIOITHIEU).HasColumnName("TRANGTHAI_GIOITHIEU");
            this.Property(t => t.NOIDUNG).HasColumnName("NOIDUNG");

            this.Property(t => t.TYLE_THANHCONG).HasColumnName("TYLE_THANHCONG");
            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
        }
    }
}
