﻿using Erp.BackOffice.Administration.Models;
using Erp.BackOffice.Filters;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using Erp.Utilities.Helpers;
using Erp.Utilities;
using Erp.BackOffice.Areas.Administration.Models;
using WebMatrix.WebData;


namespace Erp.BackOffice.Administration.Controllers
{
    [Authorize]
    [InitializeSimpleMembership]
    [Erp.BackOffice.Helpers.NoCacheHelper]
    public class SettingController : Controller
    {
        private readonly ISettingRepository _settingRepository;
        public SettingController(ISettingRepository settingRepository)
        {
            _settingRepository = settingRepository;
        }



        public ActionResult Index(string textSearch)
        {
            var q = _settingRepository.GetAlls();

            if (!string.IsNullOrEmpty(textSearch))
            {
                q = q.Where(item => item.Key.Contains(textSearch));
            }

            q = q.ToList();

            List<SettingGroupViewModel> model = new List<SettingGroupViewModel>();
            model = q.GroupBy(item => item.Code)
                .Select(item => new SettingGroupViewModel
                {
                    Name = item.Key,
                    ListSetting = item.Select(x => new SettingViewModel
                    {
                        Id = x.Id,
                        Key = x.Key,
                        Value = x.Value,
                        Note = x.Note
                    }).OrderBy(x => x.Key).ToList()
                }).OrderBy(item => item.Name).ToList();

            ViewBag.AlertMessage = TempData["AlertMessage"];
            return View(model);
        }

        public ActionResult Edit(int Id)
        {
            SettingViewModel settingVM = new SettingViewModel();
            AutoMapper.Mapper.Map(_settingRepository.GetById(Id), settingVM);
            ViewBag.CurrentUser = WebSecurity.CurrentUserName;
            return View(settingVM);
        }

        [HttpPost]
        public ActionResult Edit(SettingViewModel model)
        {
            if (ModelState.IsValid)
            {
                Setting setting = _settingRepository.GetById(model.Id);
                AutoMapper.Mapper.Map(model, setting);
                _settingRepository.Update(setting);

                TempData["AlertMessage"] = App_GlobalResources.Wording.UpdateSuccess;
                return RedirectToAction("Index");
            }

            return RedirectToAction("Edit", new { @Id = model.Id});
        }

        [HttpPost]
        public ActionResult Update(int Id, string Value)
        {
            Setting setting = _settingRepository.GetById(Id);
            setting.Value = Value;
            _settingRepository.Update(setting);
            return Content("success");
        }
        
        public ActionResult Create()
        {
            ViewBag.CurrentUser = WebSecurity.CurrentUserName;
            return View(new SettingViewModel());
        }

        [HttpPost]
        public ActionResult Create(SettingViewModel model, bool? IsPopup)
        {
            if (ModelState.IsValid)
            {
                Setting setting = new Setting();
                AutoMapper.Mapper.Map(model, setting);
                _settingRepository.Insert(setting);

                if (IsPopup==true)
                {
                    return RedirectToAction("_ClosePopup", "Home", new { area = "", FunctionCallback = "ClosePopupAndReloadPage" });
                }
                else
                {
                    TempData[Globals.SuccessMessageKey] = App_GlobalResources.Wording.InsertSuccess;
                    return RedirectToAction("Index");
                }
            }

            return RedirectToAction("Create");
        }


        [HttpPost]
        public ActionResult Delete(int Id)
        {

            if (_settingRepository.GetById(Id).IsLocked == true)
            {
                TempData["AlertMessage"] = App_GlobalResources.Wording.SettingDeleteError;
                return RedirectToAction("Index");
            }

            _settingRepository.Delete(Id);
            TempData["AlertMessage"] = App_GlobalResources.Wording.DeleteSuccess;
            return RedirectToAction("Index");
        }
    }
}
