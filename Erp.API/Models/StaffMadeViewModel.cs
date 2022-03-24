using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class StaffMadeViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }

        public int? AssignedUserId { get; set; }
        public string AssignedUserName { get; set; }
        public Nullable<int> UserId { get; set; }
        public string Status { get; set; }
        public string Note { get; set; }
        public Nullable<int> SchedulingHistoryId { get; set; }
        public string FullName { get; set; }
        public string UserCode { get; set; }
        public string ProfileImage { get; set; }
        public Nullable<System.DateTime> WorkDay { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string RoomName { get; set; }
        public string FloorName { get; set; }
        public string Type { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public string SchedulingCode { get; set; }
        public string SchedulingStatus { get; set; }
        public string CustomerName { get; set; }
        public string CustomerCode { get; set; }
        public string CustomerImage { get; set; }

        public Nullable<System.DateTime> ExpectedWorkDay { get; set; }
        public Nullable<int> TotalMinute { get; set; }
        public Nullable<System.DateTime> ExpectedEndDate { get; set; }
        public Nullable<System.DateTime> EndDate { get; set; }
        public string Name_Bed { get; set; }
        
    }
}