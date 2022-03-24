
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class Sale_BaoCaoKetQuaBanHangTheoTungNgayViewModel
    {
        public DateTime? CreatedDate { get; set; }
        public decimal? ReceiptAmount { get; set; }
        public decimal? PaymentAmount { get; set; }
        public decimal? DifferenceAmount { get; set; }

    }
}