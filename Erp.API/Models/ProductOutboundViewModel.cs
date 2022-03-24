using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class ProductOutboundViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public string Code { get; set; }
        public string Note { get; set; }
        public Nullable<int> WarehouseSourceId { get; set; }
        public string WarehouseSourceName { get; set; }
        public Nullable<int> WarehouseKeeperId { get; set; }
        public string Type { get; set; }
        public Nullable<int> InvoiceId { get; set; }
        public string InvoiceCode { get; set; }
        public Nullable<int> WarehouseDestinationId { get; set; }
        public string WarehouseDestinationName { get; set; }
        public decimal? TotalAmount { get; set; }
        public int? PurchaseOrderId { get; set; }
        public string PurchaseOrderCode { get; set; }
        public int? SalerId { get; set; }
        public string ReasonManual { get; set; }
        public string CustomerName { get; set; }
        public bool IsArchive { get; set; }
        public bool AllowEdit { get; set; }
        public string CancelReason { get; set; }
        public List<ProductOutboundDetailViewModel> DetailList { get; set; }
        public IEnumerable<SelectListItem> SelectList_WarehouseSource { get; set; }
        public List<WarehouseLocationItemViewModel> LocationItemList { get; set; }
    }
}