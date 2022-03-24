using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Account.Entities
{
    public class vwCustomer
    {
        public vwCustomer()
        {
            
        }

        public int Id { get; set; }
        public Nullable<bool> IsDeleted { get; set; }
        public Nullable<int> CreatedUserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<int> ModifiedUserId { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }

        public string Code { get; set; }
        public string CompanyName { get; set; }
        public int? BranchId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public Nullable<System.DateTime> Birthday { get; set; }
        public Nullable<bool> Gender { get; set; }
        public string Note { get; set; }
        public string Address { get; set; }
        public string WardId { get; set; }
        public string DistrictId { get; set; }
        public string CityId { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Email { get; set; }
        public int? Point { get; set; }

        public string ProvinceName { get; set; }
        public string DistrictName { get; set; }
        public string WardName { get; set; }
        public string GenderName { get; set; }
        public string CardCode { get; set; }
        public string SearchFullName { get; set; }
        public string Image { get; set; }
        public string CardIssuedName { get; set; }
        public string IdCardNumber { get; set; }
        public Nullable<System.DateTime> IdCardDate { get; set; }
        public string IdCardIssued { get; set; }
        public string CustomerType { get; set; }
        public string PositionCode { get; set; }
        public int? UserId { get; set; }

        public string BranchName { get; set; }
        public string BranchCode { get; set; }
        public string UserName { get; set; }
        public int? ManagerStaffId { get; set; }
        public Nullable<bool> IsBonusSales { get; set; }
        public bool? KhCuMuonBo { get; set; }
        public bool? KhCuThanPhien { get; set; }
        public bool? KhLauNgayKhongTuongTac { get; set; }
        public bool? KhMoiDenVaHuaQuaiLai { get; set; }
        public bool? KhMoiDenVaKinhTeYeu { get; set; }
        public string ManagerStaffName { get; set; }

        public string ManagerUserName { get; set; }
        public string SkinSkinLevel { get; set; }
        public string HairlLevel { get; set; }
        public string GladLevel { get; set; }

        public string cus_crm { get; set; }
        public bool? isLock { get; set; }
        
        public string EconomicStatus { get; set; }
        public string CustomerGroup { get; set; }
        public string Phoneghep { get; set; }

        public int? NhomHuongDS { get; set; }
        public string NhomNVKD { get; set; }
        
    }
}
