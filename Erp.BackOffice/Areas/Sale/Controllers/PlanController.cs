using Erp.BackOffice.Filters;
using Erp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;
using Erp.Domain.Crm.Interfaces;
using Erp.BackOffice.Areas.Cms.Models;
using Erp.Domain.Crm.Entities;
using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.Domain.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Data.Entity.Infrastructure;
using System.Text.RegularExpressions;
using Erp.Domain.Account.Interfaces;
using Erp.Utilities;
using Erp.BackOffice.Areas.Administration.Models;
using Erp.BackOffice.Account.Models;
using PagedList;
using Erp.Domain.Crm.Helper;
using System.IO;
using System.Web;
//sdfghuio
namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class PlanController : Controller
    {
        private readonly IPlanRepository PlanRepository;
        private readonly IKH_TUONGTACRepository KH_TUONGTACRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IUserRepository userRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IProductInvoiceRepository productInvoiceRepository;
        private readonly IInquiryCardRepository InquiryCardRepository;
        private readonly IInquiryCardDetailRepository inquiryCardDetailRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;


        public PlanController (
            IPlanRepository _Plan,
            IKH_TUONGTACRepository _kH_TUONGTACRepository,
            ICategoryRepository _category,
            IUserRepository _user,
            ICustomerRepository _customer,
            IProductInvoiceRepository _productInvoice,
            IInquiryCardRepository _InquiryCard,
            IInquiryCardDetailRepository inquiryCard,
            ITemplatePrintRepository _templatePrint
        )
        {
            PlanRepository = _Plan;
            KH_TUONGTACRepository = _kH_TUONGTACRepository;
            categoryRepository = _category;
            userRepository = _user;
            customerRepository = _customer;
            productInvoiceRepository = _productInvoice;
            InquiryCardRepository = _InquiryCard;
            inquiryCardDetailRepository = inquiryCard;
            templatePrintRepository = _templatePrint;
        }
        public IEnumerable<KH_TUONGTACViewModel> GETKH_TUONGTAC()
        {

            IEnumerable<KH_TUONGTACViewModel> q = KH_TUONGTACRepository.GetAllvwKH_TUONGTAC()
                  .Select(item => new KH_TUONGTACViewModel
                  {
                      KH_TUONGTAC_ID = item.KH_TUONGTAC_ID,
                      CustomerCode = item.CustomerCode,
                      CustomerName = item.CustomerName,
                      Phone = item.Phone,
                      NGAYLAP = item.NGAYLAP,
                      //THANG = item.THANG.Value,
                      //NAM = item.NAM.Value,
                      HINHTHUC_TUONGTAC = item.HINHTHUC_TUONGTAC,
                      LOAI_TUONGTAC = item.LOAI_TUONGTAC,
                      PHANLOAI_TUONGTAC = item.PHANLOAI_TUONGTAC,
                      TINHTRANG_TUONGTAC = item.TINHTRANG_TUONGTAC,
                      MUCDO_TUONGTAC = item.MUCDO_TUONGTAC,
                      GHI_CHU = item.GHI_CHU,
                      GIAIPHAP_TUONGTAC = item.GIAIPHAP_TUONGTAC,
                      NGAYTUONGTAC_TIEP = item.NGAYTUONGTAC_TIEP,
                      GIOTUONGTAC_TIEP = item.GIOTUONGTAC_TIEP,
                      MUCCANHBAO_TUONGTAC = item.MUCCANHBAO_TUONGTAC,
                      HINH_ANH = item.HINH_ANH,
                      BranchId = item.BranchId,
                      FullName = item.FullName,
                      ModifiedDate = item.ModifiedDate,
                      CreatedDate = item.CreatedDate,
                      //LICHTUONGTATIEP = item.NGAYTUONGTAC_TIEP + "/" + item.THANG.Value + "/" + item.NAM.Value + " " + item.GIOTUONGTAC_TIEP,
                      GIO_TUONGTAC = item.GIO_TUONGTAC,
                      NGUOILAP_ID = item.NGUOILAP_ID,
                      KHACHHANG_ID = item.KHACHHANG_ID,
                      KETQUA_SAUTUONGTAC = item.KETQUA_SAUTUONGTAC

                  });

            return q;
        }
        public ViewResult PlanUseSkinCare(string CustomerCodeOrName, string ProductName, int? BranchId, string StartDate, string EndDate, string THANHTOAN, int? PhieuConTu, int? PhieuConDen, int? ManagerStaffId)
        {   
            // 2 dòng tìm theo tên và mã
           // string CustomerName,
           // string CustomerCode
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);
            BranchId = intBrandID;
            IEnumerable<vwPlanuseSkinCareViewModel> q = null;
            //if (ManagerStaffId == null || ManagerStaffId == 0)
            //{
            //   // ManagerStaffId = WebSecurity.CurrentUserId;
            //    return View(q);
            //}
            q = PlanRepository.GetvwAllvwPlanuseSkinCare()
                 .Select(item => new vwPlanuseSkinCareViewModel
                 {
                     BranchId = item.BranchId,
                     CustomerCode = item.CustomerCode,
                     CustomerName = item.CustomerName,
                     Phone = item.Phone,
                     ProductInvoiceCode = item.ProductInvoiceCode,
                     ProductName = item.ProductName,
                     SOLUONG = item.SOLUONG,
                     soluongdung = item.soluongdung,
                     soluongtra = item.soluongtra,
                     soluongchuyen = item.soluongchuyen,
                     soluongconlai = item.soluongconlai,
                     Type = item.Type,
                     ModifiedDate = item.ModifiedDate,
                     GladLevel = item.GladLevel,
                     TargetId = item.TargetId,
                     CreatedDate = item.CreatedDate,
                     ProductInvoiceId = item.ProductInvoiceId,
                     THANGTOANHET = item.THANGTOANHET,
                     CustomerId = item.CustomerId,
                     ManagerStaffId = item.ManagerStaffId,
                     ManagerName = item.ManagerName
                 });   
   
            if (BranchId != null && BranchId > 0)
            {
                q = q.Where(x => x.BranchId == BranchId).ToList();
            }

            if (!string.IsNullOrEmpty(CustomerCodeOrName))
            {        
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerCode).Contains(Helpers.Common.ChuyenThanhKhongDau(CustomerCodeOrName)) || x.CustomerName.Contains(CustomerCodeOrName)).ToList();
            }
            //if (!string.IsNullOrEmpty(CustomerName))
            //{
            //    //CustomerName = Helpers.Common.ChuyenThanhKhongDau(CustomerName);
            //    //q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(CustomerName));
            //}
            //if (!string.IsNullOrEmpty(CustomerCode))
            //{
            //    q = q.Where(x => x.CustomerCode.Contains(CustomerCode));
            //}

            if (!string.IsNullOrEmpty(ProductName))
            {
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.ProductName).Contains(Helpers.Common.ChuyenThanhKhongDau(ProductName))).ToList();
            }

            if (ManagerStaffId != null && ManagerStaffId > 0)
            {

                q = q.Where(x => x.ManagerStaffId == ManagerStaffId).ToList();
            }

            if (PhieuConTu != null)
            {
                if (PhieuConDen != null)
                {
                    q = q.Where(x => x.soluongconlai >= PhieuConTu && x.soluongconlai <= PhieuConDen).ToList();
                }
            }

            if (!string.IsNullOrEmpty(THANHTOAN))
            {
                THANHTOAN = Helpers.Common.ChuyenThanhKhongDau(THANHTOAN);
                if (THANHTOAN == "het")
                {
                    //THANHTOAN = null;
                    q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.THANGTOANHET).Contains(THANHTOAN)).ToList();
                }
                else
                {
                    q = q.Where(x => x.THANGTOANHET == null).ToList();
                }
            }
            //Lọc theo ngày

            if (!string.IsNullOrEmpty(StartDate))
            {
                var startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                DateTime d_startDate = Convert.ToDateTime(startDate);
                if (!string.IsNullOrEmpty(EndDate))
                {
                    var endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime d_endDate = Convert.ToDateTime(endDate);
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.ModifiedDate >= d_startDate && x.ModifiedDate <= d_endDate).ToList();

                }
            }
            ///
            if (StartDate == null && EndDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");

                DateTime d_startDate1, d_endDate1;
                if (DateTime.TryParseExact(StartDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate1))
                {
                    if (DateTime.TryParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate1))
                    {
                        d_endDate1 = d_endDate1.AddHours(23).AddMinutes(59);
                        //ViewBag.retDateTime = d_endDate;
                        //ViewBag.aDateTime = d_startDate;
                        q = q.Where(x => x.ModifiedDate >= d_startDate1 && x.ModifiedDate <= d_endDate1).ToList();
                    }
                }
            }

            ///

            //var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            //{
            //    BranchId = (Erp.BackOffice.Helpers.Common.CurrentUser.BranchId == null ? 0 : Erp.BackOffice.Helpers.Common.CurrentUser.BranchId),
            //    UserId = WebSecurity.CurrentUserId,
            //}).ToList();

            //List<string> listNhanvien = new List<string>();


            //for (int i = 0; i < dataNhanvien.Count; i++)
            //{
            //    listNhanvien.Add(dataNhanvien[i].Id.ToString());
            //}
            //ViewBag.NGUOILAP = WebSecurity.CurrentUserId;

            //var filteredOrders = from order in q
            //                     where listNhanvien.Contains(order.ManagerStaffId.ToString())
            //                     select order;
            ////var LSTT = KH_TUONGTACRepository.GetAllKH_TUONGTAC();
            ////ViewBag.LICHSUTUONGTAC = LSTT;

            //List<int> listNVien = new List<int>();
            //for (int i = 0; i < dataNhanvien.Count; i++)
            //{
            //    listNVien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            //}
            //listNVien.Add(WebSecurity.CurrentUserId);
            //var user = userRepository.GetAllUsers().Select(item => new UserViewModel
            //{
            //    Id = item.Id,
            //    UserName = item.UserName,
            //    FullName = item.FullName,
            //    Status = item.Status,

            //}).Where(x => x.Status == UserStatus.Active && listNVien.Contains(x.Id)).ToList();
            //ViewBag.User = user;
           
            var data = GETKH_TUONGTAC();
            ViewBag.LICHSUTUONGTAC = data;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);

        }

        #region Create
        public ActionResult Create(string NGAYLAP, int? NGUOILAP_ID)
        {
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);

            if (DateTime.Parse(NGAYLAP) < (DateTime.Now.Date))
            {
                TempData["Seccess"] = "Ngày lập phải lớn hơn ngày hiện tại";
                return RedirectToAction("Index");
            }
            var DUOITOC = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "DUOITOC");
            ViewBag.DUOITOC = DUOITOC;
            var DODAITOC = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "DODAITOC");
            ViewBag.DODAITOC = DODAITOC;
            var DADAU = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "DADAU");
            ViewBag.DADAU = DADAU;
            var MAMTOC = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "MAMTOC");
            ViewBag.MAMTOC = MAMTOC;
            var THANTOC = categoryRepository.GetAllCategories()
                 .Select(item => new CategoryViewModel
                 {
                     Value = item.Value,
                     Code = item.Code,
                     Name = item.Name,
                 }).Where(x => x.Code == "THANTOC");
            ViewBag.THANTOC = THANTOC;

            var CO = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "CO");
            ViewBag.CO = CO;
            var MAT = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "MAT");
            ViewBag.MAT = MAT;
            var BODY = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "BODY");
            ViewBag.BODY = BODY;
            var DAMAT = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "DAMAT");
            ViewBag.DAMAT = DAMAT;

            var data = GETKH_TUONGTAC();
            if (NGUOILAP_ID != null)
            {
                data.Where(x => x.NGUOILAP_ID == NGUOILAP_ID);
            }
            ViewBag.KH_TUONGTAC = data;

            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = (intBrandID == null ? 0 : intBrandID),
                UserId = NGUOILAP_ID,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }





            var CustomerList = customerRepository.GetAllvwCustomer()
               .Select(item => new CustomerViewModel
               {
                   Code = item.Code,
                   Id = item.Id,
                   CompanyName = item.CompanyName,
                   Image = item.Image,
                   ManagerStaffId = item.ManagerStaffId,
                   isLock = item.isLock,
                   BranchId = item.BranchId
               }).Where(x => x.BranchId == intBrandID && x.isLock != true).ToList();

            //if (!Erp.BackOffice.Filters.SecurityFilter.IsAdmin())
            //{

                CustomerList = CustomerList.Where(x => (x.ManagerStaffId != null && listNhanvien.Contains(x.ManagerStaffId.Value))).ToList();
           // }


            if (CustomerList.Count  == 0)
            {
                return Content("<script>alert('Nhân viên chưa có khách hàng để quản lý!')</script>");
            }







            //CustomerList= CustomerList.Where(x => x.isLock != true);


            ViewBag.NGUOI_LAP = NGUOILAP_ID;
            ViewBag.CustomerList = CustomerList;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View();
        }
        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Create(KH_TUONGTACViewModel model, bool? IsPopup)
        {
            var NGUOI_LAP = Request["NGUOILAP_ID"];
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);

            try
            {
                if (ModelState.IsValid)
                {

                    var KH_TUONGTAC = new KH_TUONGTAC();
                    AutoMapper.Mapper.Map(model, KH_TUONGTAC);
                    KH_TUONGTAC.IsDeleted = false;
                    KH_TUONGTAC.CreatedUserId = WebSecurity.CurrentUserId;
                    KH_TUONGTAC.CreatedDate = DateTime.Now;
                    KH_TUONGTAC.ModifiedDate = DateTime.Now;
                    if (model.NGUOILAP_ID != null)
                    {
                        KH_TUONGTAC.NGUOILAP_ID = model.NGUOILAP_ID;
                    }
                    else if (NGUOI_LAP != null)
                    {
                        KH_TUONGTAC.NGUOILAP_ID = Convert.ToInt32(NGUOI_LAP);
                    }
                    else
                    {
                        KH_TUONGTAC.NGUOILAP_ID = WebSecurity.CurrentUserId;
                    }
                    KH_TUONGTAC.GIO_TUONGTAC = model.GIO_TUONGTAC;
                    KH_TUONGTAC.NGAYLAP = model.NGAYLAP;
                    //KH_TUONGTAC.THANG = int.Parse(model.NGAYTUONGTAC_TIEP.Substring(3, 2));
                    //KH_TUONGTAC.NAM = int.Parse(model.NGAYTUONGTAC_TIEP.Substring(6, 4));
                    KH_TUONGTAC.THANG = int.Parse(model.NGAYLAP.Substring(3,2) );//DateTime.Now.Month;
                    KH_TUONGTAC.NAM = int.Parse(model.NGAYLAP.Substring(6, 4));//DateTime.Now.Year;

                    if (KH_TUONGTAC.NGAYTUONGTAC_TIEP != null)
                    {
                        KH_TUONGTAC.NGAYTUONGTAC_TIEP = model.NGAYTUONGTAC_TIEP.Substring(0, 10);
                        KH_TUONGTAC.GIOTUONGTAC_TIEP = model.NGAYTUONGTAC_TIEP.Substring(10, 6);

                    }
                    else
                    {
                        KH_TUONGTAC.NGAYTUONGTAC_TIEP = null;
                        KH_TUONGTAC.GIOTUONGTAC_TIEP = null;
                    }
                    KH_TUONGTAC.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);


                    KH_TUONGTACRepository.InsertKH_TUONGTAC(KH_TUONGTAC);

                    var path = System.Web.HttpContext.Current.Server.MapPath("~" + Helpers.Common.GetSetting("Plan"));
                    if (Request.Files["file-image"] != null)
                    {
                        var file = Request.Files["file-image"];
                        if (file.ContentLength > 0)
                        {
                            string image_name = KH_TUONGTAC.KH_TUONGTAC_ID + "." + file.FileName.Split('.').Last();
                            bool isExists = System.IO.Directory.Exists(path);
                            if (!isExists)
                                System.IO.Directory.CreateDirectory(path);
                            file.SaveAs(path + image_name);
                            KH_TUONGTAC.HINH_ANH = image_name;
                            KH_TUONGTACRepository.UpdateKH_TUONGTAC(KH_TUONGTAC);
                        }

                    }
                    if (IsPopup == true)
                    {
                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;

                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                    }
                    else
                    {
                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                        return RedirectToAction("Index");
                    }
                    //TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                    //return RedirectToAction("Index");

                }
            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("Create");
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                string idDeleteAll = Request["DeleteId-checkbox"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    var item = KH_TUONGTACRepository.GetKH_TUONGTACById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        item.IsDeleted = true;
                        KH_TUONGTACRepository.UpdateKH_TUONGTAC(item);
                    }
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return RedirectToAction("Detail");
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("Detail");
            }
        }
        #endregion

        #region Detail
        public ActionResult Detail(int? NGUOILAP_ID, string NGAYLAP, string search, string FullName)
        {
            var model = GETListKH_TUONGTAC(NGUOILAP_ID, NGAYLAP, search, FullName);

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(model);
        }

        public List<KH_TUONGTACViewModel> GETListKH_TUONGTAC(int? NGUOILAP_ID, string NGAYLAP, string search, string FullName)
        {
            //if (!string.IsNullOrEmpty(search))
            //{
            //    DateTime ngaylap = Convert.ToDateTime(NGAYLAP);
            //    NGAYLAP = ngaylap.AddDays(-1).ToString("dd/MM/yyyy");

            //}
            NGUOILAP_ID = NGUOILAP_ID == null ? 0 : NGUOILAP_ID;
            NGAYLAP = NGAYLAP == null ? "" : NGAYLAP;
            FullName = FullName == null ? "" : FullName;
            var q = KH_TUONGTACRepository.GetAllvwKH_TUONGTAC().Where(x => x.IsDeleted != true);
            var model = q.Select(item => new KH_TUONGTACViewModel
            {
                KH_TUONGTAC_ID = item.KH_TUONGTAC_ID,
                NGAYLAP = item.NGAYLAP,
                THANG = item.THANG,
                NAM = item.NAM,
                HINHTHUC_TUONGTAC = item.HINHTHUC_TUONGTAC,
                LOAI_TUONGTAC = item.LOAI_TUONGTAC,
                PHANLOAI_TUONGTAC = item.PHANLOAI_TUONGTAC,
                TINHTRANG_TUONGTAC = item.TINHTRANG_TUONGTAC,
                MUCDO_TUONGTAC = item.MUCDO_TUONGTAC,
                GHI_CHU = item.GHI_CHU,
                GIAIPHAP_TUONGTAC = item.GIAIPHAP_TUONGTAC,
                NGAYTUONGTAC_TIEP = item.NGAYTUONGTAC_TIEP,
                GIOTUONGTAC_TIEP = item.GIOTUONGTAC_TIEP,
                MUCCANHBAO_TUONGTAC = item.MUCCANHBAO_TUONGTAC,
                HINH_ANH = item.HINH_ANH,
                BranchId = item.BranchId,
                FullName = item.FullName,
                ModifiedDate = item.ModifiedDate,
                CreatedDate = item.CreatedDate,
                GIO_TUONGTAC = item.GIO_TUONGTAC,
                //LICHTUONGTATIEP = item.NGAYTUONGTAC_TIEP + "/" + item.THANG.Value + "/" + item.NAM.Value + " " + item.GIOTUONGTAC_TIEP,
                NGUOILAP_ID = item.NGUOILAP_ID,
                Phone = item.Phone,
                CustomerName = item.CustomerName,
                CustomerCode = item.CustomerCode,
                CustomerId = item.KHACHHANG_ID,
                KETQUA_SAUTUONGTAC = item.KETQUA_SAUTUONGTAC,
            }).OrderByDescending(m => m.CustomerCode).ThenByDescending(m => m.CreatedDate).ToList();

            bool hasSearch = false;
            if (!string.IsNullOrEmpty(NGAYLAP))
            {
                NGAYLAP = Helpers.Common.ChuyenThanhKhongDau(NGAYLAP);
                model = model.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.NGAYLAP).Contains(NGAYLAP)).ToList();
                hasSearch = true;
            }
            //if (!string.IsNullOrEmpty(NGAYKH))
            //{
            //    NGAYKH = Helpers.Common.ChuyenThanhKhongDau(NGAYKH);
            //    model = model.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.NGAYLAP).Contains(NGAYKH)).ToList();
            //    hasSearch = true;
            //}
            if (NGUOILAP_ID != null && NGUOILAP_ID.Value > 0)
            {
                model = model.Where(x => x.NGUOILAP_ID == NGUOILAP_ID).ToList();
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return model;
        }
        public PartialViewResult GetGETListKH_TUONGTAC(int? NGUOILAP_ID, string NGAYLAP, string search, string FullName)
        {
            var model = GETListKH_TUONGTAC(NGUOILAP_ID, NGAYLAP, search, FullName);
            return PartialView(model);
        }
        #endregion
        #region Edit
        public ActionResult Edit(int? NGUOILAP_ID, int? KH_TUONGTAC_ID, string CustomerName, string NGAYLAP, int KHACHHANG_ID)
        {

            var DUOITOC = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "DUOITOC");
            ViewBag.DUOITOC = DUOITOC;
            var DODAITOC = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "DODAITOC");
            ViewBag.DODAITOC = DODAITOC;
            var DADAU = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "DADAU");
            ViewBag.DADAU = DADAU;
            var MAMTOC = categoryRepository.GetAllCategories()
                .Select(item => new CategoryViewModel
                {
                    Value = item.Value,
                    Code = item.Code,
                    Name = item.Name,
                }).Where(x => x.Code == "MAMTOC");
            ViewBag.MAMTOC = MAMTOC;
            var THANTOC = categoryRepository.GetAllCategories()
                 .Select(item => new CategoryViewModel
                 {
                     Value = item.Value,
                     Code = item.Code,
                     Name = item.Name,
                 }).Where(x => x.Code == "THANTOC");
            ViewBag.THANTOC = THANTOC;

            var CO = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "CO");
            ViewBag.CO = CO;
            var MAT = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "MAT");
            ViewBag.MAT = MAT;
            var BODY = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "BODY");
            ViewBag.BODY = BODY;
            var DAMAT = categoryRepository.GetAllCategories()
             .Select(item => new CategoryViewModel
             {
                 Value = item.Value,
                 Code = item.Code,
                 Name = item.Name,
             }).Where(x => x.Code == "DAMAT");
            ViewBag.DAMAT = DAMAT;
            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = Erp.BackOffice.Helpers.Common.CurrentUser.BranchId,
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }
            var data = GETKH_TUONGTAC().Where(x => x.KHACHHANG_ID == KHACHHANG_ID && x.NGUOILAP_ID == NGUOILAP_ID);
            ViewBag.KH_TUONGTAC = data;

            var CustomerList = customerRepository.GetAllvwCustomer()
               .Select(item => new CustomerViewModel
               {
                   Code = item.Code,
                   Id = item.Id,
                   CompanyName = item.CompanyName,
                   Image = item.Image,
                   ManagerStaffId = item.ManagerStaffId
               }).Where(x => listNhanvien.Contains(x.ManagerStaffId.Value));

            //if (!Erp.BackOffice.Filters.SecurityFilter.IsAdmin())
            //{
            //    CustomerList = CustomerList.Where(x => x.ManagerStaffId == WebSecurity.CurrentUserId);
            //}
            ViewBag.CustomerList = CustomerList;
            ViewBag.NGUOILAP = NGUOILAP_ID;

            var KH_TUONGTAC = KH_TUONGTACRepository.GetKH_TUONGTACById(KH_TUONGTAC_ID.Value);
            if (KH_TUONGTAC != null && KH_TUONGTAC.IsDeleted != true)
            {
                var KH_TUONGTAC1 = new KH_TUONGTAC();

                var model = new KH_TUONGTACViewModel();
                AutoMapper.Mapper.Map(KH_TUONGTAC, model);
                //KH_TUONGTAC.NGAYTUONGTAC_TIEP= model.NGAYTUONGTAC_TIEP;
                //KH_TUONGTAC.GIOTUONGTAC_TIEP = model.GIOTUONGTAC_TIEP;
                KH_TUONGTAC1.NGAYTUONGTAC_TIEP = model.NGAYTUONGTAC_TIEP;
                KH_TUONGTAC1.GIOTUONGTAC_TIEP = model.GIOTUONGTAC_TIEP;
                // 20/4/2019 

                return View(model);
            }

         
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View();
        }



        public ActionResult EditView(int? NGUOILAP_ID)
        {


            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = Erp.BackOffice.Helpers.Common.CurrentUser.BranchId,
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }
            var data = GETKH_TUONGTAC();
            ViewBag.KH_TUONGTAC = data;


            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View();
        }
        [HttpPost]
        public ActionResult Edit(KH_TUONGTACViewModel model)
        {
            var NGUOI_LAP = Request["NGUOILAP_ID"];
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);

            if (ModelState.IsValid)
            {
                var KH_TUONGTAC = KH_TUONGTACRepository.GetKH_TUONGTACById(model.KH_TUONGTAC_ID);
                AutoMapper.Mapper.Map(model, KH_TUONGTAC);
                KH_TUONGTAC.ModifiedUserId = WebSecurity.CurrentUserId;
                if (model.NGUOILAP_ID != null) {
                    KH_TUONGTAC.NGUOILAP_ID = model.NGUOILAP_ID;
                } else if (NGUOI_LAP != null)
                {
                    KH_TUONGTAC.NGUOILAP_ID = Convert.ToInt32(NGUOI_LAP);
                }
                else
                {
                    KH_TUONGTAC.NGUOILAP_ID = WebSecurity.CurrentUserId;
                }
               
                KH_TUONGTAC.ModifiedDate = DateTime.Now;
                KH_TUONGTAC.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                KH_TUONGTAC.THANG = int.Parse(model.NGAYLAP.Substring(3, 2));//DateTime.Now.Month;
                KH_TUONGTAC.NAM = int.Parse(model.NGAYLAP.Substring(6, 4));//DateTime.Now.Year;
                //string pNgay = model.NGAYTUONGTAC_TIEP.Substring(0, 10);
                //KH_TUONGTAC.NGAYTUONGTAC_TIEP = pNgay;
                KH_TUONGTAC.NGAYTUONGTAC_TIEP = model.NGAYTUONGTAC_TIEP;

                KH_TUONGTAC.GIOTUONGTAC_TIEP = model.GIOTUONGTAC_TIEP;
                var path = Helpers.Common.GetSetting("Plan");
                var filepath = System.Web.HttpContext.Current.Server.MapPath("~" + path);
                if (Request.Files["file-image"] != null)
                {
                    var file = Request.Files["file-image"];
                    if (file.ContentLength > 0)
                    {
                        FileInfo fi = new FileInfo(Server.MapPath("~" + path) + KH_TUONGTAC.HINH_ANH);
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }

                        string image_name = KH_TUONGTAC.KH_TUONGTAC_ID + "." + file.FileName.Split('.').Last();

                        bool isExists = System.IO.Directory.Exists(filepath);
                        if (!isExists)
                            System.IO.Directory.CreateDirectory(filepath);
                        file.SaveAs(filepath + image_name);
                        KH_TUONGTAC.HINH_ANH = image_name;
                    }
                }
                KH_TUONGTACRepository.UpdateKH_TUONGTAC(KH_TUONGTAC);
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
            }
            return View(model);
        }
        #endregion
        public ViewResult Index(int? Month, int? year, int? NGUOILAP, int? BranchId)
        {
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);
            BranchId = intBrandID;
            //NGUOILAP = NGUOILAP == null ? WebSecurity.CurrentUserId : NGUOILAP;
            Month = Month == null ? DateTime.Now.Month : Month;
            year = year == null ? DateTime.Now.Year : year;
            var q = KH_TUONGTACRepository.GetAllvwKH_TUONGTAC().Where(x => x.IsDeleted != true);

            DateTime d_startDate, d_endDate;

            if (Month == null && year == null)
            {
                d_startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                d_endDate = d_startDate.AddMonths(1).AddDays(-1);
                ViewBag.retDateTime = d_endDate;
                ViewBag.aDateTime = d_startDate;
            }

            if (Month != null)
            {
                if (year != null)
                {
                    d_startDate = new DateTime(year.Value, Month.Value, 1);
                    d_endDate = d_startDate.AddMonths(1).AddDays(-1);
                    ViewBag.retDateTime = d_endDate;
                    ViewBag.aDateTime = d_startDate;
                    q = q.Where(x => x.THANG == Month && x.NAM == year);
                }
            }


            //begin hoapd loc ra nhan vien ma minh quan ly
            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = (BranchId == null ? 0 : BranchId),
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }


            var user = userRepository.GetAllUsers().Select(item => new UserViewModel
            {
                Id = item.Id,
                UserName = item.UserName,
                FullName = item.FullName,
                Status = item.Status,
                BranchId = item.BranchId
            }).Where(x => x.Status == UserStatus.Active && listNhanvien.Contains(x.Id)).ToList();

            //var cus= KH_TUONGTACRepository.GetAllKH_TUONGTAC().Select(item => new KH_TUONGTACViewModel
            //{
            //    KH_TUONGTAC_ID = item.KH_TUONGTAC_ID,

            //    KHACHHANG_ID=item.KHACHHANG_ID

            //}).Where(x => x.THANG==Month && x.NAM==year).ToList();
            //ViewBag.Customer = cus;
            var customer = KH_TUONGTACRepository.GetAllKH_TUONGTAC().Select(item => new KH_TUONGTACViewModel
            {
                KH_TUONGTAC_ID = item.KH_TUONGTAC_ID,
                THANG = item.THANG,
                NAM = item.NAM,
                NGUOILAP_ID = item.NGUOILAP_ID,
                KHACHHANG_ID = item.KHACHHANG_ID,
                KETQUA_SAUTUONGTAC = item.KETQUA_SAUTUONGTAC,
                NGAYLAP = item.NGAYLAP,
            }).Where(x => x.THANG == Month && x.NAM == year && listNhanvien.Contains(x.NGUOILAP_ID.Value)).ToList();
            //ViewBag.Customer = customer;

            //dùng cho tài khoản dùng chung
            var user2branch = user.Where(x => x.Id == WebSecurity.CurrentUserId).FirstOrDefault();
            if (BranchId != null)
            {
                q = q.Where(x => x.BranchId == BranchId);
                user = user.Where(x => x.BranchId == BranchId).ToList();
                if (user2branch != null && user2branch.BranchId == 0)
                {
                    user.Add(user2branch);
                }
            }
            if (NGUOILAP != null && NGUOILAP > 0)
            {
                q = q.Where(x => x.NGUOILAP_ID == NGUOILAP);
                user = user.Where(x => x.Id == NGUOILAP).ToList();
            }
            if (NGUOILAP == null)
            {
                user = user.ToList();
            }
            var model = q.Select(item => new KH_TUONGTACViewModel
            {
                KH_TUONGTAC_ID = item.KH_TUONGTAC_ID,
                NGAYLAP = item.NGAYLAP,
                HINHTHUC_TUONGTAC = item.HINHTHUC_TUONGTAC,
                LOAI_TUONGTAC = item.LOAI_TUONGTAC,
                PHANLOAI_TUONGTAC = item.PHANLOAI_TUONGTAC,
                TINHTRANG_TUONGTAC = item.TINHTRANG_TUONGTAC,
                MUCDO_TUONGTAC = item.MUCDO_TUONGTAC,
                GHI_CHU = item.GHI_CHU,
                GIAIPHAP_TUONGTAC = item.GIAIPHAP_TUONGTAC,
                NGAYTUONGTAC_TIEP = item.NGAYTUONGTAC_TIEP,
                GIOTUONGTAC_TIEP = item.GIOTUONGTAC_TIEP,
                MUCCANHBAO_TUONGTAC = item.MUCCANHBAO_TUONGTAC,
                HINH_ANH = item.HINH_ANH,
                BranchId = item.BranchId,
                FullName = item.FullName,
                GIO_TUONGTAC = item.GIO_TUONGTAC,
                NGUOILAP_ID = item.NGUOILAP_ID,

                //THANG = item.THANG,
                //NAM = item.NAM,
                ModifiedDate = item.ModifiedDate,
                CreatedDate = item.CreatedDate,
                //LICHTUONGTATIEP = item.NGAYTUONGTAC_TIEP + "/" + item.THANG + "/" + item.NAM + " " + item.GIOTUONGTAC_TIEP,
            }).ToList();
            ViewBag.Customer = customer;

            ViewBag.user = user;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(model);
        }

        #region ExportExcel
        public List<vwPlanuseSkinCareViewModel> IndexExportPlanUseSkinCare(string CustomerCodeOrName, string ProductName, int? BranchId, string StartDate, string EndDate, string THANHTOAN, int? PhieuConTu, int? PhieuConDen, int? ManagerStaffId)
        {
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);
            BranchId = intBrandID;
            //if (ManagerStaffId == null || ManagerStaffId == 0)
            //{
            //   // ManagerStaffId = WebSecurity.CurrentUserId;
            //    return View(q);
            //}
            var q = PlanRepository.GetvwAllvwPlanuseSkinCare()
                 .Select(item => new vwPlanuseSkinCareViewModel
                 {
                     BranchId = item.BranchId,
                     CustomerCode = item.CustomerCode,
                     CustomerName = item.CustomerName,
                     Phone = item.Phone,
                     ProductInvoiceCode = item.ProductInvoiceCode,
                     ProductName = item.ProductName,
                     SOLUONG = item.SOLUONG,
                     soluongdung = item.soluongdung,
                     soluongtra = item.soluongtra,
                     soluongchuyen = item.soluongchuyen,
                     soluongconlai = item.soluongconlai,
                     Type = item.Type,
                     ModifiedDate = item.ModifiedDate,
                     GladLevel = item.GladLevel,
                     TargetId = item.TargetId,
                     CreatedDate = item.CreatedDate,
                     ProductInvoiceId = item.ProductInvoiceId,
                     THANGTOANHET = item.THANGTOANHET,
                     CustomerId = item.CustomerId,
                     ManagerStaffId = item.ManagerStaffId,
                     ManagerName = item.ManagerName
                 }).ToList();

            if (BranchId != null && BranchId > 0)
            {
                q = q.Where(x => x.BranchId == BranchId).ToList();
            }

            if (!string.IsNullOrEmpty(CustomerCodeOrName))
            {
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerCode).Contains(Helpers.Common.ChuyenThanhKhongDau(CustomerCodeOrName)) || x.CustomerName.Contains(CustomerCodeOrName)).ToList();
            }
            //if (!string.IsNullOrEmpty(CustomerName))
            //{
            //    //CustomerName = Helpers.Common.ChuyenThanhKhongDau(CustomerName);
            //    //q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(CustomerName));
            //}
            //if (!string.IsNullOrEmpty(CustomerCode))
            //{
            //    q = q.Where(x => x.CustomerCode.Contains(CustomerCode));
            //}

            if (!string.IsNullOrEmpty(ProductName))
            {
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.ProductName).Contains(Helpers.Common.ChuyenThanhKhongDau(ProductName))).ToList();
            }

            if (ManagerStaffId != null && ManagerStaffId > 0)
            {

                q = q.Where(x => x.ManagerStaffId == ManagerStaffId).ToList();
            }

            if (PhieuConTu != null)
            {
                if (PhieuConDen != null)
                {
                    q = q.Where(x => x.soluongconlai >= PhieuConTu && x.soluongconlai <= PhieuConDen).ToList();
                }
            }

            if (!string.IsNullOrEmpty(THANHTOAN))
            {
                THANHTOAN = Helpers.Common.ChuyenThanhKhongDau(THANHTOAN);
                if (THANHTOAN == "het")
                {
                    //THANHTOAN = null;
                    q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.THANGTOANHET).Contains(THANHTOAN)).ToList();
                }
                else
                {
                    q = q.Where(x => x.THANGTOANHET == null).ToList();
                }
            }
            //Lọc theo ngày

            if (!string.IsNullOrEmpty(StartDate))
            {
                var startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                DateTime d_startDate = Convert.ToDateTime(startDate);
                if (!string.IsNullOrEmpty(EndDate))
                {
                    var endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
                    DateTime d_endDate = Convert.ToDateTime(endDate);
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.ModifiedDate >= d_startDate && x.ModifiedDate <= d_endDate).ToList();

                }
            }
            ///
            if (StartDate == null && EndDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");

                DateTime d_startDate1, d_endDate1;
                if (DateTime.TryParseExact(StartDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate1))
                {
                    if (DateTime.TryParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate1))
                    {
                        d_endDate1 = d_endDate1.AddHours(23).AddMinutes(59);
                        //ViewBag.retDateTime = d_endDate;
                        //ViewBag.aDateTime = d_startDate;
                        q = q.Where(x => x.ModifiedDate >= d_startDate1 && x.ModifiedDate <= d_endDate1).ToList();
                    }
                }
            }

            return q;
        }

        public ActionResult ExportPlanUseSkinCare(string CustomerCodeOrName, string ProductName, int? BranchId, string StartDate, string EndDate, string THANHTOAN, int? PhieuConTu, int? PhieuConDen, int? ManagerStaffId, bool ExportExcel = false)
        {
            var data = IndexExportPlanUseSkinCare(CustomerCodeOrName, ProductName, BranchId, StartDate, EndDate, THANHTOAN, PhieuConTu, PhieuConDen, ManagerStaffId);

            var model = new TemplatePrintViewModel();
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            model.Content = template.Content;
            model.Content = model.Content.Replace("{DataTable}", buildHtmlPlanUseSkinCare(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Kế hoạch sử dụng phiếu CSD");

            Response.AppendHeader("content-disposition", "attachment;filename=" + "DS_KHSDPhieuCSD" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();

            return View(model);
        }

        string buildHtmlPlanUseSkinCare(List<vwPlanuseSkinCareViewModel> detailList)
        {
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th rowspan=\"2\">STT</th>";
            detailLists += "		<th rowspan=\"2\">Mã khách hàng</th>";
            detailLists += "		<th rowspan=\"2\">Tên khách hàng</th>";
            detailLists += "		<th rowspan=\"2\">Người quản lý</th>";
            detailLists += "		<th rowspan=\"2\">Điện thoại</th>";
            detailLists += "		<th colspan=\"9\">Đơn hàng</th>";
            detailLists += "		<th rowspan=\"2\">Thởi điểm chăm sóc gần nhất</th>";
            detailLists += "		<th rowspan=\"2\">Mức độ hài lòng</th>";
            detailLists += "	</tr>";

            detailLists += "	<tr>";
            detailLists += "		<th>Đơn hàng</th>";
            detailLists += "		<th>Tên dịch vụ</th>";
            detailLists += "		<th>Số lượng tổng</th>";
            detailLists += "		<th>Đã sử dụng</th>";
            detailLists += "		<th>Số lượng trả</th>";
            detailLists += "		<th>Số lượng chuyển</th>";
            detailLists += "		<th>Còn lại</th>";
            detailLists += "		<th>Thanh toán hết</th>";
            detailLists += "		<th>Loại CS</th>";
            detailLists += "	</tr>";

            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
                + "<td>" + item.CustomerCode + "</td>"
                + "<td>" + item.CustomerName + "</td>"
                + "<td>" + item.ManagerName + "</td>\r\n"
                + "<td>" + item.Phone.ToString() + "</td>\r\n"
                + "<td style=\"text-align:center\">" + item.ProductInvoiceCode + "<br>" + item.CreatedDate.Value.ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td>" + item.ProductName + "</td>\r\n"
                + "<td>" + item.SOLUONG + "</td>\r\n"
                + "<td>" + item.soluongdung + "</td>\r\n"
                + "<td>" + item.soluongtra + "</td>\r\n"
                + "<td>" + item.soluongchuyen + "</td>\r\n"
                + "<td>" + item.soluongconlai + "</td>\r\n"
                + "<td>" + (item.THANGTOANHET == "Het" ? "Hết" : item.THANGTOANHET == "null" ? "Chưa có đơn hàng trong phiếu thu" : "Chưa") + "</td>\r\n"
                + "<td>" + (item.Type == "SkinScan" ? "CSD" : item.Type == "CheckingHair" ? "CST" : "") + "</td>\r\n"
                + "<td>" + (item.ModifiedDate == null ? "" : item.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm")) + "</td>\r\n"
                + "<td>" + item.GladLevel + "</td>\r\n"
                + "</tr>\r\n";
            }

            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }
        #endregion


        #region Ke Thua

        [HttpPost]
        public ActionResult ViewKeThua(string NGAYLAP, int NGUOILAP_ID)
        {
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);

            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spCopykehoachdukien", new
            {
                BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID),
                UserId = NGUOILAP_ID,
                Pngaylap = NGAYLAP,
                Month = int.Parse(NGAYLAP.Substring(3, 2)),
                year = int.Parse(NGAYLAP.Substring(6, 4))
            }).ToList();

            return RedirectToAction("Index", new { Month = int.Parse(NGAYLAP.Substring(3, 2)), year = int.Parse(NGAYLAP.Substring(6, 4)), NGUOILAP = WebSecurity.CurrentUserId, BranchId = intBrandID });

        }










        //[HttpPost]
        //[AllowAnonymous]
        //public ActionResult ViewKeThua(KH_TUONGTACViewModel model)
        //{
        //    try
        //    {
        //        if (ModelState.IsValid)
        //        {
        //            DateTime date = DateTime.Parse(model.NGAYLAP);
        //            foreach (var item in model.KH_TUONGTAC)
        //            {
        //                if (item.is_checked == 1)
        //                {
        //                    var KH_TUONGTAC = new KH_TUONGTAC();
        //                    //AutoMapper.Mapper.Map(model.KH_TUONGTAC, KH_TUONGTAC);
        //                    KH_TUONGTAC.IsDeleted = false;
        //                    KH_TUONGTAC.CreatedUserId = WebSecurity.CurrentUserId;
        //                    KH_TUONGTAC.CreatedDate = DateTime.Now;
        //                    KH_TUONGTAC.ModifiedDate = DateTime.Now;
        //                    KH_TUONGTAC.NGUOILAP_ID = WebSecurity.CurrentUserId;
        //                    KH_TUONGTAC.GIO_TUONGTAC = item.GIO_TUONGTAC;
        //                    KH_TUONGTAC.NGAYLAP = date.ToString("dd/MM/yyyy");
        //                    KH_TUONGTAC.THANG = int.Parse(item.LICHTUONGTATIEP.Substring(3, 1));
        //                    KH_TUONGTAC.NAM = int.Parse(item.LICHTUONGTATIEP.Substring(5, 4));
        //                    KH_TUONGTAC.NGAYTUONGTAC_TIEP = item.LICHTUONGTATIEP.Substring(0, 2);
        //                    KH_TUONGTAC.GIOTUONGTAC_TIEP = item.LICHTUONGTATIEP.Substring(10, 4);
        //                    KH_TUONGTAC.BranchId = Erp.BackOffice.Helpers.Common.CurrentUser.BranchId;
        //                    KH_TUONGTAC.KHACHHANG_ID = item.CustomerId;
        //                    KH_TUONGTAC.HINHTHUC_TUONGTAC = item.HINHTHUC_TUONGTAC;
        //                    KH_TUONGTAC.GIO_TUONGTAC = item.GIO_TUONGTAC;
        //                    KH_TUONGTAC.LOAI_TUONGTAC = item.LOAI_TUONGTAC;
        //                    KH_TUONGTAC.PHANLOAI_TUONGTAC = item.PHANLOAI_TUONGTAC;
        //                    KH_TUONGTAC.TINHTRANG_TUONGTAC = item.TINHTRANG_TUONGTAC;
        //                    KH_TUONGTAC.MUCDO_TUONGTAC = item.MUCDO_TUONGTAC;
        //                    KH_TUONGTAC.GIAIPHAP_TUONGTAC = item.GIAIPHAP_TUONGTAC;
        //                    KH_TUONGTAC.MUCCANHBAO_TUONGTAC = item.MUCCANHBAO_TUONGTAC;
        //                    KH_TUONGTAC.GHI_CHU = item.GHI_CHU;
        //                    KH_TUONGTAC.HINH_ANH = item.HINH_ANH;

        //                    var path = System.Web.HttpContext.Current.Server.MapPath("~" + Helpers.Common.GetSetting("product-image-folder"));
        //                    if (Request.Files["file-image"] != null)
        //                    {
        //                        var file = Request.Files["file-image"];
        //                        if (file.ContentLength > 0)
        //                        {
        //                            string image_name = "KH_TUONGTAC_" + Helpers.Common.ChuyenThanhKhongDau(Regex.Replace(KH_TUONGTAC.TINHTRANG_TUONGTAC, @"\s+", "_")) + "." + file.FileName.Split('.').Last();
        //                            bool isExists = System.IO.Directory.Exists(path);
        //                            if (!isExists)
        //                                System.IO.Directory.CreateDirectory(path);
        //                            file.SaveAs(path + image_name);
        //                            KH_TUONGTAC.HINH_ANH = image_name;
        //                        }
        //                    }
        //                    KH_TUONGTACRepository.InsertKH_TUONGTAC(KH_TUONGTAC);

        //                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
        //                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
        //                }
        //            }

        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //    }

        //    return View();
        //}
        #endregion
        #region LichSuTuongTac
        public ViewResult LichSuTuongTac(int? NGUOILAP_ID, int? KH_TUONGTAC_ID, string CustomerName, string NGAYLAP, int KHACHHANG_ID)
        {
            var data = GETKH_TUONGTAC().Where(x => x.KHACHHANG_ID == KHACHHANG_ID && x.NGUOILAP_ID == NGUOILAP_ID);
            ViewBag.KH_TUONGTAC = data;
            return View();
        }
        #endregion
        #region ThongKeKH
        public ActionResult ThongKeKH(int? branchId, string StartDate, string EndDate)
        {
            branchId = branchId == null ? 0 : branchId;

            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var data = SqlHelper.QuerySP<InquiryCardViewModel>("spSale_ThongKeKH", new
            {
                branchId = branchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
            }).ToList();
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            data = data.GroupBy(x => x.CustomerId).Select(x => x.First()).OrderByDescending(x => x.CreatedDate).ToList();
            return View(data);
        }
        #endregion

        public ViewResult KH_TuongTac(string StartDate, string EndDate, int? userID)
        {
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";
            if (request.Cookies["BRANCH_ID_SPA_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_SPA_CookieName"].Value;
                if (strBrandID == "")
                {
                    strBrandID = "0";
                }
            }

            //get  CurrentUser.branchId

            if ((Helpers.Common.CurrentUser.BranchId != null) && (Helpers.Common.CurrentUser.BranchId > 0))
            {
                strBrandID = Helpers.Common.CurrentUser.BranchId.ToString();
            }

            int? intBrandID = int.Parse(strBrandID);

            if (StartDate == null && EndDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }


            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = intBrandID,
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }

            var user = userRepository.GetAllUsers().Select(item => new UserViewModel
            {
                Id = item.Id,
                UserName = item.UserName,
                FullName = item.FullName,
                Status = item.Status,
                BranchId = item.BranchId
            }).Where(x => x.Status == UserStatus.Active).ToList();

            ViewBag.user = user.Where(x => listNhanvien.Contains(x.Id));



            var use = KH_TUONGTACRepository.GetAllvwCRM_TK_TUONGTAC().Select(item => new KH_TUONGTACViewModel
            {
                NGUOI_LAP = item.NGUOI_LAP,
                //NgayLap = Convert.ToDateTime(item.NGAYLAP),
                NGAYLAP = item.NGAYLAP,
                TONG_QL = item.TONG_QL,
                NGUOILAP_ID = item.NGUOILAP_ID,
                TONG_PLAN = item.TONG_PLAN,
                SOTUONGTAC = item.SOTUONGTAC,
                SO_QUAHAN = item.SO_QUAHAN,
                CHUA_PLAN = item.CHUA_PLAN,
                CHUA_PLAN_NEXT = item.CHUA_PLAN_NEXT
            }).ToList();

            use = use.Where(x => listNhanvien.Contains(x.NGUOILAP_ID.Value)).ToList();

            foreach(var item in use)
            {
                item.Ngay_Lap = Convert.ToDateTime(item.NGAYLAP);
            }
            use = use.OrderByDescending(x => x.Ngay_Lap).ToList();

            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(StartDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(EndDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    use = use.Where(x => Convert.ToDateTime(x.NGAYLAP) >= d_startDate && Convert.ToDateTime(x.NGAYLAP) <= d_endDate).ToList();
                }
            }

            if (userID != null)
            {
                use = use.Where(x => x.NGUOILAP_ID == userID).ToList();
            }
           
            return View(use);


        }

    }
}
