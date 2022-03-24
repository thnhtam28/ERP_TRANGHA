﻿using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Text;
using System.Web.Mvc;
using Erp.Utilities;
using WebMatrix.WebData;
using Erp.BackOffice.Helpers;
using System.Web.Script.Serialization;
using Erp.Domain.Sale.Repositories;
using System.Data.Entity;
using System.Transactions;
using Erp.BackOffice.Areas.Cms.Models;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class InventoryMaterialController : Controller
    {
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IMaterialOrServiceRepository MaterialRepository;
        private readonly IInventoryMaterialRepository InventoryMaterialRepository;
        private readonly IMaterialInboundRepository MaterialInboundRepository;
        private readonly IMaterialOutboundRepository MaterialOutboundRepository;
        private readonly IPhysicalInventoryMaterialRepository PhysicalInventoryMaterialRepository;
        private readonly IUserRepository userRepository;
        public InventoryMaterialController(
            IInventoryMaterialRepository _InventoryMaterial
            , IMaterialOrServiceRepository _Material
            , IWarehouseRepository _Warehouse
            , IMaterialInboundRepository _MaterialInbound
            , IMaterialOutboundRepository _MaterialOutbound
            , IPhysicalInventoryMaterialRepository _PhysicalInventoryMaterial
            , IUserRepository _user
            )
        {
            WarehouseRepository = _Warehouse;
            MaterialRepository = _Material;
            InventoryMaterialRepository = _InventoryMaterial;
            MaterialInboundRepository = _MaterialInbound;
            MaterialOutboundRepository = _MaterialOutbound;
            userRepository = _user;
            PhysicalInventoryMaterialRepository = _PhysicalInventoryMaterial;

        }

        #region Index
        public ViewResult Index(int? WarehouseId, string txtCode, string conHang, string category, string txtSearch, string manufacturer, int? page, int? BranchId, string txtInfo)
        {
            WarehouseId = WarehouseId == null ? 0 : WarehouseId;
            category = category == null ? "" : category;
            txtSearch = txtSearch == null ? "" : txtSearch;
            txtCode = txtCode == null ? "" : txtCode;
            txtInfo = txtInfo == null ? "" : txtInfo;
            manufacturer = manufacturer == null ? "" : manufacturer;
            BranchId = BranchId == null ? 0 : BranchId;
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
            if (string.IsNullOrEmpty(conHang))
            {
                conHang = "1";
            }

            if (!Erp.BackOffice.Filters.SecurityFilter.IsAdmin())
            {
                BranchId = Erp.BackOffice.Helpers.Common.CurrentUser.BranchId;
            }
            var listInventory = Domain.Helper.SqlHelper.QuerySP<InventoryMaterialViewModel>("spSale_Get_InventoryMaterial", new { WarehouseId = WarehouseId, HasQuantity = conHang, MaterialCode = txtCode, MaterialName = txtSearch, CategoryCode = category, ProductGroup = "", BranchId = BranchId, LoCode = "", MaterialId = "", ExpiryDate = "" });
            var listProduct = Domain.Helper.SqlHelper.QuerySP<InventoryMaterialViewModel>("spSale_Get_ListMaterialFromInventoryMaterial", new { WarehouseId = WarehouseId, HasQuantity = conHang, MaterialCode = txtCode, MaterialName = txtSearch, CategoryCode = category, ProductGroup = "", BranchId = BranchId, CityId = "", DistrictId = "" });
            var warehousecategoryList = Domain.Helper.SqlHelper.QuerySP<CategoryViewModel>("GetAllDanhMucKho").Where(x => x.CategoryValue == "VT" && x.BranchId == BranchId).ToList();
            var warehouseList = new List<WarehouseViewModel>();

            warehouseList = WarehouseRepository.GetAllWarehouse()
                .Select(item => new WarehouseViewModel
                {
                    Id = item.Id,
                    Name = item.Name,
                    Code = item.Code,
                    Address = item.Address,
                    Note = item.Note,
                    BranchId = item.BranchId,
                    Categories = item.Categories
                }).OrderBy(x => x.Name).ToList();

            warehouseList = warehouseList.Where(x => x.Categories == "VT" && x.BranchId == BranchId).ToList();
            if (WarehouseId != null && WarehouseId > 0)
            {
                warehouseList = warehouseList.Where(x =>x.Id == WarehouseId).ToList();
            }
            if (!string.IsNullOrEmpty(manufacturer))
            {
                listInventory = listInventory.Where(x => x.Manufacturer == manufacturer).ToList();
                listProduct = listProduct.Where(x => x.Manufacturer == manufacturer).ToList();
            }
            //gộp textbox
            if (!string.IsNullOrEmpty(txtInfo))
            {
                listInventory = listInventory.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.MaterialName).Contains(Helpers.Common.ChuyenThanhKhongDau(txtInfo)) || x.MaterialCode.ToLower().Contains(Helpers.Common.ChuyenThanhKhongDau(txtInfo))).ToList();
                listProduct = listProduct.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.MaterialName).Contains(Helpers.Common.ChuyenThanhKhongDau(txtInfo)) || x.MaterialCode.ToLower().Contains(Helpers.Common.ChuyenThanhKhongDau(txtInfo))).ToList();
            }
            ViewBag.inventoryList = listInventory.ToList();
            ViewBag.listProduct = listProduct.ToList();
            ViewBag.warehousecategoryList = warehousecategoryList;
            ViewBag.warehouseList = warehouseList.ToList();
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];

            return View();
        }
        #endregion

        #region Detail
        public ActionResult Detail(int? Id, int? WarehouseId, string LoCode, int? day, int? month, int? year)
        {
            var Product = MaterialRepository.GetvwMaterialById(Id.Value);
            if (Product != null && Product.IsDeleted != true)
            {
                var model = new MaterialViewModel();
                AutoMapper.Mapper.Map(Product, model);

                var inboundDetails = MaterialInboundRepository.GetAllvwMaterialInboundDetailByMaterialId(Id.Value).AsEnumerable()
                    .Where(item => item.IsArchive && item.LoCode == LoCode);
                var outboundDetails = MaterialOutboundRepository.GetAllvwMaterialOutboundDetailByMaterialId(Id.Value).AsEnumerable()
                     .Where(item => item.IsArchive && item.LoCode == LoCode);

                if (WarehouseId != null && WarehouseId > 0)
                {
                    inboundDetails = inboundDetails.Where(x => x.WarehouseDestinationId == WarehouseId);
                    outboundDetails = outboundDetails.Where(x => x.WarehouseSourceId == WarehouseId);
                }
                if (day != null && month != null && year != null)
                {
                    inboundDetails = inboundDetails.Where(x => x.ExpiryDate != null && x.ExpiryDate.Value.Day == day && x.ExpiryDate.Value.Month == month && x.ExpiryDate.Value.Year == year);
                    outboundDetails = outboundDetails.Where(x => x.ExpiryDate != null && x.ExpiryDate.Value.Day == day && x.ExpiryDate.Value.Month == month && x.ExpiryDate.Value.Year == year);
                }
                else
                {
                    inboundDetails = inboundDetails.Where(x => x.ExpiryDate == null);
                    outboundDetails = outboundDetails.Where(x => x.ExpiryDate == null);
                }
                inboundDetails = inboundDetails.OrderBy(x => x.MaterialInboundId);
                outboundDetails = outboundDetails.OrderBy(x => x.MaterialOutboundId);
                ViewBag.inboundDetails = inboundDetails;
                ViewBag.outboundDetails = outboundDetails;

                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        #endregion

        #region Json
        public JsonResult GetListMaterialJsonByWarehouseId(int? warehouseId)
        {
            if (warehouseId == null)
                return Json(new List<int>(), JsonRequestBehavior.AllowGet);

            var list = Erp.Domain.Helper.SqlHelper.QuerySP<InventoryMaterialViewModel>("spSale_Get_InventoryMaterial", new { WarehouseId = warehouseId, HasQuantity = "1", MaterialCode = "", MaterialName = "", CategoryCode = "", ProductGroup = "", BranchId = "", LoCode = "", MaterialId = "", ExpiryDate = "" }).ToList();
            var json_data = list.Select(item => new
            {
                item.Id,
                Code = item.MaterialCode,
                Image = Helpers.Common.KiemTraTonTaiHinhAnh(item.Image_Name, "Material", "product"),
                Unit = item.MaterialUnit,
                Price = item.MaterialPriceOutbound,
                Text = item.MaterialCode + " - " + item.MaterialName + " (" + Helpers.Common.PhanCachHangNgan2(item.MaterialPriceOutbound) + "/" + item.MaterialUnit + ")",
                Name = item.MaterialName,
                Value = item.MaterialId,
                QuantityTotalInventory = item.Quantity,
                LoCode = item.LoCode,
                CategoryCode = item.CategoryCode,
                ExpiryDate = (item.ExpiryDate == null ? "" : item.ExpiryDate.Value.ToString("dd/MM/yyyy")),
                Note ="SL:" + item.Quantity + "  Lô:" + item.LoCode + "  HSD:" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd/MM/yyyy") : "")
            });
            return Json(json_data, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Check or Update
        public static string Check(string materialName, int materialId, string LoCode, DateTime? ExpiryDate, int warehouseId, int quantityIn, int quantityOut)
        {

            return Update(materialName, materialId, LoCode, ExpiryDate, warehouseId, quantityIn, quantityOut, false);

        }


        public static string Check_mobile(string materialName, int materialId, string LoCode, DateTime? ExpiryDate, int warehouseId, int quantityIn, int quantityOut, int pCurrentUserId)
        {
            return Update_mobile(materialName, materialId, LoCode, ExpiryDate, warehouseId, quantityIn, quantityOut, pCurrentUserId, false);
        }

        public static string Update(string materialName, int materialId, string LoCode, DateTime? ExpiryDate, int warehouseId, int quantityIn, int quantityOut, bool isArchive = true)
        {
            string error = "";
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                               new System.TimeSpan(0, 30, 0)))
            {
                try
                {
                    MaterialInboundRepository materialInboundRepository = new MaterialInboundRepository(new Domain.Sale.ErpSaleDbContext());
                    MaterialOutboundRepository materialOutboundRepository = new Domain.Sale.Repositories.MaterialOutboundRepository(new Domain.Sale.ErpSaleDbContext());
                    InventoryMaterialRepository inventoryMaterialRepository = new Domain.Sale.Repositories.InventoryMaterialRepository(new Domain.Sale.ErpSaleDbContext());
                    LoCode = LoCode == null ? "" : LoCode;
                    var d_ExpiryDate = (ExpiryDate != null ? DateTime.ParseExact(ExpiryDate.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");
                    var inbound = Domain.Helper.SqlHelper.QuerySP<vwMaterialInboundDetail>("spSale_GetMaterialInboundDetail", new
                    {
                        MaterialId = materialId,
                        LoCode = LoCode,
                        ExpiryDate = d_ExpiryDate,
                        WarehouseDestinationId = warehouseId
                    }).ToList();

                    var outbound = Domain.Helper.SqlHelper.QuerySP<vwMaterialOutboundDetail>("spSale_GetMaterialOutboundDetail", new
                    {
                        MaterialId = materialId,
                        LoCode = LoCode,
                        ExpiryDate = d_ExpiryDate,
                        WarehouseSourceId = warehouseId
                    });
                    var inventoryMaterialCurrent_List = Erp.Domain.Helper.SqlHelper.QuerySP<InventoryMaterial>("spSale_Get_InventoryMaterial",
                             new
                             {
                                 WarehouseId = warehouseId,
                                 HasQuantity = "1",
                                 MaterialCode = "",
                                 MaterialName = "",
                                 CategoryCode = "",
                                 ProductGroup = "",
                                 BranchId = "",
                                 LoCode = LoCode,
                                 MaterialId = materialId,
                                 ExpiryDate = ExpiryDate
                             }).ToList();
                    if (string.IsNullOrEmpty(LoCode) || LoCode == "")
                    {
                        inbound = inbound.Where(x => string.IsNullOrEmpty(x.LoCode) || x.LoCode == "").ToList();
                        outbound = outbound.Where(x => string.IsNullOrEmpty(x.LoCode) || x.LoCode == "").ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => string.IsNullOrEmpty(x.LoCode)).ToList();
                    }
                    else
                    {
                        inbound = inbound.Where(x => x.LoCode == LoCode).ToList();
                        outbound = outbound.Where(x => x.LoCode == LoCode).ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => x.LoCode == LoCode).ToList();
                    }
                    if (ExpiryDate == null)
                    {
                        inbound = inbound.Where(x => x.ExpiryDate == null).ToList();
                        outbound = outbound.Where(x => x.ExpiryDate == null).ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => x.ExpiryDate == null).ToList();
                    }
                    else
                    {
                        inbound = inbound.Where(x => x.ExpiryDate == ExpiryDate).ToList();
                        outbound = outbound.Where(x => x.ExpiryDate == ExpiryDate).ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => x.ExpiryDate == ExpiryDate).ToList();
                    }
                    var qty_inbound = inbound.Sum(x => x.Quantity);
                    var qty_outbound = outbound.Sum(x => x.Quantity);
                    var inventoryMaterial = (inbound.Count() > 0 ? qty_inbound : 0) - (outbound.Count() > 0 ? qty_outbound : 0) + quantityIn - quantityOut;


                    for (int i = 0; i < inventoryMaterialCurrent_List.Count(); i++)
                    {
                        if (i > 0)
                        {
                            var id = inventoryMaterialCurrent_List[i].Id;
                            inventoryMaterialRepository.DeleteInventoryMaterial(id);
                        }
                    }
                    //Sau khi thay đổi dữ liệu phải đảm bảo tồn kho >= 0
                    if (inventoryMaterial >= 0)//inventory >= 0)
                    {
                        if (isArchive)
                        {
                            if (inventoryMaterialCurrent_List.Count() > 0)
                            {
                                if (inventoryMaterialCurrent_List[0].Quantity != inventoryMaterial)
                                {
                                    inventoryMaterialCurrent_List[0].Quantity = inventoryMaterial;
                                    inventoryMaterialRepository.UpdateInventoryMaterial(inventoryMaterialCurrent_List[0]);
                                }
                            }
                            else
                            {
                                var insert = new InventoryMaterial();
                                insert.IsDeleted = false;
                                insert.CreatedUserId = WebSecurity.CurrentUserId;
                                insert.ModifiedUserId = WebSecurity.CurrentUserId;
                                insert.CreatedDate = DateTime.Now;
                                insert.ModifiedDate = DateTime.Now;
                                insert.WarehouseId = warehouseId;
                                insert.MaterialId = materialId;
                                insert.Quantity = inventoryMaterial;
                                insert.LoCode = LoCode;
                                insert.ExpiryDate = ExpiryDate;
                                inventoryMaterialRepository.InsertInventoryMaterial(insert);
                            }
                        }
                    }
                    else
                    {
                        error += string.Format("<br/>Dữ liệu tồn kho của sản phẩm \"{0}\" là {1}: không hợp lệ!", materialName, inventoryMaterial);
                    }

                    scope.Complete();
                }
                catch (DbUpdateException)
                {
                }
            }
            return error;
        }



        public static string Update_mobile(string materialName, int materialId, string LoCode, DateTime? ExpiryDate, int warehouseId, int quantityIn, int quantityOut, int pCurrentUserId, bool isArchive = true)
        {
            string error = "";
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                new System.TimeSpan(0, 30, 0)))
            {
                try
                {

                    MaterialInboundRepository materialInboundRepository = new MaterialInboundRepository(new Domain.Sale.ErpSaleDbContext());
                    MaterialOutboundRepository materialtOutboundRepository = new Domain.Sale.Repositories.MaterialOutboundRepository(new Domain.Sale.ErpSaleDbContext());
                    InventoryMaterialRepository inventoryMaterialRepository = new Domain.Sale.Repositories.InventoryMaterialRepository(new Domain.Sale.ErpSaleDbContext());
                    LoCode = LoCode == null ? "" : LoCode;
                    var d_ExpiryDate = (ExpiryDate != null ? DateTime.ParseExact(ExpiryDate.Value.ToString("dd/MM/yyyy"), "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss") : "");

                    //lấy tất cả các phiếu nhập
                    var inbound = Domain.Helper.SqlHelper.QuerySP<vwMaterialInboundDetail>("spSale_GetMaterialInboundDetail", new
                    {
                        MaterialId = materialId,
                        LoCode = LoCode,
                        ExpiryDate = d_ExpiryDate,
                        WarehouseDestinationId = warehouseId
                    }).ToList();

                    //lất tất cả các phiếu xuất
                    var outbound = Domain.Helper.SqlHelper.QuerySP<vwMaterialOutboundDetail>("spSale_GetMaterialOutboundDetail", new
                    {
                        MaterialId = materialId,
                        LoCode = LoCode,
                        ExpiryDate = d_ExpiryDate,
                        WarehouseSourceId = warehouseId
                    });

                    //lấy tồn kho hiện 
                    var inventoryMaterialCurrent_List = Erp.Domain.Helper.SqlHelper.QuerySP<InventoryMaterial>("spSale_Get_InventoryMaterial",
                        new
                        {
                            WarehouseId = warehouseId,
                            HasQuantity = "1",
                            Code = "",
                            MaterialName = "",
                            CategoryCode = "",
                            ProductGroup = "",
                            BranchId = "",
                            LoCode = LoCode,
                            Materialid = materialId,
                            ExpiryDate = ExpiryDate
                        }).ToList();


                    //lấy các sản phẩm nhập, xuất, tồn kho theo lô
                    if (string.IsNullOrEmpty(LoCode) || LoCode == "")
                    {
                        inbound = inbound.Where(x => string.IsNullOrEmpty(x.LoCode) || x.LoCode == "").ToList();
                        outbound = outbound.Where(x => string.IsNullOrEmpty(x.LoCode) || x.LoCode == "").ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => string.IsNullOrEmpty(x.LoCode)).ToList();
                    }
                    else
                    {
                        inbound = inbound.Where(x => x.LoCode == LoCode).ToList();
                        outbound = outbound.Where(x => x.LoCode == LoCode).ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => x.LoCode == LoCode).ToList();
                    }

                    //lấy các sản phẩm nhập, xuất, tồn kho theo lô và ngày hết hạn
                    if (ExpiryDate == null)
                    {
                        inbound = inbound.Where(x => x.ExpiryDate == null).ToList();
                        outbound = outbound.Where(x => x.ExpiryDate == null).ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => x.ExpiryDate == null).ToList();
                    }
                    else
                    {
                        inbound = inbound.Where(x => x.ExpiryDate == ExpiryDate).ToList();
                        outbound = outbound.Where(x => x.ExpiryDate == ExpiryDate).ToList();
                        inventoryMaterialCurrent_List = inventoryMaterialCurrent_List.Where(x => x.ExpiryDate == ExpiryDate).ToList();
                    }


                    var qty_inbound = inbound.Sum(x => x.Quantity);
                    var qty_outbound = outbound.Sum(x => x.Quantity);
                    //tính lại tồn kho dựa vào nhập và xuất
                    var inventoryMaterial = (inbound.Count() > 0 ? qty_inbound : 0) - (outbound.Count() > 0 ? qty_outbound : 0) + quantityIn - quantityOut;


                    //begin xóa các dòng double của sản phẩm và lô của sản phẩm đó, chỉ giữ lại 1 dòng đầu tiên
                    for (int i = 0; i < inventoryMaterialCurrent_List.Count(); i++)
                    {
                        if (i > 0)
                        {
                            var id = inventoryMaterialCurrent_List[i].Id;
                            inventoryMaterialRepository.DeleteInventoryMaterial(id);
                        }
                    }
                    //end xóa các dòng double của sản phẩm và lô của sản phẩm đó, chỉ giữ lại 1 dòng đầu tiên

                    //Sau khi thay đổi dữ liệu phải đảm bảo tồn kho >= 0
                    if (true)//inventory >= 0)
                    {
                        if (isArchive)
                        {
                            if (inventoryMaterialCurrent_List.Count() > 0)
                            {
                                if (inventoryMaterialCurrent_List[0].Quantity != inventoryMaterial)
                                {
                                    inventoryMaterialCurrent_List[0].Quantity = inventoryMaterial;
                                    inventoryMaterialRepository.UpdateInventoryMaterial(inventoryMaterialCurrent_List[0]);
                                }
                            }
                            else
                            {
                                var insert = new InventoryMaterial();
                                insert.IsDeleted = false;
                                insert.CreatedUserId = pCurrentUserId;
                                insert.ModifiedUserId = pCurrentUserId;
                                insert.CreatedDate = DateTime.Now;
                                insert.ModifiedDate = DateTime.Now;
                                insert.WarehouseId = warehouseId;
                                insert.MaterialId = materialId;
                                insert.Quantity = inventoryMaterial;
                                insert.LoCode = LoCode;
                                insert.ExpiryDate = ExpiryDate;
                                inventoryMaterialRepository.InsertInventoryMaterial(insert);
                            }
                        }
                    }
                    else
                    {
                        error += string.Format("<br/>Dữ liệu tồn kho của sản phẩm \"{0}\" là {1}: không hợp lệ!", materialName, inventoryMaterial);
                    }
                    scope.Complete();
                    return error;

                }
                catch (Exception ex)
                {

                    return ex.Message;
                }
            }

        }
        #endregion

        #region UpdateAll
        public ActionResult UpdateAll(string url)
        {
            string rs = "";
            using (var scope = new TransactionScope(TransactionScopeOption.Required,
                                new System.TimeSpan(0, 30, 0)))
            {
                try
                {
                    var inventoryList = InventoryMaterialRepository.GetAllInventoryMaterial().ToList();
                    var inbound1 = MaterialInboundRepository.GetAllvwMaterialInboundDetail().ToList();
                    var outbound1 = MaterialOutboundRepository.GetAllvwMaterialOutboundDetail().ToList();
                    foreach (var item in inventoryList)
                    {
                        var warehouseId = item.WarehouseId.Value;
                        var MaterialId = item.MaterialId.Value;
                        var inbound = inbound1.ToList();
                        var outbound = outbound1.ToList();

                        if (string.IsNullOrEmpty(item.LoCode))
                        {
                            inbound = inbound.Where(x => string.IsNullOrEmpty(x.LoCode)).ToList();
                            outbound = outbound.Where(x => string.IsNullOrEmpty(x.LoCode)).ToList();
                        }
                        else
                        {
                            inbound = inbound.Where(x => x.LoCode == item.LoCode).ToList();
                            outbound = outbound.Where(x => x.LoCode == item.LoCode).ToList();
                        }
                        if (item.ExpiryDate == null)
                        {
                            inbound = inbound.Where(x => x.ExpiryDate == null).ToList();
                            outbound = outbound.Where(x => x.ExpiryDate == null).ToList();
                        }
                        else
                        {
                            inbound = inbound.Where(x => x.ExpiryDate == item.ExpiryDate).ToList();
                            outbound = outbound.Where(x => x.ExpiryDate == item.ExpiryDate).ToList();
                        }

                        var _inbound = inbound.Where(x => x.IsArchive
                              && x.MaterialId == MaterialId
                              && x.WarehouseDestinationId == warehouseId
                              ).ToList().Sum(x => x.Quantity);

                        var _outbound = outbound.Where(x => x.IsArchive
                            && x.MaterialId == MaterialId
                            && x.WarehouseSourceId == warehouseId
                            ).ToList().Sum(x => x.Quantity);
                        var inventory = (_inbound == null ? 0 : _inbound) - (_outbound == null ? 0 : _outbound);

                        if (item.Quantity != inventory)
                        {
                            rs += "<br/>" + item.MaterialId + " | " + item.Quantity + " => " + inventory;
                            item.Quantity = inventory;
                            InventoryMaterialRepository.UpdateInventoryMaterial(item);
                        }

                    }
                    scope.Complete();
                }
                catch (DbUpdateException)
                {
                    TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.Error;
                    return Redirect(url);
                }
            }
            TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess + rs;
            return Redirect(url);
        }
        #endregion
    }
}
