using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Repositories
{
    class Sale_TARGET_NVKDRepository : GenericRepository<ErpSaleDbContext, Sale_TARGET_NVKD>, ISale_TARGET_NVKDRepository
    {
        public Sale_TARGET_NVKDRepository(ErpSaleDbContext context) : base(context)
        {

        }

        public void DeleteSale_TARGET_NVKD(int month, int year)
        {
            Sale_TARGET_NVKD del_Sale_TARGET_NVKD = GetSale_TARGET_NVKDByMonthOfYear(month, year);
            Context.Sale_TARGET_NVKD.Remove(del_Sale_TARGET_NVKD);
            Context.Entry(del_Sale_TARGET_NVKD).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public IQueryable<Sale_TARGET_NVKD> GetAllSale_TARGET_NVKD()
        {
            return Context.Sale_TARGET_NVKD.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public Sale_TARGET_NVKD GetSale_TARGET_NVKDByMonthOfYear(int month, int year)
        {
            return Context.Sale_TARGET_NVKD.SingleOrDefault(item => item.month == month && item.year == year && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public Sale_TARGET_NVKD GetSale_TARGET_NVKDById(int Id)
        {
            return Context.Sale_TARGET_NVKD.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public Sale_TARGET_NVKD GetSale_TARGET_NVKDByMonthYearBranch(int month, int year, int branchid)
        {
            return Context.Sale_TARGET_NVKD.SingleOrDefault(item => item.month == month && item.year == year && item.BranchId == branchid && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertSale_TARGET_NVKD(Sale_TARGET_NVKD Sale_TARGET_NVKD)
        {
            Context.Sale_TARGET_NVKD.Add(Sale_TARGET_NVKD);
            Context.Entry(Sale_TARGET_NVKD).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void UpdateSale_TARGET_NVKD(Sale_TARGET_NVKD Sale_TARGET_NVKD)
        {
            try
            {

                Context.Entry(Sale_TARGET_NVKD).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }
        }
    }
}
