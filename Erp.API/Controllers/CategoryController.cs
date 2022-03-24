using Erp.API.Helpers;
using Erp.API.Models;
using Erp.Domain.Account.Repositories;
using Erp.Domain.Entities;
using Erp.Domain.Helper;
using Erp.Domain.Interfaces;
using Erp.Domain.Repositories;
using Erp.Domain.Sale.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Web;
using System.Web.Http;
using System.Web.Script.Serialization;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
namespace Erp.API.Controllers
{
    public class CategoryController : ApiController
    {
        //public HttpResponseMessage GetListReport()
        //{
        //    PageMenuRepository pageMenuRepository = new Domain.Repositories.PageMenuRepository(new Domain.ErpDbContext());
        //    var q = pageMenuRepository.GetPageMenus("vi-VN").Where(item => item.IsVisible == true).ToList();
        //    var model = q.Where(item => item.ParentId == 1653).OrderBy(x => x.OrderNo)
        //        .Select(item => new PageMenuViewModel
        //        {
        //            Id = item.Id,
        //            Name = item.Name,
        //            Url = item.PageUrl
        //        }).ToList();

        //    foreach (var item in model)
        //    {
        //        item.ListSubmenu = q.Where(x => x.ParentId == item.Id).OrderBy(x => x.OrderNo)
        //            .Select(x => new PageMenuViewModel
        //            {
        //                Name = x.Name,
        //                Url = x.PageUrl
        //            }).ToList();
        //    }

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(model));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}

        public HttpResponseMessage GetList(string date, string code, bool prefix = false)
        {
            if (string.IsNullOrEmpty(code))
            {
                var resp1 = new HttpResponseMessage(HttpStatusCode.OK);
                resp1.Content = new StringContent(JsonConvert.SerializeObject(null));
                resp1.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp1;
            }

            if (string.IsNullOrEmpty(date))
            {
                var resp1 = new HttpResponseMessage(HttpStatusCode.OK);
                resp1.Content = new StringContent(JsonConvert.SerializeObject(null));
                resp1.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp1;
            }

            DateTime dt = DateTime.ParseExact(date, "yyyyMMddHHmmss", null);

            CategoryRepository categoryRepository = new CategoryRepository(new Erp.Domain.ErpDbContext());
            var model = categoryRepository.GetCategoryByCode(code)
                .Where(item => item.ModifiedDate > dt)
                .Select(x => new { Id = x.Id, Name = (prefix ? x.Value + " - " + x.Name : x.Name), Code = x.Code, ParentId = x.ParentId, Value = x.Value, OrderNo = x.OrderNo == null ? 0 : x.OrderNo.Value })
                .OrderBy(x => x.OrderNo)
                .ToList();

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }

        #region Supplier
        public HttpResponseMessage GetListSupplier(string date)
        {
            if (string.IsNullOrEmpty(date))
            {
                var resp1 = new HttpResponseMessage(HttpStatusCode.OK);
                resp1.Content = new StringContent(JsonConvert.SerializeObject(null));
                resp1.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp1;
            }

            DateTime dt = DateTime.ParseExact(date, "yyyyMMddHHmmss", null);

            SupplierRepository supplierRepository = new SupplierRepository(new Domain.Sale.ErpSaleDbContext());
            var model = supplierRepository.GetAllvwSupplier()
                //.Where(item => item.ModifiedDate > dt)
                .Select(item => new
                {
                    Id = item.Id,
                    Code = item.Code,
                    Name = item.Name
                })
                .OrderBy(item => item.Name);
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }

        [HttpPost]
        public HttpResponseMessage CreateSupplier([FromBody] Domain.Sale.Entities.Supplier model)
        {
            SupplierRepository supplierRepository = new SupplierRepository(new Domain.Sale.ErpSaleDbContext());
            var Supplier = supplierRepository.GetSupplierByCode(model.Code);
            var supplier = new Domain.Sale.Entities.Supplier();
            if (Supplier != null)
            {
                //Supplier.Status = "all";
                supplierRepository.UpdateSupplier(Supplier);
                supplier = Supplier;
            }
            else
            {
                supplier.IsDeleted = false;
                supplier.CreatedDate = DateTime.Now;
                supplier.ModifiedDate = DateTime.Now;

                supplier.Code = model.Code;
                supplier.Name = model.Name;
                supplier.Phone = model.Code;
                //supplier.IsCustomer = false;
                //supplier.Status = "all";

                supplierRepository.InsertSupplier(supplier);
            }


            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(new { supplier.Id, supplier.Code, supplier.Name }));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        //b/c chi tiết bán hàng
        //public HttpResponseMessage GetListProductInvoiceDetail(string StartDate, string EndDate, int? BranchId, int? SalerId, int? SupplierId, string CategoryCode, string Manufacturer, string Size, string Material, string ProductType, string SerialNumber, string Color, string ProductCode, string CustomerType, int? CustomerId, string StatusPayment
        //    , string WarehouseSourceId)
        //{
        //    //if (string.IsNullOrEmpty(StartDate) == true && string.IsNullOrEmpty(EndDate) == true && string.IsNullOrEmpty(CategoryCode) == true
        //    // && string.IsNullOrEmpty(Manufacturer) == true && BranchId == null && SalerId == null && SupplierId == null)
        //    //    return null;

        //    WarehouseSourceId = WarehouseSourceId == null ? "" : WarehouseSourceId;
        //    SupplierId = SupplierId == null ? 0 : SupplierId;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    CustomerId = CustomerId == null ? 0 : CustomerId;
        //    CustomerType = CustomerType == null ? "" : CustomerType;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    Color = Color == null ? "" : Color;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && SupplierId == null && CustomerId == null)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var dataInbound = SqlHelper.QuerySP<BaoCaoChiTietBanHangViewModel>("spSale_BaoCaobanHang_DoanhThuChiTiet", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        BranchId = BranchId,
        //        SalerId = SalerId,
        //        SupplierId = SupplierId,
        //        CustomerId = CustomerId,
        //        CustomerType = CustomerType,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        SerialNumber = SerialNumber,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        WarehouseSourceId = WarehouseSourceId
        //    }).ToList();

        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        dataInbound = dataInbound.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Material))
        //    {
        //        dataInbound = dataInbound.Where(x => x.ProductName.Contains(Material)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(ProductType))
        //    {
        //        dataInbound = dataInbound.Where(x => x.ProductName.Contains(ProductType)).ToList();
        //    }
        //    if (StatusPayment == "no_payment")
        //    {
        //        dataInbound = dataInbound.Where(x => x.Amount > 0 && x.RemainAmount > 0 && x.Amount == x.RemainAmount).ToList();
        //    }
        //    if (StatusPayment == "part_payment")
        //    {
        //        dataInbound = dataInbound.Where(x => x.RemainAmount > 0 && x.RemainAmount < x.Amount).ToList();
        //    }
        //    if (StatusPayment == "paid")
        //    {
        //        dataInbound = dataInbound.Where(x => x.RemainAmount == 0).ToList();
        //    }
        //    foreach (var item in dataInbound)
        //    {
        //        item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    }

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(dataInbound));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        ////b/c chi tiết nhập hàng
        //public object GetListPurchaseOrderDetail(string StartDate, string EndDate, int? BranchId, string SupplierName, string CategoryCode, string Manufacturer, string WarehouseDestinationId, string Size, string Material, string ProductType, string SerialNumber, string Color, string ProductCode, string StatusPayment, string IsDeposit)
        //{
        //    if (string.IsNullOrEmpty(StartDate) == true && string.IsNullOrEmpty(EndDate) == true && string.IsNullOrEmpty(CategoryCode) == true
        //     && string.IsNullOrEmpty(Manufacturer) == true && BranchId == null && SupplierName == null)
        //        return null;
        //    SupplierName = SupplierName == null ? "" : SupplierName;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    WarehouseDestinationId = WarehouseDestinationId == null ? "" : WarehouseDestinationId;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    Color = Color == null ? "" : Color;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    IsDeposit = IsDeposit == null ? "" : IsDeposit;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var dataPurchaseOrderDetail = SqlHelper.QuerySP<BaoCaoChiTietNhapHangViewModel>("spSale_BaoCaoNhapHang", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        BranchId = BranchId,
        //        SupplierName = SupplierName,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        WarehouseDestinationId = WarehouseDestinationId,
        //        SerialNumber = SerialNumber,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        IsDeposit = IsDeposit
        //    }).ToList();
        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Material))
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.ProductName.Contains(Material)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(ProductType))
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.ProductName.Contains(ProductType)).ToList();
        //    }
        //    if (StatusPayment == "no_payment")
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.Amount == x.RemainAmount).ToList();
        //    }
        //    if (StatusPayment == "part_payment")
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.RemainAmount > 0 && x.RemainAmount < x.Amount).ToList();
        //    }
        //    if (StatusPayment == "paid")
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.RemainAmount == 0).ToList();
        //    }
        //    foreach (var item in dataPurchaseOrderDetail)
        //    {
        //        item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    }
        //    return dataPurchaseOrderDetail;
        //}
        ////b/c bàn giao sản phẩm
        //public HttpResponseMessage GetListPurchaseOrderDetailService(string StartDate, string EndDate, int? BranchId, string SupplierName, int? AssignedUserId, string CategoryCode, string Manufacturer, string WarehouseDestinationId, string Size, string Material, string ProductType, string SerialNumber, string AssignStatus, string Color, string ProductCode, int? Percent, string AdjustmentType, string IsApproved, string IsPayment)
        //{
        //    //if (string.IsNullOrEmpty(StartDate) == true && string.IsNullOrEmpty(EndDate) == true)
        //    //    return null;
        //    AssignedUserId = AssignedUserId == null ? 0 : AssignedUserId;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    Percent = Percent == null ? 0 : Percent;
        //    AdjustmentType = AdjustmentType == null ? "" : AdjustmentType;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    AssignStatus = AssignStatus == null ? "" : AssignStatus;
        //    SupplierName = SupplierName == null ? "" : SupplierName;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    WarehouseDestinationId = WarehouseDestinationId == null ? "" : WarehouseDestinationId;
        //    Color = Color == null ? "" : Color;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        //    var data = new List<BaoCaoBanGiaDichVuViewModel>();

        //    data = SqlHelper.QuerySP<BaoCaoBanGiaDichVuViewModel>("spSale_BaoCaoHangLamDichVuNhanVien", new
        //   {
        //       StartDate = d_startDate,
        //       EndDate = d_endDate,
        //       BranchId = BranchId,
        //       AssignedUserId = AssignedUserId,
        //       CategoryCode = CategoryCode,
        //       Manufacturer = Manufacturer,
        //       AssignStatus = AssignStatus,
        //       WarehouseDestinationId = WarehouseDestinationId,
        //       SerialNumber = SerialNumber,
        //       SupplierName = SupplierName,
        //       Color = Color,
        //       ProductCode = ProductCode,
        //       Percent = Percent,
        //       AdjustmentType = AdjustmentType,
        //       IsApproved = IsApproved,
        //       IsPayment = IsPayment,
        //   }).ToList();

        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Material))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Material)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(ProductType))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(ProductType)).ToList();
        //    }
        //    //if (IsApproved != null)
        //    //{
        //    //    dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.IsApproved == IsApproved).ToList();
        //    //}
        //    //if (IsPayment != null)
        //    //{
        //    //    dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.IsPayment == IsPayment).ToList();
        //    //}
        //    //foreach (var item in dataPurchaseOrderDetail)
        //    //{
        //    //    item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    //}

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        ////b/c bàn giao sản phẩm tổng hợp
        //public HttpResponseMessage GetListPurchaseOrderDetailServiceAll(string StartDate, string EndDate, int? BranchId, string SupplierName, string CategoryCode, string Manufacturer, string WarehouseDestinationId, string Size, string Material, string ProductType, string SerialNumber, string Color, string ProductCode, string AssignStatus)
        //{
        //    //if (string.IsNullOrEmpty(StartDate) == true && string.IsNullOrEmpty(EndDate) == true)
        //    //    return null;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SupplierName = SupplierName == null ? "" : SupplierName;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    WarehouseDestinationId = WarehouseDestinationId == null ? "" : WarehouseDestinationId;
        //    Color = Color == null ? "" : Color;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        //    var data = new List<BaoCaoBanGiaDichVuViewModel>();
        //    if (AssignStatus == "Chưa bàn giao")
        //    {
        //        data = SqlHelper.QuerySP<BaoCaoBanGiaDichVuViewModel>("spSale_BaoCaoHangLamDichVuChuaBanGiao")
        //            .Select(x => new BaoCaoBanGiaDichVuViewModel { 
        //                Id = x.Id,
        //                ProductName = x.ProductName ,
        //                ProductCode = x.ProductCode,
        //                TongSoLan = 0,
        //                Status = x.Status
        //            }).ToList();
        //    }
        //    else
        //    {
        //        data = SqlHelper.QuerySP<BaoCaoBanGiaDichVuViewModel>("spSale_BaoCaoHangLamDichVuNhanVienTongHop", new
        //        {
        //            StartDate = d_startDate,
        //            EndDate = d_endDate,
        //            BranchId = BranchId,
        //            CategoryCode = CategoryCode,
        //            Manufacturer = Manufacturer,
        //            WarehouseDestinationId = WarehouseDestinationId,
        //            SerialNumber = SerialNumber,
        //            SupplierName = SupplierName,
        //            Color = Color,
        //            ProductCode = ProductCode,
        //        }).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Material))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Material)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(ProductType))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(ProductType)).ToList();
        //    }
        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        ////b/c công nợ khách hàng
        //public object GetListTransactionLiabilities(string StartDate, string EndDate, string CustomerCode, string strWarehouse, int? SalerId)
        //{
        //    if (string.IsNullOrEmpty(CustomerCode) == true)
        //        CustomerCode = "";
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    int WarehouseId = 0;
        //    if (!string.IsNullOrEmpty(strWarehouse))
        //    {
        //        WarehouseRepository warehouseRepository = new WarehouseRepository(new Domain.Sale.ErpSaleDbContext());
        //        var warehouse = warehouseRepository.GetAllWarehouse().Where(x => x.Code == strWarehouse);
        //        if (warehouse.Count() > 0)
        //        {
        //            WarehouseId = warehouse.FirstOrDefault().Id;
        //        }
        //        else
        //        {
        //            return "errors";
        //        }
        //    }
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    var dataTransactionLiabilities = SqlHelper.QuerySP<BaoCaoCongNoKhachHangViewModel>("spSale_BaoCaoCongNoTongHopKHVaNCC", new
        //    {
        //        TargetCode = CustomerCode,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        WarehouseId = WarehouseId,
        //        SalerId = SalerId
        //    }).ToList();
        //    foreach (var item in dataTransactionLiabilities)
        //    {
        //        if (item.TongNoDauKy < 0)
        //        {
        //            var a = Common.PhanCachHangNgan2(item.TongNoDauKy).Replace("-", "(");
        //            item.TongNoDauKy_Text = a + ")";
        //        }
        //        else
        //        {
        //            item.TongNoDauKy_Text = Common.PhanCachHangNgan2(item.TongNoDauKy);
        //        }
        //        if (item.TongNoCuoiKy < 0)
        //        {
        //            var b = Common.PhanCachHangNgan2(item.TongNoCuoiKy).Replace("-", "(");
        //            item.TongNoCuoiKy_Text = b + ")";
        //        }
        //        else
        //        {
        //            item.TongNoCuoiKy_Text = Common.PhanCachHangNgan2(item.TongNoCuoiKy);
        //        }
        //    }
        //    return dataTransactionLiabilities;
        //}
        //public object GetListTransactionLiabilitiesCustomer(string StartDate, string EndDate, string CustomerCode, string strWarehouse, int? SalerId)
        //{
        //    if (string.IsNullOrEmpty(CustomerCode) == true)
        //        CustomerCode = "";
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    int WarehouseId = 0;
        //    if (!string.IsNullOrEmpty(strWarehouse))
        //    {
        //        WarehouseRepository warehouseRepository = new WarehouseRepository(new Domain.Sale.ErpSaleDbContext());
        //        var warehouse = warehouseRepository.GetAllWarehouse().Where(x => x.Code == strWarehouse);
        //        if (warehouse.Count() > 0)
        //        {
        //            WarehouseId = warehouse.FirstOrDefault().Id;
        //        }
        //        else
        //        {
        //            return "errors";
        //        }
        //    }
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    var dataTransactionLiabilities = SqlHelper.QuerySP<BaoCaoCongNoKhachHangViewModel>("spSale_BaoCaoCongNoKhachHang", new
        //    {
        //        TargetCode = CustomerCode,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        WarehouseId = WarehouseId,
        //        SalerId = SalerId
        //    }).Where(x => x.TargetModule == "Customer").ToList();
        //    foreach (var item in dataTransactionLiabilities)
        //    {
        //        if (item.TongNoDauKy < 0)
        //        {
        //            var a = Common.PhanCachHangNgan2(item.TongNoDauKy).Replace("-", "(");
        //            item.TongNoDauKy_Text = a + ")";
        //        }
        //        else
        //        {
        //            item.TongNoDauKy_Text = Common.PhanCachHangNgan2(item.TongNoDauKy);
        //        }
        //        if (item.TongNoCuoiKy < 0)
        //        {
        //            var b = Common.PhanCachHangNgan2(item.TongNoCuoiKy).Replace("-", "(");
        //            item.TongNoCuoiKy_Text = b + ")";
        //        }
        //        else
        //        {
        //            item.TongNoCuoiKy_Text = Common.PhanCachHangNgan2(item.TongNoCuoiKy);
        //        }
        //    }
        //    return dataTransactionLiabilities;
        //}
        //public object GetListTransactionLiabilitiesSupplier(string StartDate, string EndDate, string CustomerCode, string strWarehouse, int? SalerId)
        //{
        //    if (string.IsNullOrEmpty(CustomerCode) == true)
        //        CustomerCode = "";
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    int WarehouseId = 0;
        //    if (!string.IsNullOrEmpty(strWarehouse))
        //    {
        //        WarehouseRepository warehouseRepository = new WarehouseRepository(new Domain.Sale.ErpSaleDbContext());
        //        var warehouse = warehouseRepository.GetAllWarehouse().Where(x => x.Code == strWarehouse);
        //        if (warehouse.Count() > 0)
        //        {
        //            WarehouseId = warehouse.FirstOrDefault().Id;
        //        }
        //        else
        //        {
        //            return "errors";
        //        }
        //    }
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    var dataTransactionLiabilities = SqlHelper.QuerySP<BaoCaoCongNoKhachHangViewModel>("spSale_BaoCaoCongNoKhachHang", new
        //    {
        //        TargetCode = CustomerCode,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        WarehouseId = WarehouseId,
        //        SalerId = SalerId
        //    }).Where(x => x.TargetModule == "Supplier").ToList();
        //    foreach (var item in dataTransactionLiabilities)
        //    {
        //        if (item.TongNoDauKy < 0)
        //        {
        //            var a = Common.PhanCachHangNgan2(item.TongNoDauKy).Replace("-", "(");
        //            item.TongNoDauKy_Text = a + ")";
        //        }
        //        else
        //        {
        //            item.TongNoDauKy_Text = Common.PhanCachHangNgan2(item.TongNoDauKy);
        //        }
        //        if (item.TongNoCuoiKy < 0)
        //        {
        //            var b = Common.PhanCachHangNgan2(item.TongNoCuoiKy).Replace("-", "(");
        //            item.TongNoCuoiKy_Text = b + ")";
        //        }
        //        else
        //        {
        //            item.TongNoCuoiKy_Text = Common.PhanCachHangNgan2(item.TongNoCuoiKy);
        //        }
        //    }
        //    return dataTransactionLiabilities;
        //}
        //// b/c quản lý hàng hóa
        //public HttpResponseMessage GetBaoCaoChiTietNhapHang(string StartDate, string EndDate, int? BranchId, string WarehouseDestinationId, string CategoryCode, string Manufacturer, string Size, string Material, string ProductType, string SerialNumber, string SupplierName, string Color, string ProductCode
        //    , string OutboundStatus, string PaymentStatus, string ReceiptStatus)
        //{
        //    if (string.IsNullOrEmpty(StartDate) == true && string.IsNullOrEmpty(EndDate) == true)
        //        return null;
        //    WarehouseRepository warehouseRepository = new WarehouseRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //    WarehouseDestinationId = WarehouseDestinationId == null ? "" : WarehouseDestinationId;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    SupplierName = SupplierName == null ? "" : SupplierName;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    OutboundStatus = OutboundStatus == null ? "" : OutboundStatus;
        //    PaymentStatus = PaymentStatus == null ? "" : PaymentStatus;
        //    ReceiptStatus = ReceiptStatus == null ? "" : ReceiptStatus;

        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    Color = Color == null ? "" : Color;
        //    ProductCode = ProductCode == null ? "" : ProductCode;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var dataPurchaseOrderDetail = SqlHelper.QuerySP<QuanLyPhamTheoKhoViewModel>("spSale_LocSanPhamTheoKho", new
        //    {
        //        WarehouseDestinationId = WarehouseDestinationId,
        //        BranchId = BranchId,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        SupplierName = SupplierName,
        //        SerialNumber = SerialNumber,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        OutboundStatus = OutboundStatus,
        //        PaymentStatus = PaymentStatus,
        //        ReceiptStatus = ReceiptStatus
        //    }).ToList();

        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Material))
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.ProductName.Contains(Material)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(ProductType))
        //    {
        //        dataPurchaseOrderDetail = dataPurchaseOrderDetail.Where(x => x.ProductName.Contains(ProductType)).ToList();
        //    }

        //    //foreach (var item in dataPurchaseOrderDetail)
        //    //{
        //    //    item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    //}

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(dataPurchaseOrderDetail));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        ////b/c nhập xuất tồn
        //public HttpResponseMessage GetListNhapXuatTon(string StartDate, string EndDate, string CategoryCode, string Manufacturer, string SupplierName, string Size, string Material, string ProductType, string SerialNumber, string Color, string ProductCode, string WarehouseId)
        //{
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    SupplierName = SupplierName == null ? "" : SupplierName;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    Color = Color == null ? "" : Color;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    WarehouseId = WarehouseId == null ? "" : WarehouseId;
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var data = SqlHelper.QuerySP<BaoCaoNhapXuatTonViewModel>("spSale_BaoCaoNhapXuatTon_New", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        SerialNumber = SerialNumber,
        //        SupplierName = SupplierName,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        WarehouseId = WarehouseId
        //    }).ToList();

        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(Material))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Material)).ToList();
        //    }

        //    if (!string.IsNullOrEmpty(ProductType))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(ProductType)).ToList();
        //    }

        //    //foreach (var item in data)
        //    //{
        //    //    item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    //}

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}

        //// b/c lãi lỗ trong kinh doanh
        //public object GetList_BCLaiLoTrongKinhDoanh(string StartDate, string EndDate, string CategoryCode, string Manufacturer, int? SalerId, string WarehouseId, string Size, string Material, string ProductType, string SerialNumber, string SupplierName, string Color, string ProductCode, string IsProfit)
        //{
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    WarehouseId = WarehouseId == null ? "" : WarehouseId;
        //    SupplierName = SupplierName == null ? "" : SupplierName;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    Color = Color == null ? "" : Color;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    IsProfit = IsProfit == null ? "" : IsProfit;
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var data = SqlHelper.QuerySP<BaoCaoLaiLoTrongKinhDoanhViewModel>("spSale_BaoCaoLaiLoTrongKinhDoanh", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        SalerId = SalerId,
        //        WarehouseId = WarehouseId,
        //        SerialNumber = SerialNumber,
        //        SupplierName = SupplierName,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        IsProfit = IsProfit
        //    }).ToList();
        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Material))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(Material)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(ProductType))
        //    {
        //        data = data.Where(x => x.ProductName.Contains(ProductType)).ToList();
        //    }
        //    foreach (var item in data)
        //    {
        //        item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    }
        //    return data;
        //}
        ////b/c tổng hợp thu/chi
        //public HttpResponseMessage GetList_BCTongHopThuChi(string StartDate, string EndDate, string PaymentMethod, string Name, int? SalerId, string Category, string Code, string Phone, string LaCongNo, int? CustomerId, string WarehouseId)
        //{
        //    PaymentMethod = PaymentMethod == null ? "" : PaymentMethod;
        //    Name = Name == null ? "" : Name;
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    CustomerId = CustomerId == null ? 0 : CustomerId;

        //    WarehouseId = WarehouseId == null ? "" : WarehouseId;

        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var data = SqlHelper.QuerySP<BaoCaoTongHopThuChiViewModel>("spSale_BaoCaoTongHopThuChi", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        PaymentMethod = PaymentMethod,
        //        Name = Name,
        //        SalerId = SalerId,
        //        CustomerId = CustomerId,
        //        WarehouseId = WarehouseId
        //    }).ToList();

        //    PaymentRepository paymentRepository = new PaymentRepository(new Domain.Account.ErpAccountDbContext());
        //    ReceiptRepository receiptRepository = new ReceiptRepository(new Domain.Account.ErpAccountDbContext());
        //    double? payment_amount = 0;
        //    double? receipt_amount = 0;
        //    aDateTime = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null) : aDateTime);
        //    var payment_list = paymentRepository.GetAllPayment().Where(x => x.VoucherDate < aDateTime);
        //    var receipt_list = receiptRepository.GetAllReceipt().Where(x => x.VoucherDate < aDateTime);
        //    if (!string.IsNullOrEmpty(PaymentMethod))
        //    {
        //        payment_list = payment_list.Where(x => x.PaymentMethod == PaymentMethod);
        //        receipt_list = receipt_list.Where(x => x.PaymentMethod == PaymentMethod);
        //    }
        //    if (!string.IsNullOrEmpty(Code))
        //    {
        //        payment_list = payment_list.Where(x => x.Code.Contains(Code));
        //        receipt_list = receipt_list.Where(x => x.Code.Contains(Code));
        //        data = data.Where(x => x.Code.Contains(Code)).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(Name))
        //    {
        //        payment_list = payment_list.Where(x => x.Name == Name);
        //        receipt_list = receipt_list.Where(x => x.Name == Name);
        //    }
        //    if (!string.IsNullOrEmpty(Phone))
        //    {
        //        payment_list = payment_list.Where(x => x.Phone.Contains(Phone));
        //        receipt_list = receipt_list.Where(x => x.Phone.Contains(Phone));
        //        data = data.Where(x => x.Phone.Contains(Phone)).ToList();
        //    }
        //    if (SalerId != null && SalerId.Value > 0)
        //    {
        //        payment_list = payment_list.Where(x => x.CreatedUserId == SalerId);
        //        receipt_list = receipt_list.Where(x => x.CreatedUserId == SalerId);
        //    }
        //    if (CustomerId != null && CustomerId.Value > 0)
        //    {
        //        payment_list = payment_list.Where(x => x.SupplierId == CustomerId);
        //        receipt_list = receipt_list.Where(x => x.CustomerId == CustomerId);
        //    }
        //    if (!string.IsNullOrEmpty(LaCongNo) && LaCongNo == "true")
        //    {
        //        payment_list = payment_list.Where(x => x.LaCongNo == true);
        //        receipt_list = receipt_list.Where(x => x.LaCongNo == true);
        //        data = data.Where(x => x.LaCongNo == true).ToList();
        //    }
        //    if (!string.IsNullOrEmpty(LaCongNo) && LaCongNo == "false")
        //    {
        //        payment_list = payment_list.Where(x => x.LaCongNo == false);
        //        receipt_list = receipt_list.Where(x => x.LaCongNo == false);
        //        data = data.Where(x => x.LaCongNo == false).ToList();
        //    }

        //    if (Category == "payment")
        //    {
        //        receipt_list = null;
        //        data = data.Where(x => x.Category == "payment").ToList();
        //    }
        //    if (Category == "receipt")
        //    {
        //        payment_list = null;
        //        data = data.Where(x => x.Category == "receipt").ToList();
        //    }
        //    if (payment_list != null && payment_list.Count() > 0)
        //    {
        //        payment_amount = payment_list.Sum(x => x.Amount);
        //    }
        //    if (receipt_list != null && receipt_list.Count() > 0)
        //    {
        //        receipt_amount = receipt_list.Sum(x => x.Amount);
        //    }
        //    //tổng tiền đầu kỳ
        //    var first_amount = receipt_amount - payment_amount;
        //    for (int i = 0; i < data.Count(); i++)
        //    {
        //        if (i == 0)
        //        {
        //            data[i].FirstAmount = Convert.ToDecimal(first_amount);
        //        }
        //        else
        //        {
        //            data[i].FirstAmount = data[i - 1].LastAmount;
        //        }
        //        data[i].LastAmount = (data[i].FirstAmount - data[i].Amount_Payment + data[i].Amount_Receipt);
        //    }

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        ////b/c thu chi theo hình thức thanh toán
        //public HttpResponseMessage GetList_BCThuChiTheoHinhThucThanhToan(string StartDate, string EndDate, string SupplierName)
        //{
        //    SupplierName = SupplierName == null ? "" : SupplierName;
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var data = SqlHelper.QuerySP<BaoCaoThuChiTheoHinhThucThanhToanViewModel>("spSale_BaoCaoThuChiTheoHinhThucThanhToan", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        SupplierName = SupplierName
        //    }).ToList();
        //    //PaymentRepository paymentRepository = new PaymentRepository(new Domain.Account.ErpAccountDbContext());
        //    //ReceiptRepository receiptRepository = new ReceiptRepository(new Domain.Account.ErpAccountDbContext());

        //    //aDateTime = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null) : aDateTime);
        //    //var payment_list = paymentRepository.GetAllPayment().Where(x => x.VoucherDate < aDateTime).ToList();
        //    //var receipt_list = receiptRepository.GetAllReceipt().Where(x => x.VoucherDate < aDateTime).ToList();


        //    //for (int i = 0; i < data.Count(); i++)
        //    //{
        //    //    double? payment_amount = 0;
        //    //    double? receipt_amount = 0;
        //    //    var payment = payment_list.Where(x => x.PaymentMethod == data[i].PaymentMethod).ToList();
        //    //    if (payment.Count() > 0)
        //    //        payment_amount = payment.Sum(x => x.Amount);

        //    //    var receipt = receipt_list.Where(x => x.PaymentMethod == data[i].PaymentMethod).ToList();
        //    //    if (receipt.Count() > 0)
        //    //        receipt_amount = receipt.Sum(x => x.Amount);
        //    //    //tổng tiền đầu kỳ
        //    //    var first_amount = receipt_amount - payment_amount;
        //    //    data[i].FirstAmount = Convert.ToDecimal(first_amount);
        //    //    data[i].LastAmount = data[i].FirstAmount + data[i].Amount_Total;
        //    //}

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        ////b/c chi tiết hàng bán trả lại khách hàng
        //public object GetListSaleReturnDetail(string StartDate, string EndDate, int? BranchId, int? SalerId, string CategoryCode, string Manufacturer, string Size, string Material, string ProductType, string SerialNumber, string Color, string ProductCode, string CustomerCode
        //    , string WarehouseSourceId)
        //{
        //    //if (string.IsNullOrEmpty(StartDate) == true && string.IsNullOrEmpty(EndDate) == true && string.IsNullOrEmpty(CategoryCode) == true
        //    // && string.IsNullOrEmpty(Manufacturer) == true && BranchId == null && SalerId == null && SupplierId == null)
        //    //    return null;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    //CustomerId = CustomerId == null ? 0 : CustomerId;
        //    CustomerCode = CustomerCode == null ? "" : CustomerCode;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    Color = Color == null ? "" : Color;
        //    ProductType = ProductType == null ? "" : ProductType;
        //    Material = Material == null ? "" : Material;
        //    WarehouseSourceId = WarehouseSourceId == null ? "" : WarehouseSourceId;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(CustomerCode))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var dataInbound = SqlHelper.QuerySP<BaoCaoChiTietHangBanTraLaiViewModel>("spSale_BaoCaoChiTietHangBanTraLai", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        BranchId = BranchId,
        //        SalerId = SalerId,
        //        CustomerCode = CustomerCode,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        SerialNumber = SerialNumber,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        ProductType = ProductType,
        //        ProductMaterial = Material,
        //        WarehouseSourceId = WarehouseSourceId
        //    }).ToList();

        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        dataInbound = dataInbound.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }

        //    foreach (var item in dataInbound)
        //    {
        //        item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    }

        //    return dataInbound;
        //}

        ////b/c chi tiết hàng bán trả lại nhà cung cấp
        //public HttpResponseMessage GetListPurchaseReturnDetail(string StartDate, string EndDate, int? BranchId, int? SalerId, string CategoryCode, string Manufacturer, string Size, string Material, string ProductType, string SerialNumber, string Color, string ProductCode, string SupplierCode, string WarehouseSourceId)
        //{
        //    //if (string.IsNullOrEmpty(StartDate) == true && string.IsNullOrEmpty(EndDate) == true && string.IsNullOrEmpty(CategoryCode) == true
        //    // && string.IsNullOrEmpty(Manufacturer) == true && BranchId == null && SalerId == null && SupplierId == null)
        //    //    return null;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    //CustomerId = CustomerId == null ? 0 : CustomerId;
        //    SupplierCode = SupplierCode == null ? "" : SupplierCode;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    Color = Color == null ? "" : Color;
        //    ProductType = ProductType == null ? "" : ProductType;
        //    Material = Material == null ? "" : Material;
        //    WarehouseSourceId = WarehouseSourceId == null ? "" : WarehouseSourceId;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(SupplierCode))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var dataInbound = SqlHelper.QuerySP<BaoCaoChiTietTraHangNCCViewModel>("spSale_BaoCaoChiTietTraHangNCC", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        BranchId = BranchId,
        //        SalerId = SalerId,
        //        SupplierCode = SupplierCode,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        SerialNumber = SerialNumber,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        ProductType = ProductType,
        //        ProductMaterial = Material,
        //        WarehouseSourceId = WarehouseSourceId
        //    }).ToList();
        //    if (!string.IsNullOrEmpty(Size))
        //    {
        //        dataInbound = dataInbound.Where(x => x.ProductName.Contains(Size)).ToList();
        //    }

        //    //foreach (var item in dataInbound)
        //    //{
        //    //    item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");
        //    //}

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(dataInbound));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}

        ////b/c bán hàng theo nhân viên
        //public object GetListSaleStaffs(int? SalerId, string StartDate, string EndDate)
        //{
        //    if (SalerId == null)
        //    {
        //        SalerId = 0;
        //    }
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        //    var dataSaleStaffs = SqlHelper.QuerySP<BaoCaoBanHangTheoNhanVien>("spSale_BaoCaoBanHangTheoNhanVien", new
        //    {
        //        SalerId = SalerId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //    }).ToList();
        //    return dataSaleStaffs;
        //}
        //// b/c bán hàng theo tháng
        //public object GetListSaleByMonth(int? Year)
        //{
        //    if (Year == null)
        //    {
        //        Year = DateTime.Now.Year;
        //    }
        //    ////    return null;
        //    //DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    //// Cộng thêm 1 tháng và trừ đi một ngày.
        //    //DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    //var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    //var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        //    var dataSaleByMonth = SqlHelper.QuerySP<BaoCaoBanHangTheoThang>("spSale_BaoCaoBanHangTheoThang", new
        //    {
        //        Year = Year
        //    }).ToList();
        //    return dataSaleByMonth;
        //}
        //// b/c doanh thu theo khách hàng
        //public object GetListReceiptCustomer(string CustomerCode, string StartDate, string EndDate, string MinAmount, string MaxAmount)
        //{
        //    if (string.IsNullOrEmpty(CustomerCode))
        //    {
        //        CustomerCode = "";
        //    }
        //    //    return null;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    // Cộng thêm 1 tháng và trừ đi một ngày.
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    var d_startDate = (StartDate != null ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : aDateTime.ToString("yyyy-MM-dd HH:mm:ss"));
        //    var d_endDate = (EndDate != null ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : retDateTime.ToString("yyyy-MM-dd HH:mm:ss"));

        //    var dataSaleStaffs = SqlHelper.QuerySP<BaoCaoDoanhThuTheoKhachHang>("spSale_BaoCaoDoanhThuTheoKhachHang", new
        //    {
        //        CustomerCode = CustomerCode,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        MinAmount = MinAmount,
        //        MaxAmount = MaxAmount
        //    }).ToList();
        //    return dataSaleStaffs;
        //}

        //// b/c kết quả bán hàng theo tháng
        //public object GetListSaleResultByMonth(int? Month, int? Year, int? WarehouseId, int? SupplierId, string ProductCode, string Category, string ProductType, string Color)
        //{

        //    //    return null;

        //    var dataSaleStaffs = SqlHelper.QuerySP<BaoCaoDoanhThuTheoKhachHang>("spSale_BaoCaoDoanhThuTheoKhachHang", new
        //    {

        //    }).ToList();
        //    return dataSaleStaffs;
        //}

        //public object GetListSale_BaoCaoDoanhThuTheoKhachHang(string CustomerCode, string StartDate, string EndDate, decimal? MinAmount, decimal? MaxAmount)
        //{
        //    CustomerCode = CustomerCode == null ? "" : CustomerCode;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;
        //    MinAmount = MinAmount == null ? 0 : MinAmount;
        //    MaxAmount = MaxAmount == null ? 0 : MaxAmount;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(CustomerCode))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoDoanhThuTheoKhachHangViewModel>("spSale_BaoCaoDoanhThuTheoKhachHang", new
        //    {
        //        CustomerCode = CustomerCode,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        MinAmount = MinAmount,
        //        MaxAmount = MaxAmount
        //    }).ToList();
        //    return data;
        //}
        //public object GetListSale_BaoCaoBanHangTheoChiNhanh(int? WarehouseId, string StartDate, string EndDate)
        //{
        //    WarehouseId = WarehouseId == null ? 0 : WarehouseId;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(WarehouseId.ToString()))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoBanHangTheoChiNhanhViewModel>("spSale_BaoCaoBanHangTheoChiNhanh", new
        //    {
        //        WarehouseId = WarehouseId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();
        //    return data;
        //}
        //public object GetListSale_BaoCaoBanHangTheoNhanVien(int? SalerId, string StartDate, string EndDate)
        //{
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && SalerId == null)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoBanHangTheoNhanVienViewModel>("spSale_BaoCaoBanHangTheoNhanVien", new
        //    {
        //        SalerId = SalerId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();
        //    return data;
        //}
        //public object GetListSale_BaoCaoBanHangTheoThang(int? Year)
        //{
        //    Year = Year == null ? 0 : Year;


        //    var data = SqlHelper.QuerySP<Sale_BaoCaoBanHangTheoThangViewModel>("spSale_BaoCaoBanHangTheoThang", new
        //    {
        //        Year = Year
        //    }).ToList();
        //    return data;
        //}

        //public object GetListSale_BaoCaoChiTietTraDiem(string StartDate, string EndDate, string CustomerCode, string CustomerName)
        //{
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;
        //    CustomerCode = CustomerCode == null ? "" : CustomerCode;
        //    CustomerName = CustomerName == null ? "" : CustomerName;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(CustomerCode))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoChiTietTraDiemViewModel>("spSale_BaoCaoChiTietTraDiem", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        CustomerCode = CustomerCode,
        //        CustomerName = CustomerName
        //    }).ToList();
        //    return data;
        //}


        //public object GetListSale_BaoCaoThongkeDiemKhachHang(string CustomerName, string StartDate, string EndDate)
        //{
        //    CustomerName = CustomerName == null ? "" : CustomerName;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(CustomerName))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoThongkeDiemKhachHangViewModel>("spSale_BaoCaoThongkeDiemKhachHang", new
        //    {
        //        CustomerName = CustomerName,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();
        //    return data;
        //}


        //public object GetListSale_BaoCaoKetQuaBanHangTheoThang(int? Month, int? Year, int? WarehouseId, int? SupplierId, string ProductCode, string Category, string Manufacturer, string ProductType, string Color, string Material, string Size, string SerialNumber)
        //{
        //    Month = Month == null ? 0 : Month;
        //    Year = Year == null ? 0 : Year;
        //    WarehouseId = WarehouseId == null ? 0 : WarehouseId;
        //    SupplierId = SupplierId == null ? 0 : SupplierId;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    Category = Category == null ? "" : Category;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    ProductType = ProductType == null ? "" : ProductType;
        //    Color = Color == null ? "" : Color;
        //    Material = Material == null ? "" : Material;
        //    Size = Size == null ? "" : Size;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;


        //    var data = SqlHelper.QuerySP<Sale_BaoCaoKetQuaBanHangTheoThangViewModel>("spSale_BaoCaoKetQuaBanHangTheoThang", new
        //    {
        //        Month = Month,
        //        Year = Year,
        //        WarehouseId = WarehouseId,
        //        SupplierId = SupplierId,
        //        ProductCode = ProductCode,
        //        Category = Category,
        //        Manufacturer = Manufacturer,
        //        ProductType = ProductType,
        //        Color = Color,
        //        Material = Material,
        //        Size = Size,
        //        SerialNumber = SerialNumber
        //    }).ToList();
        //    return data;
        //}


        //public object GetListSale_BaoCaoKetQuaBanHangTheoTungNgay(int? WarehouseId, string StartDate, string EndDate)
        //{
        //    WarehouseId = WarehouseId == null ? 0 : WarehouseId;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;
        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && WarehouseId == null)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");


        //    var data = SqlHelper.QuerySP<Sale_BaoCaoKetQuaBanHangTheoTungNgayViewModel>("spSale_BaoCaoKetQuaBanHangTheoTungNgay", new
        //    {
        //        WarehouseId = WarehouseId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();
        //    return data;
        //}
        //public object GetListSale_BaoCaoTongHopTichDiem(string CustomerCode, string CustomerName, string StartDate, string EndDate)
        //{
        //    CustomerCode = CustomerCode == null ? "" : CustomerCode;
        //    CustomerName = CustomerName == null ? "" : CustomerName;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(CustomerCode))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoTongHopTichDiemViewModel>("spSale_BaoCaoTongHopTichDiem", new
        //    {
        //        CustomerCode = CustomerCode,
        //        CustomerName = CustomerName,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();

        //    return data;
        //}

        //public object GetListSale_BaoCaoKhachHangNoQuaDinhMuc(string SupplierCode, string Date)
        //{
        //    SupplierCode = SupplierCode == null ? "" : SupplierCode;
        //    Date = Date == null ? "" : Date;
        //    DateTime start_d;
        //    if (DateTime.TryParseExact(Date, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out start_d))
        //    {
        //        start_d = start_d.AddHours(23);
        //    }
        //    var date = start_d.ToString("dd/MM/yyyy HH:mm");
        //    var d_Date = (!string.IsNullOrEmpty(date) ? DateTime.ParseExact(date.ToString(), "dd/MM/yyyy HH:mm", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoKhachHangNoQuaDinhMucViewModel>("spSale_BaoCaoKhachHangNoQuaDinhMuc", new
        //    {
        //        SupplierCode = SupplierCode,
        //        Date = d_Date
        //    }).ToList();
        //    return data;
        //}


        //public object GetListSale_BaoCaoTongHopThuChiTheoNhanVien(int? UserId, string StartDate, string EndDate)
        //{
        //    UserId = UserId == null ? 0 : UserId;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoTongHopThuChiTheoNhanVienViewModel>("spSale_BaoCaoTongHopThuChiTheoNhanVien", new
        //    {
        //        UserId = UserId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();
        //    return data;
        //}
        //public object GetListSale_BaoCaoCongNoTheoNhanVien(int? SalerId, int? WarehouseId, string StartDate, string EndDate)
        //{
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    WarehouseId = WarehouseId == null ? 0 : WarehouseId;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && SalerId == null)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoCongNoTheoNhanVienViewModel>("spSale_BaoCaoCongNoTheoNhanVien", new
        //    {
        //        SalerId = SalerId,
        //        WarehouseId = WarehouseId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();
        //    return data;
        //}

        //public object GetListSale_BaoCaoHangHoaTheoHoaDon(string CustomerCode, int? WarehouseSourceId, int? SalerId, string StartDate, string EndDate)
        //{
        //    CustomerCode = CustomerCode == null ? "" : CustomerCode;
        //    WarehouseSourceId = WarehouseSourceId == null ? 0 : WarehouseSourceId;
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && string.IsNullOrEmpty(CustomerCode))
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoHangHoaTheoHoaDonViewModel>("spSale_BaoCaoHangHoaTheoHoaDon", new
        //    {
        //        CustomerCode = CustomerCode,
        //        WarehouseSourceId = WarehouseSourceId,
        //        SalerId = SalerId,
        //        StartDate = d_startDate,
        //        EndDate = d_endDate
        //    }).ToList();
        //    return data;
        //}


        //public object GetListBanGiaoTien(string StartDate, string EndDate, int? SalerId)
        //{
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;
        //    SalerId = SalerId == null ? 0 : SalerId;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<BanGiaoTienViewModel>("spBanGiaoTien", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        SalerId = SalerId
        //    }).ToList();
        //    return data;
        //}


        //public HttpResponseMessage GetListSale_BaoCaoGiamGiaSauBanHang(string StartDate, string EndDate, int? BranchId, int? SalerId, int? CustomerId, string CategoryCode, string Manufacturer, string SerialNumber, string Color, string ProductCode, string ProductType, string ProductMaterial, string Size, string WarehouseSourceId)
        //{
        //    StartDate = StartDate == null ? "" : StartDate;
        //    EndDate = EndDate == null ? "" : EndDate;
        //    BranchId = BranchId == null ? 0 : BranchId;
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    CustomerId = CustomerId == null ? 0 : CustomerId;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    Color = Color == null ? "" : Color;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    ProductType = ProductType == null ? "" : ProductType;
        //    ProductMaterial = ProductMaterial == null ? "" : ProductMaterial;
        //    Size = Size == null ? "" : Size;
        //    WarehouseSourceId = WarehouseSourceId == null ? "" : WarehouseSourceId;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (StartDate == null && EndDate == null && CustomerId <= 0)
        //    {
        //        StartDate = aDateTime.ToString("dd/MM/yyyy");
        //        EndDate = retDateTime.ToString("dd/MM/yyyy");
        //    }
        //    var d_startDate = (!string.IsNullOrEmpty(StartDate) ? DateTime.ParseExact(StartDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_endDate = (!string.IsNullOrEmpty(EndDate) ? DateTime.ParseExact(EndDate.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

        //    var data = SqlHelper.QuerySP<Sale_BaoCaoGiamGiaSauBanHangViewModel>("spSale_BaoCaoGiamGiaSauBanHang", new
        //    {
        //        StartDate = d_startDate,
        //        EndDate = d_endDate,
        //        BranchId = BranchId,
        //        SalerId = SalerId,
        //        CustomerId = CustomerId,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        SerialNumber = SerialNumber,
        //        Color = Color,
        //        ProductCode = ProductCode,
        //        ProductType = ProductType,
        //        ProductMaterial = ProductMaterial,
        //        Size = Size,
        //        WarehouseSourceId = WarehouseSourceId
        //    }).ToList();

        //    //foreach (var item in data)
        //    //{
        //    //    item.ProductImage = Erp.API.Helpers.Common.KiemTraTonTaiAnhKhacUrl(item.ProductImage, "upload_path_PurchaseOrder", "product");

        //    //}

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}


        //public HttpResponseMessage GetListSale_BaoCaoHangDatCoc(string Start, string End, int? SalerId, int? CustomerId, string ProductCode, string Code, string CategoryCode, string Manufacturer, string SerialNumber, string Color, string ProductType, string Material)
        //{
        //    SalerId = SalerId == null ? 0 : SalerId;
        //    CustomerId = CustomerId == null ? 0 : CustomerId;
        //    ProductCode = ProductCode == null ? "" : ProductCode;
        //    Code = Code == null ? "" : Code;
        //    CategoryCode = CategoryCode == null ? "" : CategoryCode;
        //    Manufacturer = Manufacturer == null ? "" : Manufacturer;
        //    SerialNumber = SerialNumber == null ? "" : SerialNumber;
        //    Color = Color == null ? "" : Color;
        //    ProductType = ProductType == null ? "" : ProductType;
        //    Material = Material == null ? "" : Material;

        //    DateTime aDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
        //    DateTime retDateTime = aDateTime.AddMonths(1).AddDays(-1);
        //    if (Start == null && End == null)
        //    {
        //        Start = aDateTime.ToString("dd/MM/yyyy");
        //        End = retDateTime.ToString("dd/MM/yyyy");
        //    }

        //    var d_start = (!string.IsNullOrEmpty(Start) ? DateTime.ParseExact(Start.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
        //    var d_end = (!string.IsNullOrEmpty(End) ? DateTime.ParseExact(End.ToString(), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");



        //    var data = SqlHelper.QuerySP<Sale_BaoCaoHangDatCocViewModel>("spSale_BaoCaoHangDatCoc", new
        //    {
        //        Start = d_start,
        //        End = d_end,
        //        SalerId = SalerId,
        //        CustomerId = CustomerId,
        //        ProductCode = ProductCode,
        //        Code = Code,
        //        CategoryCode = CategoryCode,
        //        Manufacturer = Manufacturer,
        //        SerialNumber = SerialNumber,
        //        Color = Color,
        //        ProductType = ProductType,
        //        Material = Material
        //    }).ToList();

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}


        //<append_content_action_here>
    }
}