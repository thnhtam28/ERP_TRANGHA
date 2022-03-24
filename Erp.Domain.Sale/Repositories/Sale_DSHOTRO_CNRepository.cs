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
    class Sale_DSHOTRO_CNRepository : GenericRepository<ErpSaleDbContext, Sale_DSHOTRO_CN>, ISale_DSHOTRO_CNRepository
    {
        public Sale_DSHOTRO_CNRepository(ErpSaleDbContext context) : base(context)
        {

        }

        public void DeleteSale_DSHOTRO_CN(int month, int year)
        {
            Sale_DSHOTRO_CN del_Sale_TARGET_NVKD = GetSale_DSHOTRO_CNByMonthOfYear(month, year);
            Context.Sale_DSHOTRO_CN.Remove(del_Sale_TARGET_NVKD);
            Context.Entry(del_Sale_TARGET_NVKD).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public IQueryable<Sale_DSHOTRO_CN> GetAllSale_DSHOTRO_CN()
        {
            return Context.Sale_DSHOTRO_CN.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public Sale_DSHOTRO_CN GetSale_DSHOTRO_CNByMonthOfYear(int month, int year)
        {
            return Context.Sale_DSHOTRO_CN.SingleOrDefault(item => item.Month == month && item.Year == year && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public Sale_DSHOTRO_CN GetSale_DSHOTRO_CNById(int Id)
        {
            return Context.Sale_DSHOTRO_CN.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public Sale_DSHOTRO_CN GetSale_DSHOTRO_CNByMonthYearBranch(int month, int year, int branchid)
        {
            return Context.Sale_DSHOTRO_CN.SingleOrDefault(item => item.Month == month && item.Year == year && item.BranchId == branchid && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertSale_DSHOTRO_CN(Sale_DSHOTRO_CN Sale_TARGET_NVKD)
        {
            Context.Sale_DSHOTRO_CN.Add(Sale_TARGET_NVKD);
            Context.Entry(Sale_TARGET_NVKD).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void UpdateSale_DSHOTRO_CN(Sale_DSHOTRO_CN Sale_TARGET_NVKD)
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
