using System;
using System.Collections.Generic;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Entities.Mapping
{
    public class vwMembership_parentMap : EntityTypeConfiguration<vwMembership_parent>
    {
        public vwMembership_parentMap()
        {
            // Primary Key
            this.HasKey(t => t.Id);

            // Properties
            //this.Property(t => t.Name).HasMaxLength(150);
            this.Property(t => t.Status).HasMaxLength(50);
            this.Property(t => t.Code).HasMaxLength(100);

            // Table & Column Mappings
            this.ToTable("vwSale_Membership_parent");
            this.Property(t => t.Id).HasColumnName("Id");
            this.Property(t => t.CreatedUserId).HasColumnName("CreatedUserId");
            this.Property(t => t.CreatedDate).HasColumnName("CreatedDate");
            this.Property(t => t.ModifiedUserId).HasColumnName("ModifiedUserId");
            this.Property(t => t.ModifiedDate).HasColumnName("ModifiedDate");
            this.Property(t => t.AssignedUserId).HasColumnName("AssignedUserId");

            this.Property(t => t.CustomerId).HasColumnName("CustomerId");
            this.Property(t => t.ProductInvoiceId).HasColumnName("ProductInvoiceId");
            this.Property(t => t.Status).HasColumnName("Status");
            this.Property(t => t.Code).HasColumnName("Code");
            this.Property(t => t.QRCode).HasColumnName("QRCode");
            this.Property(t => t.Note).HasColumnName("Note");
            this.Property(t => t.SerialNumber).HasColumnName("SerialNumber");
            this.Property(t => t.ExpiryDate).HasColumnName("ExpiryDate");
            this.Property(t => t.Total).HasColumnName("Total");
            this.Property(t => t.ProductInvoiceId_Return).HasColumnName("ProductInvoiceId_Return");
            this.Property(t => t.isPrint).HasColumnName("isPrint");
            this.Property(t => t.NumberPrint).HasColumnName("NumberPrint");
            this.Property(t => t.BranchId).HasColumnName("BranchId");
            this.Property(t => t.is_extend).HasColumnName("is_extend");
            this.Property(t => t.ExpiryDateOld).HasColumnName("ExpiryDateOld");
            this.Property(t => t.idcu).HasColumnName("idcu");

            this.Property(t => t.ProductInvoiceDetailId).HasColumnName("ProductInvoiceDetaiId");
            this.Property(t => t.ProductId).HasColumnName("ProductId");
            this.Property(t => t.ProductName).HasColumnName("ProductName");
            this.Property(t => t.ProductCode).HasColumnName("ProductCode");
            this.Property(t => t.Quantity).HasColumnName("Quantity");
            this.Property(t => t.ProductType).HasColumnName("ProductType");

            this.Property(t => t.solandadung).HasColumnName("solandadung");

            this.Property(t => t.ManagerName).HasColumnName("ManagerName");
            this.Property(t => t.CustomerName).HasColumnName("CustomerName");
            this.Property(t => t.CustomerCode).HasColumnName("CustomerCode");
            this.Property(t => t.ManagerId).HasColumnName("ManagerId");
            this.Property(t => t.TargetCode).HasColumnName("TargetCode");
            this.Property(t => t.ChiefUserId).HasColumnName("ChiefUserId");
            this.Property(t => t.ChiefUserFullName).HasColumnName("ChiefUserFullName");
            this.Property(t => t.UserTypeName_kd).HasColumnName("UserTypeName_kd");
            this.Property(t => t.tienconno).HasColumnName("tienconno");


        }
    }
}
