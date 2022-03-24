using System.Globalization;
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
using Erp.BackOffice.Areas.Cms.Models;
using Erp.Domain.Crm.Interfaces;
using Erp.BackOffice.Crm.Models;
using System.Transactions;
using System.Text.RegularExpressions;
using Erp.Domain.Account.Interfaces;
using System.Web;

//using Erp.Domain.Account.Repositories;

namespace Erp.BackOffice.Sale.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class AdviseCardController : Controller
    {
        private readonly IAdviseCardRepository AdviseCardRepository;
        private readonly IUserRepository userRepository;
        private readonly ICategoryRepository categoryRepository;
        private readonly IQuestionRepository questionRepository;
        private readonly IAnswerRepository answerRepository;
        private readonly IAdviseCardDetailRepository adviseCardDetailRepository;
        private readonly ITemplatePrintRepository templatePrintRepository;
        private readonly ICustomerRepository customerRepository;
        public AdviseCardController(
            IAdviseCardRepository _AdviseCard
            , IUserRepository _user
            , ICategoryRepository category
            , IQuestionRepository question
            , IAnswerRepository answer
            , IAdviseCardDetailRepository adviseCardDetail
                     , ITemplatePrintRepository templatePrint
            , ICustomerRepository customer
            )
        {
            AdviseCardRepository = _AdviseCard;
            userRepository = _user;
            categoryRepository = category;
            questionRepository = question;
            answerRepository = answer;
            adviseCardDetailRepository = adviseCardDetail;
            templatePrintRepository = templatePrint;
            customerRepository = customer;
        }

        #region Index

        public ViewResult Index(string startDate, string endDate, string txtCode, string txtCusInfo, int? Status, int? BranchId, int? CounselorId, int? CreateUserId, string type)
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

            BranchId = int.Parse(strBrandID);

            if (startDate == null && endDate == null)
            {

                DateTime aDateTime = new DateTime(DateTime.Now.Year, 1, 1);
                // Cộng thêm 1 tháng và trừ đi một ngày.
                DateTime retDateTime = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1).AddMonths(1).AddDays(-1);



                startDate = aDateTime.ToString("dd/MM/yyyy");
                endDate = retDateTime.ToString("dd/MM/yyyy");
            }


            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {

            }
            if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
            {
                d_endDate = d_endDate.AddHours(23).AddMinutes(59);
            }

            if ((d_startDate == null) || (d_endDate == null))
            {
                return View();
            }

            IEnumerable<AdviseCardViewModel> q = AdviseCardRepository.GetvwAllAdviseCard().Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate)
                .Select(item => new AdviseCardViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    //CreatedUserName = item.CreatedUserName,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    //ModifiedUserName = item.ModifiedUserName,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    Note = item.Note,
                    //CustomerAddress=item.
                    //CustomerAddress=item.CustomerAddress,
                    //CustomerBirthday=item.CustomerBirthday,
                    CreatedUserCode = item.CreatedUserCode,
                    CounselorId = item.CounselorId,
                    CreatedUserName = item.CreatedUserName,
                    CounselorName = item.CounselorName,
                    IsActived = item.IsActived,
                    CustomerCode = item.CustomerCode,
                    CustomerName = item.CustomerName,
                    BranchId = item.BranchId,
                    Type = item.Type
                }).OrderByDescending(m => m.CreatedDate);

            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = Helpers.Common.ChuyenThanhKhongDau(txtCode);
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Code).Contains(txtCode)).ToList();
            }
            if (!string.IsNullOrEmpty(txtCusInfo))
            {
           
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(Helpers.Common.ChuyenThanhKhongDau(txtCusInfo)) || x.CustomerCode.Contains(txtCusInfo)).ToList();
            }
            //if (!string.IsNullOrEmpty(txtCusCode))
            //{            
            //        txtCusCode = Helpers.Common.ChuyenThanhKhongDau(txtCusCode);
            //        q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Code).Contains(txtCusCode)).ToList();                         
            //}
            if (!string.IsNullOrEmpty(type))
            {
                q = q.Where(x => x.Type == type).ToList();
            }
            //if (!string.IsNullOrEmpty(txtCusName))
            //{
            //    txtCusName = Helpers.Common.ChuyenThanhKhongDau(txtCusName);
            //    q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.CustomerName).Contains(txtCusName)).ToList();
            //}
          
            if (BranchId != null && BranchId.Value > 0)
            {
                q = q.Where(x => x.BranchId == BranchId).ToList();
            }

            //if (Helpers.Common.CurrentUser.BranchId != null)
            //{
            //    q = q.Where(x => x.BranchId == Helpers.Common.CurrentUser.BranchId.Value).ToList();
            //}

            if (CounselorId != null && CounselorId.Value > 0)
            {
                q = q.Where(x => x.CounselorId == CounselorId).ToList();
            }
            if (CreateUserId != null && CreateUserId.Value > 0)
            {
                q = q.Where(x => x.CreatedUserId == CreateUserId).ToList();
            }
            if (Status != null)
            {
                if (Status == 1)
                    q = q.Where(x => x.IsActived == true).ToList();
                else
                    q = q.Where(x => x.IsActived == false).ToList();
            }
            else
            {
                q = q.Where(x => x.IsActived == false).ToList();
            }
            ViewBag.SuccessMessage = TempData["SuccessMessage"];
            ViewBag.FailedMessage = TempData["FailedMessage"];
            ViewBag.AlertMessage = TempData["AlertMessage"];
            ViewBag.AdviseTypeError = TempData["AdviseTypeError"];
            return View(q);
        }
        #endregion

        #region Create
        public ViewResult Create(int? Id, AdviseCardViewModel model)
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
            //var AdviseCard = customerRepository.GetCustomerById(Id.Value);

            // var model = new AdviseCardViewModel();
            //var AdviseCard = AdviseCardRepository.GetvwAdviseCardById(Id.Value);
            if (Id == null)
            {
                //var model = new AdviseCardViewModel();
                model.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("AdviseCard");
                model.BranchId = intBrandID;
                return View(model);
            }
            else
            {
                var AdviseCard = customerRepository.GetCustomerById(Id.Value);

                var Service = new AdviseCard();
                AutoMapper.Mapper.Map(model, Service);
                //AdviseCard.IsDeleted = false;
                //Service.CreatedUserId = WebSecurity.CurrentUserId;
                //Service.ModifiedUserId = WebSecurity.CurrentUserId;
                //Service.AssignedUserId = WebSecurity.CurrentUserId;
                Service.IsDeleted = false;
                //Service.CreatedDate = DateTime.Now;
                //Service.ModifiedDate = DateTime.Now;
                //Service.IsActived = false;
                model.CustomerName =AdviseCard.CompanyName;
                model.CustomerId = AdviseCard.Id;
                Service.BranchId = AdviseCard.BranchId;
                 model.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("AdviseCard");

                AdviseCardRepository.InsertAdviseCard(Service);
                AdviseCardRepository.UpdateAdviseCard(Service);
                Erp.BackOffice.Helpers.Common.SetOrderNo("AdviseCard");

                //Service.c
                //Service.
                return View(model); 

            }
            return View();
            
        }

        [HttpPost]
        public ActionResult Create(AdviseCardViewModel model, bool? IsPopup)
        {
            var vwAdviseCard = AdviseCardRepository.GetvwAllAdviseCard();
            var q = vwAdviseCard.Select(item => new AdviseCardViewModel
            {
                Id = item.Id,
                CreatedUserId = item.CreatedUserId,
                CreatedDate = item.CreatedDate,
                ModifiedUserId = item.ModifiedUserId,
                //ModifiedUserName = item.ModifiedUserName,
                ModifiedDate = item.ModifiedDate,
                Code = item.Code,
                Note = item.Note,
                CreatedUserCode = item.CreatedUserCode,
                CounselorId = item.CounselorId,
                CreatedUserName = item.CreatedUserName,
                CounselorName = item.CounselorName,
                IsActived = item.IsActived,
                CustomerId = item.CustomerId,
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                BranchId = item.BranchId,
                Type = item.Type
            }).ToList();

            q = q.Where(x => x.CustomerId == model.CustomerId && x.BranchId== model.BranchId && x.Type == model.Type).ToList();

            foreach (var item in q)
            {
                if (item.Type == model.Type && item.BranchId == model.BranchId)
                {
                    if (IsPopup == true)
                    {
                        TempData["AdviseTypeError"] = "Đã tồn tại phiếu tư vấn có cùng loại tư vấn";
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                    }
                    else
                    {
                        TempData["AdviseTypeError"] = "Đã tồn tại phiếu tư vấn có cùng loại tư vấn";
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                    }
                }
            } 
            if (ModelState.IsValid)
            {      
                    var AdviseCard = new AdviseCard();
                    AutoMapper.Mapper.Map(model, AdviseCard);
                    AdviseCard.IsDeleted = false;
                    AdviseCard.CreatedUserId = WebSecurity.CurrentUserId;
                    AdviseCard.ModifiedUserId = WebSecurity.CurrentUserId;
                    AdviseCard.AssignedUserId = WebSecurity.CurrentUserId;
                    AdviseCard.CreatedDate = DateTime.Now;
                    AdviseCard.ModifiedDate = DateTime.Now;
                    AdviseCard.IsActived = false;
                    //AdviseCard.BranchId = Erp.BackOffice.Helpers.Common.CurrentUser.BranchId;
                    AdviseCardRepository.InsertAdviseCard(AdviseCard);
                    //cập nhật lại mã phiếu yêu cầu
                    AdviseCard.Code = Erp.BackOffice.Helpers.Common.GetOrderNo("AdviseCard", model.Code);
                    AdviseCardRepository.UpdateAdviseCard(AdviseCard);
                    Erp.BackOffice.Helpers.Common.SetOrderNo("AdviseCard");
                    ///
                    if (IsPopup == true)
                    {
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                    }
                    else
                    {
                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                        //gọi JavaScriptToRun để chạy code Jquery trong View
                        model.JavaScriptToRun = "CloseCurrentPopup("+ AdviseCard.Id +")";
                    //return RedirectToAction("Detail", "AdviseCard", new { Id = AdviseCard.Id });
                    return View(model);
                    }
                

            }
                
        
            return View(model);
        }
        #endregion

        #region Edit
        public ActionResult Edit(int? Id)
        {
            var AdviseCard = AdviseCardRepository.GetvwAdviseCardById(Id.Value);
            if (AdviseCard != null && AdviseCard.IsDeleted != true)
            {
                var model = new AdviseCardViewModel();
                AutoMapper.Mapper.Map(AdviseCard, model);

                //if (model.CreatedUserId != Helpers.Common.CurrentUser.Id && Helpers.Common.CurrentUser.UserTypeId != 1)
                //{
                //    TempData["FailedMessage"] = "NotOwner";
                //    return RedirectToAction("Index");
                //}                

                return View(model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult Edit(AdviseCardViewModel model, bool? IsPopup)
        {
            if (ModelState.IsValid)
            {
                if (Request["Submit"] == "Save")
                {
                    var AdviseCard = AdviseCardRepository.GetAdviseCardById(model.Id);
                    AutoMapper.Mapper.Map(model, AdviseCard);
                    AdviseCard.ModifiedUserId = WebSecurity.CurrentUserId;
                    AdviseCard.ModifiedDate = DateTime.Now;
                    AdviseCardRepository.UpdateAdviseCard(AdviseCard);
                    if (IsPopup == true)
                    {
                        return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                    }
                    else
                    {
                        TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.UpdateSuccess;
                        return RedirectToAction("Index");
                    }

                }

                return View(model);
            }

            return View(model);

            //if (Request.UrlReferrer != null)
            //    return Redirect(Request.UrlReferrer.AbsoluteUri);
            //return RedirectToAction("Index");
        }

        #endregion

        #region Detail
        public ActionResult Detail(int? Id)
        {
            var AdviseCard = AdviseCardRepository.GetvwAdviseCardById(Id.Value);
            var _category = categoryRepository.GetAllCategories();

            var _list_question = questionRepository.GetAllQuestion().Where(x => x.IsActivated == true)
                              .Select(item1 => new QuestionViewModel
                              {
                                  Id = item1.Id,
                                  Name = item1.Name,
                                  Type = item1.Type,
                                  Category = item1.Category,
                                  OrderNo = item1.OrderNo,
                                  Content = item1.Content
                              }).OrderByDescending(m => m.OrderNo);

            var _answer = answerRepository.GetAllAnswer().Where(x => x.IsActivated == true).Select(item1 => new AnswerViewModel
                              {
                                  Id = item1.Id,
                                  Content = item1.Content,
                                  QuestionId = item1.QuestionId,
                                  OrderNo = item1.OrderNo
                              }).OrderByDescending(m => m.OrderNo);

            if (AdviseCard != null && AdviseCard.IsDeleted != true)
            {
                var model = new AdviseCardViewModel();
                AutoMapper.Mapper.Map(AdviseCard, model);
                model.ListAdviseType = new List<CategoryViewModel>();
                model.ListAdviseType = _category.Where(x => x.Code == AdviseCard.Type).Select(x => new CategoryViewModel
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name,
                    OrderNo = x.OrderNo,
                    Value = x.Value,
                    ParentId = x.ParentId,
                    Level = 1
                }).OrderBy(x => x.OrderNo).ToList();

                foreach (var item in model.ListAdviseType)
                {
                    item.ListQuestion = _list_question.Where(x => x.Category == item.Value).OrderBy(x => x.OrderNo).ToList();
                    foreach (var item1 in item.ListQuestion)
                    {
                        item1.DetailList = _answer.Where(x => x.QuestionId == item1.Id).OrderBy(x => x.OrderNo).ToList();
                    }

                    var aa = _category.Where(x => x.Code == item.Value).Select(x => new CategoryViewModel
                    {
                        Code = x.Code,
                        Id = x.Id,
                        Name = x.Name,
                        OrderNo = x.OrderNo,
                        Value = x.Value,
                        ParentId = x.ParentId,
                        Level = 2
                    }).OrderBy(x => x.OrderNo).ToList();

                    if (aa.Count() > 0)
                    {
                        foreach (var q in aa)
                        {
                            q.ListQuestion = _list_question.Where(x => x.Category == q.Value).OrderBy(x => x.OrderNo).ToList();
                            foreach (var p in q.ListQuestion)
                            {
                                p.DetailList = _answer.Where(x => x.QuestionId == p.Id).OrderBy(x => x.OrderNo).ToList();
                            }
                        }
                        model.ListAdviseType = model.ListAdviseType.Union(aa).ToList();
                    }
                }
                return View(model);
            }



            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion

        #region Delete
        [HttpPost]
        public ActionResult Delete()
        {
            try
            {
                string idDeleteAll = Request["Delete"];
                string[] arrDeleteId = idDeleteAll.Split(',');
                for (int i = 0; i < arrDeleteId.Count(); i++)
                {
                    var item = AdviseCardRepository.GetAdviseCardById(int.Parse(arrDeleteId[i], CultureInfo.InvariantCulture));
                    if (item != null)
                    {
                        var list = adviseCardDetailRepository.GetAllAdviseCardDetailById(item.Id).ToList();
                        foreach (var q in list)
                        {
                            adviseCardDetailRepository.DeleteAdviseCardDetailRs(q.Id);
                        }
                        AdviseCardRepository.DeleteAdviseCardRs(item.Id);
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

        [HttpPost]
        public ActionResult SaveItem(int AdviseCardId, int QuestionId, int TargetId, string TargetModule, string content, string Note, bool IsChecked)
        {
            using (var scope = new TransactionScope(TransactionScopeOption.Required))
            {
                try
                {
                    var update = adviseCardDetailRepository.GetAdviseCardDetailById(AdviseCardId, QuestionId, TargetId);
                    if (update == null)
                    {
                        var add = new AdviseCardDetail();
                        add.IsDeleted = false;
                        add.CreatedUserId = WebSecurity.CurrentUserId;
                        add.ModifiedUserId = WebSecurity.CurrentUserId;
                        add.CreatedDate = DateTime.Now;
                        add.ModifiedDate = DateTime.Now;
                        add.Content = content;
                        add.Note = Note;
                        add.AdviseCardId = AdviseCardId;
                        add.QuestionId = QuestionId;
                        add.TargetId = TargetId;
                        add.TargetModule = TargetModule;
                        adviseCardDetailRepository.InsertAdviseCardDetail(add);
                    }
                    else
                    {
                        if (IsChecked)
                        {
                            update.ModifiedUserId = WebSecurity.CurrentUserId;
                            update.ModifiedDate = DateTime.Now;
                            update.Content = content;
                            update.Note = Note;
                            update.AdviseCardId = AdviseCardId;
                            update.QuestionId = QuestionId;
                            update.TargetId = TargetId;
                            update.TargetModule = TargetModule;
                            adviseCardDetailRepository.UpdateAdviseCardDetail(update);
                        }
                        else
                        {
                            adviseCardDetailRepository.DeleteAdviseCardDetail(update.Id);
                        }
                    }
                    scope.Complete();
                }
                catch (DbUpdateException)
                {
                    return Content("error");
                }
            }
            return Content("success");
        }

        #region  - LoadAdviseCardDetailById -
        public JsonResult LoadAdviseCardDetailById(int AdviseCardId)
        {
            var list = adviseCardDetailRepository.GetAllAdviseCardDetailById(AdviseCardId).Select(x => new
            {
                Id = x.Id,
                TargetId = x.TargetId,
                Content = x.Content,
                Note = x.Note,
                QuestionId = x.QuestionId,
                TargetModule = x.TargetModule
            }).ToList();

            return Json(list, JsonRequestBehavior.AllowGet);
        }
        #endregion

        #region Print
        public ActionResult Print(int? Id)
        {
            var AdviseCard = AdviseCardRepository.GetvwAdviseCardById(Id.Value);
            var _category = categoryRepository.GetAllCategories();

            var _list_question = questionRepository.GetAllQuestion().Where(x => x.IsActivated == true)
                              .Select(item1 => new QuestionViewModel
                              {
                                  Id = item1.Id,
                                  Name = item1.Name,
                                  Type = item1.Type,
                                  Category = item1.Category,
                                  OrderNo = item1.OrderNo,
                                  Content = item1.Content
                              }).OrderByDescending(m => m.OrderNo);

            var _answer = answerRepository.GetAllAnswer().Where(x => x.IsActivated == true).Select(item1 => new AnswerViewModel
            {
                Id = item1.Id,
                Content = item1.Content,
                QuestionId = item1.QuestionId,
                OrderNo = item1.OrderNo
            }).OrderByDescending(m => m.OrderNo);

            if (AdviseCard != null && AdviseCard.IsDeleted != true)
            {
                var model = new AdviseCardViewModel();
                AutoMapper.Mapper.Map(AdviseCard, model);
                model.ListAdviseType = new List<CategoryViewModel>();
                model.ListAdviseType = _category.Where(x => x.Code == AdviseCard.Type).Select(x => new CategoryViewModel
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name,
                    OrderNo = x.OrderNo,
                    Value = x.Value,
                    ParentId = x.ParentId,
                    Level = 1
                }).OrderBy(x => x.OrderNo).ToList();

                foreach (var item in model.ListAdviseType)
                {
                    item.ListQuestion = _list_question.Where(x => x.Category == item.Value).OrderBy(x => x.OrderNo).ToList();
                    foreach (var item1 in item.ListQuestion)
                    {
                        item1.DetailList = _answer.Where(x => x.QuestionId == item1.Id).OrderBy(x => x.OrderNo).ToList();
                    }

                    var aa = _category.Where(x => x.Code == item.Value).Select(x => new CategoryViewModel
                    {
                        Code = x.Code,
                        Id = x.Id,
                        Name = x.Name,
                        OrderNo = x.OrderNo,
                        Value = x.Value,
                        ParentId = x.ParentId,
                        Level = 2
                    }).OrderBy(x => x.OrderNo).ToList();

                    if (aa.Count() > 0)
                    {
                        foreach (var q in aa)
                        {
                            q.ListQuestion = _list_question.Where(x => x.Category == q.Value).OrderBy(x => x.OrderNo).ToList();
                            foreach (var p in q.ListQuestion)
                            {
                                p.DetailList = _answer.Where(x => x.QuestionId == p.Id).OrderBy(x => x.OrderNo).ToList();
                            }
                        }
                        model.ListAdviseType = model.ListAdviseType.Union(aa).ToList();
                    }
                }
                var list = adviseCardDetailRepository.GetAllAdviseCardDetailById(model.Id).Select(x => new AdviseCardDetailViewModel
                {
                    Id = x.Id,
                    TargetId = x.TargetId,
                    Content = x.Content,
                    Note = x.Note,
                    QuestionId = x.QuestionId,
                    TargetModule = x.TargetModule
                }).ToList();
                var _model = new TemplatePrintViewModel();
                //lấy logo công ty
                var logo = Erp.BackOffice.Helpers.Common.GetSetting("LogoCompany");
                var company = Erp.BackOffice.Helpers.Common.GetSetting("companyName");
                var address = Erp.BackOffice.Helpers.Common.GetSetting("addresscompany");
                var phone = Erp.BackOffice.Helpers.Common.GetSetting("phonecompany");
                var fax = Erp.BackOffice.Helpers.Common.GetSetting("faxcompany");
                var bankcode = Erp.BackOffice.Helpers.Common.GetSetting("bankcode");
                var bankname = Erp.BackOffice.Helpers.Common.GetSetting("bankname");
                var ImgLogo = "<div class=\"logo\"><img src=" + logo + " height=\"63\" /></div>";
                //lấy template phiếu xuất.
                var template = templatePrintRepository.GetAllTemplatePrint().Where(x => x.Code.Contains("AdviseCard")).OrderByDescending(x => x.CreatedDate).FirstOrDefault();
                //truyền dữ liệu vào template.
                _model.Content = template.Content;

                _model.Content = _model.Content.Replace("{DataTable}", buildHtml1(model, list));
                _model.Content = _model.Content.Replace("{System.Logo}", ImgLogo);
                _model.Content = _model.Content.Replace("{System.CompanyName}", company);
                _model.Content = _model.Content.Replace("{System.AddressCompany}", address);
                _model.Content = _model.Content.Replace("{System.PhoneCompany}", phone);
                _model.Content = _model.Content.Replace("{System.Fax}", fax);
                _model.Content = _model.Content.Replace("{System.BankCodeCompany}", bankcode);
                _model.Content = _model.Content.Replace("{System.BankNameCompany}", bankname);
                _model.Content = _model.Content.Replace("{PrintDate}", DateTime.Now.ToString("dd/MM/yyyy HH:mm"));
                _model.Content = _model.Content.Replace("{CustomerCode}", model.CustomerCode);
                _model.Content = _model.Content.Replace("{CustomerName}", model.CustomerName);
                _model.Content = _model.Content.Replace("{Note}", model.Note);
                _model.Content = _model.Content.Replace("{CreateStaffName}", model.CreatedUserName);
                _model.Content = _model.Content.Replace("{CounselorName}", model.CounselorName);
                _model.Content = _model.Content.Replace("{Title}", model.Type == "SkinScan" ? "PHIẾU CHĂM SÓC DA" : "PHIẾU CHĂM SÓC TÓC");

                return View(_model);
            }
            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion

        string buildHtml1(AdviseCardViewModel model, List<AdviseCardDetailViewModel> detail)
        {

            string detailLists = "";

            foreach (var item in model.ListAdviseType.Where(x => x.Level == 1))
            {
                detailLists += "<p><strong>" + item.Name + "</strong></p>\r\n";
                if (item.Value == "SkinScan1")
                {
                    detailLists += buildHtml_Partial1(item, detail);
                }
                if (item.Value == "SkinScan2" || item.Value == "SkinScan6" || item.Value == "CheckingHair1" || item.Value == "CheckingHair2" || item.Value == "CheckingHair3" || item.Value == "CheckingHair5")
                {
                    detailLists += buildHtml_Partial2(item, detail);
                }
                if (item.Value == "SkinScan3")
                {
                    detailLists += buildHtml_Partial3(item, detail);
                }
                if (item.Value == "SkinScan4")
                {
                    detailLists += buildHtml_Partial4(item, detail);
                }
                  if (item.Value == "SkinScan5")
                {
                    detailLists += buildHtml_Partial6(item, detail);
                }
                if (item.Value == "CheckingHair4")
                {
                    var list = model.ListAdviseType.Where(x => x.Level == 2 && x.Code == item.Value).ToList();
                    detailLists += buildHtml_Partial5(list, detail);
                }
                else
                {
                    if (model.ListAdviseType.Any(x => x.Level == 2 && x.Code == item.Value))
                    {
                        foreach (var ii in model.ListAdviseType.Where(x => x.Level == 2 && x.Code == item.Value))
                        {
                            detailLists += "<p>" + item.Name + "</p>\r\n";
                            detailLists += buildHtml_Partial2(item, detail);
                        }
                    }
                }
            }
            return detailLists;
        }
        string buildHtml_Partial1(CategoryViewModel item, List<AdviseCardDetailViewModel> detail)
        {
            string detailLists = "";
            detailLists += "<p><table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th>Nội dung</th>\r\n";
            detailLists += "		<th>Chi tiết</th>\r\n";
            detailLists += "		<th>Ghi chú</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            for (int i = 0; i < item.ListQuestion.Count(); i++)
            {
                // dữ liệu của khách hàng
                var data = detail.FirstOrDefault(x => x.QuestionId == item.ListQuestion[i].Id && x.TargetId == 0 && x.TargetModule == "Answer");
                //
                detailLists += "<tr class=\"text-left\"><td>" + item.ListQuestion[i].Name + "</td>";
                detailLists += "<td class=\"text-left\">" + (data == null ? "" : data.Content) + "</td><td class=\"text-left\">";
                detailLists += (data == null ? "" : data.Note) + "</td></tr>";
            }
            detailLists += "</tbody>\r\n</table>\r\n</p>";
            return detailLists;
        }
        string buildHtml_Partial2(CategoryViewModel item, List<AdviseCardDetailViewModel> detail)
        {
            string detailLists = "";
            foreach (var question in item.ListQuestion)
            {
                detailLists += "<p>" + question.Name + "\r\n";
                if (question.Type == "input")
                {
                    // dữ liệu của khách hàng
                    var data = detail.FirstOrDefault(x => x.QuestionId == question.Id && x.TargetId == 0 && x.TargetModule == "Answer");
                    //
                    detailLists += (data == null ? "" : data.Content) + "</p>\r\n";
                }
                if (question.Type == "check")
                {
                    foreach (var answer in question.DetailList)
                    {
                        // dữ liệu của khách hàng
                        var data = detail.FirstOrDefault(x => x.QuestionId == question.Id && x.TargetId == answer.Id && x.TargetModule == "Answer");
                        //
                        detailLists += "<input type=\"checkbox\" " + (data == null ? "" : "checked") + "/>" + answer.Content + "\r\n";
                    }
                    detailLists += "</p>\r\n";
                }
                if (question.Type == "all")
                {
                    foreach (var answer in question.DetailList)
                    {
                        // dữ liệu của khách hàng
                        var data = detail.FirstOrDefault(x => x.QuestionId == question.Id && x.TargetId == answer.Id && x.TargetModule == "Answer");
                        //
                        detailLists += "<input type=\"checkbox\" " + (data == null ? "" : "checked") + "/>" + answer.Content + "\r\n";
                        detailLists += "Content_" + question.Id + "_" + answer.Id + "\r\n";
                    }
                    detailLists += "</p>\r\n";
                }
            }
            return detailLists;
        }
        string buildHtml_Partial3(CategoryViewModel item, List<AdviseCardDetailViewModel> detail)
        {
            string detailLists = "";
            detailLists += "<p><table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th></th>\r\n";
            detailLists += "		<th>Trạng thái</th>\r\n";
            detailLists += "		<th>Chi tiết</th>\r\n";
            detailLists += "		<th>Ghi chú</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            for (int i = 0; i < item.ListQuestion.Count(); i++)
            {
                detailLists += "<tr class=\"text-left\"><td>" + item.ListQuestion[i].OrderNo + "</td>";
                detailLists += "<td class=\"text-left\">" + item.ListQuestion[i].Name + "</td>";
                detailLists += "<td class=\"text-left\">";
                foreach (var answer in item.ListQuestion[i].DetailList)
                {
                    // dữ liệu của khách hàng
                    var data = detail.FirstOrDefault(x => x.QuestionId == item.ListQuestion[i].Id && x.TargetId == answer.Id && x.TargetModule == "Answer");
                    //
                    detailLists += "<input type=\"checkbox\" " + (data == null ? "" : "checked") + "/>" + answer.Content + "\r\n";
                }
                detailLists += "</td><td>";
                // dữ liệu của khách hàng
                var _data = detail.FirstOrDefault(x => x.QuestionId == item.ListQuestion[i].Id && x.TargetId == 0 && x.TargetModule == "Answer");
                //
                detailLists += (_data == null ? "" : _data.Content) + "</td></tr>";
            }
            detailLists += "</tbody>\r\n</table>\r\n</p>";
            return detailLists;
        }
        string buildHtml_Partial4(CategoryViewModel item, List<AdviseCardDetailViewModel> detail)
        {
            string detailLists = "";
            detailLists += "<p><table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "		<th></th>\r\n";
            detailLists += "		<th>Chi tiết</th>\r\n";
            detailLists += "		<th>Ghi chú</th>\r\n";
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            for (int i = 0; i < item.ListQuestion.Count(); i++)
            {
                detailLists += "<tr><td class=\"text-left\">" + item.ListQuestion[i].Name + "</td>";
                detailLists += "<td class=\"text-left\">" + item.ListQuestion[i].Content.Replace("\n", "<br>") + "</td>";
                detailLists += "<td class=\"text-left\">";
                foreach (var answer in item.ListQuestion[i].DetailList)
                {
                    // dữ liệu của khách hàng
                    var data = detail.FirstOrDefault(x => x.QuestionId == item.ListQuestion[i].Id && x.TargetId == answer.Id && x.TargetModule == "Answer");
                    //
                    detailLists += "<input type=\"checkbox\" " + (data == null ? "" : "checked") + "/>" + answer.Content + "\r\n";
                }
                detailLists += "</td></tr>";
            }
            detailLists += "</tbody>\r\n</table>\r\n</p>";
            return detailLists;
        }
        string buildHtml_Partial5(List<CategoryViewModel> Model, List<AdviseCardDetailViewModel> detail)
        {
            string detailLists = "";
            detailLists += "<p><table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "<th></th>\r\n";
            foreach (var ii in Model)
            {
                detailLists += "<th>" + ii.Name + "</th>\r\n";
            }
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
            detailLists += "<tr><td>KẾT LUẬN</td>";
            foreach (var ii in Model)
            {
                foreach (var item in ii.ListQuestion.Where(x => x.Category == ii.Value && x.Name == "KẾT LUẬN"))
                {
                    // dữ liệu của khách hàng
                    var data = detail.FirstOrDefault(x => x.QuestionId == item.Id && x.TargetId == 0 && x.TargetModule == "Answer");
                    //
                    detailLists += "<td class=\"text-left\">" + (data == null ? "" : data.Content) + "</td>";
                }
            }
            detailLists += "</tr>";
            detailLists += "<tr><td>GIẢI PHÁP</td>";
            foreach (var ii in Model)
            {
                foreach (var item in ii.ListQuestion.Where(x => x.Category == ii.Value && x.Name == "GIẢI PHÁP"))
                {
                    // dữ liệu của khách hàng
                    var data = detail.FirstOrDefault(x => x.QuestionId == item.Id && x.TargetId == 0 && x.TargetModule == "Answer");
                    //
                    detailLists += "<td class=\"text-left\">" + (data == null ? "" : data.Content) + "</td>";
                }
            }
            detailLists += "</tr>";
            detailLists += "<tr><td></td>";
            foreach (var ii in Model)
            {
                detailLists += "<td class=\"text-left\">";
                foreach (var item in ii.ListQuestion.Where(x => x.Category == ii.Value && x.Name == "Sản phẩm"))
                {

                    detailLists += "<p>" + item.Name + "</p>\r\n";
                    if (item.Type == "input")
                    {
                        // dữ liệu của khách hàng
                        var data = detail.FirstOrDefault(x => x.QuestionId == item.Id && x.TargetId == 0 && x.TargetModule == "Answer");
                        //
                        detailLists += "<p>" + (data == null ? "" : data.Content) + "</p>\r\n";
                    }
                    if (item.Type == "check")
                    {
                        foreach (var answer in item.DetailList)
                        {
                            // dữ liệu của khách hàng
                            var data = detail.FirstOrDefault(x => x.QuestionId == item.Id && x.TargetId == answer.Id && x.TargetModule == "Answer");
                            //
                            detailLists += "<input type=\"checkbox\" " + (data == null ? "" : "checked") + "/>" + answer.Content + "\r\n";
                        }
                    }
                    if (item.Type == "all")
                    {
                        foreach (var answer in item.DetailList)
                        {
                            // dữ liệu của khách hàng
                            var data = detail.FirstOrDefault(x => x.QuestionId == item.Id && x.TargetId == answer.Id && x.TargetModule == "Answer");
                            //
                            detailLists += "<input type=\"checkbox\" " + (data == null ? "" : "checked") + "/>" + answer.Content + "\r\n";
                            detailLists += "<p>" + (data == null ? "" : data.Content) + "</p>\r\n";
                        }
                    }
                }
                detailLists += "</td>";
            }
            detailLists += "</tr>";
            detailLists += "</tbody>\r\n</table>\r\n</p>";
            return detailLists;
        }

        string buildHtml_Partial6(CategoryViewModel item, List<AdviseCardDetailViewModel> detail)
        {
            var data = Erp.BackOffice.Helpers.SelectListHelper.GetAllProductAndService();
            string detailLists = "";
            detailLists += "<p><table class=\"invoice-detail\">\r\n";
            detailLists += "<thead>\r\n";
            detailLists += "	<tr>\r\n";
            detailLists += "<th></th>\r\n";
            foreach (var ii in item.ListQuestion)
            {
                detailLists += "<th>" + ii.Name + "</th>\r\n";
            }
            detailLists += "	</tr>\r\n";
            detailLists += "</thead>\r\n";
            detailLists += "<tbody>\r\n";
           
            foreach (var ii in data.GroupBy(x=>x.CategoryCode))
            {
                detailLists += "<tr><td>"+ii.Key+"</td>";
                foreach (var question in item.ListQuestion)
                {
                    detailLists += "<td>";
                     if(question.Name=="SẢN PHẨM")
                      {
                          foreach (var q in data.Where(x => x.CategoryCode == ii.Key && x.Type == "product"))
                          {
                              // dữ liệu của khách hàng
                              var _data = detail.FirstOrDefault(x => x.QuestionId == question.Id && x.TargetId == q.Id && x.TargetModule == "Product");
                              //
                              detailLists += "<input type=\"checkbox\" " + (_data == null ? "" : "checked") + "/>" + q.Name + "\r\n";
                          }
                      }
                     if (question.Name == "DỊCH VỤ")
                     {
                         foreach (var q in data.Where(x => x.CategoryCode == ii.Key && x.Type == "service"))
                         {
                             // dữ liệu của khách hàng
                             var _data = detail.FirstOrDefault(x => x.QuestionId == question.Id && x.TargetId == q.Id && x.TargetModule == "Product");
                             //
                             detailLists += "<input type=\"checkbox\" " + (_data == null ? "" : "checked") + "/>" + q.Name + "\r\n";
                         }
                     }
                     detailLists += "</td>";
                }
                  detailLists += "</tr>";
            }
            detailLists += "</tr>";
            detailLists += "</tbody>\r\n</table>\r\n</p>";
            return detailLists;
        }

        #region Detail
        [AllowAnonymous]
        public ActionResult DetailApp(int? Id)
        {
            var AdviseCard = AdviseCardRepository.GetvwAdviseCardById(Id.Value);
            var _category = categoryRepository.GetAllCategories();

            var _list_question = questionRepository.GetAllQuestion().Where(x => x.IsActivated == true)
                              .Select(item1 => new QuestionViewModel
                              {
                                  Id = item1.Id,
                                  Name = item1.Name,
                                  Type = item1.Type,
                                  Category = item1.Category,
                                  OrderNo = item1.OrderNo,
                                  Content = item1.Content
                              }).OrderByDescending(m => m.OrderNo);

            var _answer = answerRepository.GetAllAnswer().Where(x => x.IsActivated == true).Select(item1 => new AnswerViewModel
            {
                Id = item1.Id,
                Content = item1.Content,
                QuestionId = item1.QuestionId,
                OrderNo = item1.OrderNo
            }).OrderByDescending(m => m.OrderNo);

            if (AdviseCard != null && AdviseCard.IsDeleted != true)
            {
                var model = new AdviseCardViewModel();
                AutoMapper.Mapper.Map(AdviseCard, model);
                model.ListAdviseType = new List<CategoryViewModel>();
                model.ListAdviseType = _category.Where(x => x.Code == AdviseCard.Type).Select(x => new CategoryViewModel
                {
                    Code = x.Code,
                    Id = x.Id,
                    Name = x.Name,
                    OrderNo = x.OrderNo,
                    Value = x.Value,
                    ParentId = x.ParentId,
                    Level = 1
                }).OrderBy(x => x.OrderNo).ToList();

                foreach (var item in model.ListAdviseType)
                {
                    item.ListQuestion = _list_question.Where(x => x.Category == item.Value).OrderBy(x => x.OrderNo).ToList();
                    foreach (var item1 in item.ListQuestion)
                    {
                        item1.DetailList = _answer.Where(x => x.QuestionId == item1.Id).OrderBy(x => x.OrderNo).ToList();
                    }

                    var aa = _category.Where(x => x.Code == item.Value).Select(x => new CategoryViewModel
                    {
                        Code = x.Code,
                        Id = x.Id,
                        Name = x.Name,
                        OrderNo = x.OrderNo,
                        Value = x.Value,
                        ParentId = x.ParentId,
                        Level = 2
                    }).OrderBy(x => x.OrderNo).ToList();

                    if (aa.Count() > 0)
                    {
                        foreach (var q in aa)
                        {
                            q.ListQuestion = _list_question.Where(x => x.Category == q.Value).OrderBy(x => x.OrderNo).ToList();
                            foreach (var p in q.ListQuestion)
                            {
                                p.DetailList = _answer.Where(x => x.QuestionId == p.Id).OrderBy(x => x.OrderNo).ToList();
                            }
                        }
                        model.ListAdviseType = model.ListAdviseType.Union(aa).ToList();
                    }
                }
                return View(model);
            }



            if (Request.UrlReferrer != null)
                return Redirect(Request.UrlReferrer.AbsoluteUri);
            return RedirectToAction("Index");
        }
        #endregion

        #region GetInfoAdviseCard
        [AllowAnonymous]
        public JsonResult GetInfoAdviseCard(int id)
        {

            var q = AdviseCardRepository.GetvwAdviseCardById(id);
            var model = new AdviseCardViewModel();
            AutoMapper.Mapper.Map(q, model);
            return Json(model, JsonRequestBehavior.AllowGet);
        }
        #endregion
    }
}
