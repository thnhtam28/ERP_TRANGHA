﻿using System.Globalization;
using Erp.BackOffice.Sale.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using Erp.Domain.Sale.Entities;
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
using Erp.Domain.Staff.Entities;
using Erp.Domain.Staff.Interfaces;
using Erp.Domain.Account.Interfaces;
using Erp.Domain.Helper;
using System.Transactions;
using System.Web;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class RequestInboundMaterialController : Controller
    {
        private readonly IRequestInboundRepository RequestInboundRepository;
        private readonly IUserRepository userRepository;
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IProductOrServiceRepository ProductRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IMaterialInboundRepository materialInboundRepository;
        private readonly IMaterialOutboundRepository materialOutboundRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        private readonly IBranchRepository branchRepository;
        private readonly IMaterialRepository materialRepository;
        public RequestInboundMaterialController(
            IRequestInboundRepository _RequestInbound
            , IUserRepository _user
            , IWarehouseRepository warehouse
            , IProductOrServiceRepository product
            , ICategoryRepository category
            , IMaterialOutboundRepository materialOutbound
            , IMaterialInboundRepository materialInbound
            , ITemplatePrintRepository templatePrint
            , IBranchRepository branch
            , IMaterialRepository material
            )
        {
            RequestInboundRepository = _RequestInbound;
            userRepository = _user;
            WarehouseRepository = warehouse;
            ProductRepository = product;
            categoryRepository = category;
            materialInboundRepository = materialInbound;
            materialOutboundRepository = materialOutbound;
            templatePrintRepository = templatePrint;
            branchRepository = branch;
            materialRepository = material;
        }

        #region Index

        public ViewResult Index(string txtCode, string startDate, string endDate, string status, int? branchId, string Error, string ErrorSuccess)
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

            IEnumerable<RequestInboundViewModel> q = RequestInboundRepository.GetAllvwRequestInbound()
                .Select(item => new RequestInboundViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    BarCode = item.BarCode,
                    BranchId = item.BranchId,
                    BranchName = item.BranchName,
                    //CancelReason=item.CancelReason,
                    Note = item.Note,
                    //ShipName=item.ShipName,
                    //ShipPhone=item.ShipPhone,
                    Status = item.Status,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    //WarehouseSourceId=item.WarehouseSourceId,
                    //WarehouseSourceName=item.WarehouseSourceName,
                    TotalAmount = item.TotalAmount,
                    Error = item.Error,
                    ErrorQuantity = item.ErrorQuantity,
                    TypeRequest=item.TypeRequest
                }).Where(x=>x.TypeRequest=="VT").ToList();
            if (!string.IsNullOrEmpty(txtCode))
            {
                q = q.Where(x => x.Code.ToLowerOrEmpty().Contains(txtCode.Trim().ToLower()));
            }

            //if (branchId != null && branchId.Value > 0)
            //{
            //    q = q.Where(x => x.BranchId == branchId);
            //}

            if (!string.IsNullOrEmpty(status))
            {
                q = q.Where(x => x.Status == status);
            }
            if (Error == "True")
            {
                q = q.Where(x => x.Error == true);
            }
            if (ErrorSuccess == "True")
            {
                q = q.Where(x => x.ErrorQuantity != null && x.ErrorQuantity.Value <= 0);
            }
            if (ErrorSuccess == "False")
            {
                q = q.Where(x => x.ErrorQuantity != null && x.ErrorQuantity.Value > 0);
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
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);
            if (user.UserTypeId == 1)
            {
                q = q.OrderByDescending(m => m.Id);
            }
            else
            {
                //if (Erp.BackOffice.Filters.SecurityFilter.IsAdminSystemManager() || Erp.BackOffice.Filters.SecurityFilter.IsAdminDrugStore() || Erp.BackOffice.Filters.SecurityFilter.IsTrinhDuocVien() || Erp.BackOffice.Filters.SecurityFilter.IsDrugStore())
                //{
                //    q = q.Where(x => ("," + user.DrugStore + ",").Contains("," + x.BranchId + ",") == true);
                //}
                if (Erp.BackOffice.Filters.SecurityFilter.IsAdmin() || user.UserTypeCode == "KT")
                {
                    q = q.Where(x => x.Status != "cancel").OrderByDescending(m => m.Id).ToList();
                }
                else
                {
                    if (Erp.BackOffice.Filters.SecurityFilter.AccessRight("Create", "RequestInbound", "Sale"))
                    {
                        var a = q;
                        q = q.Where(x => x.Status != "cancel"
                            && x.BranchId == user.BranchId
                            ).OrderByDescending(m => m.Id).ToList();
                        a = a.Where(x => x.Status == "cancel" && x.CreatedUserId == user.Id).OrderByDescending(m => m.ModifiedDate).ToList();
                        q = q.Union(a).ToList();
                    }
                    else
                    {
                        q = q.Where(x => x.Status != "cancel"
                             && x.BranchId == user.BranchId
                            ).OrderByDescending(m => m.Id).ToList();
                    }
                }
            }
            ViewBag.User = user;
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Detail
        public ActionResult Detail(int? Id)
        {
            var model = new RequestInboundViewModel();
            var modelMaterialInbound = new MaterialInboundViewModel();
            var modelMaterialOutbound = new MaterialOutboundViewModel();
            var requestInbound = new vwRequestInbound();
            var materialInbound = new vwMaterialInbound();
            var materialOutbound = new vwMaterialOutbound();
            if (Id != null && Id.Value > 0)
            {
                requestInbound = RequestInboundRepository.GetvwRequestInboundById(Id.Value);
                if (requestInbound.InboundId != null && requestInbound.InboundId.Value > 0)
                {
                    materialInbound = materialInboundRepository.GetvwMaterialInboundFullById(requestInbound.InboundId.Value);
                    AutoMapper.Mapper.Map(materialInbound, modelMaterialInbound);
                    modelMaterialInbound.CreatedUserName = userRepository.GetUserById(modelMaterialInbound.CreatedUserId.Value).FullName;
                }
                if (requestInbound.OutboundId != null && requestInbound.OutboundId.Value > 0)
                {
                    materialOutbound = materialOutboundRepository.GetvwMaterialOutboundFullById(requestInbound.OutboundId.Value);
                    AutoMapper.Mapper.Map(materialOutbound, modelMaterialOutbound);
                    modelMaterialOutbound.CreatedUserName = userRepository.GetUserById(modelMaterialOutbound.CreatedUserId.Value).FullName;
                }
            }
            ViewBag.productInbound = modelMaterialInbound;
            ViewBag.productOutbound = modelMaterialOutbound;
            AutoMapper.Mapper.Map(requestInbound, model);
            model.CreatedUserName = userRepository.GetUserById(requestInbound.CreatedUserId.Value).FullName;
            model.DetailList = RequestInboundRepository.GetAllvwRequestInboundDetailsByInvoiceId(requestInbound.Id).AsEnumerable().Select(x =>
                new RequestInboundDetailViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    CategoryCode = x.CategoryCode,
                    MaterialName = x.MaterialName,
                    MaterialCode = x.MaterialCode,
                    ProductGroup = x.ProductGroup,
                    Manufacturer = x.Manufacturer,
                    ProductBarCode = x.ProductBarCode,
                    RequestInboundId = x.RequestInboundId,
                    QuantityRemaining = x.QuantityRemaining,
                    ProductGroupName = x.ProductGroupName,
                    Image_Name = x.Image_Name
                }).OrderBy(x => x.Id).ToList();
            var warehouse = WarehouseRepository.GetAllWarehouse().Where(x => x.IsSale == true && x.BranchId == null).ToList();
            if (warehouse.Count() > 0)
            {
                // var wh = warehouse.FirstOrDefault().Id;
                var productList = Domain.Helper.SqlHelper.QuerySP<InventoryMaterial>("spSale_Get_InventoryMaterial", new { WarehouseId = "", HasQuantity = "1", MaterialCode = "", MaterialName = "", CategoryCode = "", ProductGroup = "", BranchId = "", LoCode = "", MaterialId = "", ExpiryDate = ""});
                productList = productList.Where(id1 => warehouse.Any(id2 => id2.Id == id1.WarehouseId)).ToList();
                foreach (var item in model.DetailList)
                {
                    var quantity = productList.Where(x => x.MaterialId == item.ProductId).Sum(x => x.Quantity);
                    item.QuantityInventoryKT = quantity == null ? 0 : quantity;
                    item.QuantityNotCondition = item.QuantityInventoryKT <= 0 ? 1 : 0;
                }
            }

            return View(model);
        }


        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete(int Id, string CancelReason)
        {
            var requestInbound = RequestInboundRepository.GetRequestInboundById(Id);
            if (requestInbound != null)
            {
                //Kiểm tra phân quyền Chi nhánh
                if (!(Filters.SecurityFilter.IsAdmin()))
                {
                    return Content("Mẫu tin không tồn tại! Không có quyền truy cập!");
                }
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {
                        requestInbound.ModifiedUserId = WebSecurity.CurrentUserId;
                        requestInbound.ModifiedDate = DateTime.Now;
                        //requestInbound.IsDeleted = true;

                        requestInbound.CancelReason = CancelReason;
                        requestInbound.Status = "cancel";
                        RequestInboundRepository.UpdateRequestInbound(requestInbound);
                        if (requestInbound.InboundId != null)
                        {
                            var inbound = materialInboundRepository.GetMaterialInboundById(requestInbound.InboundId.Value);
                            Erp.BackOffice.Sale.Controllers.MaterialInboundController.UnArchiveAndDelete(inbound);
                        }
                        if (requestInbound.OutboundId != null)
                        {
                            var outbound = materialOutboundRepository.GetMaterialOutboundById(requestInbound.OutboundId.Value);
                            Erp.BackOffice.Sale.Controllers.MaterialOutboundController.UnArchiveAndDelete(outbound);
                        }
                        scope.Complete();
                        TempData[Globals.SuccessMessageKey] = Erp.BackOffice.App_GlobalResources.Wording.DeleteSuccess;
                        return RedirectToAction("Index");
                    }
                    catch (DbUpdateException)
                    {
                        return Content("Fail");
                    }
                }
                //return RedirectToAction("Detail", new { Id = requestInbound.Id });
            }

            return RedirectToAction("Index");
        }
        #endregion

        #region LoadProductItem
        public PartialViewResult LoadProductItem(int OrderNo, int MaterialId, string MaterialName, string Unit, int Quantity, decimal Price, string MaterialCode, string MaterialType)
        {
            var model = new RequestInboundDetailViewModel();
            model.OrderNo = OrderNo;
            model.MaterialId = MaterialId;
            model.MaterialName = MaterialName;
            model.Unit = Unit;
            model.Quantity = Quantity;
            model.Price = Price;
            model.MaterialCode = MaterialCode;
            return PartialView(model);
        }
        #endregion

        #region Create And Edit
        public ActionResult Create(int? Id)
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
            int? branchId = intBrandID;
            RequestInboundViewModel model = new RequestInboundViewModel();
            model.DetailList = new List<RequestInboundDetailViewModel>();
            if (Id.HasValue && Id > 0)
            {
                var requestInbound = RequestInboundRepository.GetvwRequestInboundById(Id.Value);
                AutoMapper.Mapper.Map(requestInbound, model);

                var detailList = RequestInboundRepository.GetAllvwRequestInboundDetailsByInvoiceId(requestInbound.Id).ToList();
                AutoMapper.Mapper.Map(detailList, model.DetailList);
            }
            var warehouseList = WarehouseRepository.GetvwAllWarehouse().AsEnumerable();
            var user = userRepository.GetByvwUserName(Helpers.Common.CurrentUser.UserName);

            warehouseList = warehouseList.Where(x => x.Categories == "VT").ToList();
            if(branchId>0)
            {
                warehouseList = warehouseList.Where(x => x.BranchId == branchId).ToList();
            }
            if (!Erp.BackOffice.Filters.SecurityFilter.IsAdmin() && !Erp.BackOffice.Filters.SecurityFilter.IsKeToan())
            {
                warehouseList = warehouseList.Where(x => ("," + user.WarehouseId + ",").Contains("," + x.Id + ",") == true);
            }
            var _warehouseList = warehouseList.Select(item => new SelectListItem
            {
                Text = item.Name + " (" + item.BranchName + ")",
                Value = item.Id.ToString()
            });
            ViewBag.warehouseList = _warehouseList;
            //string image_folder_product = Helpers.Common.GetSetting("product-image-folder");
            var materialList = materialRepository.GettAllMaterial().AsEnumerable()
                  .Select(item => new MaterialViewModel
                  {
                      Code = item.Code,
                      Barcode = item.Barcode,
                      Name = item.Name,
                      Id = item.Id,
                      CategoryCode = string.IsNullOrEmpty(item.CategoryCode) ? "Sản phẩm khác" : item.CategoryCode,
                      Unit = item.Unit,
                      PriceOutbound = item.PriceOutBound,
                      Image_Name = Erp.BackOffice.Helpers.Common.KiemTraTonTaiHinhAnh(item.Image_Name, "Material", "product")
                  }).ToList();
            ViewBag.materialList = materialList;
            model.CreatedDate = DateTime.Now;
            model.CreatedUserName = Helpers.Common.CurrentUser.FullName;
            ViewBag.isAdmin = Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId == 1 ? true : false;
            if (model.DetailList != null && model.DetailList.Count > 0)
            {
                foreach (var item in model.DetailList)
                {
                    var product = materialList.Where(i => i.Id == item.ProductId).FirstOrDefault();
                    if (product == null)
                    {
                        item.Id = 0;
                    }
                }

                model.DetailList.RemoveAll(x => x.Id == 0);

                int n = 0;
                foreach (var item in model.DetailList)
                {
                    item.OrderNo = n;
                    n++;
                }
            }
            return View(model);
        }

        [HttpPost]
        public ActionResult Create(RequestInboundViewModel model)
        {
            if (ModelState.IsValid && model.DetailList.Count != 0)
            {
                var requestInbound = new Domain.Sale.Entities.RequestInbound();
                using (var scope = new TransactionScope(TransactionScopeOption.Required))
                {
                    try
                    {

                        AutoMapper.Mapper.Map(model, requestInbound);
                        if (model.Id > 0)
                        {

                        }
                        else
                        {
                            requestInbound.IsDeleted = false;
                            requestInbound.CreatedUserId = WebSecurity.CurrentUserId;
                            requestInbound.ModifiedUserId = WebSecurity.CurrentUserId;
                            requestInbound.CreatedDate = DateTime.Now;
                            requestInbound.ModifiedDate = DateTime.Now;
                            var branch = WarehouseRepository.GetWarehouseById(requestInbound.WarehouseDestinationId.Value);
                            requestInbound.BranchId = branch.BranchId;
                            requestInbound.Status = "new";
                            requestInbound.TypeRequest = "VT";

                        }
                        //duyệt qua danh sách sản phẩm mới xử lý tình huống user chọn 2 sản phầm cùng id
                        List<Domain.Sale.Entities.RequestInboundDetail> listNewCheckSameId = new List<Domain.Sale.Entities.RequestInboundDetail>();
                        foreach (var group in model.DetailList.GroupBy(x => x.MaterialId))
                        {
                            var product = materialRepository.GetMaterialById(group.Key);

                            listNewCheckSameId.Add(new Domain.Sale.Entities.RequestInboundDetail
                            {
                                ProductId = group.Key,
                                Quantity = group.Sum(x => x.Quantity),
                                Unit = product.Unit,
                                Price = product.PriceOutBound,
                                IsDeleted = false,
                                CreatedUserId = WebSecurity.CurrentUserId,
                                ModifiedUserId = WebSecurity.CurrentUserId,
                                CreatedDate = DateTime.Now,
                                ModifiedDate = DateTime.Now,
                            });
                        }

                        requestInbound.TotalAmount = listNewCheckSameId.Sum(x => x.Price * x.Quantity);

                        if (model.Id > 0)
                        {
                            requestInbound.Status = "new";
                            RequestInboundRepository.UpdateRequestInbound(requestInbound);
                            var listDetail = RequestInboundRepository.GetAllRequestInboundDetailsByInvoiceId(requestInbound.Id).ToList();
                            //xóa danh sách dữ liệu cũ dưới database
                            RequestInboundRepository.DeleteRequestInboundDetail(listDetail);
                            //thêm mới toàn bộ database
                            foreach (var item in listNewCheckSameId)
                            {
                                item.RequestInboundId = requestInbound.Id;
                                RequestInboundRepository.InsertRequestInboundDetail(item);
                            }
                            //lấy thông tin chi nhánh gửi yêu cầu hiện vào notification
                            //   var branchName = branchRepository.GetBranchById(model.BranchId.Value);

                            //Apply business process flow
                            Crm.Controllers.ProcessController.Run("RequestInbound", "Create", requestInbound.Id, requestInbound.ModifiedUserId, null, requestInbound);
                        }
                        else
                        {
                            RequestInboundRepository.InsertRequestInbound(requestInbound, listNewCheckSameId);

                            //cập nhật lại mã hóa đơn
                            requestInbound.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("requestInbound", model.Code);
                            RequestInboundRepository.UpdateRequestInbound(requestInbound);
                            Erp.BackOffice.Helpers.Common.SetOrderNo("requestInbound");

                            //string prefix = Erp.BackOffice.Helpers.Common.GetSetting("prefixOrderNo_RequestInbound");
                            //requestInbound.Code = Erp.BackOffice.Helpers.Common.GetCode(prefix, requestInbound.Id);
                            //RequestInboundRepository.UpdateRequestInbound(requestInbound);

                            //  var branchName = branchRepository.GetBranchById(requestInbound.BranchId.Value);

                            //run process task 
                            Crm.Controllers.ProcessController.Run("RequestInbound", "Create", requestInbound.Id, requestInbound.ModifiedUserId, null, requestInbound);

                        }
                        scope.Complete();
                    }
                    catch (DbUpdateException)
                    {
                        TempData[Globals.FailedMessageKey] = App_GlobalResources.Wording.Error;
                        return View(model);
                    }
                }
                return RedirectToAction("Detail", new { Id = requestInbound.Id });
            }
            return View(model);
        }
        #endregion

        #region Cancel //bên gửi thu hồi yêu cầu gửi đi
        public ActionResult Cancel(int Id)
        {
            var q = RequestInboundRepository.GetRequestInboundById(Id);
            q.Status = "cancel";
            q.ModifiedUserId = WebSecurity.CurrentUserId;
            RequestInboundRepository.UpdateRequestInbound(q);

            Crm.Controllers.ProcessController.Run("RequestInbound", "Cancel", q.Id, q.ModifiedUserId, null, q);
            return RedirectToAction("Index");
        }
        #endregion

        #region refure -  bên nhận Từ chối duyệt yêu cầu.
        public ActionResult Refure(int? Id)
        {
            var q = RequestInboundRepository.GetvwRequestInboundById(Id.Value);
            if (q != null && q.IsDeleted != true)
            {
                var model = new RequestInboundViewModel();
                AutoMapper.Mapper.Map(q, model);
                return View(model);
            }
            return RedirectToAction("Edit", new { Id = q.Id });
        }
        [HttpPost]
        public ActionResult Refure(RequestInboundViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var q = RequestInboundRepository.GetRequestInboundById(model.Id);
                    q.Status = "refure";
                    q.CancelReason = model.CancelReason;
                    q.ModifiedUserId = WebSecurity.CurrentUserId;
                    RequestInboundRepository.UpdateRequestInbound(q);

                    //gửi notifications cho người được phân quyền.
                    Crm.Controllers.ProcessController.Run("RequestInbound", "Refure", q.Id, q.CreatedUserId, null, q);

                    return View("_ClosePopup");
                }
                return RedirectToAction("Edit", new { Id = model.Id });
            }
            TempData[Globals.SuccessMessageKey] = "Yêu cầu: " + model.Code + " bị từ chối không thành công.";
            return RedirectToAction("Index");
        }
        #endregion

        #region  Shipping Đang giao hàng.
        public ActionResult Shipping(int? Id)
        {
            var q = RequestInboundRepository.GetvwRequestInboundById(Id.Value);
            if (q != null && q.IsDeleted != true)
            {
                var model = new RequestInboundViewModel();
                AutoMapper.Mapper.Map(q, model);
                return View(model);
            }
            return RedirectToAction("Edit", new { Id = q.Id });
        }
        [HttpPost]
        public ActionResult Shipping(RequestInboundViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var q = RequestInboundRepository.GetRequestInboundById(model.Id);
                    q.Status = "shipping";
                    q.ShipName = model.ShipName;
                    q.ShipPhone = model.ShipPhone;
                    q.ModifiedUserId = WebSecurity.CurrentUserId;
                    RequestInboundRepository.UpdateRequestInbound(q);

                    //gửi notifications cho người được phân quyền.
                    Crm.Controllers.ProcessController.Run("RequestInbound", "Shipping", q.Id, q.CreatedUserId, null, q);

                    return View("_ClosePopup");
                }
                return RedirectToAction("Edit", new { Id = model.Id });
            }
            TempData[Globals.SuccessMessageKey] = "Yêu cầu: " + model.Code + " giao hàng không thành công. Vui lòng kiểm tra lại thông tin bên giao hàng";
            return RedirectToAction("Index");
        }
        #endregion


        #region Print
        public ActionResult Print(int? Id)
        {
            //Lấy thông tin phiếu xuất kho
            var requestOutbound = RequestInboundRepository.GetvwRequestInboundById(Id.Value);

            if (requestOutbound != null)
            {
                //Lấy template và replace dữ liệu vào phiếu xuất.
                TemplatePrint template = null;

                template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("RequestInbound")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                if (template == null)
                    return Content("No template!");

                string output = template.Content;

                //Header
                //Lấy thông tin cài đặt cho phiếu in
                var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
                var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
                var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
                var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
                var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
                var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
                var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
                var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"73\" /></div>";

                output = output.Replace("{System.Logo}", ImgLogo);
                output = output.Replace("{System.CompanyName}", company);
                output = output.Replace("{System.AddressCompany}", address);
                output = output.Replace("{System.PhoneCompany}", phone);
                output = output.Replace("{System.Fax}", fax);
                output = output.Replace("{System.BankCodeCompany}", bankcode);
                output = output.Replace("{System.BankNameCompany}", bankname);

                string day = requestOutbound.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm");
                string warehouseDestination = requestOutbound.WarehouseDestinationName;
                string branchName = requestOutbound.BranchName;
                string note = requestOutbound.Note;
                string code = requestOutbound.Code;
                string CreateUserName = userRepository.GetUserById(requestOutbound.CreatedUserId.Value).FullName;

                output = output.Replace("{Day}", day);

                output = output.Replace("{warehouseDestination}", warehouseDestination);
                output = output.Replace("{branchName}", branchName);
                output = output.Replace("{Note}", note);

                output = output.Replace("{Code}", code);
                output = output.Replace("{CreateUserName}", CreateUserName);
                //Mid
                output = output.Replace("{DetailList}", buildHtmlDetailList(Id.Value));

                //Footer
                ViewBag.PrintContent = output;
                return View();
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        string buildHtmlDetailList(int Id)
        {
            decimal tong_tien = 0;
            //Lấy danh sách sản phẩm xuất kho
            var outboundDetails = RequestInboundRepository.GetAllvwRequestInboundDetailsByInvoiceId(Id).AsEnumerable()
                    .Select(x => new RequestInboundDetailViewModel
                    {
                        Id = x.Id,
                        Price = x.Price,
                        ProductId = x.ProductId,
                        MaterialName = x.MaterialName,
                        MaterialCode = x.MaterialCode,
                        RequestInboundId = x.RequestInboundId,
                        Quantity = x.Quantity,
                        Unit = x.Unit,
                        CategoryCode = x.CategoryCode,
                        ProductGroup = x.ProductGroup,
                    }).ToList();
            //Tạo table html chi tiết phiếu xuất
            string detailList = "<table class=\"invoice-detail\">\r\n";
            detailList += "<thead>\r\n";
            detailList += "	<tr>\r\n";
            detailList += "		<th>STT</th>\r\n";
            detailList += "		<th>M&atilde; h&agrave;ng</th>\r\n";
            detailList += "		<th>T&ecirc;n mặt h&agrave;ng</th>\r\n";
            detailList += "		<th>ĐVT</th>\r\n";
            detailList += "		<th>Số lượng</th>\r\n";
            detailList += "		<th>Đơn gi&aacute;</th>\r\n";
            detailList += "		<th>Th&agrave;nh tiền</th>\r\n";
            detailList += "	</tr>\r\n";
            detailList += "</thead>\r\n";
            detailList += "<tbody><tbody>\r\n";

            foreach (var item in outboundDetails)
            {
                decimal thanh_tien = item.Quantity.Value * item.Price.Value;
                tong_tien += thanh_tien;

                detailList += "<tr>\r\n"
                 + "<td class=\"text-center\">" + (outboundDetails.ToList().IndexOf(item) + 1) + "</td>\r\n"
                 + "<td class=\"text-left\">" + item.ProductCode + "</td>\r\n"
                 + "<td class=\"text-left\">" + item.ProductName + "</td>\r\n"
                 + "<td class=\"text-center\">" + item.Unit + "</td>\r\n"
                 + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.Quantity) + "</td>\r\n"
                 + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.Price, null) + "</td>\r\n"
                 + "<td class=\"text-right\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(thanh_tien, null) + "</td>\r\n"
                 + "</tr>\r\n";
            }

            detailList += "</tbody>\r\n";
            detailList += "<tfoot>\r\n";
            detailList += "<tr><td colspan=\"4\" class=\"text-right\"></td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(outboundDetails.Sum(x => x.Quantity))
                         + "</td><td class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                         + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_tien, null)
                         + "</td></tr>\r\n";
            detailList += "<tr><td colspan=\"7\"><strong>Tiền bằng chữ: " + Erp.BackOffice.Helpers.Common.ChuyenSoThanhChu_2(tong_tien.ToString()) + "</strong></td></tr>\r\n";
            detailList += "</tfoot>\r\n</table>\r\n";

            return detailList;
        }
        #endregion
    }
}
