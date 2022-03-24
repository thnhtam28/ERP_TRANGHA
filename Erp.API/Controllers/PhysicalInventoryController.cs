using Erp.API.Models;
using Erp.Domain.Account.Repositories;
using Erp.Domain.Entities;
using Erp.Domain.Helper;
using Erp.Domain.Interfaces;
using Erp.Domain.Repositories;
using Erp.Domain.Sale.Entities;
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
using Erp.Domain.Account.Entities;
using System.Reflection;
using System.Web.Hosting;
namespace Erp.API.Controllers
{
    public static class StringExtention
    {
        public static string ToLowerOrEmpty(this string input)
        {
            return (input != null ? input.ToLower() : "");
        }
    }

    public class PhysicalInventoryController : ApiController
    {
        #region Create
        [HttpPost]
        public string Create([FromBody] PhysicalInventoryViewModel model)
        {
            try
            {
                if (model.DetailList.Count != 0)
                {
                    #region Khởi tạo Repository
                    ProductRepository ProductRepository = new ProductRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    InventoryRepository InventoryRepository = new InventoryRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    PhysicalInventoryRepository PhysicalInventoryRepository = new PhysicalInventoryRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    UserRepository UserRepository = new UserRepository(new Domain.ErpDbContext());
                    #endregion

                    if (model.CreatedUserId != null)
                    {
                        var User = UserRepository.GetUserById(model.CreatedUserId.Value);
                        model.BranchId = User.BranchId;
                    }

                    PhysicalInventory PhysicalInventory = new PhysicalInventory();
                    PhysicalInventory.IsDeleted = false;
                    PhysicalInventory.CreatedUserId = model.CreatedUserId;
                    PhysicalInventory.CreatedDate = DateTime.Now;
                    PhysicalInventory.ModifiedUserId = model.CreatedUserId;
                    PhysicalInventory.ModifiedDate = DateTime.Now;
                    PhysicalInventory.Code = model.Code;
                    PhysicalInventory.Note = model.Note;
                    PhysicalInventory.Status = "Khởi tạo";
                    PhysicalInventory.BranchId = model.BranchId;
                    PhysicalInventory.IsExchange = false;
                    PhysicalInventory.IsCompleted = false;
                    List<PhysicalInventoryDetail> PhysicalInventoryDetailList = new List<PhysicalInventoryDetail>();
                    var inventory = InventoryRepository.GetAllInventory().ToList();
                    foreach (var item in inventory)
                    {
                        var Product = ProductRepository.GetProductById(item.ProductId.Value);
                        var detailProduct = model.DetailList.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                        if (detailProduct != null)
                        {
                            PhysicalInventoryDetailList.Add(new PhysicalInventoryDetail
                            {
                                CreatedDate = DateTime.Now,
                                CreatedUserId = model.CreatedUserId,
                                IsDeleted = false,
                                Note = detailProduct.Note,
                                ProductId = Product.Id,
                                WarehouseId = item.WarehouseId.Value,
                                QuantityInInventory = item.Quantity.Value,
                                QuantityRemaining = detailProduct.QuantityRemaining,
                                QuantityDiff = detailProduct.QuantityRemaining - item.Quantity.Value
                            });
                        }
                        else
                        {
                            PhysicalInventoryDetailList.Add(new PhysicalInventoryDetail
                            {
                                CreatedDate = null,
                                CreatedUserId = model.CreatedUserId,
                                IsDeleted = false,
                                Note = detailProduct.Note,
                                ProductId = Product.Id,
                                WarehouseId = item.WarehouseId.Value,
                                QuantityInInventory = item.Quantity.Value,
                                QuantityRemaining = 0,
                                QuantityDiff = 0
                            });
                        }
                    }
                    PhysicalInventoryRepository.InsertPhysicalInventory(PhysicalInventory, PhysicalInventoryDetailList);

                    //cập nhật lại mã kiểm kho
                    PhysicalInventory.Code = Erp.API.Helpers.Common.GetOrderNo("InventoryCheck");
                    PhysicalInventoryRepository.UpdatePhysicalInventory(PhysicalInventory);
                    Erp.API.Helpers.Common.SetOrderNo("InventoryCheck");

                    return "success";
                }
                return "failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Update
        [HttpPost]
        public string Update([FromBody] PhysicalInventoryViewModel model)
        {
            try
            {
                if (model.DetailList.Count != 0)
                {
                    #region Khởi tạo Repository
                    ProductRepository ProductRepository = new ProductRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    InventoryRepository InventoryRepository = new InventoryRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    PhysicalInventoryRepository PhysicalInventoryRepository = new PhysicalInventoryRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    UserRepository UserRepository = new UserRepository(new Domain.ErpDbContext());
                    #endregion

                    if (model.CreatedUserId != null)
                    {
                        var User = UserRepository.GetUserById(model.CreatedUserId.Value);
                        model.BranchId = User.BranchId;
                    }

                    PhysicalInventory PhysicalInventory = PhysicalInventoryRepository.GetPhysicalInventoryById(model.Id);
                    List<PhysicalInventoryDetail> PhysicalInventoryDetailList = PhysicalInventoryRepository.GetAllPhysicalInventoryDetail(PhysicalInventory.Id).ToList();
                    foreach (var item in model.DetailList)
                    {
                        //var inventory = InventoryRepository.GetInventoryByProductId(item.ProductId);
                        var PhysicalInventoryDetai = PhysicalInventoryDetailList.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
                        if (PhysicalInventoryDetai != null)
                        {
                            PhysicalInventoryDetai.CreatedDate = DateTime.Now;
                            PhysicalInventoryDetai.CreatedUserId = model.CreatedUserId;
                            PhysicalInventoryDetai.Note = item.Note;
                            //PhysicalInventoryDetai.QuantityInInventory = inventory.Quantity.Value;
                            PhysicalInventoryDetai.QuantityRemaining += item.QuantityRemaining;
                            PhysicalInventoryDetai.QuantityDiff = item.QuantityRemaining - PhysicalInventoryDetai.QuantityInInventory;

                        }
                        PhysicalInventoryRepository.UpdatePhysicalInventoryDetail(PhysicalInventoryDetai);
                    }
                    //cập nhật lại mã kiểm kho
                    PhysicalInventory.Code = Erp.API.Helpers.Common.GetOrderNo("InventoryCheck");
                    PhysicalInventoryRepository.UpdatePhysicalInventory(PhysicalInventory);
                    Erp.API.Helpers.Common.SetOrderNo("InventoryCheck");

                    return "success";
                }
                return "failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        public HttpResponseMessage Get(string ProductCode)
        {
            var resp = new HttpResponseMessage(HttpStatusCode.OK);

            ProductRepository productRepository = new ProductRepository(new Domain.Sale.ErpSaleDbContext());
            var model = productRepository.GetAllvwProduct()
                .Where(item => item.Type == "product" && item.Code == ProductCode).FirstOrDefault();
            if (model != null)
            {
                model.Image_Name = Helpers.Common.GetSetting("url_upload_path_PurchaseOrderAPI") + model.Image_Name;
            }

            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
    }
}