using System.Globalization;
using Erp.BackOffice.Filters;
using Erp.Domain.Staff.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Staff.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Staff.Models;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Helpers;
//using Erp.Domain.Helper;
using Newtonsoft.Json;
using Erp.Domain.Account.Interfaces;
using Erp.BackOffice.Account.Models;
using System.Data;
using System.Web;
using Erp.Domain.Account.Helper;
using Erp.BackOffice.Areas.Sale.Models;
using Erp.Domain.Sale.Repositories;
using System.Text.RegularExpressions;
using Erp.BackOffice.Areas.Cms.Models;
using System.Transactions;
using PagedList;
using Erp.BackOffice.Areas.Administration.Models;
using ClosedXML.Excel;
using System.IO;
using Erp.BackOffice.Helpers;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class SaleReportController : Controller
    {
        private readonly IBranchRepository BranchRepository;
        private readonly IUserRepository userRepository;
        private readonly IBranchDepartmentRepository branchDepartmentRepository;
        private readonly IStaffsRepository staffRepository;
        private readonly ISaleReportRepository saleReportRepository;
        private readonly IProductInvoiceRepository invoiceRepository;
        private readonly IPurchaseOrderRepository purchaseOrderRepository;
        private readonly IWarehouseRepository warehouseRepository;
        private readonly IProductInboundRepository inboundRepository;
        private readonly IProductOutboundRepository outboundRepository;
        private readonly ISalesReturnsRepository salesReturnsRepository;
        private readonly ICustomerRepository customerRepository;
        private readonly IQueryHelper QueryHelper;
        private readonly IRequestInboundRepository requestInboundRepository;
        private readonly IProductOrServiceRepository ProductRepository;
        private readonly IInventoryRepository inventoryRepository;
        private readonly ICommisionStaffRepository commisionStaffRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        private readonly ITotalDiscountMoneyNTRepository TotalDiscountMoneyNTRepository;
        private readonly ISettingRepository settingRepository;
        private readonly IProductOrServiceRepository productOrServiceRepository;
        private readonly IProductSampleRepository productSampleRepository;
        private readonly IProductInvoiceRepository productInvoiceRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IBC_DOANHSO_NHANHANGRepository BC_DOANHSO_NHANHANGRepository;        //private readonly IStaffMadeRepository staffMadeRepository;
        private readonly IHOAHONG_NVKDRepository HOAHONG_NVKDRepository;
        private readonly IReceiptRepository ReceiptRepository;

        public SaleReportController(
            IBranchRepository _Branch
            , IUserRepository _user
            , IBranchDepartmentRepository branchDepartment
            , IStaffsRepository staff
            , ISaleReportRepository saleReport
            , IProductInvoiceRepository invoice
            , IPurchaseOrderRepository purchaseOrder
            , IWarehouseRepository warehouse
            , IProductInboundRepository inbound
            , IProductOutboundRepository outbound
            , ISalesReturnsRepository salesReturns
            , IQueryHelper _QueryHelper
             , ICustomerRepository _Customer
            , IRequestInboundRepository requestInbound
            , IProductOrServiceRepository product
            , IInventoryRepository inventory
            , ICommisionStaffRepository commisionStaff
            , ITemplatePrintRepository templatePrint
            , ITotalDiscountMoneyNTRepository totalDiscountMoneyNT
            , ISettingRepository setting
            , IProductInvoiceRepository productInvoice
            , ICategoryRepository category
            , IBC_DOANHSO_NHANHANGRepository BC_DOANHSO_NHANHANG
            , IHOAHONG_NVKDRepository HOAHONG_NVKD
            , IReceiptRepository _Receipt
            )
        {
            BranchRepository = _Branch;
            userRepository = _user;
            branchDepartmentRepository = branchDepartment;
            staffRepository = staff;
            saleReportRepository = saleReport;
            invoiceRepository = invoice;
            purchaseOrderRepository = purchaseOrder;
            warehouseRepository = warehouse;
            inboundRepository = inbound;
            outboundRepository = outbound;
            salesReturnsRepository = salesReturns;
            customerRepository = _Customer;
            QueryHelper = _QueryHelper;
            requestInboundRepository = requestInbound;
            ProductRepository = product;
            inventoryRepository = inventory;
            commisionStaffRepository = commisionStaff;
            templatePrintRepository = templatePrint;
            TotalDiscountMoneyNTRepository = totalDiscountMoneyNT;
            settingRepository = setting;
            productInvoiceRepository = productInvoice;
            categoryRepository = category;
            BC_DOANHSO_NHANHANGRepository = BC_DOANHSO_NHANHANG;
            HOAHONG_NVKDRepository = HOAHONG_NVKD;
            ReceiptRepository = _Receipt;
        }

        #region Báo cáo kho
        public ActionResult Inventory()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse();
            //.Where(x => ("," + Helpers.Common.CurrentUser.DrugStore + ",").Contains("," + x.BranchId + ",") == true);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();
        }
        public ActionResult BaoCaoNhapXuatTon()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse();
            //.Where(x => ("," + Helpers.Common.CurrentUser.DrugStore + ",").Contains("," + x.BranchId + ",") == true);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();

        }
        public ActionResult BaoCaoXuatKho()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse();
            //.Where(x => ("," + Helpers.Common.CurrentUser.DrugStore + ",").Contains("," + x.BranchId + ",") == true);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();
        }
        public ActionResult ChartInboundAndOutboundInMonth(string single, int? year, int? month, int? quarter, int? week, string group, string branchId)
        {
            var model = new ChartInboundAndOutboundInMonthViewModel();
            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            group = string.IsNullOrEmpty(group) ? "" : group;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            branchId = branchId == null ? "" : branchId.ToString();
            //if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan() && string.IsNullOrEmpty(branchId))
            //{
            //    branchId = Helpers.Common.CurrentUser.DrugStore;
            //}

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;
            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
            var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

            var dataInbound = SqlHelper.QuerySP<ChartItem>("spSale_ReportProductInbound", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductGroup = group,
                branchId = branchId
            });

            var dataOutbound = SqlHelper.QuerySP<ChartItem>("spSale_ReportProductOutbound", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductGroup = group,
                branchId = branchId
            });

            //Xử lý dữ liệu
            if (dataInbound.Count() > 0)
            {
                foreach (var item in dataInbound)
                {
                    if (!string.IsNullOrEmpty(item.label))
                    {
                        item.label = item.label.Trim().Replace("\t", "");
                    }
                }
            }

            if (dataOutbound.Count() > 0)
            {
                foreach (var item in dataOutbound)
                {
                    if (!string.IsNullOrEmpty(item.label))
                    {
                        item.label = item.label.Trim().Replace("\t", "");
                    }
                }
            }

            var category = dataInbound.Select(item => item.label).Union(dataOutbound.Select(item => item.label));
            var qGroupTemp = dataInbound.Select(item => new { GroupName = item.group, NumberOfInbound = Convert.ToInt32(item.data), NumberOfOutbound = 0 })
                .Union(dataOutbound.Select(item => new { GroupName = item.group, NumberOfInbound = 0, NumberOfOutbound = Convert.ToInt32(item.data) }));

            //Thống kế theo nhóm sản phẩm
            var qGroup = qGroupTemp.GroupBy(
                item => item.GroupName,
                (key, g) => new
                {
                    GroupName = key,
                    NumberOfInbound = g.Sum(i => i.NumberOfInbound),
                    NumberOfOutbound = g.Sum(i => i.NumberOfOutbound)
                }
            );

            model.jsonInbound = JsonConvert.SerializeObject(dataInbound);

            model.jsonOutbound = JsonConvert.SerializeObject(dataOutbound);

            model.jsonCategory = JsonConvert.SerializeObject(category);

            model.GroupList = qGroup.ToDataTable();

            if (dataInbound.Count() > 0)
            {
                model.TongNhap = dataInbound.Sum(item => Convert.ToInt32(item.data));
            }
            else
            {
                model.TongNhap = 0;
            }

            if (dataOutbound.Count() > 0)
            {
                model.TongXuat = dataOutbound.Sum(item => Convert.ToInt32(item.data));
            }
            else
            {
                model.TongXuat = 0;
            }

            model.Year = year.Value;
            model.Month = month.Value;
            model.Single = single;
            model.Week = week.Value;
            return View(model);
        }
        public ActionResult RequestInbound(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, string branchId, int? WarehouseId)
        {
            SaleReportRequestInboundViewModel model = new SaleReportRequestInboundViewModel();
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = string.IsNullOrEmpty(branchId) ? "" : branchId;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            //if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan() && string.IsNullOrEmpty(branchId))
            //{
            //    branchId = Helpers.Common.CurrentUser.DrugStore;
            //}
            var jsonData = new List<ChartItem>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var qThongKeYeuCau = requestInboundRepository.GetAllvwRequestInbound()
                   .Where(x => x.CreatedDate > StartDate
                       && x.CreatedDate < EndDate).ToList();
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
            if (!string.IsNullOrEmpty(branchId))
            {
                qThongKeYeuCau = qThongKeYeuCau.Where(item => ("," + branchId + ",").Contains("," + item.BranchId + ",") == true).ToList();
            }

            //Thống kê đơn hàng khởi tạo/đang xử lý
            model.NumberOfRequestInbound = qThongKeYeuCau.Count();
            model.NumberOfRequestInbound_Error_success = qThongKeYeuCau.Where(item => item.Error == true && (item.ErrorQuantity.Value <= 0)).Count();
            model.NumberOfRequestInbound_Error_no_success = qThongKeYeuCau.Where(item => item.Error == true && item.ErrorQuantity.Value > 0).Count();
            model.NumberOfRequestInbound_Error = qThongKeYeuCau.Where(item => item.Error == true).Count();
            model.NumberOfRequestInbound_inbound_complete = qThongKeYeuCau.Where(x => x.Status == "inbound_complete").Count();
            model.NumberOfRequestInbound_InProgress = qThongKeYeuCau.Where(x => x.Status == "ApprovedASM").Count();
            model.NumberOfRequestInbound_new = qThongKeYeuCau.Where(x => x.Status == "new").Count();
            model.NumberOfRequestInbound_refure = qThongKeYeuCau.Where(x => x.Status == "refure").Count();
            model.NumberOfRequestInbound_shipping = qThongKeYeuCau.Where(x => x.Status == "shipping").Count();
            model.NumberOfRequestInbound_wait_shipping = qThongKeYeuCau.Where(x => x.Status == "ApprovedKT").Count();
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.branchId = branchId;
            return View(model);
        }
        #endregion

        #region Báo cáo bán hàng/doanh thu
        public ActionResult Summary(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId)
        {
            SaleReportSumaryViewModel model = new SaleReportSumaryViewModel();

            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
            //
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
            //Thống kê doanh thu bán hàng
            var qProductInvoice = invoiceRepository.GetAllvwProductInvoice().AsEnumerable()
                .Where(x => x.IsArchive == true && x.CreatedDate > StartDate && x.CreatedDate < EndDate).ToList();

            //if (!string.IsNullOrEmpty(CityId))
            //{
            //    qProductInvoice = qProductInvoice.Where(item => item.CityId == CityId);
            //}
            //if (!string.IsNullOrEmpty(DistrictId))
            //{
            //    qProductInvoice = qProductInvoice.Where(item => item.DistrictId == DistrictId);
            //}
            if (branchId > 0)
            {
                qProductInvoice = qProductInvoice.Where(item => item.BranchId == branchId).ToList();
            }
            // Thống kê đơn hàng khởi tạo/đang xử lý
            //model.NumberOfProductInvoice_Pendding = qProductInvoice.AsEnumerable()
            // .Where(x => x.Status == App_GlobalResources.Wording.OrderStatus_pending).Count();
            //model.NumberOfProductInvoice_InProgress = qProductInvoice.AsEnumerable()
            // .Where(x => x.Status == App_GlobalResources.Wording.OrderStatus_inprogress).Count();

            if (qProductInvoice.Count() > 0)
            {
                decimal AmountCommissionStaff = 0;

                var Revenue = qProductInvoice.Sum(item => item.TotalAmount);
                var NumberOfProductInvoice = qProductInvoice.Count();
                var TotalDayInvoice = qProductInvoice.GroupBy(x => new { x.Day, x.Month, x.Year }).ToList();
                model.Revenue = Revenue;
                model.NumberOfProductInvoice = NumberOfProductInvoice;
                model.TotalDayInvoice = TotalDayInvoice.Count() > 0 ? TotalDayInvoice.Count() : 0;
                model.AmountCommissionStaff = AmountCommissionStaff;
            }

            var qThongKeBanHang_TheoKhachHang = new List<ChartItem>();
            var title = "";
            if (branchId == null)
            {
                if (string.IsNullOrEmpty(DistrictId))
                {
                    if (string.IsNullOrEmpty(CityId))
                    {
                        //Thống kê bán hàng theo khách hàng
                        //qThongKeBanHang_TheoKhachHang = qProductInvoice.Where(item => item.TotalAmount > 0).Select(item => new { item.CityId, item.ProvinceName, item.TotalAmount })
                        //   .ToList()
                        //   .GroupBy(l => new { l.CityId, l.ProvinceName })
                        //   .Select(cl => new ChartItem
                        //   {
                        //       label = cl.Key.ProvinceName,
                        //       data = cl.Sum(i => i.TotalAmount),
                        //       id = cl.Key.CityId,
                        //   }).ToList();
                        title = "Thống kê doanh số theo Tỉnh/Thành phố";
                    }
                    else
                    {
                        //Thống kê bán hàng theo khách hàng
                        //qThongKeBanHang_TheoKhachHang = qProductInvoice.Where(item => item.TotalAmount > 0).Select(item => new { item.DistrictId, item.DistrictName, item.TotalAmount })
                        //   .ToList()
                        //   .GroupBy(l => new { l.DistrictId, l.DistrictName })
                        //   .Select(cl => new ChartItem
                        //   {
                        //       label = cl.Key.DistrictName,
                        //       data = cl.Sum(i => i.TotalAmount),
                        //       id = cl.Key.DistrictId,
                        //   }).ToList();
                        title = "Thống kê doanh số theo Quận/Huyện";
                    }

                }
                else
                {
                    //Thống kê bán hàng theo khách hàng
                    qThongKeBanHang_TheoKhachHang = qProductInvoice.Where(item => item.TotalAmount > 0).Select(item => new { item.BranchId, item.BranchName, item.TotalAmount })
                       .ToList()
                       .GroupBy(l => new { l.BranchId, l.BranchName })
                       .Select(cl => new ChartItem
                       {
                           label = cl.Key.BranchName,
                           data = cl.Sum(i => i.TotalAmount),
                           id = cl.Key.BranchId.ToString(),
                       }).ToList();
                    title = "Thống kê doanh số theo chi nhánh";
                }

            }
            else
            {
                //Thống kê bán hàng theo khách hàng
                //qThongKeBanHang_TheoKhachHang = qProductInvoice.Where(item => item.TotalAmount > 0).Select(item => new { item.CustomerId, item.CustomerName, item.TotalAmount })
                //   .ToList()
                //   .GroupBy(l => new { l.CustomerId, l.CustomerName })
                //   .Select(cl => new ChartItem
                //   {
                //       label = cl.Key.CustomerName,
                //       data = cl.Sum(i => i.TotalAmount),
                //       id = cl.Key.CustomerId.Value.ToString(),
                //   }).ToList();
                title = "Thống kê doanh số theo khách hàng";
            }
            //Thống kê bán hàng theo nhân viên bán
            var qThongKeBanHang_TheoNhanVien = qProductInvoice.Where(item => item.TotalAmount > 0).Select(item => new { item.CreatedUserId, item.StaffCreateName, item.TotalAmount })
                .ToList()
                .GroupBy(l => l.CreatedUserId)
                .Select(cl => new ChartItem
                {
                    label = cl.FirstOrDefault().StaffCreateName,
                    data = cl.Sum(i => i.TotalAmount).ToString(),
                }).ToList();
            ViewBag.jsonThongKeBanHang_TheoKhachHang = JsonConvert.SerializeObject(qThongKeBanHang_TheoKhachHang);
            ViewBag.jsonThongKeBanHang_TheoNhanVien = JsonConvert.SerializeObject(qThongKeBanHang_TheoNhanVien);
            ViewBag.Title_qThongKeBanHang_TheoKhachHang = title;
            ViewBag.single = single;
            ViewBag.Year = year;
            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");
            return View(model);
        }

        public ActionResult ChartInvoiceDayInMonth(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId)
        {
            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
            var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

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
            //
            if (single == "month" || single == "week")
            {
                var data = SqlHelper.QuerySP<ChartItem>("spSale_ChartInvoiceDayInMonth", new
                {
                    StartDate = d_startDate,
                    EndDate = d_endDate,
                    branchId = branchId,
                    CityId = CityId,
                    DistrictId = DistrictId
                });

                var jsonData = new List<ChartItem>();
                for (DateTime dt = StartDate; dt <= EndDate; dt = dt.AddDays(1))
                {
                    string label = dt.Day + "/" + dt.Month;
                    var obj = data.Where(item => item.label == label).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new ChartItem();
                        obj.label = label;
                        obj.data = 0;
                    }

                    jsonData.Add(obj);
                }

                string json = JsonConvert.SerializeObject(jsonData);
                ViewBag.json = json;
            }
            else
            {
                var data = SqlHelper.QuerySP<ChartItem>("spSale_ChartInvoiceDayInMonth2", new
                {
                    StartDate = d_startDate,
                    EndDate = d_endDate,
                    branchId = branchId,
                    CityId = CityId,
                    DistrictId = DistrictId
                });

                var jsonData = new List<ChartItem>();
                for (int i = StartDate.Month; i <= EndDate.Month; i++)
                {
                    string label = i + "/" + year.Value;
                    var obj = data.Where(item => item.label == label).FirstOrDefault();
                    if (obj == null)
                    {
                        obj = new ChartItem();
                        obj.label = label;
                        obj.data = 0;
                    }

                    jsonData.Add(obj);
                }

                string json = JsonConvert.SerializeObject(jsonData);
                ViewBag.json = json;
            }

            return View();
        }

        public ActionResult ChartProductSaleInMonth(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId)
        {
            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
            var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
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
            //

            var data = SqlHelper.QuerySP<ChartItem>("spSale_ChartProductSaleInMonth", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            });

            string json = JsonConvert.SerializeObject(data);
            ViewBag.json = json;

            return View();
        }

        public ActionResult ChartServiceSaleInMonth(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId)
        {
            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
            var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

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
            //

            var data = SqlHelper.QuerySP<ChartItem>("spSale_ChartServiceSaleInMonth", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            });

            string json = JsonConvert.SerializeObject(data);
            ViewBag.json = json;

            return View();
        }
        public ActionResult Commision()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
                SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }

        public ActionResult BaoCaoBanHang()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }
        public ActionResult InvoiceByBranch()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
                SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }

        public ActionResult InvoiceByCustomer()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
                SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }

        public ActionResult BaoCaoDonHang()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }
        public ActionResult BaoCaoSoLuongBan()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }
        public ActionResult BaoCaoHangBanTraLai()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }

        #endregion

        #region Báo cáo thu/chi/công nợ
        public ActionResult ReceiptList()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }
        public ActionResult ReceiptListCustomer()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            //Danh sách nhân viên sale
            var SaleList = userRepository.GetUserbyUserType("Sales Excutive").Select(item => new { item.FullName, item.Id }).ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id + ""
                });

            ViewBag.SaleList = SaleList;
            //Danh sách khách hàng
            ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);//customerRepository.GetAllCustomer().Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId.Value)
            //.Select(item => new SelectListItem
            //{
            //    Value = item.Id + "",
            //    Text = item.CompanyName
            //}).ToList();

            //ViewBag.customerList = customerList;
            return View();
        }
        public ActionResult PaymentList()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }

        public ActionResult BaoCaoCongNoTongHop()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            //Danh sách nhân viên sale
            var SaleList = userRepository.GetUserbyUserType("Sales Excutive").Select(item => new { item.FullName, item.Id }).ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id + ""
                });

            ViewBag.SaleList = SaleList;
            //Danh sách khách hàng
            ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }

        public ActionResult BaoCaoCongNoChiTiet()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }

        public ActionResult BaoCaoDoanhThu()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            //Danh sách nhân viên sale
            var SaleList = userRepository.GetUserbyUserType("Sales Excutive").Select(item => new { item.FullName, item.Id }).ToList()
                .Select(x => new SelectListItem
                {
                    Text = x.FullName,
                    Value = x.Id + ""
                });

            ViewBag.SaleList = SaleList;
            //Danh sách khách hàng
            ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }

        public ActionResult BaoCaoCongNoPhaiThu()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            //Danh sách khách hàng
            ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }
        public ActionResult BaoCaoTongChi()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            return View();
        }

        public ActionResult BaoCaoCongNoTongHopNCC()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            ////Danh sách nhân viên sale
            //var SaleList = userRepository.GetUserbyUserType("Sales Excutive").Select(item => new { item.FullName, item.Id }).ToList()
            //    .Select(x => new SelectListItem
            //    {
            //        Text = x.FullName,
            //        Value = x.Id + ""
            //    });

            //ViewBag.SaleList = SaleList;
            //Danh sách khách hàng
            //ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }

        public ActionResult BaoCaoCongNoPhaiTraNCC()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
               SelectListItem
            { Text = x.Name, Value = x.Id + "" });
            ////Danh sách khách hàng
            //ViewBag.customerList = SelectListHelper.GetSelectList_Customer(null, null);
            return View();
        }
        #endregion

        #region BaoCaoKhoTheoNgay
        public ActionResult BaoCaoKhoTheoNgay(string group, string category, string manufacturer, int? page, string StartDate, string EndDate, string WarehouseId)
        {
            group = group == null ? "" : group;
            category = category == null ? "" : category;
            manufacturer = manufacturer == null ? "" : manufacturer;
            WarehouseId = WarehouseId == null ? "" : WarehouseId;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;

            if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan() && string.IsNullOrEmpty(WarehouseId))
            {
                WarehouseId = Helpers.Common.CurrentUser.WarehouseId;
            }


            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var data = SqlHelper.QuerySP<spBaoCaoNhapXuatTon_TuanViewModel>("spSale_BaoCaoNhapXuatTon_Tuan", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            }).Where(x => x.ProductType == "product").ToList();
            var product_outbound = SqlHelper.QuerySP<spBaoCaoXuatViewModel>("spSale_BaoCaoXuat", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            }).Where(x => x.ProductType == "product").ToList();
            //var pager = new Pager(data.Count(), page, 20);

            var Items = data.OrderBy(m => m.ProductCode)
              //.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
              .Select(item => new spBaoCaoNhapXuatTon_TuanViewModel
              {
                  CategoryCode = item.CategoryCode,
                  First_InboundQuantity = item.First_InboundQuantity,
                  First_OutboundQuantity = item.First_OutboundQuantity,
                  First_Remain = item.First_Remain,
                  Last_InboundQuantity = item.Last_InboundQuantity,
                  Last_OutboundQuantity = item.Last_OutboundQuantity,
                  ProductCode = item.ProductCode,
                  ProductId = item.ProductId,
                  ProductName = item.ProductName,
                  ProductUnit = item.ProductUnit,
                  Remain = item.Remain,
                  ProductMinInventory = item.ProductMinInventory,
                  LoCode = item.LoCode,
                  ExpiryDate = item.ExpiryDate,
                  WarehouseName = item.WarehouseName,
                  WarehouseId = item.WarehouseId
              }).ToList();

            ViewBag.productInvoiceDetailList = product_outbound;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];


            return View(Items);
        }

        public ActionResult ExportBaoCaoKhoTheoNgay(string StartDate, string EndDate, string manufacturer, string group, string category, string WarehouseId, bool ExportExcel = false)
        {
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlBaoCaoKhoTheoNgay(StartDate, EndDate, manufacturer, group, category, WarehouseId));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo tồn kho theo ngày");

            Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoKhoTheoNgay" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();

            return View(model);
        }

        string buildHtmlBaoCaoKhoTheoNgay(string StartDate, string EndDate, string manufacturer, string group, string category, string WarehouseId)
        {
            group = group == null ? "" : group;
            category = category == null ? "" : category;
            manufacturer = manufacturer == null ? "" : manufacturer;
            WarehouseId = WarehouseId == null ? "" : WarehouseId;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;

            if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan() && string.IsNullOrEmpty(WarehouseId))
            {
                WarehouseId = Helpers.Common.CurrentUser.WarehouseId;
            }


            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var data = SqlHelper.QuerySP<spBaoCaoNhapXuatTon_TuanViewModel>("spSale_BaoCaoNhapXuatTon_Tuan", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            }).Where(x => x.ProductType == "product").ToList();

            var productInvoiceDetailList = SqlHelper.QuerySP<spBaoCaoXuatViewModel>("spSale_BaoCaoXuat", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            }).Where(x => x.ProductType == "product").ToList();

            var Items = data.OrderBy(m => m.ProductCode)
                //.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
              .Select(item => new spBaoCaoNhapXuatTon_TuanViewModel
              {
                  CategoryCode = item.CategoryCode,
                  First_InboundQuantity = item.First_InboundQuantity,
                  First_OutboundQuantity = item.First_OutboundQuantity,
                  First_Remain = item.First_Remain,
                  Last_InboundQuantity = item.Last_InboundQuantity,
                  Last_OutboundQuantity = item.Last_OutboundQuantity,
                  ProductCode = item.ProductCode,
                  ProductId = item.ProductId,
                  ProductName = item.ProductName,
                  ProductUnit = item.ProductUnit,
                  Remain = item.Remain,
                  ProductMinInventory = item.ProductMinInventory,
                  LoCode = item.LoCode,
                  ExpiryDate = item.ExpiryDate,
                  WarehouseName = item.WarehouseName,
                  WarehouseId = item.WarehouseId
              }).ToList();

            var tonkho = Convert.ToInt32(Helpers.Common.GetSetting("TonKhoDinhMuc"));

            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th rowspan=\"2\">Nhóm</th>\r\n";
            detailLists += "		<th rowspan=\"2\">STT</th>\r\n";
            detailLists += "		<th rowspan=\"2\">Mã sản phẩm</th>\r\n";
            detailLists += "		<th rowspan=\"2\">Tên sản phẩm</th>\r\n";
            detailLists += "		<th rowspan=\"2\">Kho</th>\r\n";
            detailLists += "		<th rowspan=\"2\">Số lô</th>\r\n";
            detailLists += "		<th rowspan=\"2\">Hạn dùng</th>\r\n";
            detailLists += "		<th rowspan=\"2\">Đơn vị</th>\r\n";
            detailLists += "		<th>Tồn cũ</th>\r\n";
            detailLists += "		<th>Nhập mới</th>\r\n";
            detailLists += "		<th>Tổng tồn đầu</th>\r\n";
            foreach (var item in productInvoiceDetailList.GroupBy(x => x.ProductOutboundCode).OrderBy(x => x.Key))
            {
                detailLists += "<th>" + item.Key + "<br>" + item.FirstOrDefault().CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") + "</th>";
            }

            detailLists += "		<th>Tổng xuất</th>\r\n";
            detailLists += "		<th>Tổng tồn cuối tuần</th>\r\n";
            detailLists += "		<th rowspan=\"2\">Báo động < " + tonkho + " sản phẩm</th>\r\n";
            detailLists += "	</tr>\r\n";

            detailLists += "	<tr>\r\n";
            detailLists += "        <th>" + Items.Sum(x => x.First_Remain) + "</th>";
            detailLists += "        <th>" + Items.Sum(x => x.Last_InboundQuantity) + "</th>";
            detailLists += "        <th>" + (Items.Sum(x => x.Last_InboundQuantity) + Items.Sum(x => x.First_Remain)) + "</th>";
            foreach (var item in productInvoiceDetailList.GroupBy(x => x.ProductOutboundCode).OrderBy(x => x.Key))
            {
                detailLists += "<th>" + item.Sum(x => x.Quantity) + "</th>";
            }
            detailLists += "        <th>" + Items.Sum(x => x.Last_OutboundQuantity) + "</th>";
            detailLists += "        <th>" + Items.Sum(x => x.Remain) + "</th>";
            detailLists += "	</tr>\r\n";

            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var qq in Items.GroupBy(x => x.CategoryCode))
            {
                var row_count = 0;
                foreach (var item in Items.Where(x => x.CategoryCode == qq.Key))
                {
                    detailLists += "<tr>\r\n";
                    if (row_count == 0)
                    {
                        row_count++;
                        detailLists += "<td class=\"text-left \" rowspan=\"" + Items.Where(x => x.CategoryCode == qq.Key).Count() + "\">" + item.CategoryCode + "</td>\r\n";
                    }

                    detailLists += "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                    + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
                    + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                    + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                    + "<td class=\"text-left \">" + item.LoCode + "</td>\r\n"
                    + "<td class=\"text-left \">" + (item.ExpiryDate == null ? "" : item.ExpiryDate.Value.ToString("dd/MM/yyyy")) + "</td>\r\n"
                    + "<td class=\"text-left \">" + item.ProductUnit + "</td>\r\n"
                    + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.First_Remain) + "</td>\r\n"
                    + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity) + "</td>\r\n"
                    + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity + item.First_Remain) + "</td>\r\n";
                    foreach (var ii in productInvoiceDetailList.GroupBy(x => x.ProductOutboundCode).OrderBy(x => x.Key))
                    {
                        if (ii.Where(x => x.LoCode == item.LoCode && x.ExpiryDate == item.ExpiryDate && x.ProductId == item.ProductId && x.WarehouseSourceId == item.WarehouseId).Count() > 0)
                        {
                            detailLists += "<td class=\"text-right\" style=\"font-weight:bold\">" + (Helpers.Common.PhanCachHangNgan2(ii.Where(x => x.LoCode == item.LoCode && x.ExpiryDate == item.ExpiryDate && x.ProductId == item.ProductId && x.WarehouseSourceId == item.WarehouseId).Sum(x => x.Quantity))) + "</td>";
                        }
                        else
                        {
                            detailLists += "<td class=\"text-right\" style=\"font-weight:bold\"></td>";
                        }
                    }
                    detailLists += "<td class=\"text-right\" style=\"font-weight:bold\">" + (Helpers.Common.PhanCachHangNgan2(item.Last_OutboundQuantity)) + "</td>"
                    + "<td class=\"text-right\" style=\"font-weight:bold\">" + (Helpers.Common.PhanCachHangNgan2(item.Remain)) + "</td>"
                    + "<td class=\"text-right \" style=\"color: red; font-weight:bold\">" + (tonkho >= item.Remain ? "BĐ" : "") + "</td>\r\n"
                    + "</tr>\r\n";
                }
            }
            detailLists += "</tbody>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion
        public ViewResult InventoryWarning(int? WarehouseId)
        {
            var listProduct = ProductRepository.GetAllvwProductByType("product");
            var viewModel = new IndexViewModel<ProductViewModel>
            {
                Items = listProduct.Where(m => m.QuantityTotalInventory < m.MinInventory)
                .Select(item => new ProductViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    PriceInbound = item.PriceInbound,
                    Type = item.Type,
                    Unit = item.Unit,
                    ModifiedDate = item.ModifiedDate,
                    MinInventory = item.MinInventory,
                    QuantityTotalInventory = item.QuantityTotalInventory,
                    CategoryCode = item.CategoryCode,

                }).ToList(),
                //Pager = pager
            };

            List<InventoryViewModel> inventoryList = new List<InventoryViewModel>();
            foreach (var item in viewModel.Items)
            {
                List<InventoryViewModel> inventoryP = inventoryRepository.GetAllvwInventoryByProductId(item.Id)
                .Select(itemV => new InventoryViewModel
                {

                    ProductId = itemV.ProductId,
                    Quantity = itemV.Quantity,
                    WarehouseId = itemV.WarehouseId,
                    ProductCode = itemV.ProductCode,
                    ProductName = itemV.ProductName,
                    WarehouseName = itemV.WarehouseName,
                    CBTK = itemV.CBTK,

                }).Where(u => u.CBTK > 0).ToList();
                if (WarehouseId != null)
                {
                    inventoryP = inventoryP.Where(u => u.WarehouseId == WarehouseId).ToList();
                    if (inventoryP.Count() > 0)
                    {
                        inventoryList.AddRange(inventoryP);
                    }
                }
                else
                {
                    inventoryP = inventoryP.Where(u => u.WarehouseId == 8).ToList();
                    if (inventoryP.Count() > 0)
                    {
                        inventoryList.AddRange(inventoryP);
                    }
                }
                //if (inventoryP.Count > 0)
                //inventoryList.AddRange(inventoryP);
            }
            inventoryList = inventoryList.OrderBy(u => u.Quantity).ToList();
            ViewBag.inventoryList = inventoryList;

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            return View(viewModel);
        }
        public ActionResult ProductInvoiceList(string ProductGroup, string manufacturer, string CategoryCode, int? BranchId, string startDate, string endDate, int? ProductId)
        {
            var q = invoiceRepository.GetAllvwInvoiceDetails().Where(x => x.ProductType == "product" && x.IsArchive == true);

            if (startDate == null && endDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString();
                endDate = retDateTime.ToString();
            }

            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }

            if (BranchId != null && BranchId.Value > 0)
            {
                q = q.Where(x => x.BranchId == BranchId);
            }
            if (!string.IsNullOrEmpty(ProductGroup))
            {
                q = q.Where(x => x.ProductGroup == ProductGroup);
            }
            if (!string.IsNullOrEmpty(manufacturer))
            {
                q = q.Where(x => x.Manufacturer == manufacturer);
            }
            if (!string.IsNullOrEmpty(CategoryCode))
            {
                q = q.Where(x => x.CategoryCode == CategoryCode);
            }
            if (ProductId != null && ProductId.Value > 0)
            {
                q = q.Where(x => x.ProductId == ProductId);
            }
            var model = q.Select(item => new ProductInvoiceDetailViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Amount = item.Amount,
                CategoryCode = item.CategoryCode,
                ProductType = item.ProductType,
                ProductCode = item.ProductCode,
                ProductGroup = item.ProductGroup,
                ProductInvoiceCode = item.ProductInvoiceCode,
                Price = item.Price,
                ProductId = item.ProductId,
                ProductInvoiceDate = item.ProductInvoiceDate,
                ProductInvoiceId = item.ProductInvoiceId,
                ProductName = item.ProductName,
                Quantity = item.Quantity,
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                BranchId = item.BranchId,
                BranchName = item.BranchName
            }).OrderByDescending(m => m.ModifiedDate).ToList();
            return View(model);
        }

        public ActionResult XuatExcel(string html)
        {
            Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoTonKho" + DateTime.Now.ToString("dd_MM_yyyy") + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(html);
            Response.End();
            return Content("success");
        }
        #region Báo cáo mua hàng
        public ActionResult PurchaseOrderBySupplier()
        {
            ViewBag.branchList = BranchRepository.GetAllBranch().AsEnumerable().Select(x => new
                SelectListItem
            { Text = x.Name, Value = x.Id + "" });

            return View();
        }
        #endregion

        public ActionResult InventoryQueryExpiryDate()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse()
                .Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();
        }


        public ActionResult CommisionStaff(string BranchId, int? year, int? month, int? StaffId)
        {
            var q = commisionStaffRepository.GetAllvwCommisionStaff();
            if (year != null && year.Value > 0)
            {
                q = q.Where(x => x.year == year);
            }
            else
            {
                q = q.Where(x => x.year == DateTime.Now.Year);
            }
            if (!string.IsNullOrEmpty(BranchId))
            {
                q = q.Where(x => ("," + BranchId + ",").Contains("," + x.BranchId + ",") == true);
            }
            if (year != null && year.Value > 0)
            {
                q = q.Where(x => x.year == year);
            }
            if (month != null && month.Value > 0)
            {
                q = q.Where(x => x.month == month);
            }
            if (StaffId != null && StaffId.Value > 0)
            {
                q = q.Where(x => x.StaffId == StaffId);
            }
            var model = q.Select(item => new CommisionStaffViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                AmountOfCommision = item.AmountOfCommision,
                BranchId = item.BranchId,
                BranchName = item.BranchName,
                InvoiceDetailId = item.InvoiceDetailId,
                InvoiceId = item.InvoiceId,
                InvoiceType = item.InvoiceType,
                IsResolved = item.IsResolved,
                month = item.month,
                Note = item.Note,
                PercentOfCommision = item.PercentOfCommision,
                ProductCode = item.ProductCode,
                ProductImage = item.ProductImage,
                ProductInvoiceCode = item.ProductInvoiceCode,
                ProductName = item.ProductName,
                StaffCode = item.StaffCode,
                StaffId = item.StaffId,
                StaffName = item.StaffName,
                StaffProfileImage = item.StaffProfileImage,
                year = item.year
            }).OrderByDescending(m => m.CreatedDate).ToList();
            return View(model);
        }
        public ActionResult ListCommisionStaff(int? StaffId)
        {
            var q = commisionStaffRepository.GetAllvwCommisionStaff();
            q = q.Where(x => x.year == DateTime.Now.Year);
            if (StaffId != null && StaffId.Value > 0)
            {
                q = q.Where(x => x.StaffId == StaffId);
            }
            var model = q.Select(item => new CommisionStaffViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                AmountOfCommision = item.AmountOfCommision,
                BranchId = item.BranchId,
                BranchName = item.BranchName,
                InvoiceDetailId = item.InvoiceDetailId,
                InvoiceId = item.InvoiceId,
                InvoiceType = item.InvoiceType,
                IsResolved = item.IsResolved,
                month = item.month,
                Note = item.Note,
                PercentOfCommision = item.PercentOfCommision,
                ProductCode = item.ProductCode,
                ProductImage = item.ProductImage,
                ProductInvoiceCode = item.ProductInvoiceCode,
                ProductName = item.ProductName,
                StaffCode = item.StaffCode,
                StaffId = item.StaffId,
                StaffName = item.StaffName,
                StaffProfileImage = item.StaffProfileImage,
                year = item.year

            }).OrderByDescending(m => m.CreatedDate).ToList();
            return View(model);
        }

        #region DoanhSOTheoNgay
        public ActionResult BaoCaoDoanhThuTheoNgay(string startDate, string endDate, string CityId, string DistrictId, int? branchId)
        {
            var q = invoiceRepository.GetAllvwProductInvoice().Where(x => x.IsArchive == true || x.Status == "Đặt cọc");
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
            if (startDate == null && endDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }
            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    ViewBag.retDateTime = d_endDate;
                    ViewBag.aDateTime = d_startDate;
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }
            var branch_list = BranchRepository.GetAllBranch().Select(item => new BranchViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Code = item.Code,
                DistrictId = item.DistrictId,
                CityId = item.CityId
            }).ToList();
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
            //
            if (branchId != null && branchId.Value > 0)
            {
                q = q.Where(x => x.BranchId == branchId);
                branch_list = branch_list.Where(x => x.Id == branchId).ToList();
            }
            if (!string.IsNullOrEmpty(CityId))
            {
                q = q.Where(x => x.CityId == CityId);
                branch_list = branch_list.Where(x => x.CityId == CityId).ToList();
            }
            if (!string.IsNullOrEmpty(DistrictId))
            {
                q = q.Where(x => x.DistrictId == DistrictId);
                branch_list = branch_list.Where(x => x.DistrictId == DistrictId).ToList();
            }
            var model = q.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedDate = item.CreatedDate,
                BranchId = item.BranchId,
                TotalAmount = item.TotalAmount,
                RemainingAmount = item.RemainingAmount,
                PaidAmount = item.PaidAmount,
                Month = item.Month,
                Day = item.Day,
                Year = item.Year
            }).OrderByDescending(m => m.CreatedDate).ToList();

            // xử lý hủy chuyển
            foreach (var item in model)
            {
                var dcchuyen = invoiceRepository.GetMMByNewId(item.Id);
                if (dcchuyen != null)
                {
                    var bichuyen = invoiceRepository.GetvwProductInvoiceById(dcchuyen.FromProductInvoiceId.Value);
                    if (bichuyen.IsArchive == true)
                    {
                        item.PaidAmount = (decimal)(item.PaidAmount - dcchuyen.TotalAmount);
                        item.TotalAmount = (decimal)(item.TotalAmount - dcchuyen.TotalAmount);
                    }
                }
            }

            //if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan())
            //{
            //    branch_list = branch_list
            //        .Where(x => x.Id == user.BranchId)
            //        .ToList();
            //    model = model
            //        .Where(x => x.BranchId == user.BranchId)
            //        .ToList();
            //}
            ViewBag.branchList = branch_list;
            return View(model);
        }

        public ActionResult ExportDSTheoNgay(string startDate, string endDate, int? branchId)
        {
            var q = invoiceRepository.GetAllvwProductInvoice().Where(x => x.IsArchive == true || x.Status == "Đặt cọc");
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
            if (startDate == null || startDate == "" && endDate == null || endDate == "")
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }
            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    ViewBag.retDateTime = d_endDate;
                    ViewBag.aDateTime = d_startDate;
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }
            var branch_list = BranchRepository.GetAllBranch().Select(item => new BranchViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Code = item.Code,
                DistrictId = item.DistrictId,
                CityId = item.CityId
            }).ToList();
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
            //
            if (branchId != null && branchId.Value > 0)
            {
                q = q.Where(x => x.BranchId == branchId);
                branch_list = branch_list.Where(x => x.Id == branchId).ToList();
            }

            var listinvoice = q.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedDate = item.CreatedDate,
                BranchId = item.BranchId,
                TotalAmount = item.TotalAmount,
                RemainingAmount = item.RemainingAmount,
                PaidAmount = item.PaidAmount,
                Month = item.Month,
                Day = item.Day,
                Year = item.Year
            }).OrderByDescending(m => m.CreatedDate).ToList();

            // xử lý hủy chuyển
            foreach (var item in listinvoice)
            {
                var dcchuyen = invoiceRepository.GetMMByNewId(item.Id);
                if (dcchuyen != null)
                {
                    var bichuyen = invoiceRepository.GetvwProductInvoiceById(dcchuyen.FromProductInvoiceId.Value);
                    if (bichuyen.IsArchive == true)
                    {
                        item.PaidAmount = (decimal)(item.PaidAmount - dcchuyen.TotalAmount);
                        item.TotalAmount = (decimal)(item.TotalAmount - dcchuyen.TotalAmount);
                    }
                }
            }



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
            model.Content = model.Content.Replace("{DataTable}", BuildHtmlDoanhSoTheoNgay(ViewBag.aDateTime, ViewBag.retDateTime, listinvoice, branch_list));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Doanh số theo ngày");

            Response.AppendHeader("content-disposition", "attachment;filename=" + "DoanhSoTheoNgay" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View();
        }

        string BuildHtmlDoanhSoTheoNgay(DateTime d_startDate, DateTime d_endDate, List<ProductInvoiceViewModel> listinvoice, List<BranchViewModel> branch)
        {
            string detailLists = "<table border=\"1\" class=\"invoice-detail\" >\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Chi nhánh</th>\r\n";

            for (DateTime dt = d_startDate; dt <= d_endDate; dt = dt.AddDays(1))
            {
                detailLists += " <th class=\"text-center\" style=\"width:100px;\">" + dt.ToString("dd/MM/yyyy") + "</th>";
            }
            detailLists += "<th class=\"text - center\" style=\"width: 80px; \">Số ngày</th>";
            detailLists += "<th class=\"text-center\" style=\"width:80px;\">Tổng tiền</th> </tr> </thead>";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in branch.OrderBy(x => x.Name))
            {


                detailLists += "<tr>"
                   + "<td>" + index + "</td>" +
                    "<td>" + item.Name + "</td>";
                for (DateTime dt = d_startDate; dt <= d_endDate; dt = dt.AddDays(1))
                {

                    var date = dt.ToString("dd/MM/yyyy");
                    var list = listinvoice.Where(x => x.CreatedDate.Value.ToString("dd/MM/yyyy") == dt.ToString("dd/MM/yyyy") && x.BranchId == item.Id);
                    if (list.Count() > 0)
                    {
                        detailLists += "<td class=\"text-right\">" +

                                    CommonSatic.ToCurrencyStr(list.Sum(a => a.TotalAmount), null).Replace(".", ",") +

                            "</td>";
                    }
                    else
                    {
                        detailLists += "<td class=\"text-right\">0</td>";
                    }
                }
                detailLists += " <td class=\"text-right\">" +
                       listinvoice.Where(x => x.BranchId == item.Id).GroupBy(x => new { x.Day, x.Month, x.Year }).Count() +
                   "</td>" +
                    "<td class=\"text-right\">" +
                        (CommonSatic.ToCurrencyStr(listinvoice.Where(x => x.BranchId == item.Id).Sum(x => x.TotalAmount), null)).Replace(".", ",")
                        +
                    "</td></tr>";
                index++;

            }
            detailLists += "</tbody><tfoot>";
            detailLists += "<tr>" +
                "<td class=\"text-center\" colspan=\"2\">Tổng</td>";
            for (DateTime dt = d_startDate; dt <= d_endDate; dt = dt.AddDays(1))
            {
                var list = listinvoice.Where(x => x.CreatedDate.Value.ToString("dd/MM/yyyy") == dt.ToString("dd/MM/yyyy"));
                if (list.Count() > 0)
                {
                    detailLists += " <td class=\"text-right\">" + (CommonSatic.ToCurrencyStr(list.Sum(x => x.TotalAmount), null)).Replace(".", ",") + "</td>";
                }
                else
                {
                    detailLists += "<td class=\"text-right\">0</td>";
                }
            }
            detailLists += " <td class=\"text-right\"></td>" +
                "<td class=\"text-right\" > " + CommonSatic.ToCurrencyStr(listinvoice.Sum(x => x.TotalAmount), null).Replace(".", ",") + "</td></tr> </tfoot></table>";






            return detailLists;

        }
        #endregion
        public ActionResult DoanhThuNgay(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? BranchId, int? CreatedUserId, int? ProductId, string startDate, string endDate, int? ManagerStaffId, string CustomerName, string Status, string CustomerCode)
        {
            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            CustomerCode = CustomerCode == null ? "" : CustomerCode;
            BranchId = BranchId == null ? 0 : BranchId;
            ProductId = ProductId == null ? 0 : ProductId;
            ManagerStaffId = ManagerStaffId == null ? 0 : ManagerStaffId;
            CustomerName = CustomerName == null ? "" : CustomerName;
            Status = Status == null ? "complete" : Status;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
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
            //

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

            var data = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("spDoanhThuNgay", new
            {
                Status = Status,
                ManagerStaffId = ManagerStaffId,
                CustomerName = CustomerName,
                ProductId = ProductId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                CityId = "",
                DistrictId = ""
            }).OrderByDescending(m => m.CreatedDate).ToList();

            //
            if (Status == "tatca")
            {
                Status = "";

                data = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("spDoanhThuNgay", new
                {
                    Status = Status,
                    ManagerStaffId = ManagerStaffId,
                    CustomerName = CustomerName,
                    ProductId = ProductId,
                    StartDate = d_startDate,
                    EndDate = d_endDate,
                    BranchId = BranchId,
                    CityId = "",
                    DistrictId = ""
                }).OrderByDescending(m => m.CreatedDate).ToList();

                data = data.Where(x => x.Status == "complete" || x.Status == "Đặt cọc").ToList();

            }
            if (!string.IsNullOrEmpty(CustomerName))
            {
                CustomerName = Helpers.Common.ChuyenThanhKhongDau(CustomerName);
                data = data.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(CustomerName)).ToList();
            }
            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "delete")
                {
                    data = data.Where(x => x.IsDeleted == true).ToList();
                }
                //Lấy hóa đơn đã thanh toán hết
                if (Status == "complete")
                {
                    //data = data.Where(x => x.IsArchive == true).ToList();
                    var q = productInvoiceRepository.GetAllvwProductInvoice().Where(x => x.IsArchive == true && (x.TotalDebit - x.TotalCredit == 0)).ToList();
                    var data2 = new List<ProductInvoiceDetailViewModel>();
                    foreach (var i in q)
                    {
                        foreach (var j in data)
                        {
                            if (j.ProductInvoiceCode == i.Code)
                            {
                                data2.Add(j);
                            }
                        }
                    }
                    data = data2;
                }
                if (Status == "pending")
                {
                    data = data.Where(x => x.Status == "pending").ToList();
                }
                if (Status == "Đặt cọc")
                {
                    //var productListId = ProductRepository.GetAllvwProductAndService()
                    //    .Where(item => item.Code == "ĐC").Select(item => item.Id).ToList();

                    //if (productListId.Count > 0)
                    //{
                    //    List<int> listProductInboundId = new List<int>();
                    //    foreach (var id in productListId)
                    //    {
                    //        var list = invoiceRepository.GetAllvwInvoiceDetails().Where(x => x.ProductCode == "ĐC")
                    //            .Select(item => item.ProductInvoiceId.Value).Distinct().ToList();

                    //        listProductInboundId.AddRange(list);
                    //    }

                    //    data = data.Where(item => listProductInboundId.Contains(item.Id)).ToList();
                    data = data.Where(x => x.Status == "Đặt cọc").ToList();
                }



            }
            return View(data);
        }



        public ActionResult DoanhThuNgay_export(string single, int? year, int? month, int? quarter, int? week, int? BranchId, string Status)
        {
            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            BranchId = BranchId == null ? 0 : BranchId;
            Status = Status == null ? "" : Status;
            if (single == "") { single = "month"; }
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

            var data = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("spDoanhThuNgay", new
            {
                Status = Status,
                ManagerStaffId = 0,
                CustomerName = "",
                ProductId = 0,
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                CityId = "",
                DistrictId = ""
            }).OrderByDescending(m => m.CreatedDate).ToList();

            if (!string.IsNullOrEmpty(Status))
            {
                if (Status == "delete")
                {
                    data = data.Where(x => x.IsDeleted == true).ToList();
                }
                if (Status == "complete")
                {
                    //data = data.Where(x => x.Status == "complete").ToList();

                    var q = productInvoiceRepository.GetAllvwProductInvoice().Where(x => x.IsArchive == true && (x.TotalDebit - x.TotalCredit == 0)).ToList();
                    var data2 = new List<ProductInvoiceDetailViewModel>();
                    foreach (var i in q)
                    {
                        foreach (var j in data)
                        {
                            if (j.ProductInvoiceCode == i.Code)
                            {
                                data2.Add(j);
                            }
                        }
                    }
                    data = data2;

                }
                if (Status == "pending")
                {
                    data = data.Where(x => x.Status == "pending").ToList();
                }
                if (Status == "Đặt cọc")
                {
                    //var productListId = ProductRepository.GetAllvwProductAndService()
                    //    .Where(item => item.Code == "ĐC").Select(item => item.Id).ToList();

                    //if (productListId.Count > 0)
                    //{
                    //    List<int> listProductInboundId = new List<int>();
                    //    foreach (var id in productListId)
                    //    {
                    //        var list = invoiceRepository.GetAllvwInvoiceDetails().Where(x => x.ProductCode == "ĐC")
                    //            .Select(item => item.ProductInvoiceId.Value).Distinct().ToList();

                    //        listProductInboundId.AddRange(list);
                    //    }

                    //    data = data.Where(item => listProductInboundId.Contains(item.Id)).ToList();

                    //}
                    data = data.Where(x => x.Status == "Đặt cọc").ToList();
                }
            }

            var model = new TemplatePrintViewModel();
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
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintDoanhso(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo doanh thu");

            Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoDoanhThu" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            //Response.ContentEncoding = System.Text.Encoding.GetEncoding("unicode");
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            // Response.Write("<font style='font-size:11.0pt; font-family:Calibri;'>");
            //Response.Write("<BR><BR><BR>");
            Response.Write(model.Content);
            // Response.Write("</font>");
            Response.Flush();
            Response.Close();
            Response.End();
            return View(model);


        }

        //public ActionResult DoanhThuNgay(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? BranchId, int? CreatedUserId, int? ProductId, string startDate, string endDate)
        //{
        //var q = invoiceRepository.GetAllvwInvoiceDetails().AsEnumerable().Where(x => x.IsArchive == true);
        //    var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
        //    year = year == null ? DateTime.Now.Year : year;
        //    month = month == null ? DateTime.Now.Month : month;
        //    single = single == null ? "month" : single;
        //    quarter = quarter == null ? 1 : quarter;
        //    CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
        //    DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
        //    {
        //        BranchId = Helpers.Common.CurrentUser.BranchId;
        //    }
        //    Calendar calendar = CultureInfo.InvariantCulture.Calendar;
        //    var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        //    week = week == null ? weekdefault : week;
        //    DateTime StartDate = DateTime.Now;
        //    DateTime EndDate = DateTime.Now;

        //    ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
        //    if (!string.IsNullOrEmpty(startDate) && !string.IsNullOrEmpty(startDate))
        //    {
        //        StartDate = Convert.ToDateTime(DateTime.ParseExact(startDate, "dd/MM/yyyy", CultureInfo.InvariantCulture));
        //        EndDate = Convert.ToDateTime(DateTime.ParseExact(endDate, "dd/MM/yyyy", CultureInfo.InvariantCulture)).AddHours(23).AddMinutes(59);
        //    }
        //    q = q.Where(x => x.ProductInvoiceDate >= StartDate && x.ProductInvoiceDate <= EndDate);
        //    if (BranchId != null && BranchId.Value > 0)
        //    {
        //        q = q.Where(item => item.BranchId == BranchId);
        //    }
        //    if (CreatedUserId != null && CreatedUserId.Value > 0)
        //    {
        //        q = q.Where(x => x.CreatedUserId == CreatedUserId);
        //    }
        //    if (ProductId != null && ProductId.Value > 0)
        //    {
        //        q = q.Where(x => x.ProductId == ProductId);
        //    }
        //    if (!string.IsNullOrEmpty(CityId))
        //    {
        //        q = q.Where(x => x.CityId == CityId);
        //    }
        //    if (!string.IsNullOrEmpty(DistrictId))
        //    {
        //        q = q.Where(x => x.DistrictId == DistrictId);
        //    }
        //    //var model = q.Select(item => new ProductInvoiceDetailViewModel
        //    //{
        //    //    Id = item.Id,
        //    //    IsDeleted = item.IsDeleted,
        //    //    CreatedUserId = item.CreatedUserId,
        //    //    CreatedDate = item.CreatedDate,
        //    //    ModifiedUserId = item.ModifiedUserId,
        //    //    ModifiedDate = item.ModifiedDate,
        //    //    Amount = item.Amount,
        //    //    CategoryCode = item.CategoryCode,
        //    //    ProductType = item.ProductType,
        //    //    ProductCode = item.ProductCode,
        //    //    ProductGroup = item.ProductGroup,
        //    //    ProductInvoiceCode = item.ProductInvoiceCode,
        //    //    Price = item.Price,
        //    //    ProductId = item.ProductId,
        //    //    ProductInvoiceDate = item.ProductInvoiceDate,
        //    //    ProductInvoiceId = item.ProductInvoiceId,
        //    //    ProductName = item.ProductName,
        //    //    Quantity = item.Quantity,
        //    //    CustomerId = item.CustomerId,
        //    //    CustomerCode = item.CustomerCode,
        //    //    CustomerName = item.CustomerName,
        //    //    BranchId = item.BranchId,
        //    //    BranchName = item.BranchName,
        //    //    CustomerPhone = item.CustomerPhone,
        //    //    Origin = item.Origin,
        //    //}).OrderByDescending(m => m.CreatedDate).ToList();


        //    var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
        //    var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

        //    var data = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("spDoanhThuNgay", new
        //    {
        //        ProductId = ProductId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        BranchId = BranchId,
        //        CityId = CityId,
        //        DistrictId = DistrictId
        //    }).ToList();
        //    //return View(data);
        //    return View(data);
        //}
        #region Doanh so thang
        public ActionResult DoanhSoThang(int? branchId, string startDate, string endDate)
        {
            var q = invoiceRepository.GetAllvwProductInvoice().AsEnumerable().Where(x => x.Status == "Đặt cọc" || x.IsArchive == true); //x.Status == "complete" || 
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);

            branchId = branchId == null ? 0 : branchId;

            if (startDate == null && endDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }
            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    ViewBag.retDateTime = d_endDate;
                    ViewBag.aDateTime = d_startDate;
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }

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
            //
            if (branchId > 0)
            {
                q = q.Where(item => item.BranchId == branchId);
            }

            var model = q.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,

                CityId = item.CityId,
                Code = item.Code,
                DistrictId = item.DistrictId,
                TotalAmount = item.TotalAmount,
                DiscountAmount = item.DiscountAmount,
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                BranchId = item.BranchId,
                BranchName = item.BranchName,
                CustomerPhone = item.CustomerPhone,
                Status = item.Status,
                tienconno = item.tienconno,
                TotalCredit = item.TotalCredit,
                TotalDebit = item.TotalDebit,
                IsArchive = item.IsArchive,
                PaidAmount = item.PaidAmount,
                RemainingAmount = item.RemainingAmount



                //    StaffId = item.StaffId
            }).OrderByDescending(m => m.CreatedDate).ToList();

            // xử lý hủy chuyển
            foreach (var item in model)
            {
                var dcchuyen = invoiceRepository.GetMMByNewId(item.Id);
                if (dcchuyen != null)
                {
                    var bichuyen = invoiceRepository.GetvwProductInvoiceById(dcchuyen.FromProductInvoiceId.Value);
                    if (bichuyen.IsArchive == true)
                    {
                        item.PaidAmount = (decimal)(item.PaidAmount - dcchuyen.TotalAmount);
                        item.TotalAmount = (decimal)(item.TotalAmount - dcchuyen.TotalAmount);
                    }
                }
            }



            return View(model);
        }

        public ActionResult ExportDoanhSoThang(int? branchId, string startDate, string endDate)
        {

            var q = invoiceRepository.GetAllvwProductInvoice().AsEnumerable().Where(x => x.Status == "Đặt cọc" || x.IsArchive == true); //x.Status == "complete" || x.IsArchive == true
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);

            branchId = branchId == null ? 0 : branchId;

            if (startDate == null && endDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }
            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    ViewBag.retDateTime = d_endDate;
                    ViewBag.aDateTime = d_startDate;
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
                }
            }

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
            //
            if (branchId > 0)
            {
                q = q.Where(item => item.BranchId == branchId);
            }

            var listinvoice = q.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,

                CityId = item.CityId,
                Code = item.Code,
                DistrictId = item.DistrictId,
                TotalAmount = item.TotalAmount,
                DiscountAmount = item.DiscountAmount,
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                BranchId = item.BranchId,
                BranchName = item.BranchName,
                CustomerPhone = item.CustomerPhone,
                Status = item.Status,
                tienconno = item.tienconno,
                TotalCredit = item.TotalCredit,
                TotalDebit = item.TotalDebit,
                IsArchive = item.IsArchive,
                PaidAmount = item.PaidAmount,
                RemainingAmount = item.RemainingAmount



                //    StaffId = item.StaffId
            }).OrderByDescending(m => m.CreatedDate).ToList();

            // xử lý hủy chuyển
            foreach (var item in listinvoice)
            {
                var dcchuyen = invoiceRepository.GetMMByNewId(item.Id);
                if (dcchuyen != null)
                {
                    var bichuyen = invoiceRepository.GetvwProductInvoiceById(dcchuyen.FromProductInvoiceId.Value);
                    if (bichuyen.IsArchive == true)
                    {
                        item.PaidAmount = (decimal)(item.PaidAmount - dcchuyen.TotalAmount);
                        item.TotalAmount = (decimal)(item.TotalAmount - dcchuyen.TotalAmount);
                    }
                }
            }


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
            model.Content = model.Content.Replace("{DataTable}", BuildHtmlDS(listinvoice));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Doanh số theo tháng");

            Response.AppendHeader("content-disposition", "attachment;filename=" + "DoanhSoThang" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();


            return View(listinvoice);
        }
        string BuildHtmlDS(List<ProductInvoiceViewModel> listinvoice)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\" >\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Trạng thái</th>\r\n";
            detailLists += "		<th>Ngày tạo</th>\r\n";
            detailLists += "		<th>Chi nhánh</th>\r\n";
            detailLists += "		<th>Mã đơn hàng</th>\r\n";
            detailLists += "		<th>Tên khách hàng</th>\r\n";
            detailLists += "		<th>Số ĐT</th>\r\n";
            detailLists += "		<th>Doanh số</th>\r\n";
            detailLists += "		<th>Doanh thu</th>\r\n";


            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in listinvoice)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n";

                switch (item.Status)
                {
                    case "pending":
                        detailLists += "<td class=\"text-left \">Khởi tạo</td>\r\n";
                        break;
                    case "inprogress":
                        detailLists += "<td class=\"text-left \">Đang xử lý</td>\r\n";
                        break;
                    case "shipping":
                        detailLists += "<td class=\"text-left \">Khởi tạo</td>\r\n";
                        break;
                    case "complete":
                        detailLists += "<td class=\"text-left \">Hoàn thành</td>\r\n";
                        break;
                    case "delete":
                        detailLists += "<td class=\"text-left \">Hủy</td>\r\n";
                        break;
                    case "Đặt cọc":
                        detailLists += "<td class=\"text-left \">Đặt cọc</td>\r\n";
                        break;

                }



                detailLists += "<td class=\"text-left code_product\">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.BranchName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerPhone + "</td>\r\n"


                + "<td class=\"text-center\">" + CommonSatic.ToCurrencyStr(item.TotalAmount, null).Replace(".", ",") + "</td>\r\n"

                + "<td class=\"text-center\">" + CommonSatic.ToCurrencyStr(item.PaidAmount, null).Replace(".", ",") + "</td>\r\n"


                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"8\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(listinvoice.Sum(x => x.PaidAmount), null).Replace(".", ",")
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }

        #endregion
        public ActionResult Sale_BaoCaoXuatKho()
        {

            return View();
        }
        public PartialViewResult _GetListSale_BCXK(string startDate, string endDate, string CityId, string DistrictId, int? branchId, int? WarehouseId)
        {
            //year = year == null ? DateTime.Now.Year : year;
            //month = month == null ? DateTime.Now.Month : month;
            //single = single == null ? "month" : single;
            //quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            //if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            //{
            //    branchId = Helpers.Common.CurrentUser.BranchId;
            //}
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            //Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            //var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            //week = week == null ? weekdefault : week;
            //DateTime StartDate = DateTime.Now;
            //DateTime EndDate = DateTime.Now;

            //ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
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

            if (startDate == null && endDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                endDate = retDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate);

            DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate);

            d_endDate = d_endDate.AddHours(23).AddMinutes(59);
            //model = model.Where(x => x.VoucherDate >= d_startDate && x.VoucherDate <= d_endDate).ToList();

            var data = new List<Sale_BaoCaoXuatKhoViewModel>();
            data = SqlHelper.QuerySP<Erp.BackOffice.Sale.Models.Sale_BaoCaoXuatKhoViewModel>("spSale_BaoCaoXuatKho", new
            {
                StartDate = d_startDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = d_endDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).Where(x => x.ProductType == "product").ToList();
            ViewBag.StartDate = d_startDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = d_endDate.ToString("dd/MM/yyyy");
            return PartialView(data);
        }

        public ActionResult Sale_BaoCaoNhapXuatTon()
        {
            return View();
        }

        public PartialViewResult _GetListSale_BCNXT(string CityId, string DistrictId, int? branchId, int? WarehouseId, string startDate, string endDate)
        {
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
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

            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            //Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            //var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            var d_startDate = DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(endDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = "",
                DistrictId = ""
            }).Where(x => x.ProductType == "product").ToList();

            ViewBag.StartDate = startDate;
            ViewBag.EndDate = endDate;
            return PartialView(data);
        }

        public ActionResult Sale_BaoCaoTonKho()
        {
            return View();
        }

        public PartialViewResult _GetListSale_BCTK(string CityId, string DistrictId, int? BranchId, int? WarehouseId, string ProductGroup, string CategoryCode, string Manufacturer)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            //if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            //{
            //    BranchId = Helpers.Common.CurrentUser.BranchId;
            //}
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;
            Manufacturer = Manufacturer == null ? "" : Manufacturer;

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

            var data = SqlHelper.QuerySP<Sale_BaoCaoTonKhoViewModel>("spSale_BaoCaoTonKho", new
            {
                BranchId = BranchId,
                WarehouseId = WarehouseId,
                ProductGroup = ProductGroup,
                CategoryCode = CategoryCode,
                Manufacturer = Manufacturer,
                CityId = CityId,
                DistrictId = DistrictId
            }).Where(x => x.Type == "product").ToList();
            return PartialView(data);
        }
        public ActionResult Sale_BaoCaoCongNoTongHop()
        {
            return View();
        }

        public PartialViewResult _GetListSale_BCCNTH(string StartDate, string EndDate, int? BranchId, int? SalerId)
        {
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            BranchId = BranchId == null ? 0 : BranchId;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            SalerId = SalerId == null ? 0 : SalerId;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null && BranchId > 0)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_BaoCaoCongNoTongHopViewModel>("spSale_BaoCaoCongNoTongHop", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SalerId = SalerId
            }).ToList();
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            return PartialView(data);
        }

        public ActionResult Sale_LiabilitiesDrugStore(string single, int? year, int? month, int? quarter, int? week, int? branchId, string CityId, string DistrictId)
        {
            single = single == null ? "month" : single;
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            quarter = quarter == null ? 1 : quarter;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

            var data = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_LiabilitiesDrugStore_haopd", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).ToList();
            return View(data);
        }

        public ActionResult Sale_BCDoanhSoTheoSP(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId, int? CreatedUserId, int? productId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
            ViewBag.aDateTime = StartDate;
            ViewBag.retDateTime = EndDate;
            var q = invoiceRepository.GetAllvwInvoiceDetails().AsEnumerable().Where(x => x.Status == "complete" || x.Status == "Đặt cọc");
            if (branchId > 0)
            {
                q = q.Where(item => item.BranchId == branchId);
            }
            if (CreatedUserId != null && CreatedUserId.Value > 0)
            {
                q = q.Where(x => x.CreatedUserId == CreatedUserId);
            }
            if (productId != null && productId.Value > 0)
            {
                q = q.Where(x => x.ProductId == productId);
            }
            if (!string.IsNullOrEmpty(CityId))
            {
                q = q.Where(x => x.CityId == CityId);
            }
            if (!string.IsNullOrEmpty(DistrictId))
            {
                q = q.Where(x => x.DistrictId == DistrictId);
            }
            //Lọc theo ngày
            q = q.Where(x => x.ProductInvoiceDate >= StartDate && x.ProductInvoiceDate <= EndDate);

            var model = q.Select(item => new ProductInvoiceDetailViewModel
            {
                Id = item.Id,
                BranchId = item.BranchId,
                ProductId = item.ProductId,
                Amount = item.Amount,
                BranchName = item.BranchName,
                ProductName = item.ProductName,
                ProductCode = item.ProductCode,
                Status = item.Status
            }).ToList();

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
            //
            if (branchId != null && branchId.Value > 0)
            {
                // var drug_store = SelectListHelper.GetSelectList_DepartmentbyBranch(branchId, null, null);
                model = model.Where(id1 => id1.BranchId == branchId).ToList();
            }
            var list_branch = model.GroupBy(x => new { x.BranchId, x.BranchName })
                .Select(x => new BranchViewModel
                {
                    Id = x.Key.BranchId,
                    Name = x.Key.BranchName
                }).ToList();
            ViewBag.branchList = list_branch;

            var product_list = model.GroupBy(x => new { x.ProductId, x.ProductCode, x.ProductName })
                .Select(x => new ProductViewModel
                {
                    Id = x.Key.ProductId.Value,
                    Name = x.Key.ProductName,
                    Code = x.Key.ProductCode
                }).Where(x => x.Code != null).ToList();
            ViewBag.productList = product_list;
            return View(model);
        }

        public PartialViewResult ChartInboundAndOutbound(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId, int? WarehouseId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            var jsonData = new List<ChartItem>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).ToList();

            foreach (var item in data.GroupBy(x => x.ProductCode))
            {
                var obj = new Erp.BackOffice.Sale.Models.ChartItem();
                obj.label = item.Key;
                obj.data = item.Sum(x => x.Last_InboundQuantity);
                obj.data2 = item.Sum(x => x.Last_OutboundQuantity);
                jsonData.Add(obj);
            }
            //ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            //ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");
            string json = JsonConvert.SerializeObject(jsonData);
            ViewBag.json = json;
            return PartialView(data);
        }


        public PartialViewResult ChartInboundAndOutboundMaterialInventory(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId, int? WarehouseId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            var jsonData = new List<ChartItem>();
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

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

            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonKhoVatTuViewModel>("spSale_BaoCaoNhapXuatTonKhoVatTu", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).ToList();

            foreach (var item in data.GroupBy(x => x.MaterialCode))
            {
                var obj = new Erp.BackOffice.Sale.Models.ChartItem();
                obj.label = item.Key;
                obj.data = item.Sum(x => x.Last_InboundQuantity);
                obj.data2 = item.Sum(x => x.Last_OutboundQuantity);
                jsonData.Add(obj);
            }
            //ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            //ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");
            string json = JsonConvert.SerializeObject(jsonData);
            ViewBag.json = json;
            return PartialView(data);
        }

        //public ActionResult PrintDTNgay(string startDate, string endDate, string CityId, string DistrictId, int? branchId, bool ExportExcel = false)
        //{
        //    var model = new TemplatePrintViewModel();
        //    //lấy logo công ty
        //    var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
        //    var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
        //    var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
        //    var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
        //    var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
        //    var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
        //    var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
        //    var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
        //    //lấy hóa đơn.
        //    var q = invoiceRepository.GetAllvwProductInvoice().Where(x => x.IsArchive == true);
        //    var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
        //    if (startDate == null && endDate == null)
        //    {
        //        DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //        // Cộng thêm 1 tháng và trừ đi một ngày.
        //        DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //        startDate = aDateTime.ToString("dd/MM/yyyy");
        //        endDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    //Lọc theo ngày
        //    DateTime d_startDate, d_endDate;
        //    if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
        //    {
        //        if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
        //        {
        //            d_endDate = d_endDate.AddHours(23).AddMinutes(59);
        //            ViewBag.retDateTime = d_endDate;
        //            ViewBag.aDateTime = d_startDate;
        //            q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
        //        }
        //    }
        //    var branch_list = BranchRepository.GetAllBranch().Where(x => x.ParentId != null).Select(item => new BranchViewModel
        //    {
        //        Id = item.Id,
        //        Name = item.Name,
        //        Code = item.Code,
        //        DistrictId = item.DistrictId,
        //        CityId = item.CityId
        //    }).ToList();
        //    if (branchId != null && branchId.Value > 0)
        //    {
        //        q = q.Where(x => x.BranchId == branchId);
        //        branch_list = branch_list.Where(x => x.Id == branchId).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(CityId))
        //    {
        //        q = q.Where(x => x.CityId == CityId);
        //        branch_list = branch_list.Where(x => x.CityId == CityId).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(DistrictId))
        //    {
        //        q = q.Where(x => x.DistrictId == DistrictId);
        //        branch_list = branch_list.Where(x => x.DistrictId == DistrictId).ToList();
        //    }
        //    var detail = q.Select(item => new ProductInvoiceViewModel
        //    {
        //        Id = item.Id,
        //        IsDeleted = item.IsDeleted,
        //        CreatedDate = item.CreatedDate,
        //        BranchId = item.BranchId,
        //        TotalAmount = item.TotalAmount,
        //        RemainingAmount = item.RemainingAmount,
        //        PaidAmount = item.PaidAmount,
        //        Month = item.Month,
        //        Day = item.Day,
        //        Year = item.Year
        //    }).OrderByDescending(m => m.CreatedDate).ToList();
        //    if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan())
        //    {
        //        branch_list = branch_list.Where(x => ("," + user.DrugStore + ",").Contains("," + x.Id + ",") == true).ToList();
        //    }

        //    //lấy template phiếu xuất.
        //    var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("ProductInvoice")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        //    //truyền dữ liệu vào template.
        //    model.Content = template.Content;

        //    model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintDTNgay(detail, d_startDate, d_endDate));
        //    model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
        //    model.Content = model.Content.Replace("{System.CompanyName}", company);
        //    model.Content = model.Content.Replace("{System.AddressCompany}", address);
        //    model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
        //    model.Content = model.Content.Replace("{System.Fax}", fax);
        //    model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
        //    model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
        //    model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

        //    if (ExportExcel)
        //    {
        //        Response.AppendHeader("content-disposition", "attachment;filename=" + "DoanhThuTheoNgay" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
        //        Response.Charset = "";
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Write(model.Content);
        //        Response.End();
        //    }
        //    return View(model);
        //}

        //string buildHtmlDetailList_PrintDTNgay(List<ProductInvoiceViewModel> detailList, DateTime d_startDate, DateTime d_endDate)
        //{
        //    decimal? tong_tien = 0;

        //    //Tạo table html chi tiết phiếu xuất
        //    string detailLists = "<table class=\"invoice-detail\">\r\n";
        //    detailLists += "<thead>\r\n";
        //    detailLists += "	<tr>\r\n";

        //    detailLists += "		<th>STT</th>\r\n";
        //    detailLists += "		<th>Chi nhánh</th>\r\n";
        //    for (DateTime dt = d_startDate; dt <= d_endDate; dt = dt.AddDays(1))
        //    {
        //        detailLists += "		<th>" + dt.ToString("dd/MM/yyyy") + "</th>\r\n";
        //    }
        //    detailLists += "		<th>Số ngày</th>\r\n";
        //    detailLists += "		<th>Tổng tiền</th>\r\n";
        //    detailLists += "	</tr>\r\n";
        //    detailLists += "</thead>\r\n";
        //    detailLists += "<tbody>\r\n";
        //    var index = 1;

        //    foreach (var item in detailList)
        //    {
        //        decimal? subTotal = item.Quantity * item.Price.Value;

        //        tong_tien += subTotal;
        //        detailLists += "<tr>\r\n"
        //        + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
        //        + "<td class=\"text-left code_product\">" + item.BranchName + "</td>\r\n";
        //        for (DateTime dt = d_startDate; dt <= d_endDate; dt = dt.AddDays(1))
        //        {
        //            detailLists += "<td class=\"text-left \">" + item.ProductName + "</td>\r\n";
        //        }
        //        detailLists += "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
        //          + "<td class=\"text-right\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
        //          + "<td class=\"text-center\">" + item.Unit + "</td>\r\n"
        //          + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity) + "</td>\r\n"
        //          + "<td class=\"text-right code_product\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Price, null) + "</td>\r\n"
        //          + "<td class=\"text-right chiet_khau\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(subTotal, null) + "</td>\r\n"
        //          + "</tr>\r\n";
        //    }
        //    detailLists += "</tbody>\r\n";
        //    detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
        //    detailLists += "<tr><td colspan=\"8\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
        //                 + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_tien, null)
        //                 + "</td></tr>\r\n";
        //    if (model.TaxFee > 0)
        //    {
        //        var vat = tong_tien * Convert.ToDecimal(model.TaxFee);
        //        var tong = tong_tien + vat;
        //        detailLists += "<tr><td colspan=\"8\" class=\"text-right\">VAT (" + model.TaxFee + "%)</td><td class=\"text-right\">"
        //                + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(vat, null)
        //                + "</td></tr>\r\n";
        //        detailLists += "<tr><td colspan=\"8\" class=\"text-right\">Tổng tiền cần thanh toán</td><td class=\"text-right\">"
        //            + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong, null)
        //            + "</td></tr>\r\n";
        //    }


        //    detailLists += "<tr><td colspan=\"8\" class=\"text-right\">Đã thanh toán</td><td class=\"text-right\">"
        //                + (model.PaidAmount > 0 ? Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(model.PaidAmount, null) : "0")
        //                + "</td></tr>\r\n";
        //    detailLists += "<tr><td colspan=\"8\" class=\"text-right\">Còn lại phải thu</td><td class=\"text-right\">"
        //                + (model.RemainingAmount > 0 ? Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(model.RemainingAmount, null) : "0")
        //                + "</td></tr>\r\n";
        //    detailLists += "</tfoot>\r\n</table>\r\n";

        //    return detailLists;
        //}

        public ActionResult PrintBCXK(string startDate, string endDate, string CityId, string DistrictId, int? branchId, int? WarehouseId, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            //year = year == null ? DateTime.Now.Year : year;
            //month = month == null ? DateTime.Now.Month : month;
            //single = single == null ? "month" : single;
            //quarter = quarter == null ? 1 : quarter;

            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            //if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            //{
            //    branchId = Helpers.Common.CurrentUser.BranchId;
            //}
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

            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            //Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            //var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            ////week = week == null ? weekdefault : week;
            //DateTime StartDate = DateTime.Now;
            //DateTime EndDate = DateTime.Now;

            //ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);
            if (startDate == null && endDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString("yyyy-MM-dd HH:mm:ss");
                endDate = retDateTime.ToString("yyyy-MM-dd HH:mm:ss");
            }
            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate);

            DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate);

            d_endDate = d_endDate.AddHours(23).AddMinutes(59);
            //model = model.Where(x => x.VoucherDate >= d_startDate && x.VoucherDate <= d_endDate).ToList();

            var data = new List<Sale_BaoCaoXuatKhoViewModel>();
            data = SqlHelper.QuerySP<Erp.BackOffice.Sale.Models.Sale_BaoCaoXuatKhoViewModel>("spSale_BaoCaoXuatKho", new
            {
                StartDate = d_startDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = d_endDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = "",
                DistrictId = ""
            }).Where(x => x.ProductType == "product").ToList();
            ViewBag.StartDate = d_startDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = d_endDate.ToString("dd/MM/yyyy");

            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintBCXK")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCXK(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoXuatKho" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.Flush();
                Response.End();
            }
            return View(model);
        }
        string buildHtmlDetailList_PrintBCXK(List<Sale_BaoCaoXuatKhoViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã SP</th>\r\n";
            detailLists += "		<th>Tên SP</th>\r\n";
            detailLists += "		<th>Danh mục</th>\r\n";
            detailLists += "		<th>Nhà sản xuất</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>Đơn giá</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Xuất bán</th>\r\n";
            detailLists += "		<th>Xuất vận chuyển</th>\r\n";
            detailLists += "		<th>Xuất sử dụng</th>\r\n";
            detailLists += "		<th>Tổng xuất</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                var subTotal = item.invoice + item._internal + item.service;
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CategoryCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Manufacturer + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Price, null) + "</td>\r\n"
                + "<td class=\"text-center\">" + item.Unit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.invoice) + "</td>\r\n"
            + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item._internal) + "</td>\r\n"
              + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.service) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(subTotal, null) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"10\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.invoice), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x._internal), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.service), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.service + x._internal + x.invoice), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }

        public ActionResult PrintBCTK(string CityId, string DistrictId, int? BranchId, int? WarehouseId, string ProductGroup, string CategoryCode, string Manufacturer, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            BranchId = BranchId == null ? 0 : BranchId;

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
            //if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            //{
            //    BranchId = Helpers.Common.CurrentUser.BranchId;
            //}
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;
            Manufacturer = Manufacturer == null ? "" : Manufacturer;

            var data = SqlHelper.QuerySP<Sale_BaoCaoTonKhoViewModel>("spSale_BaoCaoTonKho", new
            {
                BranchId = BranchId,
                WarehouseId = WarehouseId,
                ProductGroup = ProductGroup,
                CategoryCode = CategoryCode,
                Manufacturer = Manufacturer,
                CityId = "",
                DistrictId = ""
            }).Where(x => x.Type == "product").ToList();

            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintBCTK")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCTK(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoTonKho" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlDetailList_PrintBCTK(List<Sale_BaoCaoTonKhoViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã SP</th>\r\n";
            detailLists += "		<th>Tên SP</th>\r\n";
            detailLists += "		<th>Danh mục</th>\r\n";
            detailLists += "		<th>Nhà sản xuất</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Số lượng</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CategoryCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Manufacturer + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-center\">" + item.ProductUnit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"9\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }


        string buildHtmlDetailList_PrintDoanhso(List<ProductInvoiceDetailViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\" >\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Ngày tạo</th>\r\n";
            detailLists += "		<th>Chi nhánh</th>\r\n";
            detailLists += "		<th>Mã đơn hàng</th>\r\n";
            detailLists += "		<th>Mã khách hàng</th>\r\n";
            detailLists += "		<th>Tên khách hàng</th>\r\n";
            detailLists += "		<th>Số ĐT</th>\r\n";
            detailLists += "		<th>Chứng từ xuất kho</th>\r\n";
            detailLists += "		<th>Tên SP/DV</th>\r\n";
            detailLists += "		<th>Nhóm sản phẩm</th>\r\n";
            detailLists += "		<th>Nhãn hàng</th>\r\n";
            detailLists += "		<th>Doanh thu</th>\r\n";
            detailLists += "		<th>Số lượng</th>\r\n";
            detailLists += "		<th>Đơn giá</th>\r\n";

            detailLists += "		<th>Giảm giá VIP</th>\r\n";
            detailLists += "		<th>Giảm giá KM</th>\r\n";
            detailLists += "		<th>Giảm giá ĐB</th>\r\n";
            detailLists += "		<th>Doanh thu sau giảm</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductInvoiceDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.BranchName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductInvoiceCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerPhone + "</td>\r\n"
                + "<td class=\"text-right\">" + item.ProductOutboundCode + "</td>\r\n"
                + "<td class=\"text-right\">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-center\">" + item.ProductGroup + "</td>\r\n"
                + "<td class=\"text-center\">" + item.Origin + "</td>\r\n"
                + "<td class=\"text-center\">" + CommonSatic.ToCurrencyStr(item.Amount, null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-center\">" + item.Quantity + "</td>\r\n"
                + "<td class=\"text-center\">" + CommonSatic.ToCurrencyStr(item.Price, null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-center\">" + item.Discount_VIP + "</td>\r\n"
                + "<td class=\"text-center\">" + item.Discount_KM + "</td>\r\n"
                + "<td class=\"text-center\">" + item.Discount_DB + "</td>\r\n"
                + "<td class=\"text-center\">" + CommonSatic.ToCurrencyStr(item.Tiensaugiam, null).Replace(".", ",") + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"10\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }

        public ActionResult PrintBCNXT(string StartDate, string EndDate, string CityId, string DistrictId, int? branchId, int? WarehouseId, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;

            //if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            //{
            //    branchId = Helpers.Common.CurrentUser.BranchId;
            //}
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

            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);

            var d_startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");



            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = "",
                DistrictId = ""
            }).Where(x => x.ProductType == "product").ToList();
            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintBCNXT")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCNXT(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));

            if (ExportExcel)
            {
                ViewBag.closePopup = "true";

                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoXuatNhapTon" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.Flush();
                Response.Close();
                Response.End();

            }
            return View(model);
        }

        string buildHtmlDetailList_PrintBCNXT(List<Sale_BaoCaoNhapXuatTonViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã SP</th>\r\n";
            detailLists += "		<th>Tên SP</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>Đơn giá nhập</th>\r\n";
            detailLists += "		<th>Đơn giá xuất</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Tồn đầu kỳ</th>\r\n";

            detailLists += "		<th>Nhập trong kỳ</th>\r\n";
            detailLists += "		<th>Xuất trong kỳ</th>\r\n";
            detailLists += "		<th>Tồn cuối kỳ</th>\r\n";
            detailLists += "		<th>Tổng tiền tồn</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right code_product\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound, null) + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceOutbound, null) + "</td>\r\n"
                + "<td class=\"text-center\">" + item.ProductUnit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.First_Remain) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_OutboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Remain, null) + "</td>\r\n"
                + "<td class=\"text-right chiet_khau\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound * item.Remain, null) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"9\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.First_Remain), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_InboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_OutboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain), null)
                          + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain * x.PriceInbound), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }


        public ActionResult Sale_BCTong(int? year, int? month, string CityId, string DistrictId, int? branchId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            int? week = 1;
            int quarter = 1;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, "month", year.Value, month.Value, quarter, ref week);
            bool hasSearch = false;

            var q = TotalDiscountMoneyNTRepository.GetvwAllTotalDiscountMoneyNT().ToList();
            if (year != null && year.Value > 0)
            {
                q = q.Where(x => x.Year == year).ToList();
                //   hasSearch = true;
            }
            if (branchId > 0)
            {
                hasSearch = true;
                q = q.Where(item => item.DrugStoreId == branchId).ToList();
            }
            if (month != null && month.Value > 0)
            {
                //   hasSearch = true;
                q = q.Where(x => x.Month == month).ToList();
            }
            if (hasSearch)
            {
                if (q.Count() > 0)
                {
                    decimal pDoanhsoTong = 0;
                    var TotalDiscountMoneyNT = q.FirstOrDefault();
                    if (TotalDiscountMoneyNT != null && TotalDiscountMoneyNT.IsDeleted != true)
                    {
                        var model = new TotalDiscountMoneyNTViewModel();
                        AutoMapper.Mapper.Map(TotalDiscountMoneyNT, model);
                        model.ListNXT = new List<Sale_BaoCaoNhapXuatTonViewModel>();
                        var invoice = invoiceRepository.GetAllvwProductInvoice().ToList().Where(x => x.BranchId == branchId && x.IsArchive == true);
                        pDoanhsoTong = invoice.Sum(x => x.TotalAmount);
                        if (year != null && year.Value > 0)
                        {
                            invoice = invoice.Where(x => x.Year == year).ToList();
                            //   hasSearch = true;
                        }
                        if (month != null && month.Value > 0)
                        {
                            //   hasSearch = true;
                            invoice = invoice.Where(x => x.Month == month).ToList();
                        }


                        model.DoanhSo = invoice.Sum(x => x.TotalAmount);
                        //model.ChietKhauCoDinh = invoice.Sum(x => x.FixedDiscount);
                        //model.ChietKhauDotXuat = invoice.Sum(x => x.IrregularDiscount);


                        //#region Hoapd tinh lai VIP

                        //var qProductInvoiceVIP = invoiceRepository.GetAllvwProductInvoice().AsEnumerable()
                        //    .Where(item => (string.IsNullOrEmpty(branchId) || ("," + branchId + ",").Contains("," + item.BranchId + ",") == true && item.IsArchive == true));

                        //if (year != null && year.Value > 0)
                        //{
                        //    qProductInvoiceVIP = qProductInvoiceVIP.Where(x => x.Year <= year).ToList();
                        //    //   hasSearch = true;
                        //}
                        //if (month != null && month.Value > 0)
                        //{
                        //    //   hasSearch = true;
                        //    qProductInvoiceVIP = qProductInvoiceVIP.Where(x => ((x.Month <= month && x.Year == year) || (x.Year < year))).ToList();
                        //}

                        //decimal pDoansoVip = qProductInvoiceVIP.Sum(x => x.TotalAmount);
                        //var setting = settingRepository.GetSettingByKey("setting_point").Value;
                        //setting = string.IsNullOrEmpty(setting) ? "0" : setting;
                        //var rf = pDoansoVip / Convert.ToDecimal(setting);
                        //string[] arrVal = rf.ToString().Split(',');
                        //var value = int.Parse(arrVal[0], CultureInfo.InstalledUICulture);
                        //model.PointVIP = value;

                        //#endregion








                        model.ListNXT = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon", new
                        {
                            StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            WarehouseId = "",
                            branchId = branchId,
                            CityId = CityId,
                            DistrictId = DistrictId
                        }).ToList();
                        return View(model);
                    }
                }
                else
                {
                    var model = new TotalDiscountMoneyNTViewModel();
                    model.ListNXT = new List<Sale_BaoCaoNhapXuatTonViewModel>();
                    ViewBag.FailedMessage = "Không tìm thấy dữ liệu";
                    return View(model);
                }
            }
            else
            {
                var model = new TotalDiscountMoneyNTViewModel();
                model.ListNXT = new List<Sale_BaoCaoNhapXuatTonViewModel>();
                ViewBag.FailedMessage = "Chưa chọn Chi nhánh";
                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Sale_BCTong");
        }

        #region PrintBCTong
        public ActionResult PrintBCTong(int? year, int? month, string CityId, string DistrictId, string branchId, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = string.IsNullOrEmpty(branchId) ? "" : branchId;
            int? week = 1;
            int quarter = 1;
            //if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan() && string.IsNullOrEmpty(branchId))
            //{
            //    branchId = Helpers.Common.CurrentUser.DrugStore;
            //}

            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, "month", year.Value, month.Value, quarter, ref week);
            bool hasSearch = false;

            var q = TotalDiscountMoneyNTRepository.GetvwAllTotalDiscountMoneyNT().ToList();
            if (year != null && year.Value > 0)
            {
                q = q.Where(x => x.Year == year).ToList();
            }
            if (!string.IsNullOrEmpty(branchId))
            {
                hasSearch = true;
                q = q.Where(item => (string.IsNullOrEmpty(branchId) || ("," + branchId + ",").Contains("," + item.DrugStoreId + ",") == true)).ToList();
            }
            if (month != null && month.Value > 0)
            {
                q = q.Where(x => x.Month == month).ToList();
            }
            if (hasSearch)
            {
                if (q.Count() > 0)
                {
                    var TotalDiscountMoneyNT = q.FirstOrDefault();
                    if (TotalDiscountMoneyNT != null && TotalDiscountMoneyNT.IsDeleted != true)
                    {
                        var model2 = new TotalDiscountMoneyNTViewModel();
                        AutoMapper.Mapper.Map(TotalDiscountMoneyNT, model2);
                        model2.ListNXT = new List<Sale_BaoCaoNhapXuatTonViewModel>();
                        var invoice = invoiceRepository.GetAllvwProductInvoice().ToList().Where(x => ("," + branchId + ",").Contains("," + x.BranchId + ",") == true && x.IsArchive == true);
                        decimal pDoansotong = invoice.Sum(x => x.TotalAmount);

                        if (year != null && year.Value > 0)
                        {
                            invoice = invoice.Where(x => x.Year == year).ToList();
                        }
                        if (month != null && month.Value > 0)
                        {
                            invoice = invoice.Where(x => x.Month == month).ToList();
                        }

                        model2.DoanhSo = invoice.Sum(x => x.TotalAmount);
                        //model2.ChietKhauCoDinh = invoice.Sum(x => x.FixedDiscount);
                        //model2.ChietKhauDotXuat = invoice.Sum(x => x.IrregularDiscount);




                        #region Hoapd tinh lai VIP

                        var qProductInvoiceVIP = invoiceRepository.GetAllvwProductInvoice().AsEnumerable()
                            .Where(item => (string.IsNullOrEmpty(branchId) || ("," + branchId + ",").Contains("," + item.BranchId + ",") == true && item.IsArchive == true));

                        if (year != null && year.Value > 0)
                        {
                            qProductInvoiceVIP = qProductInvoiceVIP.Where(x => x.Year <= year).ToList();
                            //   hasSearch = true;
                        }
                        if (month != null && month.Value > 0)
                        {
                            //   hasSearch = true;
                            qProductInvoiceVIP = qProductInvoiceVIP.Where(x => ((x.Month <= month && x.Year == year) || (x.Year < year))).ToList();
                        }

                        decimal pDoansoVip = qProductInvoiceVIP.Sum(x => x.TotalAmount);
                        var setting = settingRepository.GetSettingByKey("setting_point").Value;
                        setting = string.IsNullOrEmpty(setting) ? "0" : setting;
                        var rf = pDoansoVip / Convert.ToDecimal(setting);
                        string[] arrVal = rf.ToString().Split(',');
                        var value = int.Parse(arrVal[0], CultureInfo.InstalledUICulture);
                        model2.PointVIP = value;

                        #endregion















                        model2.ListNXT = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon", new
                        {
                            StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                            WarehouseId = "",
                            branchId = branchId,
                            CityId = CityId,
                            DistrictId = DistrictId
                        }).ToList();
                        //lấy template phiếu xuất.
                        var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintBCTong")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                        //truyền dữ liệu vào template.
                        model.Content = template.Content;
                        model.Content = model.Content.Replace("{DrugStoreName}", model2.DrugStoreName);
                        model.Content = model.Content.Replace("{Address}", model2.Address + ", " + model2.WardName + ", " + model2.DistrictName + ", " + model2.ProvinceName);
                        model.Content = model.Content.Replace("{DoanhSo}", model2.DoanhSo.ToCurrencyStr(null));
                        model.Content = model.Content.Replace("{CKCD}", model2.ChietKhauCoDinh.ToCurrencyStr(null));
                        model.Content = model.Content.Replace("{CKDX}", model2.ChietKhauDotXuat.ToCurrencyStr(null));
                        model.Content = model.Content.Replace("{MonthYear}", model2.Month + " NĂM " + model2.Year);
                        model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                        model.Content = model.Content.Replace("{QuantityDay}", Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(model2.QuantityDay));
                        model.Content = model.Content.Replace("{PercentDecrease}", Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(model2.PercentDecrease, null));
                        model.Content = model.Content.Replace("{DecreaseAmount}", Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(model2.DecreaseAmount, null));
                        model.Content = model.Content.Replace("{DiscountAmount}", Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(model2.DiscountAmount, null));
                        model.Content = model.Content.Replace("{RemainingAmount}", Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(model2.RemainingAmount, null));
                        model.Content = model.Content.Replace("{PointVIP}", Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(model2.PointVIP));
                        model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCTong(model2.ListNXT));
                        model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
                        model.Content = model.Content.Replace("{System.CompanyName}", company);
                        model.Content = model.Content.Replace("{System.AddressCompany}", address);
                        model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
                        model.Content = model.Content.Replace("{System.Fax}", fax);
                        model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
                        model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
                        model.Content = model.Content.Replace("{MonthYearDS}", model2.Month + "/" + model2.Year);

                        if (ExportExcel)
                        {
                            Response.AppendHeader("content-disposition", "attachment;filename=" + DateTime.Now.ToString("yyyyMMdd") + "BCTong" + ".xls");
                            Response.Charset = "";
                            Response.Cache.SetCacheability(HttpCacheability.NoCache);
                            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                            Response.Write(model.Content);
                            Response.End();
                        }
                    }
                }
                else
                {
                    ViewBag.FailedMessage = "Không tìm thấy dữ liệu";
                    return View(model);
                }
            }
            else
            {
                ViewBag.FailedMessage = "Chưa chọn Chi nhánh";
                return View(model);
            }

            return View(model);
        }
        string buildHtmlDetailList_PrintBCTong(List<Sale_BaoCaoNhapXuatTonViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã SP</th>\r\n";
            detailLists += "		<th>Tên SP</th>\r\n";
            //detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>Đơn giá nhập</th>\r\n";
            detailLists += "		<th>Đơn giá xuất</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Tồn đầu kỳ</th>\r\n";

            detailLists += "		<th>Nhập trong kỳ</th>\r\n";
            detailLists += "		<th>Xuất trong kỳ</th>\r\n";
            detailLists += "		<th>Tồn cuối kỳ</th>\r\n";
            detailLists += "		<th>Tổng tiền tồn</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                //+ "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right expiry_date\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound, null) + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceOutbound, null) + "</td>\r\n"
                + "<td class=\"text-center\">" + item.ProductUnit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.First_Remain) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_OutboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Remain, null) + "</td>\r\n"
                + "<td class=\"text-right chiet_khau\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound * item.Remain, null) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"8\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.First_Remain), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_InboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_OutboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain), null)
                          + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain * x.PriceInbound), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion

        public ActionResult Sale_BaoCaoCongNoPhaiTraNCC()
        {
            return View();
        }
        public PartialViewResult _GetListSale_BaoCaoCongNoPhaiTraNCC(string StartDate, string EndDate, int? BranchId, int? SupplierId)
        {
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            BranchId = BranchId == null ? 0 : BranchId;
            SupplierId = SupplierId == null ? 0 : SupplierId;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null && BranchId != null && BranchId.Value > 0)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_BaoCaoCongNoPhaiTraNCCViewModel>("spSale_BaoCaoCongNoPhaiTraNCC", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SupplierId = SupplierId
            }).ToList();
            return PartialView(data);
        }

        #region Sale_BaoCaoCongNoTongHopNCC
        public ActionResult Sale_BaoCaoCongNoTongHopNCC()
        {
            return View();
        }

        public List<Sale_BaoCaoCongNoTongHopNCCViewModel> GETSale_BaoCaoCongNoTongHopNCC(string StartDate, string EndDate, int? BranchId, int? SupplierId)
        {
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            BranchId = BranchId == null ? 0 : BranchId;
            SupplierId = SupplierId == null ? 0 : SupplierId;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null && BranchId != null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = Domain.Helper.SqlHelper.QuerySP<Sale_BaoCaoCongNoTongHopNCCViewModel>("spSale_BaoCaoCongNoTongHopNCC", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId,
                SupplierId = SupplierId
            }).ToList();
            return data;
        }

        public PartialViewResult _GetListSale_BaoCaoCongNoTongHopNCC(string StartDate, string EndDate, int? BranchId, int? SupplierId)
        {
            var data = GETSale_BaoCaoCongNoTongHopNCC(StartDate, EndDate, BranchId, SupplierId);
            return PartialView(data);
        }

        public ActionResult PrintSale_BaoCaoCongNoTongHopNCC(string StartDate, string EndDate, int? BranchId, int? SupplierId, bool ExportExcel = false)
        {
            var data = GETSale_BaoCaoCongNoTongHopNCC(StartDate, EndDate, BranchId, SupplierId);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_BaoCaoCongNoTongHopNCC(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo công nợ chưa trả theo đơn mua hàng");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_BaoCaoCongNoTongHopNCC" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_BaoCaoCongNoTongHopNCC(List<Sale_BaoCaoCongNoTongHopNCCViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày hóa đơn</th>";
            detailLists += "		<th>Số hóa đơn</th>";
            detailLists += "		<th>Mã nhà cung cấp</th>";
            detailLists += "		<th>Tên nhà cung cấp</th>";
            detailLists += "		<th>Trị giá công nợ gốc</th>";
            detailLists += "		<th>Trị giá còn phải trả</th>";
            detailLists += "		<th>Ngày phải trả</th>";
            detailLists += "		<th>Số ngày quá hạn</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td>" + (index++) + "</td>"
                + "<td>" + (item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
               + "<td>" + item.MaChungTuGoc + "</td>"
               + "<td>" + item.SupplierCode + "</td>"
               + "<td>" + item.SupplierName + "</td>"
               + "<td class=\"text-right\">" + item.TotalAmount + "</td>"
               + "<td class=\"text-right\">" + item.cnt + "</td>"
               + "<td>" + (item.NextPaymentDate.HasValue ? item.NextPaymentDate.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
               + "<td class=\"text-right\">" + item.daytra + "</td>"
               + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font-weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td>";
            detailLists += "</td>";
            detailLists += "<td>";
            detailLists += "</td>";
            detailLists += "<td>";
            detailLists += "</td>";
            detailLists += "<td>";
            detailLists += "</td>";
            detailLists += "<td>";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.TotalAmount), null); detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.cnt), null); detailLists += "</td>";
            detailLists += "<td>";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.daytra), null); detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }
        #endregion
        #region Sale_Get_Membership
        public ActionResult Sale_Get_Membership()
        {
            return View();
        }


        public List<Sale_Get_MembershipViewModel> GETSale_Get_Membership(int? BranchId, string Type, string StartDate, string EndDate, string StatusExpiryDate, int? ProductId)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            Type = Type == null ? "" : Type;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductId = ProductId == null ? 0 : ProductId;
            StatusExpiryDate = StatusExpiryDate == null ? "" : StatusExpiryDate;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Get_MembershipViewModel>("spSale_Get_Membership", new
            {
                BranchId = BranchId,
                Type = Type,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductId = ProductId,
                StatusExpiryDate = StatusExpiryDate
            }).ToList();
            return data;
        }


        public PartialViewResult _GetListSale_Get_Membership(int? BranchId, string Type, string StartDate, string EndDate, string StatusExpiryDate, int? ProductId)
        {
            var data = GETSale_Get_Membership(BranchId, Type, StartDate, EndDate, StatusExpiryDate, ProductId);
            return PartialView(data);
        }


        public ActionResult PrintSale_Get_Membership(int? BranchId, string Type, string StartDate, string EndDate, string StatusExpiryDate, int? ProductId, bool ExportExcel = false)
        {
            var data = GETSale_Get_Membership(BranchId, Type, StartDate, EndDate, StatusExpiryDate, ProductId);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Get_Membership(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh sách phiếu dịch vụ hết hạn");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_Get_Membership" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }


        string buildHtmlSale_Get_Membership(List<Sale_Get_MembershipViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo phiếu</th>";
            detailLists += "		<th>Mã phiếu</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Mã dịch vụ</th>";
            detailLists += "		<th>Tên dịch vụ</th>";
            detailLists += "		<th>Mã chứng từ liên quan</th>";
            detailLists += "		<th>Ngày hết hạn</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td>" + (index++) + "</td>"
               + "<td>" + item.CreatedDate + "</td>"
               + "<td>" + item.Code + "</td>"
               + "<td>" + item.CustomerCode + "</td>"
               + "<td>" + item.CustomerName + "</td>"
               + "<td>" + item.ProductCode + "</td>"
               + "<td>" + item.ProductName + "</td>"
               + "<td>" + item.TargetCode + "</td>"
               + "<td>" + item.ExpiryDate + "</td>"
               + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font-weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text-right\">";
            detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Customer
        public ActionResult Sale_Get_Customer()
        {
            return View();
        }


        public List<Sale_Get_CustomerViewModel> GETSale_Get_Customer(int? KhCuMuonBo, int? KhMoiDenVaKinhTeYeu, int? KhMoiDenVaHuaQuayLai, int? KhLauNgayKhongTuongTac, int? KhCuThanPhien, string SkinLevel, string HairlLevel, string GladLevel, int? branchId, string StartDate, string EndDate, int? StartAge, int? EndAge, string CustomerType, string CityId, string DistrictId, int? BranchName)
        {

            branchId = branchId == null ? 0 : branchId;

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

            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            StartAge = StartAge == null ? 0 : StartAge;
            EndAge = EndAge == null ? 0 : EndAge;
            CustomerType = CustomerType == null ? "" : CustomerType;
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            KhCuMuonBo = KhCuMuonBo == null ? 0 : KhCuMuonBo;
            BranchName = BranchName == null ? 0 : BranchName;
            KhMoiDenVaKinhTeYeu = KhMoiDenVaKinhTeYeu == null ? 0 : KhMoiDenVaKinhTeYeu;
            KhMoiDenVaHuaQuayLai = KhMoiDenVaHuaQuayLai == null ? 0 : KhMoiDenVaHuaQuayLai;
            KhLauNgayKhongTuongTac = KhLauNgayKhongTuongTac == null ? 0 : KhLauNgayKhongTuongTac;
            KhCuThanPhien = KhCuThanPhien == null ? 0 : KhCuThanPhien;
            SkinLevel = SkinLevel == null ? "" : SkinLevel;
            HairlLevel = HairlLevel == null ? "" : HairlLevel;
            GladLevel = GladLevel == null ? "" : GladLevel;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Get_CustomerViewModel>("spSale_Get_Customer", new
            {
                branchId = branchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                StartAge = StartAge,
                EndAge = EndAge,
                CustomerType = CustomerType,
                CityId = "",
                DistrictId = "",
                BranchName = BranchName,
                SkinLevel = SkinLevel,
                HairlLevel = HairlLevel,
                GladLevel = GladLevel,
                KhCuMuonBo = KhCuMuonBo,
                KhMoiDenVaKinhTeYeu = KhMoiDenVaKinhTeYeu,
                KhMoiDenVaHuaQuayLai = KhMoiDenVaHuaQuayLai,
                KhLauNgayKhongTuongTac = KhLauNgayKhongTuongTac,
                KhCuThanPhien = KhCuThanPhien
            }).ToList();
            return data;
        }


        public PartialViewResult _GetListSale_Get_Customer(int? KhCuMuonBo, int? KhMoiDenVaKinhTeYeu, int? KhMoiDenVaHuaQuayLai, int? KhLauNgayKhongTuongTac, int? KhCuThanPhien, string SkinLevel, string HairlLevel, string GladLevel, int? branchId, string StartDate, string EndDate, int? StartAge, int? EndAge, string CustomerType, string CityId, string DistrictId, int? BranchName)
        {
            var data = GETSale_Get_Customer(KhCuMuonBo, KhMoiDenVaKinhTeYeu, KhMoiDenVaHuaQuayLai, KhLauNgayKhongTuongTac, KhCuThanPhien, SkinLevel, HairlLevel, GladLevel, branchId, StartDate, EndDate, StartAge, EndAge, CustomerType, CityId, DistrictId, BranchName);
            return PartialView(data);
        }


        public ActionResult PrintSale_Get_Customer(int? KhCuMuonBo, int? KhMoiDenVaKinhTeYeu, int? KhMoiDenVaHuaQuayLai, int? KhLauNgayKhongTuongTac, int? KhCuThanPhien, string SkinLevel, string HairlLevel, string GladLevel, int? branchId, string StartDate, string EndDate, int? StartAge, int? EndAge, string CustomerType, string CityId, string DistrictId, int? BranchName, bool ExportExcel = false)
        {
            var data = GETSale_Get_Customer(KhCuMuonBo, KhMoiDenVaKinhTeYeu, KhMoiDenVaHuaQuayLai, KhLauNgayKhongTuongTac, KhCuThanPhien, SkinLevel, HairlLevel, GladLevel, branchId, StartDate, EndDate, StartAge, EndAge, CustomerType, CityId, DistrictId, BranchName);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Get_Customer(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách hàng");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_Get_Customer" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }


        string buildHtmlSale_Get_Customer(List<Sale_Get_CustomerViewModel> detailList)
        {
            string detailLists = "<table border=\"1\" class=\"invoice - detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Điện thoại</th>";
            detailLists += "        <th>Mã khách hàng</th>";
            detailLists += "        <th>Địa chỉ</th>";
            detailLists += "		<th>Khách mới đến và hứa quay lại</th>";
            detailLists += "		<th>Khách mới đến kinh tế yếu</th>";
            detailLists += "		<th>Khách cũ muốn bỏ</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td>" + (index++) + "</td>"
                + "<td>" + (item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
                + "<td>" + item.CompanyName + "</td>"
                + "<td>" + item.Phone + "</td>"
                + "<td>" + item.Code + "</td>"
                + "<td>" + item.Address + "</td>"
                + "<td>" + (item.KhMoiDenVaHuaQuayLai == null || item.KhMoiDenVaHuaQuayLai == 0 ? "" : "V") + "</td>"
                + "<td>" + (item.KhMoiDenKinhTeYeu == null || item.KhMoiDenKinhTeYeu == 0 ? "" : "V") + "</td>"
                + "<td>" + (item.KhCuMuonBo == null || item.KhCuMuonBo == 0 ? "" : "V") + "</td>"
                + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Sale_BaoCaoVatTu
        public ActionResult Sale_BaoCaoVatTu()
        {
            return View();
        }
        public List<Sale_BaoCaoVatTuViewModel> GETSale_BaoCaoVatTu(string CityId, string DistrictId, int? BranchId, int? WarehouseId, string ProductGroup, string CategoryCode, string Manufacturer)
        {
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);

            var data = SqlHelper.QuerySP<Sale_BaoCaoVatTuViewModel>("spSale_BaoCaoVatTu", new
            {
                ProductGroup = ProductGroup,
                CategoryCode = CategoryCode
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_BaoCaoVatTu(string CityId, string DistrictId, int? BranchId, int? WarehouseId, string ProductGroup, string CategoryCode, string Manufacturer)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;
            Manufacturer = Manufacturer == null ? "" : Manufacturer;
            var data = SqlHelper.QuerySP<Sale_BaoCaoVatTuViewModel>("spSale_BaoCaoVatTu", new
            {
                BranchId = BranchId,
                WarehouseId = WarehouseId,
                ProductGroup = ProductGroup,
                CategoryCode = CategoryCode,
                Manufacturer = Manufacturer,
                CityId = CityId,
                DistrictId = DistrictId
            }).ToList();
            return PartialView(data);
        }

        public ActionResult PrintSale_BaoCaoVatTu(string CityId, string DistrictId, int? BranchId, int? WarehouseId, string ProductGroup, string CategoryCode, string Manufacturer, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";

            BranchId = BranchId == null ? 0 : BranchId;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;
            Manufacturer = Manufacturer == null ? "" : Manufacturer;

            var data = SqlHelper.QuerySP<Sale_BaoCaoVatTuViewModel>("spSale_BaoCaoVatTu", new
            {
                BranchId = BranchId,
                WarehouseId = WarehouseId,
                ProductGroup = ProductGroup,
                CategoryCode = CategoryCode,
                Manufacturer = Manufacturer,
                CityId = CityId,
                DistrictId = DistrictId
            }).ToList();


            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            model.Content = template.Content;
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_BaoCaoVatTu(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo vật tư");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_BaoCaoVatTu" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }

        string buildHtmlSale_BaoCaoVatTu(List<Sale_BaoCaoVatTuViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã vật tư</th>\r\n";
            detailLists += "		<th>Tên vật tư</th>\r\n";
            detailLists += "		<th>Danh mục</th>\r\n";
            detailLists += "		<th>Nhà sản xuất</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Số lượng</th>\r\n";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.MaterialCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.MaterialName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CategoryCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Manufacturer + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-center\">" + item.MaterialUnit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr>";
            detailLists += "<tr><td colspan=\"9\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }

        #endregion

        #region BaoCaoXuatVatTu
        public ActionResult Sale_BaoCaoXuatVatTu()
        {
            return View();
        }


        public List<Sale_BaoCaoXuatVatTuViewModel> GETSale_BaoCaoXuatVatTu(string StartDate, string EndDate, int? WarehouseId, int? branchId, int? CityId, int? DistrictId)
        {
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            branchId = branchId == null ? 0 : branchId;
            CityId = CityId == null ? 0 : CityId;
            DistrictId = DistrictId == null ? 0 : DistrictId;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
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

            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_BaoCaoXuatVatTuViewModel>("spSale_BaoCaoXuatVatTu", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).ToList();
            return data;
        }


        public PartialViewResult _GetListSale_BaoCaoXuatVatTu(string StartDate, string EndDate, int? WarehouseId, int? branchId, int? CityId, int? DistrictId)
        {
            var data = GETSale_BaoCaoXuatVatTu(StartDate, EndDate, WarehouseId, branchId, CityId, DistrictId);
            return PartialView(data);
        }


        public ActionResult PrintSale_BaoCaoXuatVatTu(string StartDate, string EndDate, int? WarehouseId, int? branchId, int? CityId, int? DistrictId, bool ExportExcel = false)
        {
            var data = GETSale_BaoCaoXuatVatTu(StartDate, EndDate, WarehouseId, branchId, CityId, DistrictId);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_BaoCaoXuatVatTu(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo xuất vật tư");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_BaoCaoXuatVatTu" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }


        string buildHtmlSale_BaoCaoXuatVatTu(List<Sale_BaoCaoXuatVatTuViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice - detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Mã vật tư</th>";
            detailLists += "		<th>Tên vật tư</th>";
            detailLists += "		<th>Mã danh mục</th>";
            detailLists += "		<th>Nhà sản xuất</th>";
            detailLists += "		<th>Kho quản lý</th>";
            detailLists += "		<th>Đơn vị</th>";
            detailLists += "		<th>Đơn giá</th>";
            detailLists += "		<th>Đớn giá</th>";
            detailLists += "		<th>Số lô</th>";
            detailLists += "		<th>Hạn dùng</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td>" + (index++) + "</td>"
               + "<td>" + item.MaterialCode + "</td>"
               + "<td>" + item.MaterialName + "</td>"
               + "<td>" + item.CategoryCode + "</td>"
               + "<td>" + item.Manufacturer + "</td>"
               + "<td>" + item.WarehouseName + "</td>"
               + "<td>" + item.Unit + "</td>"
               + "<td>" + item.Price + "</td>"
               + "<td>" + item.invoice + "</td>"
               + "<td>" + item.LoCode + "</td>"
               + "<td>" + item.ExpiryDate + "</td>"
               + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Price), null); detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.invoice), null); detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Sale_BaoCaoNhapXuatTonVatTu_Tuan
        public ActionResult Sale_BaoCaoNhapXuatTonVatTu_Tuan()
        {
            return View();
        }


        public List<Sale_BaoCaoNhapXuatTonVatTu_TuanViewModel> GETSale_BaoCaoNhapXuatTonVatTu_Tuan(string StartDate, string EndDate, int? WarehouseId, string CategoryCode, string Manufacturer)
        {
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;
            Manufacturer = Manufacturer == null ? "" : Manufacturer;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonVatTu_TuanViewModel>("spSale_BaoCaoNhapXuatTonVatTu_Tuan", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = CategoryCode,
                Manufacturer = Manufacturer
            }).ToList();
            return data;
        }


        public PartialViewResult _GetListSale_BaoCaoNhapXuatTonVatTu_Tuan(string StartDate, string EndDate, int? WarehouseId, string CategoryCode, string Manufacturer)
        {
            var data = GETSale_BaoCaoNhapXuatTonVatTu_Tuan(StartDate, EndDate, WarehouseId, CategoryCode, Manufacturer);
            return PartialView(data);
        }


        public ActionResult PrintSale_BaoCaoNhapXuatTonVatTu_Tuan(string StartDate, string EndDate, int? WarehouseId, string CategoryCode, string Manufacturer, bool ExportExcel = false)
        {
            var data = GETSale_BaoCaoNhapXuatTonVatTu_Tuan(StartDate, EndDate, WarehouseId, CategoryCode, Manufacturer);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_BaoCaoNhapXuatTonVatTu_Tuan(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo xuất nhập tồn ngày");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_BaoCaoNhapXuatTonVatTu_Tuan" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }


        string buildHtmlSale_BaoCaoNhapXuatTonVatTu_Tuan(List<Sale_BaoCaoNhapXuatTonVatTu_TuanViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice - detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Mã danh mục</th>";
            detailLists += "		<th>Mã vật tư</th>";
            detailLists += "		<th>Tên vật tư</th>";
            detailLists += "		<th>Kho</th>";
            detailLists += "		<th>Số lô</th>";
            detailLists += "		<th>Hạn dùng</th>";
            detailLists += "		<th>Đơn vị</th>";
            detailLists += "		<th>Tồn cũ</th>";
            detailLists += "		<th>Nhập mới</th>";
            detailLists += "		<thTổng tồn đầu</th>";
            detailLists += "		<th>Tổng xuất</th>";
            detailLists += "		<th>Tổng tồn cuối tuần</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td>" + (index++) + "</td>"
               + "<td>" + item.CategoryCode + "</td>"
               + "<td>" + item.MaterialCode + "</td>"
               + "<td>" + item.MaterialName + "</td>"
               + "<td>" + item.WarehouseName + "</td>"
               + "<td>" + item.LoCode + "</td>"
               + "<td>" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>"
               + "<td>" + item.MaterialUnit + "</td>"
               + "<td>" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.First_Remain) + "</td>"
               + "<td>" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity) + "</td>"
               + "<td>" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity + item.First_Remain) + "</td>"
               + "<td>" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Remain) + "</td>"
               + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region BaoCaoNhapXuatTonKhoVatTu
        public ActionResult Sale_BaoCaoNhapXuatTonKhoVatTu()
        {
            return View();
        }

        public List<Sale_BaoCaoNhapXuatTonKhoVatTuViewModel> GETSale_BaoCaoNhapXuatTonKhoVatTu(string StartDate, string EndDate, string CityId, string DistrictId, int? branchId, int? WarehouseId)
        {
            CityId = "";
            DistrictId = "";
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;

            var d_startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");

            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonKhoVatTuViewModel>("spSale_BaoCaoNhapXuatTonKhoVatTu", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).ToList();
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            return data;
        }

        public PartialViewResult _GetListSale_BaoCaoNhapXuatTonKhoVatTu(string StartDate, string EndDate, string CityId, string DistrictId, int? branchId, int? WarehouseId)
        {
            var data = GETSale_BaoCaoNhapXuatTonKhoVatTu(StartDate, EndDate, CityId, DistrictId, branchId, WarehouseId);
            return PartialView(data);
        }

        public ActionResult PrintSale_BaoCaoNhapXuatTonKhoVatTu(string StartDate, string EndDate, string CityId, string DistrictId, int? branchId, int? WarehouseId, bool ExportExcel = false)
        {
            var data = GETSale_BaoCaoNhapXuatTonKhoVatTu(StartDate, EndDate, CityId, DistrictId, branchId, WarehouseId);
            var model = new TemplatePrintViewModel();
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
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_BaoCaoNhapXuatTonKhoVatTu(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo xuất nhập tồn kho vật tư");

            if (ExportExcel)
            {
                ViewBag.closePopup = "true";
                Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_BaoCaoNhapXuatTonKhoVatTu" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }

        string buildHtmlSale_BaoCaoNhapXuatTonKhoVatTu(List<Sale_BaoCaoNhapXuatTonKhoVatTuViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã vật tư</th>\r\n";
            detailLists += "		<th>Tên vật tư</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>Đơn giá nhập</th>\r\n";
            detailLists += "		<th>Đơn giá xuất</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Tồn đầu kỳ</th>\r\n";

            detailLists += "		<th>Nhập trong kỳ</th>\r\n";
            detailLists += "		<th>Xuất trong kỳ</th>\r\n";
            detailLists += "		<th>Tồn cuối kỳ</th>\r\n";
            detailLists += "		<th>Tổng tiền tồn</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.MaterialCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.MaterialName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right code_product\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound, null) + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceOutbound, null) + "</td>\r\n"
                + "<td class=\"text-center\">" + item.MaterialUnit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.First_Remain) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_OutboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Remain, null) + "</td>\r\n"
                + "<td class=\"text-right chiet_khau\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound * item.Remain, null) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"9\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.First_Remain), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_InboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_OutboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain), null)
                          + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain * x.PriceInbound), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }


        #endregion

        #region spSale_Get_TrangThietBi
        public ActionResult spSale_Get_TrangThietBi()
        {
            return View();
        }

        public List<spSale_Get_TrangThietBiViewModel> GETspSale_Get_TrangThietBi(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductId = ProductId == null ? 0 : ProductId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
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

            var data = SqlHelper.QuerySP<spSale_Get_TrangThietBiViewModel>("spSale_Get_TrangThietBi", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductId = ProductId,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListspSale_Get_TrangThietBi(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            var data = GETspSale_Get_TrangThietBi(ProductId, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintspSale_Get_TrangThietBi(int? ProductId, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETspSale_Get_TrangThietBi(ProductId, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlspSale_Get_TrangThietBi(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Tổng hợp trang thiết bị");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_TrangThietBi" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlspSale_Get_TrangThietBi(List<spSale_Get_TrangThietBiViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice - detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo</th>";
            detailLists += "		<th>Trang thiết bị</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Mã xếp lịch</th>";
            detailLists += "		<th>Ngày làm</th>";
            detailLists += "		<th>Ngày kết thúc</th>";
            detailLists += "		<th>Thời gian</th>";
            detailLists += "		<th>Phòng</th>";
            detailLists += "		<th>Trạng thái</th>";
            detailLists += "		<th>Ghi chú</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td>" + (index++) + "</td>"
                + "<td>" + (item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
                + "<td>" + item.EquipmentName + "</td>"
                + "<td>" + item.CustomerName + "</td>"
                + "<td>" + item.CustomerCode + "</td>"
                + "<td>" + item.Code + "</td>"
                + "<td>" + (item.WorkDay.HasValue ? item.WorkDay.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
                + "<td>" + (item.EndDate.HasValue ? item.EndDate.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
                + "<td>" + item.TotalMinute + "</td>"
                + "<td>" + item.RoomName + "</td>"
                + "<td>" + item.Status + "</td>"
                + "<td>" + item.Note + "</td>"
                + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Sale_SchedulingHistory
        public ActionResult Sale_SchedulingHistory()
        {
            return View();
        }

        public List<Sale_SchedulingHistoryViewModel> GETSale_SchedulingHistory(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductId = ProductId == null ? 0 : ProductId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            //var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            //var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
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

            var data = SqlHelper.QuerySP<Sale_SchedulingHistoryViewModel>("spSale_SchedulingHistory", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductId = ProductId,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_SchedulingHistory(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_SchedulingHistory(ProductId, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_SchedulingHistory(int? ProductId, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_SchedulingHistory(ProductId, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_SchedulingHistory(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo số lượng tour theo khung giờ");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.Flush();
                Response.Close();


                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_SchedulingHistory(List<Sale_SchedulingHistoryViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>Nhóm sản phẩm</th>";
            detailLists += "		<th>Mã sản phẩm</th>";
            detailLists += "		<th>Tên sản phẩm</th>";
            detailLists += "		<th>Số lượng</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            detailLists += "<tr><td colspan=\"3\" class=\"text-left\">Chưa có nhóm</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null)
                         + "</tr>\r\n";
            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-left code_product\"> </td>\r\n"
                //+ "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity)
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"4\" class=\"text-right\">Tổng cộng:" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null) + "</td>"
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }

        #endregion

        #region Sale_TotalTourOfStaff
        public ActionResult Sale_TotalTourOfStaff()
        {
            return View();
        }

        public List<Sale_TotalTourOfStaffViewModel> _GETSale_TotalTourOfStaff(int? UserId, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            UserId = UserId == null ? 0 : UserId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            //var d_startDate = DateTime.ParseExact(StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            //var d_endDate = DateTime.ParseExact(EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
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
            var data = SqlHelper.QuerySP<Sale_TotalTourOfStaffViewModel>("spSale_TotalTourOfStaff", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                UserId = UserId,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_TotalTourOfStaff(int? UserId, int? BranchId, string StartDate, string EndDate)
        {
            var data = _GETSale_TotalTourOfStaff(UserId, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_TotalTourOfStaff(int? UserId, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = _GETSale_TotalTourOfStaff(UserId, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_TotalTourOfStaff(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo tổng số lượng tour của nhân viên");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoTongSoTourCuaNhanVien" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.Flush();
                Response.Close();


                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_TotalTourOfStaff(List<Sale_TotalTourOfStaffViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Mã nhân viên</th>";
            detailLists += "		<th>Tên nhân viên</th>";
            detailLists += "		<th>Số lượng</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.IdNV + "</td>\r\n"
                + "<td class=\"text-left \">" + item.FullName + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"4\" class=\"text-right\">Tổng cộng:" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null) + "</td>"
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }
        #endregion

        #region DetailTourOfStaff
        public ActionResult DetailTourOfStaff(string StartDate, string EndDate, int? UserId, int? BranchId)
        {
            var data = GETSale_DetailTourOfStaff(StartDate, EndDate, UserId, BranchId);
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;
            ViewBag.BranchId = BranchId;
            ViewBag.FullName = data[0].NameNV;
            ViewBag.UserId = UserId;
            return View(data);
        }

        public List<Sale_DetailTourOfStaffViewModel> GETSale_DetailTourOfStaff(string StartDate, string EndDate, int? UserId, int? BranchId)
        {
            Sale_DetailTourOfStaffViewModel model = new Sale_DetailTourOfStaffViewModel();
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy HH:mm");
                EndDate = retDateTime.ToString("dd/MM/yyyy HH:mm");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_DetailTourOfStaffViewModel>("spDetailTourOfStaff", new
            {
                UserId = UserId,
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
            }).ToList();

            foreach (var item in data)
            {
                // vì vẫn còn 1 số dữ liệu trong database bị tạo sai nên cần kiểm tra giá trị của CreateDate và EndDate
                if (item.EndDate.HasValue && item.CreatedDate.HasValue)
                {
                    item.TotalMinute = Convert.ToInt32(item.EndDate.Value.Subtract(item.WorkDay.Value).TotalMinutes);
                    item.days = item.TotalMinute / 1440;
                    item.hours = (item.TotalMinute % 1440) / 60;
                    item.mins = item.TotalMinute % 60;
                    item.TotalDayHourMinute = item.hours + "giờ " + item.mins + "phút";
                    item.getCreatedDate = item.CreatedDate.Value.ToShortDateString();
                    item.getPeriodOfTime = item.WorkDay.Value.ToString("HH:mm:ss") + " - " + item.EndDate.Value.ToString("HH:mm:ss");
                }
            }
            return data;
        }


        string buildHtmlSale_DetailTourOfStaff(List<Sale_DetailTourOfStaffViewModel> detailList)
        {
            string detailLists = "<table border=\"1\"  class=\"invoice-detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Dịch Vụ</th>";
            detailLists += "		<th  style=\"width:100px\"> Ngày</th>";
            detailLists += "		<th>Thời gian</th>";
            detailLists += "		<th style=\"width:60px\">Thời lượng</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>"
                 + "<td class=\"text-center orderNo\">" + item.ProductName + "</td>"
                + "<td class=\"text-left code_product\">" + item.getCreatedDate + "</td>"
                + "<td class=\"text-left \">" + item.getPeriodOfTime + "</td>"
                + "<td class=\"text-right orderNo\">" + item.TotalDayHourMinute + "</td>"
                + "</tr>";
            }
            detailLists += "</tbody>";
            return detailLists;
        }
        public ActionResult PrintSale_DetailTourOfStaff(string StartDate, string EndDate, int? UserId, int? BranchId, bool ExportExcel = false)
        {
            var data = GETSale_DetailTourOfStaff(StartDate, EndDate, UserId, BranchId);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_DetailTourOfStaff(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo chi tiết các tour của KTV");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spDetailOfStaff" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.Flush();
                Response.Close();
                Response.End();
            }
            return View(model);
        }

        #endregion

        #region Sale_StaffMade
        public ActionResult Sale_StaffMade()
        {
            return View();
        }

        public List<Sale_StaffMadeViewModel> GETSale_StaffMade(int? BranchId, string StartDate, string EndDate)
        {
            Sale_StaffMadeViewModel model = new Sale_StaffMadeViewModel();
            //var q = staffMadeRepository.GetvwAllStaffMade().Where(x => x.Status == "accept" && x.SchedulingStatus == "complete");
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy HH:mm");
                EndDate = retDateTime.ToString("dd/MM/yyyy HH:mm");
            }
            //var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            //var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
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

            var data = SqlHelper.QuerySP<Sale_StaffMadeViewModel>("spSale_StaffMade", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
            }).ToList();
            //StaffMadeRepository staffMadeRepository = new StaffMadeRepository(new Erp.Domain.Sale.ErpSaleDbContext());
            //var data = staffMadeRepository.GetvwAllStaffMade().Where(x =>x.Status == "accept" && x.SchedulingStatus == "complete").Select(x => new Sale_StaffMadeViewModel
            //{
            //    Id = x.Id,
            //    WorkDay = x.WorkDay,
            //    EndDate = x.EndDate,
            //    FullName=x.FullName,
            //    UserCode=x.UserCode
            //}).ToList();
            //model.CountSchedulingHistory = data == null ? 0 : data.Count();
            foreach (var item in data)
            {
                item.TotalMinute = Convert.ToInt32(item.EndDate.Value.Subtract(item.WorkDay.Value).TotalMinutes);
                item.days = item.TotalMinute / 1440;
                item.hours = (item.TotalMinute % 1440) / 60;
                item.mins = item.TotalMinute % 60;
                item.TotalDayHourMinute = item.hours + "giờ " + item.mins + "phút";
            }
            data.Sum(x => x.TotalMinute);
            data.GroupBy(x => x.FullName);
            int tot_mins = data == null ? 0 : data.Sum(x => x.TotalMinute.Value);
            int days = tot_mins / 1440;
            int hours = (tot_mins % 1440) / 60;
            int mins = tot_mins % 60;
            //tổng số ngày,giờ, phút mà nhân viên đã làm
            model.SumTotalMinute = hours + "giờ " + mins + "phút";
            ViewBag.SumToalMinute = model.SumTotalMinute;
            return data;
        }
        public PartialViewResult _GetListSale_StaffMade(int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_StaffMade(BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_StaffMade(int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_StaffMade(BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_StaffMade(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo tổng số giờ làm liệu trình của kỹ thuật viên");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_StaffMade" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.Flush();
                Response.Close();
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_StaffMade(List<Sale_StaffMadeViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th  style=\"width:100px\">Mã người dùng</th>";
            detailLists += "		<th>Tên kỹ thuật viên</th>";
            detailLists += "		<th style=\"width:60px\">Thời gian</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>"
                + "<td class=\"text-left code_product\">" + item.UserCode + "</td>"
                + "<td class=\"text-left \">" + item.FullName + "</td>"
                + "<td class=\"text-right orderNo\">" + item.TotalDayHourMinute + "</td>"//Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.TotalMinute) 
                + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font-weight:bold\">";
            detailLists += "<tr><td colspan=\"3\" class=\"text-right\">Tổng</td><td class=\"text-right\">"
                         + ViewBag.SumToalMinute
                         + "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        //#region DoanhThuNgay
        //public ActionResult DoanhThuNgay()
        //{
        //    return View();
        //}

        //public List<Sale_StaffMadeViewModel> DoanhThuNgay(int? ProductId, int? BranchId, string StartDate, string EndDate)
        //{
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;
        //    ProductId = ProductId == null ? 0 : ProductId;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy HH:mm");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy HH:mm");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_StaffMadeViewModel>("spDoanhThuNgay", new
        //    {
        //        BranchId = BranchId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        ProductId = ProductId,
        //    }).ToList();
        //    return data;
        //}
        ////public static String convert(int? mins)
        ////{
        ////    int? hours = (mins - mins % 60) / 60;
        ////    return "" + hours + ":" + (mins - hours * 60);
        ////}
        //public PartialViewResult _GetListDoanhThuNgay(int? ProductId, int? BranchId, string StartDate, string EndDate)
        //{
        //    var data = GETSale_StaffMade(ProductId, BranchId, StartDate, EndDate);
        //    return PartialView(data);
        //}
        //public ActionResult PrintDoanhThuNgay(int? ProductId, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        //{
        //    var data = GETSale_StaffMade(ProductId, BranchId, StartDate, EndDate);
        //    var model = new TemplatePrintViewModel();
        //    var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
        //    var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
        //    var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
        //    var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
        //    var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
        //    var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
        //    var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
        //    var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
        //    var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        //    model.Content = template.Content;
        //    model.Content = model.Content.Replace("{DataTable}", buildHtmlDoanhThuNgay(data));
        //    model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
        //    model.Content = model.Content.Replace("{System.CompanyName}", company);
        //    model.Content = model.Content.Replace("{System.AddressCompany}", address);
        //    model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
        //    model.Content = model.Content.Replace("{System.Fax}", fax);
        //    model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
        //    model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
        //    model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        //    model.Content = model.Content.Replace("{Title}", "Báo cáo tổng số giờ làm liệu trình của kỹ thuật viên");
        //    if (ExportExcel)
        //    {
        //        Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_StaffMade" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
        //        Response.Charset = "";
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Write(model.Content);
        //        Response.End();
        //    }
        //    return View(model);
        //}
        //string buildHtmlDoanhThuNgay(List<Sale_StaffMadeViewModel> detailList)
        //{
        //    string detailLists = "<table class=\"invoice-detail\">";
        //    detailLists += "<thead>";
        //    detailLists += "	<tr>";
        //    detailLists += "		<th>STT</th>";
        //    detailLists += "		<th  style=\"width:100px\">Mã người dùng</th>";
        //    detailLists += "		<th>Tên kỹ thuật viên</th>";
        //    detailLists += "		<th style=\"width:60px\">Thời gian</th>";
        //    detailLists += "	</tr>";
        //    detailLists += "</thead>";
        //    detailLists += "<tbody>";
        //    var index = 1;
        //    foreach (var item in detailList)
        //    {
        //        detailLists += "<tr>"
        //        + "<td class=\"text-center orderNo\">" + (index++) + "</td>"
        //        + "<td class=\"text-left code_product\">" + item.UserCode + "</td>"
        //        + "<td class=\"text-left \">" + item.FullName + "</td>"
        //        + "<td class=\"text-right orderNo\">" + item.TotalMinute + "</td>"//Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.TotalMinute) 
        //        + "</tr>";
        //    }
        //    detailLists += "</tbody>";
        //    detailLists += "<tfoot style=\"font-weight:bold\">";
        //    //detailLists += "<tr><td colspan=\"3\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
        //    //             + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.TotalMinute), null)
        //    //             + "</tr>";
        //    detailLists += "</tfoot></table>";
        //    return detailLists;
        //}

        //#endregion
        // <append_content_action_here>

        #region BaoCaoKhoGoiLieuTrinh
        public ActionResult BaoKhoGoiLieuTrinhTheoNgay(string group, string category, string manufacturer, int? page, string StartDate, string EndDate, string WarehouseId)
        {
            group = group == null ? "" : group;
            category = category == null ? "" : category;
            manufacturer = manufacturer == null ? "" : manufacturer;
            WarehouseId = WarehouseId == null ? "" : WarehouseId;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;

            if (!Filters.SecurityFilter.IsAdmin() && !Filters.SecurityFilter.IsKeToan() && string.IsNullOrEmpty(WarehouseId))
            {
                WarehouseId = Helpers.Common.CurrentUser.WarehouseId;
            }


            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            var data = SqlHelper.QuerySP<spBaoCaoNhapXuatTon_TuanViewModel>("spSale_BaoCaoNhapXuatTon_Tuan", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            }).Where(x => x.ProductType == "productpackage").ToList();
            var product_outbound = SqlHelper.QuerySP<spBaoCaoXuatViewModel>("spSale_BaoCaoXuat", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                WarehouseId = WarehouseId,
                CategoryCode = category,
                ProductGroup = group,
                Manufacturer = manufacturer
            }).Where(x => x.ProductType == "productpackage").ToList();
            //var pager = new Pager(data.Count(), page, 20);

            var Items = data.OrderBy(m => m.ProductCode)
              //.Skip((pager.CurrentPage - 1) * pager.PageSize).Take(pager.PageSize)
              .Select(item => new spBaoCaoNhapXuatTon_TuanViewModel
              {
                  ProductType = item.ProductType,
                  CategoryCode = item.CategoryCode,
                  First_InboundQuantity = item.First_InboundQuantity,
                  First_OutboundQuantity = item.First_OutboundQuantity,
                  First_Remain = item.First_Remain,
                  Last_InboundQuantity = item.Last_InboundQuantity,
                  Last_OutboundQuantity = item.Last_OutboundQuantity,
                  ProductCode = item.ProductCode,
                  ProductId = item.ProductId,
                  ProductName = item.ProductName,
                  ProductUnit = item.ProductUnit,
                  Remain = item.Remain,
                  ProductMinInventory = item.ProductMinInventory,
                  LoCode = item.LoCode,
                  ExpiryDate = item.ExpiryDate,
                  WarehouseName = item.WarehouseName,
                  WarehouseId = item.WarehouseId
              }).ToList();

            ViewBag.productInvoiceDetailList = product_outbound;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];


            return View(Items);
        }
        #endregion

        #region BaoCaoTonKhoGoiLieuTrinh
        public ActionResult Sale_BaoCaoTonKhoGLT()
        {
            return View();
        }

        public PartialViewResult _GetListSale_BaoCaoTonKhoGLT(string CityId, string DistrictId, int? BranchId, int? WarehouseId, string ProductGroup, string CategoryCode, string Manufacturer)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;
            Manufacturer = Manufacturer == null ? "" : Manufacturer;
            var data = SqlHelper.QuerySP<Sale_BaoCaoTonKhoViewModel>("spSale_BaoCaoTonKho", new
            {
                BranchId = BranchId,
                WarehouseId = WarehouseId,
                ProductGroup = ProductGroup,
                CategoryCode = CategoryCode,
                Manufacturer = Manufacturer,
                CityId = CityId,
                DistrictId = DistrictId
            }).Where(x => x.Type == "productpackage").ToList();
            return PartialView(data);
        }
        public ActionResult PrintBCTKGLT(string CityId, string DistrictId, int? BranchId, int? WarehouseId, string ProductGroup, string CategoryCode, string Manufacturer, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            BranchId = BranchId == null ? 0 : BranchId;
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
            //if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            //{
            //    BranchId = Helpers.Common.CurrentUser.BranchId;
            //}
            CityId = CityId == null ? "" : CityId;
            DistrictId = DistrictId == null ? "" : DistrictId;
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            CategoryCode = CategoryCode == null ? "" : CategoryCode;
            Manufacturer = Manufacturer == null ? "" : Manufacturer;

            var data = SqlHelper.QuerySP<Sale_BaoCaoTonKhoViewModel>("spSale_BaoCaoTonKho", new
            {
                BranchId = BranchId,
                WarehouseId = WarehouseId,
                ProductGroup = ProductGroup,
                CategoryCode = CategoryCode,
                Manufacturer = Manufacturer,
                CityId = "",
                DistrictId = ""
            }).Where(x => x.Type == "productpackage").ToList();

            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCTKGLT(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo kho gói liệu trình");

            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoTonKhoGLT" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlDetailList_PrintBCTKGLT(List<Sale_BaoCaoTonKhoViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã SP</th>\r\n";
            detailLists += "		<th>Tên SP</th>\r\n";
            detailLists += "		<th>Danh mục</th>\r\n";
            detailLists += "		<th>Nhà sản xuất</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Số lượng</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CategoryCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Manufacturer + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-center\">" + item.ProductUnit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"9\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion

        #region BaoCaoXuatKhoGoiLieuTrinh
        public ActionResult BaoCaoXuatKhoGLT()
        {
            var warehouseList = warehouseRepository.GetAllWarehouse();
            //.Where(x => ("," + Helpers.Common.CurrentUser.DrugStore + ",").Contains("," + x.BranchId + ",") == true);
            ViewBag.warehouseList = warehouseList.AsEnumerable().Select(x => new SelectListItem { Value = x.Id + "", Text = x.Name });
            return View();
        }
        public ActionResult Sale_BaoCaoXuatKhoGLT()
        {
            return View();
        }
        public ActionResult PrintBCXKGLT(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId, int? WarehouseId, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = new List<Sale_BaoCaoXuatKhoViewModel>();
            data = SqlHelper.QuerySP<Erp.BackOffice.Sale.Models.Sale_BaoCaoXuatKhoViewModel>("spSale_BaoCaoXuatKho", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = "",
                DistrictId = ""
            }).Where(x => x.ProductType == "productpackage").ToList();
            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");

            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCXKGLT(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo xuất kho túi liệu trình");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoXuatKhoGLT" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlDetailList_PrintBCXKGLT(List<Sale_BaoCaoXuatKhoViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã SP</th>\r\n";
            detailLists += "		<th>Tên SP</th>\r\n";
            detailLists += "		<th>Danh mục</th>\r\n";
            detailLists += "		<th>Nhà sản xuất</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>Đơn giá</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Xuất bán</th>\r\n";
            detailLists += "		<th>Xuất vận chuyển</th>\r\n";
            detailLists += "		<th>Xuất sử dụng</th>\r\n";
            detailLists += "		<th>Tổng xuất</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                var subTotal = item.invoice + item._internal + item.service;
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CategoryCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Manufacturer + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Price, null) + "</td>\r\n"
                + "<td class=\"text-center\">" + item.Unit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.invoice) + "</td>\r\n"
            + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item._internal) + "</td>\r\n"
              + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.service) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(subTotal, null) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"10\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.invoice), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x._internal), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.service), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.service + x._internal + x.invoice), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        public PartialViewResult _GetListSale_BCXKGLT(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId, int? WarehouseId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = new List<Sale_BaoCaoXuatKhoViewModel>();
            data = SqlHelper.QuerySP<Erp.BackOffice.Sale.Models.Sale_BaoCaoXuatKhoViewModel>("spSale_BaoCaoXuatKho", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).Where(x => x.ProductType == "productpackage").ToList();
            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");
            return PartialView(data);
        }

        #endregion

        # region Sale_BaoCaoNhapXuatTonGoiLieuTrinh
        public ActionResult Sale_BaoCaoNhapXuatTonGLT()
        {
            return View();
        }

        public PartialViewResult _GetListSale_BCNXTGLT(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId, int? WarehouseId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = CityId,
                DistrictId = DistrictId
            }).Where(x => x.ProductType == "productpackage").ToList();
            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");
            return PartialView(data);
        }
        public ActionResult PrintBCNXTGLT(string single, int? year, int? month, int? quarter, int? week, string CityId, string DistrictId, int? branchId, int? WarehouseId, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = SqlHelper.QuerySP<Sale_BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                WarehouseId = WarehouseId,
                branchId = branchId,
                CityId = "",
                DistrictId = ""
            }).Where(x => x.ProductType == "productpackage").ToList();
            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintBCNXTGLT(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo nhập/xuất/tồn gói liệu trình");
            if (ExportExcel)
            {
                ViewBag.closePopup = "true";
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoXuatNhapTonkhoGLT" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }

        string buildHtmlDetailList_PrintBCNXTGLT(List<Sale_BaoCaoNhapXuatTonViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã SP</th>\r\n";
            detailLists += "		<th>Tên SP</th>\r\n";
            detailLists += "		<th>Kho</th>\r\n";
            detailLists += "		<th>Số Lô</th>\r\n";
            detailLists += "		<th>Hạn dùng</th>\r\n";
            detailLists += "		<th>Đơn giá nhập</th>\r\n";
            detailLists += "		<th>Đơn giá xuất</th>\r\n";
            detailLists += "		<th>ĐVT</th>\r\n";
            detailLists += "		<th>Tồn đầu kỳ</th>\r\n";

            detailLists += "		<th>Nhập trong kỳ</th>\r\n";
            detailLists += "		<th>Xuất trong kỳ</th>\r\n";
            detailLists += "		<th>Tồn cuối kỳ</th>\r\n";
            detailLists += "		<th>Tổng tiền tồn</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.WarehouseName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.LoCode + "</td>\r\n"
                + "<td class=\"text-right code_product\">" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd-MM-yyyy") : "") + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound, null) + "</td>\r\n"
                + "<td class=\"text-right \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceOutbound, null) + "</td>\r\n"
                + "<td class=\"text-center\">" + item.ProductUnit + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.First_Remain) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_InboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Last_OutboundQuantity) + "</td>\r\n"
                + "<td class=\"text-right orderNo\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Remain, null) + "</td>\r\n"
                + "<td class=\"text-right chiet_khau\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceInbound * item.Remain, null) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"9\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.First_Remain), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_InboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Last_OutboundQuantity), null)
                         + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain), null)
                          + "</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Remain * x.PriceInbound), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }

        #endregion

        #region Sale_Get_Membership_StatusExpired
        public ActionResult Sale_Get_Membership_StatusExpired()
        {
            return View();
        }

        public List<Sale_Get_Membership_StatusExpiredViewModel> GETSale_Get_Membership_StatusExpired(int? BranchId, string StartDate, string EndDate, string Type)
        {
            Sale_Get_Membership_StatusExpiredViewModel model = new Sale_Get_Membership_StatusExpiredViewModel();
            //var q = staffMadeRepository.GetvwAllStaffMade().Where(x => x.Status == "accept" && x.SchedulingStatus == "complete");
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            Type = Type == null ? "" : Type;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Get_Membership_StatusExpiredViewModel>("spSale_Get_Membership_StatusExpired", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                Type = Type,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_Get_Membership_StatusExpired(int? BranchId, string StartDate, string EndDate, string Type)
        {
            var data = GETSale_Get_Membership_StatusExpired(BranchId, StartDate, EndDate, Type);
            return PartialView(data);
        }
        public ActionResult PrintSale_Get_Membership_StatusExpired(int? BranchId, string StartDate, string EndDate, string Type, bool ExportExcel = false)
        {
            var data = GETSale_Get_Membership_StatusExpired(BranchId, StartDate, EndDate, Type);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Get_Membership_StatusExpired(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh sách khách không đi CSD/CST");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_StaffMade" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_Get_Membership_StatusExpired(List<Sale_Get_Membership_StatusExpiredViewModel> detailList)
        {
            string detailLists = "<table border=\"1\"  class=\"invoice-detail\">";
            detailLists += "<thead>";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th  style=\"width:100px\">Ngày tạo</th>";
            detailLists += "		<th>Mã phiếu</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th style=\"width:60px\">Tên khách hàng</th>";
            detailLists += "		<th style=\"width:60px\">Người quản lý</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>";
            detailLists += "<tbody>";
            var index = 1;
            foreach (var item in detailList)
            {
                detailLists += "<tr>"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>"
                + "<td class=\"text-left code_product\">" + item.Code + "</td>"
                + "<td class=\"text-left code_product\">" + item.CreatedDate + "</td>"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>"
                + "<td class=\"text-right orderNo\">" + item.CustomerName + "</td>"
                + "<td class=\"text-left code_product\">" + item.ManagerName + "</td>"
                + "</tr>";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font-weight:bold\">";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }
        #endregion

        #region KhachSuDongDVCSD
        public ActionResult Sale_Get_Sale_SchedulingHistory()
        {
            return View();
        }

        public List<Sale_Get_Sale_SchedulingHistoryViewModel> GETSale_Get_Sale_SchedulingHistory(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductId = ProductId == null ? 0 : ProductId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy HH:mm");
                EndDate = retDateTime.ToString("dd/MM/yyyy HH:mm");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Get_Sale_SchedulingHistoryViewModel>("spSale_Get_Sale_SchedulingHistory", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductId = ProductId,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_Get_Sale_SchedulingHistory(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_Get_Sale_SchedulingHistory(ProductId, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_Get_Sale_SchedulingHistory(int? ProductId, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_Get_Sale_SchedulingHistory(ProductId, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Get_Sale_SchedulingHistory(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách sử dụng dịch vụ");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_Get_Sale_SchedulingHistory(List<Sale_Get_Sale_SchedulingHistoryViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Mã phiếu</th>";
            detailLists += "		<th>Mã sản phẩm</th>";
            detailLists += "		<th>Tên sản phẩm</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left code_product\">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }

        #endregion

        #region ThongKeLuongBanHangTheoTungKhach
        public ActionResult Sale_Get_ProductInvoiceDetail()
        {
            return View();
        }


        public List<Sale_Get_ProductInvoiceDetailViewModel> GetSale_Get_ProductInvoiceDetail(int? BranchId, string StartDate, string EndDate, int? CustomerId)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            CustomerId = CustomerId == null ? 0 : CustomerId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Get_ProductInvoiceDetailViewModel>("spSale_Get_ProductInvoiceDetail", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                CustomerId = CustomerId,
            }).ToList();
            return data;
        }


        public PartialViewResult _GetListSale_Get_ProductInvoiceDetail(int? BranchId, string StartDate, string EndDate, int? CustomerId)
        {
            var data = GetSale_Get_ProductInvoiceDetail(BranchId, StartDate, EndDate, CustomerId);
            return PartialView(data);
        }


        //public ActionResult PrintSale_Get_ProductInvoiceDetail(int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        //{
        //    var data = GetSale_Get_ProductInvoiceDetail(BranchId, StartDate, EndDate);
        //    var model = new TemplatePrintViewModel();
        //    var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
        //    var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
        //    var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
        //    var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
        //    var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
        //    var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
        //    var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
        //    var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
        //    var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        //    model.Content = template.Content;
        //    model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Get_ProductInvoiceDetail(data));
        //    model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
        //    model.Content = model.Content.Replace("{System.CompanyName}", company);
        //    model.Content = model.Content.Replace("{System.AddressCompany}", address);
        //    model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
        //    model.Content = model.Content.Replace("{System.Fax}", fax);
        //    model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
        //    model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
        //    model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        //    model.Content = model.Content.Replace("{Title}", "Khách hàng");
        //    if (ExportExcel)
        //    {
        //        Response.AppendHeader("content-disposition", "attachment;filename=" + "Sale_Get_Customer" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
        //        Response.Charset = "";
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Write(model.Content);
        //        Response.End();
        //    }
        //    return View(model);
        //}


        //string buildHtmlSale_Get_ProductInvoiceDetail(List<Sale_Get_ProductInvoiceDetailViewModel> detailList)
        //{
        //    string detailLists = "<table class=\"invoice - detail\">";
        //    detailLists += "<thead>";
        //    detailLists += "	<tr>";
        //    detailLists += "		<th>Tên khách hàng</th>";
        //    detailLists += "		<th>Stt</th>";
        //    detailLists += "		<th>Mã khách hàng</th>";
        //    detailLists += "		<th>Mã sản phẩm</th>";
        //    detailLists += "        <th>Tên sản phẩm</th>";
        //    detailLists += "        <th>Số lượng</th>";
        //    detailLists += "	</tr>";
        //    detailLists += "</thead>";
        //    detailLists += "<tbody>";
        //    var index = 1;
        //    foreach (var item in detailList)
        //    {
        //        detailLists += "<tr>"
        //        + "<td>" + item.CustomerName + "</td>"
        //        + "<td>" + (index++) + "</td>"
        //        + "<td>" + item.CustomerCode + "</td>"
        //        + "<td>" + item.ProductCode + "</td>"
        //        + "<td>" + item.ProductName + "</td>"
        //        + "<td>" + item.Quantity + "</td>"
        //        + "</tr>";
        //    }
        //    return detailLists;
        //}
        #endregion

        #region Sale_Report_KhachMuaDV
        public ActionResult Sale_Report_KhachMuaDV()
        {
            return View();
        }

        public List<Sale_Report_KhachMuaDVViewModel> GETSale_Report_KhachMuaDV(string ProductGroup, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Report_KhachMuaDVViewModel>("spSale_Report_KhachMuaDV", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductGroup = ProductGroup,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_Report_KhachMuaDV(string ProductGroup, int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_Report_KhachMuaDV(ProductGroup, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_Report_KhachMuaDV(string ProductGroup, int? BranchId, string StartDate, string EndDate, string Type, bool ExportExcel = false)
        {
            var data = GETSale_Report_KhachMuaDV(ProductGroup, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Report_KhachMuaDV(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách hàng mua sản phẩm");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_Report_KhachMuaDV(List<Sale_Report_KhachMuaDVViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Mã sản phẩm/dịch vụ</th>";
            detailLists += "		<th>Tên sản phẩm/dịch vụ</th>";
            detailLists += "		<th>Nhóm sản phẩm/dịch vụ</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CategoryCode + "</td>\r\n"
                + "</tr>";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }

        #endregion

        #region KhachHangTangSample
        public ActionResult Sale_ProductSampleDetail()
        {
            return View();
        }

        public List<Sale_ProductSampleDetailViewModel> GETSale_ProductSampleDetail(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductId = ProductId == null ? 0 : ProductId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_ProductSampleDetailViewModel>("spSale_ProductSampleDetail", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductId = ProductId,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_ProductSampleDetail(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_ProductSampleDetail(ProductId, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_ProductSampleDetail(int? ProductId, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_ProductSampleDetail(ProductId, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_ProductSampleDetail(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách tặng sample");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_ProductSampleDetail" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_ProductSampleDetail(List<Sale_ProductSampleDetailViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo</th>";
            detailLists += "		<th>Trạng thái</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Mã phiếu tặng</th>";
            detailLists += "		<th>Mã sản phẩm</th>";
            detailLists += "		<th>Tên sản phẩm</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center \">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Status + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CompanyName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductSampleCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Sale_Report_SaleReturns
        public ActionResult Sale_Report_SaleReturns()
        {
            return View();
        }

        public List<Sale_Report_SaleReturnsViewModel> GETSale_Report_SaleReturns(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductId = ProductId == null ? 0 : ProductId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Report_SaleReturnsViewModel>("spSale_Report_SaleReturns", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductId = ProductId,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_Report_SaleReturns(int? ProductId, int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_Report_SaleReturns(ProductId, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_Report_SaleReturns(int? ProductId, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_Report_SaleReturns(ProductId, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Report_SaleReturns(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Doanh số đổi hàng ");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_ProductSampleDetail" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_Report_SaleReturns(List<Sale_Report_SaleReturnsViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Trạng thái</th>";
            detailLists += "		<th>Mã đơn hàng</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Số lượng</th>";
            detailLists += "		<th>Tổng tiền</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center \">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Status + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.TotalAmount + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"5\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">";
            detailLists += Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.TotalAmount), null); detailLists += "</td>";
            detailLists += Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null); detailLists += "</td>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Sale_Report_CustomerIsCNC
        public ActionResult Sale_Report_CustomerIsCNC(int? ProductId, int? BranchId, string StartDate, string EndDate, string Type, int? IsCNC)
        {
            var data = GETSale_Report_CustomerIsCNC(ProductId, BranchId, StartDate, EndDate, Type, IsCNC);
            return View(data);
        }

        public List<ProductInvoiceDetailViewModel> GETSale_Report_CustomerIsCNC(int? ProductId, int? BranchId, string StartDate, string EndDate, string Type, int? IsCNC)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            ProductId = ProductId == null ? 0 : ProductId;
            Type = Type == null ? "" : Type;
            IsCNC = IsCNC == null ? 0 : IsCNC;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("spSale_Report_CustomerIsCNC", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                ProductId = ProductId,
                Type = Type,
                IsCNC = IsCNC,
            }).ToList();
            return data;
        }
        //public PartialViewResult _GetListSale_Report_CustomerIsCNC(int? ProductId, int? BranchId, string StartDate, string EndDate, string Type, int? IsCNC)
        //{
        //    var data = GETSale_Report_CustomerIsCNC(ProductId, BranchId, StartDate, EndDate,Type,IsCNC);
        //    return PartialView(data);
        //}
        //public ActionResult PrintSale_Report_CustomerIsCNC(int? ProductId, int? BranchId, string StartDate, string EndDate, string Type, int? IsCNC, bool ExportExcel = false)
        //{
        //    var data = GETSale_Report_CustomerIsCNC(ProductId, BranchId, StartDate, EndDate, Type, IsCNC);
        //    var model = new TemplatePrintViewModel();
        //    var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
        //    var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
        //    var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
        //    var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
        //    var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
        //    var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
        //    var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
        //    var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
        //    var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
        //    model.Content = template.Content;
        //    model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Report_CustomerIsCNC(data));
        //    model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
        //    model.Content = model.Content.Replace("{System.CompanyName}", company);
        //    model.Content = model.Content.Replace("{System.AddressCompany}", address);
        //    model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
        //    model.Content = model.Content.Replace("{System.Fax}", fax);
        //    model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
        //    model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
        //    model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
        //    model.Content = model.Content.Replace("{Title}", "Khách hàng mua dịch vụ");
        //    if (ExportExcel)
        //    {
        //        Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
        //        Response.Charset = "";
        //        Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //        Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        //        Response.Write(model.Content);
        //        Response.End();
        //    }
        //    return View(model);
        //}
        //string buildHtmlSale_Report_CustomerIsCNC(List<Sale_Report_CustomerIsCNCViewModel> detailList)
        //{
        //    string detailLists = "<table class=\"invoice-detail\">\r\n";
        //    detailLists += "<thead>\r\n";
        //    detailLists += "	<tr>";
        //    detailLists += "		<th>STT</th>";
        //    detailLists += "		<th>Ngày tạo</th>";
        //    detailLists += "		<th>Mã khách hàng</th>";
        //    detailLists += "		<th>Tên khách hàng</th>";
        //    detailLists += "		<th>Mã sản phẩm/dịch vụ</th>";
        //    detailLists += "		<th>Tên sản phẩm/dịch vụ</th>";
        //    detailLists += "		<th>Nhóm sản phẩm/dịch vụ</th>";
        //    detailLists += "	</tr>";
        //    detailLists += "</thead>\r\n";
        //    detailLists += "<tbody>\r\n";
        //    var index = 1;

        //    foreach (var item in detailList)
        //    {
        //        detailLists += "<tr>\r\n"
        //        + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
        //        + "<td class=\"text-left \">" + item.CreatedDate + "</td>\r\n"
        //        + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
        //        + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
        //        + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
        //        + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
        //        + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
        //        + "</tr>";
        //    }
        //    detailLists += "</tbody>\r\n";
        //    detailLists += "</tfoot>\r\n</table>\r\n";
        //    return detailLists;
        //}

        #endregion

        #region Sale_Report_KhachVip
        public ActionResult Sale_Report_KhachVip()
        {
            return View();
        }

        public List<Sale_Report_KhachVipViewModel> GETSale_Report_KhachVip(int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Report_KhachVipViewModel>("spSale_Report_KhachVip", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_Report_KhachVip(int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_Report_KhachVip(BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_Report_KhachVip(int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_Report_KhachVip(BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Report_KhachVip(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách Vip");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_Report_KhachVip(List<Sale_Report_KhachVipViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Xếp hạng</th>";
            detailLists += "		<th>Trạng thái</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CompanyName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Ratings + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Status + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Sale_BaoCao_KhachTuVanMaKhongMua
        public ActionResult Sale_BaoCao_KhachTuVanMaKhongMua()
        {
            return View();
        }

        public List<Sale_BaoCao_KhachTuVanMaKhongMuaViewModel> GETSale_BaoCao_KhachTuVanMaKhongMua(string EconomicStatus, int? BranchId, string StartDate, string EndDate)
        {
            BranchId = BranchId == null ? 0 : BranchId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            EconomicStatus = EconomicStatus == null ? "" : EconomicStatus;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_BaoCao_KhachTuVanMaKhongMuaViewModel>("spSale_BaoCao_KhachTuVanMaKhongMua", new
            {
                BranchId = BranchId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                EconomicStatus = EconomicStatus
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_BaoCao_KhachTuVanMaKhongMua(string EconomicStatus, int? BranchId, string StartDate, string EndDate)
        {
            var data = GETSale_BaoCao_KhachTuVanMaKhongMua(EconomicStatus, BranchId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_BaoCao_KhachTuVanMaKhongMua(string EconomicStatus, int? BranchId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_BaoCao_KhachTuVanMaKhongMua(EconomicStatus, BranchId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_BaoCao_KhachTuVanMaKhongMua(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách tư vẫn mà không mua");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_BaoCao_KhachTuVanMaKhongMua(List<Sale_BaoCao_KhachTuVanMaKhongMuaViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Trạng thái</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CompanyName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Status + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region DoanhSoThucBan
        public ActionResult Sale_BaoCaoDoanhSoThucBan()
        {
            return View();
        }

        public PartialViewResult _GetListSale_BaoCaoDoanhSoThucBan(string single, int? year, int? month, int? quarter, int? week, string ProductGroup, string Origin, int? BranchId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            Origin = Origin == null ? "" : Origin;
            BranchId = BranchId == null ? 0 : BranchId;
            //CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            //DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = SqlHelper.QuerySP<Sale_BaoCaoDoanhSoThucBanViewModel>("spSale_BaoCaoDoanhSoThucBan", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                BranchId = BranchId,
                ProductGroup = ProductGroup,
                Origin = Origin,
                //CityId = CityId,
                //DistrictId=DistrictId,
            }).ToList();
            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");
            return PartialView(data);
        }
        public ActionResult PrintSale_BaoCaoDoanhSoThucBan(string single, int? year, int? month, int? quarter, int? week, string ProductGroup, string Origin, int? BranchId, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            BranchId = BranchId == null ? 0 : BranchId;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = SqlHelper.QuerySP<Sale_BaoCaoDoanhSoThucBanViewModel>("spSale_BaoCaoDoanhSoThucBan", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                BranchId = BranchId,
                ProductGroup = ProductGroup,
                Origin = Origin
            }).ToList();
            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintSale_BaoCaoDoanhSoThucBan(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Doanh số thực bán");
            if (ExportExcel)
            {
                ViewBag.closePopup = "true";
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoDoanhSoThucBan" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }

        string buildHtmlDetailList_PrintSale_BaoCaoDoanhSoThucBan(List<Sale_BaoCaoDoanhSoThucBanViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Ngày tạo</th>\r\n";
            detailLists += "		<th>Chi nhánh</th>\r\n";
            detailLists += "		<th>Mã đơn hàng</th>\r\n";
            detailLists += "		<th>Tên khách hàng</th>\r\n";
            detailLists += "		<th>Doanh số</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-center \">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-center \">" + item.BranchName + "</td>\r\n"
                + "<td class=\"text-center \">" + item.ProductInvoiceCode + "</td>\r\n"
                + "<td class=\"text-center\">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.TotalAmount + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"5\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.TotalAmount), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion
        #region DoanhSoAoBan
        public ActionResult spSale_BaoCaoDoanhSoAoBan()
        {
            return View();
        }

        public PartialViewResult _GetListspSale_BaoCaoDoanhSoAoBan(string single, int? year, int? month, int? quarter, int? week, string ProductGroup, string Origin, int? BranchId, int? IsCNC, int? UserId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            Origin = Origin == null ? "" : Origin;
            BranchId = BranchId == null ? 0 : BranchId;
            UserId = UserId == null ? 0 : UserId;
            IsCNC = IsCNC == null ? 0 : IsCNC;
            //CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            //DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = SqlHelper.QuerySP<spSale_BaoCaoDoanhSoAoBanViewModel>("spSale_BaoCaoDoanhSoAoBan", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                BranchId = BranchId,
                ProductGroup = ProductGroup,
                Origin = Origin,
                UserId = UserId,
                IsCNC = IsCNC,
            }).ToList();
            ViewBag.StartDate = StartDate.ToString("dd/MM/yyyy");
            ViewBag.EndDate = EndDate.ToString("dd/MM/yyyy");
            return PartialView(data);
        }
        public ActionResult PrintspSale_BaoCaoDoanhSoAoBan(string single, int? year, int? month, int? quarter, int? week, string ProductGroup, string Origin, int? BranchId, int? UserId, int? IsCNC, bool ExportExcel = false)
        {
            var model = new TemplatePrintViewModel();
            //lấy logo công ty
            var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
            var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
            var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
            var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
            var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
            var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
            var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
            var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;
            single = single == null ? "month" : single;
            quarter = quarter == null ? 1 : quarter;
            ProductGroup = ProductGroup == null ? "" : ProductGroup;
            Origin = Origin == null ? "" : Origin;
            BranchId = BranchId == null ? 0 : BranchId;
            UserId = UserId == null ? 0 : UserId;
            IsCNC = IsCNC == null ? 0 : IsCNC;
            if (!Filters.SecurityFilter.IsAdmin() && BranchId == 0)
            {
                BranchId = Helpers.Common.CurrentUser.BranchId;
            }
            Calendar calendar = CultureInfo.InvariantCulture.Calendar;
            var weekdefault = calendar.GetWeekOfYear(DateTime.Now, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
            week = week == null ? weekdefault : week;
            DateTime StartDate = DateTime.Now;
            DateTime EndDate = DateTime.Now;

            ViewBag.DateRangeText = Helpers.Common.ConvertToDateRange(ref StartDate, ref EndDate, single, year.Value, month.Value, quarter.Value, ref week);

            var data = SqlHelper.QuerySP<spSale_BaoCaoDoanhSoAoBanViewModel>("spSale_BaoCaoDoanhSoAoBan", new
            {
                StartDate = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndDate = EndDate.ToString("yyyy-MM-dd HH:mm:ss"),
                BranchId = BranchId,
                ProductGroup = ProductGroup,
                Origin = Origin,
                UserId = UserId,
                IsCNC = IsCNC,
            }).ToList();
            //lấy template phiếu xuất.
            var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("PrintReport")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
            //truyền dữ liệu vào template.
            model.Content = template.Content;

            model.Content = model.Content.Replace("{DataTable}", buildHtmlDetailList_PrintspSale_BaoCaoDoanhSoAoBan(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Doanh số ảo bán");
            if (ExportExcel)
            {
                ViewBag.closePopup = "true";
                Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoDoanhSoThucBan" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }

        string buildHtmlDetailList_PrintspSale_BaoCaoDoanhSoAoBan(List<spSale_BaoCaoDoanhSoAoBanViewModel> detailList)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Ngày tạo</th>\r\n";
            detailLists += "		<th>Chi nhánh</th>\r\n";
            detailLists += "		<th>Mã đơn hàng</th>\r\n";
            detailLists += "		<th>Tên khách hàng</th>\r\n";
            detailLists += "		<th>Mã nhân viên</th>\r\n";
            detailLists += "		<th>Tên nhân viên</th>\r\n";
            detailLists += "		<th>Doanh số</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-center \">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-center \">" + item.BranchName + "</td>\r\n"
                + "<td class=\"text-center \">" + item.ProductInvoiceCode + "</td>\r\n"
                + "<td class=\"text-center\">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.UserCode + "</td>\r\n"
                + "<td class=\"text-right\">" + item.UserName + "</td>\r\n"
                + "<td class=\"text-right\">" + item.TotalAmount + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"7\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.TotalAmount), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion

        #region KhuyenMai/GiamGia
        public ActionResult Sale_Report_CommissionCus()
        {
            return View();
        }

        public List<Sale_Report_CommissionCusViewModel> GETSale_Report_CommissionCus(string Type)
        {
            Type = Type == null ? "" : Type;
            //StartDate = StartDate == null ? "" : StartDate;
            //EndDate = EndDate == null ? "" : EndDate;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            //if (StartDate == null && EndDate == null)
            //{
            //    StartDate = aDateTime.ToString("dd/MM/yyyy");
            //    EndDate = retDateTime.ToString("dd/MM/yyyy");
            //}
            //var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            //var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Report_CommissionCusViewModel>("spSale_Report_CommissionCus", new
            {
                Type = Type
                //BranchId = BranchId,
                //StartDate = d_startDate,
                //EndDate = d_endDate,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_Report_CommissionCus(string Type)
        {
            var data = GETSale_Report_CommissionCus(Type);
            return PartialView(data);
        }
        public ActionResult PrintSSale_Report_CommissionCus(string Type, bool ExportExcel = false)
        {
            var data = GETSale_Report_CommissionCus(Type);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Report_CommissionCus(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách Vip");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_Report_CommissionCus(List<Sale_Report_CommissionCusViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Chương trình khuyễn mãi</th>";
            detailLists += "		<th>Mã sản phẩm/dịch vụ</th>";
            detailLists += "		<th>Tên sản phẩm dịch vụ</th>";
            detailLists += "		<th>Số lượng</th>";
            detailLists += "		<th>HSD(tháng)</th>";
            detailLists += "		<th>Ngày bắt đầu</th>";
            detailLists += "		<th>Ngày kết thúc</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Name + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Quantity + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ExpiryMonth + "</td>\r\n"
                + "<td class=\"text-left \">" + item.StartDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.EndDate + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>";
            detailLists += "<tfoot style=\"font - weight:bold\">";
            detailLists += "<tr>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "<td class=\"text - right\">";
            detailLists += "</td>";
            detailLists += "</tr>";
            detailLists += "</tfoot></table>";
            return detailLists;
        }

        #endregion

        #region Sale_Report_TuiLieuTrinh
        public ActionResult Sale_Report_TuiLieuTrinh()
        {
            return View();
        }

        public List<Sale_Report_TuiLieuTrinhViewModel> GETSale_Report_TuiLieuTrinh(int? ServiceId, string StartDate, string EndDate)
        {
            ServiceId = ServiceId == null ? 0 : ServiceId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_Report_TuiLieuTrinhViewModel>("spSale_Report_TuiLieuTrinh", new
            {
                ServiceId = ServiceId,
                StartDate = d_startDate,
                EndDate = d_endDate,
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_Report_TuiLieuTrinh(int? ServiceId, string StartDate, string EndDate)
        {
            var data = GETSale_Report_TuiLieuTrinh(ServiceId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_Report_TuiLieuTrinh(int? ServiceId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_Report_TuiLieuTrinh(ServiceId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_Report_TuiLieuTrinh(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Khách sử dụng dịch vụ");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_Report_TuiLieuTrinh(List<Sale_Report_TuiLieuTrinhViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Mã dịch vụ</th>";
            detailLists += "		<th>Tên dịch vụ</th>";
            detailLists += "		<th>Mã túi liệu trình</th>";
            detailLists += "		<th>Tên túi liệu trình</th>";
            detailLists += "		<th>Số lượng</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ServiceCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ServiceName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Quantity + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr><td colspan=\"5\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(detailList.Sum(x => x.Quantity), null)
                         + "</tr>\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }

        #endregion

        #region Sale_BaoCaoPhieuCSDDoiTen
        public ActionResult Sale_BaoCaoPhieuCSDDoiTen()
        {
            return View();
        }

        public List<Sale_BaoCaoPhieuCSDDoiTenViewModel> GETSale_BaoCaoPhieuCSDDoiTen(int? BranchId, int? ProductId, string StartDate, string EndDate)
        {
            ProductId = ProductId == null ? 0 : ProductId;
            StartDate = StartDate == null ? "" : StartDate;
            EndDate = EndDate == null ? "" : EndDate;
            BranchId = BranchId == null ? 0 : BranchId;
            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
            if (StartDate == null && EndDate == null)
            {
                StartDate = aDateTime.ToString("dd/MM/yyyy");
                EndDate = retDateTime.ToString("dd/MM/yyyy");
            }
            var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

            var data = SqlHelper.QuerySP<Sale_BaoCaoPhieuCSDDoiTenViewModel>("spSale_BaoCaoPhieuCSDDoiTen", new
            {
                ProductId = ProductId,
                StartDate = d_startDate,
                EndDate = d_endDate,
                BranchId = BranchId
            }).ToList();
            return data;
        }
        public PartialViewResult _GetListSale_BaoCaoPhieuCSDDoiTen(int? BranchId, int? ProductId, string StartDate, string EndDate)
        {
            var data = GETSale_BaoCaoPhieuCSDDoiTen(BranchId, ProductId, StartDate, EndDate);
            return PartialView(data);
        }
        public ActionResult PrintSale_BaoCaoPhieuCSDDoiTen(int? BranchId, int? ProductId, string StartDate, string EndDate, bool ExportExcel = false)
        {
            var data = GETSale_BaoCaoPhieuCSDDoiTen(BranchId, ProductId, StartDate, EndDate);
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_BaoCaoPhieuCSDDoiTen(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Phiếu chăm sóc da đổi tên");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Get_Sale_SchedulingHistory" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_BaoCaoPhieuCSDDoiTen(List<Sale_BaoCaoPhieuCSDDoiTenViewModel> detailList)
        {
            string detailLists = "<table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            detailLists += "		<th>Ngày tạo phiếu</th>";
            detailLists += "		<th>Mã phiếu</th>";
            detailLists += "		<th>Tên khách hàng</th>";
            detailLists += "		<th>Tên sản phẩm/dịch vụ</th>";
            detailLists += "		<th>Ngày hết hạn</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CreatedDate + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CompanyName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ExpiryDate + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }

        #endregion

        #region DonHangTheoNhanHang
        public List<ProductInvoiceViewModel> GETDonHangTheoNhanHang(int? Year, string txtCode, string txtMinAmount, string txtMaxAmount, string txtCusName, string startDate, string endDate, int? BranchId, string txtCustomerCode, string countForBrand, int? userTypeId, string GDNDT)
        {
            Year = Year == null ? DateTime.Now.Year : Year;
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
            var q = productInvoiceRepository.GetAllvwProductInvoiceFull_NVKD();
            //.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId);
            var model = q.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                TotalAmount = item.TotalAmount,
                //FixedDiscount = item.FixedDiscount,
                TaxFee = item.TaxFee,
                CodeInvoiceRed = item.CodeInvoiceRed,
                Status = item.Status,
                IsArchive = item.IsArchive,
                Note = item.Note,
                CancelReason = item.CancelReason,
                BranchId = item.BranchId,
                CustomerId = item.CustomerId,
                BranchName = item.BranchName,
                ManagerStaffId = item.ManagerStaffId,
                ManagerStaffName = item.ManagerStaffName,
                ManagerUserName = item.ManagerUserName,
                CountForBrand = item.CountForBrand,
                TotalDebit = item.TotalDebit,
                TotalCredit = item.TotalCredit,
                TongConNo = (item.TotalDebit - item.TotalCredit),
                UserTypeName = item.UserTypeName,
                UserTypeId = item.UserTypeId,
                Month = item.Month,
                Year = item.Year,
                GDDauTienThanhToanHet = item.GDDauTienThanhToanHet,
                GDNgayDauTienThanhToanHet = item.GDNgayDauTienThanhToanHet,
                SPHangHoa = item.SPHangHoa,
                SPDichvu = item.SPDichvu,
                Hangduoctang = item.Hangduoctang,
                Discount_VIP = (float)item.Discount_VIP,
                Discount_KM = (float)item.Discount_KM,
                Discount_DB = (float)item.Discount_DB,
                VoucherDate = item.VoucherDate,
                TyleHuong = item.TyleHuong
            }).ToList();

            if (startDate == null && endDate == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }
            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    model = model.Where(x => x.VoucherDate >= d_startDate && x.VoucherDate <= d_endDate).ToList();
                }
            }
            //if (Year != null)
            //{
            //    model = model.Where(x => x.Year == Year).ToList();
            //}
            if (!string.IsNullOrEmpty(txtCode))
            {
                model = model.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode.Trim().ToLower())
                    //|| x.WarehouseSourceId == warehouseSourceId
                    ).ToList();
            }

            if (!string.IsNullOrEmpty(txtCusName))
            {
                txtCusName = Helpers.Common.ChuyenThanhKhongDau(txtCusName);
                model = model.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(txtCusName)).ToList();
            }



            //if (Helpers.Common.CurrentUser.BranchId != null)
            //{
            //    model = model.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId.Value).ToList();
            //}



            //begin hoapd tinh cho truong hop neu khach mua don dau KM 50% ma mua don tiep theo sau thi van duoc coi la moi
            //b1. loc ra tat ca cac don khang co khuyen mai 50%
            var modelKM50 = model.Where(x => (Helpers.Common.NVL_NUM_DECIMAL_NEW(x.Discount_DB) + Helpers.Common.NVL_NUM_DECIMAL_NEW(x.Discount_KM)) == 50).ToList();
            foreach (var item in modelKM50)
            {
                if (item.CustomerCode == "37023")
                {
                    int a = 1;
                }
                if (item.GDNgayDauTienThanhToanHet == "Y")
                {
                    //b2. tim tat cac cac don hang cua khach hang nay mua sau ngay tren va sap xep theo thu tu giam dan
                    var modelKM50_KH = model.Where(x => x.CustomerId == item.CustomerId && x.CreatedDate >= item.CreatedDate && x.Id != item.Id).OrderBy(c => c.CreatedDate).ToList();
                    if ((modelKM50_KH != null) && (modelKM50_KH.Count() > 0))
                    {
                        var modelKM50_KH_UPDATE = modelKM50_KH.Where(x => x.VoucherDate.ToString().Substring(0, 10) == modelKM50_KH[0].VoucherDate.ToString().Substring(0, 10)).OrderBy(c => c.CreatedDate).ToList();

                        if (modelKM50_KH_UPDATE != null && modelKM50_KH_UPDATE.Count > 0)
                        {
                            foreach (var item1 in modelKM50_KH_UPDATE)
                            {
                                var modelKM50_SETY = model.Where(x => x.Id == item1.Id).FirstOrDefault();
                                modelKM50_SETY.GDNgayDauTienThanhToanHet = "Y";
                                //các đơn chuyển từ đơn mới này cũng là mới luôn
                                var check = invoiceRepository.GetMMByOldId(modelKM50_SETY.Id);
                                if (check != null)
                                {
                                    var modelNext = model.SingleOrDefault(x => x.Id == check.ToProductInvoiceId);

                                    if (modelNext != null)
                                    {
                                        modelNext.GDNgayDauTienThanhToanHet = "Y";
                                    }
                                }
                                //
                            }
                        }



                    }

                }
            }


            //end hoapd tinh cho truong hop neu khach mua don dau KM 50% ma mua don tiep theo sau thi van duoc coi la moi
            foreach (var item in model)
            {
                if (item.TyleHuong == null)
                {
                    item.TyleHuong = 100;
                }

                //Tim kiem don da chot
                var itemdaco = BC_DOANHSO_NHANHANGRepository.GetBC_DOANHSO_NHANHANGByMaDH(item.Code, BranchId);

                if (itemdaco != null)
                {
                    item.Status = "dachot";
                    var Idx = productInvoiceRepository.GetvwProductInvoiceByCode(BranchId.Value, item.Code);
                    if (Idx != null)
                    {
                        item.Id = Idx.Id;
                        item.Note = Idx.Note;
                    }
                }

                //Kiểm tra đơn hàng quá 3 tháng hủy chuyển - nếu là mới thì đơn dc chuyển sẽ là cũ
                var monthSl = DateTime.Now.Year * 12 + DateTime.Now.Month - (item.Year * 12 + item.Month);

                //Kiem tra don hang co huy chuyen hay khong 
                var checkYN = invoiceRepository.GetMMByOldId(item.Id);
                if (checkYN != null)
                {
                    var modelNext = model.SingleOrDefault(x => x.Id == checkYN.ToProductInvoiceId);
                    var productinvoice = invoiceRepository.GetProductInvoiceById(item.Id);
                    if (modelNext != null && item.GDNgayDauTienThanhToanHet == "Y" && checkYN.TotalAmount == productinvoice.TotalAmount && monthSl <= 3)
                    {
                        modelNext.GDNgayDauTienThanhToanHet = "Y";
                    }
                    else if (modelNext != null && item.GDNgayDauTienThanhToanHet == "Y" && checkYN.TotalAmount != productinvoice.TotalAmount)
                    {
                        modelNext.GDNgayDauTienThanhToanHet = "N";
                    }
                    if (modelNext != null && item.GDNgayDauTienThanhToanHet == "Y" && monthSl > 3)
                    {
                        modelNext.GDNgayDauTienThanhToanHet = "N";
                    }
                }

                //Tim kiem don da chot
                //var itemdaco = BC_DOANHSO_NHANHANGRepository.GetBC_DOANHSO_NHANHANGByMaDH(item.Code, BranchId);

                if (itemdaco != null)
                {
                    //item.Status = "dachot";
                    //var Idx = productInvoiceRepository.GetvwProductInvoiceByCode(BranchId.Value, item.Code);
                    //item.Id = Idx.Id;
                    //item.Note = Idx.Note;

                    var checkDonchot = invoiceRepository.GetMMByOldId(item.Id);
                    if (checkDonchot != null)
                    {
                        var modelNext_1 = model.SingleOrDefault(x => x.Id == checkDonchot.ToProductInvoiceId);
                        var productinvoice_1 = invoiceRepository.GetProductInvoiceById(item.Id);
                        if (modelNext_1 != null && item.GDNgayDauTienThanhToanHet == "Y" && (item.CountForBrand == "ORLANE PARIS" || item.CountForBrand == "DICHVU" ||
                            item.CountForBrand == "CONGNGHECAO"))
                        {
                            if (modelNext_1.CountForBrand == "ORLANE PARIS" || modelNext_1.CountForBrand == "DICHVU" || modelNext_1.CountForBrand == "CONGNGHECAO")
                            {
                                modelNext_1.GDNgayDauTienThanhToanHet = "Y";
                            }
                        }

                        if (modelNext_1 != null && item.GDNgayDauTienThanhToanHet == "Y" && (item.CountForBrand == "ANNAYAKE" || item.CountForBrand == "LEONOR GREYL"))
                        {
                            if (modelNext_1.CountForBrand == "ORLANE PARIS" || modelNext_1.CountForBrand == "DICHVU" || modelNext_1.CountForBrand == "CONGNGHECAO")
                            {
                                modelNext_1.GDNgayDauTienThanhToanHet = "N";
                            }
                        }

                    }
                }

            }


            if (!string.IsNullOrEmpty(GDNDT))
            {
                ///txtCusName = Helpers.Common.ChuyenThanhKhongDau(GDNDT);
                model = model.Where(x => x.GDNgayDauTienThanhToanHet == GDNDT).ToList();
            }

            decimal minAmount;
            if (decimal.TryParse(txtMinAmount, out minAmount))
            {
                model = model.Where(x => x.TotalAmount >= minAmount).ToList();
            }

            decimal maxAmount;
            if (decimal.TryParse(txtMaxAmount, out maxAmount))
            {
                if (maxAmount > 0)
                {
                    model = model.Where(x => x.TotalAmount <= maxAmount).ToList();
                }
            }

            if (BranchId != null && BranchId.Value > 0)
            {
                model = model.Where(x => x.BranchId == BranchId).ToList();
            }

            //bo sung nhom nguoi dung - cong
            if (userTypeId != null && userTypeId.Value > 0)
            {
                model = model.Where(x => x.UserTypeId == userTypeId).ToList();
            }
            if (!string.IsNullOrEmpty(txtCustomerCode))
            {
                txtCustomerCode = Helpers.Common.ChuyenThanhKhongDau(txtCustomerCode);
                model = model.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerCode).Contains(txtCustomerCode)).ToList();
            }
            if (!string.IsNullOrEmpty(countForBrand))
            {
                if (countForBrand == "TongOrlane")
                {
                    model = model.Where(x => x.CountForBrand == "DICHVU" || x.CountForBrand == "CONGNGHECAO" || x.CountForBrand == "ORLANE PARIS").ToList();
                }
                else
                {
                    countForBrand = Helpers.Common.ChuyenThanhKhongDau(countForBrand);
                    model = model.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CountForBrand).Contains(countForBrand)).ToList();
                }
            }
            return model;
        }
        public ActionResult DoanhSoTheoNhanHang(int? Year, string txtCode, string txtMinAmount, string txtMaxAmount, string txtCusName, string startDate, string endDate, int? BranchId, string txtCustomerCode, string countForBrand, int? userTypeId, string GDNDT)
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
            var model = GETDonHangTheoNhanHang(Year, txtCode, txtMinAmount, txtMaxAmount, txtCusName, startDate, endDate, BranchId, txtCustomerCode, countForBrand, userTypeId, GDNDT);
            // model = model.OrderByDescending(c => c.CreatedDate).ToList();
            model = model.OrderByDescending(c => c.CustomerCode).ToList();
            var category = categoryRepository.GetAllCategories()
            .Select(item => new CategoryViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Name = item.Name
            }).Where(x => x.Code == "Origin").ToList();


            ViewBag.category = category;
            ViewBag.tongtien = model.Sum(x => x.TotalAmount);
            ViewBag.tienconno = model.Sum(x => x.TongConNo);
            ViewBag.tongthu = model.Sum(x => x.TotalDebit);
            ViewBag.dathanhtoan = model.Sum(x => x.TotalCredit);
            return View(model);
        }

        public ActionResult PrintDoanhSoTheoNhanHang(int? Year, string txtCode, string txtMinAmount, string txtMaxAmount, string txtCusName, string startDate, string endDate, int? BranchId, string txtCustomerCode, string countForBrand, int? userTypeId, string GDNDT, bool ExportExcel = false)
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
            var data = GETDonHangTheoNhanHang(Year, txtCode, txtMinAmount, txtMaxAmount, txtCusName, startDate, endDate, BranchId, txtCustomerCode, countForBrand, userTypeId, GDNDT);
            data = data.OrderByDescending(c => c.CustomerCode).ToList();
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDoanhSoTheoNhanHang(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Doanh số theo nhãn hàng");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "spSale_Doanhsotheonhanhang" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }




            return View(model);
        }
        string buildHtmlDoanhSoTheoNhanHang(List<ProductInvoiceViewModel> detailList)
        {
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>";
            detailLists += "		<th>STT</th>";
            //detailLists += "		<th>Trạng thái</th>";
            detailLists += "		<th>Ngày tạo</th>";
           // detailLists += "		<th>Người tạo</th>";
            detailLists += "		<th>Mã đơn hàng</th>";
            detailLists += "		<th>Tổng tiền</th>";
            detailLists += "		<th>Đã thanh toán</th>";
            detailLists += "		<th>Còn nợ</th>";
            detailLists += "		<th>Tỷ lệ hưởng</th>";
            detailLists += "		<th>Doanh số thực NVKD</th>";
            detailLists += "		<th>Khách hàng</th>";
            detailLists += "		<th>Mã khách hàng</th>";
            detailLists += "		<th>Nhân viên quản lý</th>";
            detailLists += "		<th>Nhóm quản lý</th>";
           // detailLists += "		<th>CT xuất kho</th>";
           // detailLists += "		<th>Chi nhánh</th>";
            detailLists += "		<th>Ngày kết toán</th>";
            //detailLists += "		<th>TT ghi sổ</th>";
            detailLists += "		<th>Nhãn hàng</th>";

            detailLists += "		<th>GD NĐT</th>";
            detailLists += "		<th>SP HH</th>";
            detailLists += "		<th>SP DV</th>";
            detailLists += "		<th>Hàng tặng</th>";
            detailLists += "		<th>Giảm giá VIP</th>";
            detailLists += "		<th>Giảm giá KM</th>";
            detailLists += "		<th>Giảm giá ĐB</th>";
            detailLists += "		<th>Ghi chú</th>";
            detailLists += "	</tr>";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in detailList)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
              //  + "<td class=\"text-left \">" + (item.Status == "complete" ? "Hoàn thành" : "Hủy chuyển") + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CreatedDate.Value.ToString("dd/MM/yyyy") + " </td>\r\n"
              //  + "<td class=\"text-left \">" + item.CreatedUserName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + CommonSatic.ToCurrencyStr(item.TotalAmount, null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-left \">" + CommonSatic.ToCurrencyStr(item.TotalCredit, null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-left \">" + CommonSatic.ToCurrencyStr(item.TongConNo, null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-left \">" + item.TyleHuong + "</td>\r\n"
                 + "<td class=\"text-left \">" + CommonSatic.ToCurrencyStr((decimal)(item.TyleHuong) * item.TotalAmount / 100, null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.UserTypeName + "</td>\r\n"
              //  + "<td class=\"text-left \">" + item.ProductInvoiceOldCode + "</td>\r\n"
             //   + "<td class=\"text-left \">" + item.BranchName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.VoucherDate.Value.ToString("dd/MM/yyyy") + "</td>\r\n"
             //   + "<td class=\"text-left \">" + (item.IsArchive == true ? "Đã ghi sổ" : "Chưa ghi sổ") + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CountForBrand + "</td>\r\n"
                //+ "<td class=\"text-left \">" + item.GDDauTienThanhToanHet + "</td>\r\n"
                 + "<td class=\"text-left \">" + item.GDNgayDauTienThanhToanHet + "</td>\r\n"
                + "<td class=\"text-left \">" + item.SPHangHoa + "</td>\r\n"
                + "<td class=\"text-left \">" + item.SPDichvu + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Hangduoctang + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Discount_VIP + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Discount_KM + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Discount_DB + "</td>\r\n"
                + "<td class=\"text-left \">" + item.GHI_CHU + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";
            return detailLists;
        }

        //Chốt sản phẩm
        [HttpPost]
        public ActionResult DaChot()
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

            try
            {
                string idDeleteAll = Request["DeleteId-checkbox"];
                //string gdall = Request["selectgd"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                //string[] gdallId = gdall.Split(',');
                var listCodeDaco = new List<string>();
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    for (int i = 0; i < arrDeleteId.Count(); i++)
                    {
                        //string gdall = Request["selectgd_"+ arrDeleteId[i]];
                        var item = productInvoiceRepository.GetvwProductInvoice_NVKDById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                        //Kiểm tra đơn hàng đã có
                        var itemdaco = BC_DOANHSO_NHANHANGRepository.GetBC_DOANHSO_NHANHANGByMaDH(item.Code, intBrandID);
                        if (item != null)
                        {
                            if (itemdaco == null)
                            {
                                var bc_nhanhang = new BC_DOANHSO_NHANHANG();
                                bc_nhanhang.BranchId = item.BranchId;
                                bc_nhanhang.CHI_NHANH = item.BranchName;

                                bc_nhanhang.THANG = item.Month;
                                bc_nhanhang.NAM = item.Year;
                                bc_nhanhang.TRANG_THAI = item.Status;
                                bc_nhanhang.NGAY_TAO = item.CreatedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                bc_nhanhang.MA_DONHANG = item.Code;
                                bc_nhanhang.TONG_TIEN = item.TotalAmount;
                                bc_nhanhang.TONG_THU = item.TotalDebit;
                                bc_nhanhang.DA_THANHTOAN = item.TotalCredit;
                                bc_nhanhang.CON_NO = item.tienconno;
                                bc_nhanhang.TEN_KH = item.CustomerName;
                                bc_nhanhang.MA_KH = item.CustomerCode;
                                bc_nhanhang.KHACHANG_ID = item.CustomerId;
                                bc_nhanhang.NHANVIEN_QLY = item.ManagerStaffName;
                                bc_nhanhang.CHI_NHANH = item.BranchName;
                                bc_nhanhang.NGAY_KETTOAN = item.ModifiedDate.Value.ToString("yyyy-MM-dd HH:mm:ss");
                                bc_nhanhang.TRANGTHAI_GHISO = item.IsArchive.ToString();
                                bc_nhanhang.GIAMGIA_VIP = item.Discount_VIP;

                                bc_nhanhang.GIAMGIA_KM = item.Discount_KM;
                                bc_nhanhang.GIAMGIA_DB = item.Discount_DB;
                                bc_nhanhang.TINH_CHO = item.CountForBrand;
                                bc_nhanhang.HANG_TANG = item.Hangduoctang;
                                bc_nhanhang.GD_DT = item.GDDauTienThanhToanHet;
                                bc_nhanhang.GD_NDT = item.GDNgayDauTienThanhToanHet;
                                bc_nhanhang.SP_HH = item.SPHangHoa;
                                bc_nhanhang.SP_DV = item.SPDichvu;
                                bc_nhanhang.TYLE_HUONG = item.TyleHuong;
                                bc_nhanhang.GHI_CHU = item.Note;
                                bc_nhanhang.IsDeleted = false;
                                bc_nhanhang.CreatedDate = DateTime.Now;
                                bc_nhanhang.CreatedUserId = WebSecurity.CurrentUserId;

                                bc_nhanhang.NHOM_QUANLY = item.UserTypeName;
                                bc_nhanhang.NHOM_QUANLY_ID = item.UserTypeId;


                                //Insert DB


                                BC_DOANHSO_NHANHANGRepository.InsertBC_DOANHSO_NHANHANG(bc_nhanhang);

                            }
                            else
                            {
                                listCodeDaco.Add(item.Code);
                            }
                        }


                    }
                    if (listCodeDaco.Count > 0)
                    {

                        ViewBag.ErrorMesseage = ("Đơn Hàng Đã Chốt: " + string.Join(", ", listCodeDaco));
                        return RedirectToAction("DoanhSoTheoNhanHang", new { Message = ViewBag.ErrorMesseage });


                    }
                    scope.Complete();
                }
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                return RedirectToAction("DoanhSoTheoNhanHang");
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("DoanhSoTheoNhanHang");
            }

        }

        #region TyleHoaHong
        public ViewResult IndexHoaHong()
        {
            var q = HOAHONG_NVKDRepository.GetAllHOAHONG_NVKD();
            var model = q.Select(item => new HOAHONG_NVKDViewModel
            {
                Id = item.Id,
                STT = item.STT,
                ModifiedDate = item.ModifiedDate,
                TYLE_TARGET = item.TYLE_TARGET,
                MAX_TARGET = item.MAX_TARGET,
                MIN_TARGET = item.MIN_TARGET,
                TYLE_HOAHONG = item.TYLE_HOAHONG


            }).ToList();
            return View(model);
        }

        public ViewResult CreateHoaHong()
        {
            var model = new HOAHONG_NVKDViewModel();

            var stt = HOAHONG_NVKDRepository.GetAllHOAHONG_NVKD().ToList();

            model.STT = stt.Count + 1;
            return View(model);
        }
        [HttpPost]
        public ActionResult CreateHoaHong(HOAHONG_NVKDViewModel model)
        {
            if (ModelState.IsValid)
            {
                var LoyaltyPoint = new HOAHONG_NVKD();
                AutoMapper.Mapper.Map(model, LoyaltyPoint);
                LoyaltyPoint.IsDeleted = false;
                LoyaltyPoint.CreatedUserId = WebSecurity.CurrentUserId;
                LoyaltyPoint.ModifiedUserId = WebSecurity.CurrentUserId;
                //.AssignedUserId = WebSecurity.CurrentUserId;
                LoyaltyPoint.CreatedDate = DateTime.Now;
                LoyaltyPoint.ModifiedDate = DateTime.Now;
                HOAHONG_NVKDRepository.InsertHOAHONG_NVKD(LoyaltyPoint);

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("IndexHoaHong");
            }
            return View(model);

        }

        public ActionResult EditHoaHong(int? Id)
        {
            var LoyaltyPoint = HOAHONG_NVKDRepository.GetHOAHONG_NVKDById(Id.Value);
            if (LoyaltyPoint != null && LoyaltyPoint.IsDeleted != true)
            {
                var model = new HOAHONG_NVKDViewModel();
                AutoMapper.Mapper.Map(LoyaltyPoint, model);

                if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                {
                    TempData["FailedMessage"] = "NotOwner";
                    return RedirectToAction("IndexHoaHong");
                }

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("IndexHoaHong");
        }

        [HttpPost]
        public ActionResult EditHoaHong(HOAHONG_NVKDViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var LoyaltyPoint = HOAHONG_NVKDRepository.GetHOAHONG_NVKDById(model.Id);
                    AutoMapper.Mapper.Map(model, LoyaltyPoint);
                    LoyaltyPoint.ModifiedUserId = WebSecurity.CurrentUserId;
                    LoyaltyPoint.ModifiedDate = DateTime.Now;
                    HOAHONG_NVKDRepository.UpdateHOAHONG_NVKD(LoyaltyPoint);

                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                    return RedirectToAction("IndexHoaHong");
                }



                return View(model);
            }

            return View(model);

            //if (Request.UrlReferrer != null)
            //    return Redirect(Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult DeleteHoaHong()
        {
            try
            {
                string id = Request["Delete"];
                if (id != null)
                {
                    var item = HOAHONG_NVKDRepository.GetHOAHONG_NVKDById(int.Parse(id, CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                        {
                            TempData["FailedMessage"] = "NotOwner";
                            return RedirectToAction("IndexHoaHong");
                        }

                        item.IsDeleted = true;
                        HOAHONG_NVKDRepository.UpdateHOAHONG_NVKD(item);
                    }
                }
                else
                {
                    string idDeleteAll = Request["DeleteId-checkbox"];
                    string[] arrDeleteId = idDeleteAll.Split(',');
                    for (int i = 0; i < arrDeleteId.Count(); i++)
                    {
                        var item = HOAHONG_NVKDRepository.GetHOAHONG_NVKDById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                        if (item != null)
                        {
                            if (item.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                            {
                                TempData["FailedMessage"] = "NotOwner";
                                return RedirectToAction("IndexHoaHong");
                            }

                            item.IsDeleted = true;
                            HOAHONG_NVKDRepository.UpdateHOAHONG_NVKD(item);
                        }
                    }
                }
               
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return RedirectToAction("IndexHoaHong");
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("IndexHoaHong");
            }
        }

        #endregion


        #endregion

        //----tiendev
        #region Sale_TotalCustomerOfDayReport
        public ActionResult Sale_TotalCustomerOfDayReport()
        {

            var category = categoryRepository.GetAllCategories()
            .Select(item => new CategoryViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Name = item.Name
            }).Where(x => x.Code == "Origin").ToList();
            ViewBag.category = category;
            return View();
        }

        public PartialViewResult _GetListSale_TotalCustomerOfDayReport(string Date, int? BranchId, string typeReport)
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

            #region tính toán các thông số
            var data = GETSale_TotalCustomerOfDayReport(Date, BranchId, typeReport);

            var getDate = (!string.IsNullOrEmpty(Date) ? DateTime.ParseExact(Date.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd") : "");

            // tổng số đơn chuyển trong ngày
            var TransferList = SqlHelper.QuerySP<CancelledTransferredInvoiceViewModel>("spGetAllMoneyMoveByStatus", new
            {
                Date = getDate,
                BranchId = BranchId
            }).ToList();

            //Lấy đơn hàng đc chuyển để tính doanh thu, doanh số

            //Lấy đơn bị chuyển mà đc mua trong ngày
            var transferinday = new List<CancelledTransferredInvoiceViewModel>();
            //
            if (TransferList.Count > 0)
            {
                foreach (var item in TransferList)
                {
                    var invoice = productInvoiceRepository.GetvwProductInvoiceById(item.ToProductInvoiceId);
                    if (invoice != null)
                    {
                        var frominvoice = productInvoiceRepository.GetvwProductInvoiceById(item.FromProductInvoiceId);
                        if (frominvoice != null && frominvoice.IsArchive == true)
                        {
                            item.CountForBrand = invoice.CountForBrand;
                            if (frominvoice.CreatedDate.Value.ToString("dd/MM/yyyy") == Date)
                            {
                                transferinday.Add(item);
                            }
                        }
                    }
                }
            }


            var phieuthu = new List<Domain.Account.Entities.vwReceipt>();
            //Lấy phiếu thu để tính doanh thu
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(Date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(Date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    phieuthu = ReceiptRepository.GetAllReceipt().Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                }
            }
            if (BranchId > 0)
            {
                phieuthu = phieuthu.Where(x => x.BranchId == BranchId).ToList();
            }

            List<ReceiptViewModel> doanhthu = phieuthu.Select(item => new ReceiptViewModel
            {
                Id = item.Id,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Name = item.Name,
                Code = item.Code,
                Amount = item.Amount,
                Address = item.Address,
                Note = item.Note,
                Payer = item.Payer,
                SalerId = item.SalerId,
                CompanyName = item.CompanyName,
                VoucherDate = item.VoucherDate,
                SalerName = item.SalerName,
                CustomerId = item.CustomerId,
                IsArchive = item.IsArchive,
                LoaiChungTuGoc = item.LoaiChungTuGoc,
                MaChungTuGoc = item.MaChungTuGoc,
                CustomerCode = item.CustomerCode,
                NVQL = item.NVQL,
                BranchId = item.BranchId
            }).OrderByDescending(x => x.CreatedDate).ToList();


            var ReceiptDif = new List<ReceiptViewModel>();
            var doanhthu2 = new List<ReceiptViewModel>();
            foreach (var i in doanhthu)
            {
                var pi = productInvoiceRepository.GetvwProductInvoiceByCode(i.BranchId.Value, i.MaChungTuGoc);
                if (pi != null)
                {
                    i.CountForBrand = pi.CountForBrand;
                    if (pi.CreatedDate.Value.ToString("dd/MM/yyyy") != i.CreatedDate.Value.ToString("dd/MM/yyyy"))
                    {
                        ReceiptDif.Add(i);
                    }
                    else
                    {
                        doanhthu2.Add(i);
                    }
                }

            }

            // int TongDonHangHuyTrongNgay = TransferList.Where(x => x.Status == "delete").Count();
            int TongDonHangHuyTrongNgay = TransferList.Count();

            //var listsumCountForBrand = productInvoiceRepository.GetAllProductInvoice().Where(x=>x.CreatedDate >=getdate).GroupBy(x => x.CountForBrand)
            //                                                                                         .Select(g => new
            //                                                                                         {
            //                                                                                             CountForBrand = g.Key,
            //                                                                                             TotalAmount = g.Select(gg => gg.TotalAmount).Sum()
            //                                                                                        });  

            // lấy đơn hàng theo nhãn
            var listProudctInvoice_CountForBrand = SqlHelper.QuerySP<Sale_TotalAmoutByBrandOfDayViewModel>("spSale_GroupProductInvoiceByBrand", new
            {
                Date = getDate,
                BranchId = BranchId
            });

            var oldtransfer = new List<CancelledTransferredInvoiceViewModel>();
            var newtransfer = new List<CancelledTransferredInvoiceViewModel>();
            var newReceipt = new List<ReceiptViewModel>();
            var oldReceipt = new List<ReceiptViewModel>();

            var oldtransferinday = new List<CancelledTransferredInvoiceViewModel>();
            var newtransferinday = new List<CancelledTransferredInvoiceViewModel>();

            //Tính ảo, thực mới theo nhãn
            foreach (var item1 in listProudctInvoice_CountForBrand)
            {


                foreach (var item2 in data.ListNewCustomer)
                {
                    foreach (var invoiceday in transferinday)
                    {
                        if (invoiceday.CustomerId == item2.CustomerId)
                        {
                            if (newtransfer.Count > 0)
                            {
                                var old = newtransfer.Where(x => x.Id == invoiceday.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    newtransferinday.Add(invoiceday);
                                }
                            }
                            else
                            {
                                newtransferinday.Add(invoiceday);
                            }
                        }
                    }
                    foreach (var invoice in TransferList)
                    {
                        if (invoice.CustomerId == item2.CustomerId)
                        {
                            if (newtransfer.Count > 0)
                            {
                                var old = newtransfer.Where(x => x.Id == invoice.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    newtransfer.Add(invoice);
                                }
                            }
                            else
                            {
                                newtransfer.Add(invoice);
                            }
                        }
                    }
                    foreach (var phieu in doanhthu2)
                    {
                        if (phieu.CustomerId == item2.CustomerId)
                        {
                            if (newReceipt.Count > 0)
                            {
                                var phieumoi = newReceipt.Where(x => x.Id == phieu.Id).FirstOrDefault();
                                if (phieumoi == null)
                                {
                                    newReceipt.Add(phieu);
                                }
                            }
                            else
                            {
                                newReceipt.Add(phieu);
                            }
                        }
                    }
                    // tính cho KH mới
                    if (item1.CustomerId == item2.CustomerId)
                    {
                        if (item1.CountForBrand == "ANNAYAKE")
                        {
                            data.TongAoMoiANNAYAKE += item1.TotalAmount.Value;
                            data.TongThucMoiANNAYAKE += item1.DoanhThu.Value;
                            data.countANNAYAKE_Moi++;
                        }
                        else if (item1.CountForBrand == "ORLANE PARIS")
                        {
                            data.TongAoMoiORLANEPARIS += item1.TotalAmount.Value;
                            data.TongThucMoiORLANEPARIS += item1.DoanhThu.Value;
                            data.countORLANEPARIS_Moi++;
                        }
                        else if (item1.CountForBrand == "DICHVU")
                        {
                            data.TongAoMoiDICHVU += item1.TotalAmount.Value;
                            data.TongThucMoiDICHVU += item1.DoanhThu.Value;
                            data.countDICHVU_Moi++;
                        }
                        else if (item1.CountForBrand == "LEONOR GREYL")
                        {
                            data.TongAoMoiLEONORGREYL += item1.TotalAmount.Value;
                            data.TongThucMoiLEONORGREYL += item1.DoanhThu.Value;
                            data.countLEONORGREYL_Moi++;
                        }
                        else
                        {
                            data.TongAoMoiCONGNGHECAO += item1.TotalAmount.Value;
                            data.TongThucMoiCONGNGHECAO += item1.DoanhThu.Value;
                            data.countCONGNGHECAO_Moi++;
                        }
                    }
                }
                // tính cho KH cũ
                foreach (var item2 in data.ListOldCustomer)
                {
                    foreach (var invoiceday in transferinday)
                    {
                        if (invoiceday.CustomerId == item2.CustomerId)
                        {
                            if (oldtransfer.Count > 0)
                            {
                                var old = oldtransfer.Where(x => x.Id == invoiceday.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    oldtransferinday.Add(invoiceday);
                                }
                            }
                            else
                            {
                                oldtransferinday.Add(invoiceday);
                            }
                        }
                    }


                    foreach (var invoice in TransferList)
                    {
                        if (invoice.CustomerId == item2.CustomerId)
                        {
                            if (oldtransfer.Count > 0)
                            {
                                var old = oldtransfer.Where(x => x.Id == invoice.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    oldtransfer.Add(invoice);
                                }
                            }
                            else
                            {
                                oldtransfer.Add(invoice);
                            }


                        }
                    }
                    foreach (var phieu in doanhthu2)
                    {
                        if (phieu.CustomerId == item2.CustomerId)
                        {
                            if (oldReceipt.Count > 0)
                            {
                                var phieumoi = oldReceipt.Where(x => x.Id == phieu.Id).FirstOrDefault();
                                if (phieumoi == null)
                                {
                                    oldReceipt.Add(phieu);
                                }
                            }
                            else
                            {
                                oldReceipt.Add(phieu);
                            }
                        }
                    }
                    if (item1.CustomerId == item2.CustomerId)
                    {
                        if (item1.CountForBrand == "ANNAYAKE")
                        {
                            data.TongAoCuANNAYAKE += item1.TotalAmount.Value;
                            data.TongThucCuANNAYAKE += item1.DoanhThu.Value;
                            data.countANNAYAKE_Cu++;
                        }
                        else if (item1.CountForBrand == "ORLANE PARIS")
                        {
                            data.TongAoCuORLANEPARIS += item1.TotalAmount.Value;
                            data.TongThucCuORLANEPARIS += item1.DoanhThu.Value;
                            data.countORLANEPARIS_Cu++;
                        }
                        else if (item1.CountForBrand == "DICHVU")
                        {
                            data.TongAoCuDICHVU += item1.TotalAmount.Value;
                            data.TongThucCuDICHVU += item1.DoanhThu.Value;
                            data.countDICHVU_Cu++;
                        }
                        else if (item1.CountForBrand == "LEONOR GREYL")
                        {
                            data.TongAoCuLEONORGREYL += item1.TotalAmount.Value;
                            data.TongThucCuLEONORGREYL += item1.DoanhThu.Value;
                            data.countLEONORGREYL_Cu++;
                        }
                        else
                        {
                            data.TongAoCuCONGNGHECAO += item1.TotalAmount.Value;
                            data.TongThucCuCONGNGHECAO += item1.DoanhThu.Value;
                            data.countCONGNGHECAO_Cu++;
                        }
                    }
                }
            }
            //
            var TongAoMoi = data.TongAoMoiANNAYAKE + data.TongAoMoiORLANEPARIS + data.TongAoMoiLEONORGREYL + data.TongAoMoiDICHVU + data.TongAoMoiCONGNGHECAO;
            var TongThucMoi = data.TongThucMoiANNAYAKE + data.TongThucMoiORLANEPARIS + data.TongThucMoiLEONORGREYL + data.TongThucMoiDICHVU + data.TongThucMoiCONGNGHECAO;
            var TongAoCu = data.TongAoCuANNAYAKE + data.TongAoCuORLANEPARIS + data.TongAoCuLEONORGREYL + data.TongAoCuDICHVU + data.TongAoCuCONGNGHECAO;
            var TongThucCu = data.TongThucCuANNAYAKE + data.TongThucCuORLANEPARIS + data.TongThucCuLEONORGREYL + data.TongThucCuDICHVU + data.TongThucCuCONGNGHECAO;
            //tinh lai doanh so
            //cu anna
            data.TongAoCuANNAYAKE = (decimal)(data.TongAoCuANNAYAKE - oldtransfer.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney));
            //cu leo
            data.TongAoCuLEONORGREYL = (decimal)(data.TongAoCuLEONORGREYL - oldtransfer.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney));
            //cu orlane
            data.TongAoCuORLANEPARIS = (decimal)(data.TongAoCuORLANEPARIS - oldtransfer.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney));
            //cu dichvu
            data.TongAoCuDICHVU = (decimal)(data.TongAoCuDICHVU - oldtransfer.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney));
            //cu cnc
            data.TongAoCuCONGNGHECAO = (decimal)(data.TongAoCuCONGNGHECAO - oldtransfer.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney));

            //moi anna
            data.TongAoMoiANNAYAKE = (decimal)(data.TongAoMoiANNAYAKE - newtransfer.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney));
            //moi leo
            data.TongAoMoiLEONORGREYL = (decimal)(data.TongAoMoiLEONORGREYL - newtransfer.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney));
            //moi orlane
            data.TongAoMoiORLANEPARIS = (decimal)(data.TongAoMoiORLANEPARIS - newtransfer.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney));
            //moi dichvu
            data.TongAoMoiDICHVU = (decimal)(data.TongAoMoiDICHVU - newtransfer.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney));
            //moi cnc
            data.TongAoMoiCONGNGHECAO = (decimal)(data.TongAoMoiCONGNGHECAO - newtransfer.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney));
            ///

            //Tinh lai doanh thu:
            data.TongThucCuANNAYAKE = (decimal)oldReceipt.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.Amount);
            data.TongThucCuLEONORGREYL = (decimal)oldReceipt.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.Amount);
            data.TongThucCuORLANEPARIS = (decimal)oldReceipt.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.Amount);
            data.TongThucCuDICHVU = (decimal)oldReceipt.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.Amount);
            data.TongThucCuCONGNGHECAO = (decimal)oldReceipt.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.Amount);


            data.TongThucMoiANNAYAKE = (decimal)newReceipt.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.Amount);
            data.TongThucMoiLEONORGREYL = (decimal)newReceipt.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.Amount);
            data.TongThucMoiORLANEPARIS = (decimal)newReceipt.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.Amount);
            data.TongThucMoiDICHVU = (decimal)newReceipt.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.Amount);
            data.TongThucMoiCONGNGHECAO = (decimal)newReceipt.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.Amount);
            //

            data.TongAoMoi += data.TongAoMoiANNAYAKE + data.TongAoMoiORLANEPARIS + data.TongAoMoiLEONORGREYL + data.TongAoMoiDICHVU + data.TongAoMoiCONGNGHECAO;
            data.TongThucMoi += data.TongThucMoiANNAYAKE + data.TongThucMoiORLANEPARIS + data.TongThucMoiLEONORGREYL + data.TongThucMoiDICHVU + data.TongThucMoiCONGNGHECAO;
            data.TongNoMoi = TongAoMoi - TongThucMoi;




            data.TongAoCu += data.TongAoCuANNAYAKE + data.TongAoCuORLANEPARIS + data.TongAoCuLEONORGREYL + data.TongAoCuDICHVU + data.TongAoCuCONGNGHECAO;
            data.TongThucCu += data.TongThucCuANNAYAKE + data.TongThucCuORLANEPARIS + data.TongThucCuLEONORGREYL + data.TongThucCuDICHVU + data.TongThucCuCONGNGHECAO;
            data.TongNoCu = TongAoCu - TongThucCu;


            data.TongDoanhSo = data.TongAoMoi + data.TongAoCu;
            data.TongDoanhThu = data.TongThucMoi + data.TongThucCu;
            data.TongNo = data.TongNoMoi + data.TongNoCu;

            #endregion
            //Tien thu no
            ViewBag.thuno = ReceiptDif.Sum(x => x.Amount);
            //phân loại theo mới/cũ
            ViewBag.TongAoCu = data.TongAoCu;
            ViewBag.TongAoMoi = data.TongAoMoi;
            ViewBag.TongThucCu = data.TongThucCu;
            ViewBag.TongThucMoi = data.TongThucMoi;
            ViewBag.TongNoCu = data.TongNoCu;
            ViewBag.TongNoMoi = data.TongNoMoi;
            //phân loại theo hãn, cũ ,mới
            ViewBag.TongAoCuANNAYAKE = data.TongAoCuANNAYAKE;
            ViewBag.TongAoCuLEONORGREYL = data.TongAoCuLEONORGREYL;
            ViewBag.TongAoCuORLANEPARIS = data.TongAoCuORLANEPARIS;
            ViewBag.TongAoCuCONGNGHECAO = data.TongAoCuCONGNGHECAO;
            ViewBag.TongAoCuDICHVU = data.TongAoCuDICHVU;

            ViewBag.TongAoMoiANNAYAKE = data.TongAoMoiANNAYAKE;
            ViewBag.TongAoMoiLEONORGREYL = data.TongAoMoiLEONORGREYL;
            ViewBag.TongAoMoiORLANEPARIS = data.TongAoMoiORLANEPARIS;
            ViewBag.TongAoMoiCONGNGHECAO = data.TongAoMoiCONGNGHECAO;
            ViewBag.TongAoMoiDICHVU = data.TongAoMoiDICHVU;

            ViewBag.TongThucCuANNAYAKE = data.TongThucCuANNAYAKE;
            ViewBag.TongThucCuLEONORGREYL = data.TongThucCuLEONORGREYL;
            ViewBag.TongThucCuORLANEPARIS = data.TongThucCuORLANEPARIS;
            ViewBag.TongThucCuCONGNGHECAO = data.TongThucCuCONGNGHECAO;
            ViewBag.TongThucCuDICHVU = data.TongThucCuDICHVU;

            ViewBag.TongThucMoiANNAYAKE = data.TongThucMoiANNAYAKE;
            ViewBag.TongThucMoiLEONORGREYL = data.TongThucMoiLEONORGREYL;
            ViewBag.TongThucMoiORLANEPARIS = data.TongThucMoiORLANEPARIS;
            ViewBag.TongThucMoiCONGNGHECAO = data.TongThucMoiCONGNGHECAO;
            ViewBag.TongThucMoiDICHVU = data.TongThucMoiDICHVU;
            //số lượng đơn hàng
            ViewBag.countANNAYAKE_Cu = data.countANNAYAKE_Cu;
            ViewBag.countORLANEPARIS_Cu = data.countORLANEPARIS_Cu;
            ViewBag.countDICHVU_Cu = data.countDICHVU_Cu;
            ViewBag.countLEONORGREYL_Cu = data.countLEONORGREYL_Cu;
            ViewBag.countCONGNGHECAO_Cu = data.countCONGNGHECAO_Cu;

            ViewBag.countANNAYAKE_Moi = data.countANNAYAKE_Moi;
            ViewBag.countORLANEPARIS_Moi = data.countORLANEPARIS_Moi;
            ViewBag.countDICHVU_Moi = data.countDICHVU_Moi;
            ViewBag.countLEONORGREYL_Moi = data.countLEONORGREYL_Moi;
            ViewBag.countCONGNGHECAO_Moi = data.countCONGNGHECAO_Moi;
            //tổng
            ViewBag.TongDoanhThu = data.TongDoanhThu;
            ViewBag.TongDoanhSo = data.TongDoanhSo;
            ViewBag.TongNo = data.TongNo;
            ViewBag.typeReport = typeReport;
            ViewBag.TongDonHangHuyTrongNgay = TongDonHangHuyTrongNgay;
            return PartialView(data);
        }

        Sale_TotalCustomerOfDayViewModel GETSale_TotalCustomerOfDayReport(string Date, int? BranchId, string typeReport)
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

            var getDate = (!string.IsNullOrEmpty(Date) ? DateTime.ParseExact(Date.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
            if (!string.IsNullOrEmpty(typeReport) && typeReport == "SPDV")
            {
                var listOldCustomer = SqlHelper.QuerySP<Sale_TotalCustomerOfDayViewModel>("spSale_TotalCustomerOfDay", new
                {
                    Date = getDate,
                    BranchId = BranchId,
                    loaiKH = 0
                }).ToList();

                var listNewCustomer = SqlHelper.QuerySP<Sale_TotalCustomerOfDayViewModel>("spSale_TotalCustomerOfDay", new
                {
                    Date = getDate,
                    BranchId = BranchId,
                    loaiKH = 1
                }).ToList();

                Sale_TotalCustomerOfDayViewModel data = new Sale_TotalCustomerOfDayViewModel();
                data.ListOldCustomer = listOldCustomer;
                data.totalOldCustomer = listOldCustomer.Count;
                data.ListNewCustomer = listNewCustomer;
                data.totalNewCustomer = listNewCustomer.Count;
                return data;
            }
            else
            {
                var listOldCustomer = SqlHelper.QuerySP<Sale_TotalCustomerOfDayViewModel>("spGet_DailyCustomerUsingService", new
                {
                    Date = getDate,
                    BranchId = BranchId,
                    loaiKH = 0
                }).ToList();

                var listNewCustomer = SqlHelper.QuerySP<Sale_TotalCustomerOfDayViewModel>("spGet_DailyCustomerUsingService", new
                {
                    Date = getDate,
                    BranchId = BranchId,
                    loaiKH = 1
                }).ToList();

                Sale_TotalCustomerOfDayViewModel data = new Sale_TotalCustomerOfDayViewModel();
                data.ListOldCustomer = listOldCustomer;
                data.totalOldCustomer = listOldCustomer.Count;
                data.ListNewCustomer = listNewCustomer;
                data.totalNewCustomer = listNewCustomer.Count;
                return data;
            }
        }

        public ActionResult PrintSale_TotalCustomerOfDayReport(string Date, int? BranchId, bool IsDetail, string typeReport, bool ExportExcel = false)
        {
            var data = GETSale_TotalCustomerOfDayReport(Date, BranchId, typeReport);
            var getDate = (!string.IsNullOrEmpty(Date) ? DateTime.ParseExact(Date.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd") : "");

            #region tính toán các thông số

            //var listsumCountForBrand = productInvoiceRepository.GetAllProductInvoice().Where(x=>x.CreatedDate >=getdate).GroupBy(x => x.CountForBrand)
            //                                                                                         .Select(g => new
            //                                                                                         {
            //                                                                                             CountForBrand = g.Key,
            //                                                                                             TotalAmount = g.Select(gg => gg.TotalAmount).Sum()
            //                                                                                        });
            // tổng số đơn chuyển trong ngày
            var TransferList = SqlHelper.QuerySP<CancelledTransferredInvoiceViewModel>("spGetAllMoneyMoveByStatus", new
            {
                Date = getDate,
                BranchId = BranchId
            }).ToList();

            //Lấy đơn hàng đc chuyển để tính doanh thu, doanh số

            //Lấy đơn bị chuyển mà đc mua trong ngày
            var transferinday = new List<CancelledTransferredInvoiceViewModel>();
            //

            if (TransferList.Count > 0)
            {
                foreach (var item in TransferList)
                {
                    var invoice = productInvoiceRepository.GetvwProductInvoiceById(item.ToProductInvoiceId);
                    if (invoice != null)
                    {
                        var frominvoice = productInvoiceRepository.GetvwProductInvoiceById(item.FromProductInvoiceId);
                        if (frominvoice != null && frominvoice.IsArchive == true)
                        {
                            item.CountForBrand = invoice.CountForBrand;
                            if (frominvoice.CreatedDate.Value.ToString("dd/MM/yyyy") == Date)
                            {
                                transferinday.Add(item);
                            }
                        }
                    }
                }
            }


            var phieuthu = new List<Domain.Account.Entities.vwReceipt>();
            //Lấy phiếu thu để tính doanh thu
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(Date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(Date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    phieuthu = ReceiptRepository.GetAllReceipt().Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                }
            }

            List<ReceiptViewModel> doanhthu = phieuthu.Select(item => new ReceiptViewModel
            {
                Id = item.Id,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Name = item.Name,
                Code = item.Code,
                Amount = item.Amount,
                Address = item.Address,
                Note = item.Note,
                Payer = item.Payer,
                SalerId = item.SalerId,
                CompanyName = item.CompanyName,
                VoucherDate = item.VoucherDate,
                SalerName = item.SalerName,
                CustomerId = item.CustomerId,
                IsArchive = item.IsArchive,
                LoaiChungTuGoc = item.LoaiChungTuGoc,
                MaChungTuGoc = item.MaChungTuGoc,
                CustomerCode = item.CustomerCode,
                NVQL = item.NVQL,
                BranchId = item.BranchId
            }).OrderByDescending(x => x.CreatedDate).ToList();


            var ReceiptDif = new List<ReceiptViewModel>();
            var doanhthu2 = new List<ReceiptViewModel>();
            foreach (var i in doanhthu)
            {
                var pi = productInvoiceRepository.GetvwProductInvoiceByCode(i.BranchId.Value, i.MaChungTuGoc);
                if (pi != null)
                {
                    i.CountForBrand = pi.CountForBrand;
                    if (pi.CreatedDate.Value.ToString("dd/MM/yyyy") != i.CreatedDate.Value.ToString("dd/MM/yyyy"))
                    {
                        ReceiptDif.Add(i);
                    }
                    else
                    {
                        doanhthu2.Add(i);
                    }
                }

            }

            //Tính ảo, thực mới theo nhãn
            //var getDate = (!string.IsNullOrEmpty(Date) ? DateTime.ParseExact(Date.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd") : "");
            var listProudctInvoice_CountForBrand = SqlHelper.QuerySP<Sale_TotalAmoutByBrandOfDayViewModel>("spSale_GroupProductInvoiceByBrand", new
            {
                Date = getDate,
                BranchId = BranchId,
            });

            var oldtransfer = new List<CancelledTransferredInvoiceViewModel>();
            var newtransfer = new List<CancelledTransferredInvoiceViewModel>();
            var newReceipt = new List<ReceiptViewModel>();
            var oldReceipt = new List<ReceiptViewModel>();

            var oldtransferinday = new List<CancelledTransferredInvoiceViewModel>();
            var newtransferinday = new List<CancelledTransferredInvoiceViewModel>();


            foreach (var item1 in listProudctInvoice_CountForBrand)
            {
                foreach (var item2 in data.ListNewCustomer)
                {

                    foreach (var invoiceday in transferinday)
                    {
                        if (invoiceday.CustomerId == item2.CustomerId)
                        {
                            if (newtransfer.Count > 0)
                            {
                                var old = newtransfer.Where(x => x.Id == invoiceday.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    newtransferinday.Add(invoiceday);
                                }
                            }
                            else
                            {
                                newtransferinday.Add(invoiceday);
                            }
                        }
                    }

                    foreach (var invoice in TransferList)
                    {
                        if (invoice.CustomerId == item2.CustomerId)
                        {
                            if (newtransfer.Count > 0)
                            {
                                var old = newtransfer.Where(x => x.Id == invoice.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    newtransfer.Add(invoice);
                                }
                            }
                            else
                            {
                                newtransfer.Add(invoice);
                            }
                        }
                    }
                    foreach (var phieu in doanhthu2)
                    {
                        if (phieu.CustomerId == item2.CustomerId)
                        {
                            if (newReceipt.Count > 0)
                            {
                                var phieumoi = newReceipt.Where(x => x.Id == phieu.Id).FirstOrDefault();
                                if (phieumoi == null)
                                {
                                    newReceipt.Add(phieu);
                                }
                            }
                            else
                            {
                                newReceipt.Add(phieu);
                            }
                        }
                    }

                    // tính cho KH mới
                    if (item1.CustomerId == item2.CustomerId)
                    {
                        if (item1.CountForBrand == "ANNAYAKE")
                        {
                            data.TongAoMoiANNAYAKE += item1.TotalAmount.Value;
                            data.TongThucMoiANNAYAKE += item1.PaidAmount.Value;
                            data.countANNAYAKE_Moi++;
                        }
                        else if (item1.CountForBrand == "ORLANE PARIS")
                        {
                            data.TongAoMoiORLANEPARIS += item1.TotalAmount.Value;
                            data.TongThucMoiORLANEPARIS += item1.PaidAmount.Value;
                            data.countORLANEPARIS_Moi++;
                        }
                        else if (item1.CountForBrand == "DICHVU")
                        {
                            data.TongAoMoiDICHVU += item1.TotalAmount.Value;
                            data.TongThucMoiDICHVU += item1.PaidAmount.Value;
                            data.countDICHVU_Moi++;
                        }
                        else if (item1.CountForBrand == "LEONOR GREYL")
                        {
                            data.TongAoMoiLEONORGREYL += item1.TotalAmount.Value;
                            data.TongThucMoiLEONORGREYL += item1.PaidAmount.Value;
                            data.countLEONORGREYL_Moi++;
                        }
                        else
                        {
                            data.TongAoMoiCONGNGHECAO += item1.TotalAmount.Value;
                            data.TongThucMoiCONGNGHECAO += item1.PaidAmount.Value;
                            data.countCONGNGHECAO_Moi++;
                        }
                    }
                }
                // tính cho KH cũ
                foreach (var item2 in data.ListOldCustomer)
                {

                    foreach (var invoiceday in transferinday)
                    {
                        if (invoiceday.CustomerId == item2.CustomerId)
                        {
                            if (oldtransfer.Count > 0)
                            {
                                var old = oldtransfer.Where(x => x.Id == invoiceday.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    oldtransferinday.Add(invoiceday);
                                }
                            }
                            else
                            {
                                oldtransferinday.Add(invoiceday);
                            }
                        }
                    }

                    foreach (var invoice in TransferList)
                    {
                        if (invoice.CustomerId == item2.CustomerId)
                        {
                            if (oldtransfer.Count > 0)
                            {
                                var old = oldtransfer.Where(x => x.Id == invoice.Id).FirstOrDefault();
                                if (old == null)
                                {
                                    oldtransfer.Add(invoice);
                                }
                            }
                            else
                            {
                                oldtransfer.Add(invoice);
                            }


                        }
                    }
                    foreach (var phieu in doanhthu2)
                    {
                        if (phieu.CustomerId == item2.CustomerId)
                        {
                            if (oldReceipt.Count > 0)
                            {
                                var phieumoi = oldReceipt.Where(x => x.Id == phieu.Id).FirstOrDefault();
                                if (phieumoi == null)
                                {
                                    oldReceipt.Add(phieu);
                                }
                            }
                            else
                            {
                                oldReceipt.Add(phieu);
                            }
                        }
                    }

                    if (item1.CustomerId == item2.CustomerId)
                    {
                        if (item1.CountForBrand == "ANNAYAKE")
                        {
                            data.TongAoCuANNAYAKE += item1.TotalAmount.Value;
                            data.TongThucCuANNAYAKE += item1.PaidAmount.Value;
                            data.countANNAYAKE_Cu++;
                        }
                        else if (item1.CountForBrand == "ORLANE PARIS")
                        {
                            data.TongAoCuORLANEPARIS += item1.TotalAmount.Value;
                            data.TongThucCuORLANEPARIS += item1.PaidAmount.Value;
                            data.countORLANEPARIS_Cu++;
                        }
                        else if (item1.CountForBrand == "DICHVU")
                        {
                            data.TongAoCuDICHVU += item1.TotalAmount.Value;
                            data.TongThucCuDICHVU += item1.PaidAmount.Value;
                            data.countDICHVU_Cu++;
                        }
                        else if (item1.CountForBrand == "LEONOR GREYL")
                        {
                            data.TongAoCuLEONORGREYL += item1.TotalAmount.Value;
                            data.TongThucCuLEONORGREYL += item1.PaidAmount.Value;
                            data.countLEONORGREYL_Cu++;
                        }
                        else
                        {
                            data.TongAoCuCONGNGHECAO += item1.TotalAmount.Value;
                            data.TongThucCuCONGNGHECAO += item1.PaidAmount.Value;
                            data.countCONGNGHECAO_Cu++;
                        }
                    }
                }
            }

            var TongAoMoi = data.TongAoMoiANNAYAKE + data.TongAoMoiORLANEPARIS + data.TongAoMoiLEONORGREYL + data.TongAoMoiDICHVU + data.TongAoMoiCONGNGHECAO;
            var TongThucMoi = data.TongThucMoiANNAYAKE + data.TongThucMoiORLANEPARIS + data.TongThucMoiLEONORGREYL + data.TongThucMoiDICHVU + data.TongThucMoiCONGNGHECAO;
            var TongAoCu = data.TongAoCuANNAYAKE + data.TongAoCuORLANEPARIS + data.TongAoCuLEONORGREYL + data.TongAoCuDICHVU + data.TongAoCuCONGNGHECAO;
            var TongThucCu = data.TongThucCuANNAYAKE + data.TongThucCuORLANEPARIS + data.TongThucCuLEONORGREYL + data.TongThucCuDICHVU + data.TongThucCuCONGNGHECAO;

            //tinh lai doanh so
            //cu anna
            data.TongAoCuANNAYAKE = (decimal)(data.TongAoCuANNAYAKE - oldtransfer.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney));
            //cu leo
            data.TongAoCuLEONORGREYL = (decimal)(data.TongAoCuLEONORGREYL - oldtransfer.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney));
            //cu orlane
            data.TongAoCuORLANEPARIS = (decimal)(data.TongAoCuORLANEPARIS - oldtransfer.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney));
            //cu dichvu
            data.TongAoCuDICHVU = (decimal)(data.TongAoCuDICHVU - oldtransfer.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney));
            //cu cnc
            data.TongAoCuCONGNGHECAO = (decimal)(data.TongAoCuCONGNGHECAO - oldtransfer.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney) + oldtransferinday.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney));

            //moi anna
            data.TongAoMoiANNAYAKE = (decimal)(data.TongAoMoiANNAYAKE - newtransfer.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.TransferredMoney));
            //moi leo
            data.TongAoMoiLEONORGREYL = (decimal)(data.TongAoMoiLEONORGREYL - newtransfer.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.TransferredMoney));
            //moi orlane
            data.TongAoMoiORLANEPARIS = (decimal)(data.TongAoMoiORLANEPARIS - newtransfer.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.TransferredMoney));
            //moi dichvu
            data.TongAoMoiDICHVU = (decimal)(data.TongAoMoiDICHVU - newtransfer.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.TransferredMoney));
            //moi cnc
            data.TongAoMoiCONGNGHECAO = (decimal)(data.TongAoMoiCONGNGHECAO - newtransfer.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney) + newtransferinday.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.TransferredMoney));
            ///

            //Tinh lai doanh thu:
            data.TongThucCuANNAYAKE = (decimal)oldReceipt.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.Amount);
            data.TongThucCuLEONORGREYL = (decimal)oldReceipt.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.Amount);
            data.TongThucCuORLANEPARIS = (decimal)oldReceipt.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.Amount);
            data.TongThucCuDICHVU = (decimal)oldReceipt.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.Amount);
            data.TongThucCuCONGNGHECAO = (decimal)oldReceipt.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.Amount);


            data.TongThucMoiANNAYAKE = (decimal)newReceipt.Where(x => x.CountForBrand == "ANNAYAKE").Sum(x => x.Amount);
            data.TongThucMoiLEONORGREYL = (decimal)newReceipt.Where(x => x.CountForBrand == "LEONOR GREYL").Sum(x => x.Amount);
            data.TongThucMoiORLANEPARIS = (decimal)newReceipt.Where(x => x.CountForBrand == "ORLANE PARIS").Sum(x => x.Amount);
            data.TongThucMoiDICHVU = (decimal)newReceipt.Where(x => x.CountForBrand == "DICHVU").Sum(x => x.Amount);
            data.TongThucMoiCONGNGHECAO = (decimal)newReceipt.Where(x => x.CountForBrand == "CONGNGHECAO").Sum(x => x.Amount);
            //


            data.TongAoMoi += data.TongAoMoiANNAYAKE + data.TongAoMoiORLANEPARIS + data.TongAoMoiLEONORGREYL + data.TongAoMoiDICHVU + data.TongAoMoiCONGNGHECAO;
            data.TongThucMoi += data.TongThucMoiANNAYAKE + data.TongThucMoiORLANEPARIS + data.TongThucMoiLEONORGREYL + data.TongThucMoiDICHVU + data.TongThucMoiCONGNGHECAO;
            data.TongNoMoi = TongAoMoi - TongThucMoi;




            data.TongAoCu += data.TongAoCuANNAYAKE + data.TongAoCuORLANEPARIS + data.TongAoCuLEONORGREYL + data.TongAoCuDICHVU + data.TongAoCuCONGNGHECAO;
            data.TongThucCu += data.TongThucCuANNAYAKE + data.TongThucCuORLANEPARIS + data.TongThucCuLEONORGREYL + data.TongThucCuDICHVU + data.TongThucCuCONGNGHECAO;
            data.TongNoCu = TongAoCu - TongThucCu;



            data.TongDoanhSo = data.TongAoMoi + data.TongAoCu;
            data.TongDoanhThu = data.TongThucMoi + data.TongThucCu;
            data.TongNo = data.TongNoMoi + data.TongNoCu;
            data.TienThuNo = (decimal)ReceiptDif.Sum(x => x.Amount);

            #endregion
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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSale_TotalCustomerOfDayReport(data, IsDetail));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Báo cáo tổng số khách hàng trong ngày");
            if (ExportExcel)
            {
                if (typeReport == "SPDV")
                    Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoTongSoKH_MuaSPDV_TrongNgay" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                else
                    Response.AppendHeader("content-disposition", "attachment;filename=" + "BaoCaoTongSoKH_CSD_TrongNgay" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.Flush();
                Response.Close();


                Response.End();
            }
            return View(model);
        }
        string buildHtmlSale_TotalCustomerOfDayReport(Sale_TotalCustomerOfDayViewModel detailList, bool IsDetail)
        {
            if (!IsDetail)
            {
                string detailLists = "<table class=\"invoice-detail\">\r\n";
                detailLists += "<thead>\r\n";
                detailLists += "	<tr>";
                detailLists += "		<th>STT</th>";
                detailLists += "		<th>Loại Khách hàng</th>";
                detailLists += "		<th>Số lượng</th>";
                detailLists += "	</tr>";
                detailLists += "</thead>\r\n";
                detailLists += "<tbody>\r\n";

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + 1 + "</td>\r\n"
                + "<td class=\"text-left \">" + "Khách hàng cũ" + "</td>\r\n"
                + "<td class=\"text-left \">" + detailList.totalOldCustomer + "</td>\r\n"
                + "</tr>\r\n";

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + 2 + "</td>\r\n"
                + "<td class=\"text-left \">" + "khách hàng mới" + "</td>\r\n"
                + "<td class=\"text-left \">" + detailList.totalNewCustomer + "</td>\r\n"
                + "</tr>\r\n";
                detailLists += "</tbody>\r\n";
                detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Doanh số: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongDoanhSo, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Doanh Thu: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongDoanhThu, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Tổng nợ: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongNo, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Thu nợ: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TienThuNo, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Doanh thu thực tế: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr((detailList.TienThuNo + detailList.TongDoanhThu), null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "</tfoot>\r\n</table>\r\n";
                return detailLists;
            }
            else
            {

                //danh sách khách hàng mới
                string detailLists = "<table class=\"invoice-detail\">\r\n";
                detailLists += "<thead>\r\n";
                detailLists += "	<tr>";
                detailLists += "		<th colspan=\"4\">KHÁCH HÀNG MỚI</th>";
                detailLists += "	</tr>";
                detailLists += "</thead>\r\n";
                detailLists += "<tbody>\r\n";
                detailLists += "	<tr>";
                detailLists += "		<th>STT</th>";
                detailLists += "		<th>Mã khách hàng</th>";
                detailLists += "		<th>Tên khách hàng</th>";
                detailLists += "		<th>Phone</th>";
                detailLists += "	</tr>";
                for (int i = 0; i < detailList.ListNewCustomer.Count; i++)
                {
                    detailLists += "<tr>\r\n"
                   + "<td class=\"text-center \">" + (i + 1) + "</td>\r\n"
                   + "<td class=\"text-left \">" + detailList.ListNewCustomer[i].CustomerId + "</td>\r\n"
                   + "<td class=\"text-left \">" + detailList.ListNewCustomer[i].CustomerName + "</td>\r\n"
                   + "<td class=\"text-left \">" + detailList.ListNewCustomer[i].CustomerPhone + "</td>\r\n"
                   + "</tr>\r\n";
                }
                // danh sách khách mới
                detailLists += "</tbody>\r\n";
                detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
                detailLists += "<td colspan=\"4\" class=\"text-right\" > Tổng khách mới: " + detailList.totalNewCustomer + "</td>\r\n";
                detailLists += "	<tr>";
                detailLists += "<td colspan=\"4\" class=\"text-center\"> ẢO MỚI </td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo mới ANNAYAKE: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoMoiANNAYAKE, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo mới ORLANEPARIS: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoMoiORLANEPARIS, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo mới DICHVU: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoMoiDICHVU, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo mới LEONORGREYL: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoMoiLEONORGREYL, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo mới CONGNGHECAO: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoMoiCONGNGHECAO, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td colspan=\"4\" class=\"text-center\"> THỰC MỚI </td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực mới ANNAYAKE: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucMoiANNAYAKE, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực mới ORLANEPARIS: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucMoiORLANEPARIS, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực mới DICHVU: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucMoiDICHVU, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực mới LEONORGREYL: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucMoiLEONORGREYL, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực mới CONGNGHECAO: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucMoiCONGNGHECAO, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Tổng Thực mới: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucMoi, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td colspan=\"4\" class=\"text-center\"> NỢ MỚI</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Nợ Mới: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongNoMoi, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";

                detailLists += "</tfoot>\r\n</table>\r\n";


                //danh sách khách hàng cũ
                detailLists += "\n";
                detailLists += "<table class=\"invoice-detail\">\r\n";
                detailLists += "<thead>\r\n";
                detailLists += "	<tr>";
                detailLists += "		<th colspan=\"4\">KHÁCH HÀNG CŨ</th>";
                detailLists += "	</tr>";
                detailLists += "</thead>\r\n";
                detailLists += "<tbody>\r\n";
                detailLists += "	<tr>";
                detailLists += "		<th>STT</th>";
                detailLists += "		<th>Mã khách hàng</th>";
                detailLists += "		<th>Tên khách hàng</th>";
                detailLists += "		<th>Phone</th>";
                detailLists += "	</tr>";
                for (int i = 0; i < detailList.ListOldCustomer.Count; i++)
                {
                    detailLists += "<tr>\r\n"
                   + "<td class=\"text-center \">" + (i + 1) + "</td>\r\n"
                   + "<td class=\"text-left \">" + detailList.ListOldCustomer[i].CustomerId + "</td>\r\n"
                   + "<td class=\"text-left \">" + detailList.ListOldCustomer[i].CustomerName + "</td>\r\n"
                   + "<td class=\"text-left \">" + detailList.ListOldCustomer[i].CustomerPhone + "</td>\r\n"
                   + "</tr>\r\n";
                }

                detailLists += "</tbody>\r\n";
                detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
                detailLists += "	<tr>";
                detailLists += "<td colspan=\"4\" class=\"text-right\" > Tổng khách cũ: " + detailList.totalOldCustomer + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td colspan=\"4\" class=\"text-center\"> ẢO CŨ </td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo cũ ANNAYAKE: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoCuANNAYAKE, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo cũ ORLANEPARIS: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoCuORLANEPARIS, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo cũ DICHVU: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoCuDICHVU, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo cũ LEONORGREYL: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoCuLEONORGREYL, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Ảo cũ CONGNGHECAO: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongAoCuCONGNGHECAO, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td colspan=\"4\" class=\"text-center\"> THỰC CŨ </td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực cũ ANNAYAKE: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucCuANNAYAKE, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực cũ ORLANEPARIS: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucCuORLANEPARIS, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực cũ DICHVU: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucCuDICHVU, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực cũ LEONORGREYL: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucCuLEONORGREYL, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Thực cũ CONGNGHECAO: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucCuCONGNGHECAO, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Tổng thực cũ: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongThucCu, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td colspan=\"4\" class=\"text-center\"> NỢ CŨ</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Nợ cũ: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongNoCu, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "<td colspan=\"4\" class=\"text-center\"> TỔNG KẾT </td>\r\n";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Doanh số: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongDoanhSo, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Doanh Thu: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongDoanhThu, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Tổng nợ: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TongNo, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";

                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\"> Thu nợ: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr(detailList.TienThuNo, null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                detailLists += "	<tr>";
                detailLists += "<td class=\"text-left\">Doanh thu thực tế: </td> <td colspan =\"3\" class=\"text-center\">" + CommonSatic.ToCurrencyStr((detailList.TienThuNo + detailList.TongDoanhThu), null).Replace(".", ",") + "</td>\r\n";
                detailLists += "	</tr>";
                //detailLists += "	<tr>";
                //detailLists += "<td class=\"text-left\"> Tổng hủy chuyển: </td> <td colspan =\"3\" class=\"text-center\">" + detailList.totalOldCustomer + "</td>\r\n";
                //detailLists += "	</tr>";
                detailLists += "</tfoot>\r\n</table>\r\n";

                return detailLists;
            }
        }

        public ActionResult Sale_DetailCustomerOfDay(string Date, int? BranchId, int typeCustomer, string typeReport)
        {
            var data = GETSale_TotalCustomerOfDayReport(Date, BranchId, typeReport);
            ViewBag.GetDate = Date;
            ViewBag.typeReport = typeReport;
            if (typeCustomer == 1)
            {
                return View(data.ListOldCustomer);
            }
            else
            {
                return View(data.ListNewCustomer);
            }
        }

        public ActionResult GetDetailListProductInvoice(int Id, string Date)
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

            var getDate = !string.IsNullOrEmpty(Date) ? DateTime.ParseExact(Date.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            var BranchId = intBrandID;
            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_GetProductInvoiceOfCustomerByUserId", new
            {
                StartDate = getDate,
                EndDate = getDate,
                BranchId = BranchId,
                UserId = Id
            }).ToList();
            return View(model);
        }
        public ActionResult GetDetailListService(int Id, string Date)
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
            var getDate = !string.IsNullOrEmpty(Date) ? DateTime.ParseExact(Date.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd") : DateTime.Now.ToString("yyyy-MM-dd");
            var BranchId = intBrandID;
            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_GetProductInvoiceOfCustomerByUserId", new
            {
                StartDate = getDate,
                EndDate = getDate,
                BranchId = BranchId,
                UserId = Id
            }).ToList();
            return View(model);
        }

        public ActionResult GetDaily_Cancelled_Tranferred_ProductInvoice(string Date)
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

            var getDate = (!string.IsNullOrEmpty(Date) ? DateTime.ParseExact(Date.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd") : "");
            var BranchId = intBrandID;
            var model = SqlHelper.QuerySP<CancelledTransferredInvoiceViewModel>("spGetAllMoneyMoveByStatus", new
            {
                Date = getDate,
                BranchId = BranchId
            }).ToList();
            ViewBag.GetDate = getDate;
            return View(model);
        }

        public ActionResult GetListThuNo(string Date)
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

            var productinvoice = new List<vwProductInvoice>();

            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(Date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(Date, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    productinvoice = productInvoiceRepository.GetAllvwProductInvoice().Where(x => x.ModifiedDate >= d_startDate && x.ModifiedDate <= d_endDate && x.CreatedDate < d_startDate).ToList();
                }
            }
            if (intBrandID > 0)
            {
                productinvoice = productinvoice.Where(x => x.BranchId == intBrandID).ToList();
            }

            var model = productinvoice.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ShipCityName = item.ShipCityName,
                TotalAmount = item.TotalAmount,

                TaxFee = item.TaxFee,
                CodeInvoiceRed = item.CodeInvoiceRed,
                Status = item.Status,
                IsArchive = item.IsArchive,

                PaidAmount = item.PaidAmount,
                RemainingAmount = item.RemainingAmount,
                Note = item.Note,
                CancelReason = item.CancelReason,
                BranchId = item.BranchId,
                CustomerId = item.CustomerId,
                BranchName = item.BranchName,
                ManagerStaffId = item.ManagerStaffId,
                ManagerStaffName = item.ManagerStaffName,
                ManagerUserName = item.ManagerUserName,
                CountForBrand = item.CountForBrand,
                TotalDebit = item.TotalDebit,
                TotalCredit = item.TotalCredit,
                TongConNo = (item.TotalDebit - item.TotalCredit),
                UserTypeName = item.UserTypeName,
                CreatedUserName = item.StaffCreateName,
                PaymentMethod = item.PaymentMethod,
                DoanhThu = item.DoanhThu

            }).ToList();
            ViewBag.GetDate = Date;
            return View(model);
        }

        #endregion

        #region TopDichVuBanChay_BanCham
        public ActionResult TopDichVuBanChay_BanCham(string StartDate, string EndDate, string Brand, int? SortBy, int? branchId)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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

            //get sort type
            int? _sortby = SortBy == null ? 1 : SortBy;

            var model = SqlHelper.QuerySP<ChartItem>("spSale_TopDichVuBanChay_BanCham", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                Type = _sortby,
                branchId = branchId,
            });

            if (!string.IsNullOrEmpty(Brand))
            {
                model = model.Where(x => x.id == Brand);
            }

            var category = categoryRepository.GetAllCategories()
            .Select(item => new CategoryViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Name = item.Name
            }).Where(x => x.Code == "Origin").ToList();

            decimal total_slb = 0;
            decimal total_ds = 0;
            foreach (var item in model)
            {
                total_slb += Convert.ToDecimal(item.data2);
                total_ds += Convert.ToDecimal(item.data);
            }

            ViewBag.category = category;
            ViewBag.SortBy = _sortby;
            ViewBag.total_slb = total_slb;
            ViewBag.total_ds = total_ds;
            return View(model);
        }

        public List<ChartItem> IndexExportTopDichVuBanChay_BanCham(string StartDate, string EndDate, string Brand, int? SortBy, int? branchId)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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

            //get sort type
            int? _sortby = SortBy == null ? 1 : SortBy;

            var q = SqlHelper.QuerySP<ChartItem>("spSale_TopDichVuBanChay_BanCham", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                Type = _sortby,
                branchId = branchId,
            }).ToList();

            if (!string.IsNullOrEmpty(Brand))
            {
                q = q.Where(x => x.id == Brand).ToList();
            }

            return q;
        }

        public ActionResult ExportTopDichVuBanChay_BanCham(string StartDate, string EndDate, string Brand, int? SortBy, int? branchId, bool ExportExcel = false)
        {
            string tenfile = "", title = "";
            if (SortBy == 1 || SortBy == null)
            {
                tenfile = "20dvbanchay";
                title = "20 Dịch Vụ Bán Chạy Nhất Theo Thời Gian";
            }
            else
            {
                tenfile = "20dvbancham";
                title = "20 Dịch Vụ Bán Chậm Nhất Theo Thời Gian";
            }

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                tenfile += "_tu" + (Convert.ToDateTime(StartDate)).ToString("yyyyMMdd") + "den" + (Convert.ToDateTime(EndDate)).ToString("yyyyMMdd");
                title += " Từ " + StartDate + " Đến " + EndDate;
            }
            else
            {
                tenfile += "toanthoigian";
            }

            var data = IndexExportTopDichVuBanChay_BanCham(StartDate, EndDate, Brand, SortBy, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlTopDichVuBanChay_BanCham(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách " + title);
            //if (ExportExcel)
            //{
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DS_" + tenfile + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            //}
            return View(model);
        }

        string buildHtmlTopDichVuBanChay_BanCham(List<ChartItem> data)
        {
            decimal tong_slb = 0;
            decimal tong_ds = 0;

            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã Dịch Vụ</th>\r\n";
            detailLists += "		<th>Tên Dịch Vụ</th>\r\n";
            detailLists += "		<th>Số Lượng Bán</th>\r\n";
            detailLists += "		<th>Doanh Số</th>\r\n";
            detailLists += "		<th>Nhãn Hàng</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                tong_slb += Convert.ToDecimal(item.data2);
                tong_ds += Convert.ToDecimal(item.data);

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.label + "</td>\r\n"
                + "<td class=\"text-left \">" + item.label2 + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.data2), null) + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.data), null) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.id + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"3\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_slb, null) + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_ds, null) + "</tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion

        #region SanPhamBanChay/Cham
        public ActionResult TopSanPhamBanChay_BanCham(string startDate, string endDate, string Brand, int? SortBy, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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

            //get sort type
            int? _sortby = SortBy == null ? 1 : SortBy;

            var category = categoryRepository.GetAllCategories()
            .Select(item => new CategoryViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Name = item.Name
            }).Where(x => x.Code == "Origin").ToList();

            var model = SqlHelper.QuerySP<ChartItem>("spSale_TopProduct", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId,
                SortBy = _sortby
            }).ToList();

            if (!string.IsNullOrEmpty(Brand))
            {
                model = model.Where(x => x.group == Brand).ToList();
            }

            ViewBag.category = category;
            ViewBag.SortBy = _sortby;
            return View(model);
        }

        public List<ChartItem> IndexExportTopSanPhamBanChay_BanCham(string startDate, string endDate, string Brand, int? SortBy, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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

            //get sort type
            int? _sortby = SortBy == null ? 1 : SortBy;

            var q = SqlHelper.QuerySP<ChartItem>("[spSale_TopProduct]", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                SortBy = _sortby,
                branchId = branchId,
            }).ToList();

            if (!string.IsNullOrEmpty(Brand))
            {
                q = q.Where(x => x.group == Brand).ToList();
            }

            return q;
        }

        public ActionResult ExportTopSanPhamBanChay_BanCham(string StartDate, string EndDate, string Brand, int? SortBy, int? branchId, bool ExportExcel = false)
        {
            string tenfile = "", title = "";
            if (SortBy == 1 || SortBy == null)
            {
                tenfile = "20spbanchay";
                title = "20 Sản Phẩm Bán Chạy Nhất Theo Thời Gian";
            }
            else
            {
                tenfile = "20spbancham";
                title = "20 Sản Phẩm Bán Chậm Nhất Theo Thời Gian";
            }

            if (!string.IsNullOrEmpty(StartDate) && !string.IsNullOrEmpty(EndDate))
            {
                tenfile += "_tu" + (Convert.ToDateTime(StartDate)).ToString("yyyyMMdd") + "den" + (Convert.ToDateTime(EndDate)).ToString("yyyyMMdd");
                title += " Từ " + StartDate + " Đến " + EndDate;
            }
            else
            {
                tenfile += "toanthoigian";
            }

            var data = IndexExportTopSanPhamBanChay_BanCham(StartDate, EndDate, Brand, SortBy, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlTopSanPhamBanChay_BanCham(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách " + title);
            //if (ExportExcel)
            //{
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DS_" + tenfile + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            //}
            return View(model);
        }

        string buildHtmlTopSanPhamBanChay_BanCham(List<ChartItem> data)
        {
            decimal tong_slb = 0;
            decimal tong_ds = 0;

            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã sản phẩm</th>\r\n";
            detailLists += "		<th>Tên sản phẩm</th>\r\n";
            detailLists += "		<th>Nhãn hàng</th>\r\n";
            detailLists += "		<th>Số lượng</th>\r\n";
            detailLists += "		<th>Doanh số</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                tong_slb += Convert.ToDecimal(item.data);
                tong_ds += Convert.ToDecimal(item.data2);

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.id + "</td>\r\n"
                + "<td class=\"text-left \">" + item.label + "</td>\r\n"
                + "<td class=\"text-left \">" + item.group + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.data), null) + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.data2), null) + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"4\" class=\"text-right\">Tổng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_slb, null) + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_ds, null) + "</tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion

        #region DanhSachKhachTheoNhanHang
        public ActionResult DanhSachKhachTheoNhanHang(string StartDate, string EndDate, string ManagerStaffName, string CustomerCode, string Phone, string Brand, int? branchId)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;

            // Lấy ngày đầu tháng và cuối tháng của ngày hiện tại
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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
            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_ListCustomerByBrand", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            });

            if (!string.IsNullOrEmpty(ManagerStaffName))
            {
                model = model.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ManagerStaffName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(ManagerStaffName)));
            }

            if (!string.IsNullOrEmpty(CustomerCode))
            {
                model = model.Where(x => x.CustomerCode.Contains(CustomerCode) || Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(CustomerCode)));
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                model = model.Where(x => x.Phone.Contains(Phone));
            }

            if (!string.IsNullOrEmpty(Brand))
            {
                model = model.Where(x => x.CountForBrand == Brand);
            }

            decimal total_ds = 0;
            decimal total_dt = 0;
            decimal total_cn = 0;
            foreach (var item in model)
            {
                total_ds += Convert.ToDecimal(item.TotalAmount);
                total_dt += Convert.ToDecimal(item.DoanhThu);
                total_cn += Convert.ToDecimal(item.tienconno);
            }

            var category = categoryRepository.GetAllCategories()
            .Select(item => new CategoryViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Name = item.Name
            }).Where(x => x.Code == "Origin").ToList();

            ViewBag.category = category;
            ViewBag.total_ds = total_ds;
            ViewBag.total_dt = total_dt;
            ViewBag.total_cn = total_cn;

            return View(model);

        }

        public List<ProductInvoiceViewModel> IndexExportDanhSachKhachHangTheoNhanHang(string StartDate, string EndDate, string ManagerStaffName, string CustomerCode, string Phone, string Brand, int? branchId)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;

            // Lấy ngày đầu tháng và cuối tháng của ngày hiện tại
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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
            var q = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_ListCustomerByBrand", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            if (!string.IsNullOrEmpty(ManagerStaffName))
            {
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ManagerStaffName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(ManagerStaffName))).ToList();
            }

            if (!string.IsNullOrEmpty(CustomerCode))
            {
                q = q.Where(x => x.CustomerCode.Contains(CustomerCode) || Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(CustomerCode))).ToList();
            }

            if (!string.IsNullOrEmpty(Phone))
            {
                q = q.Where(x => x.Phone.Contains(Phone)).ToList();
            }

            if (!string.IsNullOrEmpty(Brand))
            {
                q = q.Where(x => x.CountForBrand == Brand).ToList();
            }

            return q;
        }

        string buildHtmlDanhSachKhachHangTheoNhanHang(List<ProductInvoiceViewModel> data)
        {
            decimal? tong_ds = 0;
            decimal? tong_dt = 0;
            decimal? tong_cn = 0;

            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng Doanh Số</th>\r\n";
            detailLists += "		<th>Mã Số</th>\r\n";
            detailLists += "		<th>Tên</th>\r\n";
            detailLists += "		<th>Số Điện Thoại</th>\r\n";
            detailLists += "		<th>Địa Chỉ</th>\r\n";
            detailLists += "		<th>Nhãn Hàng</th>\r\n";
            detailLists += "		<th>Ngày Bắt Đầu Giao Dịch</th>\r\n";
            detailLists += "		<th>Lần Cuối Giao Dịch</th>\r\n";
            detailLists += "		<th>Doanh Số</th>\r\n";
            detailLists += "		<th>Doanh Thu</th>\r\n";
            detailLists += "		<th>Còn Nợ</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                tong_ds += item.TotalAmount == null ? 0 : item.TotalAmount;
                tong_dt += item.DoanhThu == null ? 0 : item.DoanhThu;
                tong_cn += item.tienconno == null ? 0 : item.tienconno;

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Phone + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Address + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CountForBrand + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.StartDate).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.EndDate).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.TotalAmount), null).Replace(".",",") + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.DoanhThu), null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.tienconno), null).Replace(".", ",") + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"10\" class=\"text-right\">Tổng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_ds, null).Replace(".", ",") + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_dt, null).Replace(".", ",") + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_cn, null).Replace(".", ",") + "</tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }

        public ActionResult ExportDanhSachKhachHangTheoNhanHang(string StartDate, string EndDate, string ManagerStaffName, string CustomerCode, string Phone, string Brand, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportDanhSachKhachHangTheoNhanHang(StartDate, EndDate, ManagerStaffName, CustomerCode, Phone, Brand, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDanhSachKhachHangTheoNhanHang(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách Khách Hàng Theo Nhãn Hàng");
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DSKHTheoNhanHang" + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View(model);
        }
        #endregion

        #region DSKhachHangMoiPhatSinhCoGiaoDich
        public ActionResult DSKhachHangMoiCoPhatSinhGiaoDich(string StartDate, string EndDate, int? branchId)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;

            // Lấy ngày đầu tháng và cuối tháng của ngày hiện tại
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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
            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_ListNewCustomerByBrand", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            });


            decimal total_ds = 0;
            decimal total_dt = 0;
            decimal total_cn = 0;

            foreach (var item in model)
            {
                total_ds += Convert.ToDecimal(item.TotalAmount);
                total_dt += Convert.ToDecimal(item.DoanhThu);
                total_cn += Convert.ToDecimal(item.tienconno);
            }

            ViewBag.total_ds = total_ds;
            ViewBag.total_dt = total_dt;
            ViewBag.total_cn = total_cn;
            return View(model);
        }

        public List<ProductInvoiceViewModel> IndexExportDSKhachHangMoiCoPhatSinhGiaoDich(string StartDate, string EndDate, int? branchId)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;

            // Lấy ngày đầu tháng và cuối tháng của ngày hiện tại
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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
            var q = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_ListNewCustomerByBrand", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return q;
        }

        public ActionResult ExportDSKhachHangMoiCoPhatSinhGiaoDich(string StartDate, string EndDate, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportDSKhachHangMoiCoPhatSinhGiaoDich(StartDate, EndDate, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDSKhachHangMoiCoPhatSinhGiaoDich(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách Khách Hàng Mới Có Phát Sinh Giao Dịch");
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DS_KHMoiCoPhatSinhGiaoDich" + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View(model);
        }

        string buildHtmlDSKhachHangMoiCoPhatSinhGiaoDich(List<ProductInvoiceViewModel> data)
        {
            decimal? tong_ds = 0;
            decimal? tong_dt = 0;
            decimal? tong_cn = 0;

            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng Doanh Số</th>\r\n";
            detailLists += "		<th>Mã Số</th>\r\n";
            detailLists += "		<th>Tên</th>\r\n";
            detailLists += "		<th>Số Điện Thoại</th>\r\n";
            detailLists += "		<th>Địa Chỉ</th>\r\n";
            detailLists += "		<th>Nhãn Hàng</th>\r\n";
            detailLists += "		<th>Doanh Số</th>\r\n";
            detailLists += "		<th>Doanh Thu</th>\r\n";
            detailLists += "		<th>Còn Nợ</th>\r\n";
            detailLists += "		<th>Ngày Bắt Đầu Giao Dịch</th>\r\n";
            detailLists += "		<th>Lần Cuối Giao Dịch</th>\r\n";
            detailLists += "		<th>Lần Chăm Sóc Gần Nhất</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                tong_ds += item.TotalAmount == null ? 0 : item.TotalAmount;
                tong_dt += item.DoanhThu == null ? 0 : item.DoanhThu;
                tong_cn += item.tienconno == null ? 0 : item.tienconno;

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Phone + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Address + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CountForBrand + "</td>\r\n"
                + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.TotalAmount), null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.DoanhThu), null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.tienconno), null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.StartDate).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.EndDate).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.NgayCSGN).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"8\" class=\"text-right\">Tổng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_ds, null).Replace(".", ",") + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_dt, null).Replace(".", ",") + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_cn, null).Replace(".", ",") + "</td>" + "<td colspan=\"3\"> </td>" + "</tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion
        #region Tần Suất CSD Khách Vip
        public List<LogVipViewModel> IndexExportTanSuatCSDKhachVip(string StartDate, string EndDate, int? branchId)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            //get cookie brachID ]\
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
            var q = SqlHelper.QuerySP<LogVipViewModel>("sp_StatisticFrequencyByTime", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return q;
        }
        public ActionResult TanSuatCSDKhachVip(string StartDate, string EndDate, int? branchId, string txtCode)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;

            // Lấy ngày đầu tháng và cuối tháng của ngày hiện tại
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);

            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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
            var model = SqlHelper.QuerySP<LogVipViewModel>("sp_StatisticFrequencyByTime", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            });
            // tìm kiếm theo mã kh, tên
            if (string.IsNullOrEmpty(txtCode) == false)
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();

                txtCode = txtCode == "" ? "~" : Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtCode);
                model = model.Where(x => x.FirstName.ToLowerOrEmpty().Contains(txtCode)
                || x.LastName.ToLowerOrEmpty().Contains(txtCode) || x.CustomerCode.ToLowerOrEmpty().Contains(txtCode)
                    || Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(txtCode));
            }
            return View(model);

        }
        string buildHtmlTanSuatCSDKhachVip(List<LogVipViewModel> data)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Nhân Viên Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng Doanh Số</th>\r\n";
            detailLists += "		<th>Mã Khách Hàng</th>\r\n";
            detailLists += "		<th>Tên Khách Hàng</th>\r\n";
            detailLists += "		<th>Xếp Hạng Vip</th>\r\n";
            detailLists += "		<th>Số Lần CSD</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.FirstName + item.LastName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Name + "</td>\r\n"
                + "<td class=\"text-left \">" + item.solan + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        public ActionResult ExportTanSuatCSDKhachVip(string StartDate, string EndDate, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportTanSuatCSDKhachVip(StartDate, EndDate, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlTanSuatCSDKhachVip(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Tần Suất CSD Khách Vip");
            Response.AppendHeader("content-disposition", "attachment;filename=" + "TanSuatCSD" + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View(model);
        }
        #endregion
        #region Tần Suất CSD Khách Theo Tháng
        public List<ChartItem> IndexExportTanSuatCSDKhachTheoThang(int? month, int? year, int? branchId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            //get cookie brachID ]\
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
            var q = SqlHelper.QuerySP<ChartItem>("sp_TanSuatCSDTheoThang", new
            {
                Month = month,
                Year = year,
                branchId = branchId
            }).ToList();

            return q;
        }
        public ActionResult TanSuatCSDKhachTheoThang(int? month, int? year, int? branchId)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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
            var model = SqlHelper.QuerySP<ChartItem>("sp_TanSuatCSDTheoThang", new
            {
                Month = month,
                Year = year,
                branchId = branchId
            });

            return View(model);

        }
        string buildHtmlTanSuatCSDKhachTheoThang(List<ChartItem> data)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Nhân Viên Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng Doanh Số</th>\r\n";
            detailLists += "		<th>Tên Khách Hàng</th>\r\n";
            detailLists += "		<th>Tần Suất CSD</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.group + "</td>\r\n"
                + "<td class=\"text-left \">" + item.label + "</td>\r\n"
                + "<td class=\"text-left \">" + item.label2 + "</td>\r\n"
                + "<td class=\"text-left \">" + item.id + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        public ActionResult ExportTanSuatCSDKhachTheoThang(int? month, int? year, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportTanSuatCSDKhachTheoThang(month, year, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlTanSuatCSDKhachTheoThang(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Tần Suất CSD Khách Tháng " + month);
            Response.AppendHeader("content-disposition", "attachment;filename=" + "TanSuatCSDThang" + month + "Nam" + year + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View(model);
        }
        #endregion


        #region SL dự đoán hàng hóa dựa trên KHBH của chi nhánh theo tháng
        public List<ProductInvoiceDetailViewModel> IndexExportSLdudoanduatrenKHBHtheothang(int? month, int? year, int? branchId, string txtproductname, string countForBrand)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            //get cookie brachID ]\
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
            var q = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("GetCrmDetailByBranch", new
            {
                Month = month,
                Year = year,
                branchId = branchId
            }).ToList();

            if (!string.IsNullOrEmpty(countForBrand))
            {
                q = q.Where(x => x.CountForBrand == countForBrand).ToList();
            }
            if (!string.IsNullOrEmpty(txtproductname))
            {
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ProductName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtproductname))).ToList();
            }
            return q;
        }
        public ActionResult SoLuongSPTheoKHBHTheoCN(int? month, int? year, int? branchId, string txtproductname, string countForBrand)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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
            var model = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("GetCrmDetailByBranch", new
            {
                Month = month,
                Year = year,
                branchId = branchId
            });

            var category = categoryRepository.GetAllCategories()
            .Select(item => new CategoryViewModel
            {
                Id = item.Id,
                Code = item.Code,
                Value = item.Value,
                Name = item.Name
            }).Where(x => x.Code == "Origin").ToList();

            if (!string.IsNullOrEmpty(countForBrand))
            {
                model = model.Where(x => x.CountForBrand == countForBrand).ToList();
            }
            if (!string.IsNullOrEmpty(txtproductname))
            {
                model = model.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ProductName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtproductname)));
            }
            ViewBag.category = category;
                
            return View(model);

        }
        string buildHtmlSLdudoanduatrenKHBHtheothang(List<ProductInvoiceDetailViewModel> data)
        {
            
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>Kế Hoạch Tháng</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhãn Hàng</th>\r\n";
            detailLists += "		<th>Tên Sản Phẩm</th>\r\n";
            detailLists += "		<th>Số Lượng Sản Phẩm</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + item.Month + item.Year +"</td>\r\n"
                + "<td class=\"text-left \">" + item.BranchName+ "</td>\r\n"
                + "<td class=\"text-left \">" + item.CountForBrand+ "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-right \">" + item.Quantity + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        public ActionResult ExportSLdudoanduatrenKHBHtheothang(int? month, int? year, int? branchId, string txtproductname, string countForBrand, bool ExportExcel = false)
        {
            var data = IndexExportSLdudoanduatrenKHBHtheothang(month, year, branchId, txtproductname, countForBrand);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSLdudoanduatrenKHBHtheothang(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Số lượng dự đoán hàng hóa dựa trên KHBH của chi nhánh theo tháng " + month);
            Response.AppendHeader("content-disposition", "attachment;filename=" + "SLdudoanduatrenKHBHtheothang" + month + "Nam" + year + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View(model);
        }
        #endregion
        #region SL dự đoán hàng hóa dựa trên KHBH của chi nhánh theo tháng của nhân viên
        public List<ProductInvoiceDetailViewModel> IndexExportSLdudoanduatrenKHBHtheothangcuanNV(int? month, int? year, int? branchId,string brandname,  string ManagerStaffId, string txtproductname)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            //get cookie brachID ]\
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
            var q = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("GetCrmDetailByStaff", new
            {
                Month = month,
                Year = year,
                branchId = branchId
            }).ToList();

            if (!string.IsNullOrEmpty(brandname))
            {
                q = q.Where(x => x.CountForBrand == brandname).ToList();
            }

            if (!string.IsNullOrEmpty(ManagerStaffId))
            {
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ManagerStaffId.ToString()).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(ManagerStaffId))).ToList();
            }

            if (!string.IsNullOrEmpty(txtproductname))
            {
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ProductName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtproductname))).ToList();
            }
            return q;
        }
        public ActionResult SoLuongSPTheoKHBHTheoCNcuaNV(int? month, int? year, int? branchId, string countForBrand, string ManagerStaffId, string txtproductname)
        {
            year = year == null ? DateTime.Now.Year : year;
            month = month == null ? DateTime.Now.Month : month;

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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
            var model = SqlHelper.QuerySP<ProductInvoiceDetailViewModel>("GetCrmDetailByStaff", new
            {
                Month = month,
                Year = year,
                branchId = branchId
            });

            if (!string.IsNullOrEmpty(countForBrand))
            {
                model = model.Where(x => x.CountForBrand == countForBrand);
            }

            if (!string.IsNullOrEmpty(ManagerStaffId))
            {
                model = model.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ManagerStaffId.ToString()).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(ManagerStaffId)));
            }

            if (!string.IsNullOrEmpty(txtproductname))
            {
                model = model.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.ProductName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtproductname)));
            }

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
        string buildHtmlSLdudoanduatrenKHBHtheothangcuaNV(List<ProductInvoiceDetailViewModel> data)
        {

            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>Kế Hoạch Tháng</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm hưởng doanh số</th>\r\n";
            detailLists += "		<th>Nhãn Hàng</th>\r\n";
            detailLists += "		<th>Tên Sản Phẩm</th>\r\n";
            detailLists += "		<th>Số Lượng Sản Phẩm</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + item.Month + item.Year + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CountForBrand + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-right \">" + item.Quantity + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        public ActionResult ExportSLdudoanduatrenKHBHtheothangcuaNV(int? month, int? year, int? branchId, string brandname,string ManagerStaffId, string txtproductname, bool ExportExcel = false)
        {
            var data = IndexExportSLdudoanduatrenKHBHtheothangcuanNV(month, year, branchId, brandname, ManagerStaffId, txtproductname);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlSLdudoanduatrenKHBHtheothangcuaNV(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Số lượng dự đoán hàng hóa dựa trên KHBH của chi nhánh theo tháng của nhân viên" + month);
            Response.AppendHeader("content-disposition", "attachment;filename=" + "SoLuongSPTheoKHBHTheoCNcuaNV" + month + "Nam" + year + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View(model);
        }
        #endregion
        #region Danh Sách Khách Đạt Thẻ Vip Theo Thời Gian  Lựa Chọn
        public List<ProductInvoiceViewModel> IndexExportDSKH_DatTheVip(string StartDate, string EndDate, int? branchId , string SalerId, string LoyaltyPointId, string CustomerCode)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);
            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            //get cookie brachID ]\
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

            var q = SqlHelper.QuerySP<ProductInvoiceViewModel>("sp_CustomerListGainVipCard", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();
            
            if (!string.IsNullOrEmpty(SalerId))
            {
                q = q.Where(x => x.ManagerStaffId == Convert.ToInt32(SalerId)).ToList();
            }
            if (!string.IsNullOrEmpty(CustomerCode))
            {
                q = q.Where(x => x.CustomerCode.Contains(CustomerCode) || Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(CustomerCode))).ToList();
            }
            if (!string.IsNullOrEmpty(LoyaltyPointId))
            {
                q = q.Where(x => x.IdHang == Convert.ToInt32(LoyaltyPointId)).ToList();
            }

            return q;
        }
        public ActionResult DSKH_DatTheVip(string StartDate, string EndDate, int? branchId, string SalerId, string LoyaltyPointId, string CustomerCode)
        {
            StartDate = string.IsNullOrEmpty(StartDate) ? "" : StartDate;
            EndDate = string.IsNullOrEmpty(EndDate) ? "" : EndDate;
            SalerId = SalerId == " " ? "" : SalerId;
            LoyaltyPointId = LoyaltyPointId == " " ? "" : LoyaltyPointId;
            CustomerCode = CustomerCode == " " ? "" : CustomerCode;
            // Lấy ngày đầu tháng và cuối tháng của ngày hiện tại
            DateTime now = DateTime.Now;
            DateTime _startDate = new DateTime(now.Year, now.Month, 1);
            DateTime _endDate = _startDate.AddMonths(1).AddDays(-1);
            var d_startDate = (!string.IsNullOrEmpty(StartDate) == true ? DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : _startDate.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(EndDate) == true ? DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : _endDate.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;


            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }
            

            //get cookie brachID ]\
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
            decimal total_tongtotal = 0;
            decimal total_tongtienthu = 0;
            decimal total_tongtienno = 0;
            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("sp_CustomerListGainVipCard", new
            {
               StartDate = d_startDate,
               EndDate = d_endDate,
               branchId = branchId
        });

            foreach (var item in model)
            {
                total_tongtotal += Convert.ToDecimal(item.total);
                total_tongtienthu += Convert.ToDecimal(item.tienthu);
                total_tongtienno += Convert.ToDecimal(item.tienno);
            }
            if (!string.IsNullOrEmpty(SalerId))
            {
                model = model.Where(x => x.ManagerStaffId == Convert.ToInt32(SalerId));
            }

            if (!string.IsNullOrEmpty(CustomerCode))
            {
                model = model.Where(x => x.CustomerCode.Contains(CustomerCode) || Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(CustomerCode)));
            }
            if (string.IsNullOrEmpty(LoyaltyPointId) == false)
            {
                model = model.Where(x => x.IdHang == Convert.ToInt32(LoyaltyPointId));
            }
            ViewBag.total_tongtotal = total_tongtotal;
            ViewBag.total_tongtienthu = total_tongtienthu;
            ViewBag.total_tongtienno = total_tongtienno;
            return View(model);

        }
        string buildHtmlDSKH_DatTheVip(List<ProductInvoiceViewModel> data)
        {
            decimal? total_tongtotal = 0;
            decimal? total_tongtienthu = 0;
            decimal? total_tongtienno = 0;
            
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng Doanh Số</th>\r\n";
            detailLists += "		<th>Mã Số</th>\r\n";
            detailLists += "		<th>Tên Khách Hàng</th>\r\n";
            detailLists += "		<th>Tuổi</th>\r\n";
            detailLists += "		<th>Số Điện Thoại</th>\r\n";
            detailLists += "		<th>Địa Chỉ</th>\r\n";
            detailLists += "		<th>Doanh Số</th>\r\n";
            detailLists += "		<th>Doanh Thu</th>\r\n";
            detailLists += "		<th>Còn Nợ</th>\r\n";
            detailLists += "		<th>Loại Thẻ Đạt</th>\r\n";
            detailLists += "		<th>Năm</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                total_tongtotal += item.total == null ? 0 : item.total;
                total_tongtienthu += item.tienthu == null ? 0 : item.tienthu;
                total_tongtienno += item.tienno == null ? 0 : item.tienno;
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName +"</td>\r\n"
                + "<td class=\"text-left \">" + item.tuoi + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerPhone + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Address + "</td>\r\n"
                + "<td class=\"text-left \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.total), null).Replace(".", ",") + "</td>\r\n"
                +"<td class=\"text-left \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.tienthu), null).Replace(".", ",") + "</td>\r\n"
                +"<td class=\"text-left \">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(Convert.ToDecimal(item.tienno), null).Replace(".", ",") + "</td>\r\n"
                + "<td class=\"text-left \">" + item.TenHang + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NAM + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"8\" class=\"text-right\">Tổng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(total_tongtotal, null).Replace(".", ",") + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(total_tongtienthu, null).Replace(".", ",") + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(total_tongtienno, null).Replace(".", ",") + "</tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        public ActionResult ExportDSKH_DatTheVip(string StartDate, string EndDate, int? branchId, string SalerId, string LoyaltyPointId, string CustomerCode, bool ExportExcel = false)
        {
            SalerId = SalerId == " " ? "" : SalerId;
            LoyaltyPointId = LoyaltyPointId == " " ? "" : LoyaltyPointId;
            CustomerCode = CustomerCode == " " ? "" : CustomerCode;
            var data = IndexExportDSKH_DatTheVip(StartDate, EndDate, branchId, SalerId, LoyaltyPointId, CustomerCode);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDSKH_DatTheVip(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách Khách Hàng Đạt Thẻ Vip");
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DSKH_DatTheVip" + ".xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            return View(model);
        }
        #endregion
        #region Danh sách khách hàng tạm ngưng giao dịch
        public ActionResult DSKHTamNgungGiaoDich(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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


            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_KhachHangTamNgung", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return View(model);
        }

        public List<ProductInvoiceViewModel> IndexExportDSKHTamNgungGiaoDich(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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

            var q = SqlHelper.QuerySP<ProductInvoiceViewModel>("[spSale_KhachHangTamNgung]", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return q;
        }

        public ActionResult ExportDSKHTamNgungGiaoDich(string StartDate, string EndDate, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportDSKHTamNgungGiaoDich(StartDate, EndDate, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDSKHTamNgungGiaoDich(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách Khách Hàng Tạm Ngưng Giao Dịch" );
            //if (ExportExcel)
            //{
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DS_KHTamNgungGiaoDich.xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            //}
            return View(model);
        }

        string buildHtmlDSKHTamNgungGiaoDich(List<ProductInvoiceViewModel> data)
        {
            decimal tong_doanhso = 0;
            decimal tong_doanhthu = 0;
            decimal tong_conno = 0;
            int tong_sophieucon = 0;

            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng DS</th>\r\n";
            detailLists += "		<th>Thời Gian Tạm Ngưng (ngày)</th>\r\n";
            detailLists += "		<th>Mã số</th>\r\n";
            detailLists += "		<th>Tên</th>\r\n";
            detailLists += "		<th>SĐT</th>\r\n";
            detailLists += "		<th>Địa Chỉ</th>\r\n";
            detailLists += "		<th>Ngày GDGN</th>\r\n";
            detailLists += "		<th>Ngày CSGN</th>\r\n";
            detailLists += "		<th>Doanh Số</th>\r\n";
            detailLists += "		<th>Doanh Thu</th>\r\n";
            detailLists += "		<th>Còn Nợ</th>\r\n";
            detailLists += "		<th>Số Phiếu Còn</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                tong_doanhso += Convert.ToDecimal(item.TotalAmount);
                tong_doanhthu += Convert.ToDecimal(item.tiendathu);
                tong_conno += Convert.ToDecimal(item.tienconno);
                tong_sophieucon += Convert.ToInt32(item.SoPhieuCon);

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Day + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Phone + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Address + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NgayGDGN + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NgayCSGN + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.TotalAmount, null) + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.tiendathu, null) + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.tienconno, null) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.SoPhieuCon + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"10\" class=\"text-right\">Tổng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_doanhso, null) + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_doanhthu, null) + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_conno, null) + "</td>" + "<td class=\"text-right\">"
                        + tong_sophieucon + "</tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion
        #region Danh sách khách hàng hết phiếu CSD
        public ActionResult DSKHHetPhieuCSD(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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


            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_KHHetPhieuCSD", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return View(model);
        }

        public List<ProductInvoiceViewModel> IndexExportDSKHHetPhieuCSD(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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

            var q = SqlHelper.QuerySP<ProductInvoiceViewModel>("[spSale_KHHetPhieuCSD]", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return q;
        }

        public ActionResult ExportDSKHHetPhieuCSD(string StartDate, string EndDate, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportDSKHHetPhieuCSD(StartDate, EndDate, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDSKHHetPhieuCSD(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách Khách Hàng Hết Phiếu CSD");
            //if (ExportExcel)
            //{
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DSKH_HetPhieuCSD.xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            //}
            return View(model);
        }

        string buildHtmlDSKHHetPhieuCSD(List<ProductInvoiceViewModel> data)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng DS</th>\r\n";
            detailLists += "		<th>Tên Khách Hàng</th>\r\n";
            detailLists += "		<th>Mã Khách Hàng</th>\r\n";
            detailLists += "		<th>SĐT</th>\r\n";
            detailLists += "		<th>Tên Liệu Trình</th>\r\n";
            detailLists += "		<th>Ngày GDGN</th>\r\n";
            detailLists += "		<th>Ngày CSGN</th>\r\n";
            detailLists += "		<th>Ngày GDĐT</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Phone + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductName + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime((item.NgayGDGN)).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime((item.NgayCSGN)).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime((item.NgayGDDT)).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion

        #region Danh sách khách hàng tạm ngưng chăm sóc da
        public ActionResult DSKHTamNgungCSD(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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


            var model = SqlHelper.QuerySP<ProductInvoiceViewModel>("spSale_KhachHangTamNgungCSD", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return View(model);
        }

        public List<ProductInvoiceViewModel> IndexExportDSKHTamNgungCSD(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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

            var q = SqlHelper.QuerySP<ProductInvoiceViewModel>("[spSale_KhachHangTamNgungCSD]", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return q;
        }

        public ActionResult ExportDSKHTamNgungCSD(string StartDate, string EndDate, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportDSKHTamNgungCSD(StartDate, EndDate, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDSKHTamNgungCSD(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách Khách Hàng Tạm Ngưng Chăm Sóc Da");
            //if (ExportExcel)
            //{
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DS_KHTamNgungCSD.xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            //}
            return View(model);
        }

        string buildHtmlDSKHTamNgungCSD(List<ProductInvoiceViewModel> data)
        {
            decimal tong_doanhso = 0;
            decimal tong_doanhthu = 0;
            decimal tong_conno = 0;
            int tong_sophieucon = 0;

            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Người Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng DS</th>\r\n";
            detailLists += "		<th>Thời Gian Tạm Ngưng (ngày)</th>\r\n";
            detailLists += "		<th>Mã số</th>\r\n";
            detailLists += "		<th>Tên</th>\r\n";
            detailLists += "		<th>SĐT</th>\r\n";
            detailLists += "		<th>Địa Chỉ</th>\r\n";
            detailLists += "		<th>Ngày GDGN</th>\r\n";
            detailLists += "		<th>Ngày CSGN</th>\r\n";
            detailLists += "		<th>Doanh Số</th>\r\n";
            detailLists += "		<th>Doanh Thu</th>\r\n";
            detailLists += "		<th>Còn Nợ</th>\r\n";
            detailLists += "		<th>Số Phiếu Còn</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                tong_doanhso += Convert.ToDecimal(item.TotalAmount);
                tong_doanhthu += Convert.ToDecimal(item.tiendathu);
                tong_conno += Convert.ToDecimal(item.tienconno);
                tong_sophieucon += Convert.ToInt32(item.SoPhieuCon);

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ManagerStaffName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhomNVKD + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Day + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Phone + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Address + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.NgayGDGN).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.NgayCSGN).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.TotalAmount, null) + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.tiendathu, null) + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.tienconno, null) + "</td>\r\n"         
                + "<td class=\"text-left \">" + item.SoPhieuCon + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"10\" class=\"text-right\">Tổng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_doanhso, null) + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_doanhthu, null) + "</td>" + "<td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_conno, null) + "</td>" + "<td class=\"text-right\">"
                        + tong_sophieucon + "</tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion



        #region Doanh So Tong
        public ViewResult Revenue(string start, string end, int? UserId, int? size, int? page)
        {
            if (start == null && end == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                start = aDateTime.ToString("dd/MM/yyyy");
                end = retDateTime.ToString("dd/MM/yyyy");
            }
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";

            if (request.Cookies["BRANCH_ID_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_CookieName"].Value;
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



            var productinvoice = invoiceRepository.GetAllvwProductInvoice().AsEnumerable().Where(x => (x.BranchId == intBrandID || intBrandID == 0) && (x.Status == "complete"));
            var invoicedetail = invoiceRepository.GetAllvwInvoiceDetails().AsEnumerable().Where(x => x.BranchId == intBrandID || intBrandID == 0); 
            var branch_list = BranchRepository.GetAllBranch().ToList();
            var salereturn = salesReturnsRepository.GetAllSalesReturns().AsEnumerable().Where(x => x.BranchId == intBrandID || intBrandID == 0);

            var salereturndt = salesReturnsRepository.GetAllvwReturnsDetails().AsEnumerable().Where(x => x.BranchId == intBrandID || intBrandID == 0);



            var salereturnmodeldt = salereturndt.Select(x => new SalesReturnsDetailViewModel
            {
                BranchId = x.BranchId,
                CreatedUserId = x.NguoiTao,
                CreatedDate = x.NgayTao,
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity,
                ProductName = x.ProductName,
                ProductCode = x.ProductCode,
            }).ToList();

            //begin hoapd them vao de tinh tien ban hang, tien mat, chuyen khoan, quet the
            ViewBag.tienbanhang = 0;
            ViewBag.sodonhang = 0;
            ViewBag.tienhangtra = 0;
            ViewBag.sodonhangtra = 0;
            ViewBag.tienbanhang_TM = 0;
            ViewBag.tienbanhang_CK = 0;
            ViewBag.tienbanhang_THE = 0;





            //end hoapd them vao de tinh tien ban hang, tien mat, chuyen khoan, quet the


            //Lấy ds don hang 
            var modeldetail = invoicedetail.Select(x => new ProductInvoiceDetailViewModel
            {
                Id = x.Id,
                Price = x.Price,
                ProductId = x.ProductId,
                //PromotionDetailId = x.PromotionDetailId,
                //PromotionId = x.PromotionId,
                //PromotionValue = x.PromotionValue,
                Quantity = x.Quantity,
                Unit = x.Unit,
                ProductType = x.ProductType,
                //FixedDiscount = x.FixedDiscount,
                //FixedDiscountAmount = x.FixedDiscountAmount,
                //IrregularDiscountAmount = x.IrregularDiscountAmount,
                //IrregularDiscount = x.IrregularDiscount,
                CategoryCode = x.CategoryCode,
                ProductInvoiceCode = x.ProductInvoiceCode,
                ProductName = x.ProductName,
                ProductCode = x.ProductCode,
                ProductInvoiceId = x.ProductInvoiceId,
                ProductGroup = x.ProductGroup,
                //CheckPromotion = x.CheckPromotion,
                //IsReturn = x.IsReturn,
                //Status = x.Status,
                Amount = x.Amount,
                LoCode = x.LoCode,
                ExpiryDate = x.ExpiryDate,
                Origin = x.Origin,
                //TaxFeeAmount = x.TaxFeeAmount,
                //TaxFee = x.TaxFee,
                //Amount2 = x.Amount2,
                BranchId = x.BranchId,
                //QuantitySaleReturn = x.QuantitySaleReturn,
                CreatedDate = x.CreatedDate,
                CreatedUserId = x.CreatedUserId

            }).ToList();


            var model = productinvoice.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                ShipCityName = item.ShipCityName,
                TotalAmount = item.TotalAmount,
                //FixedDiscount = item.FixedDiscount,
                TaxFee = item.TaxFee,
                CodeInvoiceRed = item.CodeInvoiceRed,
                Status = item.Status,
                IsArchive = item.IsArchive,
                //ProductOutboundId = item.ProductOutboundId,
                //ProductOutboundCode = item.ProductOutboundCode,
                PaidAmount = item.PaidAmount,
                RemainingAmount = item.RemainingAmount,
                Note = item.Note,
                CancelReason = item.CancelReason,
                BranchId = item.BranchId,
                CustomerId = item.CustomerId,
                BranchName = item.BranchName,
                ManagerStaffId = item.ManagerStaffId,
                ManagerStaffName = item.ManagerStaffName,
                ManagerUserName = item.ManagerUserName,
                CountForBrand = item.CountForBrand,
                TotalDebit = item.TotalDebit,
                TotalCredit = item.TotalCredit,
                TongConNo = item.tienconno,
                tienconno = item.tienconno,
                tiendathu = item.tiendathu,
                UserTypeName = item.NhomNVKD,
                CreatedUserName = item.StaffCreateName,
                PaymentMethod = item.PaymentMethod,
                DoanhThu = item.DoanhThu


            }).OrderByDescending(m => m.Id).ToList();

            //Lay nguoi ban
            var user = userRepository.GetAllvwUsers();
            var usermodel = user.Select(item => new UserViewModel
            {
                Id = item.Id,
                FullName = item.LastName + " " + item.FirstName,
                BranchId = item.BranchId,
                Status = item.Status
            }).Where(x => x.Status == Domain.Entities.UserStatus.Active).ToList();


            //Lay cua hang
            var branch = branch_list.Select(item => new BranchViewModel
            {
                Id = item.Id,
                Name = item.Name

            }).ToList();
            //Lấy hàng trả 
            var salereturnmodel = salereturn.Select(x => new SalesReturnsViewModel
            {
                Id = x.Id,
                BranchId = x.BranchId,
                CreatedUserId = x.CreatedUserId,
                CreatedDate = x.CreatedDate,
                TotalAmount = x.TotalAmount

            }).ToList();

            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(start, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(end, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    model = model.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                    modeldetail = modeldetail.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                    salereturnmodel = salereturnmodel.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                    salereturnmodeldt = salereturnmodeldt.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                }
            }

            if (intBrandID != null && intBrandID > 0)
            {
                model = model.Where(x => x.BranchId == intBrandID).ToList();
                branch = branch.Where(x => x.Id == intBrandID).ToList();
                modeldetail = modeldetail.Where(x => x.BranchId == intBrandID).ToList();
                //usermodel = usermodel.Where(x => x.BranchId == intBrandID).ToList();
                salereturnmodel = salereturnmodel.Where(x => x.BranchId == intBrandID).ToList();
            }
            if (UserId != null)
            {
                model = model.Where(x => x.CreatedUserId == UserId).ToList();
                modeldetail = modeldetail.Where(x => x.CreatedUserId == UserId).ToList();
                salereturnmodel = salereturnmodel.Where(x => x.CreatedUserId == UserId).ToList();
                salereturnmodeldt = salereturnmodeldt.Where(x => x.CreatedUserId == UserId).ToList();
            }



            //phân trang trong foreach
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });


            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }


            ViewBag.size = items;
            ViewBag.currentSize = size;

            page = page ?? 1;

            int pageSize = (size ?? 10);


            int pageNumber = (page ?? 1);
            //end phân trang trong foreach

            ViewBag.page = page;
            ViewBag.Listdetails = modeldetail;
            ViewBag.BranchList = branch;
            ViewBag.user = usermodel;
            ViewBag.salereturn = salereturnmodel;
            ViewBag.salereturnmodeldt = salereturnmodeldt;
            ViewBag.model = model;



            if (model.Count() > 0)
            {
                ViewBag.tienbanhang = model.Sum(x => x.TotalAmount);
                ViewBag.sodonhang = model.Count();
                ViewBag.tienhangtra = salereturnmodel.Sum(x => x.TotalAmount);
                ViewBag.sodonhangtra = salereturnmodel.Count();
                //ViewBag.tienbanhang_TM = model.Sum(x => x.MoneyPay);
                //ViewBag.tienbanhang_CK = model.Sum(x => x.TransferPay);
                //ViewBag.tienbanhang_THE = model.Sum(x => x.ATMPay);
            }

            var a = model.ToPagedList(pageNumber, pageSize);

            return View(model.ToPagedList(pageNumber, pageSize));
            //return View(model);
        }

        public PartialViewResult _RevenueProduct(string start, string end, int? page, int? size, string searchproduct)
        {
            if (start == null && end == null)
            {
                DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
                start = aDateTime.ToString("dd/MM/yyyy");
                end = retDateTime.ToString("dd/MM/yyyy");
            }
            ViewBag.start = start;
            ViewBag.end = end;
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";

            if (request.Cookies["BRANCH_ID_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_CookieName"].Value;
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

            var details = invoiceRepository.GetAllvwInvoiceDetails().ToList();
            var productlist = ProductRepository.GetAllProduct().AsEnumerable();


            var salereturn = salesReturnsRepository.GetAllvwReturnsDetails().AsEnumerable().Where(x => x.BranchId == intBrandID || intBrandID == 0);



            var salereturnmodel = salereturn.Select(x => new SalesReturnsDetailViewModel
            {
                BranchId = x.BranchId,
                CreatedUserId = x.CreatedUserId,
                CreatedDate = x.CreatedDate,
                ProductId = x.ProductId,
                Price = x.Price,
                Quantity = x.Quantity,
                ProductName = x.ProductName,
                ProductCode = x.ProductCode,
            }).ToList();








            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(start, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(end, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);

                    details = details.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                    salereturnmodel = salereturnmodel.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                }
            }

            if (intBrandID != null && intBrandID > 0)
            {

                details = details.Where(x => x.BranchId == intBrandID).ToList();

                salereturnmodel = salereturnmodel.Where(x => x.BranchId == intBrandID).ToList();
            }

            if (!string.IsNullOrEmpty(searchproduct))
            {
                searchproduct = searchproduct.Trim();

                searchproduct = Helpers.Common.ChuyenThanhKhongDau(searchproduct);

                details = details.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.ProductCode).ToLower().Contains(searchproduct) || Helpers.Common.ChuyenThanhKhongDau(x.ProductName).ToLower().Contains(searchproduct)).ToList();


            }
            List<ProductViewModel> model = new List<ProductViewModel>();
            foreach (var group in details.GroupBy(x => x.ProductId))
            {
                var product = ProductRepository.GetvwProductById(group.Key.Value);
                if (product != null)
                {
                    model.Add(new ProductViewModel
                    {
                        Id = group.Key.Value,
                        QuantityTotalInventory = salereturnmodel.Where(y => y.ProductId == product.Id).Sum(x => x.Quantity),
                        Unit = group.FirstOrDefault().Unit,
                        PriceOutbound = product.PriceOutbound,
                        Quantity = group.Sum(x => x.Quantity.Value),
                        Code = product.Code,
                        Name = product.Name,
                        CategoryCode = product.CategoryCode,
                        Amount = group.Sum(x => x.Amount)
                    });
                }
            }

            //begin su ly cho truong hop tra hang khong co san pham ban trong thoi gian thong ke
            if (salereturnmodel.Count > 0)
            {
                foreach (var group in salereturnmodel.GroupBy(x => x.ProductId))
                {
                    var product = model.Where(x => x.Id == group.FirstOrDefault().ProductId);
                    if (product.Count() == 0)
                    {
                        model.Add(new ProductViewModel
                        {
                            Id = group.Key.Value,
                            QuantityTotalInventory = salereturnmodel.Where(y => y.ProductId == group.FirstOrDefault().ProductId).Sum(x => x.Quantity),
                            Unit = group.FirstOrDefault().UnitProduct,
                            PriceOutbound = group.FirstOrDefault().Price,
                            Quantity = 0,
                            Code = group.FirstOrDefault().ProductCode,
                            Name = group.FirstOrDefault().ProductName,
                            CategoryCode = group.FirstOrDefault().CategoryCode,
                            Amount = 0
                        });
                    }
                }
            }
            //end su ly cho truong hop tra hang khong co san pham ban trong thoi gian thong ke



            //phân trang trong foreach
            List<SelectListItem> items = new List<SelectListItem>();
            items.Add(new SelectListItem { Text = "10", Value = "10" });
            items.Add(new SelectListItem { Text = "25", Value = "25" });
            items.Add(new SelectListItem { Text = "50", Value = "50" });
            items.Add(new SelectListItem { Text = "100", Value = "100" });
            items.Add(new SelectListItem { Text = "200", Value = "200" });


            foreach (var item in items)
            {
                if (item.Value == size.ToString()) item.Selected = true;
            }


            ViewBag.size = items;
            ViewBag.currentSize = size;




            page = page ?? 1;

            int pageSize = (size ?? 10);


            int pageNumber = (page ?? 1);
            //end phân trang trong foreach

            ViewBag.tienhangtra = 0;
            ViewBag.sodonhangtra = 0;



            if (salereturnmodel.Count() > 0)
            {
                ViewBag.tienhangtra = salereturnmodel.Sum(x => (x.Price * x.Quantity));
                ViewBag.sodonhangtra = salereturnmodel.Sum(x => x.Quantity);
            }

            ViewBag.model = model;
            return PartialView(model.ToPagedList(pageNumber, pageSize));
        }



        //Xuat excel
        public ActionResult ExportRevenue(string start, string end, int? UserId, int? tab, string searchproduct)
        {


            DataTable dt = getData(start, end, UserId, tab, searchproduct);
            //Name of File  
            string fileName = "DoanhSo" + DateTime.Now + ".xlsx";
            using (XLWorkbook wb = new XLWorkbook())
            {
                //Add DataTable in worksheet  
                wb.Worksheets.Add(dt);
                using (MemoryStream stream = new MemoryStream())
                {
                    wb.SaveAs(stream);
                    //Return xlsx Excel File  
                    return File(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
                }
            }

        }

        public DataTable getData(string start, string end, int? UserId, int? tab, string searchproduct)
        {
            //get cookie brachID 
            HttpRequestBase request = this.HttpContext.Request;
            string strBrandID = "0";

            if (request.Cookies["BRANCH_ID_CookieName"] != null)
            {
                strBrandID = request.Cookies["BRANCH_ID_CookieName"].Value;
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


            var productinvoice = invoiceRepository.GetAllvwProductInvoice().AsEnumerable();
            var invoicedetail = invoiceRepository.GetAllvwInvoiceDetails().AsEnumerable();
            var branch_list = BranchRepository.GetAllBranch().ToList();
            var salereturn = salesReturnsRepository.GetAllSalesReturns().AsEnumerable();
            //
            //Lấy ds don hang 
            var modeldetail = invoicedetail.Select(x => new ProductInvoiceDetailViewModel
            {
                Id = x.Id,
                Price = x.Price,
                ProductId = x.ProductId,
                //PromotionDetailId = x.PromotionDetailId,
                //PromotionId = x.PromotionId,
                //PromotionValue = x.PromotionValue,
                Quantity = x.Quantity,
                Unit = x.Unit,
                ProductType = x.ProductType,
                //FixedDiscount = x.FixedDiscount,
                //FixedDiscountAmount = x.FixedDiscountAmount,
                //IrregularDiscountAmount = x.IrregularDiscountAmount,
                //IrregularDiscount = x.IrregularDiscount,
                CategoryCode = x.CategoryCode,
                ProductInvoiceCode = x.ProductInvoiceCode,
                ProductName = x.ProductName,
                ProductCode = x.ProductCode,
                ProductInvoiceId = x.ProductInvoiceId,
                ProductGroup = x.ProductGroup,
                //CheckPromotion = x.CheckPromotion,
                //IsReturn = x.IsReturn,
                //Status = x.Status,
                Amount = x.Amount,
                LoCode = x.LoCode,
                ExpiryDate = x.ExpiryDate,
                Origin = x.Origin,
                //TaxFeeAmount = x.TaxFeeAmount,
                //TaxFee = x.TaxFee,
                //Amount2 = x.Amount2,
                BranchId = x.BranchId,
               // QuantitySaleReturn = x.QuantitySaleReturn,
                CreatedDate = x.CreatedDate,
                CreatedUserId = x.CreatedUserId

            }).ToList();


            var model = productinvoice.Select(item => new ProductInvoiceViewModel
            {
                Id = item.Id,
                IsDeleted = item.IsDeleted,
                CreatedUserId = item.CreatedUserId,
                //UserName = item.UserName,
                CreatedDate = item.CreatedDate,
                //CreatedDateTemp = item.CreatedDateTemp,
                ModifiedUserId = item.ModifiedUserId,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                TotalAmount = item.TotalAmount,
                //FixedDiscount = item.FixedDiscount,
                TaxFee = item.TaxFee,
                CodeInvoiceRed = item.CodeInvoiceRed,
                Status = item.Status,
                IsArchive = item.IsArchive,
                //ProductOutboundId = item.ProductOutboundId,
                //ProductOutboundCode = item.ProductOutboundCode,
                Note = item.Note,
                CancelReason = item.CancelReason,
                BranchId = item.BranchId,
                CustomerId = item.CustomerId,
                BranchName = item.BranchName,
                //CompanyName = item.CompanyName,
                //Email = item.Email,
                CustomerPhone = item.CustomerPhone,
                //Address = item.Address,
                //DiscountTabBillAmount = item.DiscountTabBillAmount,
                //DiscountTabBill = item.DiscountTabBill,
                //isDisCountAllTab = item.isDisCountAllTab,
                //DisCountAllTab = item.DisCountAllTab,
                //Birthday = item.Birthday,
                //Gender = item.Gender,
                //Mobile = item.Mobile,
                //TienTra = item.TienTra,
                //SalerFullName = item.SalerFullName,
                //MoneyPay = item.MoneyPay,
                //TransferPay = item.TransferPay,
                //ATMPay = item.ATMPay

            }).OrderByDescending(m => m.Id).ToList();

            //Lay nguoi ban
            var user = userRepository.GetAllvwUsers();
            var usermodel = user.Select(item => new UserViewModel
            {
                Id = item.Id,
                FullName = item.LastName + " " + item.FirstName,
                BranchId = item.BranchId
            }).ToList();


            //Lay cua hang
            var branch = branch_list.Select(item => new BranchViewModel
            {
                Id = item.Id,
                Name = item.Name

            }).ToList();
            //Lấy hàng trả 
            var salereturnmodel = salereturn.Select(x => new SalesReturnsViewModel
            {
                Id = x.Id,
                BranchId = x.BranchId,
                CreatedUserId = x.CreatedUserId,
                CreatedDate = x.CreatedDate

            }).ToList();

            //Lọc theo ngày
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(start, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(end, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    model = model.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                    modeldetail = modeldetail.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                    salereturnmodel = salereturnmodel.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                }
            }

            if (intBrandID != null && intBrandID > 0)
            {
                model = model.Where(x => x.BranchId == intBrandID).ToList();
                branch = branch.Where(x => x.Id == intBrandID).ToList();
                modeldetail = modeldetail.Where(x => x.BranchId == intBrandID).ToList();
                usermodel = usermodel.Where(x => x.BranchId == intBrandID).ToList();
                salereturnmodel = salereturnmodel.Where(x => x.BranchId == intBrandID).ToList();
            }
            if (UserId != null)
            {
                model = model.Where(x => x.CreatedUserId == UserId).ToList();
                modeldetail = modeldetail.Where(x => x.CreatedUserId == UserId).ToList();
                salereturnmodel = salereturnmodel.Where(x => x.CreatedUserId == UserId).ToList();
            }

            //Creating DataTable  
            DataTable dt = new DataTable();
            //Setiing Table Name  

            if (tab == 1) //theo thoi gian
            {
                dt.TableName = "Doanh số theo đơn hàng";
                //Add Columns  
                dt.Columns.Add("Đơn hàng", typeof(string));
                dt.Columns.Add("Ngày bán", typeof(string));
                dt.Columns.Add("Thu ngân", typeof(string));
                dt.Columns.Add("Khách hàng", typeof(string));
                dt.Columns.Add("Số lượng", typeof(string));
                dt.Columns.Add("Thành tiền", typeof(string));
                dt.Columns.Add("Giảm giá", typeof(string));
                dt.Columns.Add("Tổng tiền", typeof(string));
                dt.Columns.Add("Tiền mặt", typeof(string));
                dt.Columns.Add("Chuyển khoản", typeof(string));
                dt.Columns.Add("Thẻ", typeof(string));
                //dt.Columns.Add("Trạng thái", typeof(string));
                dt.Columns.Add("Ghi chú", typeof(string));
                dt.Columns.Add("Tên hàng", typeof(string));
                dt.Columns.Add("Mã hàng", typeof(string));
                dt.Columns.Add("SL", typeof(string));
                dt.Columns.Add("Giá bán", typeof(string));
                dt.Columns.Add("Giam giá", typeof(string));
                dt.Columns.Add("Thành tiên", typeof(string));
                //Add Rows in DataTable  
                //dt.Rows.Add(1, "A12-000011266", 15, 300000);
                //dt.Rows.Add(2, "A13-000014533", 20, 200000);
                //dt.Rows.Add(3, "A13-000014535", 20, 400000);
                foreach (var item in model)
                {
                    //var sol = modeldetail.Where(x => x.ProductInvoiceId == item.Id).Sum(x => x.Quantity);
                    //var thanhtien = modeldetail.Where(x => x.ProductInvoiceId == item.Id).Sum(x => x.Quantity * x.Price);
                    //dt.Rows.Add(item.Code, item.CreatedDate.ToString(), item.SalerFullName, item.CustomerName, CommonSatic.ToCurrencyStr(sol, null), CommonSatic.ToCurrencyStr(thanhtien, null),
                    //    CommonSatic.ToCurrencyStr(thanhtien - item.TotalAmount, null), CommonSatic.ToCurrencyStr(item.TotalAmount, null),
                    //    CommonSatic.ToCurrencyStr(item.MoneyPay, null), CommonSatic.ToCurrencyStr(item.ATMPay, null), CommonSatic.ToCurrencyStr(item.TransferPay, null), item.Note);
                    //var detail = modeldetail.Where(x => x.ProductInvoiceId == item.Id).ToList();
                    //foreach (var i in detail)
                    //{
                    //    if (i.IrregularDiscountAmount != null)
                    //    {
                    //        dt.Rows.Add("", "", "", "", "", "", "", "",
                    //                "", "", "", "", i.ProductName, i.ProductCode, CommonSatic.ToCurrencyStr(i.Quantity, null), CommonSatic.ToCurrencyStr(i.Price, null),
                    //                     CommonSatic.ToCurrencyStr(i.IrregularDiscountAmount, null)
                    //                    , CommonSatic.ToCurrencyStr(i.Amount, null));
                    //    }
                    //    else
                    //    {
                    //        dt.Rows.Add("", "", "", "", "", "", "", "",
                    //            "", "", "", "", i.ProductName, i.ProductCode, CommonSatic.ToCurrencyStr(i.Quantity, null), CommonSatic.ToCurrencyStr(i.Price, null),
                    //                 "0"
                    //                , CommonSatic.ToCurrencyStr(i.Amount, null));
                    //    }
                    //}


                }
                dt.AcceptChanges();
            }

            if (tab == 2) //theo nguoi ban
            {
                dt.TableName = "Doanh số theo nhân viên";
                //Add Columns  
                dt.Columns.Add("Người bán", typeof(string));
                dt.Columns.Add("Tiền bán hàng", typeof(string));
                dt.Columns.Add("Số đơn hàng", typeof(string));
                dt.Columns.Add("Hàng hóa bán", typeof(string));
                dt.Columns.Add("Tiền trả hàng", typeof(string));
                dt.Columns.Add("Đơn hàng trả", typeof(string));
                dt.Columns.Add("Hàng hóa trả", typeof(string));
                dt.Columns.Add("Mã đơn hàng", typeof(string));
                dt.Columns.Add("Ngày", typeof(string));
                dt.Columns.Add("Khách hàng", typeof(string));
                dt.Columns.Add("SL hàng", typeof(string));
                dt.Columns.Add("Tổng tiền", typeof(string));
                //Add Rows in DataTable  

                foreach (var item in usermodel)
                {
                    //var salert = salereturn.Where(x => x.CreatedUserId == item.Id).ToList();
                    //var list = model.Where(x => x.CreatedUserId == item.Id).ToList();
                    //var detail = modeldetail.Where(x => x.CreatedUserId == item.Id).ToList();
                    //dt.Rows.Add(item.FullName, CommonSatic.ToCurrencyStr(list.Sum(x => x.TotalAmount), null), list.Count().ToString(), detail.Sum(x => x.Quantity).ToString(),
                    //    CommonSatic.ToCurrencyStr(list.Sum(x => x.TienTra), null)
                    //    , salert.Count().ToString(), detail.Sum(x => x.Quantity - x.QuantitySaleReturn).ToString());
                    //foreach (var i in list)
                    //{
                    //    var sl = modeldetail.Where(x => x.ProductInvoiceId == i.Id).Sum(x => x.Quantity);
                    //    dt.Rows.Add("", "", "", "", ""
                    //   , "", "", i.Code, i.CreatedDate, i.CustomerName, sl.ToString(), CommonSatic.ToCurrencyStr(i.TotalAmount, null));
                    //}
                }
                dt.AcceptChanges();
            }

            if (tab == 3) //theo cua hangs
            {
                dt.TableName = "Doanh số theo cửa hàng";
                //Add Columns  
                dt.Columns.Add("Cửa hàng", typeof(string));
                dt.Columns.Add("Tiền bán hàng", typeof(string));
                dt.Columns.Add("Số đơn hàng", typeof(string));
                dt.Columns.Add("Hàng hóa bán", typeof(string));
                dt.Columns.Add("Tiền trả hàng", typeof(string));
                dt.Columns.Add("Đơn hàng trả", typeof(string));
                dt.Columns.Add("Hàng hóa trả", typeof(string));
                dt.Columns.Add("Mã đơn hàng", typeof(string));
                dt.Columns.Add("Ngày", typeof(string));
                dt.Columns.Add("Khách hàng", typeof(string));
                dt.Columns.Add("SL hàng", typeof(string));
                dt.Columns.Add("Tổng tiền", typeof(string));
                //Add Rows in DataTable  
                foreach (var item in branch)
                {
                    //var salert = salereturn.Where(x => x.BranchId == item.Id).ToList();
                    //var list = model.Where(x => x.BranchId == item.Id).ToList();
                    //var detail = modeldetail.Where(x => x.BranchId == item.Id).ToList();
                    //dt.Rows.Add(item.Name, CommonSatic.ToCurrencyStr(list.Sum(x => x.TotalAmount), null), list.Count().ToString(), detail.Sum(x => x.Quantity).ToString(),
                    //    CommonSatic.ToCurrencyStr(list.Sum(x => x.TienTra), null)
                    //    , salert.Count().ToString(), detail.Sum(x => x.Quantity - x.QuantitySaleReturn).ToString());
                    //foreach (var i in list)
                    //{
                    //    var sl = modeldetail.Where(x => x.ProductInvoiceId == i.Id).Sum(x => x.Quantity);
                    //    dt.Rows.Add("", "", "", "", ""
                    //   , "", "", i.Code, i.CreatedDate, i.CustomerName, sl.ToString(), CommonSatic.ToCurrencyStr(i.TotalAmount, null));
                    //}
                }
                dt.AcceptChanges();
            }
            if (tab == 4)
            {
                List<ProductViewModel> productlist = new List<ProductViewModel>();
                if (!string.IsNullOrEmpty(searchproduct))
                {
                    searchproduct = searchproduct.Trim();

                    searchproduct = Helpers.Common.ChuyenThanhKhongDau(searchproduct);

                    modeldetail = modeldetail.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.ProductCode).ToLower().Contains(searchproduct)
                    || Helpers.Common.ChuyenThanhKhongDau(x.ProductName).ToLower().Contains(searchproduct)).ToList();


                }
                foreach (var group in modeldetail.GroupBy(x => x.ProductId))
                {
                    var product = ProductRepository.GetvwProductById(group.Key.Value);
                    if (product != null)
                    {
                        //productlist.Add(new ProductViewModel
                        //{
                        //    Id = group.Key.Value,
                        //    QuantityTotalInventory = group.Sum(x => x.QuantitySaleReturn),
                        //    Unit = group.FirstOrDefault().Unit,
                        //    PriceOutbound = product.PriceOutbound,
                        //    Quantity = group.Sum(x => x.Quantity),
                        //    Code = product.Code,
                        //    Name = product.Name,
                        //    CategoryCode = product.CategoryCode,
                        //    Amount = group.Sum(x => x.Amount)
                        //});
                    }
                }
                dt.TableName = "Doanh số theo hàng hóa";
                //Add Columns  
                dt.Columns.Add("Mã sản phẩm", typeof(string));
                dt.Columns.Add("Tên sản phẩm", typeof(string));
                dt.Columns.Add("SL bán", typeof(int));
                dt.Columns.Add("Tiền bán hàng", typeof(string));
                dt.Columns.Add("SL trả", typeof(int));
                dt.Columns.Add("Tiền trả hàng", typeof(string));
                foreach (var item in productlist)
                {
                    dt.Rows.Add(item.Code, item.Name, item.Quantity, CommonSatic.ToCurrencyStr(item.Amount, null), item.Quantity - item.QuantityTotalInventory, CommonSatic.ToCurrencyStr((item.Quantity - item.QuantityTotalInventory) * item.PriceOutbound, null));
                }

            }
            return dt;
        }


        #endregion

        #region Danh sách khách hàng cũ hẹn đặt CSD
        public ActionResult DSKHC_HenCSD(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            //get cookie brachID ]\
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


            var model = SqlHelper.QuerySP<OldCusListSkinCareViewModel>("sp_DSKHC_HenCSD", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return View(model);
        }

        public List<OldCusListSkinCareViewModel> IndexExportDSKHC_HenCSD(string startDate, string endDate, int? branchId)
        {
            startDate = string.IsNullOrEmpty(startDate) ? "" : startDate;
            endDate = string.IsNullOrEmpty(endDate) ? "" : endDate;

            DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);

            // Cộng thêm 1 tháng và trừ đi một ngày.
            DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1).AddHours(23).AddMinutes(59);

            var d_startDate = (!string.IsNullOrEmpty(startDate) == true ? DateTime.ParseExact(startDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
            var d_endDate = (!string.IsNullOrEmpty(endDate) == true ? DateTime.ParseExact(endDate, "dd/MM/yyyy", null).AddHours(23).AddMinutes(59).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

            branchId = branchId == null ? 0 : branchId;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

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

            var q = SqlHelper.QuerySP<OldCusListSkinCareViewModel>("[sp_DSKHC_HenCSD]", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                branchId = branchId
            }).ToList();

            return q;
        }

        public ActionResult ExportDSKHC_HenCSD(string StartDate, string EndDate, int? branchId, bool ExportExcel = false)
        {
            var data = IndexExportDSKHC_HenCSD(StartDate, EndDate, branchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtmlDSKHC_HenCSD(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh Sách Khách Cũ Đặt Hẹn Chăm Sóc Da");
            //if (ExportExcel)
            //{
            Response.AppendHeader("content-disposition", "attachment;filename=" + "DSKHC_HenCSD.xls");
            Response.Charset = "";
            Response.Cache.SetCacheability(HttpCacheability.NoCache);
            Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
            Response.Write(model.Content);
            Response.End();
            //}
            return View(model);
        }

        string buildHtmlDSKHC_HenCSD(List<OldCusListSkinCareViewModel> data)
        {
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Nhân Viên Quản Lý</th>\r\n";
            detailLists += "		<th>Nhóm Hưởng DS</th>\r\n";
            detailLists += "		<th>Mã Khách Hàng</th>\r\n";
            detailLists += "		<th>Tên Khách Hàng</th>\r\n";
            detailLists += "		<th>SĐT</th>\r\n";
            detailLists += "		<th>Địa Chỉ</th>\r\n";
            detailLists += "		<th>Thời Gian CSDGN</th>\r\n";
            detailLists += "		<th>Ngày Hẹn CSD</th>\r\n";
            detailLists += "		<th>Giờ Hẹn CSD</th>\r\n";
            detailLists += "		<th>Chương Trình</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {

                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NhanVienQL + "</td>\r\n"
                + "<td class=\"text-left \">" + item.NHomHuongDS + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CustomerName + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Phone + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Address + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.NgayCSDGN).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.NgayHen).ToString("dd/MM/yyyy") + "</td>\r\n"
                + "<td class=\"text-left \">" + Convert.ToDateTime(item.GioHen).ToString("hh:mm:ss tt") + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ChuongTrinh + "</td>\r\n"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion
    }
}
    