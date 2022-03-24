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
    public class ServiceOrderController : ApiController
    {
        #region GetList
        public HttpResponseMessage GetList(int AssignedUserId, string txtProductCode, string startDate, string endDate, int page = 1, int numberPerPage = 10)
        {
            AssignedRepository assignedRepository = new Domain.Sale.Repositories.AssignedRepository(new Domain.Sale.ErpSaleDbContext());
            AssignServiceRepository assignServiceRepository = new AssignServiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());

            var q = assignedRepository.GetAllvwAssigned()
                .Where(item => item.AssignedUserId == AssignedUserId);

            if (!string.IsNullOrEmpty(txtProductCode))
            {
                txtProductCode = txtProductCode.Trim().ToUpper();
                q = q.Where(x => x.ProductCode.Contains(txtProductCode));
            }

            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.AssignedDate >= d_startDate && x.AssignedDate <= d_endDate);
                }
            }

            var model = q.OrderBy(m => m.AssignedDate)
                .Skip((page - 1) * numberPerPage)
                .Take(numberPerPage)
                .Select(item=> new AssignedViewModel
                {
                    Id = item.Id,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    Image = item.ProductImage,
                    LaborAmount = item.LaborAmount,
                    LaborPaidAmount = item.LaborPaidAmount,
                    LaborRemainAmount = item.LaborRemainAmount,
                    AssignedUserId = item.AssignedUserId,
                    CreatedUserId = item.CreatedUserId,
                    AssignedDate = item.AssignedDate,
                    Status = item.Status,
                    Note = item.Note,
                    IsApproved = item.IsApproved,
                    IsPayment = item.IsPayment
                }).ToList();

            foreach(var item in model)
            {
                item.Image = Helpers.Common.GetSetting("url_upload_path_PurchaseOrderAPI") + item.Image;
                var list = assignServiceRepository.GetAllvwAssignService()
                    .Where(i => i.AssignedId == item.Id)
                    .Select(i => new AssignServiceViewModel
                    {
                        Id = i.Id,
                        AssignedUserId = item.AssignedUserId,
                        CreatedUserId = item.CreatedUserId,
                        ServiceName = i.ServiceName,
                        Price = i.Price,
                        AdjustmentPrice = i.AdjustmentPrice,
                        AdjustmentType = i.AdjustmentType,
                        Status = i.Status,
                        Note = i.Note
                    }).ToList();
                item.AssignService = list;
            }

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region Update
        [HttpPost]
        public string Update([FromBody] AssignService model)
        {
            try
            {
                AssignServiceRepository AssignServiceRepository = new AssignServiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                AssignedRepository assignedRepository = new AssignedRepository(new Domain.Sale.ErpSaleDbContext());

                var AssignService = AssignServiceRepository.GetAssignServiceById(model.Id);

                if (AssignService != null)
                {
                    AssignService.Status = model.Status;
                    AssignServiceRepository.UpdateAssignService(AssignService);

                    var LaborAmount = AssignServiceRepository.GetAllAssignService().Where(x => x.AssignedId == AssignService.AssignedId && x.Status == "Hoàn thành").Sum(item => item.AdjustmentPrice);
                    var assigned = assignedRepository.GetAssignedById(AssignService.AssignedId.Value);
                    assigned.LaborAmount = LaborAmount;
                    assigned.LaborPaidAmount = 0;
                    assigned.LaborRemainAmount = assigned.LaborAmount;

                    assignedRepository.UpdateAssigned(assigned);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }   
        }
        #endregion

        #region Add
        [HttpPost]
        public string Add([FromBody] AssignService model)
        {
            try
            {
                ProductRepository ProductRepository = new Domain.Sale.Repositories.ProductRepository(new Domain.Sale.ErpSaleDbContext());                
                AssignServiceRepository AssignServiceRepository = new AssignServiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                AssignedRepository assignedRepository = new AssignedRepository(new Domain.Sale.ErpSaleDbContext());

                var Service = ProductRepository.GetProductById(model.ServiceId.Value);                

                AssignService AssignService = new AssignService();
                AssignService.IsDeleted = false;
                AssignService.CreatedUserId = model.CreatedUserId;
                AssignService.ModifiedUserId = model.CreatedUserId;
                AssignService.CreatedDate = DateTime.Now;
                AssignService.ModifiedDate = DateTime.Now;
                AssignService.AssignedUserId = model.AssignedUserId;
                AssignService.AssignedId = model.AssignedId;
                AssignService.ServiceId = model.ServiceId;
                AssignService.Price = model.Price;
                AssignService.AdjustmentPrice = model.Price;
                AssignService.Percent = 0;
                AssignService.Status = model.Status;
                AssignService.AdjustmentPrice = AssignService.Price;
                AssignService.PriceLog = Service.PriceInbound + "," + Service.PriceInbound2 + "," + Service.PriceInbound3;
                AssignServiceRepository.InsertAssignService(AssignService);

                var LaborAmount = AssignServiceRepository.GetAllAssignService().Where(x => x.AssignedId == model.AssignedId && x.Status == "Hoàn thành").Sum(item => item.AdjustmentPrice);

                var assigned = assignedRepository.GetAssignedById(model.AssignedId.Value);
                assigned.LaborAmount = LaborAmount;
                assigned.LaborPaidAmount = 0;
                assigned.LaborRemainAmount = assigned.LaborAmount;

                assignedRepository.UpdateAssigned(assigned);

                return AssignService.Id.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Update
        [HttpPost]
        public string Delete([FromBody] AssignService model)
        {
            try
            {
                AssignServiceRepository AssignServiceRepository = new AssignServiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                AssignedRepository assignedRepository = new AssignedRepository(new Domain.Sale.ErpSaleDbContext());

                var AssignService = AssignServiceRepository.GetAssignServiceById(model.Id);

                if (AssignService != null)
                {
                    var LaborAmount = AssignServiceRepository.GetAllAssignService().Where(x => x.AssignedId == AssignService.AssignedId && x.Status == "Hoàn thành").Sum(item => item.AdjustmentPrice);
                    var assigned = assignedRepository.GetAssignedById(AssignService.AssignedId.Value);
                    assigned.LaborAmount = LaborAmount;
                    assigned.LaborPaidAmount = 0;
                    assigned.LaborRemainAmount = assigned.LaborAmount;

                    assignedRepository.UpdateAssigned(assigned);

                    AssignServiceRepository.DeleteAssignService(AssignService.Id);
                }
                return "success";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion
    }
}