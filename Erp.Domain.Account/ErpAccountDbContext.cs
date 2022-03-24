using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using Erp.Domain.Account.Entities;
using System.Reflection;

namespace Erp.Domain.Account
{
    public class ErpAccountDbContext : DbContext, IDbContext
    {
        static ErpAccountDbContext()
        {
            Database.SetInitializer<ErpAccountDbContext>(null);
        }

        public ErpAccountDbContext()
            : base("Name=ErpDbContext")
        {
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }
        public DbSet<Customer> Customer { get; set; }
        public DbSet<Contact> Contact { get; set; }
        public DbSet<vwContact> vwContact { get; set; }
        public DbSet<ContractLease> ContractLease { get; set; }
        public DbSet<vwContractLease> vwContractLease { get; set; }
        public DbSet<InfoPartyA> InfoPartyA { get; set; }
        public DbSet<vwInfoPartyA> vwInfoPartyA { get; set; }
        public DbSet<ContractSell> ContractSell { get; set; }
        public DbSet<vwContractSell> vwContractSell { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public DbSet<vwContract> vwContract { get; set; }
        public DbSet<ProcessPayment> ProcessPayment { get; set; }
        public DbSet<vwProcessPayment> vwProcessPayment { get; set; }
        public DbSet<TransactionLiabilities> TransactionLiabilities { get; set; }
        public DbSet<vwTransactionLiabilities> vwTransactionLiabilities { get; set; }
        public DbSet<vwCustomer> vwCustomer { get; set; }
        public DbSet<vwLogContractbyCondos> vwLogContractbyCondos { get; set; }
        public DbSet<Receipt> Receipt { get; set; }
        public DbSet<ReceiptDetail> ReceiptDetail { get; set; }
        public DbSet<vwReceipt> vwReceipt { get; set; }
        public DbSet<vwAccount_Liabilities> vwAccount_Liabilities { get; set; }
        public DbSet<Payment> Payment { get; set; }
        public DbSet<vwPayment> vwPayment { get; set; }
        public DbSet<CustomerDiscount> CustomerDiscount { get; set; }
        public DbSet<CustomerCommitment> CustomerCommitment { get; set; }
        public DbSet<CustomerUser> CustomerUser { get; set; }
        public DbSet<vwCustomerUser> vwCustomerUser { get; set; }
        public DbSet<Transaction> Transaction { get; set; }
        public DbSet<TransactionRelationship> TransactionRelationship { get; set; }
        public DbSet<vwTransactionRelationship> vwTransactionRelationship { get; set; }
        public DbSet<MemberCard> MemberCard { get; set; }
        public DbSet<vwMemberCard> vwMemberCard { get; set; }
        public DbSet<PaymentDetail> PaymentDetail { get; set; }

        public DbSet<MemberCardDetail> MemberCardDetail { get; set; }
        public DbSet<LogReceipt> LogReceipt { get; set; }

        public DbSet<vwLogReceipt> vwLogReceipt { get; set; }

        public DbSet<vwLogCustomer> vwLogCustomer { get; set; }
        public DbSet<CustomerRecommend> CustomerRecommend { get; set; }
        public DbSet<vwCustomerRecommend> vwCustomerRecommend { get; set; }



        //<append_content_DbSet_here>
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // mapping bằng scan Assembly
            modelBuilder.Configurations.AddFromAssembly(Assembly.GetAssembly(GetType())); //Current Assembly
            base.OnModelCreating(modelBuilder);
        }
    }

    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        int SaveChanges();
        void Dispose();
    }
}
