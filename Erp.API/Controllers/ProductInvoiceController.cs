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
namespace Erp.API.Controllers
{
    public class ProductInvoiceController : ApiController
    {
        #region Create
        [HttpPost]
        public string Create([FromBody] ProductInvoiceViewModel model)
        {

            try
            {
                if (model.DetailList.Count != 0)
                {
                    #region Khởi tạo Repository
                    CategoryRepository categoryRepository = new CategoryRepository(new Erp.Domain.ErpDbContext());
                    ProductOrServiceRepository ProductRepository = new ProductOrServiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    SupplierRepository SupplierRepository = new SupplierRepository(new Erp.Domain.Sale.ErpSaleDbContext());
                    CustomerRepository CustomerRepository = new CustomerRepository(new Erp.Domain.Account.ErpAccountDbContext());
                    ProductInvoiceRepository productInvoiceRepository = new ProductInvoiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());

                    BOLogRepository BOLogRepository = new BOLogRepository(new Erp.Domain.ErpDbContext());
                    UserRepository UserRepository = new UserRepository(new Domain.ErpDbContext());

                    #endregion

                    var User = UserRepository.GetUserById(model.CreatedUserId.Value);
                    model.BranchId = User.BranchId;
                    ProductInvoice productInvoice = null;
                    productInvoice = new ProductInvoice();
                    AutoMapper.Mapper.CreateMap<ProductInvoiceViewModel, ProductInvoice>();
                    AutoMapper.Mapper.Map(model, productInvoice);
                    productInvoice.IsDeleted = false;
                    productInvoice.ModifiedUserId = model.CreatedUserId;
                    productInvoice.CreatedDate = DateTime.Now;
                    productInvoice.Status = "pending";
                    productInvoice.IsArchive = false;
                    productInvoice.IsReturn = false;
                    productInvoice.RemainingAmount = 0;
                    productInvoice.TotalAmount = model.TongTienSauVAT;
                    productInvoice.Discount = 0;
                    productInvoice.DiscountAmount = 0;
                    productInvoice.ModifiedDate = DateTime.Now;
                    productInvoice.PaidAmount = 0;
                    productInvoice.RemainingAmount = productInvoice.TotalAmount;
                    //Duyệt qua danh sách sản phẩm mới xử lý tình huống user chọn 2 sản phầm cùng id
                    string strBrand = "";
                    List<ProductInvoiceDetail> listNewCheckSameId = new List<ProductInvoiceDetail>();
                    model.DetailList.RemoveAll(x => x.Quantity <= 0);
                    foreach (var group in model.DetailList)
                    {
                        var product = ProductRepository.GetProductById(group.ProductId.Value);
                        strBrand = product.Origin;
                        listNewCheckSameId.Add(new ProductInvoiceDetail
                        {
                            ProductId = product.Id,
                           // ProductType = product.Type,
                            Quantity = group.Quantity,
                            Unit = product.Unit,
                            Price = group.Price,
                            IsDeleted = false,
                            CreatedUserId = model.CreatedUserId,
                            ModifiedUserId = model.CreatedUserId,
                            CreatedDate = DateTime.Now,
                            ModifiedDate = DateTime.Now,
                            LoCode = group.LoCode,
                            ExpiryDate = group.ExpiryDate,
                            Discount = 0,
                            DiscountAmount = 0,
                        });
                    }

                    //hoapd them nhan hang
                    productInvoice.CountForBrand = strBrand;

                    //hàm thêm mới
                    productInvoiceRepository.InsertProductInvoice(productInvoice, listNewCheckSameId);
                    //cập nhật lại mã hóa đơn
                    productInvoice.Code = Erp.API.Helpers.Common.GetOrderNo("ProductInvoice", model.Code);
                    productInvoiceRepository.UpdateProductInvoice(productInvoice);
                    Erp.API.Helpers.Common.SetOrderNo("ProductInvoice");

                    //Thêm vào quản lý chứng từ
                    CreateTransaction(new TransactionViewModel
                    {
                        TransactionModule = "ProductInvoice",
                        TransactionCode = productInvoice.Code,
                        TransactionName = "Bán hàng"
                    }, model.CreatedUserId);
                    return "success";
                }
                return "failed";
            }
            catch (Exception ex)
            {
                return ex.Message;
            }
            // return "success";
        }

        #region CreateTransaction
        public static void CreateTransaction(TransactionViewModel model, int? CreatedUserId)
        {
            TransactionRepository transactionRepository = new Domain.Account.Repositories.TransactionRepository(new Domain.Account.ErpAccountDbContext());
            var transaction = transactionRepository.GetAllTransaction()
                .Where(item => item.TransactionCode == model.TransactionCode).FirstOrDefault();

            if (transaction == null)
            {
                transaction = new Transaction();

                transaction.IsDeleted = false;
                transaction.CreatedUserId = CreatedUserId;
                transaction.ModifiedUserId = CreatedUserId;
                transaction.AssignedUserId = CreatedUserId;
                transaction.CreatedDate = DateTime.Now;
                transaction.ModifiedDate = DateTime.Now;
                transaction.TransactionModule = model.TransactionModule;
                transaction.TransactionCode = model.TransactionCode;
                transaction.TransactionName = model.TransactionName;
                transactionRepository.InsertTransaction(transaction);
            }
            else
            {
                transaction.ModifiedUserId = CreatedUserId;
                transaction.ModifiedDate = DateTime.Now;
                transaction.TransactionName = model.TransactionName;
                transactionRepository.UpdateTransaction(transaction);
            }
        }
        #endregion

        #region CreateRelationship
        public static void CreateRelationship(TransactionRelationshipViewModel model)
        {
            TransactionRepository transactionRelationshipRepository = new Domain.Account.Repositories.TransactionRepository(new Domain.Account.ErpAccountDbContext());
            var transactionRelationship = new TransactionRelationship();
            transactionRelationship.IsDeleted = false;
            //transactionRelationship.CreatedUserId = WebSecurity.CurrentUserId;
            //transactionRelationship.ModifiedUserId = WebSecurity.CurrentUserId;
            //transactionRelationship.AssignedUserId = WebSecurity.CurrentUserId;
            transactionRelationship.CreatedDate = DateTime.Now;
            transactionRelationship.ModifiedDate = DateTime.Now;
            transactionRelationship.TransactionA = model.TransactionA;
            transactionRelationship.TransactionB = model.TransactionB;
            transactionRelationshipRepository.InsertTransactionRelationship(transactionRelationship);
        }
        #endregion

        #region CreateTransactionLiabilities
        public static TransactionLiabilities CreateTransactionLiabilities(string TransactionCode, string TransactionModule, string TransactionName, string TargetCode, string TargetModule, decimal Debit, decimal Credit, string MaChungTuGoc, string LoaiChungTuGoc, string PaymentMethod, DateTime? NextPaymentDate, string Note, int? MaChungTuGoc_DetailId = null, int? MaChungTuGoc_DetailId2 = null)
        {
            TransactionLiabilitiesRepository transactionRepository = new TransactionLiabilitiesRepository(new Domain.Account.ErpAccountDbContext());

            var transaction = new TransactionLiabilities();
            transaction.IsDeleted = false;
            //transaction.CreatedUserId = WebSecurity.CurrentUserId;
            //transaction.ModifiedUserId = WebSecurity.CurrentUserId;
            //transaction.AssignedUserId = WebSecurity.CurrentUserId;
            transaction.CreatedDate = DateTime.Now;
            transaction.ModifiedDate = DateTime.Now;

            transaction.TransactionCode = TransactionCode;
            transaction.TransactionModule = TransactionModule;
            transaction.TransactionName = TransactionName;
            transaction.TargetCode = TargetCode;
            transaction.TargetModule = TargetModule;
            transaction.Debit = Debit;
            transaction.Credit = Credit;
            transaction.PaymentMethod = PaymentMethod;
            transaction.NextPaymentDate = NextPaymentDate;
            transaction.MaChungTuGoc = MaChungTuGoc;
            transaction.LoaiChungTuGoc = LoaiChungTuGoc;
            transaction.Note = Note;
            //transaction.MaChungTuGoc_DetailId = MaChungTuGoc_DetailId;
            //transaction.MaChungTuGoc_DetailId2 = MaChungTuGoc_DetailId2;
            transactionRepository.InsertTransaction(transaction);
            return transaction;
        }
        #endregion

        #region insertInvoice
        void insertInvoice(ProductInvoiceViewModel model, ProductInvoice productInvoice, List<ProductInvoiceDetail> listNewCheckSameId, int CreatedUserId, ProductInvoiceRepository productInvoiceRepository)
        {
            productInvoiceRepository.InsertProductInvoice(productInvoice, listNewCheckSameId);
            foreach (var item in listNewCheckSameId)
            {
                model.DetailList.Where(x => x.ProductId == item.ProductId).FirstOrDefault().Id = item.Id;
            }
            //Cập nhật lại mã hóa đơn
            productInvoice.Code = Erp.API.Helpers.Common.GetOrderNo("ProductInvoice", model.Code);
            productInvoiceRepository.UpdateProductInvoice(productInvoice);
            Erp.API.Helpers.Common.SetOrderNo("ProductInvoice");

            //Thêm vào quản lý chứng từ
            CreateTransaction(new TransactionViewModel
            {
                TransactionModule = "ProductInvoice",
                TransactionCode = productInvoice.Code,
                TransactionName = "Bán hàng"
            }, CreatedUserId);
            //SavePost(productInvoice.Id, "ProductInvoice", "Bán hàng (" + productInvoice.Code + ")", CreatedUserId, postRepository);

        }
        #endregion

        //#region archiveOutbound
        //void archiveOutbound(List<ProductInvoiceDetailViewModel> DetailList, ProductInvoice productInvoice, int BranchId, int CreatedUserId, WarehouseRepository WarehouseRepository, ProductOutboundRepository ProductOutboundRepository, WarehouseLocationItemRepository WarehouseLocationItemRepository, PostRepository postRepository)
        //{
        //    ProductOutboundViewModel productOutboundViewModel = new ProductOutboundViewModel();
        //    var warehouseDefault = WarehouseRepository.GetAllWarehouse().Where(x => x.BranchId == BranchId && x.IsSale == true).FirstOrDefault();

        //    //Nếu trong đơn hàng có sản phẩm thì xuất kho
        //    if (warehouseDefault != null)
        //    {
        //        productOutboundViewModel.InvoiceId = productInvoice.Id;
        //        productOutboundViewModel.InvoiceCode = productInvoice.Code;
        //        productOutboundViewModel.WarehouseSourceId = warehouseDefault.Id;
        //        productOutboundViewModel.Note = "Xuất kho cho đơn hàng " + productInvoice.Code;

        //        //Lấy dữ liệu cho chi tiết
        //        productOutboundViewModel.DetailList = new List<ProductOutboundDetailViewModel>();

        //        foreach (var item in DetailList)
        //        {
        //            var ProductOutboundDetail = new ProductOutboundDetailViewModel();
        //            ProductOutboundDetail.IsDeleted = false;
        //            ProductOutboundDetail.CreatedDate = DateTime.Now;
        //            ProductOutboundDetail.CreatedUserId = CreatedUserId;
        //            ProductOutboundDetail.ModifiedDate = DateTime.Now;
        //            ProductOutboundDetail.ModifiedUserId = CreatedUserId;
        //            ProductOutboundDetail.ProductId = item.ProductId;
        //            ProductOutboundDetail.Quantity = item.Quantity;
        //            ProductOutboundDetail.Price = item.Price;
        //            ProductOutboundDetail.Id = item.Id;

        //            productOutboundViewModel.DetailList.Add(ProductOutboundDetail);
        //        }

        //        var productOutbound = CreateFromInvoice(ProductOutboundRepository, productOutboundViewModel, productInvoice.Code, CreatedUserId, BranchId);
        //        //SavePost(productInvoice.Id, "ProductInvoice", "Xuất kho bán hàng (" + productOutbound.Code + ")", CreatedUserId, postRepository);
        //        //Ghi sổ chứng từ phiếu xuất
        //        Archive(ProductOutboundRepository, WarehouseLocationItemRepository, productOutbound, CreatedUserId);
        //    }
        //}
        //#endregion

        //#region archiveReciept
        //void archiveReciept(ProductInvoiceViewModel model,
        //    CustomerRepository customerRepository,
        //    ProductInvoiceRepository productInvoiceRepository,
        //    PurchaseOrderRepository PurchaseOrderRepository,
        //    ReceiptRepository ReceiptRepository,
        //    ReceiptDetailRepository ReceiptDetailRepository,
        //    PaymentRepository paymentRepository,
        //    PaymentDetailRepository paymentDetailRepository,
        //    ProductOrServiceRepository ProductRepository)
        //{
        //    //Bắt đầu ghi sổ
        //    var customer = customerRepository.GetvwCustomerById(model.CustomerId.Value);
        //    var productInvoice = productInvoiceRepository.GetProductInvoiceById(model.Id);
        //    var listproductInvoiceDetail = new List<ProductInvoiceDetail>();
        //    if (model.InvoiceDetailId != null)
        //    {
        //        listproductInvoiceDetail = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(model.Id).Where(x => x.Id == model.InvoiceDetailId).ToList();
        //    }
        //    else
        //    {
        //        listproductInvoiceDetail = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(model.Id).ToList();
        //    }

        //    decimal tongSoTienThanhToan = model.PaymentList.Where(x => x.Amount > 0).Sum(x => x.Amount);
        //    if (tongSoTienThanhToan > 0)
        //    {
        //        //Thêm thanh toán đối trừ
        //        foreach (var item in model.PaymentList.Where(x => x.Name == "Đối trừ" && x.ProductInvoiceDetailId != null))
        //        {
        //            var productInvoiceDetailViewModel = model.DetailList.Where(x => x.OrderNo == item.ProductInvoiceDetailId).FirstOrDefault();
        //            if (productInvoiceDetailViewModel != null)
        //            {
        //                //var Payment = createPayment(customer, null, item.Name, item.Amount, model.CreatedUserId, paymentRepository);
        //                var productInvoiceDetail = listproductInvoiceDetail.Where(x => x.Id == productInvoiceDetailViewModel.Id).FirstOrDefault();
        //                if (item.SubList != null)
        //                {
        //                    foreach (var subItem in item.SubList)
        //                    {
        //                        //Mã hàng A
        //                        var ProductA = ProductRepository.GetProductById(productInvoiceDetail.ProductId.Value);
        //                        //Mã hàng B
        //                        var PurchaseOrderDetail = PurchaseOrderRepository.GetPurchaseOrderDetailById(subItem.PurchaseOrderDetailId.Value);
        //                        var PurchaseOrder = PurchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderDetail.PurchaseOrderId.Value);
        //                        var ProductB = ProductRepository.GetProductById(PurchaseOrderDetail.ProductId.Value);
        //                        //createReceiptDetail(Receipt, productInvoice, "Thu tiền khách hàng", subItem.Amount, item.Name, productInvoiceDetail.Id, subItem.ProductInvoiceDetailId, model.CreatedUserId, ReceiptDetailRepository);
        //                        //Ghi Có TK 131 - Phải trả nhà cung cấp.
        //                        CreateTransactionLiabilities(
        //                             "DT-" + ProductA.Code + "-" + ProductB.Code,
        //                            "Exchange",
        //                            "Đối trừ",
        //                            customer.Code,
        //                            "Customer",
        //                            0,
        //                            Convert.ToDecimal(subItem.Amount),
        //                            productInvoice.Code,
        //                            "ProductInvoice",
        //                            item.Name,
        //                            null,
        //                            null,
        //                            productInvoiceDetail.Id,
        //                            PurchaseOrderDetail.Id);

        //                        //productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + subItem.Amount;
        //                        //productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                        //Chi tiền đối trừ mã mua lại
        //                        //createPaymentDetail(Payment, PurchaseOrder.Code, subItem.Amount, item.Name, PurchaseOrderDetail.Id, productInvoiceDetail.Id, model.CreatedUserId, paymentDetailRepository);
        //                        CreateTransactionLiabilities(
        //                             "DT-" + ProductB.Code + "-" + ProductA.Code,
        //                            "Exchange",
        //                            "Đối trừ",
        //                            customer.Code,
        //                             "Customer",
        //                            0,
        //                            Convert.ToDecimal(subItem.Amount),
        //                            PurchaseOrder.Code,
        //                            "PurchaseOrder",
        //                            item.Name,
        //                            null,
        //                            null,
        //                             PurchaseOrderDetail.Id,
        //                              productInvoiceDetail.Id);
        //                        //update Payment
        //                        //Payment.MaChungTuGoc = PurchaseOrder.Code;
        //                        //paymentRepository.UpdatePayment(Payment);
        //                        ////Thêm chứng từ liên quan
        //                        //CreateRelationship(new TransactionRelationshipViewModel
        //                        //{
        //                        //    TransactionA = Payment.Code,
        //                        //    TransactionB = PurchaseOrder.Code
        //                        //});
        //                        // lấy chi tết PurchaseOrder update lại

        //                        //PurchaseOrderDetail.PaidAmount += subItem.Amount;
        //                        //PurchaseOrderDetail.RemainAmount = PurchaseOrderDetail.Amount - PurchaseOrderDetail.PaidAmount;
        //                        PurchaseOrderRepository.UpdatePurchaseOrderDetail(PurchaseOrderDetail);
        //                        // lấy PurchaseOrder update lại

        //                        PurchaseOrder.PaidAmount += subItem.Amount;
        //                        PurchaseOrder.RemainingAmount = PurchaseOrder.TotalAmount - PurchaseOrder.PaidAmount;
        //                        PurchaseOrderRepository.UpdatePurchaseOrder(PurchaseOrder);
        //                    }
        //                }
        //            }
        //            //Tính lại số tiền cho từng mã
        //        }

        //        //Thêm thanh toán tiền mặt/chuyển khoản
        //        var danh_sach_ma_hang_con_lai = listproductInvoiceDetail.Where(x => x.Quantity > 0).ToList();
        //        var danh_sach_thanh_toan = model.PaymentList.Where(x => x.Amount > 0 && x.Name != "Đối trừ").ToList();
        //        int i = 0;
        //        int j = 0;
        //        var tongTienThanhToan2 = danh_sach_thanh_toan.Sum(item => item.Amount);
        //        var Receipt = new Receipt();
        //        if (tongTienThanhToan2 > 0)
        //        {
        //            Receipt = createReceipt(customer, productInvoice, null, tongTienThanhToan2, model.CreatedUserId, ReceiptRepository);
        //        }

        //        while (tongTienThanhToan2 > 0)
        //        {
        //            var productInvoiceDetail = danh_sach_ma_hang_con_lai[i];
        //            var receipt = danh_sach_thanh_toan[j];

        //            if (productInvoiceDetail.RemainAmount <= receipt.Amount)
        //            {
        //                createReceiptDetail(Receipt, productInvoice, "Thu tiền khách hàng", productInvoiceDetail.RemainAmount.Value, receipt.Name, productInvoiceDetail.Id, null, model.CreatedUserId, ReceiptDetailRepository);
        //                CreateTransactionLiabilities(
        //                Receipt.Code,
        //                "Receipt",
        //                "Thu tiền khách hàng",
        //                customer.Code,
        //                 "Customer",
        //                0,
        //                Convert.ToDecimal(productInvoiceDetail.RemainAmount),
        //                productInvoice.Code,
        //                "ProductInvoice",
        //                receipt.Name,
        //                null,
        //                null,
        //                productInvoiceDetail.Id);

        //                receipt.Amount = receipt.Amount - productInvoiceDetail.RemainAmount.Value;
        //                tongTienThanhToan2 = tongTienThanhToan2 - productInvoiceDetail.RemainAmount.Value;

        //                //Cập nhật lại chi tiết đơn hàng
        //                productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + productInvoiceDetail.RemainAmount;
        //                productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;

        //                if (receipt.Amount == 0)
        //                {
        //                    j++;
        //                }

        //                i++;
        //            }
        //            else
        //            {
        //                createReceiptDetail(Receipt, productInvoice, "Thu tiền khách hàng", receipt.Amount, receipt.Name, productInvoiceDetail.Id, null, model.CreatedUserId, ReceiptDetailRepository);
        //                CreateTransactionLiabilities(
        //                Receipt.Code,
        //                "Receipt",
        //                "Thu tiền khách hàng",
        //                customer.Code,
        //                "Customer",
        //                0,
        //                Convert.ToDecimal(receipt.Amount),
        //                productInvoice.Code,
        //                "ProductInvoice",
        //                receipt.Name,
        //                null,
        //                null,
        //                productInvoiceDetail.Id);

        //                tongTienThanhToan2 = tongTienThanhToan2 - receipt.Amount;

        //                //Cập nhật lại chi tiết đơn hàng
        //                productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + receipt.Amount;
        //                productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;

        //                if (productInvoiceDetail.RemainAmount == 0)
        //                {
        //                    i++;
        //                }
        //                j++;
        //            }
        //        }
        //    }

        //    //Cập nhật đơn hàng
        //    productInvoice.ModifiedUserId = model.CreatedUserId;
        //    productInvoice.ModifiedDate = DateTime.Now;

        //    productInvoice.PaidAmount += tongSoTienThanhToan;
        //    productInvoice.RemainingAmount = productInvoice.TotalAmount - productInvoice.PaidAmount;
        //    productInvoice.IsArchive = true;

        //    if (productInvoice.RemainingAmount > 0)
        //    {
        //        productInvoice.Status = "waiting";
        //    }
        //    else
        //    {
        //        productInvoice.Status = "complete";
        //    }
        //    productInvoiceRepository.UpdateProductInvoice(productInvoice);
        //    // cập nhật lại chi tiết đơn hàng
        //    foreach (var item in listproductInvoiceDetail)
        //    {
        //        item.ProductInvoiceId = productInvoice.Id;
        //        productInvoiceRepository.UpdateProductInvoiceDetail(item);
        //    }
        //}
        //#endregion

        //#region archiveRecieptPending
        //void archiveRecieptPending(ProductInvoiceViewModel model, CustomerRepository customerRepository, ProductInvoiceRepository productInvoiceRepository, PurchaseOrderRepository PurchaseOrderRepository, ReceiptRepository ReceiptRepository, ReceiptDetailRepository ReceiptDetailRepository, PaymentRepository paymentRepository, PaymentDetailRepository paymentDetailRepository, ProductRepository ProductRepository, SalesDiscountRepository SalesDiscountRepository, SalesReturnsRepository SalesReturnsRepository)
        //{
        //    //Bắt đầu ghi sổ
        //    var customer = customerRepository.GetvwCustomerById(model.CustomerId.Value);
        //    var productInvoice = productInvoiceRepository.GetProductInvoiceById(model.Id);
        //    var listproductInvoiceDetail = new List<ProductInvoiceDetail>();
        //    if (model.InvoiceDetailId != null)
        //    {
        //        listproductInvoiceDetail = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(model.Id).Where(x => x.Id == model.InvoiceDetailId).ToList();
        //    }
        //    else
        //    {
        //        listproductInvoiceDetail = productInvoiceRepository.GetAllInvoiceDetailsByInvoiceId(model.Id).ToList();
        //    }
        //    decimal tongSoTienThanhToan = model.PayTotalAmount.Value;
        //    var danh_sach_ma_hang = listproductInvoiceDetail.Where(x => x.Quantity > 0).ToList();
        //    int i = 0;
        //    foreach (var item in model.ExchangeVoucherList)
        //    {
        //        if (item.IsCheck == true)
        //        {
        //            var productInvoiceDetail = danh_sach_ma_hang[i];
        //            if (item.LoaiCT == "ReceiptPending")
        //            {
        //                var receipt = ReceiptRepository.GetReceiptById(item.Id);
        //                if (tongSoTienThanhToan > 0)
        //                {
        //                    while (tongSoTienThanhToan > 0 && receipt.Amount > 0)
        //                    {

        //                        if (productInvoiceDetail.Quantity <= receipt.Amount)
        //                        {
        //                            // tạo chi tiết cho phiếu thu
        //                            createReceiptDetail(receipt, productInvoice, "Đối trừ công nợ",0, "Đối trừ công nợ", productInvoiceDetail.Id, null, model.CreatedUserId, ReceiptDetailRepository);
        //                            //Thêm chứng từ liên quan
        //                            CreateRelationship(new TransactionRelationshipViewModel
        //                            {
        //                                TransactionA = receipt.Code,
        //                                TransactionB = productInvoice.Code
        //                            });
        //                            CreateTransactionLiabilities(
        //                              receipt.Code,
        //                              "Receipt",
        //                              "Thu tiền khách hàng",
        //                              customer.Code,
        //                               "Customer",
        //                              0,
        //                              Convert.ToDecimal(productInvoiceDetail.Quantity),
        //                              productInvoice.Code,
        //                              "ProductInvoice",
        //                              "Đối trừ công nợ",
        //                              null,
        //                              null,
        //                              productInvoiceDetail.Id);
        //                            //receipt.RemainAmount = receipt.RemainAmount - productInvoiceDetail.RemainAmount.Value;
        //                            //receipt.PaidAmount += productInvoiceDetail.RemainAmount.Value;
        //                            ReceiptRepository.UpdateReceipt(receipt);
        //                            //tongSoTienThanhToan = tongSoTienThanhToan - productInvoiceDetail.RemainAmount.Value;
        //                            //Cập nhật lại chi tiết đơn hàng
        //                            //productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + productInvoiceDetail.RemainAmount;
        //                            //productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                            i++;
        //                        }
        //                        else
        //                        {
        //                            // tạo chi tiết cho phiếu thu
        //                            createReceiptDetail(receipt, productInvoice, "Đối trừ công nợ", receipt.Amount, "Đối trừ công nợ", productInvoiceDetail.Id, null, model.CreatedUserId, ReceiptDetailRepository);
        //                            //Thêm chứng từ liên quan
        //                            CreateRelationship(new TransactionRelationshipViewModel
        //                            {
        //                                TransactionA = receipt.Code,
        //                                TransactionB = productInvoice.Code
        //                            });
        //                            CreateTransactionLiabilities(
        //                              receipt.Code,
        //                              "Receipt",
        //                              "Thu tiền khách hàng",
        //                              customer.Code,
        //                               "Customer",
        //                              0,
        //                              Convert.ToDecimal(receipt.Amount),
        //                              productInvoice.Code,
        //                              "ProductInvoice",
        //                              "Đối trừ công nợ",
        //                              null,
        //                              null,
        //                              productInvoiceDetail.Id);

        //                            //tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(receipt.RemainAmount);
        //                            //Cập nhật lại chi tiết đơn hàng
        //                           // productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + Convert.ToDecimal(receipt.RemainAmount);
        //                           // productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                           // receipt.PaidAmount = Convert.ToDecimal(receipt.Amount);
        //                            //receipt.RemainAmount = 0;
        //                            ReceiptRepository.UpdateReceipt(receipt);
        //                        }
        //                    }
        //                }
        //            }
        //            else if (item.LoaiCT == "SalesDiscount")
        //            {
        //                //var SalesDiscount = SalesDiscountRepository.GetSalesDiscountById(item.Id);
        //                //if (tongSoTienThanhToan > 0)
        //                //{
        //                //    while (tongSoTienThanhToan > 0 && SalesDiscount.RemainAmount > 0)
        //                //    {
        //                //        if (productInvoiceDetail.RemainAmount <= SalesDiscount.RemainAmount)
        //                //        {
        //                //            //Thêm chứng từ liên quan
        //                //            CreateRelationship(new TransactionRelationshipViewModel
        //                //            {
        //                //                TransactionA = SalesDiscount.Code,
        //                //                TransactionB = productInvoice.Code
        //                //            });
        //                //            CreateTransactionLiabilities(
        //                //              SalesDiscount.Code,
        //                //              "SalesDiscount",
        //                //              "Hóa đơn giảm giá hàng bán",
        //                //              customer.Code,
        //                //               "Customer",
        //                //              0,
        //                //              Convert.ToDecimal(productInvoiceDetail.RemainAmount),
        //                //              productInvoice.Code,
        //                //              "ProductInvoice",
        //                //              "Đối trừ công nợ",
        //                //              null,
        //                //              null,
        //                //              productInvoiceDetail.Id);
        //                //            SalesDiscount.RemainAmount = SalesDiscount.RemainAmount - productInvoiceDetail.RemainAmount.Value;
        //                //            SalesDiscount.PaidAmount += productInvoiceDetail.RemainAmount.Value;
        //                //            SalesDiscountRepository.UpdateSalesDiscount(SalesDiscount);
        //                //            tongSoTienThanhToan = tongSoTienThanhToan - productInvoiceDetail.RemainAmount.Value;
        //                //            //Cập nhật lại chi tiết đơn hàng
        //                //            productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + productInvoiceDetail.RemainAmount;
        //                //            productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                //            i++;
        //                //        }
        //                //        else
        //                //        {

        //                //            //Thêm chứng từ liên quan
        //                //            CreateRelationship(new TransactionRelationshipViewModel
        //                //            {
        //                //                TransactionA = SalesDiscount.Code,
        //                //                TransactionB = productInvoice.Code
        //                //            });
        //                //            CreateTransactionLiabilities(
        //                //             SalesDiscount.Code,
        //                //              "SalesDiscount",
        //                //              "Hóa đơn giảm giá hàng bán",
        //                //              customer.Code,
        //                //               "Customer",
        //                //              0,
        //                //              Convert.ToDecimal(SalesDiscount.RemainAmount),
        //                //              productInvoice.Code,
        //                //              "ProductInvoice",
        //                //              "Đối trừ công nợ",
        //                //              null,
        //                //              null,
        //                //              productInvoiceDetail.Id);

        //                //            tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(SalesDiscount.RemainAmount);
        //                //            //Cập nhật lại chi tiết đơn hàng
        //                //            productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + Convert.ToDecimal(SalesDiscount.RemainAmount);
        //                //            productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                //            SalesDiscount.PaidAmount = Convert.ToDecimal(SalesDiscount.TotalAmount);
        //                //            SalesDiscount.RemainAmount = 0;
        //                //            SalesDiscountRepository.UpdateSalesDiscount(SalesDiscount);
        //                //        }
        //                //    }
        //                //}
        //            }
        //            else if (item.LoaiCT == "SalesReturns")
        //            {
        //                //var SalesReturns = SalesReturnsRepository.GetSalesReturnsById(item.Id);
        //                //if (tongSoTienThanhToan > 0)
        //                //{
        //                //    while (tongSoTienThanhToan > 0 && SalesReturns.RemainAmount > 0)
        //                //    {
        //                //        if (productInvoiceDetail.RemainAmount <= SalesReturns.RemainAmount)
        //                //        {
        //                //            //Thêm chứng từ liên quan
        //                //            CreateRelationship(new TransactionRelationshipViewModel
        //                //            {
        //                //                TransactionA = SalesReturns.Code,
        //                //                TransactionB = productInvoice.Code
        //                //            });
        //                //            CreateTransactionLiabilities(
        //                //              SalesReturns.Code,
        //                //              "SalesReturns",
        //                //              "Hóa đơn hàng bán trả lại",
        //                //              customer.Code,
        //                //               "Customer",
        //                //              0,
        //                //              Convert.ToDecimal(productInvoiceDetail.RemainAmount),
        //                //              productInvoice.Code,
        //                //              "ProductInvoice",
        //                //              "Đối trừ công nợ",
        //                //              null,
        //                //              null,
        //                //              productInvoiceDetail.Id);
        //                //            SalesReturns.RemainAmount = SalesReturns.RemainAmount - productInvoiceDetail.RemainAmount.Value;
        //                //            SalesReturns.PaidAmount += productInvoiceDetail.RemainAmount.Value;
        //                //            SalesReturnsRepository.UpdateSalesReturns(SalesReturns);
        //                //            tongSoTienThanhToan = tongSoTienThanhToan - productInvoiceDetail.RemainAmount.Value;
        //                //            //Cập nhật lại chi tiết đơn hàng
        //                //            productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + productInvoiceDetail.RemainAmount;
        //                //            productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                //            i++;
        //                //        }
        //                //        else
        //                //        {

        //                //            //Thêm chứng từ liên quan
        //                //            CreateRelationship(new TransactionRelationshipViewModel
        //                //            {
        //                //                TransactionA = SalesReturns.Code,
        //                //                TransactionB = productInvoice.Code
        //                //            });
        //                //            CreateTransactionLiabilities(
        //                //              SalesReturns.Code,
        //                //              "SalesReturns",
        //                //              "Hóa đơn hàng bán trả lại",
        //                //              customer.Code,
        //                //               "Customer",
        //                //              0,
        //                //              Convert.ToDecimal(SalesReturns.RemainAmount),
        //                //              productInvoice.Code,
        //                //              "ProductInvoice",
        //                //              "Đối trừ công nợ",
        //                //              null,
        //                //              null,
        //                //              productInvoiceDetail.Id);

        //                //            tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(SalesReturns.RemainAmount);
        //                //            //Cập nhật lại chi tiết đơn hàng
        //                //            productInvoiceDetail.PaidAmount = productInvoiceDetail.PaidAmount + Convert.ToDecimal(SalesReturns.RemainAmount);
        //                //            productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                //            SalesReturns.PaidAmount = Convert.ToDecimal(SalesReturns.TotalAmount);
        //                //            SalesReturns.RemainAmount = 0;
        //                //            SalesReturnsRepository.UpdateSalesReturns(SalesReturns);
        //                //        }
        //                //    }
        //                //}
        //            }
        //        }
        //    }

        //    //Cập nhật đơn hàng
        //    productInvoice.ModifiedUserId = model.CreatedUserId;
        //    productInvoice.ModifiedDate = DateTime.Now;

        //    //productInvoice.PaidAmount = listproductInvoiceDetail.Sum(x => x.PaidAmount);
        //    productInvoice.RemainingAmount = productInvoice.TotalAmount - productInvoice.PaidAmount;
        //    productInvoice.IsArchive = true;

        //    if (productInvoice.RemainingAmount > 0)
        //    {
        //        productInvoice.Status = "waiting";
        //    }
        //    else
        //    {
        //        productInvoice.Status = "complete";
        //    }
        //    productInvoiceRepository.UpdateProductInvoice(productInvoice);

        //    // cập nhật lại chi tiết đơn hàng
        //    foreach (var item in listproductInvoiceDetail)
        //    {
        //        item.ProductInvoiceId = productInvoice.Id;
        //        productInvoiceRepository.UpdateProductInvoiceDetail(item);
        //    }
        //}
        //#endregion

        #region createReceipt
        Receipt createReceipt(vwCustomer Customer, ProductInvoice productInvoice, string paymentMethod, decimal tongSoTienThanhToan, int? CreatedUserId, ReceiptRepository ReceiptRepository)
        {
            var receipt = new Receipt();
            receipt.IsDeleted = false;
            receipt.CreatedUserId = CreatedUserId;
            receipt.ModifiedUserId = CreatedUserId;
            receipt.AssignedUserId = CreatedUserId;
            receipt.CreatedDate = DateTime.Now;
            receipt.ModifiedDate = DateTime.Now;
            receipt.VoucherDate = DateTime.Now;
            receipt.CustomerId = Customer.Id;
            //receipt.CustomerType = "Customer";
            //receipt.Payer = Customer.Name;
            receipt.PaymentMethod = paymentMethod;
            receipt.Address = Customer.Address;
            receipt.MaChungTuGoc = productInvoice.Code;
            receipt.LoaiChungTuGoc = "ProductInvoice";
            receipt.Name = "Thu tiền khách hàng";
            receipt.Note = receipt.Name;
            //receipt.IsApproved = true;
            //receipt.Amount = Convert.ToDouble(tongSoTienThanhToan);
            //receipt.PaidAmount = Convert.ToDecimal(receipt.Amount);
            //receipt.RemainAmount = 0;
            // receipt.LaCongNo = true;
            ReceiptRepository.InsertReceipt(receipt);

            receipt.Code = Erp.API.Helpers.Common.GetOrderNo("Receipt");
            ReceiptRepository.UpdateReceipt(receipt);
            Erp.API.Helpers.Common.SetOrderNo("Receipt");

            //Thêm vào quản lý chứng từ
            CreateTransaction(new TransactionViewModel
            {
                TransactionModule = "Receipt",
                TransactionCode = receipt.Code,
                TransactionName = "Thu tiền khách hàng"
            }, CreatedUserId);

            //Thêm chứng từ liên quan
            CreateRelationship(new TransactionRelationshipViewModel
            {
                TransactionA = receipt.Code,
                TransactionB = productInvoice.Code
            });

            return receipt;
        }
        #endregion

        #region createReceiptDetail
        void createReceiptDetail(Receipt receipt, ProductInvoice productInvoice, string Name, decimal? Amount, string PaymentMethod, int? ProductInvoiceDetailId, int? PurchaseOrderDetailId, int? CreatedUserId, ReceiptDetailRepository receiptDetailRepository)
        {
            var receiptDetail = new ReceiptDetail();
            receiptDetail.IsDeleted = false;
            receiptDetail.CreatedUserId = CreatedUserId;
            receiptDetail.ModifiedUserId = CreatedUserId;
            receiptDetail.AssignedUserId = CreatedUserId;
            receiptDetail.CreatedDate = DateTime.Now;
            receiptDetail.ModifiedDate = DateTime.Now;

            receiptDetail.Name = Name;
            //receiptDetail.Amount = Convert.ToDouble(Amount);
            receiptDetail.ReceiptId = receipt.Id;
            receiptDetail.MaChungTuGoc = productInvoice.Code;
            receiptDetail.LoaiChungTuGoc = "ProductInvoice";
            //receiptDetail.PurchaseOrderDetailId = PurchaseOrderDetailId;
            //receiptDetail.ProductInvoiceDetailId = ProductInvoiceDetailId;
            //receiptDetail.PaymentMethod = PaymentMethod;
            receiptDetailRepository.InsertReceiptDetail(receiptDetail);
        }
        #endregion

        #region createPayment
        Payment createPayment(vwCustomer Customer, PurchaseOrder purchaseOrder, string paymentMethod, decimal tongSoTienThanhToan, int? CreatedUserId, PaymentRepository paymentRepository)
        {
            //Lập phiếu thu
            var payment = new Payment();
            payment.IsDeleted = false;
            payment.CreatedUserId = CreatedUserId;
            payment.ModifiedUserId = CreatedUserId;
            payment.AssignedUserId = CreatedUserId;
            payment.CreatedDate = DateTime.Now;
            payment.ModifiedDate = DateTime.Now;
            payment.VoucherDate = DateTime.Now;
            payment.TargetId = Customer.Id;
            payment.TargetName = "Customer";
            //payment.Receiver = Customer.Name;
            payment.PaymentMethod = paymentMethod;
            payment.Address = Customer.Address;
            payment.Name = "Trả tiền cho nhà cung cấp";
            payment.Note = payment.Name;
            payment.MaChungTuGoc = purchaseOrder.Code;
            payment.LoaiChungTuGoc = "PurchaseOrder";
            //payment.Amount = Convert.ToDouble(tongSoTienThanhToan);
            //payment.PaidAmount = tongSoTienThanhToan;
            //payment.RemainAmount = 0;
            //payment.LaCongNo = true;
            // payment.IsApproved = true;
            paymentRepository.InsertPayment(payment);


            payment.Code = Erp.API.Helpers.Common.GetOrderNo("Payment");
            paymentRepository.UpdatePayment(payment);
            Erp.API.Helpers.Common.SetOrderNo("Payment");
            //Thêm vào quản lý chứng từ
            CreateTransaction(new TransactionViewModel
            {
                TransactionModule = "Payment",
                TransactionCode = payment.Code,
                TransactionName = "Trả tiền cho nhà cung cấp"
            }, CreatedUserId);


            return payment;
        }
        #endregion
        void createPaymentDetail(Payment payment, string purchaseOrderCode, decimal? Amount, string PaymentMethod, int? ProductInvoiceDetailId, int? PurchaseOrderDetailId, int? CreatedUserId, PaymentDetailRepository paymentDetailRepository)
        {
            var paymentDetail = new PaymentDetail();
            paymentDetail.IsDeleted = false;
            paymentDetail.CreatedUserId = CreatedUserId;
            paymentDetail.ModifiedUserId = CreatedUserId;
            paymentDetail.AssignedUserId = CreatedUserId;
            paymentDetail.CreatedDate = DateTime.Now;
            paymentDetail.ModifiedDate = DateTime.Now;

            paymentDetail.Name = "Trả tiền cho nhà cung cấp";
            // paymentDetail.Amount = Convert.ToDouble(Amount);
            paymentDetail.PaymentId = payment.Id;
            paymentDetail.MaChungTuGoc = purchaseOrderCode;
            paymentDetail.LoaiChungTuGoc = "PurchaseOrder";
            //paymentDetail.PaymentMethod = PaymentMethod;
            //paymentDetail.ProductInvoiceDetailId = ProductInvoiceDetailId;
            //paymentDetail.PurchaseOrderDetailId = PurchaseOrderDetailId;
            paymentDetailRepository.InsertPaymentDetail(paymentDetail);
        }
        #region createPayment
        #endregion


        public static ProductOutbound CreateFromInvoice(ProductOutboundRepository productOutboundRepository,
                           ProductOutboundViewModel model,
                           string productInvoiceCode, int CreatedUserId, int BranchId, string Type = null)
        {
            var productOutbound = new Domain.Sale.Entities.ProductOutbound();
            productOutbound.IsDeleted = false;
            productOutbound.CreatedUserId = CreatedUserId;
            productOutbound.ModifiedUserId = CreatedUserId;
            productOutbound.CreatedDate = DateTime.Now;
            productOutbound.ModifiedDate = DateTime.Now;
            productOutbound.Type = Type == null ? "invoice" : Type;
            productOutbound.BranchId = BranchId;
            productOutbound.TotalAmount = model.DetailList.Sum(x => x.Price * x.Quantity);
            productOutbound.InvoiceId = model.InvoiceId;
            productOutbound.WarehouseSourceId = model.WarehouseSourceId;
            productOutbound.Note = model.Note;
            productOutboundRepository.InsertProductOutbound(productOutbound);

            //Cập nhật lại mã xuất kho
            productOutbound.Code = Erp.API.Helpers.Common.GetOrderNo("ProductOutbound");
            productOutboundRepository.UpdateProductOutbound(productOutbound);

            Erp.API.Helpers.Common.SetOrderNo("ProductOutbound");
            foreach (var item in model.DetailList)
            {
                ProductOutboundDetail productOutboundDetail = new ProductOutboundDetail();
                productOutboundDetail.IsDeleted = false;
                productOutboundDetail.CreatedUserId = CreatedUserId;
                productOutboundDetail.ModifiedUserId = CreatedUserId;
                productOutboundDetail.CreatedDate = DateTime.Now;
                productOutboundDetail.ModifiedDate = DateTime.Now;
                productOutboundDetail.ProductOutboundId = productOutbound.Id;
                // productOutboundDetail.ProductInvoiceDetailId = item.Id;
                productOutboundDetail.ProductId = item.ProductId;
                productOutboundDetail.Quantity = item.Quantity;
                productOutboundDetail.Price = item.Price;
                productOutboundRepository.InsertProductOutboundDetail(productOutboundDetail);
            }

            //Thêm vào quản lý chứng từ
            CreateTransaction(new TransactionViewModel
            {
                TransactionModule = "ProductOutbound",
                TransactionCode = productOutbound.Code,
                TransactionName = Type == null ? "Xuất kho bán hàng" : "Xuất kho trả hàng làm sạch"
            }, CreatedUserId);

            //Thêm chứng từ liên quan
            CreateRelationship(new TransactionRelationshipViewModel
            {
                TransactionA = productOutbound.Code,
                TransactionB = productInvoiceCode
            });

            return productOutbound;
        }
        public static void Archive(ProductOutboundRepository productOutboundRepository,
    WarehouseLocationItemRepository warehouseLocationItemRepository,
    ProductOutbound productOutbound,
     int CreatedUserId)
        {
            //Cập nhật lại tồn kho cho những sp trong phiếu này
            var detailList = productOutboundRepository.GetAllvwProductOutboundDetailByOutboundId(productOutbound.Id)
                .Select(item => new
                {
                    Id = item.Id,
                    ProductName = item.ProductCode + " - " + item.ProductName,
                    ProductId = item.ProductId.Value,
                    Quantity = item.Quantity
                }).ToList();
            // update WarehouseLocationItem
            foreach (var item in detailList)
            {
                var WarehouseLocationItem = warehouseLocationItemRepository.GetAllWarehouseLocationItem()
                    .Where(x => x.ProductId == item.ProductId && x.IsOut == false).Take(item.Quantity.GetValueOrDefault()).ToList();
                foreach (var i in WarehouseLocationItem)
                {
                    i.ProductOutboundId = productOutbound.Id;
                    i.ProductOutboundDetailId = item.Id;
                    warehouseLocationItemRepository.UpdateWarehouseLocationItem(i);
                }
            }
            //Update các lô/date đã xuất = true
            var listWarehouseLocationItem = warehouseLocationItemRepository.GetAllWarehouseLocationItem()
                .Where(item => item.ProductOutboundId == productOutbound.Id).ToList();

            foreach (var item in listWarehouseLocationItem)
            {
                item.IsOut = true;
                item.ModifiedUserId = CreatedUserId;
                item.ModifiedDate = DateTime.Now;
                warehouseLocationItemRepository.UpdateWarehouseLocationItem(item);
            }

            //Kiểm tra dữ liệu sau khi bỏ ghi sổ có hợp lệ không
            string check = "";
            //foreach (var item in detailList)
            //{
            //    var error = PurchaseOrderController.Check(item.ProductName, item.ProductId, productOutbound.WarehouseSourceId.Value, 0, item.Quantity, CreatedUserId);
            //    check += error;
            //}

            //if (string.IsNullOrEmpty(check))
            //{
            //    //Khi đã hợp lệ thì mới update
            //    foreach (var item in detailList)
            //    {
            //        PurchaseOrderController.Update(item.ProductName, item.ProductId, productOutbound.WarehouseSourceId.Value, 0, item.Quantity, CreatedUserId);
            //    }

            //    productOutbound.IsArchive = true;
            //    productOutboundRepository.UpdateProductOutbound(productOutbound);
            //}
        }
        //public void SavePost(int TargetID, string TargetModule, string CommentContent, int CreatedUserId, PostRepository postRepository)
        //{
        //    var post = new Post();
        //    post.IsDeleted = false;
        //    post.CreatedUserId = CreatedUserId;
        //    post.ModifiedUserId = CreatedUserId;
        //    post.AssignedUserId = CreatedUserId;
        //    post.CreatedDate = DateTime.Now;
        //    post.ModifiedDate = DateTime.Now;
        //    post.TargetModule = TargetModule;
        //    post.TargetId = TargetID;
        //    post.Content = CommentContent;
        //    postRepository.InsertPost(post);
        //}
        #endregion
        [HttpGet]
        public HttpResponseMessage GetListCustomer(string name = "", int page = 1, int numberPerPage = 10)
        {
            //if (string.IsNullOrEmpty(name))
            //{
            //    var resp1 = new HttpResponseMessage(HttpStatusCode.OK);
            //    resp1.Content = new StringContent(JsonConvert.SerializeObject(null));
            //    resp1.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            //    return resp1;
            //}
            CustomerRepository customerRepository = new CustomerRepository(new Domain.Account.ErpAccountDbContext());
            var q = customerRepository.GetAllvwCustomer().ToList();
            if (!string.IsNullOrEmpty(name))
            {
                name = Helpers.Common.ChuyenThanhKhongDau(name);
                q = q.Where(x => Helpers.Common.ChuyenThanhKhongDau(x.Code + " - " + x.CompanyName).Contains(name)).ToList();
            }
            var model = q.Select(item => new
            {
                Id = item.Id,
                Code = item.Code,
                Name = item.Code + " - " + item.CompanyName
            }).OrderByDescending(m => m.Name)
            .Skip((page - 1) * numberPerPage)
            .Take(numberPerPage);

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }



        [HttpGet]
        public HttpResponseMessage GetListProductBrand()
        {


           
            return null;
        }






        public HttpResponseMessage GetListProductInventoryAndService(int BranchId, string pCode)
        {

            ProductOrServiceRepository productRepository = new ProductOrServiceRepository(new Domain.Sale.ErpSaleDbContext());
            InventoryRepository inventoryRepository = new InventoryRepository(new Domain.Sale.ErpSaleDbContext());

            var productList = inventoryRepository.GetAllvwInventoryByBranchId(BranchId).Where(x => x.Quantity > 0
            && x.IsSale == true)
             .Select(item => new ProductViewModel
             {
                 Id = item.ProductId,
                 Name = item.ProductName,
                 Type = "product",
                 Code = item.ProductCode,
                 PriceOutbound = item.ProductPriceOutbound,
                 CategoryCode = item.CategoryCode,
                 Unit = item.ProductUnit,
                 Image_Name = item.Image_Name,
                 QuantityTotalInventory = item.Quantity,
                 LoCode = item.LoCode,
                 ExpiryDate = item.ExpiryDate,
                 Origin = item.Origin,
                 Categories = item.Categories
             }).OrderBy(item => item.CategoryCode).ToList();

            if ((pCode != null) && (pCode != ""))
            {
                productList = productList.Where(x => x.Code.ToUpper().Contains(pCode.ToUpper())).ToList();
            }

            var q = productRepository.GetAllvwService()
            .Select(item => new ProductViewModel
            {
                Id = item.Id,
                Name = item.Name,
                Type = item.Type,
                Code = item.Code,
                PriceOutbound = item.PriceOutbound,
                CategoryCode = item.CategoryCode,
                Unit = item.Unit,
                Image_Name = item.Image_Name,
                QuantityTotalInventory = 0,
                LoCode = null,
                ExpiryDate = null,
                Origin = item.Origin,
            })
            .OrderBy(item => item.Name)
            .ToList();

            if ((pCode != null) && (pCode != ""))
            {
                q = q.Where(x => x.Code.ToUpper().Contains(pCode.ToUpper())).ToList();
            }





            var data = productList.Union(q).ToList();
            var json_data = data.Select(item => new
            {
                item.Id,
                Origin = item.Origin,
                Code = item.Code,
                Image = Helpers.Common.KiemTraTonTaiHinhAnh(item.Image_Name, "product-image-folder", "product"),
                Type = item.Type,
                Unit = item.Unit,
                Price = item.PriceOutbound,
                Text = item.Code + " - " + item.Name + " (" + Helpers.Common.PhanCachHangNgan2(item.PriceOutbound) + "/" + item.Unit + ")",
                Name = item.Name,
                Value = item.Id,
                QuantityTotalInventory = item.QuantityTotalInventory,
                LoCode = item.LoCode,
                Categories = (item.Categories == null ? "" : item.Categories),
                ExpiryDate = (item.ExpiryDate == null ? "" : item.ExpiryDate.Value.ToString("dd/MM/yyyy")),
                Note = (item.Type == "product" ? item.Categories + " SL:" + item.QuantityTotalInventory + "  Lô:" + item.LoCode + "  HSD:" + (item.ExpiryDate.HasValue ? item.ExpiryDate.Value.ToString("dd/MM/yyyy") : "") : item.Categories)
            });
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(json_data));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }

        #region GetListProductInvoice
        [HttpGet]
        public HttpResponseMessage GetListProductInvoice(string startDate, string endDate, string txtCode, int? BranchId, int page = 1, int numberPerPage = 10)
        {
            ProductInvoiceRepository productInvoiceRepository = new Domain.Sale.Repositories.ProductInvoiceRepository(new Domain.Sale.ErpSaleDbContext());

            int? pUserId = Int32.Parse(txtCode);

            var q = productInvoiceRepository.GetAllvwProductInvoice().Where(x => x.BranchId == BranchId && x.CreatedUserId == pUserId).ToList();
          
            DateTime d_startDate, d_endDate;
            if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            {
                if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
                {
                    d_endDate = d_endDate.AddHours(23).AddMinutes(59);
                    q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate).ToList();
                }
            }
            var model = q.Select(item => new ProductInvoiceViewModel
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
                CustomerCode = item.CustomerCode,
                CustomerName = item.CustomerName,
                BranchId = item.BranchId,
                CustomerId = item.CustomerId,
                CustomerPhone = item.CustomerPhone,
                IsArchive = item.IsArchive,
                PaidAmount = item.PaidAmount,
                TotalAmount = item.TotalAmount,
                RemainingAmount = item.RemainingAmount,
                TaxFee = item.TaxFee,
                //ProductOutboundCode = item.ProductOutboundCode,
                //ProductOutboundId = item.ProductOutboundId,
                Status = item.Status
            }).OrderByDescending(m => m.CreatedDate)
            .Skip((page - 1) * numberPerPage)
               .Take(numberPerPage);
            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        #region Detail
        [HttpGet]
        public HttpResponseMessage Detail(int Id)
        {
            try
            {


                ProductInvoiceRepository productInvoiceRepository = new Domain.Sale.Repositories.ProductInvoiceRepository(new Domain.Sale.ErpSaleDbContext());

                var model = new ProductInvoiceViewModel();
                var data = productInvoiceRepository.GetvwProductInvoiceById(Id);
                if (data != null && data.IsDeleted != true)
                {
                    AutoMapper.Mapper.CreateMap<vwProductInvoice, ProductInvoiceViewModel>();
                    AutoMapper.Mapper.Map(data, model);
                    model.DetailList = new List<ProductInvoiceDetailViewModel>();
                    model.DetailList = productInvoiceRepository.GetAllvwInvoiceDetailsByInvoiceId(Id).Select(x => new ProductInvoiceDetailViewModel
                    {
                        Id = x.Id,
                        Amount = x.Amount,
                        Price = x.Price,
                        ProductCode = x.ProductCode,
                        ProductId = x.ProductId,
                        ProductName = x.ProductName,
                        ProductType = x.ProductType,
                        CategoryCode = x.CategoryCode,
                        ExpiryDate = x.ExpiryDate,
                        LoCode = x.LoCode,
                        Quantity = x.Quantity,
                        Unit = x.Unit,
                        ProductGroup = x.ProductGroup
                    }).OrderBy(x => x.Id).ToList();
                }

                var resp = new HttpResponseMessage(HttpStatusCode.OK);
                resp.Content = new StringContent(JsonConvert.SerializeObject(model));
                resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
                return resp;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }
        #endregion

    }
}