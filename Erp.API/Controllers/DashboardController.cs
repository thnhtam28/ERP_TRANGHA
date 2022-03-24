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

namespace Erp.API.Controllers
{
    public class DashboardController : ApiController
    {
        [AcceptVerbs("GET")]
        public HttpResponseMessage WarningDay(string StartDate, string EndDate, int UserId)
        {
            StaffMadeRepository staffMadeRepository = new StaffMadeRepository(new Erp.Domain.Sale.ErpSaleDbContext());
            ProductInvoiceRepository productInvoiceRepository = new ProductInvoiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());
         
            SaleReportWarningDayViewModel model = new SaleReportWarningDayViewModel();
            var time = DateTime.Now;
            DateTime daytime = new DateTime(time.Year, time.Month, time.Day, 0, 0, 0);
            string startday;
            string endday;
            if (string.IsNullOrEmpty(StartDate) && string.IsNullOrEmpty(EndDate))
            {
                startday = daytime.ToString();
                endday = daytime.ToString();
            }
            else
            {
                startday = StartDate;
                endday = EndDate;
            }

            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startday, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endday, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    var Transaction = productInvoiceRepository.GetAllvwProductInvoice();

                    var invoice1 = Transaction.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate && x.CreatedUserId == UserId&&x.ManagerStaffId==UserId && x.IsArchive).ToList();
                    var invoice2 = Transaction.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate && x.ManagerStaffId != UserId && x.CreatedUserId == UserId && x.IsArchive).ToList();
                    //số hóa đơn người tạo, người quản lý khách hàng là cùng nhân viên
                    model.InvoiceTotalAmountbyCreateAndManager = invoice1 == null ? 0 : invoice1.Sum(x => x.TotalAmount);//tổng tiền
                    model.CountInvoicebyCreateAndManager = invoice1 == null ? 0 : invoice1.Count();//tổng hóa đơn
                    //số hóa đơn người tạo là nhân viên đang đăng nhập nhưng người quản lý khách hàng thì ko phải
                    model.InvoiceTotalAmountbyCreateAndManager = invoice2 == null ? 0 : invoice2.Sum(x => x.TotalAmount);//tổng tiền
                    model.CountInvoicebyCreateAndManager = invoice2 == null ? 0 : invoice2.Count();//tổng hóa đơn

                    var schedulingHistory = staffMadeRepository.GetvwAllStaffMade().Where(x => x.UserId == UserId && x.Status == "accept" && x.SchedulingStatus == "complete").Select(x => new StaffMadeViewModel { 
                    Id=x.Id,
                    WorkDay=x.WorkDay,
                    EndDate=x.EndDate
                    }).ToList();
                    //số dịch vụ mà nhân viên đã tham gia
                    model.CountSchedulingHistory = schedulingHistory == null ? 0 : schedulingHistory.Count();
                    foreach (var item in schedulingHistory)
                    {
                        item.TotalMinute = Convert.ToInt32(item.EndDate.Value.Subtract(item.WorkDay.Value).TotalMinutes);
                    }
                    int tot_mins = schedulingHistory == null ? 0 : schedulingHistory.Sum(x => x.TotalMinute.Value);
                    int days = tot_mins / 1440;
                    int hours = (tot_mins % 1440) / 60;
                    int mins = tot_mins % 60;
                    //tổng số ngày,giờ, phút mà nhân viên đã làm
                    model.TotalDayHourMinute = days+"ngày "+hours+"giờ "+mins+"phút";
                }
            }
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }

    }
}