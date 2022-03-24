﻿using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Erp.BackOffice.Areas.Administration.Models
{
    public class CreateUserViewModel
    {
        [Required(ErrorMessageResourceName = "Username_Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "Username", ResourceType = typeof(Wording))]
        public string UserName { get; set; }

        [Required(ErrorMessageResourceName = "Password_Required", ErrorMessageResourceType = typeof(Error))]
        [StringLength(100, ErrorMessageResourceName = "Password_LengthError", ErrorMessageResourceType = typeof(Error), MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password", ResourceType = typeof(Wording))]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "ConfirmPassword", ResourceType = typeof(Wording))]
        [Compare("Password", ErrorMessageResourceName = "ConfirmPassword_NotMatch", ErrorMessageResourceType = typeof(Error))]
        public string ConfirmPassword { get; set; }

        [Display(Name = "Status", ResourceType = typeof(Wording))]
        public UserStatus? Status { get; set; }

        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "UserType", ResourceType = typeof(Wording))]
        public int? UserTypeId { get; set; }
        public IEnumerable<UserType> listUserType { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "FirstName", ResourceType = typeof(Wording))]
        public string FirstName { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError")]
        [Required(ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(Error))]
        [Display(Name = "LastName", ResourceType = typeof(Wording))]
        public string LastName { get; set; }

        [Display(Name = "DateOfBirth", ResourceType = typeof(Wording))]
        [DataType(DataType.Date, ErrorMessageResourceName = "DateInvalid", ErrorMessageResourceType = typeof(Error))]
        public DateTime? DateOfBirth { get; set; }

        [StringLength(200, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError")]
        [Display(Name = "Address", ResourceType = typeof(Wording))]
        public string Address { get; set; }

        [Display(Name = "PhoneNumer", ResourceType = typeof(Wording))]
        public string Mobile { get; set; }

        [StringLength(50, ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "StringError")]
        [Display(Name = "Email", ResourceType = typeof(Wording))]
        [EmailAddress(ErrorMessageResourceType = typeof(Error), ErrorMessageResourceName = "EmailError", ErrorMessage = null)]
        public string Email { get; set; }

        [Display(Name = "Gender", ResourceType = typeof(Wording))]
        public bool? Sex { get; set; }

        [Display(Name = "Bank Name")]
        public string BankName { get; set; }
        [Display(Name = "Bank OwnerName")]
        public string BankOwnerName { get; set; }
        [Display(Name = "Bank Branch")]
        public string BankBranch { get; set; }
        [Display(Name = "Bank User Number")]
        public string BankUserNumber { get; set; }
        
        [Display(Name = "BranchName", ResourceType = typeof(Wording))]
        public string BranchId { get; set; }
        [Display(Name = "ProfileImage", ResourceType = typeof(Wording))]
        public string ProfileImage { get; set; }
        //public string DrugStore { get; set; }
        public string PlayerId_web { get; set; }
        [Display(Name = "Staff_PositionID", ResourceType = typeof(Wording))]
        public int? Staff_PositionId { get; set; }
        [Display(Name = "Staff_PositionCV", ResourceType = typeof(Wording))]
        public int? IdManager { get; set; }

        [Display(Name = "Discountt", ResourceType = typeof(Wording))]
        public decimal? Discount { get; set; }
        //[Display(Name = "UserManager", ResourceType = typeof(Wording))]
        //public int? ParentId { get; set; }
        //public string ParentName { get; set; }
    }
}