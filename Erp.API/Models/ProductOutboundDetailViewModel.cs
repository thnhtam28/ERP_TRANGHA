using Erp.Domain.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Erp.API.Models
{
    public class ProductOutboundDetailViewModel
    {
        public int Id { get; set; }
        public bool? IsDeleted { get; set; }
        public int? CreatedUserId { get; set; }
        public string CreatedUserName { get; set; }
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedUserId { get; set; }
        public string ModifiedUserName { get; set; }
        public DateTime? ModifiedDate { get; set; }
        public Nullable<int> ProductOutboundId { get; set; }
        public Nullable<int> ProductId { get; set; }
        public string ProductName { get; set; }
        public string ProductCode { get; set; }
        public Nullable<int> Quantity { get; set; }
        public decimal? Price { get; set; }
        public string Unit { get; set; }
        public int? DisCount { get; set; }
        public int? DisCountAmount { get; set; }
        public string LoCode { get; set; }
        public DateTime? ExpiryDate { get; set; }
        public int OrderNo { get; set; }
        public int? WarehouseDestinationId { get; set; }
        public int? WarehouseSourceId { get; set; }
        public string ProductType { get; set; }
        public int? QuantityInInventory { get; set; }
        public List<WarehouseLocationItemViewModel> ListWarehouseLocationItemViewModel { get; set; }
    }
}