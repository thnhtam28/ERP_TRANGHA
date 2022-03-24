using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
   public class vwCRM_BH_DOANHSO_CTMap:EntityTypeConfiguration<vwCRM_BH_DOANHSO_CT>
    {
       public vwCRM_BH_DOANHSO_CTMap()
       {
           //primary key
           this.HasKey(t => t.KH_BANHANG_DOANHSO_CTIET_ID);
           //TABLE colum % maping
           this.ToTable("vwCRM_KH_BANHANG_DOANHSO_CTIET");
           this.Property(t => t.KH_BANHANG_DOANHSO_CTIET_ID).HasColumnName("KH_BANHANG_DOANHSO_CTIET_ID");
           this.Property(t => t.BranchId).HasColumnName("BranchId");
           this.Property(t => t.TONG_TIEN).HasColumnName("TONG_TIEN");
           this.Property(t => t.NGAY_MUA).HasColumnName("NGAY_MUA");
           this.Property(t => t.MA_DONHANG).HasColumnName("MA_DONHANG");
           this.Property(t => t.CON_NO).HasColumnName("CON_NO");
           this.Property(t => t.DA_TRA).HasColumnName("DA_TRA");
           this.Property(t => t.KHACHHANG_ID).HasColumnName("KHACHHANG_ID");
           this.Property(t => t.THANG).HasColumnName("THANG");
           this.Property(t => t.NAM).HasColumnName("NAM");
           this.Property(t => t.KH_BANHANG_DOANHSO_ID).HasColumnName("KH_BANHANG_DOANHSO_ID");
           this.Property(t => t.NOIDUNG).HasColumnName("NOIDUNG");
           this.Property(t => t.TYLE_THANHCONG).HasColumnName("TYLE_THANHCONG");
           this.Property(t => t.TYLE_THANHCONG_REVIEW).HasColumnName("TYLE_THANHCONG_REVIEW");
           this.Property(t => t.GHI_CHU).HasColumnName("GHI_CHU");
           this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
           this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
           this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
           this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
           this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
           this.Property(t => t.CustomerName).HasColumnName("CustomerName");
           this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
           this.Property(t => t.BranchName).HasColumnName("BranchName");
           this.Property(t => t.Phone).HasColumnName("Phone");
            this.Property(t => t.CountForBrand).HasColumnName("CountForBrand");
            this.Property(t => t.NGUOILAP_ID).HasColumnName("NGUOILAP_ID");
       }
    }
}
