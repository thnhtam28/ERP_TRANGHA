using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Erp.Domain.Entities;
using System.Globalization;
using Erp.BackOffice.Helpers;
using System;
using Erp.BackOffice.Models;
using Erp.Domain.Interfaces;
using OfficeOpenXml;
using OfficeOpenXml.Style;
using System.Drawing;
using System.Web.Security;
using WebMatrix.WebData;
using Erp.Utilities;
using System.IO;
using System.Drawing.Text;
using System.Drawing.Drawing2D;
using Erp.BackOffice.App_GlobalResources;
using Erp.Domain.Staff.Interfaces;
using Erp.BackOffice.Staff.Models;
using Erp.Domain.Helper;
using Erp.Domain.Sale.Interfaces;
using Erp.BackOffice.Administration.Models;
using Erp.Domain.Crm.Interfaces;
using Erp.BackOffice.Crm.Models;
using Erp.BackOffice.Areas.Cms.Models;
using Erp.BackOffice.Areas.Administration.Models;
using Erp.BackOffice.Sale.Models;
using Erp.Domain.Crm.Repositories;
using System.Web;

namespace Erp.BackOffice.Controllers
{
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class HomeController : Controller
    {
        private readonly IPageMenuRepository _pageMenuRepository;
        private readonly IUserRepository _userRepository;
        private readonly ILoginLogRepository _loginlogRepository;
        private readonly IInternalNotificationsRepository internalNNotificationsRepository;
        private readonly IStaffsRepository staffRepository;
        private readonly ITaskRepository taskRepository;
        private readonly ICategoryRepository _categoryRepository;
        private readonly IUserTypeRepository userTypeRepository;
        private readonly IWarehouseRepository WarehouseRepository;
        private readonly IUserRepository userRepository;
        public HomeController(
            IUserRepository userRepo
            , IPageMenuRepository pageMenuRepository
            , ILoginLogRepository loginlog
            , IInternalNotificationsRepository internalNotifications
            , IStaffsRepository staffs
            , ITaskRepository task
             , ICategoryRepository categoryRepository
            , IUserTypeRepository userType
            , IWarehouseRepository _Warehouse
            , IUserRepository _user
            )
        {
            _pageMenuRepository = pageMenuRepository;
            _loginlogRepository = loginlog;
            _userRepository = userRepo;
            internalNNotificationsRepository = internalNotifications;
            staffRepository = staffs;
            taskRepository = task;
            _categoryRepository = categoryRepository;
            userTypeRepository = userType;
            WarehouseRepository = _Warehouse;
            userRepository = _user;
        }

        [Authorize]
        public ActionResult Index()
        {

            string home_page = Helpers.Common.GetSetting("home_page");
            var id = Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId;
            //var usertype = userTypeRepository.GetUserTypeById(id);
            if (id == 21)
            {
                var url = "/Customer/Client";
                return Redirect(url);
            }
            return Redirect(home_page);
        }

        [Authorize]
        public ActionResult Dashboard(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }

        [Authorize]
        public ActionResult DashboardInventory()
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

            
            //
            var warehouseList = WarehouseRepository.GetvwAllWarehouse().AsEnumerable();
            warehouseList = warehouseList.Where(x => x.Categories != "VT").ToList();
            if(intBrandID > 0)
            {
                warehouseList = warehouseList.Where(x => x.BranchId == intBrandID).ToList();
            }
            var _warehouseList = warehouseList.Select(item => new SelectListItem
            {
                Text = item.Name + " (" + item.BranchName + ")",
                Value = item.Id.ToString()
            });
            ViewBag.warehouseList = _warehouseList;
            return View();
        }
        [Authorize]
        public ActionResult DashboardMaterialInventory()
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

            var warehouseList = WarehouseRepository.GetvwAllWarehouse().AsEnumerable();
            warehouseList = warehouseList.Where(x => x.Categories == "VT").ToList();
            if(intBrandID > 0)
            {
                warehouseList = warehouseList.Where(x => x.BranchId == intBrandID).ToList();
            }
            var _warehouseList = warehouseList.Select(item => new SelectListItem
            {
                Text = item.Name + " (" + item.BranchName + ")",
                Value = item.Id.ToString()
            });
            ViewBag.warehouseList = _warehouseList;
            return View();
        }
        [Authorize]
        public ActionResult DashboardStaff()
        {
            return View();
        }
        [Authorize]
        public ActionResult DashboardSalary()
        {
            return View();
        }
        [Authorize]
        public ActionResult DashboardSale()
        {
            return View();
        }
        [Authorize]
        public ActionResult DashboardCrm()
        {
            var category = _categoryRepository.GetCategoryByCode("task_status").Select(x => new CategoryViewModel
            {
                Code = x.Code,
                Description = x.Description,
                Id = x.Id,
                Name = x.Name,
                Value = x.Value,
                OrderNo = x.OrderNo
            }).OrderBy(x => x.OrderNo).ToList();
            ViewBag.Category = category;
            return View();
        }
        [Authorize]
        public ActionResult DashboardReport()
        {
            return View();
        }
        [Authorize]
        public ActionResult DashboardTransaction()
        {
            return View();
        }

        [Authorize]
        public ActionResult TrackRequest()
        {
            var model = ControllerContext.HttpContext.Application["ListRequest"] as List<RequestInfo>;
            return View(model.OrderByDescending(item => item.FirstDate).ToList());
        }
        [Authorize]
        public ActionResult Breadcrumb(List<Erp.BackOffice.Areas.Administration.Models.PageMenuViewModel> DataMenuItem, string controllerName, string actionName, string areaName)
        {
            var model = new List<BreadcrumbViewModel>();

            var currentUrl = ("/" + controllerName + "/" + actionName).ToLower();
            var pageMenuItem = DataMenuItem.Where(x => (x.Url != null && x.Url.ToLower() == currentUrl) || (x.PageUrl != null && x.PageUrl.ToLower() == currentUrl)).FirstOrDefault();

            if (pageMenuItem == null)
            {
                currentUrl = ("/" + controllerName + "/index").ToLower();
                pageMenuItem = DataMenuItem.Where(x => x.PageUrl != null && x.PageUrl.ToLower() == currentUrl).FirstOrDefault();
            }

            while (pageMenuItem != null)
            {
                var breadCrumb = new BreadcrumbViewModel
                {
                    Area = pageMenuItem.AreaName,
                    ParrentId = pageMenuItem.ParentId,
                    Name = pageMenuItem.Name,
                    Url = pageMenuItem.PageUrl
                };

                model.Insert(0, breadCrumb);

                if (pageMenuItem.ParentId != null)
                {
                    pageMenuItem = DataMenuItem.Where(x => x.Id == pageMenuItem.ParentId.Value).FirstOrDefault();
                }
                else
                {
                    pageMenuItem = null;
                }
            }


            return PartialView("_BreadcrumbPartial", model);

            //var page = _pageMenuRepository.GetPageByAction(areaName, controllerName, actionName);

            //if (page != null)
            //{
            //    var model = new BreadcrumbViewModel
            //    {
            //        Area = page.AreaName,
            //        Controller = page.ControllerName,
            //        Action = page.ActionName,
            //        ParrentId = page.ParentId,
            //        Name = _pageMenuRepository.GetPageName(page.Id, Session["CurrentLanguage"].ToString()),
            //        Url = page.Url
            //    };
            //    return PartialView("_BreadcrumbPartial", model);
            //}

            //return PartialView("_BreadcrumbPartial", null);
        }

        public ActionResult BreadcrumbParentPage(int parrentId)
        {
            var page = _pageMenuRepository.GetPageById(parrentId);

            if (page != null)
            {
                var model = new BreadcrumbViewModel
                {
                    Area = page.AreaName,
                    Controller = page.ControllerName,
                    Action = page.ActionName,
                    ParrentId = page.ParentId,
                    Name = _pageMenuRepository.GetPageName(page.Id, Session["CurrentLanguage"].ToString())
                };
                return PartialView("_BreadcrumbParrentPagePartial", model);
            }

            return PartialView("_BreadcrumbParrentPagePartial", null);
        }

        [AllowAnonymous]
        public ActionResult _ClosePopup()
        {
            return View();
        }

        [Authorize]
        public ActionResult Notifications()
        {

            var user = _userRepository.GetUserById(WebSecurity.CurrentUserId);
            //xóa thông báo quá hạn.
            DeleteNotifications(user.Id);
            //lấy danh sách thông báo của user hiện lên.
            var q = taskRepository.GetAllvwTask().Where(x => x.ModifiedUserId == user.Id && x.Type == "notifications").AsEnumerable()
                .Select(x => new TaskViewModel
                {
                    AssignedUserId = x.AssignedUserId,
                    CreatedDate = x.CreatedDate,
                    CreatedUserId = x.CreatedUserId,
                    FullName = x.FullName,
                    ProfileImage = Erp.BackOffice.Helpers.Common.KiemTraTonTaiHinhAnh(x.ProfileImage, "Staff", "user"),
                    Id = x.Id,
                    IsDeleted = x.IsDeleted,
                    ParentId = x.ParentId,
                    ParentType = x.ParentType,
                    Subject = x.Subject,
                    UserName = x.UserName,
                    ModifiedUserId = x.ModifiedUserId
                }).OrderByDescending(x => x.CreatedDate).ToList();
            q = q.Take(50).ToList();
            return View(q);
        }

        #region xóa thông báo quá hạn
        public static void DeleteNotifications(int? userID)
        {
            //chỉ xóa những tin nhắn quá hạn mà đã xem.
            Erp.Domain.Crm.Repositories.TaskRepository taskRepository = new Erp.Domain.Crm.Repositories.TaskRepository(new Domain.Crm.ErpCrmDbContext());
            var quantityDate = Helpers.Common.GetSetting("quantity_notifications_date");
            var date = DateTime.Now.AddDays(Convert.ToInt32(quantityDate));
            var notifications = taskRepository.GetAllTaskFull().Where(x => x.AssignedUserId == userID && x.Type == "notifications").ToList();
            notifications = notifications.Where(x => x.CreatedDate <= date).ToList();
            for (int i = 0; i < notifications.Count(); i++)
            {
                taskRepository.DeleteTask(notifications[i].Id);
            }

        }
        #endregion

        public ActionResult InteractiveChart()
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


            var user = userRepository.GetAllUsers().Select(item => new UserViewModel
            {
                Id = item.Id,
                UserName = item.UserName,
                FullName = item.FullName,
                Status = item.Status,
                BranchId = item.BranchId
            }).Where(x => x.Status == UserStatus.Active && listNhanvien.Contains(x.Id)).ToList();


            ViewBag.user = user;
            ViewBag.batbuoc = "Bắt buộc nhập";


            return View();

        }

        public List<InteractiveChartViewModel> GetList_InteractiveChart(string StartDate, string EndDate, string CityId, string DistrictId, int? branchId, int? NGUOILAP, string HINHTHUC_TUONGTAC)
        {
            // sau nay nếu có bổ xung ???
            CityId = string.IsNullOrEmpty(CityId) ? "" : CityId;
            DistrictId = string.IsNullOrEmpty(DistrictId) ? "" : DistrictId;
            branchId = branchId == null ? 0 : branchId;
            NGUOILAP = NGUOILAP == null ? 0 : NGUOILAP;
            if (!Filters.SecurityFilter.IsAdmin() && branchId == 0)
            {
                branchId = Helpers.Common.CurrentUser.BranchId;
            }

            var d_startDate = DateTime.ParseExact(StartDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            var d_endDate = DateTime.ParseExact(EndDate, "dd/MM/yyyy", null).ToString("yyyy-MM-dd HH:mm:ss");
            HINHTHUC_TUONGTAC = string.IsNullOrEmpty(HINHTHUC_TUONGTAC) ? "" : HINHTHUC_TUONGTAC;

            var data = HINHTHUC_TUONGTAC == "GOIDIEN" ? SqlHelper.QuerySP<InteractiveChartViewModel>("spGetList_InteractiveChart_phone", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                HINHTHUC_TUONGTAC = HINHTHUC_TUONGTAC,
                NGUOILAP = NGUOILAP,
                branchId = branchId
            }).ToList() : SqlHelper.QuerySP<InteractiveChartViewModel>("spGetList_InteractiveChart", new
            {
                StartDate = d_startDate,
                EndDate = d_endDate,
                HINHTHUC_TUONGTAC = HINHTHUC_TUONGTAC,
                NGUOILAP = NGUOILAP,
                branchId = branchId
            }).ToList();
            ViewBag.StartDate = StartDate;
            ViewBag.EndDate = EndDate;

            return data;
        }

        public PartialViewResult _GetList_InteractiveChart(string StartDate, string EndDate, string CityId, string DistrictId, int? branchId, int? NGUOILAP, string HINHTHUC_TUONGTAC)
        {
            ViewBag.hinhthuctuongtac = HINHTHUC_TUONGTAC;
            var data = GetList_InteractiveChart(StartDate, EndDate, CityId, DistrictId, branchId, NGUOILAP, HINHTHUC_TUONGTAC);
            return PartialView(data);
        }
        [HttpPost]
        public ActionResult _Set_GlobalCurentBranchId(int? branchId)
        {

            Session["GlobalCurentBranchId"] = branchId;


            return View();
        }
    }
}
