using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Erp.Domain.Entities
{
    public class User
    {
        public User()
        {

        }

        public int Id { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public string UserName { get; set; }
        public UserStatus? Status { get; set; }
        public string UserCode { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public Nullable<System.DateTime> DateOfBirth { get; set; }
        public string Address { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public Nullable<bool> Sex { get; set; }
        public Nullable<int> UserTypeId { get; set; }
        public Nullable<int> UserType_kd_id { get; set; }
        public string UserType_kd_id_cu { get; set; }
        public Nullable<int> LoginFailedCount { get; set; }
        public Nullable<int> LoginFailedCount2 { get; set; }
        public Nullable<int> LoginFailedCount3 { get; set; }
        public Nullable<System.DateTime> LastChangedPassword { get; set; }
        public Nullable<int> BranchId { get; set; }
        public string ProfileImage { get; set; }
        //public Nullable<int> ParentId { get; set; }
        public string PlayerId { get; set; }
        public string PlayerId_web { get; set; }
        public int? Staff_PositionId{ get; set; }
        public int? IdManager { get; set; }
        public decimal? Discount { get; set; }
        public Nullable<bool> IsLetan { get; set; }
        
    }

    public enum UserStatus
    {
        [Display(Name = "Ngừng hoạt động")]
        DeActive = 0,
        [Display(Name = "Đang hoạt động")]
        Active = 1,
        [Display(Name = "Pending")]
        Pending = -1,
        [Display(Name = "Nghỉ việc")]
        Off = -2
    }

}
