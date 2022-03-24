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

namespace Erp.API.Controllers
{
    public class PurchaseOrderController : ApiController
    {
        //    #region Create
        //    [HttpPost]
        //    public string Create([FromBody] PurchaseOrderViewModel model)
        //    {
        //        try
        //        {
        //            if (model.DetailList.Count != 0)
        //            {
        //                #region Khởi tạo Repository
        //                PurchaseOrderRepository PurchaseOrderRepository = new PurchaseOrderRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                ProductInboundRepository productInboundRepository = new ProductInboundRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                CategoryRepository categoryRepository = new CategoryRepository(new Erp.Domain.ErpDbContext());
        //                ProductRepository ProductRepository = new ProductRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                SupplierRepository SupplierRepository = new SupplierRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                CustomerRepository CustomerRepository = new CustomerRepository(new Erp.Domain.Account.ErpAccountDbContext());
        //                ProductInvoiceRepository ProductInvoiceRepository = new ProductInvoiceRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                PaymentRepository PaymentRepository = new PaymentRepository(new Erp.Domain.Account.ErpAccountDbContext());
        //                ReceiptRepository ReceiptRepository = new ReceiptRepository(new Erp.Domain.Account.ErpAccountDbContext());
        //                PaymentDetailRepository PaymentDetailRepository = new PaymentDetailRepository(new Erp.Domain.Account.ErpAccountDbContext());
        //                ReceiptDetailRepository ReceiptDetailRepository = new ReceiptDetailRepository(new Erp.Domain.Account.ErpAccountDbContext());
        //                WarehouseLocationItemRepository WarehouseLocationItemRepository = new WarehouseLocationItemRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                WarehouseRepository WarehouseRepository = new WarehouseRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                BOLogRepository BOLogRepository = new BOLogRepository(new Erp.Domain.ErpDbContext());
        //                UserRepository UserRepository = new UserRepository(new Domain.ErpDbContext());
        //                PurchaseReturnsRepository PurchaseReturnsRepository = new PurchaseReturnsRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                SalesDiscountRepository SalesDiscountRepository = new SalesDiscountRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                SalesReturnsRepository SalesReturnsRepository = new SalesReturnsRepository(new Erp.Domain.Sale.ErpSaleDbContext());
        //                #endregion

        //                PurchaseOrder purchaseOrder = null;
        //                List<PurchaseOrderDetail> listPurchaseOrderDetail = new List<PurchaseOrderDetail>();

        //                var WarehouseDestination = WarehouseRepository.GetAllWarehouse().Where(x => x.Code == model.WarehouseDestinationCode).FirstOrDefault();
        //                if (WarehouseDestination != null)
        //                {
        //                    model.WarehouseDestinationId = WarehouseDestination.Id;
        //                }
        //                // lấy BranchId
        //                var User = UserRepository.GetUserById(model.CreatedUserId.Value);
        //                model.BranchId = User.BranchId;

        //                if (model.SupplierId != null)
        //                {
        //                    var Supplier = SupplierRepository.GetSupplierById(model.SupplierId.Value);
        //                    model.SupplierId = Supplier.Id;
        //                    model.SupplierName = Supplier.Name;
        //                    model.SupplierCode = Supplier.Code;
        //                }
        //                else
        //                {
        //                    var Supplier = SupplierRepository.GetSupplierById(model.CustomerId.Value);
        //                    model.SupplierId = Supplier.Id;
        //                    model.SupplierName = Supplier.Name;
        //                    model.SupplierCode = Supplier.Code;
        //                }

        //                //Trường hợp chỉnh sửa
        //                if (purchaseOrder != null)
        //                {
        //                    //Nếu đã ghi sổ rồi thì không được sửa
        //                    if (purchaseOrder.IsArchive.Value)
        //                    {
        //                        return "failed";
        //                    }

        //                    //Kiểm tra xem nếu có xuất kho rồi thì return
        //                    var checkProductInbound = productInboundRepository.GetAllProductInbound()
        //                       .Where(item => item.PurchaseOrderId == purchaseOrder.Id).FirstOrDefault();
        //                    if (checkProductInbound != null)
        //                    {
        //                        return "failed";
        //                    }

        //                    update(ref model, ref purchaseOrder, ref listPurchaseOrderDetail, PurchaseOrderRepository);
        //                }
        //                else //Trường hợp thêm mới
        //                {
        //                    insert(ref model, ref purchaseOrder, ref listPurchaseOrderDetail, PurchaseOrderRepository, categoryRepository, ProductRepository);
        //                }

        //                //Tự động tạo phiếu nhập kho
        //                purchaseOrder.ProductInboundId = autoInbound(purchaseOrder, listPurchaseOrderDetail.Where(item => item.ParentId == null).ToList(), productInboundRepository, WarehouseLocationItemRepository);

        //                #region Ghi sổ
        //                var transactionName = "";
        //                var type = "";
        //                var module = "";
        //                switch (model.WarehouseDestinationCode)
        //                {
        //                    case "ML":
        //                        transactionName = "Hóa đơn mua lại";
        //                        type = "Supplier";
        //                        module = "PurchaseOrder";
        //                        break;
        //                    case "KG":
        //                        transactionName = "Hóa đơn ký gửi";
        //                        type = "Supplier";
        //                        module = "PurchaseOrder";
        //                        break;
        //                    case "LS":
        //                        transactionName = "Hóa đơn làm sạch";
        //                        type = "Customer";
        //                        module = "ServiceOrder";
        //                        break;
        //                }
        //                var supplier = SupplierRepository.GetSupplierById(purchaseOrder.SupplierId.Value);
        //                var Customer = new Customer();
        //                if (model.CustomerId != null)
        //                {
        //                    Customer = CustomerRepository.GetCustomerById(model.CustomerId.Value);
        //                }
        //                if (model.WarehouseDestinationCode != "KG")
        //                {
        //                    //Ghi Nợ TK 131 - Phải trả cho nhà cung cấp(tổng giá thanh toán)
        //                    CreateTransactionLiabilities(
        //                        purchaseOrder.Code,
        //                        module,
        //                        transactionName,
        //                        model.WarehouseDestinationCode == "LS" ? Customer.Code : supplier.Code,
        //                        type,
        //                        purchaseOrder.TotalAmount.Value,
        //                        0,
        //                        purchaseOrder.Code,
        //                        module,
        //                        null,
        //                        null,
        //                        null);
        //                }

        //                //Bắt đầu ghi sổ
        //                if (model.PaymentType == null || model.PaymentType == false)
        //                {
        //                    if (model.WarehouseDestinationCode == "LS")
        //                    {
        //                        archiveLS(model, purchaseOrder, listPurchaseOrderDetail.Where(x => x.ParentId == null).ToList(), SupplierRepository, ProductInvoiceRepository, PurchaseOrderRepository, PaymentRepository, PaymentDetailRepository, ReceiptRepository, ReceiptDetailRepository, CustomerRepository, ProductRepository);
        //                    }
        //                    else
        //                    {
        //                        archive(model, purchaseOrder, listPurchaseOrderDetail.Where(x => x.ParentId == null).ToList(), SupplierRepository, ProductInvoiceRepository, PurchaseOrderRepository, PaymentRepository, PaymentDetailRepository, ReceiptRepository, ReceiptDetailRepository, CustomerRepository, ProductRepository);
        //                    }
        //                }
        //                else
        //                {
        //                    if (model.WarehouseDestinationCode == "LS")
        //                    {
        //                        archivePaymentPendingLS(model, purchaseOrder, listPurchaseOrderDetail.Where(item => item.ParentId == null).ToList(), SupplierRepository, PaymentRepository, PurchaseOrderRepository, PaymentDetailRepository, ReceiptRepository, ReceiptDetailRepository, SalesDiscountRepository, SalesReturnsRepository);
        //                    }
        //                    else
        //                    {
        //                        archivePaymentPending(model, purchaseOrder, listPurchaseOrderDetail.Where(item => item.ParentId == null).ToList(), SupplierRepository, PaymentRepository, PurchaseOrderRepository, PaymentDetailRepository, PurchaseReturnsRepository);
        //                    }
        //                }

        //                #endregion

        //                return "success";
        //            }
        //            return "failed";
        //        }
        //        catch (Exception ex)
        //        {
        //            return ex.Message;
        //        }
        //        // return "success";
        //    }

        //    void insert(ref PurchaseOrderViewModel model, ref PurchaseOrder purchaseOrder, ref List<PurchaseOrderDetail> listPurchaseOrderDetail, PurchaseOrderRepository PurchaseOrderRepository, CategoryRepository categoryRepository, ProductRepository ProductRepository)
        //    {
        //        purchaseOrder = new PurchaseOrder();
        //        purchaseOrder.IsDeleted = false;
        //        purchaseOrder.CreatedUserId = model.CreatedUserId;
        //        purchaseOrder.CreatedDate = DateTime.Now;
        //        purchaseOrder.Status = "inprogress";
        //        purchaseOrder.BranchId = model.BranchId;
        //        purchaseOrder.IsArchive = false;
        //        purchaseOrder.TotalAmount = model.TotalAmount;
        //        purchaseOrder.TaxFee = 0;
        //        purchaseOrder.Discount = 0;
        //        purchaseOrder.SupplierId = model.SupplierId;
        //        purchaseOrder.WarehouseDestinationId = model.WarehouseDestinationId;
        //        purchaseOrder.PaidAmount = 0;
        //        purchaseOrder.RemainingAmount = purchaseOrder.TotalAmount;
        //        purchaseOrder.IsFromApp = false;

        //        PurchaseOrderRepository.InsertPurchaseOrder(purchaseOrder);

        //        //Cập nhật lại mã hóa đơn
        //        purchaseOrder.Code = Erp.API.Helpers.Common.GetOrderNo("PurchaseOrder_" + model.WarehouseDestinationCode);
        //        PurchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);
        //        Erp.API.Helpers.Common.SetOrderNo("PurchaseOrder_" + model.WarehouseDestinationCode);
        //        var transactionName = "";
        //        switch (model.WarehouseDestinationCode)
        //        {
        //            case "ML":
        //                transactionName = "Hóa đơn mua lại";
        //                break;
        //            case "KG":
        //                transactionName = "Hóa đơn ký gửi";
        //                break;
        //            case "LS":
        //                transactionName = "Hóa đơn làm sạch";
        //                break;
        //        }
        //        //Thêm vào quản lý chứng từ
        //        CreateTransaction(new TransactionViewModel
        //        {
        //            TransactionModule = "PurchaseOrder",
        //            TransactionCode = purchaseOrder.Code,
        //            TransactionName = transactionName
        //        });

        //        foreach (var item in model.DetailList)
        //        {
        //            //Thêm sản phẩm
        //            var product = addProduct(model, item, ProductRepository, categoryRepository);

        //            var purchaseOrderDetail = new PurchaseOrderDetail
        //            {
        //                PurchaseOrderId = purchaseOrder.Id,
        //                ProductId = product.Id,
        //                Quantity = item.Quantity,
        //                Unit = product.Unit,
        //                Price = model.WarehouseDestinationCode == "LS" ? 0 : item.Price,
        //                PriceTemp = model.WarehouseDestinationCode == "LS" ? item.Price : 0,
        //                IsDeleted = false,
        //                CreatedUserId = model.CreatedUserId,
        //                ModifiedUserId = model.CreatedUserId,
        //                CreatedDate = DateTime.Now,
        //                ModifiedDate = DateTime.Now,
        //                DisCount = item.DisCount,
        //                DisCountAmount = 0,
        //                Amount = item.Quantity * item.Price,
        //                RemainAmount = item.Quantity * item.Price,
        //                PaidAmount = 0,
        //                QuantityPurchaseReturn = item.Quantity,
        //                IsPurchaseReturn = false,
        //            };

        //            PurchaseOrderRepository.InsertPurchaseOrderDetail(purchaseOrderDetail);

        //            item.Id = purchaseOrderDetail.Id;
        //            listPurchaseOrderDetail.Add(purchaseOrderDetail);

        //            if (item.ServiceList != null && item.ServiceList.Count > 0)
        //            {
        //                foreach (var s in item.ServiceList)
        //                {
        //                    var purchaseOrderSubDetail = new PurchaseOrderDetail
        //                    {
        //                        PurchaseOrderId = purchaseOrder.Id,
        //                        ParentId = purchaseOrderDetail.Id,
        //                        ProductId = s.Id,
        //                        Quantity = 1,
        //                        Unit = s.Unit,
        //                        Price = s.Price,
        //                        IsDeleted = false,
        //                        CreatedUserId = model.CreatedUserId,
        //                        ModifiedUserId = model.CreatedUserId,
        //                        CreatedDate = DateTime.Now,
        //                        ModifiedDate = DateTime.Now,
        //                        DisCount = 0,
        //                        DisCountAmount = 0,
        //                        Amount = s.Price,
        //                        PaidAmount = s.Price,
        //                        RemainAmount = 0,
        //                        PriceLabor = s.PriceLabor
        //                    };
        //                    PurchaseOrderRepository.InsertPurchaseOrderDetail(purchaseOrderSubDetail);
        //                    listPurchaseOrderDetail.Add(purchaseOrderSubDetail);
        //                }
        //            }
        //        }



        //    }

        //    void update(ref PurchaseOrderViewModel model, ref PurchaseOrder purchaseOrder, ref List<PurchaseOrderDetail> listPurchaseOrderDetail, PurchaseOrderRepository PurchaseOrderRepository)
        //    {
        //        listPurchaseOrderDetail = PurchaseOrderRepository.GetAllOrderDetailsByOrderId(purchaseOrder.Id).ToList();

        //        foreach (var item in model.DetailList)
        //        {
        //            var purchaseOrderDetail = listPurchaseOrderDetail.Where(x => x.ProductId == item.ProductId).FirstOrDefault();
        //            if (purchaseOrderDetail != null)
        //            {
        //                purchaseOrderDetail.Price = item.Price;
        //                purchaseOrderDetail.PriceTemp = item.PriceTemp;
        //                purchaseOrderDetail.Quantity = item.Quantity;
        //                purchaseOrderDetail.Amount = item.Amount;
        //                purchaseOrderDetail.PaidAmount = 0;
        //                purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount;

        //                PurchaseOrderRepository.UpdatePurchaseOrderDetail(purchaseOrderDetail);
        //            }
        //        }

        //        purchaseOrder.TotalAmount = model.TotalAmount;
        //        purchaseOrder.PaidAmount = 0;
        //        purchaseOrder.RemainingAmount = model.TotalAmount;
        //        PurchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);
        //    }

        //    Product addProduct(PurchaseOrderViewModel model, PurchaseOrderDetailViewModel item, ProductRepository ProductRepository, CategoryRepository categoryRepository)
        //    {
        //        var product = new Domain.Sale.Entities.Product();
        //        product.IsDeleted = false;
        //        product.CreatedUserId = model.CreatedUserId;
        //        product.ModifiedUserId = model.CreatedUserId;
        //        product.CreatedDate = DateTime.Now;
        //        product.ModifiedDate = DateTime.Now;

        //        var productIndex = Convert.ToInt32(Helpers.Common.GetSetting("orderNo_ProductCode_" + model.WarehouseDestinationCode));
        //        var code = model.WarehouseDestinationCode + item.CategoryCode + getCode(productIndex + 1);

        //        product.Code = code;
        //        product.Size = item.ProductSize;
        //        product.ProductMaterial = item.ProductMaterial;
        //        product.ProductType = item.ProductType;
        //        product.Manufacturer = item.ProductManufacturer;
        //        product.CategoryCode = item.CategoryCode;
        //        product.Type = item.Type;
        //        product.PriceInbound = item.Price;
        //        product.PriceOutbound = 0;
        //        product.SerialNumber = item.SerialNumber;
        //        product.ProductColor = item.ProductColor;
        //        product.SupplierCode = model.SupplierCode;
        //        product.Type = "product";

        //        int i = 0;

        //        foreach (var img in item.ListImageUpload.Where(x => x != null && x != "").ToList())
        //        {
        //            var image_name = UploadImage(img, product.Code + "_" + i);
        //            if (i == 0)
        //            {
        //                product.Image_Name = image_name;
        //            }
        //            product.ImageList += image_name + ";";

        //            i++;
        //        }

        //        var category = categoryRepository.GetCategoryByCode("product")
        //            .Where(x => x.Value == item.CategoryCode).FirstOrDefault();

        //        product.Name = (category != null ? category.Name : "") + " "
        //            + product.Manufacturer + " "
        //            + product.ProductType + " "
        //            + product.ProductColor + " "
        //            + product.ProductMaterial + " "
        //            + product.Size + " "
        //            + product.Code + " "
        //            + item.Quantity + " "
        //            + item.Note
        //            ;

        //        ProductRepository.InsertProduct(product);
        //        Erp.API.Helpers.Common.SetSetting("orderNo_ProductCode_" + model.WarehouseDestinationCode, (productIndex + 1).ToString());
        //        return product;
        //    }

        //    string getCode(int n)
        //    {
        //        string code = n.ToString();
        //        while (code.Length < 4)
        //        {
        //            code = "0" + code;
        //        }

        //        return code;
        //    }
        //    public string UploadImage(string ImageStr, string productCode)
        //    {
        //        try
        //        {
        //            string base64 = ImageStr.Substring(ImageStr.IndexOf(',') + 1);
        //            base64 = base64.Trim('\0');
        //            byte[] bytes = Convert.FromBase64String(base64);
        //            //Save the Byte Array as Image File.
        //            string fname = Helpers.Common.ChuyenThanhKhongDau(DateTime.Now.ToString("yyyyMMddHHmmss") + "_" + productCode.Replace(" ", "") + ".jpg");
        //            string filePath = Path.Combine(Erp.API.Helpers.Common.GetSetting("upload_path_PurchaseOrderAPI"), fname);
        //            System.IO.File.WriteAllBytes(filePath, bytes);
        //            return fname;
        //        }
        //        catch { }

        //        return null;
        //    }

        //    void archive(PurchaseOrderViewModel model
        //        , PurchaseOrder purchaseOrder
        //        , List<PurchaseOrderDetail> listPurchaseOrderDetail
        //        , SupplierRepository SupplierRepository
        //        , ProductInvoiceRepository ProductInvoiceRepository
        //        , PurchaseOrderRepository PurchaseOrderRepository
        //        , PaymentRepository PaymentRepository
        //        , PaymentDetailRepository PaymentDetailRepository
        //        , ReceiptRepository ReceiptRepository
        //        , ReceiptDetailRepository ReceiptDetailRepository
        //        , CustomerRepository CustomerRepository
        //        , ProductRepository ProductRepository)
        //    {            
        //        var supplier = new Supplier();
        //        if (model.SupplierId != null)
        //        {
        //            supplier = SupplierRepository.GetSupplierById(model.SupplierId.Value);
        //        }

        //        decimal tongSoTienThanhToan = model.PaymentList.Where(x => x.Amount > 0).Sum(x => x.Amount);

        //        if (tongSoTienThanhToan > 0)
        //        {
        //            //Thêm thanh toán đối trừ
        //            foreach (var item in model.PaymentList.Where(x => x.Name == "Đối trừ" && x.PurchaseOrderDetailId != null))
        //            {
        //                var PurchaseOrderDetailViewModel = model.DetailList.Where(x => x.OrderNo == item.PurchaseOrderDetailId).FirstOrDefault();
        //                var purchaseOrderDetail = new PurchaseOrderDetail();
        //                if (PurchaseOrderDetailViewModel != null)
        //                {
        //                    purchaseOrderDetail = listPurchaseOrderDetail.Where(x => x.Id == PurchaseOrderDetailViewModel.Id).FirstOrDefault();
        //                    // tạo phiếu thu tiền đối trừ
        //                    //var Receipt = createReceipt(customer, null, "ProductInvoice", item.Name, 0, model.CreatedUserId, ReceiptRepository);
        //                    if (item.SubList != null)
        //                    {
        //                        foreach (var subItem in item.SubList)
        //                        {
        //                            //Mã hàng A
        //                            var ProductA = ProductRepository.GetProductById(purchaseOrderDetail.ProductId.Value);
        //                            //Mã hàng đối trừ
        //                            var productInvoiceDetail = ProductInvoiceRepository.GetProductInvoiceDetailById(subItem.ProductInvoiceDetailId.Value);
        //                            var productInvoice = ProductInvoiceRepository.GetProductInvoiceById(productInvoiceDetail.ProductInvoiceId.Value);
        //                            var ProductB = ProductRepository.GetProductById(productInvoiceDetail.ProductId.Value);
        //                            //Chi tiền đối trừ mã mua lại
        //                            //createPaymentDetail(Payment, purchaseOrder, "Trả tiền cho nhà cung cấp", subItem.Amount, item.Name, purchaseOrderDetail.Id, subItem.ProductInvoiceDetailId, model.CreatedUserId, PaymentDetailRepository);
        //                            CreateTransactionLiabilities(
        //                                "DT-" + ProductA.Code + "-" + ProductB.Code,
        //                                "Exchange",
        //                                "Đối trừ",
        //                                supplier.Code,
        //                                "Supplier",
        //                                0,
        //                                Convert.ToDecimal(subItem.Amount),
        //                                purchaseOrder.Code,
        //                                "PurchaseOrder",
        //                                item.Name,
        //                                null,
        //                                null,
        //                                purchaseOrderDetail.Id,
        //                                subItem.ProductInvoiceDetailId);

        //                            purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + subItem.Amount;
        //                            purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;

        //                            //createReceiptDetail(Receipt, subItem.Amount, item.Name, productInvoice.Code, "ProductInvoice", productInvoiceDetail.Id, purchaseOrderDetail.Id, model.CreatedUserId, ReceiptDetailRepository);
        //                            CreateTransactionLiabilities(
        //                                "DT-" + ProductB.Code + "-" + ProductA.Code,
        //                                "Exchange",
        //                                "Đối trừ",
        //                                supplier.Code,
        //                                "Customer",
        //                                0,
        //                                Convert.ToDecimal(subItem.Amount),
        //                                productInvoice.Code,
        //                                "ProductInvoice",
        //                                item.Name,
        //                                null,
        //                                null,
        //                                productInvoiceDetail.Id,
        //                                purchaseOrderDetail.Id);
        //                            //update Receipt
        //                            //Receipt.Amount += Convert.ToDouble(subItem.Amount);
        //                            //Receipt.PaidAmount = Convert.ToDecimal(Receipt.Amount);
        //                            //Receipt.RemainAmount = 0;
        //                            //Receipt.MaChungTuGoc = productInvoice.Code;
        //                            //ReceiptRepository.UpdateReceipt(Receipt);
        //                            ////Thêm chứng từ liên quan
        //                            //CreateRelationship(new TransactionRelationshipViewModel
        //                            //{
        //                            //    TransactionA = Receipt.Code,
        //                            //    TransactionB = productInvoice.Code
        //                            //});
        //                            //Lấy chi tiết mã bán hàng đã đối trừ update lại
        //                            productInvoiceDetail.PaidAmount += subItem.Amount;
        //                            productInvoiceDetail.RemainAmount = productInvoiceDetail.Amount - productInvoiceDetail.PaidAmount;
        //                            ProductInvoiceRepository.UpdateProductInvoiceDetail(productInvoiceDetail);

        //                            productInvoice.PaidAmount += subItem.Amount;
        //                            productInvoice.RemainingAmount = productInvoice.TotalAmount - productInvoice.PaidAmount;
        //                            ProductInvoiceRepository.UpdateProductInvoice(productInvoice);

        //                        }
        //                    }
        //                }
        //            }

        //            //Thêm thanh toán tiền mặt/chuyển khoản
        //            var danh_sach_ma_hang_con_lai = listPurchaseOrderDetail.Where(x => x.RemainAmount > 0).ToList();
        //            var danh_sach_thanh_toan = model.PaymentList.Where(x => x.Amount > 0 && x.Name != "Đối trừ").ToList();
        //            int i = 0;
        //            int j = 0;
        //            var tongTienThanhToan2 = danh_sach_thanh_toan.Sum(item => item.Amount);
        //            var Payment = new Payment();
        //            if (tongTienThanhToan2 > 0)
        //            {
        //                Payment = createPayment(supplier, purchaseOrder.Code, "", tongTienThanhToan2, model.CreatedUserId, PaymentRepository);
        //            }

        //            while (tongTienThanhToan2 > 0)
        //            {
        //                var purchaseOrderDetail = danh_sach_ma_hang_con_lai[i];
        //                var payment = danh_sach_thanh_toan[j];

        //                if (purchaseOrderDetail.RemainAmount <= payment.Amount)
        //                {

        //                    createPaymentDetail(Payment, purchaseOrder, "Trả tiền cho nhà cung cấp", purchaseOrderDetail.RemainAmount.Value, payment.Name, purchaseOrderDetail.Id, null, model.CreatedUserId, PaymentDetailRepository);
        //                    CreateTransactionLiabilities(
        //                    Payment.Code,
        //                    "Payment",
        //                    "Trả tiền cho nhà cung cấp",
        //                    supplier.Code,
        //                    "Supplier",
        //                    0,
        //                    Convert.ToDecimal(purchaseOrderDetail.RemainAmount),
        //                    purchaseOrder.Code,
        //                    "PurchaseOrder",
        //                    payment.Name,
        //                    null,
        //                    null,
        //                    purchaseOrderDetail.Id);


        //                    payment.Amount = payment.Amount - purchaseOrderDetail.RemainAmount.Value;
        //                    tongTienThanhToan2 = tongTienThanhToan2 - purchaseOrderDetail.RemainAmount.Value;

        //                    //Cập nhật lại chi tiết đơn hàng
        //                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + purchaseOrderDetail.RemainAmount;
        //                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;

        //                    if (payment.Amount == 0)
        //                    {
        //                        j++;
        //                    }

        //                    i++;
        //                }
        //                else
        //                {

        //                    createPaymentDetail(Payment, purchaseOrder, "Trả tiền cho nhà cung cấp", payment.Amount, payment.Name, purchaseOrderDetail.Id, null, model.CreatedUserId, PaymentDetailRepository);
        //                    CreateTransactionLiabilities(
        //                    Payment.Code,
        //                    "Payment",
        //                    "Trả tiền cho nhà cung cấp",
        //                    supplier.Code,
        //                    "Supplier",
        //                    0,
        //                    Convert.ToDecimal(payment.Amount),
        //                    purchaseOrder.Code,
        //                    "PurchaseOrder",
        //                    payment.Name,
        //                    null,
        //                    null,
        //                    purchaseOrderDetail.Id);

        //                    tongTienThanhToan2 = tongTienThanhToan2 - payment.Amount;

        //                    //Cập nhật lại chi tiết đơn hàng
        //                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + payment.Amount;
        //                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;

        //                    if (purchaseOrderDetail.RemainAmount == 0)
        //                    {
        //                        i++;
        //                    }
        //                    j++;
        //                }
        //            }
        //            //}
        //        }

        //        //Cập nhật đơn hàng
        //        purchaseOrder.ModifiedUserId = model.CreatedUserId;
        //        purchaseOrder.ModifiedDate = DateTime.Now;
        //        purchaseOrder.PaidAmount += tongSoTienThanhToan;
        //        purchaseOrder.RemainingAmount = purchaseOrder.TotalAmount - purchaseOrder.PaidAmount;
        //        if (model.WarehouseDestinationCode != "KG")
        //        {
        //            purchaseOrder.IsArchive = true;
        //        }
        //        else
        //        {
        //            purchaseOrder.IsArchive = false;
        //        }
        //        if (purchaseOrder.RemainingAmount > 0)
        //        {
        //            purchaseOrder.Status = "waiting";
        //        }
        //        else
        //        {
        //            purchaseOrder.Status = "complete";
        //        }

        //        PurchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);

        //        //Cập nhật lại thanh toán chi tiết đơn hàng
        //        foreach (var item in listPurchaseOrderDetail)
        //        {
        //            PurchaseOrderRepository.UpdatePurchaseOrderDetail(item);
        //        }
        //    }

        //    void archiveLS(PurchaseOrderViewModel model
        //        , PurchaseOrder purchaseOrder
        //        , List<PurchaseOrderDetail> listPurchaseOrderDetail
        //        , SupplierRepository SupplierRepository
        //        , ProductInvoiceRepository ProductInvoiceRepository
        //        , PurchaseOrderRepository PurchaseOrderRepository
        //        , PaymentRepository PaymentRepository
        //        , PaymentDetailRepository PaymentDetailRepository
        //        , ReceiptRepository ReceiptRepository
        //        , ReceiptDetailRepository ReceiptDetailRepository
        //        , CustomerRepository CustomerRepository
        //        , ProductRepository ProductRepository)
        //    {

        //        var customer = new vwCustomer();
        //        if (model.SupplierId != null)
        //        {
        //            customer = CustomerRepository.GetvwCustomerById(model.SupplierId.Value);
        //        }

        //        decimal tongSoTienThanhToan = model.PaymentList.Where(x => x.Amount > 0).Sum(x => x.Amount);

        //        if (tongSoTienThanhToan > 0)
        //        {
        //            //Thêm thanh toán đối trừ
        //            foreach (var item in model.PaymentList.Where(x => x.Name == "Đối trừ" && x.PurchaseOrderDetailId != null))
        //            {
        //                var PurchaseOrderDetailViewModel = model.DetailList.Where(x => x.OrderNo == item.PurchaseOrderDetailId).FirstOrDefault();
        //                var purchaseOrderDetail = new PurchaseOrderDetail();
        //                if (PurchaseOrderDetailViewModel != null)
        //                {
        //                    purchaseOrderDetail = listPurchaseOrderDetail.Where(x => x.Id == PurchaseOrderDetailViewModel.Id).FirstOrDefault();
        //                    //var Payment = createPayment(supplier, null, item.Name, item.Amount, model.CreatedUserId, PaymentRepository);

        //                    if (item.SubList != null)
        //                    {
        //                        foreach (var subItem in item.SubList)
        //                        {
        //                            if (model.WarehouseDestinationCode == "LS")
        //                            {
        //                                //Mã hàng A
        //                                var ProductA = ProductRepository.GetProductById(purchaseOrderDetail.ProductId.Value);

        //                                // Mã hàng B
        //                                var PurchaseOrderDetail = PurchaseOrderRepository.GetPurchaseOrderDetailById(subItem.ProductInvoiceDetailId.Value);
        //                                var PurchaseOrder = PurchaseOrderRepository.GetPurchaseOrderById(PurchaseOrderDetail.PurchaseOrderId.Value);
        //                                var ProductB = ProductRepository.GetProductById(PurchaseOrderDetail.ProductId.Value);
        //                                //tạo phiếu thu
        //                                //createReceiptDetail(Receipt, subItem.Amount, item.Name, purchaseOrder.Code, "ServiceOrder", subItem.ProductInvoiceDetailId.Value, purchaseOrderDetail.Id, model.CreatedUserId, ReceiptDetailRepository);
        //                                CreateTransactionLiabilities(
        //                                     "DT-" + ProductA.Code + "-" + ProductB.Code,
        //                                    "Exchange",
        //                                    "Đối trừ",
        //                                    customer.Code,
        //                                    "Customer",
        //                                    0,
        //                                    Convert.ToDecimal(subItem.Amount),
        //                                    purchaseOrder.Code,
        //                                    "ServiceOrder",
        //                                    item.Name,
        //                                    null,
        //                                    null,
        //                                    purchaseOrderDetail.Id,
        //                                     subItem.ProductInvoiceDetailId.Value);
        //                                purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + subItem.Amount;
        //                                purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;


        //                                //createPaymentDetail(Payment, PurchaseOrder, "Trả tiền cho nhà cung cấp", subItem.Amount, item.Name, PurchaseOrderDetail.Id, purchaseOrderDetail.Id, model.CreatedUserId, PaymentDetailRepository);
        //                                CreateTransactionLiabilities(
        //                                   "DT-" + ProductB.Code + "-" + ProductA.Code,
        //                                    "Exchange",
        //                                    "Đối trừ",
        //                                    customer.Code,
        //                                     "Supplier",
        //                                    0,
        //                                    Convert.ToDecimal(subItem.Amount),
        //                                    PurchaseOrder.Code,
        //                                    "ServiceOrder",
        //                                    item.Name,
        //                                    null,
        //                                    null,
        //                                     PurchaseOrderDetail.Id,
        //                                      purchaseOrderDetail.Id);
        //                                //update Payment
        //                                //Payment.MaChungTuGoc = PurchaseOrder.Code;
        //                                //PaymentRepository.UpdatePayment(Payment);
        //                                ////Thêm chứng từ liên quan
        //                                ////Thêm chứng từ liên quan
        //                                //CreateRelationship(new TransactionRelationshipViewModel
        //                                //{
        //                                //    TransactionA = Payment.Code,
        //                                //    TransactionB = PurchaseOrder.Code
        //                                //});
        //                                // lấy chi tết PurchaseOrder update lại

        //                                PurchaseOrderDetail.PaidAmount += subItem.Amount;
        //                                PurchaseOrderDetail.RemainAmount = PurchaseOrderDetail.Amount - PurchaseOrderDetail.PaidAmount;
        //                                PurchaseOrderRepository.UpdatePurchaseOrderDetail(PurchaseOrderDetail);
        //                                // lấy PurchaseOrder update lại

        //                                PurchaseOrder.PaidAmount += subItem.Amount;
        //                                PurchaseOrder.RemainingAmount = PurchaseOrder.TotalAmount - PurchaseOrder.PaidAmount;
        //                                PurchaseOrderRepository.UpdatePurchaseOrder(PurchaseOrder);

        //                            }
        //                        }
        //                    }
        //                }
        //            }

        //            //Thêm thanh toán tiền mặt/chuyển khoản
        //            var danh_sach_ma_hang_con_lai = listPurchaseOrderDetail.Where(x => x.RemainAmount > 0).ToList();
        //            var danh_sach_thanh_toan = model.PaymentList.Where(x => x.Amount > 0 && x.Name != "Đối trừ").ToList();
        //            int i = 0;
        //            int j = 0;
        //            var tongTienThanhToan2 = danh_sach_thanh_toan.Sum(item => item.Amount);
        //            var Receipt = new Receipt();
        //            if (tongTienThanhToan2 > 0)
        //            {
        //                Receipt = createReceipt(customer, purchaseOrder.Code, "ServiceOrder", null, tongTienThanhToan2, model.CreatedUserId, ReceiptRepository);
        //            }

        //            while (tongTienThanhToan2 > 0)
        //            {
        //                var purchaseOrderDetail = danh_sach_ma_hang_con_lai[i];
        //                var payment = danh_sach_thanh_toan[j];

        //                if (purchaseOrderDetail.RemainAmount <= payment.Amount)
        //                {
        //                    if (model.WarehouseDestinationCode == "LS")
        //                    {
        //                        createReceiptDetail(Receipt, purchaseOrderDetail.RemainAmount.Value, payment.Name, "Thu tiền khách hàng", purchaseOrder.Code, "ServiceOrder", null, purchaseOrderDetail.Id, model.CreatedUserId, ReceiptDetailRepository);

        //                        CreateTransactionLiabilities(
        //                            Receipt.Code,
        //                            "Receipt",
        //                            "Thu tiền khách hàng",
        //                             customer.Code,
        //                            "Customer",
        //                            0,
        //                            Convert.ToDecimal(purchaseOrderDetail.RemainAmount),
        //                            purchaseOrder.Code,
        //                            "ServiceOrder",
        //                            payment.Name,
        //                            null,
        //                            null,
        //                            purchaseOrderDetail.Id);
        //                    }

        //                    payment.Amount = payment.Amount - purchaseOrderDetail.RemainAmount.Value;
        //                    tongTienThanhToan2 = tongTienThanhToan2 - purchaseOrderDetail.RemainAmount.Value;

        //                    //Cập nhật lại chi tiết đơn hàng
        //                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + purchaseOrderDetail.RemainAmount;
        //                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;

        //                    if (payment.Amount == 0)
        //                    {
        //                        j++;
        //                    }

        //                    i++;
        //                }
        //                else
        //                {
        //                    if (model.WarehouseDestinationCode == "LS")
        //                    {
        //                        createReceiptDetail(Receipt, payment.Amount, payment.Name, "Thu tiền khách hàng", purchaseOrder.Code, "ServiceOrder", null, purchaseOrderDetail.Id, model.CreatedUserId, ReceiptDetailRepository);

        //                        CreateTransactionLiabilities(
        //                            Receipt.Code,
        //                            "Receipt",
        //                            "Thu tiền khách hàng",
        //                            customer.Code,
        //                            "Customer",
        //                            0,
        //                            Convert.ToDecimal(payment.Amount),
        //                            purchaseOrder.Code,
        //                            "ServiceOrder",
        //                            payment.Name,
        //                            null,
        //                            null,
        //                            purchaseOrderDetail.Id);
        //                    }

        //                    tongTienThanhToan2 = tongTienThanhToan2 - payment.Amount;

        //                    //Cập nhật lại chi tiết đơn hàng
        //                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + payment.Amount;
        //                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;

        //                    if (purchaseOrderDetail.RemainAmount == 0)
        //                    {
        //                        i++;
        //                    }
        //                    j++;
        //                }
        //            }
        //            //}
        //        }

        //        //Cập nhật đơn hàng
        //        //purchaseOrder.ModifiedUserId = WebSecurity.CurrentUserId;
        //        purchaseOrder.ModifiedDate = DateTime.Now;
        //        purchaseOrder.IsArchive = true;
        //        if (purchaseOrder.RemainingAmount > 0)
        //        {
        //            purchaseOrder.Status = "waiting";
        //        }
        //        else
        //        {
        //            purchaseOrder.Status = "complete";
        //        }

        //        purchaseOrder.PaidAmount += tongSoTienThanhToan;
        //        purchaseOrder.RemainingAmount = purchaseOrder.TotalAmount - purchaseOrder.PaidAmount;
        //        PurchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);

        //        //Cập nhật lại thanh toán chi tiết đơn hàng
        //        foreach (var item in listPurchaseOrderDetail)
        //        {
        //            PurchaseOrderRepository.UpdatePurchaseOrderDetail(item);
        //        }
        //    }

        //    void archivePaymentPending(PurchaseOrderViewModel model, PurchaseOrder purchaseOrder, List<PurchaseOrderDetail> listPurchaseOrderDetail, SupplierRepository SupplierRepository, PaymentRepository paymentRepository, PurchaseOrderRepository PurchaseOrderRepository, PaymentDetailRepository paymentDetailRepository, PurchaseReturnsRepository PurchaseReturnsRepository)
        //    {
        //        if (model.PaymentPending != null && model.PaymentPending.Count() > 0)
        //        {
        //            //Bắt đầu ghi sổ
        //            var supplier = SupplierRepository.GetvwSupplierById(model.SupplierId.Value);
        //            var module = "";
        //            var type = "";
        //            switch (model.WarehouseDestinationCode)
        //            {
        //                case "ML":
        //                case "KG":
        //                    module = "PurchaseOrder";
        //                    type = "Supplier";
        //                    break;
        //                case "LS":
        //                    module = "ServiceOrder";
        //                    type = "Customer";
        //                    break;
        //            }
        //            decimal tongSoTienThanhToan = model.PayTotalAmount.Value;
        //            var danh_sach_ma_hang = listPurchaseOrderDetail.Where(x => x.RemainAmount > 0).ToList();
        //            int i = 0;
        //            foreach (var item in model.PaymentPending)
        //            {
        //                if (item.IsCheck == true)
        //                {
        //                    var purchaseOrderDetail = danh_sach_ma_hang[i];
        //                    if (item.LoaiCT == "PaymentPending")
        //                    {
        //                        var payment = paymentRepository.GetPaymentById(item.Id);
        //                        if (tongSoTienThanhToan > 0)
        //                        {
        //                            while (tongSoTienThanhToan > 0 && payment.RemainAmount > 0)
        //                            {

        //                                if (purchaseOrderDetail.RemainAmount <= payment.RemainAmount)
        //                                {
        //                                    // tạo chi tiết cho phiếu thu
        //                                    createPaymentDetail(payment, purchaseOrder, "Đối trừ công nợ", purchaseOrderDetail.RemainAmount, "Đối trừ công nợ", purchaseOrderDetail.Id, null, model.CreatedUserId, paymentDetailRepository);
        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = payment.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });

        //                                    CreateTransactionLiabilities(
        //                                       payment.Code,
        //                                      "Payment",
        //                                      "Trả tiền cho nhà cung cấp",
        //                                      supplier.Code,
        //                                      type,
        //                                      0,
        //                                      Convert.ToDecimal(purchaseOrderDetail.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);
        //                                    payment.RemainAmount = payment.RemainAmount - purchaseOrderDetail.RemainAmount.Value;
        //                                    payment.PaidAmount += purchaseOrderDetail.RemainAmount.Value;
        //                                    paymentRepository.UpdatePayment(payment);
        //                                    tongSoTienThanhToan = tongSoTienThanhToan - purchaseOrderDetail.RemainAmount.Value;

        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + purchaseOrderDetail.RemainAmount;
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    i++;
        //                                }
        //                                else
        //                                {
        //                                    // tạo chi tiết cho phiếu thu
        //                                    createPaymentDetail(payment, purchaseOrder, "Đối trừ công nợ", payment.RemainAmount, "Đối trừ công nợ", purchaseOrderDetail.Id, null, model.CreatedUserId, paymentDetailRepository);
        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = payment.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                      payment.Code,
        //                                      "Payment",
        //                                      "Trả tiền cho nhà cung cấp",
        //                                      supplier.Code,
        //                                      type,
        //                                      0,
        //                                      Convert.ToDecimal(payment.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                     purchaseOrderDetail.Id);

        //                                    tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(payment.RemainAmount);
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + Convert.ToDecimal(payment.RemainAmount);
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    payment.PaidAmount = Convert.ToDecimal(payment.Amount);
        //                                    payment.RemainAmount = 0;
        //                                    paymentRepository.UpdatePayment(payment);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (item.LoaiCT == "PurchaseReturns")
        //                    {
        //                        var PurchaseReturns = PurchaseReturnsRepository.GetPurchaseReturnsById(item.Id);
        //                        if (tongSoTienThanhToan > 0)
        //                        {
        //                            while (tongSoTienThanhToan > 0 && PurchaseReturns.RemainAmount > 0)
        //                            {
        //                                if (purchaseOrderDetail.RemainAmount <= PurchaseReturns.RemainAmount)
        //                                {
        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = PurchaseReturns.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                      PurchaseReturns.Code,
        //                                      "PurchaseReturns",
        //                                      "Hóa đơn trả hàng nhà cung cấp",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(purchaseOrderDetail.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);
        //                                    PurchaseReturns.RemainAmount = PurchaseReturns.RemainAmount - purchaseOrderDetail.RemainAmount.Value;
        //                                    PurchaseReturns.PaidAmount += purchaseOrderDetail.RemainAmount.Value;
        //                                    PurchaseReturnsRepository.UpdatePurchaseReturns(PurchaseReturns);
        //                                    tongSoTienThanhToan = tongSoTienThanhToan - purchaseOrderDetail.RemainAmount.Value;
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + purchaseOrderDetail.RemainAmount;
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    i++;
        //                                }
        //                                else
        //                                {

        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = PurchaseReturns.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                       PurchaseReturns.Code,
        //                                      "PurchaseReturns",
        //                                      "Hóa đơn trả hàng nhà cung cấp",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(PurchaseReturns.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);

        //                                    tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(PurchaseReturns.RemainAmount);
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + Convert.ToDecimal(PurchaseReturns.RemainAmount);
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    PurchaseReturns.PaidAmount = Convert.ToDecimal(PurchaseReturns.TotalAmount);
        //                                    PurchaseReturns.RemainAmount = 0;
        //                                    PurchaseReturnsRepository.UpdatePurchaseReturns(PurchaseReturns);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }

        //        //Cập nhật đơn hàng
        //        purchaseOrder.ModifiedUserId = model.CreatedUserId;
        //        purchaseOrder.ModifiedDate = DateTime.Now;

        //        purchaseOrder.PaidAmount = listPurchaseOrderDetail.Sum(x => x.PaidAmount);
        //        purchaseOrder.RemainingAmount = purchaseOrder.TotalAmount - purchaseOrder.PaidAmount;
        //        purchaseOrder.IsArchive = true;

        //        if (purchaseOrder.RemainingAmount > 0)
        //        {
        //            purchaseOrder.Status = "waiting";
        //        }
        //        else
        //        {
        //            purchaseOrder.Status = "complete";
        //        }
        //        PurchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);

        //        // cập nhật lại chi tiết đơn hàng
        //        foreach (var item in listPurchaseOrderDetail)
        //        {
        //            item.PurchaseOrderId = purchaseOrder.Id;
        //            PurchaseOrderRepository.UpdatePurchaseOrderDetail(item);
        //        }
        //    }
        //    void archivePaymentPendingLS(PurchaseOrderViewModel model, PurchaseOrder purchaseOrder, List<PurchaseOrderDetail> listPurchaseOrderDetail, SupplierRepository SupplierRepository, PaymentRepository paymentRepository, PurchaseOrderRepository PurchaseOrderRepository, PaymentDetailRepository paymentDetailRepository, ReceiptRepository ReceiptRepository, ReceiptDetailRepository ReceiptDetailRepository, SalesDiscountRepository SalesDiscountRepository, SalesReturnsRepository SalesReturnsRepository)
        //    {
        //        if (model.PaymentPending != null && model.PaymentPending.Count() > 0)
        //        {
        //            //Bắt đầu ghi sổ
        //            var supplier = SupplierRepository.GetvwSupplierById(model.CustomerId.Value);
        //            var module = "";
        //            var type = "";
        //            switch (model.WarehouseDestinationCode)
        //            {
        //                case "ML":
        //                case "KG":
        //                    module = "PurchaseOrder";
        //                    type = "Supplier";
        //                    break;
        //                case "LS":
        //                    module = "ServiceOrder";
        //                    type = "Customer";
        //                    break;
        //            }
        //            decimal tongSoTienThanhToan = model.PayTotalAmount.Value;
        //            var danh_sach_ma_hang = listPurchaseOrderDetail.Where(x => x.RemainAmount > 0).ToList();
        //            int i = 0;
        //            foreach (var item in model.PaymentPending)
        //            {
        //                if (item.IsCheck == true)
        //                {
        //                    var purchaseOrderDetail = danh_sach_ma_hang[i];
        //                    if (item.LoaiCT == "ReceiptPending")
        //                    {
        //                        var receipt = ReceiptRepository.GetReceiptById(item.Id);
        //                        if (tongSoTienThanhToan > 0)
        //                        {
        //                            while (tongSoTienThanhToan > 0 && receipt.RemainAmount > 0)
        //                            {

        //                                if (purchaseOrderDetail.RemainAmount <= receipt.RemainAmount)
        //                                {
        //                                    // tạo chi tiết cho phiếu thu
        //                                    createReceiptDetail(receipt, purchaseOrderDetail.RemainAmount, "Đối trừ công nợ", "Đối trừ công nợ", purchaseOrder.Code, module, purchaseOrderDetail.Id, null, model.CreatedUserId, ReceiptDetailRepository);
        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = receipt.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                      receipt.Code,
        //                                      "Receipt",
        //                                      "Thu tiền khách hàng",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(purchaseOrderDetail.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);
        //                                    receipt.RemainAmount = receipt.RemainAmount - purchaseOrderDetail.RemainAmount.Value;
        //                                    receipt.PaidAmount += purchaseOrderDetail.RemainAmount.Value;
        //                                    ReceiptRepository.UpdateReceipt(receipt);
        //                                    tongSoTienThanhToan = tongSoTienThanhToan - purchaseOrderDetail.RemainAmount.Value;
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + purchaseOrderDetail.RemainAmount;
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    i++;
        //                                }
        //                                else
        //                                {
        //                                    // tạo chi tiết cho phiếu thu
        //                                    createReceiptDetail(receipt, receipt.RemainAmount, "Đối trừ công nợ", "Đối trừ công nợ", purchaseOrder.Code, module, purchaseOrderDetail.Id, null, model.CreatedUserId, ReceiptDetailRepository);
        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = receipt.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                      receipt.Code,
        //                                      "Receipt",
        //                                      "Thu tiền khách hàng",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(receipt.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);

        //                                    tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(receipt.RemainAmount);
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + Convert.ToDecimal(receipt.RemainAmount);
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    receipt.PaidAmount = Convert.ToDecimal(receipt.Amount);
        //                                    receipt.RemainAmount = 0;
        //                                    ReceiptRepository.UpdateReceipt(receipt);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (item.LoaiCT == "SalesDiscount")
        //                    {
        //                        var SalesDiscount = SalesDiscountRepository.GetSalesDiscountById(item.Id);
        //                        if (tongSoTienThanhToan > 0)
        //                        {
        //                            while (tongSoTienThanhToan > 0 && SalesDiscount.RemainAmount > 0)
        //                            {
        //                                if (purchaseOrderDetail.RemainAmount <= SalesDiscount.RemainAmount)
        //                                {
        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = SalesDiscount.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                      SalesDiscount.Code,
        //                                      "SalesDiscount",
        //                                      "Hóa đơn giảm giá hàng bán",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(purchaseOrderDetail.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);
        //                                    SalesDiscount.RemainAmount = SalesDiscount.RemainAmount - purchaseOrderDetail.RemainAmount.Value;
        //                                    SalesDiscount.PaidAmount += purchaseOrderDetail.RemainAmount.Value;
        //                                    SalesDiscountRepository.UpdateSalesDiscount(SalesDiscount);
        //                                    tongSoTienThanhToan = tongSoTienThanhToan - purchaseOrderDetail.RemainAmount.Value;
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + purchaseOrderDetail.RemainAmount;
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    i++;
        //                                }
        //                                else
        //                                {

        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = SalesDiscount.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                     SalesDiscount.Code,
        //                                      "SalesDiscount",
        //                                      "Hóa đơn giảm giá hàng bán",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(SalesDiscount.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);

        //                                    tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(SalesDiscount.RemainAmount);
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + Convert.ToDecimal(SalesDiscount.RemainAmount);
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    SalesDiscount.PaidAmount = Convert.ToDecimal(SalesDiscount.TotalAmount);
        //                                    SalesDiscount.RemainAmount = 0;
        //                                    SalesDiscountRepository.UpdateSalesDiscount(SalesDiscount);
        //                                }
        //                            }
        //                        }
        //                    }
        //                    else if (item.LoaiCT == "SalesReturns")
        //                    {
        //                        var SalesReturns = SalesReturnsRepository.GetSalesReturnsById(item.Id);
        //                        if (tongSoTienThanhToan > 0)
        //                        {
        //                            while (tongSoTienThanhToan > 0 && SalesReturns.RemainAmount > 0)
        //                            {
        //                                if (purchaseOrderDetail.RemainAmount <= SalesReturns.RemainAmount)
        //                                {
        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = SalesReturns.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                      SalesReturns.Code,
        //                                      "SalesReturns",
        //                                      "Hóa đơn hàng bán trả lại",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(purchaseOrderDetail.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);
        //                                    SalesReturns.RemainAmount = SalesReturns.RemainAmount - purchaseOrderDetail.RemainAmount.Value;
        //                                    SalesReturns.PaidAmount += purchaseOrderDetail.RemainAmount.Value;
        //                                    SalesReturnsRepository.UpdateSalesReturns(SalesReturns);
        //                                    tongSoTienThanhToan = tongSoTienThanhToan - purchaseOrderDetail.RemainAmount.Value;
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + purchaseOrderDetail.RemainAmount;
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    i++;
        //                                }
        //                                else
        //                                {

        //                                    //Thêm chứng từ liên quan
        //                                    CreateRelationship(new TransactionRelationshipViewModel
        //                                    {
        //                                        TransactionA = SalesReturns.Code,
        //                                        TransactionB = purchaseOrder.Code
        //                                    });
        //                                    CreateTransactionLiabilities(
        //                                      SalesReturns.Code,
        //                                      "SalesReturns",
        //                                      "Hóa đơn hàng bán trả lại",
        //                                      supplier.Code,
        //                                       type,
        //                                      0,
        //                                      Convert.ToDecimal(SalesReturns.RemainAmount),
        //                                      purchaseOrder.Code,
        //                                      module,
        //                                      "Đối trừ công nợ",
        //                                      null,
        //                                      null,
        //                                      purchaseOrderDetail.Id);

        //                                    tongSoTienThanhToan = tongSoTienThanhToan - Convert.ToDecimal(SalesReturns.RemainAmount);
        //                                    //Cập nhật lại chi tiết đơn hàng
        //                                    purchaseOrderDetail.PaidAmount = purchaseOrderDetail.PaidAmount + Convert.ToDecimal(SalesReturns.RemainAmount);
        //                                    purchaseOrderDetail.RemainAmount = purchaseOrderDetail.Amount - purchaseOrderDetail.PaidAmount;
        //                                    SalesReturns.PaidAmount = Convert.ToDecimal(SalesReturns.TotalAmount);
        //                                    SalesReturns.RemainAmount = 0;
        //                                    SalesReturnsRepository.UpdateSalesReturns(SalesReturns);
        //                                }
        //                            }
        //                        }
        //                    }
        //                }
        //            }
        //        }
        //        //Cập nhật đơn hàng
        //        purchaseOrder.ModifiedUserId = model.CreatedUserId;
        //        purchaseOrder.ModifiedDate = DateTime.Now;

        //        purchaseOrder.PaidAmount = listPurchaseOrderDetail.Sum(x => x.PaidAmount);
        //        purchaseOrder.RemainingAmount = purchaseOrder.TotalAmount - purchaseOrder.PaidAmount;
        //        purchaseOrder.IsArchive = true;

        //        if (purchaseOrder.RemainingAmount > 0)
        //        {
        //            purchaseOrder.Status = "waiting";
        //        }
        //        else
        //        {
        //            purchaseOrder.Status = "complete";
        //        }
        //        PurchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);

        //        // cập nhật lại chi tiết đơn hàng
        //        foreach (var item in listPurchaseOrderDetail)
        //        {
        //            item.PurchaseOrderId = purchaseOrder.Id;
        //            PurchaseOrderRepository.UpdatePurchaseOrderDetail(item);
        //        }
        //    }
        //    Payment createPayment(Supplier supplier, string purchaseOrderCode, string paymentMethod, decimal tongSoTienThanhToan, int? CreatedUserId, PaymentRepository paymentRepository)
        //    {
        //        //Lập phiếu thu
        //        var payment = new Payment();
        //        payment.IsDeleted = false;
        //        payment.CreatedUserId = CreatedUserId;
        //        payment.ModifiedUserId = CreatedUserId;
        //        payment.AssignedUserId = CreatedUserId;
        //        payment.CreatedDate = DateTime.Now;
        //        payment.ModifiedDate = DateTime.Now;
        //        payment.VoucherDate = DateTime.Now;
        //        payment.TargetId = supplier.Id;
        //        payment.TargetName = "Supplier";
        //        payment.Receiver = supplier.CompanyName;
        //        payment.PaymentMethod = paymentMethod;
        //        payment.Address = supplier.Address;
        //        payment.Name = "Trả tiền cho nhà cung cấp";
        //        payment.Note = payment.Name;
        //        payment.MaChungTuGoc = purchaseOrderCode;
        //        payment.LoaiChungTuGoc = "PurchaseOrder";
        //        payment.LaCongNo = true;
        //        payment.IsApproved = true;
        //        payment.Amount = Convert.ToDouble(tongSoTienThanhToan);
        //        payment.PaidAmount = Convert.ToDecimal(payment.Amount);
        //        payment.RemainAmount = 0;
        //        paymentRepository.InsertPayment(payment);


        //        payment.Code = Erp.API.Helpers.Common.GetOrderNo("Payment");
        //        paymentRepository.UpdatePayment(payment);
        //        Erp.API.Helpers.Common.SetOrderNo("Payment");
        //        //Thêm vào quản lý chứng từ
        //        CreateTransaction(new TransactionViewModel
        //        {
        //            TransactionModule = "Payment",
        //            TransactionCode = payment.Code,
        //            TransactionName = "Trả tiền cho nhà cung cấp"
        //        });

        //        if (purchaseOrderCode != null)
        //        {
        //            //Thêm chứng từ liên quan
        //            CreateRelationship(new TransactionRelationshipViewModel
        //            {
        //                TransactionA = payment.Code,
        //                TransactionB = purchaseOrderCode
        //            });
        //        }
        //        return payment;
        //    }
        //    void createPaymentDetail(Payment payment, PurchaseOrder purchaseOrder, string Name, decimal? Amount, string PaymentMethod, int? ProductInvoiceDetailId, int? PurchaseOrderDetailId, int? CreatedUserId, PaymentDetailRepository paymentDetailRepository)
        //    {
        //        var paymentDetail = new PaymentDetail();
        //        paymentDetail.IsDeleted = false;
        //        paymentDetail.CreatedUserId = CreatedUserId;
        //        paymentDetail.ModifiedUserId = CreatedUserId;
        //        paymentDetail.AssignedUserId = CreatedUserId;
        //        paymentDetail.CreatedDate = DateTime.Now;
        //        paymentDetail.ModifiedDate = DateTime.Now;
        //        paymentDetail.Name = Name;
        //        paymentDetail.Amount = Convert.ToDouble(Amount);
        //        paymentDetail.PaymentId = payment.Id;
        //        paymentDetail.MaChungTuGoc = purchaseOrder.Code;
        //        paymentDetail.LoaiChungTuGoc = "PurchaseOrder";
        //        paymentDetail.PaymentMethod = PaymentMethod;
        //        paymentDetail.PurchaseOrderDetailId = PurchaseOrderDetailId;
        //        paymentDetail.ProductInvoiceDetailId = ProductInvoiceDetailId;
        //        paymentDetailRepository.InsertPaymentDetail(paymentDetail);
        //    }
        //    Receipt createReceipt(vwCustomer Customer, string MaChungTuGoc, string LoaiChungTuGoc, string paymentMethod, decimal tongSoTienThanhToan, int? CreatedUserId, ReceiptRepository ReceiptRepository)
        //    {
        //        var receipt = new Receipt();
        //        receipt.IsDeleted = false;
        //        receipt.CreatedUserId = CreatedUserId;
        //        receipt.ModifiedUserId = CreatedUserId;
        //        receipt.AssignedUserId = CreatedUserId;
        //        receipt.CreatedDate = DateTime.Now;
        //        receipt.ModifiedDate = DateTime.Now;
        //        receipt.VoucherDate = DateTime.Now;
        //        receipt.CustomerId = Customer.Id;
        //        receipt.CustomerType = "Customer";
        //        receipt.IsApproved = true;
        //        receipt.Payer = Customer.Name;
        //        receipt.PaymentMethod = paymentMethod;
        //        receipt.Address = Customer.Address;
        //        receipt.Name = "Thu tiền khách hàng";
        //        receipt.Note = receipt.Name;
        //        receipt.Amount = Convert.ToDouble(tongSoTienThanhToan);
        //        receipt.PaidAmount = Convert.ToDecimal(receipt.Amount);
        //        receipt.RemainAmount = 0;
        //        receipt.LaCongNo = true;
        //        receipt.MaChungTuGoc = MaChungTuGoc;
        //        receipt.LoaiChungTuGoc = LoaiChungTuGoc;
        //        ReceiptRepository.InsertReceipt(receipt);

        //        receipt.Code = Erp.API.Helpers.Common.GetOrderNo("Receipt");
        //        ReceiptRepository.UpdateReceipt(receipt);
        //        Erp.API.Helpers.Common.SetOrderNo("Receipt");

        //        //Thêm vào quản lý chứng từ
        //        CreateTransaction(new TransactionViewModel
        //        {
        //            TransactionModule = "Receipt",
        //            TransactionCode = receipt.Code,
        //            TransactionName = "Thu tiền khách hàng"
        //        });
        //        if (MaChungTuGoc != null)
        //        {
        //            //Thêm chứng từ liên quan
        //            CreateRelationship(new TransactionRelationshipViewModel
        //            {
        //                TransactionA = receipt.Code,
        //                TransactionB = MaChungTuGoc
        //            });
        //        }
        //        return receipt;
        //    }
        //    void createReceiptDetail(Receipt receipt, decimal? Amount, string PaymentMethod, string Name, string MaChungTuGoc, string LoaiChungTuGoc, int? ProductInvoiceDetailId, int? PurchaseOrderDetailId, int? CreatedUserId, ReceiptDetailRepository receiptDetailRepository)
        //    {
        //        var receiptDetail = new ReceiptDetail();
        //        receiptDetail.IsDeleted = false;
        //        receiptDetail.CreatedUserId = CreatedUserId;
        //        receiptDetail.ModifiedUserId = CreatedUserId;
        //        receiptDetail.AssignedUserId = CreatedUserId;
        //        receiptDetail.CreatedDate = DateTime.Now;
        //        receiptDetail.ModifiedDate = DateTime.Now;

        //        receiptDetail.Name = Name;
        //        receiptDetail.Amount = Convert.ToDouble(Amount);
        //        receiptDetail.ReceiptId = receipt.Id;
        //        receiptDetail.MaChungTuGoc = MaChungTuGoc;
        //        receiptDetail.LoaiChungTuGoc = LoaiChungTuGoc;
        //        receiptDetail.ProductInvoiceDetailId = ProductInvoiceDetailId;
        //        receiptDetail.PurchaseOrderDetailId = PurchaseOrderDetailId;
        //        receiptDetail.PaymentMethod = PaymentMethod;
        //        receiptDetailRepository.InsertReceiptDetail(receiptDetail);
        //    }
        //    int autoInbound(PurchaseOrder purchaseOrder, List<PurchaseOrderDetail> listPurchaseOrderDetail, ProductInboundRepository productInboundRepository, WarehouseLocationItemRepository warehouseLocationItemRepository)
        //    {
        //        var productInbound = new Domain.Sale.Entities.ProductInbound();
        //        productInbound.IsDeleted = false;
        //        productInbound.CreatedUserId = purchaseOrder.CreatedUserId;
        //        productInbound.ModifiedUserId = purchaseOrder.CreatedUserId;
        //        productInbound.CreatedDate = DateTime.Now;
        //        productInbound.ModifiedDate = DateTime.Now;
        //        productInbound.BranchId = purchaseOrder.BranchId;
        //        productInbound.WarehouseDestinationId = purchaseOrder.WarehouseDestinationId;
        //        productInbound.VAT = 0;
        //        productInbound.TotalVAT = 0;
        //        productInbound.Total = 0;
        //        productInbound.SupplierId = purchaseOrder.SupplierId;
        //        productInbound.TotalAmount = listPurchaseOrderDetail.Sum(x => x.Price * x.Quantity).Value;
        //        productInbound.Type = "external";
        //        productInbound.PurchaseOrderId = purchaseOrder.Id;

        //        //thêm mới phiếu nhập và chi tiết phiếu nhập
        //        productInboundRepository.InsertProductInbound(productInbound);

        //        //cập nhật lại mã nhập kho
        //        productInbound.Code = Erp.API.Helpers.Common.GetOrderNo("ProductInbound");
        //        productInboundRepository.UpdateProductInbound(productInbound);
        //        Erp.API.Helpers.Common.SetOrderNo("ProductInbound");

        //        //Thêm chi tiết phiếu nhập
        //        foreach (var item in listPurchaseOrderDetail)
        //        {
        //            ProductInboundDetail productInboundDetail = new Domain.Sale.Entities.ProductInboundDetail();
        //            productInboundDetail.IsDeleted = false;
        //            productInboundDetail.CreatedUserId = purchaseOrder.CreatedUserId;
        //            productInboundDetail.ModifiedUserId = purchaseOrder.CreatedUserId;
        //            productInboundDetail.CreatedDate = DateTime.Now;
        //            productInboundDetail.ModifiedDate = DateTime.Now;
        //            productInboundDetail.ProductId = item.ProductId.Value;
        //            productInboundDetail.Quantity = item.Quantity.Value;
        //            productInboundDetail.Price = item.Price.Value;
        //            productInboundDetail.ProductInboundId = productInbound.Id;
        //            productInboundRepository.InsertProductInboundDetail(productInboundDetail);

        //            //cập nhật vị trí sản phẩm thêm vào kho cho từng sản phẩm riêng biệt
        //            for (int i = 1; i <= productInboundDetail.Quantity; i++)
        //            {
        //                var warehouseLocationItem = new WarehouseLocationItem();
        //                warehouseLocationItem.IsDeleted = false;
        //                warehouseLocationItem.CreatedUserId = purchaseOrder.CreatedUserId;
        //                warehouseLocationItem.ModifiedUserId = purchaseOrder.CreatedUserId;
        //                warehouseLocationItem.CreatedDate = DateTime.Now;
        //                warehouseLocationItem.ModifiedDate = DateTime.Now;
        //                warehouseLocationItem.ProductId = productInboundDetail.ProductId;
        //                warehouseLocationItem.ProductInboundId = productInbound.Id;
        //                warehouseLocationItem.ProductInboundDetailId = productInboundDetail.Id;
        //                warehouseLocationItem.IsOut = false;
        //                warehouseLocationItem.Shelf = "1";
        //                warehouseLocationItem.Floor = "1";
        //                warehouseLocationItem.WarehouseId = productInbound.WarehouseDestinationId;
        //                warehouseLocationItemRepository.InsertWarehouseLocationItem(warehouseLocationItem);
        //            }
        //        }

        //        //Thêm vào quản lý chứng từ
        //        CreateTransaction(new TransactionViewModel
        //         {
        //             TransactionModule = "ProductInbound",
        //             TransactionCode = productInbound.Code,
        //             TransactionName = "Nhập kho"
        //         });

        //        //Thêm chứng từ liên quan
        //        CreateRelationship(new TransactionRelationshipViewModel
        //            {
        //                TransactionA = purchaseOrder.Code,
        //                TransactionB = productInbound.Code
        //            });

        //        //Tự động nhập kho

        //        //Cập nhật lại tồn kho cho những sp trong phiếu nhập này
        //        var detailList = productInboundRepository.GetAllvwProductInboundDetailByInboundId(productInbound.Id)
        //            .Select(item => new
        //            {
        //                ProductName = item.ProductCode + " - " + item.ProductName,
        //                ProductId = item.ProductId,
        //                Quantity = item.Quantity
        //            }).ToList();

        //        //Kiểm tra dữ liệu sau khi bỏ ghi sổ có hợp lệ không
        //        string check = "";
        //        foreach (var item in detailList)
        //        {
        //            var error = Check(item.ProductName, item.ProductId, productInbound.WarehouseDestinationId.Value, item.Quantity, 0, purchaseOrder.CreatedUserId);
        //            check += error;
        //        }

        //        if (string.IsNullOrEmpty(check))
        //        {
        //            //Khi đã hợp lệ thì mới update
        //            foreach (var item in detailList)
        //            {
        //                Update(item.ProductName, item.ProductId, productInbound.WarehouseDestinationId.Value, item.Quantity, 0, purchaseOrder.CreatedUserId);
        //            }

        //            productInbound.IsArchive = true;
        //            productInboundRepository.UpdateProductInbound(productInbound);
        //        }

        //        return productInbound.Id;
        //    }

        //    #region SoSanhChinhSua
        //    public int InsertBOLog(string Name, string Action, string Controller, string Area, int Id, BOLogRepository BOLogRepositoty)
        //    {
        //        BOLog BOLog = new BOLog();
        //        //BOLog.UserId = Erp.API.Helpers.Common.CurrentUser.Id;
        //        //BOLog.UserName = Erp.API.Helpers.Common.CurrentUser.UserName;
        //        BOLog.Action = Action;
        //        BOLog.Note = Name;
        //        BOLog.CreatedDate = DateTime.Now;
        //        BOLog.Controller = Controller;
        //        BOLog.Area = Area;
        //        BOLog.TargetId = Id;
        //        BOLogRepositoty.SaveBOLog(BOLog);
        //        return BOLog.Id;
        //    }
        //    public void SoSanhBOLogDetail(int BOLogId, Object PurchaseOrderBefore, Object PurchaseOrderAfter, BOLogRepository BOLogRepositoty)
        //    {
        //        Type OrderBefore = PurchaseOrderBefore.GetType();
        //        PropertyInfo[] propsOrderBefore = OrderBefore.GetProperties();
        //        Type OrderAfter = PurchaseOrderAfter.GetType();
        //        PropertyInfo[] propsOrderAfter = OrderAfter.GetProperties();
        //        for (int i = 0; i < propsOrderBefore.Count(); i++)
        //        {
        //            if (propsOrderBefore[i].Name == propsOrderAfter[i].Name)
        //            {
        //                var Before = propsOrderBefore[i].GetValue(PurchaseOrderBefore);
        //                var After = propsOrderAfter[i].GetValue(PurchaseOrderAfter);
        //                string BeforeStr = "";
        //                string AfterStr = "";
        //                if (Before != null)
        //                {
        //                    BeforeStr = Before.ToString();
        //                }
        //                if (After != null)
        //                {
        //                    AfterStr = After.ToString();
        //                }
        //                if (BeforeStr.Contains(AfterStr) == false && BeforeStr != "" && AfterStr != "")
        //                {
        //                    BOLogDetail BOLogDetail = new BOLogDetail();
        //                    BOLogDetail.IsDeleted = false;
        //                    //BOLogDetail.CreatedUserId = WebSecurity.CurrentUserId;
        //                    BOLogDetail.CreatedDate = DateTime.Now;
        //                    BOLogDetail.BOLogId = BOLogId;
        //                    BOLogDetail.Name = propsOrderBefore[i].Name;
        //                    BOLogDetail.DataBefore = Before.ToString();
        //                    BOLogDetail.DataAfter = After.ToString();
        //                    BOLogRepositoty.InsertBOLogDetail(BOLogDetail);
        //                }
        //            }
        //        }
        //    }

        //    #endregion

        //    #region Check or Update
        //    public static string Check(string productName, int productId, int warehouseId, int quantityIn, int quantityOut, int? CreatedUserId)
        //    {
        //        return Update(productName, productId, warehouseId, quantityIn, quantityOut, CreatedUserId, false);
        //    }

        //    public static string Update(string productName, int productId, int warehouseId, int quantityIn, int quantityOut, int? CreatedUserId, bool isArchive = true)
        //    {
        //        string error = "";
        //        ProductInboundRepository productInboundRepository = new ProductInboundRepository(new Domain.Sale.ErpSaleDbContext());
        //        ProductOutboundRepository productOutboundRepository = new Domain.Sale.Repositories.ProductOutboundRepository(new Domain.Sale.ErpSaleDbContext());
        //        InventoryRepository inventoryRepository = new Domain.Sale.Repositories.InventoryRepository(new Domain.Sale.ErpSaleDbContext());

        //        var inbound = productInboundRepository.GetAllvwProductInboundDetail()
        //                .Where(x => x.IsArchive && x.ProductId == productId && x.WarehouseDestinationId == warehouseId);

        //        var outbound = productOutboundRepository.GetAllvwProductOutboundDetail()
        //            .Where(x => x.IsArchive && x.ProductId == productId && x.WarehouseSourceId == warehouseId);

        //        var inventory = (inbound.Count() == 0 ? 0 : inbound.Sum(x => x.Quantity)) - (outbound.Count() == 0 ? 0 : outbound.Sum(x => x.Quantity)) + quantityIn - quantityOut;

        //        var inventoryCurrent = inventoryRepository.GetInventoryByWarehouseIdAndProductId(warehouseId, productId);

        //        //Sau khi thay đổi dữ liệu phải đảm bảo tồn kho >= 0
        //        if (true)//inventory >= 0)
        //        {
        //            if (isArchive)
        //            {
        //                if (inventoryCurrent != null)
        //                {
        //                    if (inventoryCurrent.Quantity != inventory)
        //                    {
        //                        inventoryCurrent.Quantity = inventory;
        //                        inventoryRepository.UpdateInventory(inventoryCurrent);
        //                    }
        //                }
        //                else
        //                {
        //                    inventoryCurrent = new Inventory();
        //                    inventoryCurrent.IsDeleted = false;
        //                    inventoryCurrent.CreatedUserId = CreatedUserId;
        //                    inventoryCurrent.ModifiedUserId = CreatedUserId;
        //                    inventoryCurrent.CreatedDate = DateTime.Now;
        //                    inventoryCurrent.ModifiedDate = DateTime.Now;
        //                    inventoryCurrent.WarehouseId = warehouseId;
        //                    inventoryCurrent.ProductId = productId;
        //                    inventoryCurrent.Quantity = inventory;
        //                    inventoryRepository.InsertInventory(inventoryCurrent);
        //                }
        //            }
        //        }
        //        else
        //        {
        //            error += string.Format("<br/>Dữ liệu tồn kho của sản phẩm \"{0}\" là {1}: không hợp lệ!", productName, inventory);
        //        }

        //        return error;
        //    }
        //    #endregion

        //    #region CreateTransaction
        //    public static void CreateTransaction(TransactionViewModel model)
        //    {
        //        TransactionRepository transactionRepository = new Domain.Account.Repositories.TransactionRepository(new Domain.Account.ErpAccountDbContext());
        //        var transaction = transactionRepository.GetAllTransaction()
        //            .Where(item => item.TransactionCode == model.TransactionCode).FirstOrDefault();

        //        if (transaction == null)
        //        {
        //            transaction = new Transaction();

        //            transaction.IsDeleted = false;
        //            //transaction.CreatedUserId = WebSecurity.CurrentUserId;
        //            //transaction.ModifiedUserId = WebSecurity.CurrentUserId;
        //            //transaction.AssignedUserId = WebSecurity.CurrentUserId;
        //            transaction.CreatedDate = DateTime.Now;
        //            transaction.ModifiedDate = DateTime.Now;
        //            transaction.TransactionModule = model.TransactionModule;
        //            transaction.TransactionCode = model.TransactionCode;
        //            transaction.TransactionName = model.TransactionName;
        //            transactionRepository.InsertTransaction(transaction);
        //        }
        //        else
        //        {
        //            //transaction.ModifiedUserId = WebSecurity.CurrentUserId;
        //            transaction.ModifiedDate = DateTime.Now;
        //            transaction.TransactionName = model.TransactionName;
        //            transactionRepository.UpdateTransaction(transaction);
        //        }
        //    }
        //    #endregion

        //    #region CreateRelationship
        //    public static void CreateRelationship(TransactionRelationshipViewModel model)
        //    {
        //        TransactionRepository transactionRelationshipRepository = new Domain.Account.Repositories.TransactionRepository(new Domain.Account.ErpAccountDbContext());
        //        var transactionRelationship = new TransactionRelationship();
        //        transactionRelationship.IsDeleted = false;
        //        //transactionRelationship.CreatedUserId = WebSecurity.CurrentUserId;
        //        //transactionRelationship.ModifiedUserId = WebSecurity.CurrentUserId;
        //        //transactionRelationship.AssignedUserId = WebSecurity.CurrentUserId;
        //        transactionRelationship.CreatedDate = DateTime.Now;
        //        transactionRelationship.ModifiedDate = DateTime.Now;
        //        transactionRelationship.TransactionA = model.TransactionA;
        //        transactionRelationship.TransactionB = model.TransactionB;
        //        transactionRelationshipRepository.InsertTransactionRelationship(transactionRelationship);
        //    }
        //    #endregion

        //    #region CreateTransactionLiabilities
        //    public static TransactionLiabilities CreateTransactionLiabilities(string TransactionCode, string TransactionModule, string TransactionName, string TargetCode, string TargetModule, decimal Debit, decimal Credit, string MaChungTuGoc, string LoaiChungTuGoc, string PaymentMethod, DateTime? NextPaymentDate, string Note, int? MaChungTuGoc_DetailId = null, int? MaChungTuGoc_DetailId2 = null)
        //    {
        //        TransactionLiabilitiesRepository transactionRepository = new TransactionLiabilitiesRepository(new Domain.Account.ErpAccountDbContext());

        //        var transaction = new TransactionLiabilities();
        //        transaction.IsDeleted = false;
        //        //transaction.CreatedUserId = WebSecurity.CurrentUserId;
        //        //transaction.ModifiedUserId = WebSecurity.CurrentUserId;
        //        //transaction.AssignedUserId = WebSecurity.CurrentUserId;
        //        transaction.CreatedDate = DateTime.Now;
        //        transaction.ModifiedDate = DateTime.Now;

        //        transaction.TransactionCode = TransactionCode;
        //        transaction.TransactionModule = TransactionModule;
        //        transaction.TransactionName = TransactionName;
        //        transaction.TargetCode = TargetCode;
        //        transaction.TargetModule = TargetModule;
        //        transaction.Debit = Debit;
        //        transaction.Credit = Credit;
        //        transaction.PaymentMethod = PaymentMethod;
        //        transaction.NextPaymentDate = NextPaymentDate;
        //        transaction.MaChungTuGoc = MaChungTuGoc;
        //        transaction.LoaiChungTuGoc = LoaiChungTuGoc;
        //        transaction.Note = Note;
        //        transaction.MaChungTuGoc_DetailId = MaChungTuGoc_DetailId;
        //        transaction.MaChungTuGoc_DetailId2 = MaChungTuGoc_DetailId2;
        //        transactionRepository.InsertTransaction(transaction);
        //        return transaction;
        //    }
        //    #endregion
        //    #endregion

        //    #region GetListProductInvoiceDetailBySupplierId
        //    public HttpResponseMessage GetListProductInvoiceDetailBySupplierId(int supplierId)
        //    {
        //        SupplierRepository supplierRepository = new SupplierRepository(new Domain.Sale.ErpSaleDbContext());
        //        var supplier = supplierRepository.GetvwSupplierById(supplierId);
        //        ProductInvoiceRepository productInvoiceRepository = new ProductInvoiceRepository(new Domain.Sale.ErpSaleDbContext());
        //        var model = productInvoiceRepository.GetAllvwInvoiceDetails()
        //            .Where(item => item.CustomerId == supplier.Id && item.RemainAmount > 0)
        //            .Select(item => new
        //            {
        //                item.ProductInvoiceCode,
        //                item.ProductInvoiceDate,
        //                item.Id,
        //                item.ProductName,
        //                item.Amount,
        //                item.PaidAmount,
        //                item.RemainAmount,
        //            })
        //            .OrderByDescending(item => item.ProductInvoiceDate)
        //            .ToList()
        //            .Select(item => new
        //            {
        //                item.ProductInvoiceCode,
        //                ProductInvoiceDate = item.ProductInvoiceDate.ToString("dd/MM/yyyy HH:mm"),
        //                item.Id,
        //                item.ProductName,
        //                item.Amount,
        //                item.PaidAmount,
        //                item.RemainAmount,
        //            });

        //        var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //        resp.Content = new StringContent(JsonConvert.SerializeObject(model));
        //        resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //        return resp;
        //    }
        //    #endregion

        #region GetList
        [HttpGet]
        public HttpResponseMessage GetList(string txtCode)
        {
            PurchaseOrderRepository purchaseOrderRepository = new Domain.Sale.Repositories.PurchaseOrderRepository(new Domain.Sale.ErpSaleDbContext());
            var q = purchaseOrderRepository.GetAllvwPurchaseOrder().Where(x => x.SupplierId != null);

            if (!string.IsNullOrEmpty(txtCode))
            {
                txtCode = txtCode.Trim();
                q = q.Where(x => x.Code == txtCode);
            }

            //if (!string.IsNullOrEmpty(txtSupplierCode))
            //{
            //    txtSupplierCode = txtSupplierCode.Trim();
            //    q = q.Where(x => x.SupplierCode == txtSupplierCode);
            //}

            //if (warehouseDestinationId != null)
            //{
            //    q = q.Where(x => x.WarehouseDestinationId == warehouseDestinationId);
            //}

            ////Tìm những phiếu nhập có chứa mã sp
            //if (!string.IsNullOrEmpty(txtProductCode))
            //{
            //    var list = purchaseOrderRepository.GetvwAllOrderDetailsByOrder_all()
            //        .Where(item => item.ProductCode == txtProductCode)
            //        .Select(item => item.PurchaseOrderId.Value)
            //        .Distinct()
            //        .ToList();

            //    if (list.Count > 0)
            //    {
            //        q = q.Where(item => list.Contains(item.Id));
            //    }
            //}

            ////Lọc theo ngày
            //DateTime d_startDate, d_endDate;
            //if (DateTime.TryParseExact(startDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_startDate))
            //{
            //    if (DateTime.TryParseExact(endDate, "dd/MM/yyyy", new CultureInfo("vi-VN"), DateTimeStyles.None, out d_endDate))
            //    {
            //        d_endDate = d_endDate.AddHours(23).AddMinutes(59);
            //        q = q.Where(x => x.CreatedDate >= d_startDate && x.CreatedDate <= d_endDate);
            //    }
            //}

            var model = q.Select(item => new PurchaseOrderViewModel
                {
                    Id = item.Id,
                    CreatedUserId = item.CreatedUserId,
                    CreatedDate = item.CreatedDate,
                    ModifiedUserId = item.ModifiedUserId,
                    ModifiedDate = item.ModifiedDate,
                    Code = item.Code,
                    TotalAmount = item.TotalAmount,
                    Discount = item.Discount,
                    TaxFee = item.TaxFee,
                    Status = item.Status,
                    SupplierName = item.SupplierCode + " - " + item.SupplierName,
                    WarehouseDestinationName = item.WarehouseDestinationName,
                    WarehouseSourceName = item.WarehouseSourceName,
                    ProductInboundId = item.ProductInboundId,
                    ProductInboundCode = item.ProductInboundCode,
                    CancelReason = item.CancelReason,
                    BarCode = item.BarCode,
                    IsArchive = item.IsArchive.Value,
                    Note = item.Note,
                    SupplierId = item.SupplierId,
                    WarehouseDestinationId = item.WarehouseDestinationId,
                    IsDeleted = item.IsDeleted
                }).OrderByDescending(m => m.CreatedDate)
                .ToList();

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
            PurchaseOrderRepository purchaseOrderRepository = new Domain.Sale.Repositories.PurchaseOrderRepository(new Domain.Sale.ErpSaleDbContext());

            var purchaseOrder = new vwPurchaseOrder();
            if (Id > 0)
            {
                purchaseOrder = purchaseOrderRepository.GetvwPurchaseOrderById(Id);
            }

            var model = new PurchaseOrderViewModel();
            model.Code = purchaseOrder.Code;
            model.CreatedDate = purchaseOrder.CreatedDate;
            model.SupplierCode = purchaseOrder.SupplierCode;
            model.SupplierName = purchaseOrder.SupplierName;
            model.TotalAmount = purchaseOrder.TotalAmount;
            model.PaidAmount = purchaseOrder.PaidAmount;
            model.RemainingAmount = purchaseOrder.RemainingAmount;
            model.Note = purchaseOrder.Note;
            //model.WarehouseDestinationCode = purchaseOrder.WarehouseDestinationCode;

            //Lấy thông tin kiểm tra cho phép sửa chứng từ này hay không
            model.AllowEdit = Helpers.Common.KiemTraNgaySuaChungTu(model.CreatedDate.Value);

            ////Lấy lịch sử giao dịch thanh toán
            //var listTransaction = transactionLiabilitiesRepository.GetAllvwLichSuThanhToan()
            //            .Where(item => item.MaChungTuGoc == purchaseOrder.Code && item.Credit > 0)
            //    //.OrderByDescending(item => item.CreatedDate)
            //            .ToList();

            //model.ListTransactionLiabilities = new List<TransactionLiabilitiesViewModel>();
            //AutoMapper.Mapper.Map(listTransaction, model.ListTransactionLiabilities);

            //Lấy danh sách chi tiết đơn hàng
            model.DetailList = purchaseOrderRepository.GetvwAllOrderDetailsByOrderId(purchaseOrder.Id).Select(x =>
                new PurchaseOrderDetailViewModel
                {
                    Id = x.Id,
                    Price = x.Price,
                    ProductId = x.ProductId,
                    Quantity = x.Quantity,
                    Unit = x.Unit,
                    DisCount = x.DisCount,
                    DisCountAmount = x.DisCountAmount,
                    CategoryCode = x.CategoryCode,
                    ProductName = x.ProductName,
                    ProductCode = x.ProductCode,
                    PurchaseOrderId = x.PurchaseOrderId,
                    Amount = x.Amount,
                    IsArchive = x.IsArchive,
                    Description = x.Description,
                    ProductManufacturer = x.Manufacturer,
                    ProductGroup = x.ProductGroup,
                    PurchaseOrderCode = x.PurchaseOrderCode,
                    PurchaseOrderDate = x.PurchaseOrderDate,
                    SupplierId = x.SupplierId,
                    SupplierName = x.SupplierName,
                    //ParentId = x.ParentId,
                    //PriceTemp = x.PriceTemp,
                    //SerialNumber = x.SerialNumber,
                    //PaidAmount = x.PaidAmount,
                    //RemainAmount = x.RemainAmount,
                    //AssignedUserName = x.AssignedUserName,
                    //AssignedUserId = x.AssignedUserId,
                    //Image = x.ProductImage,
                    //IsPurchaseReturn = x.IsPurchaseReturn,
                    //ProductImageList = x.ProductImageList
                }).ToList();

            ////Lấy danh sách bàn giao
            //foreach (var item in model.DetailList)
            //{
            //    item.ServiceList = model.DetailList.Where(x => x.ParentId == item.Id).ToList();
            //    var listAssigned = assignedRepository.GetAllvwAssigned()
            //        .Where(x => x.PurchaseOrderDetailId == item.Id).ToList();

            //    item.ListAssigned = new List<AssignedViewModel>();
            //    AutoMapper.Mapper.Map(listAssigned, item.ListAssigned);

            //    if (item.ProductImageList != null)
            //    {
            //        item.ListImageUpload = item.ProductImageList.Split(new string[] { ";" }, StringSplitOptions.None).Where(x => x != "").ToList();
            //    }
            //}

            model.GroupProduct = model.DetailList.GroupBy(x => new { x.CategoryCode }, (key, group) => new PurchaseOrderDetailViewModel
            {
                CategoryCode = key.CategoryCode,
                ProductId = group.FirstOrDefault().ProductId,
                Id = group.FirstOrDefault().Id
            }).OrderBy(item => item.CategoryCode).ToList();

            ////Lấy thông tin phiếu nhập kho
            //if (purchaseOrder.ProductInboundId != null)
            //{
            //    var ProductInbound = productInboundRepository.GetvwProductInboundById(purchaseOrder.ProductInboundId.Value);
            //    model.ProductInboundViewModel = new ProductInboundViewModel();
            //    AutoMapper.Mapper.Map(ProductInbound, model.ProductInboundViewModel);
            //}

            ////Lấy danh sách chứng từ liên quan
            //model.ListTransactionRelationship = new List<TransactionRelationshipViewModel>();
            //var listTransactionRelationship = transactionRepository.GetAllvwTransactionRelationship()
            //    .Where(item => item.TransactionA == purchaseOrder.Code
            //    || item.TransactionB == purchaseOrder.Code).OrderByDescending(item => item.CreatedDate)
            //    .ToList();

            //AutoMapper.Mapper.Map(listTransactionRelationship, model.ListTransactionRelationship);

            ////int taxfee = 0;
            ////int.TryParse(Helpers.Common.GetSetting("vat"), out taxfee);
            ////model.TaxFee = taxfee;

            //ViewBag.isAdmin = Erp.BackOffice.Helpers.Common.CurrentUser.UserTypeId == 1 ? true : false;
            //if (model.ModifiedUserId != null)
            //{
            //    var ModifiedUserName = userRepository.GetUserById(model.ModifiedUserId.Value);
            //    if (ModifiedUserName != null)
            //    {
            //        model.ModifiedUserName = ModifiedUserName.FullName;
            //    }
            //}

            if (!string.IsNullOrEmpty(model.ImageList))
            {
                string path = Helpers.Common.GetSetting("upload_path_PurchaseOrder");
                model.ListImg = model.ImageList.Split(new[] { ";" }, StringSplitOptions.None)
                    .Where(item => !string.IsNullOrEmpty(item))
                    .Select(item => path + "/" + item)
                    .ToList();
            }

            var resp = new HttpResponseMessage(HttpStatusCode.OK);
            resp.Content = new StringContent(JsonConvert.SerializeObject(model));
            resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            return resp;
        }
        #endregion

        //#region UpdatePrice
        //[HttpPost]
        //public decimal UpdatePrice([FromBody] PurchaseOrderDetailViewModel model)
        //{
        //    PurchaseOrderRepository purchaseOrderRepository = new PurchaseOrderRepository(new Erp.Domain.Sale.ErpSaleDbContext());

        //    var purchaseOrderDetail = purchaseOrderRepository.GetPurchaseOrderDetailById(model.Id);
        //    purchaseOrderDetail.Price = model.Price;
        //    purchaseOrderDetail.Amount = purchaseOrderDetail.Quantity * purchaseOrderDetail.Price;

        //    purchaseOrderRepository.UpdatePurchaseOrderDetail(purchaseOrderDetail);

        //    var purchaseOrder = purchaseOrderRepository.GetPurchaseOrderById(purchaseOrderDetail.PurchaseOrderId.Value);

        //    purchaseOrder.ModifiedDate = DateTime.Now;
        //    purchaseOrder.ModifiedUserId = model.ModifiedUserId;

        //    var amount = purchaseOrderRepository.GetvwAllOrderDetailsByOrderId(purchaseOrder.Id).Sum(item => item.Amount);
        //    purchaseOrder.TotalAmount = amount;
        //    purchaseOrder.PaidAmount = 0;
        //    purchaseOrder.RemainingAmount = amount;

        //    purchaseOrderRepository.UpdatePurchaseOrder(purchaseOrder);

        //    return amount;
        //}
        //#endregion

        //#region GetListWarehouse
        //public HttpResponseMessage GetListWarehouse()
        //{
        //    WarehouseRepository WarehouseRepository = new Domain.Sale.Repositories.WarehouseRepository(new Domain.Sale.ErpSaleDbContext());
        //    var model = WarehouseRepository.GetAllWarehouse()
        //        .Select(item => new
        //        {
        //            item.Id,
        //            item.Code,
        //            item.Name
        //        }).ToList();

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(model));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        //#endregion

        //#region GetListService
        //public HttpResponseMessage GetListService()
        //{
        //    ProductRepository ProductRepository = new ProductRepository(new Domain.Sale.ErpSaleDbContext());
        //    var model = ProductRepository.GetAllProductByType("service")
        //        .Select(item => new
        //        {
        //            item.Id,
        //            item.Name,
        //            item.PriceInbound,
        //            item.PriceInbound2,
        //            item.PriceInbound3,
        //            item.PriceOutbound,
        //            item.PriceOutbound2,
        //            item.PriceOutbound3,
        //        }).ToList();
        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(model));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        //#endregion

        //#region GetListPaymentPendding
        //public HttpResponseMessage GetListPaymentPendding(int SupplierId, string WarehouseDestinationCode)
        //{
        //    PaymentRepository paymentRepository = new PaymentRepository(new Domain.Account.ErpAccountDbContext());
        //    ReceiptRepository ReceiptRepository = new ReceiptRepository(new Domain.Account.ErpAccountDbContext());
        //    PurchaseReturnsRepository PurchaseReturnsRepository = new PurchaseReturnsRepository(new Domain.Sale.ErpSaleDbContext());
        //    SalesDiscountRepository SalesDiscountRepository = new SalesDiscountRepository(new Domain.Sale.ErpSaleDbContext());
        //    SalesReturnsRepository SalesReturnsRepository = new SalesReturnsRepository(new Domain.Sale.ErpSaleDbContext());
        //    List<ExchangeVoucherViewModel> q = new List<ExchangeVoucherViewModel>();
        //    if (WarehouseDestinationCode != "LS")
        //    {
        //        q = paymentRepository.GetAllvwPayment()
        //         .Where(x => x.TargetId == SupplierId && x.RemainAmount > 0 && x.IsApproved == true)
        //         .Select(item => new ExchangeVoucherViewModel
        //         {
        //             Id = item.Id,
        //             Name = item.Name,
        //             MaCT = item.Code,
        //             Amount = item.RemainAmount,
        //             Note = item.Note,
        //             LoaiCT = "PaymentPending",
        //             IsCheck = false
        //         }).ToList();
        //        var PurchaseReturns = PurchaseReturnsRepository.GetAllvwPurchaseReturns().Where(x => x.SupplierId == SupplierId && x.RemainAmount > 0).ToList();
        //        foreach (var item in PurchaseReturns)
        //        {
        //            var ExchangeVoucher = new ExchangeVoucherViewModel();
        //            ExchangeVoucher.Id = item.Id;
        //            ExchangeVoucher.Name = "Hóa đơn trả hàng nhà cung cấp";
        //            ExchangeVoucher.MaCT = item.Code;
        //            ExchangeVoucher.Amount = item.RemainAmount;
        //            ExchangeVoucher.Note = item.Note;
        //            ExchangeVoucher.LoaiCT = "PurchaseReturns";
        //            ExchangeVoucher.IsCheck = false;
        //            q.Add(ExchangeVoucher);
        //        }
        //    }
        //    else
        //    {
        //        q = ReceiptRepository.GetAllReceipt()
        //          .Where(x => x.CustomerId == SupplierId && x.CustomerType == "Customer" && x.RemainAmount > 0 && x.IsApproved == true)
        //          .Select(item => new ExchangeVoucherViewModel
        //          {
        //              Id = item.Id,
        //              Name = item.Name,
        //              MaCT = item.Code,
        //              Amount = item.RemainAmount,
        //              Note = item.Note,
        //              LoaiCT = "ReceiptPending",
        //              IsCheck = false
        //          }).ToList();
        //        var SalesDiscount = SalesDiscountRepository.GetAllvwSalesDiscount().Where(x => x.CustomerId == SupplierId && x.RemainAmount > 0).ToList();
        //        foreach (var item in SalesDiscount)
        //        {
        //            var ExchangeVoucher = new ExchangeVoucherViewModel();
        //            ExchangeVoucher.Id = item.Id;
        //            ExchangeVoucher.Name = "Hóa đơn giảm giá hàng bán";
        //            ExchangeVoucher.MaCT = item.Code;
        //            ExchangeVoucher.Amount = item.RemainAmount;
        //            ExchangeVoucher.Note = item.Note;
        //            ExchangeVoucher.LoaiCT = "SalesDiscount";
        //            ExchangeVoucher.IsCheck = false;
        //            q.Add(ExchangeVoucher);
        //        }
        //        var SalesReturns = SalesReturnsRepository.GetAllvwSalesReturns().Where(x => x.CustomerId == SupplierId && x.RemainAmount > 0).ToList();
        //        foreach (var item in SalesReturns)
        //        {
        //            var ExchangeVoucher = new ExchangeVoucherViewModel();
        //            ExchangeVoucher.Id = item.Id;
        //            ExchangeVoucher.Name = "Hóa đơn hàng bán trả lại";
        //            ExchangeVoucher.MaCT = item.Code;
        //            ExchangeVoucher.Amount = item.RemainAmount;
        //            ExchangeVoucher.Note = item.Note;
        //            ExchangeVoucher.LoaiCT = "SalesReturns";
        //            ExchangeVoucher.IsCheck = false;
        //            q.Add(ExchangeVoucher);
        //        }
        //    }

        //    var resp = new HttpResponseMessage(HttpStatusCode.OK);
        //    resp.Content = new StringContent(JsonConvert.SerializeObject(q));
        //    resp.Content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
        //    return resp;
        //}
        //#endregion
    }
}