using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Entities.Mapping;

namespace Erp.Domain.Sale
{
    public class ErpSaleDbContext : DbContext, IDbContext
    {
        static ErpSaleDbContext()
        {
            Database.SetInitializer<ErpSaleDbContext>(null);
        }

        public ErpSaleDbContext()
            : base("Name=ErpDbContext")
        {
            // this.Configuration.LazyLoadingEnabled = false;
            // this.Configuration.ProxyCreationEnabled = false;
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        //Erp

        public DbSet<Product> Product { get; set; }
        public DbSet<vwProduct> vwProduct { get; set; }
      
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<vwSupplier> vwSupplier { get; set; }
        public DbSet<Warehouse> Warehouse { get; set; }
        public DbSet<PurchaseOrder> PurchaseOrder { get; set; }
        public DbSet<vwPurchaseOrder> vwPurchaseOrder { get; set; }
        public DbSet<PurchaseOrderDetail> PurchaseOrderDetail { get; set; }
        public DbSet<vwPurchaseOrderDetail> vwPurchaseOrderDetail { get; set; }
        public DbSet<Inventory> Inventory { get; set; }
        public DbSet<InventoryMaterial> InventoryMaterial { get; set; }
        public DbSet<vwInventory> vwInventory { get; set; }
        public DbSet<vwInventoryMaterial> vwInventoryMaterial { get; set; }
        public DbSet<InventoryByMonth> InventoryByMonth { get; set; }
        public DbSet<ProductOutbound> ProductOutbound { get; set; }
        public DbSet<vwProductOutbound> vwProductOutbound { get; set; }
        public DbSet<ProductInbound> ProductInbound { get; set; }
        public DbSet<vwProductInbound> vwProductInbound { get; set; }
        public DbSet<ProductInboundDetail> ProductInboundDetail { get; set; }
        public DbSet<ProductOutboundDetail> ProductOutboundDetail { get; set; }
        public DbSet<vwProductInboundDetail> vwProductInboundDetail { get; set; }
        public DbSet<vwProductOutboundDetail> vwProductOutboundDetail { get; set; }
        public DbSet<PhysicalInventory> PhysicalInventory { get; set; }
        public DbSet<vwPhysicalInventory> vwPhysicalInventory { get; set; }

        public DbSet<PhysicalInventoryMaterial> PhysicalInventoryMaterial { get; set; }

        public DbSet<vwPhysicalInventoryMaterial> vwPhysicalInventoryMaterial { get; set; }
        public DbSet<PhysicalInventoryDetail> PhysicalInventoryDetail { get; set; }
        public DbSet<vwPhysicalInventoryDetail> vwPhysicalInventoryDetail { get; set; }

        public DbSet<PhysicalInventoryMaterialDetail> PhysicalInventoryMaterialDetail { get; set; }

        public DbSet<vwPhysicalInventoryMaterialDetail> vwPhysicalInventoryMaterialDetail { get; set; }
        public DbSet<ObjectAttribute> ObjectAttribute { get; set; }
        public DbSet<ObjectAttributeValue> ObjectAttributeValue { get; set; }

        public DbSet<Commision> Commision { get; set; }
        public DbSet<ProductInvoice> ProductInvoice { get; set; }
        public DbSet<vwProductInvoice> vwProductInvoice { get; set; }
        public DbSet<vwProductInvoice_NVKD> vwProductInvoice_NVKD { get; set; }
        public DbSet<vwProductInvoice_return> vwProductInvoice_return { get; set; }
        public DbSet<ProductInvoiceDetail> ProductInvoiceDetail { get; set; }
        public DbSet<vwProductInvoiceDetail> vwProductInvoiceDetail { get; set; }
        public DbSet<vwCommision> vwCommision { get; set; }
        public DbSet<vwReportCustomer> vwReportCustomer { get; set; }
        public DbSet<vwReportProduct> vwReportProduct { get; set; }
        public DbSet<WarehouseLocationItem> WarehouseLocationItem { get; set; }
        public DbSet<vwWarehouseLocationItem> vwWarehouseLocationItem { get; set; }
        public DbSet<Promotion> Promotion { get; set; }
        public DbSet<PromotionDetail> PromotionDetail { get; set; }
        public DbSet<SalesReturns> SalesReturns { get; set; }
        public DbSet<vwSalesReturns> vwSalesReturns { get; set; }
        public DbSet<vwSalesReturnsDetail> vwSalesReturnsDetail { get; set; }
        public DbSet<SalesReturnsDetail> SalesReturnsDetail { get; set; }
        public DbSet<CommisionStaff> CommisionStaff { get; set; }
        public DbSet<vwCommisionStaff> vwCommisionStaff { get; set; }
        public DbSet<TemplatePrint> TemplatePrint { get; set; }
        public DbSet<vwService> vwService { get; set; }
        public DbSet<UsingServiceLog> UsingServiceLog { get; set; }
        public DbSet<ServiceCombo> ServiceCombo { get; set; }
        public DbSet<vwServiceCombo> vwServiceCombo { get; set; }
        public DbSet<vwUsingServiceLog> vwUsingServiceLog { get; set; }
        public DbSet<vwProductAndService> vwProductAndService { get; set; }
        public DbSet<RequestInbound> RequestInbound { get; set; }
        public DbSet<RequestInboundDetail> RequestInboundDetail { get; set; }
        public DbSet<UsingServiceLogDetail> UsingServiceLogDetail { get; set; }
        public DbSet<vwUsingServiceLogDetail> vwUsingServiceLogDetail { get; set; }
        public DbSet<vwRequestInbound> vwRequestInbound { get; set; }
        public DbSet<vwRequestInboundDetail> vwRequestInboundDetail { get; set; }
        public DbSet<ProductDamaged> ProductDamaged { get; set; }
        public DbSet<ServiceReminder> ServiceReminder { get; set; }
        public DbSet<ServiceReminderGroup> ServiceReminderGroup { get; set; }
        public DbSet<vwServiceReminderGroup> vwServiceReminderGroup { get; set; }
        public DbSet<LogServiceRemminder> LogServiceRemminder { get; set; }
        public DbSet<vwLogServiceRemminder> vwLogServiceRemminder { get; set; }

        public DbSet<CommisionCustomer> CommisionCustomer { get; set; }
        public DbSet<vwCommisionCustomer> vwCommisionCustomer { get; set; }
    //    public DbSet<CommisionCustomerLog> CommisionCustomerLog { get; set; }

        public DbSet<ServiceSchedule> ServiceSchedule { get; set; }
        public DbSet<vwServiceSchedule> vwServiceSchedule { get; set; }
        public DbSet<LogLoyaltyPoint> LogLoyaltyPoint { get; set; }
        public DbSet<vwLogLoyaltyPoint> vwLogLoyaltyPoint { get; set; }
        public DbSet<LoyaltyPoint> LoyaltyPoint { get; set; }
        public DbSet<CommissionCus> CommissionCus { get; set; }
        public DbSet<TotalDiscountMoneyNT> TotalDiscountMoneyNT { get; set; }
        public DbSet<vwTotalDiscountMoneyNT> vwTotalDiscountMoneyNT { get; set; }
        public DbSet<vwWarehouse> vwWarehouse { get; set; }
        public DbSet<LogVip> LogVip { get; set; }
        public DbSet<vwLogVip> vwLogVip { get; set; }
        public DbSet<Material> Material { get; set; }
        public DbSet<vwMaterial> vwMaterial { get; set; }
        public DbSet<MaterialInbound> MaterialInbound { get; set; }
        public DbSet<vwMaterialInbound> vwMaterialInbound { get; set; }
        public DbSet<MaterialInboundDetail> MaterialInboundDetail { get; set; }
        public DbSet<vwMaterialInboundDetail> vwMaterialInboundDetail { get; set; }
        public DbSet<MaterialOutbound> MaterialOutbound { get; set; }
        public DbSet<vwMaterialOutbound> vwMaterialOutbound { get; set; }
        public DbSet<MaterialOutboundDetail> MaterialOutboundDetail { get; set; }
        public DbSet<vwMaterialOutboundDetail> vwMaterialOutboundDetail { get; set; }
        public DbSet<InventoryMaterialByMonth> InventoryMaterialByMonth { get; set; }
        public DbSet<vwMaterialAndService> vwMaterialAndService { get; set; }
        public DbSet<AdviseCard> AdviseCard { get; set; }
        public DbSet<vwAdviseCard> vwAdviseCard { get; set; }
        public DbSet<AdviseCardDetail> AdviseCardDetail { get; set; }
        public DbSet<ProductDetail> ProductDetail { get; set; }
        public DbSet<InquiryCard> InquiryCard { get; set; }
        public DbSet<vwInquiryCard> vwInquiryCard { get; set; }
        public DbSet<InquiryCardDetail> InquiryCardDetail { get; set; }
        public DbSet<ServiceSteps> ServiceSteps { get; set; }
        public DbSet<Membership> Membership { get; set; }
        public DbSet<vwMembership> vwMembership { get; set; }
        public DbSet<SchedulingHistory> SchedulingHistory { get; set; }
        public DbSet<vwSchedulingHistory> vwSchedulingHistory { get; set; }
        public DbSet<StaffMade> StaffMade { get; set; }
        public DbSet<vwStaffMade> vwStaffMade { get; set; }
        public DbSet<ServiceDetail> ServiceDetail { get; set; }
        public DbSet<vwServiceDetail> vwServiceDetail { get; set; }
        public DbSet<CommisionInvoice> CommisionInvoice { get; set; }
        public DbSet<DonateProOrSer> DonateProOrSer { get; set; }
        public DbSet<vwDonateProOrSer> vwDonateProOrSer { get; set; }
        public DbSet<LogPromotion> LogPromotion { get; set; }
        public DbSet<vwProductDetail> vwProductDetail { get; set; }
        public DbSet<LogEquipment> LogEquipment { get; set; }
        public DbSet<vwLogEquipment> vwLogEquipment { get; set; }


        public DbSet<ProductSample> ProductSample { get; set; }
        public DbSet<vwProductSample> vwProductSample { get; set; }
        public DbSet<ProductSampleDetail> ProductSampleDetail { get; set; }
        public DbSet<vwProductSampleDetail> vwProductSampleDetail { get; set; }
        public DbSet<vwLogPromotion> vwLogPromotion { get; set; }
        public DbSet<vwPlanuseSkinCare> vwPlanuseSkinCares { get; set; }
        public DbSet<MoneyMove> MoneyMove { get; set; }
        public DbSet<MembershipThugon> MembershipThugon { get; set; }
        public DbSet<vwMembershipThugon> vwMembershipThugon { get; set; }
        public DbSet<Membership_parent> Membership_parent { get; set; }
        public DbSet<vwMembership_parent> vwMembership_parent { get; set; }
         public DbSet<BC_DOANHSO_NHANHANG> BC_DOANHSO_NHANHANG { get; set; }
 
        public DbSet<HOAHONG_NVKD> HOAHONG_NVKD { get; set; }
        public DbSet<Sale_TARGET_NVKD> Sale_TARGET_NVKD { get; set; }
        public DbSet<Sale_DSHOTRO_CN> Sale_DSHOTRO_CN { get; set; }


        //<append_content_DbSet_here>

        // mapping bằng scan Assembly
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            var addMethod = typeof(System.Data.Entity.ModelConfiguration.Configuration.ConfigurationRegistrar)
              .GetMethods()
              .Single(m =>
                m.Name == "Add"
                && m.GetGenericArguments().Any(a => a.Name == "TEntityType"));

            var domainCurrent = AppDomain.CurrentDomain.GetAssemblies().Where(a => a.GetName().Name == "Erp.Domain.Sale");
            foreach (var assembly in domainCurrent)
            {
                var configTypes = assembly
                  .GetTypes()
                  .Where(t => t.BaseType != null
                    && t.BaseType.IsGenericType
                    && t.BaseType.GetGenericTypeDefinition() == typeof(System.Data.Entity.ModelConfiguration.EntityTypeConfiguration<>));

                foreach (var type in configTypes)
                {
                    var entityType = type.BaseType.GetGenericArguments().Single();

                    var entityConfig = assembly.CreateInstance(type.FullName);
                    addMethod.MakeGenericMethod(entityType).Invoke(modelBuilder.Configurations, new object[] { entityConfig });
                }
            }
        }
    }

    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        void Dispose();
    }
}
