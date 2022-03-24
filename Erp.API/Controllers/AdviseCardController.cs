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
//using System.Web.Mvc;
using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Repositories;

namespace Erp.API.Controllers
{

    public class AdviseCardController : ApiController
    {

        #region GetListAdviseCard
        [HttpGet]
        public HttpResponseMessage GetListAdviseCard(string startDate, string endDate, string txtCode, int? BranchId, int page = 1, int numberPerPage = 10)
        {
            AdviseCardRepository adviseCardRepository = new Domain.Sale.Repositories.AdviseCardRepository(new Domain.Sale.ErpSaleDbContext());

            var q = adviseCardRepository.GetvwAllAdviseCard().Where(x => x.BranchId == BranchId).ToList();
            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = Helpers.Common.ChuyenThanhKhongDau(txtCode);
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Code).Contains(txtCode)).ToList();
            }
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                }
            }
            var model = q.Select(item => new AdviseCardViewModel
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
                   CreatedUserCode = item.CreatedUserCode,
                   CounselorId = item.CounselorId,
                   CreatedUserName = item.CreatedUserName,
                   CounselorName = item.CounselorName,
                   IsActived = item.IsActived,
                   CustomerCode = item.CustomerCode,
                   CustomerName = item.CustomerName,
                   BranchId = item.BranchId,
                   Type = item.Type
               }).OrderByDescending(m => m.CreatedDate)
            .Skip((page - 1) * numberPerPage)
               .Take(numberPerPage);
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(q));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region Create
        [HttpPost]
        public string Create([FromBody] AdviseCardViewModel model)
        {
            try
            {
                var time_now = DateTime.Now.ToShortDateString();
                AdviseCardRepository adviseCardRepository = new Domain.Sale.Repositories.AdviseCardRepository(new Domain.Sale.ErpSaleDbContext());
                UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                var AdviseCard = new AdviseCard();
                AutoMapper.Mapper.CreateMap<AdviseCardViewModel, AdviseCard>();
                AutoMapper.Mapper.Map(model, AdviseCard);
                AdviseCard.IsDeleted = false;
                AdviseCard.CreatedUserId = model.CreatedUserId.Value;
                AdviseCard.ModifiedUserId = model.CreatedUserId.Value;
                AdviseCard.AssignedUserId = model.CreatedUserId.Value;
                AdviseCard.CreatedDate = DateTime.Now;
                AdviseCard.ModifiedDate = DateTime.Now;
                AdviseCard.IsActived = false;
                // lấy BranchId
                var User = userRepository.GetUserById(model.CreatedUserId.Value);
                model.BranchId = User.BranchId;

                adviseCardRepository.InsertAdviseCard(AdviseCard);
                //cập nhật lại mã phiếu yêu cầu
                AdviseCard.Code = Erp.API.Helpers.Common.GetOrderNo("AdviseCard", model.Code);
                adviseCardRepository.UpdateAdviseCard(AdviseCard);
                Erp.API.Helpers.Common.SetOrderNo("AdviseCard");
                ///
                return AdviseCard.Id.ToString();
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Detail
           [HttpGet]
        public HttpResponseMessage Detail(int Id)
        {
            AdviseCardRepository adviseCardRepository = new Domain.Sale.Repositories.AdviseCardRepository(new Domain.Sale.ErpSaleDbContext());
            CategoryRepository categoryRepository = new Domain.Repositories.CategoryRepository(new Domain.ErpDbContext());
            QuestionRepository questionRepository = new Domain.Crm.Repositories.QuestionRepository(new Domain.Crm.ErpCrmDbContext());
            AnswerRepository answerRepository = new Domain.Crm.Repositories.AnswerRepository(new Domain.Crm.ErpCrmDbContext());
            var AdviseCard = adviseCardRepository.GetvwAdviseCardById(Id);
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
            var model = new AdviseCardViewModel();
            if (AdviseCard != null && AdviseCard.IsDeleted != true)
            {
                AutoMapper.Mapper.CreateMap<vwAdviseCard, AdviseCardViewModel>();
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
            }

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        [HttpPost]
        public string SaveItem(int CreatedUserId,int AdviseCardId, int QuestionId, int TargetId, string TargetModule, string content, string Note, bool IsChecked)
        {
            AdviseCardDetailRepository adviseCardDetailRepository = new Domain.Sale.Repositories.AdviseCardDetailRepository(new Domain.Sale.ErpSaleDbContext());
            UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                try
                {
                    var update = adviseCardDetailRepository.GetAdviseCardDetailById(AdviseCardId, QuestionId, TargetId);
                    if (update == null)
                    {
                        var add = new AdviseCardDetail();
                        add.IsDeleted = false;
                        add.CreatedUserId = CreatedUserId;
                        add.ModifiedUserId = CreatedUserId;
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
                            update.ModifiedUserId = CreatedUserId;
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
                         return "success";
             
                }
                catch (Exception ex)
                {
                    return ex.Message;
                }
        }

        #region  - LoadAdviseCardDetailById -
        public HttpResponseMessage LoadAdviseCardDetailById(int AdviseCardId)
        {
            AdviseCardDetailRepository adviseCardDetailRepository = new Domain.Sale.Repositories.AdviseCardDetailRepository(new Domain.Sale.ErpSaleDbContext());
            var list = adviseCardDetailRepository.GetAllAdviseCardDetailById(AdviseCardId).Select(x => new
            {
                Id = x.Id,
                TargetId = x.TargetId,
                Content = x.Content,
                Note = x.Note,
                QuestionId = x.QuestionId,
                TargetModule = x.TargetModule
            }).ToList();
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(list));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion
    }
}
