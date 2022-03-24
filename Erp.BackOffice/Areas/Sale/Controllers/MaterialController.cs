using System.Globalization;
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
using System.IO;
using System.Text.RegularExpressions;
using Erp.Domain.Entities;
using System.Web;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class MaterialController : Controller
    {
        private readonly IMaterialOrServiceRepository materialRepository;
        private readonly IObjectAttributeRepository ObjectAttributeRepository;
        private readonly ISupplierRepository SupplierRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IInventoryMaterialRepository inventoryMaterialRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        public MaterialController(
            IMaterialOrServiceRepository _Material
            , IObjectAttributeRepository _ObjectAttribute
            , ISupplierRepository _Supplier
            , IUserRepository _user
            , ICategoryRepository category
            ,IInventoryMaterialRepository inventoryMaterial
            , ITemplatePrintRepository _templatePrint
            )
        {
            materialRepository = _Material;
            ObjectAttributeRepository = _ObjectAttribute;
            SupplierRepository = _Supplier;
            userRepository = _user;
            categoryRepository = category;
            inventoryMaterialRepository = inventoryMaterial;
            templatePrintRepository = _templatePrint;
        }

        #region Index

        public ViewResult Index(string txtSearch, string txtCode, string CategoryCode, string ProductGroup)
        {
            IEnumerable<MaterialViewModel> q = materialRepository.GetAllvwMaterial().AsEnumerable()
                .Select(item => new MaterialViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    Code = item.Code,
                    PriceInbound = item.PriceInbound,
                    PriceOutbound = item.PriceOutBound,
                    //Barcode = item.Barcode,
                    //Type = item.Type,
                    Unit = item.Unit,
                    CategoryCode = item.CategoryCode,
                    ProductGroup = item.ProductGroup
                }).OrderByDescending(m => m.Id);

            if (string.IsNullOrEmpty(txtSearch) == false || string.IsNullOrEmpty(txtCode) == false)
            {
                //txtSearch = txtSearch == "" ? "~" : Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(txtSearch);
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.Name).Contains(Helpers.Common.ChuyenThanhKhongDau(txtCode)) || x.Code.ToLower().Contains(txtCode));
            }
            if (!string.IsNullOrEmpty(CategoryCode))
            {
                q = q.Where(x => x.CategoryCode == CategoryCode);
            }
            if (!string.IsNullOrEmpty(ProductGroup))
            {
                q = q.Where(x => x.ProductGroup == ProductGroup);
            }

            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create()
        {
            var model = new MaterialViewModel();
            model.PriceInbound = 0;
            model.PriceOutbound = 0;
            //model.MinInventory = 0;
            return View(model);
        }

        [HttpPost]
        [ValidateInput(false)]
        //[ValidateAntiForgeryToken]
        public ActionResult Create(MaterialViewModel model)
        {
            if (ModelState.IsValid)
            {
                var Material = new Domain.Sale.Entities.Material();
                AutoMapper.Mapper.Map(model, Material);
                Material.IsDeleted = false;
                Material.CreatedUserId = WebSecurity.CurrentUserId;
                Material.ModifiedUserId = WebSecurity.CurrentUserId;
                Material.CreatedDate = DateTime.Now;
                Material.ModifiedDate = DateTime.Now;
                Material.ProductGroup = model.ProductGroup;
                //if (model.PriceInbound == null)
                //    Material.PriceInbound = 0;
                Material.Code = Material.Code.Trim();
                var path = System.Web.HttpContext.Current.Server.MapPath("~" + Helpers.Common.GetSetting("Material"));
                if (Request.Files["file-image"] != null)
                {
                    var file = Request.Files["file-image"];
                    if (file.ContentLength > 0)
                    {
                        string image_name = "Material_" + Helpers.Common.ChuyenThanhKhongDau(Regex.Replace(Material.Code, @"\s+", "_")) + "." + file.FileName.Split('.').Last();
                        bool isExists = System.IO.Directory.Exists(path);
                        if (!isExists)
                            System.IO.Directory.CreateDirectory(path);
                        file.SaveAs(path + image_name);
                        Material.Image_Name = image_name;
                    }
                }

                materialRepository.InsertMaterial(Material);

                if (string.IsNullOrEmpty(Request["IsPopup"]) == false)
                {
                    return RedirectToAction("Edit", new { Id = model.Id, IsPopup = Request["IsPopup"] });
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                return RedirectToAction("Index");
            }

            string errors = string.Empty;
            foreach (var modalState in ModelState.Values)
            {
                errors += modalState.Value + ": ";
                foreach (var error in modalState.Errors)
                {
                    errors += error.ErrorMessage;
                }
            }

            ViewBag.errors = errors;

            return View(model);
        }

        public ActionResult CheckCodeExsist(int? id, string code)
        {
            code = code.Trim();
            var Material = materialRepository.GetAllMaterial()
                .Where(item => item.Code == code).FirstOrDefault();
            if (Material != null)
            {
                if (id == null || (id != null && Material.Id != id))
                    return Content("Trùng mã vật tư !");
                else
                {
                    return Content("");
                }
            }
            else
            {
                return Content("");
            }
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var Material = materialRepository.GetMaterialById(Id.Value);
            if (Material != null && Material.IsDeleted != true)
            {
                var model = new MaterialViewModel();
                AutoMapper.Mapper.Map(Material, model);
                string MaterialId = "," + model.Id + ",";
                var supplierList = SupplierRepository.GetAllSupplier().AsEnumerable().Where(item => ("," + item.ProductIdOfSupplier + ",").Contains(MaterialId) == true).ToList();
                ViewBag.supplierList = supplierList;

                return View(model);
            }

            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult Edit(MaterialViewModel model)
        {
            foreach (var modelKey in ModelState.Keys)
            {
                if (modelKey == "PriceInbound" || modelKey == "PriceOutbound")
                {
                    var index = ModelState.Keys.ToList().IndexOf(modelKey);
                    ModelState.Values.ElementAt(index).Errors.Clear();
                }
            }
            if (ModelState.IsValid)
            {
                var Material = materialRepository.GetMaterialById(model.Id);
                AutoMapper.Mapper.Map(model, Material);
                Material.ModifiedUserId = WebSecurity.CurrentUserId;
                Material.ModifiedDate = DateTime.Now;
                if (model.PriceInbound == null)
                    Material.PriceInbound = 0;
                var path = Helpers.Common.GetSetting("Material");
                var filepath = System.Web.HttpContext.Current.Server.MapPath("~" + path);
                if (Request.Files["file-image"] != null)
                {
                    var file = Request.Files["file-image"];
                    if (file.ContentLength > 0)
                    {
                        FileInfo fi = new FileInfo(Server.MapPath("~" + path) + Material.Image_Name);
                        if (fi.Exists)
                        {
                            fi.Delete();
                        }

                        string image_name = "Material_" + Helpers.Common.ChuyenThanhKhongDau(Regex.Replace(Material.Code, @"\s+", "_")) + "." + file.FileName.Split('.').Last();

                        bool isExists = System.IO.Directory.Exists(filepath);
                        if (!isExists)
                            System.IO.Directory.CreateDirectory(filepath);
                        file.SaveAs(filepath + image_name);
                        Material.Image_Name = image_name;
                    }
                }

                materialRepository.UpdateMaterial(Material);

                if (string.IsNullOrEmpty(Request["IsPopup"]) == false)
                {
                    return RedirectToAction("Edit", new { Id = model.Id, IsPopup = Request["IsPopup"] });
                }

                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                return RedirectToAction("Index");
            }

            string errors = string.Empty;
            foreach (var modalState in ModelState.Values)
            {
                errors += modalState.Value + ": ";
                foreach (var error in modalState.Errors)
                {
                    errors += error.ErrorMessage;
                }
            }

            ViewBag.errors = errors;

            return View(model);

            //if (Request.UrlReferrer != null)
            //    return Redirect(Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index");
        }

        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                string id = Request["Delete"];
                if (id != null)
                {
                    var item = materialRepository.GetMaterialById(int.Parse(id, CultureInfo.InvariantCulture));
                    if (item != null)
                    {

                        item.IsDeleted = true;
                        materialRepository.UpdateMaterial(item);
                    }
                }
                else
                {
                     string idDeleteAll = Request["DeleteId-checkbox"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    var item = materialRepository.GetMaterialById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {

                        item.IsDeleted = true;
                        materialRepository.UpdateMaterial(item);
                    }
                }
                }
               
                TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.DeleteSuccess;
                return RedirectToAction("Index");
            }
            catch (DbUpdateException)
            {
                TempData[Globals.FailedMessageKey] = App_GlobalResources.Error.RelationError;
                return RedirectToAction("Index");
            }
        }
        #endregion

        #region  - Json -
        public JsonResult GetListJson()
        {
            var list = materialRepository.GetAllMaterial().ToList();
            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        [AllowAnonymous]
        public JsonResult GetListMaterial()
        {
            var q = materialRepository.GetAllvwMaterial()
                .Select(item => new
                {
                    item.Id,
                    item.Name,
                    item.Code,
                    item.PriceOutBound,
                    item.ProductGroup,
                    item.Unit,
                    item.Image_Name,
                    item.CategoryCode
                })
                .OrderBy(item => item.Name)
                .ToList();
            return Json(q.Select(item => new { item.Id, Code = item.Code, Image = Helpers.Common.KiemTraTonTaiHinhAnh(item.Image_Name, "Material", "product"), Unit = item.Unit, Note = Erp.BackOffice.Helpers.Common.PhanCachHangNgan2(item.PriceOutBound)+"VNĐ/"+item.Unit, Price = item.PriceOutBound, Text = item.Name, Name = item.Name, Value = item.Id }), JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        public JsonResult GetListMaterialInventory()
        {
            var branchId = Erp.BackOffice.Helpers.Common.CurrentUser.BranchId.Value;
            var productList = inventoryMaterialRepository.GetAllvwInventoryMaterialByBranchId(branchId).Where(x => x.Quantity > 0)
             .Select(item => new
             {
                 Id = item.MaterialId,
                 Name = item.MaterialName,
                 Code = item.MaterialCode,
                 PriceOutbound = item.MaterialPriceOutbound,
                 CategoryCode = item.CategoryCode,
                 Unit = item.MaterialUnit,
                 Image_Name = item.Image_Name,
                 QuantityTotalInventory = item.Quantity,
                 LoCode = item.LoCode,
                 ExpiryDate = item.ExpiryDate
             }).OrderBy(item => item.CategoryCode).ToList();
            var json_data = productList.Select(item => new
            {
                item.Id,
                Code = item.Code,
                Image = Helpers.Common.KiemTraTonTaiHinhAnh(item.Image_Name, "Material", "product"),
                Unit = item.Unit,
                Price = item.PriceOutbound,
                Text = item.Code + " - " + item.Name + " (" + Helpers.Common.PhanCachHangNgan2(item.PriceOutbound) + "/" + item.Unit + ")",
                Name = item.Name,
                Value = item.Id,
                QuantityTotalInventory = item.QuantityTotalInventory,
                LoCode = item.LoCode,
                ExpiryDate = (item.ExpiryDate == null ? "" : item.ExpiryDate.Value.ToString("dd/MM/yyyy")),
                Note = "SL:" + item.QuantityTotalInventory + "  Lô:" + item.LoCode + "  HSD:" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd/MM/yyyy") : "")
            });

            return Json(json_data, JsonRequestBehavior.AllowGet);
        }

        #region ExportExcel
        public List<MaterialViewModel> IndexExport(string txtCode, string CategoryCode, string ProductGroup, int? BranchId)
        {
            var q = materialRepository.GetAllMaterial().AsEnumerable()
                .Select(item => new MaterialViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Name = item.Name,
                    Code = item.Code,
                    PriceInbound = item.PriceInbound,
                    PriceOutbound = item.PriceOutBound,
                    //Barcode = item.Barcode,
                    //Type = item.Type,
                    Unit = item.Unit,
                    CategoryCode = item.CategoryCode,
                    ProductGroup = item.ProductGroup
                }).OrderByDescending(m => m.Id).ToList();

            if (string.IsNullOrEmpty(txtCode) == false)
            {
                txtCode = txtCode == "" ? "~" : txtCode.ToLower();
                q = q.Where(x => Erp.BackOffice.Helpers.Common.ChuyenThanhKhongDau(x.Name).Contains(Helpers.Common.ChuyenThanhKhongDau(txtCode)) || x.Code.ToLower().Contains(txtCode)).ToList();
            }
            if (!string.IsNullOrEmpty(CategoryCode))
            {
                q = q.Where(x => x.CategoryCode == CategoryCode).ToList();
            }
            if (!string.IsNullOrEmpty(ProductGroup))
            {
                q = q.Where(x => x.ProductGroup == ProductGroup).ToList();
            }

            return q;
        }

        public ActionResult ExportExcel(string txtCode, string CategoryCode, string ProductGroup, int? BranchId, bool ExportExcel = false)
        {
            var data = IndexExport(txtCode, CategoryCode, ProductGroup, BranchId);

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
            model.Content = model.Content.Replace("{DataTable}", buildHtml(data));
            model.Content = model.Content.Replace("{System.Logo}", ImgLogo);
            model.Content = model.Content.Replace("{System.CompanyName}", company);
            model.Content = model.Content.Replace("{System.AddressCompany}", address);
            model.Content = model.Content.Replace("{System.PhoneCompany}", phone);
            model.Content = model.Content.Replace("{System.Fax}", fax);
            model.Content = model.Content.Replace("{System.BankCodeCompany}", bankcode);
            model.Content = model.Content.Replace("{System.BankNameCompany}", bankname);
            model.Content = model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
            model.Content = model.Content.Replace("{Title}", "Danh sách vật tư");
            if (ExportExcel)
            {
                Response.AppendHeader("content-disposition", "attachment;filename=" + "DS_Vattu" + DateTime.Now.ToString("yyyyMMdd") + ".xls");
                Response.Charset = "";
                Response.Cache.SetCacheability(HttpCacheability.NoCache);
                Response.ContentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
                Response.Write(model.Content);
                Response.End();
            }
            return View(model);
        }

        string buildHtml(List<MaterialViewModel> data)
        {
            decimal? tong_tien = 0;
            //Tạo table html chi tiết phiếu xuất
            string detailLists = "<table border=\"1\" class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>STT</th>\r\n";
            detailLists += "		<th>Mã Vật Tư</th>\r\n";
            detailLists += "		<th>Tên Vật Tư</th>\r\n";
            detailLists += "		<th>Giá Xuất</th>\r\n";
            detailLists += "		<th>Danh Mục</th>\r\n";
            detailLists += "		<th>Nhóm</th>\r\n";
            detailLists += "		<th>Ngày Tạo</th>\r\n";
            detailLists += "		<th>Ngày Cập Nhật</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            var index = 1;

            foreach (var item in data)
            {
                decimal? subtotal = item.PriceOutbound;
                tong_tien += subtotal;
                detailLists += "<tr>\r\n"
                + "<td class=\"text-center orderNo\">" + (index++) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Code + "</td>\r\n"
                + "<td class=\"text-left \">" + item.Name + "</td>\r\n"
                + "<td class=\"text-left\">" + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(item.PriceOutbound, null) + "</td>\r\n"
                + "<td class=\"text-left \">" + item.CategoryCode + "</td>\r\n"
                + "<td class=\"text-left \">" + item.ProductGroup + "</td>\r\n"
                + "<td>" + (item.CreatedDate.HasValue ? item.CreatedDate.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
                + "<td>" + (item.ModifiedDate.HasValue ? item.ModifiedDate.Value.ToString("dd/MM/yyyy HH:mm") : "") + "</td>"
                + "</tr>\r\n";
            }
            detailLists += "</tbody>\r\n";
            detailLists += "<tfoot style=\"font-weight:bold\">\r\n";
            detailLists += "<tr \"  style=\"font-weight:bold\"><td colspan=\"3\" class=\"text-right\">Tổng cộng</td><td class=\"text-right\">"
                        + Erp.BackOffice.Helpers.CommonSatic.ToCurrencyStr(tong_tien, null)
                        + "</td></tr>\r\n";

            detailLists += "</tfoot>\r\n</table>\r\n";

            return detailLists;
        }
        #endregion
    }
}
