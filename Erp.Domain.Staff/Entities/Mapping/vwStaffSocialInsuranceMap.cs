using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Entities.Mapping
{
    public class vwStaffSocialInsuranceMap : EntityTypeConfiguration<vwStaffSocialInsurance>
    {
        public vwStaffSocialInsuranceMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            this.Property(t => t.MedicalCode).HasMaxLength(150);
            this.Property(t => t.SocietyCode).HasMaxLength(150);


            // Table & Column Mappings
            this.ToTable("vwStaff_SocialInsurance");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.StaffId).HasColumnName("StaffId");
            this.Property(t => t.MedicalCode).HasColumnName("MedicalCode");
            this.Property(t => t.MedicalStartDate).HasColumnName("MedicalStartDate");
            this.Property(t => t.MedicalEndDate).HasColumnName("MedicalEndDate");
            this.Property(t => t.MedicalIssue).HasColumnName("MedicalIssue");
            this.Property(t => t.MedicalDefaultValue).HasColumnName("MedicalDefaultValue");
            this.Property(t => t.SocietyCode).HasColumnName("SocietyCode");
            this.Property(t => t.SocietyStartDate).HasColumnName("SocietyStartDate");
            this.Property(t => t.SocietyEndDate).HasColumnName("SocietyEndDate");
            this.Property(t => t.SocietyIssue).HasColumnName("SocietyIssue");
            this.Property(t => t.SocietyDefaultValue).HasColumnName("SocietyDefaultValue");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.StaffName).HasColumnName("StaffName");
            this.Property(t => t.Birthday).HasColumnName("Birthday");
            this.Property(t => t.IdCardDate).HasColumnName("IdCardDate");
            this.Property(t => t.IdCardIssued).HasColumnName("IdCardIssued");
            this.Property(t => t.IdCardNumber).HasColumnName("IdCardNumber");
            this.Property(t => t.Ethnic).HasColumnName("Ethnic");
            this.Property(t => t.Email).HasColumnName("Email");
            this.Property(t => t.Literacy).HasColumnName("Literacy");
            this.Property(t => t.ProvinceName).HasColumnName("ProvinceName");
            this.Property(t => t.BranchName).HasColumnName("BranchName");
            this.Property(t => t.PositionName).HasColumnName("PositionName");
            this.Property(t => t.CodeName).HasColumnName("CodeName");
            this.Property(t => t.StaffCode).HasColumnName("StaffCode");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.ProfileImage).HasColumnName("ProfileImage");
            this.Property(t => t.PC_CV).HasColumnName("PC_CV");
            this.Property(t => t.PC_TNN).HasColumnName("PC_TNN");
            this.Property(t => t.PC_TNVK).HasColumnName("PC_TNVK");
            this.Property(t => t.PC_Khac).HasColumnName("PC_Khac");
            this.Property(t => t.TienLuong).HasColumnName("TienLuong");
            this.Property(t => t.Gender).HasColumnName("Gender");

        }
    }
}
