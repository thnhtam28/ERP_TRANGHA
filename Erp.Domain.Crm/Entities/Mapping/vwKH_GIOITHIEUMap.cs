using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    class vwKH_GIOITHIEUMap : EntityTypeConfiguration<vwKH_GIOITHIEU>
    {

        public vwKH_GIOITHIEUMap()
        {
            // Primary Key
            this.HasKey(t => t.KH_GIOITHIEU_ID);

            // Properties
            this.Property(t => t.CustomerCode).HasMaxLength(20);
            this.Property(t => t.CustomerName).HasMaxLength(150);

            // Table & Column Mappings
            this.ToTable("vwCrm_KH_GIOITHIEU");
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
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.ModifiedUserName).HasColumnName("ModifiedUserName");
            this.Property(t => t.UserId).HasColumnName("UserId");
            this.Property(t => t.ManagerStaffId).HasColumnName("ManagerStaffId");
            this.Property(t => t.ManagerUserName).HasColumnName("ManagerUserName");
        }
    }
}
