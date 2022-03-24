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
using Erp.Domain.Staff.Entities;
using Erp.Domain.Staff.Repositories;
using OneSignal.CSharp.SDK;
using Push_Notification_OneSignal;
using OneSignal.CSharp.SDK.Resources;


namespace Erp.API.Controllers
{
    public class SchedulingHistoryController : ApiController
    {
        #region Detail
        [HttpGet]
        public HttpResponseMessage Detail(int Id, int UserId)
        {
            try
            {
                LogEquipmentRepository logEquipmentRepository = new Domain.Sale.Repositories.LogEquipmentRepository(new Domain.Sale.ErpSaleDbContext());
                SchedulingHistoryRepository schedulingHistoryRepository = new Domain.Sale.Repositories.SchedulingHistoryRepository(new Domain.Sale.ErpSaleDbContext());
                StaffMadeRepository staffMadeRepository = new Domain.Sale.Repositories.StaffMadeRepository(new Domain.Sale.ErpSaleDbContext());

                UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                var _SchedulingHistory = schedulingHistoryRepository.GetvwSchedulingHistoryById(Id);
                var model = new SchedulingHistoryViewModel();
                model.EquimentGroupList = new List<System.Web.Mvc.SelectListItem>();

                var user = userRepository.GetUserById(UserId);
                if (_SchedulingHistory != null && _SchedulingHistory.IsDeleted != true)
                {
                    AutoMapper.Mapper.CreateMap<vwSchedulingHistory, SchedulingHistoryViewModel>();
                    AutoMapper.Mapper.Map(_SchedulingHistory, model);
                    model.StaffMadeList = new List<StaffMadeViewModel>();
                    model.StaffMadeList = staffMadeRepository.GetvwAllStaffMade().Where(x => x.SchedulingHistoryId == Id).Select(x => new StaffMadeViewModel
                    {
                        Id = x.Id,
                        Note = x.Note,
                        UserId = x.UserId,
                        Status = x.Status,
                        SchedulingHistoryId = x.SchedulingHistoryId,
                        ProfileImage = x.ProfileImage,
                        UserCode = x.UserCode,
                        FullName = x.FullName,
                        ModifiedDate = x.ModifiedDate
                    }).ToList();
                    model.EquipmentList = new List<LogEquipmentViewModel>();
                    model.EquipmentList = logEquipmentRepository.GetvwAllLogEquipment().Where(x => x.SchedulingHistoryId == Id).Select(x => new LogEquipmentViewModel
                    {
                        Id = x.Id,
                        EquipmentId = x.EquipmentId,
                        InspectionDate = x.InspectionDate,
                        Note = x.Note,
                        StaffEquipmentCode = x.StaffEquipmentCode,
                        SchedulingHistoryId = x.SchedulingHistoryId,
                        StaffEquipmentName = x.StaffEquipmentName,
                        Status = x.Status,
                        ModifiedDate = x.ModifiedDate,
                        ModifiedUserId = x.ModifiedUserId,
                        ModifiedUserName = x.ModifiedUserId.Value.ToString(),
                        EquimentGroup = x.EquimentGroup
                    }).ToList();

                    //begin hoapd bo sung tham so lay thoi gian bao truoc

                    model.THOIGIANBAOTRUOC_GIAY = int.Parse(Erp.API.Helpers.Common.GetSetting("THOIGIANBAOTRUOC_GIAY"));
                    model.THOIGIANBAOTSAU_GIAY = int.Parse(Erp.API.Helpers.Common.GetSetting("THOIGIANBAOTSAU_GIAY"));

                    //end hoapd bo sung tham so lay thoi gian bao truoc

                    model.EquimentGroupList = Erp.API.Helpers.Common.GetSelectList_Category("EquimentGroup", null, null).Where(x => ("," + model.EquimentGroup + ",").Contains("," + x.Value + ",") == true).ToList();

                    if (model.StaffMadeList.Any(x => x.UserId == UserId) == false)
                    {
                        var _resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                        var data = new SchedulingHistoryViewModel();
                        _resp.Content = new StringContent(JsonConvert.SerializeObject(data));
                        _resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return _resp;
                    }
                    var resp1 = new HttpResponseMessage(HttpStatusCode.OK);
                    resp1.Content = new StringContent(JsonConvert.SerializeObject(model));
                    resp1.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp1;
                }
                var resp = new HttpResponseMessage(HttpStatusCode.NotFound);
                resp.Content = new StringContent(JsonConvert.SerializeObject(model));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }
            catch (Exception ex)
            {

                throw;
            }

        }
        #endregion

        #region Từ chối  duyệt nhân viên không tham gia,
        [HttpPost]
        public string RefureUser([FromBody] StaffMadeViewModel model)
        {
            try
            {
                StaffMadeRepository staffMadeRepository = new StaffMadeRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());

                var data = staffMadeRepository.GetStaffMadeById(model.Id);
                if (data != null)
                {
                    data.Note = model.Note;
                    data.ModifiedUserId = model.CreatedUserId;
                    data.ModifiedDate = DateTime.Now;
                    data.Status = "refure";
                    staffMadeRepository.UpdateStaffMade(data);
                    var user = userRepository.GetvwUserById(model.CreatedUserId.Value);
                    #region goi API


                    using (var client = new HttpClient())
                    {
                        // Initialize HTTP client
                        client.BaseAddress = new Uri("http://171.244.185.172:788/", UriKind.Absolute);
                        client.Timeout = TimeSpan.FromSeconds(50);
                        client.DefaultRequestHeaders.Accept.Clear();
                        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Integration", "bmcEpOm0=");
                        client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                        using (var response = client.GetAsync(string.Format("SchedulingHistory/ChangeStatusStaffMade?BranchId={0}&Type={1}&Note={2}&Status={3}&Id={4}", user.BranchId.Value, "LT", data.Note, data.Status, data.Id)).Result)
                        {
                            //string responseString = response.Content.ReadAsStringAsync().Result;
                            //Newtonsoft.Json.Linq.JObject json = Newtonsoft.Json.Linq.JObject.Parse(responseString);
                            //Console.WriteLine(response.StatusCode);
                        }

                    }


                    #endregion
                    //hiển thị dữ liệu tay đổi lên web cho lễ tân xem
                    //     Erp.BackOffice.Sale.Controllers.SchedulingHistoryController.ChangeStatusStaffMade(user.BranchId.Value,"LT",data.Note,data.Status,data.Id);
                    //
                    return "success";
                }
                return "error";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Approval nhân viên tham gia thực hiện
        [HttpPost]
        public string ApprovalUser([FromBody] StaffMadeViewModel model)
        {
            try
            {
                StaffMadeRepository staffMadeRepository = new StaffMadeRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                var data = staffMadeRepository.GetStaffMadeById(model.Id);
                if (data != null)
                {
                    data.Status = "accept";
                    data.ModifiedUserId = model.CreatedUserId;
                    data.ModifiedDate = DateTime.Now;
                    staffMadeRepository.UpdateStaffMade(data);
                    return "success";
                }
                return "error";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
        }
        #endregion

        #region Trang thiết bị  tham gia thực hiện
        [HttpPost]
        public HttpResponseMessage ApprovalEquipment([FromBody] LogEquipmentViewModel model)
        {
            try
            {
                //CreatedUserId,EquimentGroup,StaffEquipmentCode,SchedulingHistoryId,Id(nếu có)
                UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                LogEquipmentRepository logEquipmentRepository = new LogEquipmentRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                StaffEquipmentRepository staffEquimentRepository = new StaffEquipmentRepository(new Domain.Staff.ErpStaffDbContext());
                var DataEquipment = new List<StaffEquipmentViewModel>();
                var user = userRepository.GetUserById(model.CreatedUserId.Value);
                //lấy danh sách thiết bị thuộc nhóm
                DataEquipment = staffEquimentRepository.GetAllStaffEquipment().AsEnumerable().Where(x => x.BranchId == user.BranchId)
                      .Select(x => new StaffEquipmentViewModel
                      {
                          Id = x.Id,
                          Code = x.Code,
                          Name = x.Name,
                          Group = x.Group,
                          InspectionDate = x.InspectionDate
                      }).ToList();
                //kiểm tra xem mã thiết bị có trong danh sách hay ko
                var check_error = DataEquipment.FirstOrDefault(x => x.Code.ToUpper() == model.StaffEquipmentCode.ToUpper());
                if (check_error == null)
                {
                    //ko có trả về...
                    var resp1 = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                    resp1.Content = new StringContent(JsonConvert.SerializeObject(null));
                    resp1.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp1;
                }
                else
                {
                    //nếu đã có dữ liệu trong bảng log equipment thì chỉ update lại trạng thái là accept
                    if (model.Id > 0)
                    {
                        var data = logEquipmentRepository.GetLogEquipmentById(model.Id);
                        if (data != null)
                        {
                            data.Status = "accept";
                            data.ModifiedUserId = model.CreatedUserId;
                            data.ModifiedDate = DateTime.Now;
                            logEquipmentRepository.UpdateLogEquipment(data);

                            List<LogEquipmentViewModel> EquipmentList = new List<LogEquipmentViewModel>();
                            EquipmentList = logEquipmentRepository.GetvwAllLogEquipment().Where(x => x.SchedulingHistoryId == model.SchedulingHistoryId).Select(x => new LogEquipmentViewModel
                            {
                                Id = x.Id,
                                EquipmentId = x.EquipmentId,
                                InspectionDate = x.InspectionDate,
                                Note = x.Note,
                                StaffEquipmentCode = x.StaffEquipmentCode,
                                SchedulingHistoryId = x.SchedulingHistoryId,
                                StaffEquipmentName = x.StaffEquipmentName,
                                Status = x.Status,
                                ModifiedDate = x.ModifiedDate,
                                ModifiedUserId = x.ModifiedUserId,
                                ModifiedUserName = x.ModifiedUserId.Value.ToString(),
                                EquimentGroup = x.EquimentGroup
                            }).ToList();

                            var resp = new HttpResponseMessage(HttpStatusCode.OK);
                            resp.Content = new StringContent(JsonConvert.SerializeObject(EquipmentList));
                            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                            return resp;
                        }
                    }
                    else
                    {
                        //begin kiểm tra xem thiết bị chưa có mới cho insert
                        var EquipmentList1 = logEquipmentRepository.GetvwAllLogEquipment().Where(x => x.SchedulingHistoryId == model.SchedulingHistoryId && x.EquipmentId == check_error.Id);
                        if ((EquipmentList1 == null) || (EquipmentList1.Count() == 0))
                        {
                            //end kiểm tra xem thiết bị chưa có mới cho insert

                            //nếu chưa có dữ liệu thì thêm mới dữ liệu và mặc định là accept luôn
                            var ins = new LogEquipment();
                            ins.ModifiedUserId = model.CreatedUserId;
                            ins.ModifiedDate = DateTime.Now;
                            ins.CreatedUserId = model.CreatedUserId;
                            ins.CreatedDate = DateTime.Now;
                            ins.IsDeleted = false;
                            ins.EquipmentId = check_error.Id;
                            ins.SchedulingHistoryId = model.SchedulingHistoryId;
                            ins.Status = "accept";
                            logEquipmentRepository.InsertLogEquipment(ins);
                        }


                        List<LogEquipmentViewModel> EquipmentList = new List<LogEquipmentViewModel>();
                        EquipmentList = logEquipmentRepository.GetvwAllLogEquipment().Where(x => x.SchedulingHistoryId == model.SchedulingHistoryId).Select(x => new LogEquipmentViewModel
                        {
                            Id = x.Id,
                            EquipmentId = x.EquipmentId,
                            InspectionDate = x.InspectionDate,
                            Note = x.Note,
                            StaffEquipmentCode = x.StaffEquipmentCode,
                            SchedulingHistoryId = x.SchedulingHistoryId,
                            StaffEquipmentName = x.StaffEquipmentName,
                            Status = x.Status,
                            ModifiedDate = x.ModifiedDate,
                            ModifiedUserId = x.ModifiedUserId,
                            ModifiedUserName = x.ModifiedUserId.Value.ToString(),
                            EquimentGroup = x.EquimentGroup
                        }).ToList();



                        var resp1 = new HttpResponseMessage(HttpStatusCode.OK);
                        resp1.Content = new StringContent(JsonConvert.SerializeObject(EquipmentList));
                        resp1.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                        return resp1;
                    }
                }
                var resp2 = new HttpResponseMessage(HttpStatusCode.OK);
                resp2.Content = new StringContent(JsonConvert.SerializeObject(null));
                resp2.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp2;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region Trang thiết bị ko tham gia,
        [HttpPost]
        public HttpResponseMessage RefureEquipment([FromBody] LogEquipmentViewModel model)
        {
            try
            {
                LogEquipmentRepository logEquipmentRepository = new LogEquipmentRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                var data = logEquipmentRepository.GetLogEquipmentById(model.Id);
                if (data != null)
                {
                    data.Note = model.Note;
                    data.ModifiedUserId = model.CreatedUserId;
                    data.ModifiedDate = DateTime.Now;
                    data.Status = "refure";
                    logEquipmentRepository.UpdateLogEquipment(data);
                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp;
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(data));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }
        #endregion

        #region dùng để bắt đầu tính giờ và kết thúc công việc, nêu lý do quá thời gian thực hiện



        [HttpPost]
        public HttpResponseMessage ChangeStatus([FromBody] SchedulingHistoryViewModel model)
        {
            try
            {
                SchedulingHistoryRepository schedulingHistoryRepository = new SchedulingHistoryRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                LogEquipmentRepository logEquipmentRepository = new LogEquipmentRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                StaffEquipmentRepository staffEquipmentRepository = new StaffEquipmentRepository(new Erp.Domain.Staff.ErpStaffDbContext());
                BedRepository bedRepository = new BedRepository(new Erp.Domain.Staff.ErpStaffDbContext());
                MembershipRepository MBS = new MembershipRepository(new Erp.Domain.Sale.ErpSaleDbContext());



                var SchedulingHistory = schedulingHistoryRepository.GetSchedulingHistoryById(model.Id);
                //  var model = new SchedulingHistoryViewModel();
                if (SchedulingHistory != null)
                {
                    SchedulingHistory.Status = model.Status;
                    if (model.Status == "inprogress")
                    {
                        SchedulingHistory.WorkDay = DateTime.Now;

                        //begin cuong
                        //cap nhat lai trang thai thiet bi
                        var listEquipment = logEquipmentRepository.GetListAllLogEquipment().Where(x => x.SchedulingHistoryId == model.Id);
                        foreach (var item in listEquipment)
                        {
                            var equipment = staffEquipmentRepository.GetStaffEquipmentById(item.EquipmentId.Value);
                            equipment.StatusStaffMade = true;
                            staffEquipmentRepository.UpdateEquipment(equipment);
                        }
                        //cap nhat lai trang thai giuong
                        Bed thisbed = bedRepository.GetBedById(SchedulingHistory.BedId.Value);
                        thisbed.Trang_Thai = true;
                        bedRepository.UpdateBed(thisbed);
                        SchedulingHistory.EndDate = DateTime.Now;
                        string endtime = SchedulingHistory.EndDate.ToString();
                        string[] timeends = endtime.Split(' ');
                        SchedulingHistory.startTime = timeends[1];
                        //mcuong.fit----

                        //end cuong

                        #region Gui thong bao 1



                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        var IncludePlayerIds = new List<string>();
                        string pPlayerIdLetan = "";// Helpers.Common.GetSetting("PLAYID_LETAN");
                        //begin hoapd sua lai de co the gui dc cho nhieu le tan
                        UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                        var user = userRepository.GetUserById(SchedulingHistory.CreatedUserId.Value);
                        if (user!= null && user.IsLetan==true && user.PlayerId_web != "")
                        {
                            pPlayerIdLetan = user.PlayerId_web;
                        }


                        //end hoapd sua lai de co the gui dc cho nhieu le tan


                        if ((pPlayerIdLetan != null) && (pPlayerIdLetan != ""))
                        {
                            IncludePlayerIds.Add(pPlayerIdLetan);
                        }


                        if (IncludePlayerIds.Count > 0)
                        {


                            var client = new OneSignalClient("ZTA4ZGMyOTctZjU0ZS00Y2ExLWI4MTEtYWY5YzNiYjg0OWU0"); // Nhập API Serect Key

                            var options = new NotificationCreateOptions_new();


                            options.AppId = new Guid("872eae43-21c1-44f7-80c9-ab594b66bbb6"); // Nhập My AppID của bạn
                                                                                              //options.IncludedSegments = new List<string> { "All" };
                            options.DeliverToChromeWeb = true;
                            options.DeliverToAndroid = true;
                            options.DeliverToIos = true;
                            options.DeliverToFirefox = true;

                            options.Headings.Add(LanguageCodes.English, "Thông báo bắt đầu liệu trình");
                            options.Contents.Add(LanguageCodes.English, "KTV:" + SchedulingHistory.NameNV + "-" + SchedulingHistory.Name_Room + "-" + SchedulingHistory.Name_Bed + " bắt đầu thực hiện liệu trình CSD!");


                            //options.Url = txtLaunchURL.Text;
                            //options.ChromeWebImage = txtImageChrome.Text;
                            //dictionary.Add("IdXeplich", Id.ToString());
                            options.Data = dictionary;
                            options.IncludePlayerIds = IncludePlayerIds;
                            var result = client.Notifications.Create(options);
                        }
                        #endregion


                    }
                    if (model.Status == "complete")
                    {
                        var data = schedulingHistoryRepository.GetvwSchedulingHistoryById(model.Id);
                        SchedulingHistory.EndDate = DateTime.Now;
                        if (data.TargetModule == "Membership")
                        {
                            var info_old = MBS.GetMembershipById(data.TargetId.Value);
                            if (info_old != null)
                            {
                                info_old.Status = "complete";
                                info_old.ModifiedUserId = model.AssignedUserId;
                                info_old.ModifiedDate = DateTime.Now;
                                MBS.UpdateMembership(info_old);
                            }
                        }




                        //begin cuong
                        //cap nhat lai trang thai thiet bi
                        var listEquipment = logEquipmentRepository.GetListAllLogEquipment().Where(x => x.SchedulingHistoryId == model.Id);
                        foreach (var item in listEquipment)
                        {
                            var equipment = staffEquipmentRepository.GetStaffEquipmentById(item.EquipmentId.Value);
                            equipment.StatusStaffMade = false;
                            staffEquipmentRepository.UpdateEquipment(equipment);
                        }
                        //cap nhat lai trang thai giuong
                        Bed thisbed = bedRepository.GetBedById(SchedulingHistory.BedId.Value);
                        thisbed.Trang_Thai = false;
                        bedRepository.UpdateBed(thisbed);
                        SchedulingHistory.EndDate = DateTime.Now;
                        string endtime = SchedulingHistory.EndDate.ToString();
                        string[] timeends = endtime.Split(' ');
                        SchedulingHistory.endTime = timeends[1];
                        //mcuong.fit----

                        //end cuong
                        //tính tg nhân viên thực hiện---- cong
                        //lấy h,m,s lúc start và end
                        string[] hmsStarts = SchedulingHistory.startTime.Split(':');
                        string[] hmsEnds = SchedulingHistory.endTime.Split(':');
                        //ep kiểu
                        int hstart = int.Parse(hmsStarts[0]);
                        int mstart = int.Parse(hmsStarts[1]);
                        int sstart = int.Parse(hmsStarts[2]);
                        int hend = int.Parse(hmsEnds[0]);
                        int mend = int.Parse(hmsEnds[1]);
                        int send = int.Parse(hmsEnds[2]);
                        //tính toán tg thực hiện
                        int mExecution = 0;
                        if (hend >= hstart)
                        {
                            mExecution = (hend - hstart) * 60 + (mend - mstart);
                        }
                        else
                        {
                            hend = hend + 12;
                            mExecution = (hend - hstart) * 60 + (mend - mstart);
                        }
                        SchedulingHistory.TimeExecution = mExecution.ToString();
                        //end--- cong


                        #region Gui thong bao 1



                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        var IncludePlayerIds = new List<string>();
                        string pPlayerIdLetan = "";// Helpers.Common.GetSetting("PLAYID_LETAN");
                        //begin hoapd sua lai de co the gui dc cho nhieu le tan
                        UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                        var user = userRepository.GetUserById(SchedulingHistory.CreatedUserId.Value);
                        if (user != null && user.IsLetan == true && user.PlayerId_web != "")
                        {
                            pPlayerIdLetan = user.PlayerId_web;
                        }

                        if ((pPlayerIdLetan != null) && (pPlayerIdLetan != ""))
                        {
                            IncludePlayerIds.Add(pPlayerIdLetan);
                        }


                        if (IncludePlayerIds.Count > 0)
                        {


                            var client = new OneSignalClient("ZTA4ZGMyOTctZjU0ZS00Y2ExLWI4MTEtYWY5YzNiYjg0OWU0"); // Nhập API Serect Key

                            var options = new NotificationCreateOptions_new();


                            options.AppId = new Guid("872eae43-21c1-44f7-80c9-ab594b66bbb6"); // Nhập My AppID của bạn
                                                                                              //options.IncludedSegments = new List<string> { "All" };
                            options.DeliverToChromeWeb = true;
                            options.DeliverToAndroid = true;
                            options.DeliverToIos = true;
                            options.DeliverToFirefox = true;

                            options.Headings.Add(LanguageCodes.English, "Thông báo kết thúc liệu trình");
                            options.Contents.Add(LanguageCodes.English, "KTV:" + SchedulingHistory.NameNV + "-" + SchedulingHistory.Name_Room + "-" + SchedulingHistory.Name_Bed + " vừa mới kết thúc liệu trình CSD!");


                            //options.Url = txtLaunchURL.Text;
                            //options.ChromeWebImage = txtImageChrome.Text;
                            //dictionary.Add("IdXeplich", Id.ToString());
                            options.Data = dictionary;
                            options.IncludePlayerIds = IncludePlayerIds;
                            var result = client.Notifications.Create(options);
                        }
                        #endregion


                    }
                    SchedulingHistory.Note = SchedulingHistory.Note + model.Note;
                    schedulingHistoryRepository.UpdateSchedulingHistory(SchedulingHistory);





                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(SchedulingHistory));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp;
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(SchedulingHistory));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }



        [HttpPost]
        public HttpResponseMessage ChangeStatus_xinthem([FromBody] SchedulingHistoryViewModel model)
        {
            try
            {
                SchedulingHistoryRepository schedulingHistoryRepository = new SchedulingHistoryRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                LogEquipmentRepository logEquipmentRepository = new LogEquipmentRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                StaffEquipmentRepository staffEquipmentRepository = new StaffEquipmentRepository(new Erp.Domain.Staff.ErpStaffDbContext());
                BedRepository bedRepository = new BedRepository(new Erp.Domain.Staff.ErpStaffDbContext());
                MembershipRepository MBS = new MembershipRepository(new Erp.Domain.Sale.ErpSaleDbContext());



                var SchedulingHistory = schedulingHistoryRepository.GetSchedulingHistoryById(model.Id);
                //  var model = new SchedulingHistoryViewModel();
                if (SchedulingHistory != null)
                {
                    if (model.Status != "")
                    {
                        #region Gui thong bao 1
                        Dictionary<string, string> dictionary = new Dictionary<string, string>();
                        var IncludePlayerIds = new List<string>();
                        string pPlayerIdLetan = "";// Helpers.Common.GetSetting("PLAYID_LETAN");
                        //begin hoapd sua lai de co the gui dc cho nhieu le tan
                        UserRepository userRepository = new UserRepository(new Domain.ErpDbContext());
                        var user = userRepository.GetUserById(SchedulingHistory.CreatedUserId.Value);
                        if (user != null && user.IsLetan == true && user.PlayerId_web != "")
                        {
                            pPlayerIdLetan = user.PlayerId_web;
                        }

                        if ((pPlayerIdLetan != null) && (pPlayerIdLetan != ""))
                        {
                            IncludePlayerIds.Add(pPlayerIdLetan);
                        }


                        if (IncludePlayerIds.Count > 0)
                        {
                            var client = new OneSignalClient("ZTA4ZGMyOTctZjU0ZS00Y2ExLWI4MTEtYWY5YzNiYjg0OWU0"); // Nhập API Serect Key

                            var options = new NotificationCreateOptions_new();


                            options.AppId = new Guid("872eae43-21c1-44f7-80c9-ab594b66bbb6"); // Nhập My AppID của bạn
                                                                                              //options.IncludedSegments = new List<string> { "All" };
                            options.DeliverToChromeWeb = true;
                            options.DeliverToAndroid = true;
                            options.DeliverToIos = true;
                            options.DeliverToFirefox = true;

                            options.Headings.Add(LanguageCodes.English, "Thông báo xin thêm thời gian");
                            options.Contents.Add(LanguageCodes.English, "KTV:" + SchedulingHistory.NameNV + "-" + SchedulingHistory.Name_Room + "-" + SchedulingHistory.Name_Bed + " xin thêm thời gian: " + model.Status + " phút");


                            //options.Url = txtLaunchURL.Text;
                            //options.ChromeWebImage = txtImageChrome.Text;
                            //dictionary.Add("IdXeplich", Id.ToString());
                            options.Data = dictionary;
                            options.IncludePlayerIds = IncludePlayerIds;
                            var result = client.Notifications.Create(options);
                        }
                        #endregion


                    }


                    var resp = new HttpResponseMessage(HttpStatusCode.OK);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(SchedulingHistory));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp;
                }
                else
                {
                    var resp = new HttpResponseMessage(HttpStatusCode.NotAcceptable);
                    resp.Content = new StringContent(JsonConvert.SerializeObject(SchedulingHistory));
                    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                    return resp;
                }
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        #endregion


        #region GetListAdviseCard
        [HttpGet]
        public HttpResponseMessage GetListSchedulingbyUser(string txtCode, int UserId, int page = 1, int numberPerPage = 10)
        {
            try
            {
                StaffMadeRepository staffMadeRepository = new StaffMadeRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                var q = staffMadeRepository.GetvwAllStaffMade().Where(x => x.UserId == UserId).ToList();
                var time_now = DateTime.Now.ToShortDateString();
                if (!string.IsNullOrEmpty(txtCode))
                {
                    txtCode = Helpers.Common.ChuyenThanhKhongDau(txtCode);
                    q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.SchedulingCode).Contains(txtCode)).ToList();
                }
                q = q.Where(x => x.ExpectedWorkDay.Value.ToShortDateString() == time_now).ToList();
                var model = q.Select(item => new StaffMadeViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    CustomerCode = item.CustomerCode,
                    CustomerName = item.CustomerName,
                    BranchId = item.BranchId,
                    Type = item.Type,
                    CustomerImage = item.CustomerImage,
                    ExpectedEndDate = item.ExpectedEndDate,
                    ExpectedWorkDay = item.ExpectedWorkDay,
                    FloorName = item.FloorName,
                    FullName = item.FullName + "(" + item.FloorName + "-" + item.RoomName + "-" + item.Name_Bed + ")",
                    Note = item.Note,
                    ProductCode = item.ProductCode,
                    ProductName = item.ProductName,
                    ProfileImage = item.ProfileImage,
                    RoomName = item.RoomName,
                    SchedulingCode = item.SchedulingCode,
                    SchedulingHistoryId = item.SchedulingHistoryId,
                    SchedulingStatus = item.SchedulingStatus,
                    Status = item.Status,
                    TotalMinute = item.TotalMinute,
                    UserCode = item.UserCode ,
                }).OrderByDescending(m => m.CreatedDate)
                .Skip((page - 1) * numberPerPage)
                   .Take(numberPerPage);

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(JsonConvert.SerializeObject(model));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion
    }
}
