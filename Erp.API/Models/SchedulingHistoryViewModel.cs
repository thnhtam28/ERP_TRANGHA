using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class SchedulingHistoryViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }

        public Nullable<int> InquiryCardId { get; set; }

        public DateTime? WorkDay { get; set; }

        public Nullable<int> TotalMinute { get; set; }

        public Nullable<System.DateTime> EndDate { get; set; }
        public Nullable<System.DateTime> ExpectedEndDate { get; set; }
        public Nullable<int> CustomerId { get; set; }
        public string Status { get; set; }
        public string Code { get; set; }
        public Nullable<int> RoomId { get; set; }
        public string Note { get; set; }
        public Nullable<int> BranchId { get; set; }

        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string CreatedUserName { get; set; }
        public string CreatedUserCode { get; set; }
        public string InquiryCardCode { get; set; }
        public string Type { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string ProductCode { get; set; }
        public string ProductName { get; set; }
        public string RoomName { get; set; }
        public string FloorName { get; set; }
        public string CustomerImage { get; set; }
        public List<SchedulingHistoryViewModel> SchedulingList { get; set; }
        public List<StaffMadeViewModel> StaffMadeList { get; set; }

        public Nullable<System.DateTime> ExpectedWorkDay { get; set; }
        public string strEndDate { get; set; }
        public string ColorStatus { get; set; }
        public string strStatus { get; set; }
        public string CustomerImagePath { get; set; }
        public List<LogEquipmentViewModel> EquipmentList { get; set; }
        public string EquimentGroup { get; set; }
        public List<SelectListItem> EquimentGroupList { get; set; }
        public int? THOIGIANBAOTSAU_GIAY { get; set; }
        public int? THOIGIANBAOTRUOC_GIAY { get; set; }
    }
}