using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Entities.Mapping
{
    public class CALLogMap : EntityTypeConfiguration<CALLog>
    {
        public CALLogMap()
        {

            // Primary Key
            this.HasKey(t => t.Id);

            // Properties

            // Table & Column Mappings
            this.ToTable("Crm_CALLog");
            this.Property(t => t.Id).HasColumnName("Id");

            this.Property(t => t.IsDeleted).HasColumnName("IsDeleted");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
           // this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.stt).HasColumnName("stt");
            this.Property(t => t.keylog).HasColumnName("keylog");
            this.Property(t => t.sogoidien).HasColumnName("sogoidien");
            this.Property(t => t.sonhan).HasColumnName("sonhan");
            this.Property(t => t.ngaygoi).HasColumnName("ngaygoi");

            this.Property(t => t.trangthai).HasColumnName("trangthai");
            this.Property(t => t.thoigianthucgoi).HasColumnName("thoigianthucgoi");
            this.Property(t => t.tongthoigiangoi).HasColumnName("tongthoigiangoi");

            this.Property(t => t.linkfile).HasColumnName("linkfile");
            this.Property(t => t.CallDate).HasColumnName("CallDate");
            this.Property(t => t.dauso).HasColumnName("dauso");

           
        }
    }
}
