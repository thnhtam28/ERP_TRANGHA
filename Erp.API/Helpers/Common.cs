namespace Erp.API.Helpers
{
    using Erp.Domain;
    using Erp.Domain.Repositories;
    using Erp.Domain.Sale;
    using Erp.Domain.Sale.Repositories;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;
    using System.Text.RegularExpressions;
    using System.Web.Mvc;
    using Erp.Domain.Entities;
    using System.Linq;
    using WebMatrix.WebData;
    using System.Web;
    using System.Configuration;
    using System.Net.Mail;
    using System.Net.Mime;
    using System.Net;
    using System.Runtime.Serialization.Formatters.Binary;
    using System.IO;
    using System.Data;
    using System.ComponentModel;
    //using StackExchange.Redis;

    public class Common
    {
        //public static ConnectionMultiplexer ConnectionMultiplexer = null;

        

     
        public static void WriteEventLog(String logData)
        {
            if (!EventLog.SourceExists("ErpPlus"))
            {
                //An event log source should not be created and immediately used.
                //There is a latency time to enable the source, it should be created
                //prior to executing the application that uses the source.
                //Execute this sample a second time to use the new source.
                EventLog.CreateEventSource("ErpPlus", "ErpPlusLog");
                Console.WriteLine("CreatedEventSource");
                Console.WriteLine("Exiting, execute the application a second time to use the source.");
                // The source is created.  Exit the application to allow it to be registered.
                return;
            }

            // Create an EventLog instance and assign its source.
            EventLog myLog = new EventLog();
            myLog.Source = "ErpPlus";

            // Write an informational entry to the event log.    
            myLog.WriteEntry(logData);
        }

        public static string StripHTML(string input)
        {
            if (string.IsNullOrEmpty(input))
                return "";

            return Regex.Replace(input, "<.*?>", String.Empty);
        }

        public static string PhanCachHangNgan(object str)
        {
            try
            {
                int aa = 0;
                if (Convert.ToInt32(str) < 0)
                {
                    aa = Convert.ToInt32(str) * (-1);
                }
                else
                {
                    aa = Convert.ToInt32(str);
                }
                if (aa >= 1000)
                    return Convert.ToInt32(str).ToString("0,000").Replace(",", ".");
                else
                    return str.ToString();
            }
            catch { }
            return "";
        }
        public static string PhanCachHangNgan2(object str)
        {
            try
            {
                if (Convert.ToDecimal(str) >= 1000)
                    return Convert.ToDecimal(str).ToString("0,000").Replace(",", ".");
                else
                    return str.ToString();
            }
            catch { }
            return "";
        }


        public static string Capitalize(string value)
        {
            if (string.IsNullOrEmpty(value))
                value = "";
            return System.Globalization.CultureInfo.CurrentCulture.TextInfo.ToTitleCase(value.Trim().ToLower());
        }

        public static string ChuyenThanhKhongDau(string s)
        {
            if (string.IsNullOrEmpty(s) == true)
                return "";

            Regex regex = new Regex("\\p{IsCombiningDiacriticalMarks}+");
            string temp = s.Normalize(NormalizationForm.FormD);
            return regex.Replace(temp, String.Empty).Replace('\u0111', 'd').Replace('\u0110', 'D').ToLower();
        }

        public static string ChuyenSoThanhChu(string number)
        {
            string[] strTachPhanSauDauPhay;
            if (number.Contains(".") || number.Contains(","))
            {
                strTachPhanSauDauPhay = number.Split(',', '.');
                return (ChuyenSoThanhChu(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSoThanhChu(strTachPhanSauDauPhay[1]));
            }

            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "linh ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "năm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            return FirstCharToUpper(doc);
        }

        public static string FirstCharToUpper(string input)
        {
            if (String.IsNullOrEmpty(input))
                throw new ArgumentException("ARGH!");
            return input.First().ToString().ToUpper() + input.Substring(1);
        }

        public static string ChuyenSoThanhChu(int number)
        {
            return ChuyenSoThanhChu(number.ToString());
        }

        public static string GetUserSetting(string key)
        {
            try
            {
                UserSettingRepository userSettingRepository = new UserSettingRepository(new ErpDbContext());

                return userSettingRepository.GetUserSettingByKey(key, WebMatrix.WebData.WebSecurity.CurrentUserId);
            }
            catch { }

            return null;
        }


        public static string GetOrderNo(string key, string Code = null)
        {
            var manualOrderNo = Erp.API.Helpers.Common.GetSetting("manualOrderNo_" + key);//nhập tay
            if (manualOrderNo != null && manualOrderNo == "true" && Code != null)
            {
                return Code;
            }
            else
            {
                string machungtu_prefix = Helpers.Common.GetSetting("prefixOrderNo_" + key);//lấy mã tự động
                string machungtu_stt = Helpers.Common.GetSetting("orderNo_" + key);
                if (string.IsNullOrEmpty(machungtu_stt))
                {
                    machungtu_stt = "1";
                }
                while (machungtu_stt.Length < 4)
                {
                    machungtu_stt = "0" + machungtu_stt;
                }

                return machungtu_prefix + machungtu_stt;

            }
        }

        public static void SetOrderNo(string key)
        {
            var manualOrderNo = Erp.API.Helpers.Common.GetSetting("manualOrderNo_" + key);
            if (manualOrderNo == null || manualOrderNo != "true")
            {
                string machungtu_stt = Helpers.Common.GetSetting("orderNo_" + key);
                if (!string.IsNullOrEmpty(machungtu_stt))
                {
                    Helpers.Common.SetSetting("orderNo_" + key, (Convert.ToInt32(machungtu_stt) + 1).ToString());
                }
                else
                {
                    Helpers.Common.SetSetting("orderNo_" + key, "2");
                }
            }
        }
        public static void SetUserSetting(string key, string value)
        {
            try
            {
                UserSettingRepository userSettingRepository = new UserSettingRepository(new ErpDbContext());

                userSettingRepository.SetUserSettingByKey(key, WebMatrix.WebData.WebSecurity.CurrentUserId, value);
            }
            catch { }
        }

        public static string GetSetting(string key)
        {
            try
            {
                SettingRepository settingRepository = new SettingRepository(new ErpDbContext());

                return settingRepository.GetSettingByKey(key).Value;
            }
            catch { }

            return null;
        }

        public static void SetSetting(string key, string value)
        {
            try
            {
                SettingRepository settingRepository = new SettingRepository(new ErpDbContext());
                var setting = settingRepository.GetSettingByKey(key);
                setting.Value = value;
                settingRepository.Update(setting);
            }
            catch { }
        }
        public static Domain.Entities.Category GetCategoryByValueOrId(string keyField, object value)
        {
            try
            {
                keyField = keyField.ToLower();
                Domain.Entities.Category item = new Category();

                Domain.Repositories.CategoryRepository categoryRepository = new CategoryRepository(new ErpDbContext());
                switch (keyField)
                {
                    case "value":
                        item = categoryRepository.GetAllCategories().Where(x => x.Value == value).FirstOrDefault();
                        break;
                    case "id":
                        int id = -1;
                        if (int.TryParse(value.ToString(), out id))
                        {
                            item = categoryRepository.GetCategoryById(id);
                        }
                        break;
                }
                return item;
            }
            catch { }

            return null;
        }
        public static Domain.Entities.Category GetCategoryByValueCodeOrId(string keyField, string value, string Code)
        {
            try
            {
                keyField = keyField.ToLower();
                Domain.Entities.Category item = new Category();

                Domain.Repositories.CategoryRepository categoryRepository = new CategoryRepository(new ErpDbContext());
                switch (keyField)
                {
                    case "value":
                        item = categoryRepository.GetAllCategories().Where(x => x.Value == value && x.Code == Code).FirstOrDefault();
                        break;
                    case "id":
                        int id = -1;
                        if (int.TryParse(value.ToString(), out id))
                        {
                            item = categoryRepository.GetCategoryById(id);
                        }
                        break;
                }
                return item;
            }
            catch { }

            return null;
        }
        public static string KiemTraTonTaiHinhAnh(string Image, string NameUrlImage, string NoImage)
        {
            //NameUrlImage là cột code trong setting .
            var ImageUrl = "";
            //chọn thay thế ảnh khi không có tên hình trong database.
            switch (NoImage)
            {
                case "product":
                    NoImage = "/assets/css/images/noimage.gif";
                    break;
                case "user":
                    NoImage = "/assets/img/no-avatar.png";
                    break;
                case "service":
                    NoImage = "/assets/css/images/noimage.gif";
                    break;
            }
            //lấy đường dẫn hình ảnh
            var ImagePath = Helpers.Common.GetSetting(NameUrlImage);
            //var filepath = System.Web.HttpContext.Current.Server.MapPath("~" + ImagePath);
            //nếu có hình ảnh
            if (!string.IsNullOrEmpty(Image))
            {
                ImageUrl = ImagePath + Image;
                //kiểm tra hình ảnh có tồn tại hay không..
                //if (!System.IO.File.Exists(filepath + Image))
                //{
                //    ImageUrl = NoImage;
                //}
                //else
                //{
                //    ImageUrl = ImagePath + Image;
                //}
            }
            else
                //không có ảnh
                if (string.IsNullOrEmpty(Image))
                {
                    ImageUrl = NoImage;
                }
            return ImageUrl;
        }
        public static string GetSettingbyNote(string key)
        {
            try
            {
                SettingRepository settingRepository = new SettingRepository(new ErpDbContext());

                return settingRepository.GetSettingByKey(key).Note;
            }
            catch { }

            return null;
        }

         
        public static string GetWebConfig(string key)
        {
            return ConfigurationManager.AppSettings[key];
        }


        public static byte[] ObjectToByteArray(object obj)
        {
            if (obj == null)
                return null;
            BinaryFormatter bf = new BinaryFormatter();
            using (MemoryStream ms = new MemoryStream())
            {
                bf.Serialize(ms, obj);
                return ms.ToArray();
            }
        }
        public static string ChuyenSoThanhChu_2(string number)
        {
            string[] strTachPhanSauDauPhay;
            if (number.Contains(".") || number.Contains(","))
            {
                strTachPhanSauDauPhay = number.Split(',', '.');
                return (ChuyenSoThanhChu_2(strTachPhanSauDauPhay[0]) + "phẩy " + ChuyenSoThanhChu_2(strTachPhanSauDauPhay[1]));
            }

            string[] dv = { "", "mươi", "trăm", "nghìn", "triệu", "tỉ" };
            string[] cs = { "không", "một", "hai", "ba", "bốn", "năm", "sáu", "bảy", "tám", "chín" };
            string doc;
            int i, j, k, n, len, found, ddv, rd;

            len = number.Length;
            number += "ss";
            doc = "";
            found = 0;
            ddv = 0;
            rd = 0;

            i = 0;
            while (i < len)
            {
                //So chu so o hang dang duyet
                n = (len - i + 2) % 3 + 1;

                //Kiem tra so 0
                found = 0;
                for (j = 0; j < n; j++)
                {
                    if (number[i + j] != '0')
                    {
                        found = 1;
                        break;
                    }
                }

                //Duyet n chu so
                if (found == 1)
                {
                    rd = 1;
                    for (j = 0; j < n; j++)
                    {
                        ddv = 1;
                        switch (number[i + j])
                        {
                            case '0':
                                if (n - j == 3) doc += cs[0] + " ";
                                if (n - j == 2)
                                {
                                    if (number[i + j + 1] != '0') doc += "lẻ ";
                                    ddv = 0;
                                }
                                break;
                            case '1':
                                if (n - j == 3) doc += cs[1] + " ";
                                if (n - j == 2)
                                {
                                    doc += "mười ";
                                    ddv = 0;
                                }
                                if (n - j == 1)
                                {
                                    if (i + j == 0) k = 0;
                                    else k = i + j - 1;

                                    if (number[k] != '1' && number[k] != '0')
                                        doc += "mốt ";
                                    else
                                        doc += cs[1] + " ";
                                }
                                break;
                            case '5':
                                if ((i + j == len - 1) || (i + j + 3 == len - 1))
                                    doc += "năm ";
                                else
                                    doc += cs[5] + " ";
                                break;
                            default:
                                doc += cs[(int)number[i + j] - 48] + " ";
                                break;
                        }

                        //Doc don vi nho
                        if (ddv == 1)
                        {
                            doc += ((n - j) != 1) ? dv[n - j - 1] + " " : dv[n - j - 1];
                        }
                    }
                }


                //Doc don vi lon
                if (len - i - n > 0)
                {
                    if ((len - i - n) % 9 == 0)
                    {
                        if (rd == 1)
                            for (k = 0; k < (len - i - n) / 9; k++)
                                doc += "tỉ ";
                        rd = 0;
                    }
                    else
                        if (found != 0) doc += dv[((len - i - n + 1) % 9) / 3 + 2] + " ";
                }

                i += n;
            }

            if (len == 1)
                if (number[0] == '0' || number[0] == '5') return cs[(int)number[0] - 48];

            if (!string.IsNullOrEmpty(number))
            {
                var ky_tu_cuoi = number[len - 1].ToString();
                if (ky_tu_cuoi == "0")
                {
                    doc = doc + " đồng chẵn";
                }
                else
                {
                    doc = doc + " đồng";
                }
            }

            return doc;
        }

        public static string ChuyenSoThanhChu_2(int number)
        {
            return ChuyenSoThanhChu_2(number.ToString());
        }
        public static Object ByteArrayToObject(byte[] arrBytes)
        {
            MemoryStream memStream = new MemoryStream();
            BinaryFormatter binForm = new BinaryFormatter();
            memStream.Write(arrBytes, 0, arrBytes.Length);
            memStream.Seek(0, SeekOrigin.Begin);
            Object obj = (Object)binForm.Deserialize(memStream);
            return obj;
        }
        //hàm convert từ byte sang kilobyte, MB, GB
        public static string GetFileSize(double byteCount)
        {
            string size = "0 Bytes";
            if (byteCount >= 1073741824.0)
                size = String.Format("{0:##.##}", byteCount / 1073741824.0) + " GB";
            else if (byteCount >= 1048576.0)
                size = String.Format("{0:##.##}", byteCount / 1048576.0) + " MB";
            else if (byteCount >= 1024.0)
                size = String.Format("{0:##.##}", byteCount / 1024.0) + " KB";
            else if (byteCount > 0 && byteCount < 1024.0)
                size = byteCount.ToString() + " Bytes";

            return size;
        }
        public static string GetCodebyBranch(string prefix, int value, string BranchCode, int lenght = 6)
        {
            var numberStr = value.ToString();
            while (numberStr.Length < lenght)
            {
                numberStr = "0" + numberStr;
            }

            return prefix + BranchCode + numberStr;
        }

        public static bool KiemTraNgaySuaChungTu(DateTime CreatedDate)
        {
            var limit_daterange_for_update_data = GetSetting("limit_daterange_for_update_data");
            if (string.IsNullOrEmpty(limit_daterange_for_update_data))
            {
                limit_daterange_for_update_data = "0";
            }

            if (CreatedDate.AddDays(Convert.ToInt32(limit_daterange_for_update_data)) > DateTime.Now)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        public static string KiemTraTonTaiAnhKhacUrl(string Image, string NameUrlImage, string NoImage)
        {
            //NameUrlImage là cột code trong setting .
          
            //chọn thay thế ảnh khi không có tên hình trong database.
            switch (NoImage)
            {
                case "product":
                    NoImage = "/assets/css/images/noimage.gif";
                    break;
                case "user":
                    NoImage = "/assets/img/no-avatar.png";
                    break;
                case "service":
                    NoImage = "/assets/css/images/noimage.gif";
                    break;
            }
            //lấy đường dẫn hình ảnh
            var domain=Common.GetSetting("domain");
            var ImageUrl = domain + "";
            var ImagePath = Helpers.Common.GetSetting(NameUrlImage);
            var filepath = domain + ImagePath;
            //nếu có hình ảnh
            if (!string.IsNullOrEmpty(Image))
            {
                //ImageUrl = ImagePath + Image;
                ////kiểm tra hình ảnh có tồn tại hay không..
                //if (!System.IO.File.Exists(filepath + Image))
                //{
                //    ImageUrl = domain+NoImage;
                //}
                //else
                //{
                ImageUrl = filepath + "/" + Image;
                //}
            }
            else
                //không có ảnh
                if (string.IsNullOrEmpty(Image))
            {
                ImageUrl = domain + NoImage;
            }

            return ImageUrl;
        }

        public static SelectList GetSelectList_Category(string sCode, object SelectedValue, string NullOrNameEmpty)
        {
            return GetSelectList_Category(sCode, SelectedValue, null, NullOrNameEmpty);
        }
        public static SelectList GetSelectList_Category(string sCode, object SelectedValue, string sValueField, string NullOrNameEmpty)
        {
            CategoryRepository categoryRepository = new CategoryRepository(new Domain.ErpDbContext());
            var selectListItems = new List<SelectListItem>();

            if (NullOrNameEmpty != null)
            {
                SelectListItem itemEmpty = new SelectListItem();
                itemEmpty.Text = NullOrNameEmpty;
                itemEmpty.Value = null;
                selectListItems.Add(itemEmpty);
            }

            try
            {
                var q = categoryRepository.GetCategoryByCode(sCode);

                foreach (var i in q)
                {
                    SelectListItem item = new SelectListItem();
                    item.Text = i.Name;
                    if (sValueField != null && sValueField == "Name")
                        item.Value = i.Name;
                    else if (sValueField != null && sValueField == "Value")
                        item.Value = i.Value;
                    else
                        item.Value = i.Value;

                    selectListItems.Add(item);
                }
            }
            catch { }

            var selectList = new SelectList(selectListItems, "Value", "Text", SelectedValue);

            return selectList;
        }

    }

    public static class CommonSatic
    {
        public static DataTable ToDataTable<T>(this IList<T> data)
        {
            PropertyDescriptorCollection props =
                TypeDescriptor.GetProperties(typeof(T));
            DataTable table = new DataTable();
            for (int i = 0; i < props.Count; i++)
            {
                PropertyDescriptor prop = props[i];
                table.Columns.Add(prop.Name, prop.PropertyType);
            }
            object[] values = new object[props.Count];
            foreach (T item in data)
            {
                for (int i = 0; i < values.Length; i++)
                {
                    values[i] = props[i].GetValue(item);
                }
                table.Rows.Add(values);
            }
            return table;
        }
    }


}


