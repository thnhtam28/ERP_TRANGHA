using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Entities.Mapping
{
    public class vwUsersMap : EntityTypeConfiguration<vwUsers>
    {
        public vwUsersMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.UserName).HasMaxLength(150);
            this.Property(t => t.FirstName).HasMaxLength(50);
            this.Property(t => t.LastName).HasMaxLength(50);
            this.Property(t => t.FullName).HasMaxLength(100);
            this.Property(t => t.Address).HasMaxLength(200);
            this.Property(t => t.Mobile).HasMaxLength(200);

            // Table & Column Mappings
            this.ToTable("vwSystem_Users");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");

            this.Property(t => t.UserName).HasColumnName("UserName");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.UserCode).HasColumnName("UserCode");
            this.Property(t => t.FirstName).HasColumnName("FirstName");
            this.Property(t => t.LastName).HasColumnName("LastName");
            this.Property(t => t.FullName).HasColumnName("FullName");
            this.Property(t => t.DateOfBirth).HasColumnName("DateOfBirth");
            this.Property(t => t.Address).HasColumnName("Address");
            this.Property(t => t.Mobile).HasColumnName("Mobile");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Sex).HasColumnName("Sex");
            this.Property(t => t.UserTypeId).HasColumnName("UserTypeId");
            this.Property(t => t.LoginFailedCount).HasColumnName("LoginFailedCount");
            this.Property(t => t.LoginFailedCount2).HasColumnName("LoginFailedCount2");
            this.Property(t => t.LoginFailedCount3).HasColumnName("LoginFailedCount3");
            this.Property(t => t.LastChangedPassword).HasColumnName("LastChangedPassword");
            this.Property(t => t.UserTypeName).HasColumnName("UserTypeName");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            //this.Property(t => t.BranchCode).HasColumnName("BranchCode");
            this.Property(t => t.WarehouseId).HasColumnName("WarehouseId");
            this.Property(t => t.ProfileImage).HasColumnName("ProfileImage");
            this.Property(t => t.UserTypeCode).HasColumnName("UserTypeCode");
            this.Property(t => t.CooperationDay).HasColumnName("CooperationDay");
            this.Property(t => t.PlayerId).HasColumnName("PlayerId");
            this.Property(t => t.PlayerId_web).HasColumnName("PlayerId_web");
            this.Property(t => t.PositionName).HasColumnName("PositionName");
            this.Property(t => t.PositionCode).HasColumnName("PositionCode");
            this.Property(t => t.ChiefUserId).HasColumnName("ChiefUserId");
            this.Property(t => t.ChiefUserFullName).HasColumnName("ChiefUserFullName");
            this.Property(t => t.UserType_kd_id).HasColumnName("UserType_kd_id");
            this.Property(t => t.UserType_kd_id_cu).HasColumnName("UserType_kd_id_cu");
            this.Property(t => t.UserTypeName_kd).HasColumnName("UserTypeName_kd");
            this.Property(t => t.IsLetan).HasColumnName("IsLetan");
        }
    }
}
