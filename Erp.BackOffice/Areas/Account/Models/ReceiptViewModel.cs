using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.BackOffice.Account.Models
{
    public class ReceiptViewModel
    {
        public int Id { get; set; }

        [Display(Name = "IsDeleted")]
        public bool? IsDeleted { get; set; }

        [Display(Name = "CreatedUser", ResourceType = typeof(Wording))]
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }

        [Display(Name = "CreatedDate", ResourceType = typeof(Wording))]
        public DateTime? CreatedDate { get; set; }

        [Display(Name = "ModifiedUser", ResourceType = typeof(Wording))]
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }

        [Display(Name = "ModifiedDate", ResourceType = typeof(Wording))]
        public DateTime? ModifiedDate { get; set; }

        [Display(Name = "AssignedUserId", ResourceType = typeof(Wording))]
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [StringLength(100, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError", ErrorMessage = null)]
        [Display(Name = "Reason", ResourceType = typeof(Wording))]
        public string Name { get; set; }

        [Display(Name = "Code", ResourceType = typeof(Wording))]
        public string Code { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "Amount", ResourceType = typeof(Wording))]
        public decimal? Amount { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "PaymentMethod", ResourceType = typeof(Wording))]
        public string PaymentMethod { get; set; }
        [Display(Name = "BankAccountNo", ResourceType = typeof(Wording))]
        public string BankAccountNo { get; set; }
        [Display(Name = "BankAccountName", ResourceType = typeof(Wording))]
        public string BankAccountName { get; set; }
        [Display(Name = "BankName", ResourceType = typeof(Wording))]
        public string BankName { get; set; }
        public int? ProcessPaymentId { get; set; }
        [Display(Name = "Payers", ResourceType = typeof(Wording))]
        public string Payer { get; set; }
        [Display(Name = "Saler", ResourceType = typeof(Wording))]
        public Nullable<int> SalerId { get; set; }
        [Display(Name = "Saler", ResourceType = typeof(Wording))]
        public string SalerName { get; set; }
        [Display(Name = "Interpretations", ResourceType = typeof(Wording))]
        public string Note { get; set; }
        [Display(Name = "VoucherDate", ResourceType = typeof(Wording))]
        public Nullable<System.DateTime> VoucherDate { get; set; }
        [Display(Name = "Address", ResourceType = typeof(Wording))]
        public string Address { get; set; }
        [Display(Name = "SubjectName", ResourceType = typeof(Wording))]
        public Nullable<int> CustomerId { get; set; }
        [Display(Name = "CompanyName", ResourceType = typeof(Wording))]
        public string CompanyName { get; set; }

        [Display(Name = "DeleteNote", ResourceType = typeof(Wording))]
        public string CancelReason { get; set; }
        [Display(Name = "MaChungTuGoc", ResourceType = typeof(Wording))]
        public string MaChungTuGoc { get; set; }
        public string LoaiChungTuGoc { get; set; }
        public bool IsArchive { get; set; }
        public Nullable<int> LogReceiptId { get; set; }

        public string CustomerCode { get; set; }

        public string NVQL { get; set; }

        public string Status { get; set; }

        public int ProductInvoiceId { get; set; }

        public string ProductInvoiceCode { get; set; }

        public int? BranchId { get; set; }

        public string CountForBrand { get; set; }

    }
}