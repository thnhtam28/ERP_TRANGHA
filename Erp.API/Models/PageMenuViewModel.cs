using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Erp.API.Models
{
    public class PageMenuViewModel
    {
        public int Id { get; set; }
        public string Name { get; set; }

        public string Url { get; set; }
        public List<PageMenuViewModel> ListSubmenu { get; set; }
    }
}