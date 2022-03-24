using Erp.BackOffice.Filters;
using Erp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using WebMatrix.WebData;
using Erp.Domain.Crm.Interfaces;
using Erp.BackOffice.Areas.Cms.Models;
using Erp.BackOffice.Crm.Models;
using Erp.Domain.Crm.Entities;
using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Account.Models;
using Erp.Domain.Entities;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Data.Entity.Infrastructure;
using System.Text;
using Erp.BackOffice.Helpers;
using System.Transactions;
using System.Text.RegularExpressions;
using Erp.Domain.Account.Interfaces;
using Erp.Utilities;
using System.Data.Entity.Validation;
using PagedList;
using PagedList.Mvc;
using Erp.BackOffice.Areas.Administration.Models;
using Erp.BackOffice.App_GlobalResources;
using Erp.BackOffice.Account.Controllers;
using Erp.Domain.Account.Helper;
using System.Data.Entity;
using System.Web;
using System.Data;

namespace Erp.BackOffice.Crm.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class CRM_KH_BANHANGController : Controller
    {
        private readonly ICRM_KH_GIOITHIEURepository CRM_KH_GIOITHIEURepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly ICustomerRepository CustomerRepository;
        private readonly IUserRepository userRepository;
        private readonly ICRM_KH_BANHANGRepository CRM_KH_BANHANGRepository;
        private readonly ICRM_KH_BANHANG_CTIETRepository CRM_KH_BANHANG_CTIETRepository;
        private readonly IProductInvoiceRepository ProductInvoiceRepository;
        private readonly ITransactionLiabilitiesRepository transactionLiabilitiesRepository;
        private readonly IKH_TUONGTACRepository kH_TUONGTACRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        public CRM_KH_BANHANGController(
            ICRM_KH_BANHANGRepository _CRM_KH_BANHANG,
            ICategoryRepository _category,
            ICustomerRepository _Customer,
            IUserRepository _userRepository,
            ICRM_KH_BANHANG_CTIETRepository _CRM_KH_BANHANG_CTIET,
            IProductInvoiceRepository _ProductInvoiceRepository,
            ITransactionLiabilitiesRepository _transactionLiabilities,
            IKH_TUONGTACRepository _kH_TUONGTAC,
            ITemplatePrintRepository _templatePrintRepository
            )
        {
            CRM_KH_BANHANGRepository = _CRM_KH_BANHANG;
            categoryRepository = _category;
            CustomerRepository = _Customer;
            userRepository = _userRepository;
            CRM_KH_BANHANG_CTIETRepository = _CRM_KH_BANHANG_CTIET;
            ProductInvoiceRepository = _ProductInvoiceRepository;
            transactionLiabilitiesRepository = _transactionLiabilities;
            kH_TUONGTACRepository = _kH_TUONGTAC;
            templatePrintRepository = _templatePrintRepository;

        }
        #region Index

        public ViewResult Index(int? branchId, int? Month, int? year, int? NGUOILAP, int? checkSPSHH, string IsTHucte)
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
            branchId = intBrandID;
            int? GlobalCurentBranchId = Convert.ToInt32(Session["GlobalCurentBranchId"]);
            //branchId = (Erp.BackOffice.Helpers.Common.CurrentUser.BranchId == null ? GlobalCurentBranchId : Erp.BackOffice.Helpers.Common.CurrentUser.BranchId);
            year = year == null ? DateTime.Now.Year : year;
            Month = Month == null ? DateTime.Now.Month : Month;
            //NGUOILAP = NGUOILAP == null ? WebSecurity.CurrentUserId : NGUOILAP;
            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID),
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }

            ViewBag.KHsumANNA = 0;
            ViewBag.KHsumCNC = 0;
            ViewBag.KHsumLEO = 0;
            ViewBag.KHsumDV = 0;
            ViewBag.KHsumOP = 0;




            var q = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG();
            var user = userRepository.GetAllUsers().Select(item => new UserViewModel
            {
                Id = item.Id,
                UserName = item.UserName,
                FullName = item.FullName,
                Status = item.Status,
                BranchId = item.BranchId,
                IdManager = item.IdManager
            }).Where(x => x.Status == UserStatus.Active && listNhanvien.Contains(x.Id)).ToList();
            ViewBag.user2 = user;

            var user2branch = user.Where(x => x.Id == WebSecurity.CurrentUserId).FirstOrDefault();
            //if(listNhanvien.Count()>0)
            //{
            //    user = user.Where(x => listNhanvien.Contains(x.Id)).ToList();
            //}
            if (NGUOILAP != null && NGUOILAP > 0)
            {
                q = q.Where(x => x.NGUOILAP_ID == NGUOILAP);
                user = user.Where(x => x.Id == NGUOILAP).ToList();
            }

            //if (NGUOILAP == null)
            //{
            //    //q = q;
            //    user = user.ToList();
            //}
            //if (!Erp.BackOffice.Filters.SecurityFilter.IsAdmin())
            //{
            //    user = user.Where(x => x.Id == WebSecurity.CurrentUserId).ToList();
            //}
            if (branchId != null && branchId > 0)
            {
                user = user.Where(x => x.BranchId == branchId).ToList();
                if (user2branch != null && user2branch.BranchId == 0)
                {
                    user.Add(user2branch);
                }
                q = q.Where(x => x.BranchId == branchId);
            }

            if (Month != null)
            {
                if (year != null)
                {
                    q = q.Where(x => x.THANG == Month && x.NAM == year);
                }
            }

            var origin = categoryRepository.GetAllCategories().Select(item => new CategoryViewModel
            {
                Name = item.Name,
                Value = item.Value,
                Code = item.Code
            }).Where(item => item.Code == "Origin");
            ViewBag.Origin = origin;
            if (IsTHucte == "on")
            {
                var dataTongThucTe = SqlHelper.QuerySP<CRM_KH_BANHANGViewModel>("CrmListTongThucTe", new
                {
                    THANG = Month,
                    NAM = year,
                }).ToList();
                //ViewBag.BANHANG_CTIET_ProductInvoice = dataTongThucTe;
                dataTongThucTe = dataTongThucTe.Where(x => listNhanvien.Contains(x.NGUOILAP_ID.Value)).ToList();
                ViewBag.productInvoice = dataTongThucTe;



                if (user != null)
                {
                    foreach (var s in dataTongThucTe)
                    {
                        var name = "ANNAYAKE";
                        var name2 = "CONGNGHECAO";
                        var name3 = "LEONOR GREYL";
                        var name4 = "DICHVU";
                        var name5 = "ORLANE PARIS";

                        if (s.TotalAmount != 0)
                        {
                            if (s.CountForBrand.Equals(name))
                            {
                                var sum = s.TotalAmount;
                                ViewBag.KHsumANNA += sum;
                            }
                            if (s.CountForBrand.Equals(name2))
                            {
                                var sum = s.TotalAmount;
                                ViewBag.KHsumCNC += sum;
                            }
                            if (s.CountForBrand.Equals(name3))
                            {
                                var sum = s.TotalAmount;
                                ViewBag.KHsumLEO += sum;
                            }
                            if (s.CountForBrand.Equals(name4))
                            {
                                var sum = s.TotalAmount;
                                ViewBag.KHsumDV += sum;
                            }
                            if (s.CountForBrand.Equals(name5))
                            {
                                var sum = s.TotalAmount;
                                ViewBag.KHsumOP += sum;
                            }
                        }
                    }
                }
            }



            List<CRM_KH_BANHANG_CTIETViewModel> CrmProductInvoice = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Select(x => new CRM_KH_BANHANG_CTIETViewModel
            {
                KH_BANHANG_ID = x.KH_BANHANG_ID,
                KH_BANHANG_CTIET_ID = x.KH_BANHANG_CTIET_ID,
                Phone = x.Phone,
                KHACHHANG_ID = x.KHACHHANG_ID,
                CompanyName = x.CompanyName,
                Code = x.Code,
                TYLE_THANHCONG = x.TYLE_THANHCONG,
                TYLE_THANHCONG_REVIEW = x.TYLE_THANHCONG_REVIEW,
                GHI_CHU = x.GHI_CHU,
                IsDeleted = x.IsDeleted,
                THANG = x.THANG,
                NAM = x.NAM,
                CRM_Sale_ProductInvoiceCode = x.CRM_Sale_ProductInvoiceCode,
                CountForBrand = x.CountForBrand,
                TotalAmount = x.TotalAmount,
                ProductInvoiceId = x.ProductInvoiceId,
                NGUOILAP_ID = x.NGUOILAP_ID,
                NOIDUNG = x.NOIDUNG,
            }).Where(x => x.THANG == Month && x.NAM == year && listNhanvien.Contains(x.NGUOILAP_ID.Value)).ToList();

            ViewBag.CrmProductInvoice = CrmProductInvoice;

            //IEnumerable<ProductInvoiceViewModel> CrmProductInvoice = CRM_KH_BANHANGRepository.GetAllvwCRM_Sale_ProductInvoice().Select(item => new ProductInvoiceViewModel
            //{
            //    Id = item.Id,
            //    ManagerStaffId = item.ManagerStaffId,
            //    CountForBrand = item.CountForBrand,
            //    TotalAmount = item.TotalAmount,
            //    IsArchive = item.IsArchive,
            //    CreatedDate = item.CreatedDate,
            //    Month = item.THANG,
            //    Year = item.NAM,
            //}).Where(x => x.Month == Month && x.Year == year).ToList();
            //ViewBag.CrmProductInvoice = CrmProductInvoice;



            //IEnumerable<ProductInvoiceViewModel> productInvoice = ProductInvoiceRepository.GetAllvwProductInvoice().Select(item => new ProductInvoiceViewModel
            //{
            //    Id = item.Id,
            //    ManagerStaffId = item.ManagerStaffId,
            //    CountForBrand = item.CountForBrand,
            //    TotalAmount = item.TotalAmount,
            //    IsArchive=item.IsArchive,
            //    CreatedDate=item.CreatedDate,
            //    Month=item.CreatedDate.Value.Month,
            //    Year=item.CreatedDate.Value.Year,
            //    TongConNo=item.TotalDebit-item.TotalCredit,
            //}).Where(x => x.IsArchive == true && x.Month == Month && x.Year == year && x.TongConNo==0).ToList();

            var model = q.Select(item => new CRM_KH_BANHANGViewModel
            {
                KH_BANHANG_ID = item.KH_BANHANG_ID,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                NAM = item.NAM,
                THANG = item.THANG,
                NGUOILAP_ID = item.NGUOILAP_ID,
                CountForBrand = item.CountForBrand,
                TARGET_BRAND = item.TARGET_BRAND,
                GHI_CHU = item.GHI_CHU,
                Name = item.Name,
                FullName = item.FullName,
                BranchId = item.BranchId
            }).Where(x => x.NAM == year && x.THANG == Month && listNhanvien.Contains(x.NGUOILAP_ID.Value)).ToList();

            //if (listNhanvien.Count() > 0)
            //{
            //    model = model.Where(x => listNhanvien.Contains(x.NGUOILAP_ID.Value)).ToList();
            //}

            ViewBag.TarsumANNA = 0;
            ViewBag.TarsumCNC = 0;
            ViewBag.TarsumLEO = 0;
            ViewBag.TarsumDV = 0;
            ViewBag.TarsumOP = 0;
            //


            if (user != null)
            {
                foreach (var s in model)
                {
                    var name = "ANNAYAKE";
                    var name2 = "CONGNGHECAO";
                    var name3 = "LEONOR GREYL";
                    var name4 = "DICHVU";
                    var name5 = "ORLANE PARIS";

                    if (s.TARGET_BRAND != 0)
                    {
                        if (s.CountForBrand.Equals(name))
                        {
                            var sum = s.TARGET_BRAND;
                            ViewBag.TarsumANNA += sum;
                        }
                        if (s.CountForBrand.Equals(name2))
                        {
                            var sum = s.TARGET_BRAND;
                            ViewBag.TarsumCNC += sum;
                        }
                        if (s.CountForBrand.Equals(name3))
                        {
                            var sum = s.TARGET_BRAND;
                            ViewBag.TarsumLEO += sum;
                        }
                        if (s.CountForBrand.Equals(name4))
                        {
                            var sum = s.TARGET_BRAND;
                            ViewBag.TarsumDV += sum;
                        }
                        if (s.CountForBrand.Equals(name5))
                        {
                            var sum = s.TARGET_BRAND;
                            ViewBag.TarsumOP += sum;
                        }
                    }
                }
            }



            ViewBag.user = user;
            ViewBag.NGUOILAP = WebSecurity.CurrentUserId;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            ViewBag.AdviseTypeError = TempData["AdviseTypeError"];
            return View(model);
        }
        #endregion


        #region Create
        public ActionResult Create(int id, int month, int year)
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


            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID),
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }
            var user2 = userRepository.GetAllUsers().Select(item => new UserViewModel
            {
                Id = item.Id,
                UserName = item.UserName,
                FullName = item.FullName,
                Status = item.Status,
                BranchId = item.BranchId,
                IdManager = item.IdManager
            }).Where(x => x.Status == UserStatus.Active && listNhanvien.Contains(x.Id)).ToList();
            ViewBag.user = user2;

            var model = new CRM_KH_BANHANGViewModel();
            IEnumerable<CRM_KH_BANHANGViewModel> q = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG()
                            .Select(item => new CRM_KH_BANHANGViewModel
                            {
                                KH_BANHANG_ID = item.KH_BANHANG_ID,
                                BranchId = item.BranchId,
                                IsDeleted = item.IsDeleted,
                                CreatedDate = item.CreatedDate,
                                CreatedUserId = item.CreatedUserId,
                                ModifiedDate = item.ModifiedDate,
                                ModifiedUserId = item.ModifiedUserId,
                                NGUOILAP_ID = item.NGUOILAP_ID,
                                TARGET_BRAND = item.TARGET_BRAND,
                                GHI_CHU = item.GHI_CHU,
                                CountForBrand = item.CountForBrand,
                                THANG = item.THANG,
                                NAM = item.NAM,
                            }).Where(x => x.NGUOILAP_ID == id && x.IsDeleted == false && x.THANG == month && x.NAM == year).ToList();

            foreach (var item in q)
            {
                model.BranchId = item.BranchId;
                model.IsDeleted = item.IsDeleted;
                model.CreatedDate = item.CreatedDate;
                model.CreatedUserId = item.CreatedUserId;
                model.ModifiedDate = item.ModifiedDate;
                model.ModifiedUserId = item.ModifiedUserId;
                model.CountForBrand = item.CountForBrand;
                model.GHI_CHU = item.GHI_CHU;
                if (item.CountForBrand == "ANNAYAKE")
                {
                    model.ID_ANNAYAKE = item.KH_BANHANG_ID;
                    model.TARGET_BRAND = item.TARGET_BRAND;
                    ViewBag.GHICHU_ANNAYAKE = item.GHI_CHU;
                    ViewBag.ID_ANNAYAKE = item.KH_BANHANG_ID;
                    ViewBag.ANNAYAKE = item.TARGET_BRAND;
                }

                if (item.CountForBrand.Replace(" ", "") == "ORLANEPARIS")
                {
                    model.ID_ORLANEPARIS = item.KH_BANHANG_ID;
                    ViewBag.GHICHU_ORLANEPARIS = item.GHI_CHU;
                    ViewBag.ID_ORLANEPARIS = item.KH_BANHANG_ID;
                    ViewBag.ORLANEPARIS = item.TARGET_BRAND;
                }

                if (item.CountForBrand.Replace(" ", "") == "LEONORGREYL")
                {
                    model.ID_LEONORGREYL = item.KH_BANHANG_ID;
                    ViewBag.GHICHU_LEONORGREYL = item.GHI_CHU;
                    ViewBag.ID_LEONORGREYL = item.KH_BANHANG_ID;
                    ViewBag.LEONORGREYL = item.TARGET_BRAND;
                }

                if (item.CountForBrand.Replace(" ", "") == "CONGNGHECAO")
                {
                    model.ID_CONGNGHECAO = item.KH_BANHANG_ID;
                    ViewBag.GHICHU_CNC = item.GHI_CHU;
                    ViewBag.ID_CNC = item.KH_BANHANG_ID;
                    ViewBag.CNC = item.TARGET_BRAND;
                }


                if (item.CountForBrand.Replace(" ", "") == "DICHVU")
                {
                    model.ID_DICHVU = item.KH_BANHANG_ID;
                    ViewBag.GHICHU_DV = item.GHI_CHU;
                    ViewBag.ID_DV = item.KH_BANHANG_ID;
                    ViewBag.DV = item.TARGET_BRAND;
                }





            }

            if (id != null)
            {
                var user = userRepository.GetUserById(id);
                model.NGUOILAP_ID = user.Id;
                model.FullName = user.FullName;
            }
            else
            {
                var user = userRepository.GetUserById(WebSecurity.CurrentUserId);
                model.NGUOILAP_ID = user.Id;
                model.FullName = user.FullName;
            }
            model.CRM_KH_BANHANGList = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Select(x => new CRM_KH_BANHANG_CTIETViewModel
            {
                KH_BANHANG_ID = x.KH_BANHANG_ID,
                KH_BANHANG_CTIET_ID = x.KH_BANHANG_CTIET_ID,
                Phone = x.Phone,
                KHACHHANG_ID = x.KHACHHANG_ID,
                CompanyName = x.CompanyName,
                Code = x.Code,
                TYLE_THANHCONG = x.TYLE_THANHCONG,
                TYLE_THANHCONG_REVIEW = x.TYLE_THANHCONG_REVIEW,
                GHI_CHU = x.GHI_CHU,
                IsDeleted = x.IsDeleted,
                THANG = x.THANG,
                NAM = x.NAM,
                CRM_Sale_ProductInvoiceCode = x.CRM_Sale_ProductInvoiceCode,
                CountForBrand = x.CountForBrand,
                TotalAmount = x.TotalAmount,
                ProductInvoiceId = x.ProductInvoiceId,
                NGUOILAP_ID = x.NGUOILAP_ID,
                NOIDUNG = x.NOIDUNG,
            }).Where(x => x.IsDeleted == false && x.THANG == month && x.NAM == year && x.NGUOILAP_ID == id).ToList();
            foreach (var item in model.CRM_KH_BANHANGList)
            {
                model.KHACHHANG_ID = item.KHACHHANG_ID.Value;
            }
            ViewBag.vwCRM_KH_BANHANG = model.CRM_KH_BANHANGList;
            ViewBag.model = model;
            var ProductInvoice = ProductInvoiceRepository.GetAllvwProductInvoice().Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                ManagerStaffId = item.ManagerStaffId,
                CountForBrand = item.CountForBrand,
                TotalAmount = item.TotalAmount,
                Code = item.Code,
                CustomerId = item.CustomerId,
                CreatedDate = item.CreatedDate,
                Month = item.CreatedDate.Value.Month,
                Year = item.CreatedDate.Value.Year,
                IsArchive = item.IsArchive,
                TotalDebit = item.TotalDebit,
                TotalCredit = item.TotalCredit,
                Status = item.Status

            }).Where(x => x.Month == month && x.Year == year && x.IsArchive == true && x.Status == "complete").ToList();
            ViewBag.ProductInvoice = ProductInvoice;
            //var BANHANG_CTIET_ProductInvoice = SqlHelper.QuerySP<CRM_KH_BANHANGViewModel>("spCrm_BANHANG_CTIET_ProductInvoice", new
            //{
            //    Month=month,
            //    Year=year,
            //    ManagerStaffId=id
            //}).ToList();
            var dataTongThucTe = SqlHelper.QuerySP<CRM_KH_BANHANGViewModel>("CrmListTongThucTeByNGUOILAP_ID", new
            {
                THANG = month,
                NAM = year,
                NGUOILAP_ID = id
            }).ToList();
            ViewBag.BANHANG_CTIET_ProductInvoice = dataTongThucTe;
            IEnumerable<KH_TUONGTACViewModel> KH_TUONGTAC = kH_TUONGTACRepository.GetAllvwKH_TUONGTAC()
                  .Select(item => new KH_TUONGTACViewModel
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
                      ModifiedDate = item.ModifiedDate,
                      CreatedDate = item.CreatedDate,
                      GIO_TUONGTAC = item.GIO_TUONGTAC,
                      NGUOILAP_ID = item.NGUOILAP_ID,
                      KHACHHANG_ID = item.KHACHHANG_ID,
                      KETQUA_SAUTUONGTAC = item.KETQUA_SAUTUONGTAC

                  });


            ViewBag.LICHSUTUONGTAC = KH_TUONGTAC;
            var origin = categoryRepository.GetAllCategories().Select(item => new CategoryViewModel
            {
                Name = item.Name,
                Value = item.Value,
                Code = item.Code
            }).Where(item => item.Code == "Origin").ToList();
            ViewBag.Origin = origin;

            ViewBag.NGUOILAP = WebSecurity.CurrentUserId;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            ViewBag.AdviseTypeError = TempData["AdviseTypeError"];
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(CRM_KH_BANHANGViewModel model)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    if (model.ID_ANNAYAKE.HasValue)
                    {
                        var CRM_KH_BANHANG_ANNAYAKE = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(model.ID_ANNAYAKE.Value);
                        CRM_KH_BANHANG_ANNAYAKE.TARGET_BRAND = model.TARGET_BRAND;
                        CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG(CRM_KH_BANHANG_ANNAYAKE);
                        model.NAM = CRM_KH_BANHANG_ANNAYAKE.NAM.Value;
                        model.THANG = CRM_KH_BANHANG_ANNAYAKE.THANG.Value;
                    }

                    if (model.ID_ORLANEPARIS.HasValue)
                    {
                        var CRM_KH_BANHANG_ORLANEPARIS = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(model.ID_ORLANEPARIS.Value);
                        CRM_KH_BANHANG_ORLANEPARIS.TARGET_BRAND = model.ORLANEPARIS;
                        CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG(CRM_KH_BANHANG_ORLANEPARIS);
                        model.NAM = CRM_KH_BANHANG_ORLANEPARIS.NAM.Value;
                        model.THANG = CRM_KH_BANHANG_ORLANEPARIS.THANG.Value;
                    }
                    if (model.ID_LEONORGREYL.HasValue)
                    {
                        var CRM_KH_BANHANG_LEONORGREYL = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(model.ID_LEONORGREYL.Value);
                        CRM_KH_BANHANG_LEONORGREYL.TARGET_BRAND = model.LEONORGREYL;
                        CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG(CRM_KH_BANHANG_LEONORGREYL);
                        model.NAM = CRM_KH_BANHANG_LEONORGREYL.NAM.Value;
                        model.THANG = CRM_KH_BANHANG_LEONORGREYL.THANG.Value;
                    }
                    if (model.ID_CONGNGHECAO.HasValue)
                    {
                        var CRM_KH_BANHANG_CONGNGHECAO = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(model.ID_CONGNGHECAO.Value);
                        CRM_KH_BANHANG_CONGNGHECAO.TARGET_BRAND = model.CONGNGHECAO;
                        CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG(CRM_KH_BANHANG_CONGNGHECAO);
                        model.NAM = CRM_KH_BANHANG_CONGNGHECAO.NAM.Value;
                        model.THANG = CRM_KH_BANHANG_CONGNGHECAO.THANG.Value;
                    }
                    if (model.ID_DICHVU.HasValue)
                    {
                        var CRM_KH_BANHANG_DICHVU = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(model.ID_DICHVU.Value);
                        CRM_KH_BANHANG_DICHVU.TARGET_BRAND = model.DICHVU;
                        CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG(CRM_KH_BANHANG_DICHVU);
                        model.NAM = CRM_KH_BANHANG_DICHVU.NAM.Value;
                        model.THANG = CRM_KH_BANHANG_DICHVU.THANG.Value;
                    }


                    if (model.CRM_KH_BANHANGList != null)
                    {
                        foreach (var item in model.CRM_KH_BANHANGList)
                        {
                            var CRM_KH_BANHANG_CT = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANG_CTById(item.KH_BANHANG_CTIET_ID);
                            CRM_KH_BANHANG_CT.TYLE_THANHCONG = item.TYLE_THANHCONG;
                            CRM_KH_BANHANG_CT.TYLE_THANHCONG_REVIEW = item.TYLE_THANHCONG_REVIEW;
                            CRM_KH_BANHANG_CT.ModifiedDate = DateTime.Now;
                            CRM_KH_BANHANG_CT.ModifiedUserId = WebSecurity.CurrentUserId;
                            CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG_CT(CRM_KH_BANHANG_CT);
                        }
                    }
                    scope.Complete();
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("Create", new { Id = model.Id, month = model.THANG, year = model.NAM });
                }
                catch (DbUpdateException ex)
                {
                    TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.Error;
                    return View(model);
                }
            }

        }

        #endregion


        #region Edit
        public ActionResult Edit(int id)
        {
            var CRM_KH_BANHANG = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGByUserId(id);
            if (CRM_KH_BANHANG != null && CRM_KH_BANHANG.IsDeleted != true)
            {
                var model = new CRM_KH_BANHANGViewModel();
                AutoMapper.Mapper.Map(CRM_KH_BANHANG, model);
                model.CRM_KH_BANHANG_CTIET = new List<CRM_KH_BANHANG_CTIETViewModel>();
                model.CRM_KH_BANHANG_CTIET = CRM_KH_BANHANG_CTIETRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == model.KH_BANHANG_ID)
                    .Select(x => new CRM_KH_BANHANG_CTIETViewModel
                    {
                        KH_BANHANG_ID = x.KH_BANHANG_ID,
                        KHACHHANG_ID = x.KHACHHANG_ID,
                        TYLE_THANHCONG = x.TYLE_THANHCONG,
                        TYLE_THANHCONG_REVIEW = x.TYLE_THANHCONG_REVIEW,
                        CompanyName = x.CompanyName,
                        Phone = x.Phone,
                        Code = x.Code,
                    }).ToList();

                return View(model);
            }
            return View();
        }

        [HttpPost]
        public ActionResult Edit(CRM_KH_BANHANGViewModel model, bool? IsPopup, int? id)
        {
            if (ModelState.IsValid)
            {

                var CRM_KH_BANHANG = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(model.KH_BANHANG_ID);
                AutoMapper.Mapper.Map(model, CRM_KH_BANHANG);
                CRM_KH_BANHANG.ModifiedUserId = WebSecurity.CurrentUserId;
                CRM_KH_BANHANG.ModifiedDate = DateTime.Now;
                CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG(CRM_KH_BANHANG);

                if (model.CRM_KH_BANHANG_CTIETList.Any(x => x.Id > 0))
                {
                    //lưu danh sách thao tác thực hiện dịch vụ
                    foreach (var item in model.CRM_KH_BANHANG_CTIETList.Where(x => x.Id > 0))
                    {
                        var ins = new CRM_KH_BANHANG_CTIET();
                        ins.ModifiedUserId = WebSecurity.CurrentUserId;
                        ins.ModifiedDate = DateTime.Now;
                        ins.KHACHHANG_ID = item.Id;
                        ins.BranchId = Erp.BackOffice.Helpers.Common.CurrentUser.BranchId;
                        ins.KH_BANHANG_ID = CRM_KH_BANHANG.KH_BANHANG_ID;
                        CRM_KH_BANHANG_CTIETRepository.UpdateCRM_KH_BANHANG_CTIET(ins);

                    }
                }
                return View(model);
            }
            return View(model);
        }

        #endregion
        #region CreateProductInvoice
        public ActionResult CreateProductInvoice(int? CustomerId, string CustomerName, int? KH_BANHANG_CTIET_ID, string CountForBrand, int? Id)
        {
            var crminvoice = new CRM_Sale_ProductInvoice();

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

            ProductInvoiceViewModel model = new ProductInvoiceViewModel();
            model.InvoiceList = new List<ProductInvoiceDetailViewModel>();
            if (Id != null)
            {
                crminvoice = CRM_KH_BANHANGRepository.GetCRM_Sale_ProductInvoiceById(Id.Value);
                AutoMapper.Mapper.Map(crminvoice, model);
                CountForBrand = model.CountForBrand;
                model.InvoiceList = CRM_KH_BANHANGRepository.GetAllvwInvoiceDetailsByInvoiceId(Id.Value).Select(item => new ProductInvoiceDetailViewModel
                {
                    IsDeleted = item.IsDeleted,
                    CreatedUserId = item.CreatedUserId,
                    ModifiedUserId = item.ModifiedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedDate = item.ModifiedDate,
                    ExpiryDate = item.ExpiryDate,
                    LoCode = item.LoCode,
                    ProductId = item.ProductId.Value,
                    ProductInvoiceId = item.ProductInvoiceId,
                    Price = item.Price,
                    Quantity = item.Quantity,
                    Unit = item.Unit,
                    Discount = item.Discount,
                    DiscountAmount = item.DiscountAmount,
                    Note = item.Note,
                    is_TANG = item.is_TANG

                }).ToList();
            }
            else
            {
                var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
                model.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                model.ReceiptViewModel = new ReceiptViewModel();
                model.NextPaymentDate_Temp = DateTime.Now.AddDays(30);
                model.ReceiptViewModel.Name = "Bán hàng - Thu tiền mặt";
                model.ReceiptViewModel.Amount = 0;
                model.CountForBrand = CountForBrand;
            }
            var productList = Domain.Helper.SqlHelper.QuerySP<InventoryViewModel>("spSale_Get_Inventory", new { WarehouseId = "", HasQuantity = "1", ProductCode = "", ProductName = "", CategoryCode = "", ProductGroup = "", BranchId = model.BranchId, LoCode = "", ProductId = "", ExpiryDate = "", Origin = "" });
            productList = productList.Where(x => x.IsSale != null && x.IsSale == true);
            if (KH_BANHANG_CTIET_ID != null)
            {
                model.KH_BANHANG_CTIET_ID = KH_BANHANG_CTIET_ID.Value;
            }
            //Thêm số lượng tồn kho cho chi tiết đơn hàng đã được thêm
            if (model.InvoiceList != null && model.InvoiceList.Count > 0)
            {
                foreach (var item in model.InvoiceList)
                {
                    var product = productList.Where(i => i.ProductId == item.ProductId && i.LoCode == item.LoCode && i.ExpiryDate == item.ExpiryDate).FirstOrDefault();
                    if (product != null)
                    {
                        item.QuantityInInventory = product.Quantity;
                        item.PriceTest = product.ProductPriceOutbound;
                    }
                    else
                    {
                        item.Id = 0;
                    }
                }
            }
            int taxfee = 0;
            int.TryParse(Helpers.Common.GetSetting("vat"), out taxfee);
            model.TaxFee = taxfee;

            var category = categoryRepository.GetAllCategories()
            .Select(item => new CategoryViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Name = item.Name
            }).Where(x => x.Code == "Origin").ToList();
            ViewBag.category = category;

            return View(model);
        }
        [HttpPost]
        public ActionResult CreateProductInvoice(ProductInvoiceViewModel model, int KH_BANHANG_CTIET_ID)
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

            if (ModelState.IsValid)
            {
                CRM_Sale_ProductInvoice productInvoice = null;
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        if (model.Id == 0)
                        {
                            productInvoice = new CRM_Sale_ProductInvoice();
                            AutoMapper.Mapper.Map(model, productInvoice);
                            productInvoice.IsDeleted = false;
                            productInvoice.CreatedUserId = WebSecurity.CurrentUserId;
                            productInvoice.CreatedDate = DateTime.Now;
                            productInvoice.Status = Wording.OrderStatus_pending;
                            productInvoice.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID.Value);
                            productInvoice.IsArchive = false;
                            productInvoice.IsReturn = false;
                            productInvoice.Type = "invoice";
                            productInvoice.RemainingAmount = 0;
                            productInvoice.PaidAmount = model.TongTienSauVAT;
                            productInvoice.TotalAmount = model.TongTienSauVAT;
                            productInvoice.KH_BANHANG_CTIET_ID = KH_BANHANG_CTIET_ID;
                            productInvoice.PaymentMethod = SelectListHelper.GetSelectList_Category("FormPayment", null, "Name", null).FirstOrDefault().Value;
                            //hàm thêm mới
                            CRM_KH_BANHANGRepository.InsertProductInvoice(productInvoice, null);

                            //cập nhật lại mã hóa đơn
                            productInvoice.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("CRMProductInvoice", model.Code);

                            //begin kiểm tra trùng mã đơn hàng
                            var c = CRM_KH_BANHANGRepository.GetAllvwCRM_Sale_ProductInvoice().
                                Where(item => item.Code == productInvoice.Code).FirstOrDefault();

                            if (c != null)
                            {
                                TempData[Globals.FailedMessageKey] = "Mã đơn hàng đã bị trùng. Vui lòng kiểm tra lại!";

                                //return RedirectToAction("Create");
                                return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                            }
                            //end kiểm tra trùng mã đơn hàng
                            Erp.BackOffice.Helpers.Common.SetOrderNo("CRMProductInvoice");
                            if (model.InvoiceList.Any(x => x.Id == 0))
                            {
                                //lưu danh sách thao tác thực hiện dịch vụ
                                foreach (var item in model.InvoiceList.Where(x => x.Id == 0 && x.ProductId > 0))
                                {
                                    var ins = new CRM_Sale_ProductInvoiceDetail();
                                    ins.IsDeleted = false;
                                    ins.CreatedUserId = WebSecurity.CurrentUserId;
                                    ins.ModifiedUserId = WebSecurity.CurrentUserId;
                                    ins.CreatedDate = DateTime.Now;
                                    ins.ModifiedDate = DateTime.Now;
                                    ins.ExpiryDate = item.ExpiryDate;
                                    ins.LoCode = item.LoCode;
                                    ins.ProductId = item.ProductId.Value;
                                    ins.ProductInvoiceId = productInvoice.Id;
                                    ins.Price = item.Price;
                                    ins.Quantity = item.Quantity;
                                    ins.Unit = item.Unit;
                                    ins.Discount = 0;
                                    ins.DiscountAmount = 0;
                                    ins.Note = item.Note;
                                    ins.is_TANG = item.is_TANG;
                                    //  ins.ProductType = item.ProductType;
                                    CRM_KH_BANHANGRepository.InsertProductInvoiceDetail(ins);
                                }
                            }
                            //Thêm vào quản lý chứng từ
                            TransactionController.Create(new TransactionViewModel
                            {
                                TransactionModule = "ProductInvoice",
                                TransactionCode = productInvoice.Code,
                                TransactionName = "Bán hàng"
                            });
                        }
                        else
                        {
                            productInvoice = new CRM_Sale_ProductInvoice();
                            AutoMapper.Mapper.Map(model, productInvoice);
                            productInvoice.Status = Wording.OrderStatus_pending;
                            productInvoice.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID.Value);
                            productInvoice.IsArchive = false;
                            productInvoice.IsReturn = false;
                            productInvoice.Type = "invoice";
                            productInvoice.RemainingAmount = 0;
                            productInvoice.PaidAmount = model.TongTienSauVAT;
                            productInvoice.TotalAmount = model.TongTienSauVAT;
                            productInvoice.KH_BANHANG_CTIET_ID = KH_BANHANG_CTIET_ID;
                            productInvoice.PaymentMethod = SelectListHelper.GetSelectList_Category("FormPayment", null, "Name", null).FirstOrDefault().Value;
                            //hàm thêm mới
                            CRM_KH_BANHANGRepository.UpdateCRMProductInvoice(productInvoice);
                            var details = CRM_KH_BANHANGRepository.GetAllvwInvoiceDetailsByInvoiceId(model.Id).ToList();

                            foreach(var item in details)
                            {
                                CRM_KH_BANHANGRepository.DeleteDetailsInvoice(item.Id);
                            }
                            if (model.InvoiceList.Any(x => x.Id == 0))
                            {
                                //lưu danh sách thao tác thực hiện dịch vụ
                                foreach (var item in model.InvoiceList.Where(x => x.Id == 0 && x.ProductId > 0))
                                {
                                    var ins = new CRM_Sale_ProductInvoiceDetail();
                                    ins.IsDeleted = false;
                                    ins.CreatedUserId = WebSecurity.CurrentUserId;
                                    ins.ModifiedUserId = WebSecurity.CurrentUserId;
                                    ins.CreatedDate = DateTime.Now;
                                    ins.ModifiedDate = DateTime.Now;
                                    ins.ExpiryDate = item.ExpiryDate;
                                    ins.LoCode = item.LoCode;
                                    ins.ProductId = item.ProductId.Value;
                                    ins.ProductInvoiceId = productInvoice.Id;
                                    ins.Price = item.Price;
                                    ins.Quantity = item.Quantity;
                                    ins.Unit = item.Unit;
                                    ins.Discount = 0;
                                    ins.DiscountAmount = 0;
                                    ins.Note = item.Note;
                                    ins.is_TANG = item.is_TANG;
                                    //  ins.ProductType = item.ProductType;
                                    CRM_KH_BANHANGRepository.InsertProductInvoiceDetail(ins);
                                }
                            }
                        }
                        scope.Complete();
                    }
                    catch (DbUpdateException ex)
                    {
                        TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.Error;
                        return View(model);
                    }
                }
                return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
            }
            return View(model);
        }
        #endregion
        #region DetailProductInvoice
        public ActionResult DetailProductInvoice(int? Id, string TransactionCode, string code)
        {
            var productInvoice = new vwCRM_Sale_ProductInvoice();

            if (Id != null && Id.Value > 0)
            {
                productInvoice = CRM_KH_BANHANGRepository.GetvwCRM_Sale_ProductInvoiceFullById(Id.Value);
            }

            if (!string.IsNullOrEmpty(TransactionCode))
            {
                productInvoice = CRM_KH_BANHANGRepository.GetvwCRM_Sale_ProductInvoiceByCode(TransactionCode);
            }
            if (!string.IsNullOrEmpty(code))
            {
                productInvoice = CRM_KH_BANHANGRepository.GetvwCRM_Sale_ProductInvoiceByCode(code);
            }
            if (productInvoice == null)
            {
                return RedirectToAction("Index");
            }

            var model = new ProductInvoiceViewModel();
            AutoMapper.Mapper.Map(productInvoice, model);

            model.AllowEdit = Helpers.Common.KiemTraNgaySuaChungTu
            (model.CreatedDate.Value)
            && (Filters.SecurityFilter.IsAdmin()
            || productInvoice.BranchId == Helpers.Common.CurrentUser.BranchId
            );
            //Lấy lịch sử giao dịch thanh toán
            var listTransaction = transactionLiabilitiesRepository.GetAllvwTransaction()
                        .Where(item => item.MaChungTuGoc == productInvoice.Code)
                        .OrderByDescending(item => item.CreatedDate)
                        .ToList();

            model.ListTransactionLiabilities = new List<TransactionLiabilitiesViewModel>();
            AutoMapper.Mapper.Map(listTransaction, model.ListTransactionLiabilities);
            //Lấy danh sách chi tiết đơn hàng
            model.InvoiceList = CRM_KH_BANHANGRepository.GetAllvwInvoiceDetailsByInvoiceId(productInvoice.Id).Select(x =>
                new ProductInvoiceDetailViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    ProductType = x.ProductType,

                    CategoryCode = x.CategoryCode,
                    ProductInvoiceCode = x.ProductInvoiceCode,
                    ProductName = x.ProductName,
                    ProductCode = x.ProductCode,
                    ProductInvoiceId = x.ProductInvoiceId,
                    ProductGroup = x.ProductGroup,
                    Amount = x.is_TANG == 1 ? 0 : x.Amount,
                    LoCode = x.LoCode,
                    ExpiryDate = x.ExpiryDate,
                    Discount = x.Discount,
                    DiscountAmount = x.DiscountAmount,
                    is_TANG = x.is_TANG
                }).OrderBy(x => x.Id).ToList();

            ViewBag.isAdmin = Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId == 1 ? true : false;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            return View(model);
        }
        #endregion

        #region CreateKH_BANHANG
        public ActionResult CreateKH_BANHANG(CRM_KH_BANHANGViewModel model, int? id, string FullName, string CountForBrand, int? month, int? year)
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

            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = (intBrandID == null ? 0 : intBrandID),
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();


            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }

            //model.CustomerList = CustomerRepository.GetAllCustomer().Select(x => new CustomerViewModel
            //{
            //    Id = x.Id,
            //    CompanyName = x.CompanyName,
            //    Code = x.Code,
            //    Phone = x.Phone,
            //    ManagerStaffId = x.ManagerStaffId,
            //    CreatedDate = x.CreatedDate,
            //    isLock = x.isLock,
            //}).Where(x => x.isLock != true && listNhanvien.Contains(x.ManagerStaffId.Value)).ToList();
            //model.CustomerList = model.CustomerList.Where(x => x.ManagerStaffId == id).ToList();

            model.CRM_KH_BANHANG_CTIET = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Select(x => new CRM_KH_BANHANG_CTIETViewModel
            {
                KH_BANHANG_CTIET_ID = x.KH_BANHANG_CTIET_ID,
                BranchId = x.BranchId,
                KH_BANHANG_ID = x.KH_BANHANG_ID,
                THANG = x.THANG,
                NAM = x.NAM,
                KHACHHANG_ID = x.KHACHHANG_ID,
                NOIDUNG = x.NOIDUNG,
                TYLE_THANHCONG = x.TYLE_THANHCONG,
                TYLE_THANHCONG_REVIEW = x.TYLE_THANHCONG_REVIEW,
                GHI_CHU = x.GHI_CHU,
                IsDeleted = x.IsDeleted,
                CreatedDate = x.CreatedDate,
                CreatedUserId = x.CreatedUserId,
                ModifiedDate = x.ModifiedDate,
                ModifiedUserId = x.ModifiedUserId,
                Phone = x.Phone,
                CompanyName = x.CompanyName,
                Code = x.Code,
                CRM_Sale_ProductInvoiceCode = x.CRM_Sale_ProductInvoiceCode,
                CountForBrand = x.CountForBrand,
                TotalAmount = x.TotalAmount,
                ProductInvoiceId = x.ProductInvoiceId,
                NGUOILAP_ID = x.NGUOILAP_ID,
            }).Where(x => x.THANG == month && x.NAM == year && x.NGUOILAP_ID == id).ToList();
            if (id != null)
            {
                var user = userRepository.GetUserById(id.Value);
                model.NGUOILAP_ID = user.Id;
                model.FullName = user.FullName;
                ViewBag.user = user;
            }
            else
            {
                var user = userRepository.GetUserById(WebSecurity.CurrentUserId);
                model.NGUOILAP_ID = user.Id;
                model.FullName = user.FullName;
                ViewBag.user = user;
            }
            //var ProductInvoice = ProductInvoiceRepository.GetAllvwProductInvoice().Select(item => new ProductInvoiceViewModel
            //{
            //    Id = item.Id,
            //    ManagerStaffId = item.ManagerStaffId,
            //    CountForBrand = item.CountForBrand,
            //    TotalAmount = item.TotalAmount,
            //    Code = item.Code,
            //    CustomerId = item.CustomerId,
            //}).ToList();
            //ViewBag.ProductInvoice = ProductInvoice;
            return View(model);
        }

        [HttpPost]
        public ActionResult CreateKH_BANHANG(CRM_KH_BANHANGViewModel model)
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
            model.BranchId = intBrandID;

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var kh_banhang = CRM_KH_BANHANGRepository.GetAllCRM_KH_BANHANG().Where(x => x.NAM == model.NAM &&
                    x.THANG == model.THANG && x.NGUOILAP_ID == model.NGUOILAP_ID && x.CountForBrand == model.CountForBrand && x.BranchId == model.BranchId).ToList();
                    if (kh_banhang.Count() > 0)
                    {
                        TempData[Globals.FailedMessageKey] = "Tháng " + model.THANG + " năm " + model.NAM + " đã có kế hoạch cho nhãn hàng " + model.CountForBrand;
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });

                    }
                    else
                    {
                        var CRM_KH_BANHANG = new CRM_KH_BANHANG();
                        AutoMapper.Mapper.Map(model, CRM_KH_BANHANG);
                        CRM_KH_BANHANG.IsDeleted = false;
                        CRM_KH_BANHANG.CreatedUserId = WebSecurity.CurrentUserId;
                        CRM_KH_BANHANG.ModifiedUserId = WebSecurity.CurrentUserId;

                        CRM_KH_BANHANG.CreatedDate = DateTime.Now;
                        CRM_KH_BANHANG.ModifiedDate = DateTime.Now;
                        CRM_KH_BANHANG.NAM = model.NAM;
                        CRM_KH_BANHANG.THANG = model.THANG;
                        CRM_KH_BANHANG.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                        CRM_KH_BANHANG.CountForBrand = model.CountForBrand;
                        CRM_KH_BANHANG.TARGET_BRAND = model.TARGET_BRAND;
                        CRM_KH_BANHANG.NGUOILAP_ID = model.NGUOILAP_ID;
                        CRM_KH_BANHANGRepository.InsertCRM_KH_BANHANG(CRM_KH_BANHANG);
                    }
                    scope.Complete();
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                }
                catch (DbUpdateException ex)
                {
                    TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.Error;
                    return View(model);
                }
            }
        }
        #endregion
        #region CreateKH
        public ActionResult CreateKH(int? SalerId, int? Year, int? Month, int? KH_BANHANG_ID, string Phone, string CustomerName, string CustomerCode, string checkTK)
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

            Year = Year == null ? DateTime.Now.Year : Year;
            Month = Month == null ? DateTime.Now.Month : Month;
            Phone = Phone == null ? "" : Phone;
            CustomerCode = CustomerCode == null ? "" : CustomerCode;
            CustomerName = CustomerName == null ? "" : CustomerName;
            var CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == KH_BANHANG_ID && x.IsDeleted == false).ToList();
            var dataNhanvien = SqlHelper.QuerySP<ManagerStaff>("spgetNhanvienbacap", new
            {
                BranchId = (intBrandID == null ? 0 : intBrandID),
                UserId = WebSecurity.CurrentUserId,
            }).ToList();

            List<int> listNhanvien = new List<int>();

            bool hasSearch = false;
            for (int i = 0; i < dataNhanvien.Count; i++)
            {
                listNhanvien.Add(int.Parse(dataNhanvien[i].Id.ToString()));
            }
            var user2 = userRepository.GetAllUsers().Select(item => new UserViewModel
            {
                Id = item.Id,
                UserName = item.UserName,
                FullName = item.FullName,
                Status = item.Status,
                BranchId = item.BranchId,
                IdManager = item.IdManager
            }).Where(x => x.Status == UserStatus.Active && listNhanvien.Contains(x.Id)).ToList();
            ViewBag.user = user2;

            var model = new CRM_KH_BANHANGViewModel();
            //foreach (var item1 in CRM_KH_BANHANGCT)
            //{
            if ((SalerId == null) || (SalerId == 0))
            {
                return View(model);
            }
            model.CustomerList = CustomerRepository.GetAllvwCustomer().Select(x => new CustomerViewModel
            {
                Id = x.Id,
                CompanyName = x.CompanyName,
                Code = x.Code,
                Phone = x.Phone,
                ManagerStaffId = x.ManagerStaffId,
                CreatedDate = x.CreatedDate,
                isLock = x.isLock,
                ManagerStaffName = x.ManagerStaffName,
                ManagerUserName = x.ManagerUserName,
                BranchId = x.BranchId
            }).Where(x => (x.BranchId == intBrandID) && (x.isLock == false || x.isLock == null) /*&& x.ManagerStaffId == SalerId*/).ToList();

            if (SalerId != null)
            {
                model.CustomerList = model.CustomerList.Where(x => x.ManagerStaffId == SalerId).ToList();
            }



            //}
            //if(listNhanvien.Count() >0)
            //{
            //    model.CustomerList.Where(x =>listNhanvien.Contains(x.ManagerStaffId.Value)).ToList();
            //}
            model.CustomerList.RemoveAll(
                            x =>
                            CRM_KH_BANHANGCT.Any(
                            k => k.KHACHHANG_ID == x.Id));

            model.vwKH_SAPHETSP = SqlHelper.QuerySP<vwKH_SAPHETSPViewModel>("sp_CRMKH_SAPHETSP", new
            {
                Phone = Phone,
                //ManagerStaffId = WebSecurity.CurrentUserId,
                CustomerCode = CustomerCode,
                CustomerName = CustomerName,
                Year = Year,
                Month = Month
            }).Where(x => x.BranchId == intBrandID).ToList();






            model.MBSAPHETHANList = SqlHelper.QuerySP<MBSAPHETHANViewModel>("spMBSAPHETHAN", new
            {
                Phone = Phone,
                ManagerStaffId = WebSecurity.CurrentUserId,
                CustomerCode = CustomerCode,
                CustomerName = CustomerName,
                Year = Year,
                Month = Month
            }).Where(x => x.BranchId == intBrandID).ToList();




            model.KH_TUONGTACList = SqlHelper.QuerySP<KH_TUONGTACViewModel>("spGET_KH_TUONGTAC", new
            {
                Phone = Phone,
                ManagerStaffId = WebSecurity.CurrentUserId,
                CustomerCode = CustomerCode,
                CustomerName = CustomerName,
                Year = Year,
                Month = Month
            }).Where(x => x.BranchId == intBrandID).ToList();






            model.vwKH_SAPHETDV = SqlHelper.QuerySP<KH_SapHetDVViewModel>("sp_CRMKH_SAPHETDV", new
            {
                Phone = Phone,
                ManagerStaffId = WebSecurity.CurrentUserId,
                CustomerCode = CustomerCode,
                CustomerName = CustomerName,
                Year = Year,
                Month = Month
            }).Where(x => x.BranchId == intBrandID).ToList();



            if (!Erp.BackOffice.Filters.SecurityFilter.IsAdmin())
            {
                model.vwKH_SAPHETDV = model.vwKH_SAPHETDV.Where(x => listNhanvien.Contains(x.ManagerStaffId.Value)).ToList();
                model.CustomerList = model.CustomerList.Where(x => listNhanvien.Contains(x.ManagerStaffId.Value)).ToList();
                model.vwKH_SAPHETSP = model.vwKH_SAPHETSP.Where(x => listNhanvien.Contains(x.ManagerStaffId.Value)).ToList();
                model.KH_TUONGTACList = model.KH_TUONGTACList.Where(x => listNhanvien.Contains(x.ManagerStaffId.Value)).ToList();
                model.MBSAPHETHANList = model.MBSAPHETHANList.Where(x => listNhanvien.Contains(x.ManagerStaffId.Value)).ToList();

            }




            model.vwKH_SAPHETDV.RemoveAll(
                   x =>
                   CRM_KH_BANHANGCT.Any(
                   k => k.KHACHHANG_ID == x.CustomerId));
            model.KH_TUONGTACList.RemoveAll(
                   x =>
                   CRM_KH_BANHANGCT.Any(
                   k => k.KHACHHANG_ID == x.KHACHHANG_ID));
            model.MBSAPHETHANList.RemoveAll(
                   x =>
                   CRM_KH_BANHANGCT.Any(
                   k => k.KHACHHANG_ID == x.CustomerId));
            model.vwKH_SAPHETSP.RemoveAll(
                   x =>
                   CRM_KH_BANHANGCT.Any(
                   k => k.KHACHHANG_ID == x.CustomerId));
            model.KH_TUONGTACList = model.KH_TUONGTACList.GroupBy(x => x.KHACHHANG_ID).Select(x => x.First()).OrderByDescending(x => x.CreatedDate).ToList();
            if (checkTK == "checkSPSHH")
            {
                model.checkTK = checkTK;
            }
            if (checkTK == "checkMB")
            {
                model.checkTK = checkTK;
            }
            if (checkTK == "checkDVSHH")
            {
                model.checkTK = checkTK;
            }
            //model.KH_TUONGTACList = kH_TUONGTACRepository.GetAllvwKH_TUONGTAC().Select(x => new KH_TUONGTACViewModel
            //    {
            //        KH_TUONGTAC_ID = x.KH_TUONGTAC_ID,
            //        CustomerCode = x.CustomerCode,
            //        KHACHHANG_ID = x.KHACHHANG_ID,
            //        Phone = x.Phone,
            //        NGUOILAP_ID = x.NGUOILAP_ID,
            //        CreatedDate = x.CreatedDate,
            //        NGAYTUONGTAC_TIEP = x.NGAYTUONGTAC_TIEP,
            //        GIOTUONGTAC_TIEP = x.GIOTUONGTAC_TIEP,
            //        LICHTUONGTATIEP = x.NGAYTUONGTAC_TIEP,
            //        //THANG= int.Parse(x.NGAYTUONGTAC_TIEP.Substring(0,2)),
            //        //NAM= int.Parse(x.NGAYTUONGTAC_TIEP.Substring(3,2)),
            //        CustomerName=x.CustomerName
            //    }).ToList();
            //model.KH_TUONGTACList.Where(x=>x.LICHTUONGTATIEP!=null && int.Parse(x.LICHTUONGTATIEP.Substring(3,2))==Month && int.Parse(x.LICHTUONGTATIEP.Substring(6,4))==Year);
            if (checkTK == "checkLTT")
            {
                model.checkTK = checkTK;
            }
            if (CustomerCode != null)
            {
                CustomerCode = Helpers.Common.ChuyenThanhKhongDau(CustomerCode);
                model.vwKH_SAPHETDV = model.vwKH_SAPHETDV.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerCode).Contains(CustomerCode)).ToList();
                model.MBSAPHETHANList = model.MBSAPHETHANList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerCode).Contains(CustomerCode)).ToList();
                model.vwKH_SAPHETSP = model.vwKH_SAPHETSP.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerCode).Contains(CustomerCode)).ToList();
                model.KH_TUONGTACList = model.KH_TUONGTACList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerCode).Contains(CustomerCode)).ToList();
                model.CustomerList = model.CustomerList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Code).Contains(CustomerCode)).ToList();
                hasSearch = true;
            }
            if (CustomerName != null)
            {
                CustomerName = Helpers.Common.ChuyenThanhKhongDau(CustomerName);
                model.vwKH_SAPHETDV = model.vwKH_SAPHETDV.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CompanyName).Contains(CustomerName)).ToList();
                model.MBSAPHETHANList = model.MBSAPHETHANList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(CustomerName)).ToList();
                model.vwKH_SAPHETSP = model.vwKH_SAPHETSP.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CompanyName).Contains(CustomerName)).ToList();
                model.KH_TUONGTACList = model.KH_TUONGTACList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(CustomerName)).ToList();
                model.CustomerList = model.CustomerList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CompanyName).Contains(CustomerName)).ToList();
                hasSearch = true;
            }
            if (Phone != null)
            {
                model.vwKH_SAPHETDV = model.vwKH_SAPHETDV.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Phone).Contains(Phone)).ToList();
                model.MBSAPHETHANList = model.MBSAPHETHANList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Phone).Contains(Phone)).ToList();
                model.CustomerList = model.CustomerList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Phone).Contains(Phone)).ToList();
                model.KH_TUONGTACList = model.KH_TUONGTACList.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Phone).Contains(Phone)).ToList();
                hasSearch = true;
            }
            var ProductInvoice = ProductInvoiceRepository.GetAllvwProductInvoice().Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                ManagerStaffId = item.ManagerStaffId,
                CountForBrand = item.CountForBrand,
                TotalAmount = item.TotalAmount,
                Code = item.Code,
                CustomerId = item.CustomerId,
            }).ToList();


            ViewBag.ProductInvoice = ProductInvoice;
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateKH(CRM_KH_BANHANGViewModel model)
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


            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var q = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(model.KH_BANHANG_ID);
                    if (model.CustomerList != null)
                    {
                        foreach (var item in model.CustomerList)
                        {
                            var CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == model.KH_BANHANG_ID && x.KHACHHANG_ID == item.Id && x.IsDeleted == false);
                            if (item.is_checked != null)
                            {
                                if (CRM_KH_BANHANGCT.Count() == 0)
                                {
                                    var ins = new CRM_KH_BANHANG_CTIET();
                                    ins.IsDeleted = false;
                                    ins.CreatedUserId = WebSecurity.CurrentUserId;
                                    ins.ModifiedUserId = WebSecurity.CurrentUserId;
                                    ins.CreatedDate = DateTime.Now;
                                    ins.ModifiedDate = DateTime.Now;
                                    ins.THANG = q.THANG;
                                    ins.NAM = q.NAM;
                                    ins.KHACHHANG_ID = item.Id;
                                    ins.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                                    ins.KH_BANHANG_ID = model.KH_BANHANG_ID;
                                    ins.GHI_CHU = item.GHI_CHU;
                                    ins.TYLE_THANHCONG = 0;
                                    ins.TYLE_THANHCONG_REVIEW = 0;
                                    CRM_KH_BANHANG_CTIETRepository.InsertCRM_KH_BANHANG_CTIET(ins);
                                }
                            }
                        }
                    }
                    if (model.KH_TUONGTACList != null)
                    {
                        foreach (var item in model.KH_TUONGTACList)
                        {
                            var CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == model.KH_BANHANG_ID && x.KHACHHANG_ID == item.CustomerId && x.IsDeleted == false);
                            if (item.is_checkedKH_TUONGTAC != null)
                            {
                                if (CRM_KH_BANHANGCT.Count() == 0)
                                {
                                    var ins = new CRM_KH_BANHANG_CTIET();
                                    ins.IsDeleted = false;
                                    ins.CreatedUserId = WebSecurity.CurrentUserId;
                                    ins.ModifiedUserId = WebSecurity.CurrentUserId;
                                    ins.CreatedDate = DateTime.Now;
                                    ins.ModifiedDate = DateTime.Now;
                                    ins.THANG = q.THANG;
                                    ins.NAM = q.NAM;
                                    ins.KHACHHANG_ID = item.KHACHHANG_ID;
                                    ins.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                                    ins.KH_BANHANG_ID = model.KH_BANHANG_ID;
                                    ins.GHI_CHU = item.GHI_CHU;
                                    ins.TYLE_THANHCONG = 0;
                                    ins.TYLE_THANHCONG_REVIEW = 0;
                                    ins.NOIDUNG = item.NGAYTUONGTAC_TIEP;
                                    CRM_KH_BANHANG_CTIETRepository.InsertCRM_KH_BANHANG_CTIET(ins);
                                }
                            }
                        }
                    }
                    if (model.vwKH_SAPHETSP != null)
                    {
                        foreach (var item in model.vwKH_SAPHETSP)
                        {
                            var CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == model.KH_BANHANG_ID && x.KHACHHANG_ID == item.CustomerId && x.IsDeleted == false);
                            if (item.is_checkedSAPHETSP != null)
                            {
                                if (CRM_KH_BANHANGCT.Count() == 0)
                                {
                                    var ins = new CRM_KH_BANHANG_CTIET();
                                    ins.IsDeleted = false;
                                    ins.CreatedUserId = WebSecurity.CurrentUserId;
                                    ins.ModifiedUserId = WebSecurity.CurrentUserId;
                                    ins.CreatedDate = DateTime.Now;
                                    ins.ModifiedDate = DateTime.Now;
                                    ins.THANG = q.THANG;
                                    ins.NAM = q.NAM;
                                    ins.KHACHHANG_ID = item.CustomerId;
                                    ins.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                                    ins.KH_BANHANG_ID = model.KH_BANHANG_ID;
                                    ins.GHI_CHU = item.GHI_CHU;
                                    ins.TYLE_THANHCONG = 0;
                                    ins.TYLE_THANHCONG_REVIEW = 0;
                                    ins.NOIDUNG = item.SPSAPHETHAN;
                                    CRM_KH_BANHANG_CTIETRepository.InsertCRM_KH_BANHANG_CTIET(ins);
                                }
                            }
                        }
                    }
                    if (model.MBSAPHETHANList != null)
                    {
                        foreach (var item in model.MBSAPHETHANList)
                        {
                            var CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == model.KH_BANHANG_ID && x.KHACHHANG_ID == item.CustomerId && x.IsDeleted == false);
                            if (item.is_checkedMBSAPHETHAN != null)
                            {
                                if (CRM_KH_BANHANGCT.Count() == 0)
                                {
                                    var ins = new CRM_KH_BANHANG_CTIET();
                                    ins.IsDeleted = false;
                                    ins.CreatedUserId = WebSecurity.CurrentUserId;
                                    ins.ModifiedUserId = WebSecurity.CurrentUserId;
                                    ins.CreatedDate = DateTime.Now;
                                    ins.ModifiedDate = DateTime.Now;
                                    ins.THANG = q.THANG;
                                    ins.NAM = q.NAM;
                                    ins.KHACHHANG_ID = item.CustomerId;
                                    ins.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                                    ins.KH_BANHANG_ID = model.KH_BANHANG_ID;
                                    ins.GHI_CHU = item.GHI_CHU;
                                    ins.TYLE_THANHCONG = 0;
                                    ins.TYLE_THANHCONG_REVIEW = 0;
                                    ins.NOIDUNG = item.ExpiryDate;
                                    CRM_KH_BANHANG_CTIETRepository.InsertCRM_KH_BANHANG_CTIET(ins);
                                }
                            }
                        }
                    }
                    if (model.vwKH_SAPHETDV != null)
                    {
                        foreach (var item in model.vwKH_SAPHETDV)
                        {
                            var CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == model.KH_BANHANG_ID && x.KHACHHANG_ID == item.CustomerId && x.IsDeleted == false);
                            if (item.is_checkedKH_SAPHETDV != null)
                            {
                                if (CRM_KH_BANHANGCT.Count() == 0)
                                {
                                    var ins = new CRM_KH_BANHANG_CTIET();
                                    ins.IsDeleted = false;
                                    ins.CreatedUserId = WebSecurity.CurrentUserId;
                                    ins.ModifiedUserId = WebSecurity.CurrentUserId;
                                    ins.CreatedDate = DateTime.Now;
                                    ins.ModifiedDate = DateTime.Now;
                                    ins.THANG = q.THANG;
                                    ins.NAM = q.NAM;
                                    ins.KHACHHANG_ID = item.CustomerId;
                                    ins.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                                    ins.KH_BANHANG_ID = model.KH_BANHANG_ID;
                                    ins.GHI_CHU = item.GHI_CHU;
                                    ins.TYLE_THANHCONG = 0;
                                    ins.TYLE_THANHCONG_REVIEW = 0;
                                    ins.NOIDUNG = item.SPSAPHETHAN;
                                    CRM_KH_BANHANG_CTIETRepository.InsertCRM_KH_BANHANG_CTIET(ins);
                                }
                            }
                        }
                    }
                    scope.Complete();
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                }
                catch (Exception ex)
                {
                    return View(model);
                }
            }

        }
        #endregion
        #region KETHUA
        [AllowAnonymous]
        [HttpPost]
        public ActionResult KETHUA(CRM_KH_BANHANGViewModel model, int? KH_BANHANG_ID, string CountForBrand, int? NGUOILAP_ID)
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

            DateTime date = DateTime.Parse(model.Month);
            date = date.AddMonths(-1);
            //string THANGNAM = date.ToString("M/yyyy");
            //int THANG = int.Parse(THANGNAM.Substring(0, 1));
            //int NAM = int.Parse(THANGNAM.Substring(2, 4));
            //var model = new CRM_KH_BANHANGViewModel();
            int THANG = date.Month;
            int NAM = date.Year;
            IEnumerable<CRM_KH_BANHANGViewModel> q = CRM_KH_BANHANGRepository.GetAllCRM_KH_BANHANG().Select(x => new CRM_KH_BANHANGViewModel
            {
                KH_BANHANG_ID = x.KH_BANHANG_ID,
                BranchId = x.BranchId,
                THANG = x.THANG.Value,
                NAM = x.NAM.Value,
                NGUOILAP_ID = x.NGUOILAP_ID,
                CountForBrand = x.CountForBrand,
                TARGET_BRAND = x.TARGET_BRAND,
                GHI_CHU = x.GHI_CHU,
                IsDeleted = x.IsDeleted,
                CreatedDate = x.CreatedDate,
                CreatedUserId = x.CreatedUserId,
                ModifiedDate = x.ModifiedDate,
                ModifiedUserId = x.ModifiedUserId
            }).Where(x => x.THANG == THANG && x.NAM == NAM && x.CountForBrand == model.CountForBrand && x.NGUOILAP_ID == model.NGUOILAP_ID).ToList();
            //var CRM_KH_BANHANGS =new CRM_KH_BANHANG_CTIETViewModel();
            IEnumerable<CRM_KH_BANHANG_CTIETViewModel> CRM_KH_BANHANGS = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Select(x => new CRM_KH_BANHANG_CTIETViewModel
            {
                //KH_BANHANG_ID = x.KH_BANHANG_ID,
                KH_BANHANG_CTIET_ID = x.KH_BANHANG_CTIET_ID,
                Phone = x.Phone,
                KHACHHANG_ID = x.KHACHHANG_ID,
                CompanyName = x.CompanyName,
                Code = x.Code,
                TYLE_THANHCONG = x.TYLE_THANHCONG,
                TYLE_THANHCONG_REVIEW = x.TYLE_THANHCONG_REVIEW,
                GHI_CHU = x.GHI_CHU,
                IsDeleted = x.IsDeleted,
                THANG = x.THANG,
                NAM = x.NAM,
                CRM_Sale_ProductInvoiceCode = x.CRM_Sale_ProductInvoiceCode,
                CountForBrand = x.CountForBrand,
                TotalAmount = x.TotalAmount,
                ProductInvoiceId = x.ProductInvoiceId,
                NGUOILAP_ID = x.NGUOILAP_ID
            }).Where(x => x.THANG == THANG && x.NAM == NAM && x.CountForBrand == model.CountForBrand && x.TYLE_THANHCONG_REVIEW < 100 && x.TYLE_THANHCONG_REVIEW > -1 && x.NGUOILAP_ID == model.NGUOILAP_ID).ToList();
            if (KH_BANHANG_ID != null)
            {
                var CRM_KH_BANHANG = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGById(KH_BANHANG_ID.Value);
                //var CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANGByKHBANHANG_ID(KH_BANHANG_ID.Value);
                foreach (var item in CRM_KH_BANHANGS)
                {
                    IEnumerable<CRM_KH_BANHANG_CTIETViewModel> CRM_KH_BANHANGCT = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Select(x => new CRM_KH_BANHANG_CTIETViewModel
                    {
                        KH_BANHANG_CTIET_ID = x.KH_BANHANG_CTIET_ID,
                        Phone = x.Phone,
                        KHACHHANG_ID = x.KHACHHANG_ID,
                        CompanyName = x.CompanyName,
                        Code = x.Code,
                        TYLE_THANHCONG = x.TYLE_THANHCONG,
                        TYLE_THANHCONG_REVIEW = x.TYLE_THANHCONG_REVIEW,
                        GHI_CHU = x.GHI_CHU,
                        IsDeleted = x.IsDeleted,
                        THANG = x.THANG,
                        NAM = x.NAM,
                        CRM_Sale_ProductInvoiceCode = x.CRM_Sale_ProductInvoiceCode,
                        CountForBrand = x.CountForBrand,
                        TotalAmount = x.TotalAmount,
                        ProductInvoiceId = x.ProductInvoiceId,
                        KH_BANHANG_ID = x.KH_BANHANG_ID
                    }).Where(x => x.CountForBrand == model.CountForBrand && x.KH_BANHANG_ID == KH_BANHANG_ID && x.KHACHHANG_ID == item.KHACHHANG_ID).ToList();
                    var ins = new CRM_KH_BANHANG_CTIET();
                    if (CRM_KH_BANHANGCT.Count() == 0)
                    {
                        ins.IsDeleted = false;
                        ins.CreatedUserId = WebSecurity.CurrentUserId;
                        ins.ModifiedUserId = WebSecurity.CurrentUserId;
                        ins.CreatedDate = DateTime.Now;
                        ins.ModifiedDate = DateTime.Now;
                        ins.THANG = CRM_KH_BANHANG.THANG;
                        ins.NAM = CRM_KH_BANHANG.NAM;
                        ins.KHACHHANG_ID = item.KHACHHANG_ID;
                        ins.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                        ins.KH_BANHANG_ID = KH_BANHANG_ID;
                        ins.GHI_CHU = item.GHI_CHU;
                        ins.TYLE_THANHCONG = item.TYLE_THANHCONG_REVIEW;
                        ins.TYLE_THANHCONG_REVIEW = null;
                        //ins.NOIDUNG = "Kế thừa từ tháng trước";
                        CRM_KH_BANHANG_CTIETRepository.InsertCRM_KH_BANHANG_CTIET(ins);
                    }
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                return RedirectToAction("Index", "CRM_KH_BANHANG");
            }
            else
            {
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        var CRM_KH_BANHANG = new CRM_KH_BANHANG();
                        foreach (var item in q)
                        {
                            CRM_KH_BANHANG.IsDeleted = false;
                            CRM_KH_BANHANG.CreatedUserId = WebSecurity.CurrentUserId;
                            CRM_KH_BANHANG.ModifiedUserId = WebSecurity.CurrentUserId;
                            CRM_KH_BANHANG.CreatedDate = DateTime.Now;
                            CRM_KH_BANHANG.ModifiedDate = DateTime.Now;
                            CRM_KH_BANHANG.THANG = int.Parse(model.Month.Substring(0, 1));
                            CRM_KH_BANHANG.NAM = int.Parse(model.Month.Substring(2, 4));
                            CRM_KH_BANHANG.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                            CRM_KH_BANHANG.CountForBrand = model.CountForBrand;
                            CRM_KH_BANHANG.TARGET_BRAND = item.TARGET_BRAND;
                            CRM_KH_BANHANG.NGUOILAP_ID = model.NGUOILAP_ID;
                            CRM_KH_BANHANGRepository.InsertCRM_KH_BANHANG(CRM_KH_BANHANG);
                        }

                        foreach (var item in model.CRM_KH_BANHANGList)
                        {
                            var ins = new CRM_KH_BANHANG_CTIET();
                            ins.IsDeleted = false;
                            ins.CreatedUserId = WebSecurity.CurrentUserId;
                            ins.ModifiedUserId = WebSecurity.CurrentUserId;
                            ins.CreatedDate = DateTime.Now;
                            ins.ModifiedDate = DateTime.Now;
                            ins.THANG = CRM_KH_BANHANG.THANG;
                            ins.NAM = CRM_KH_BANHANG.NAM;
                            ins.KHACHHANG_ID = item.KHACHHANG_ID;
                            ins.BranchId = (intBrandID == null ? Convert.ToInt32(Session["GlobalCurentBranchId"]) : intBrandID);
                            ins.KH_BANHANG_ID = CRM_KH_BANHANG.KH_BANHANG_ID;
                            ins.GHI_CHU = item.GHI_CHU;
                            ins.TYLE_THANHCONG = item.TYLE_THANHCONG_REVIEW;
                            ins.TYLE_THANHCONG_REVIEW = null;
                            //ins.NOIDUNG = "Kế thừa từ tháng trước";
                            CRM_KH_BANHANG_CTIETRepository.InsertCRM_KH_BANHANG_CTIET(ins);
                        }
                        scope.Complete();
                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                        return RedirectToAction("Index", "CRM_KH_BANHANG");
                    }
                    catch (Exception ex) { }
                }
            }

            TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
            return RedirectToAction("Index");
        }
        #endregion
        #region Delete
        [AllowAnonymous]
        [HttpPost]
        public JsonResult Delete(CRM_KH_BANHANG_CTIETViewModel model)
        {
            try
            {
                var item = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANG_CTById(model.KH_BANHANG_CTIET_ID);

                if (item != null)
                {
                    item.IsDeleted = true;
                    CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG_CT(item);
                }

                if (model.ProductInvoiceId != null)
                {
                    var item1 = CRM_KH_BANHANGRepository.GetCRM_Sale_ProductInvoiceById(model.ProductInvoiceId.Value);
                    if (item1 != null)
                    {
                        item1.IsDeleted = true;
                        CRM_KH_BANHANGRepository.UpdateCRMProductInvoice(item1);
                    }
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return Json(true);
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return Json(false);
            }
        }
        #endregion

        #region Print

        public ActionResult Print_KH_BH(int id, string month, string year)
        {

            var model = new TemplatePrintViewModel();
            var model2 = new TemplatePrintViewModel();
            var model3 = new TemplatePrintViewModel();
            var model4 = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";

            //lấy template phiếu xuất.
            //var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("Print_kh_bh")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            //model.Content = template.Content;
            //if (month != null && year != null)
            //    model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCTK(id, Int32.Parse(month), Int32.Parse(year), "ANNAYAKE"));
            //model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            //model.Content = model.Content.Replace("{System.CompanyName}", company);
            //model.Content = model.Content.Replace("{System.AddressCompany}", address);
            //model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            //model.Content = model.Content.Replace("{System.Fax}", fax);
            //model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            //model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            //model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

            //model2.Content = template.Content;
            //if (month != null && year != null)
            //    model2.Content = model2.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCTK(id, Int32.Parse(month), Int32.Parse(year), "ORLANEPARIS"));
            //model2.Content = model2.Content.Replace("{System.Logo}", ImgLogo);
            //model2.Content = model2.Content.Replace("{System.CompanyName}", company);
            //model2.Content = model2.Content.Replace("{System.AddressCompany}", address);
            //model2.Content = model2.Content.Replace("{System.PhoneCompany}", phone);
            //model2.Content = model2.Content.Replace("{System.Fax}", fax);
            //model2.Content = model2.Content.Replace("{System.BankCodeCompany}", bankcode);
            //model2.Content = model2.Content.Replace("{System.BankNameCompany}", bankname);
            //model2.Content = model2.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

            //model3.Content = template.Content;
            //if (month != null && year != null)
            //    model3.Content = model3.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCTK(id, Int32.Parse(month), Int32.Parse(year), "CONGNGHECAO"));
            //model3.Content = model3.Content.Replace("{System.Logo}", ImgLogo);
            //model3.Content = model3.Content.Replace("{System.CompanyName}", company);
            //model3.Content = model3.Content.Replace("{System.AddressCompany}", address);
            //model3.Content = model3.Content.Replace("{System.PhoneCompany}", phone);
            //model3.Content = model3.Content.Replace("{System.Fax}", fax);
            //model3.Content = model3.Content.Replace("{System.BankCodeCompany}", bankcode);
            //model3.Content = model3.Content.Replace("{System.BankNameCompany}", bankname);
            //model3.Content = model3.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

            //model4.Content = template.Content;
            //if (month != null && year != null)
            //    model4.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCTK(id, Int32.Parse(month), Int32.Parse(year), "DICHVU"));
            //model4.Content = model4.Content.Replace("{System.Logo}", ImgLogo);
            //model4.Content = model4.Content.Replace("{System.CompanyName}", company);
            //model4.Content = model4.Content.Replace("{System.AddressCompany}", address);
            //model4.Content = model4.Content.Replace("{System.PhoneCompany}", phone);
            //model4.Content = model4.Content.Replace("{System.Fax}", fax);
            //model4.Content = model4.Content.Replace("{System.BankCodeCompany}", bankcode);
            //model4.Content = model4.Content.Replace("{System.BankNameCompany}", bankname);
            //model4.Content = model4.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));


            DataTable dt1 = new DataTable("ANNAYAKE");
            for (int i = 0; i < 11; i++)
                dt1.Columns.Add();
            dt1.Rows.Add(company);
            dt1.Rows.Add("Địa chỉ :" + address);
            dt1.Rows.Add("Điện thoại : " + phone + " - " + "fax: " + fax);
            dt1.Rows.Add("STK :" + bankcode + " - " + "Ngân hàng :" + bankname);
            dt1.Rows.Add("KẾ HOẠCH BÁN HÀNG");
            buildHtmlDetailList_PrintBCKHBH(dt1, id, Int32.Parse(month), Int32.Parse(year), "ANNAYAKE");

            DataTable dt2 = new DataTable("ORLANE PARIS");
            for (int i = 0; i < 11; i++)
                dt2.Columns.Add();
            dt2.Rows.Add(company);
            dt2.Rows.Add("Địa chỉ :" + address);
            dt2.Rows.Add("Điện thoại : " + phone + " - " + "fax: " + fax);
            dt2.Rows.Add("STK :" + bankcode + " - " + "Ngân hàng :" + bankname);
            dt2.Rows.Add("KẾ HOẠCH BÁN HÀNG");
            buildHtmlDetailList_PrintBCKHBH(dt2, id, Int32.Parse(month), Int32.Parse(year), "ORLANEPARIS");

            DataTable dt3 = new DataTable("LEONORGREYL");
            for (int i = 0; i < 11; i++)
                dt3.Columns.Add();
            dt3.Rows.Add(company);
            dt3.Rows.Add("Địa chỉ :" + address);
            dt3.Rows.Add("Điện thoại : " + phone + " - " + "fax: " + fax);
            dt3.Rows.Add("STK :" + bankcode + " - " + "Ngân hàng :" + bankname);
            dt3.Rows.Add("KẾ HOẠCH BÁN HÀNG");
            buildHtmlDetailList_PrintBCKHBH(dt3, id, Int32.Parse(month), Int32.Parse(year), "LEONORGREYL");

            DataTable dt4 = new DataTable("CONG NGHE CAO");
            for (int i = 0; i < 11; i++)
                dt4.Columns.Add();
            dt4.Rows.Add(company);
            dt4.Rows.Add("Địa chỉ :" + address);
            dt4.Rows.Add("Điện thoại : " + phone + " - " + "fax: " + fax);
            dt4.Rows.Add("STK :" + bankcode + " - " + "Ngân hàng :" + bankname);
            dt4.Rows.Add("KẾ HOẠCH BÁN HÀNG");
            buildHtmlDetailList_PrintBCKHBH(dt4, id, Int32.Parse(month), Int32.Parse(year), "CONGNGHECAO");

            DataTable dt5 = new DataTable("DICH VU");
            for (int i = 0; i < 11; i++)
                dt5.Columns.Add();
            dt5.Rows.Add(company);
            dt5.Rows.Add("Địa chỉ :" + address);
            dt5.Rows.Add("Điện thoại : " + phone + " - " + "fax: " + fax);
            dt5.Rows.Add("STK :" + bankcode + " - " + "Ngân hàng :" + bankname);
            dt5.Rows.Add("KẾ HOẠCH BÁN HÀNG");
            buildHtmlDetailList_PrintBCKHBH(dt5, id, Int32.Parse(month), Int32.Parse(year), "DICHVU");

            var ds = new DataSet();
            ds.Tables.Add(dt1);
            ds.Tables.Add(dt2);
            ds.Tables.Add(dt3);
            ds.Tables.Add(dt4);
            ds.Tables.Add(dt5);
            ExcelHelper.ToExcel(ds, "KeHoachBanHang" + DateTime.Now.ToString("yyyyMMdd") + ".xls", Response);

            //if (ExportExcel)
            //{
            //Response.AppendHeader("content-disposition", "attachment;filename=" + "Kế hoạch bán hàng" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            //Response.Charset = "";
            //Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            //Response.Write(model.Content);
            //Response.Flush();
            //Response.Close();
            //Response.End();
            //}


            return View(model);
        }
        void buildHtmlDetailList_PrintBCKHBH(DataTable dt, int id, int month, int year, string brand)
        {
            var model = new CRM_KH_BANHANGViewModel();
            IEnumerable<CRM_KH_BANHANGViewModel> q = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG()
                .Select(item => new CRM_KH_BANHANGViewModel
                {
                    KH_BANHANG_ID = item.KH_BANHANG_ID,
                    BranchId = item.BranchId,
                    IsDeleted = item.IsDeleted,
                    CreatedDate = item.CreatedDate,
                    CreatedUserId = item.CreatedUserId,
                    ModifiedDate = item.ModifiedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    NGUOILAP_ID = item.NGUOILAP_ID,
                    TARGET_BRAND = item.TARGET_BRAND,
                    GHI_CHU = item.GHI_CHU,
                    CountForBrand = item.CountForBrand,
                    THANG = item.THANG,
                    NAM = item.NAM,
                }).Where(x => x.NGUOILAP_ID == id && x.THANG == month && x.NAM == year).ToList();
            foreach (var item in q)
            {
                model.BranchId = item.BranchId;
                model.IsDeleted = item.IsDeleted;
                model.CreatedDate = item.CreatedDate;
                model.CreatedUserId = item.CreatedUserId;
                model.ModifiedDate = item.ModifiedDate;
                model.ModifiedUserId = item.ModifiedUserId;
                model.CountForBrand = item.CountForBrand;
                model.GHI_CHU = item.GHI_CHU;
            }
            if (id != null)
            {
                var user = userRepository.GetUserById(id);
                model.NGUOILAP_ID = user.Id;
                model.FullName = user.FullName;
            }
            else
            {
                var user = userRepository.GetUserById(WebSecurity.CurrentUserId);
                model.NGUOILAP_ID = user.Id;
                model.FullName = user.FullName;
            }
            model.CRM_KH_BANHANGList = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Select(x => new CRM_KH_BANHANG_CTIETViewModel
            {
                KH_BANHANG_ID = x.KH_BANHANG_ID,
                KH_BANHANG_CTIET_ID = x.KH_BANHANG_CTIET_ID,
                Phone = x.Phone,
                KHACHHANG_ID = x.KHACHHANG_ID,
                CompanyName = x.CompanyName,
                Code = x.Code,
                TYLE_THANHCONG = x.TYLE_THANHCONG,
                TYLE_THANHCONG_REVIEW = x.TYLE_THANHCONG_REVIEW,
                GHI_CHU = x.GHI_CHU,
                IsDeleted = x.IsDeleted,
                THANG = x.THANG,
                NAM = x.NAM,
                CRM_Sale_ProductInvoiceCode = x.CRM_Sale_ProductInvoiceCode,
                CountForBrand = x.CountForBrand,
                TotalAmount = x.TotalAmount,
                ProductInvoiceId = x.ProductInvoiceId,
                NGUOILAP_ID = x.NGUOILAP_ID,
                NOIDUNG = x.NOIDUNG,
            }).Where(x => x.IsDeleted == false && x.THANG == month && x.NAM == year && x.NGUOILAP_ID == id).ToList();

            //Tạo table html chi tiết phiếu xuất
            //string detailLists = "";
            //detailLists += "<thead>\r\n";
            //detailLists += "	<tr>\r\n";
            //detailLists += "		<th>STT</th>\r\n";
            //detailLists += "		<th>Tên khách hàng</th>\r\n";
            //detailLists += "		<th>Mã khách hàng</th>\r\n";
            //detailLists += "		<th>Điện thoại</th>\r\n";
            //detailLists += "		<th>Thông tin lưu ý</th>\r\n";
            //detailLists += "		<th>Tỷ lệ thành công(%)</th>\r\n";
            //detailLists += "		<th>Ghi chú</th>\r\n";
            //detailLists += "		<th>Đơn hàng PS</th>\r\n";
            //detailLists += "		<th>Đánh giá lại</th>\r\n";
            //detailLists += "		<th>Lịch sử tương tác</th>\r\n";
            //detailLists += "		<th>Đơn hàng dự kiến</th>\r\n";
            //detailLists += "	</tr>\r\n";
            //detailLists += "</thead>\r\n";

            dt.Rows.Add("STT", "Tên khách hàng", "Mã khách hàng", "Điện thoại", "Thông tin lưu ý", "Tỷ lệ thành công(%)", "Ghi chú", "Đơn hàng PS", "Đánh giá lại", "Lịch sử tương tác", "Đơn hàng dự kiến");

            var index = 1;


            foreach (var item in model.CRM_KH_BANHANGList)
            {
                if (item.CountForBrand.Replace(" ", "") == brand)
                {
                    dt.Rows.Add((index++), item.CompanyName, item.KHACHHANG_ID, item.Phone, item.NOIDUNG, item.TYLE_THANHCONG, item.GHI_CHU, " ", item.TYLE_THANHCONG_REVIEW, " ", item.TotalAmount);
                    //detailLists += "<tr>\r\n"
                    //+ "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                    //+ "<td class=\"text-left \">" + item.CompanyName + "</td>\r\n"
                    //+ "<td class=\"text-left \">" + item.KHACHHANG_ID + "</td>\r\n"
                    //+ "<td class=\"text-left \">" + item.Phone + "</td>\r\n"
                    //+ "<td class=\"text-left \">" + item.NOIDUNG + "</td>\r\n"
                    //+ "<td class=\"text-left \">" + item.TYLE_THANHCONG + "</td>\r\n"
                    //+ "<td class=\"text-right\">" + item.GHI_CHU + "</td>\r\n"
                    //+ "<td class=\"text-right\">" +  " "+ "</td>\r\n"
                    //+ "<td class=\"text-center\">" + item.TYLE_THANHCONG_REVIEW + "</td>\r\n"
                    //+ "<td class=\"text-center\">" + item.TotalAmount + "</td>\r\n"
                    //+ "</tr>\r\n";
                }
            }
        }
        #endregion

        #region Cập nhật tỉ lệ thành công
        [HttpPost]
        public JsonResult UpdateRate(int id, int khid, int tyle, int tylerw)
        {
            var CRM_KH_BANHANG_CT = CRM_KH_BANHANGRepository.GetCRM_KH_BANHANG_CTById(id);
            CRM_KH_BANHANG_CT.TYLE_THANHCONG = tyle;
            CRM_KH_BANHANG_CT.TYLE_THANHCONG_REVIEW = tylerw;
            CRM_KH_BANHANG_CT.ModifiedDate = DateTime.Now;
            CRM_KH_BANHANG_CT.ModifiedUserId = WebSecurity.CurrentUserId;
            CRM_KH_BANHANGRepository.UpdateCRM_KH_BANHANG_CT(CRM_KH_BANHANG_CT);
            return Json(1);
        }
        #endregion



        #region XoaKH
        [HttpPost]
        public ActionResult DeleteKH(int? month, int? year)
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
            string idDeleteAll = Request["DeleteId-checkbox"];
            //string gdall = Request["selectgd"];
            string[] arrDeleteId = idDeleteAll.Split(',');

            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    var id = int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture);
                    var kh_banhang = CRM_KH_BANHANGRepository.GetAllCRM_KH_BANHANG().Where(x => x.NAM == year &&
                    x.THANG == month && x.NGUOILAP_ID == id && x.BranchId == intBrandID).ToList();
                    if (kh_banhang.Count > 0)
                    {
                        foreach (var item in kh_banhang)
                        {
                            var chitiet = CRM_KH_BANHANGRepository.GetAllvwCRM_KH_BANHANG_CTIET().Where(x => x.KH_BANHANG_ID == item.KH_BANHANG_ID).ToList();
                            if (chitiet.Count > 0)
                            {
                                foreach (var item2 in chitiet)
                                {
                                    CRM_KH_BANHANG_CTIETRepository.DeleteCRM_KH_BANHANG_CTIETRs(item2.KH_BANHANG_CTIET_ID);
                                }
                            }
                            CRM_KH_BANHANGRepository.DeleteCRM_KH_BANHANGRs(item.KH_BANHANG_ID);
                        }
                    }


                }

                scope.Complete();
            }
            TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
            return RedirectToAction("Index");
        }
        #endregion
    }
}
