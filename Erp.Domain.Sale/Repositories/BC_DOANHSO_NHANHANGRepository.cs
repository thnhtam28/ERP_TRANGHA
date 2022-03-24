using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;


namespace Erp.Domain.Sale.Repositories
{
    public class BC_DOANHSO_NHANHANGRepository : GenericRepository<ErpSaleDbContext, BC_DOANHSO_NHANHANG>, IBC_DOANHSO_NHANHANGRepository
    {
        public BC_DOANHSO_NHANHANGRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        public IQueryable<BC_DOANHSO_NHANHANG> GetAllBC_DOANHSO_NHANHANG()
        {
            return Context.BC_DOANHSO_NHANHANG.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public BC_DOANHSO_NHANHANG GetBC_DOANHSO_NHANHANGById(int Id)
        {
            return Context.BC_DOANHSO_NHANHANG.SingleOrDefault(item => item.DOANHSO_NHANHANG_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public BC_DOANHSO_NHANHANG GetBC_DOANHSO_NHANHANGByMaDH(string Id, int? BranchId)
        {
            return Context.BC_DOANHSO_NHANHANG.SingleOrDefault(item => item.BranchId == BranchId && item.MA_DONHANG == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertBC_DOANHSO_NHANHANG(BC_DOANHSO_NHANHANG BC_DOANHSO_NHANHANG)
        {
            Context.BC_DOANHSO_NHANHANG.Add(BC_DOANHSO_NHANHANG);
            Context.Entry(BC_DOANHSO_NHANHANG).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteBC_DOANHSO_NHANHANG(int Id)
        {
            BC_DOANHSO_NHANHANG deletedBC_DOANHSO_NHANHANG = GetBC_DOANHSO_NHANHANGById(Id);
            Context.BC_DOANHSO_NHANHANG.Remove(deletedBC_DOANHSO_NHANHANG);
            Context.Entry(deletedBC_DOANHSO_NHANHANG).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeleteBC_DOANHSO_NHANHANGRs(int Id)
        {
            BC_DOANHSO_NHANHANG deleteBC_DOANHSO_NHANHANGRs = GetBC_DOANHSO_NHANHANGById(Id);
            deleteBC_DOANHSO_NHANHANGRs.IsDeleted = true;
            UpdateBC_DOANHSO_NHANHANG(deleteBC_DOANHSO_NHANHANGRs);
        }

        public void UpdateBC_DOANHSO_NHANHANG(BC_DOANHSO_NHANHANG BC_DOANHSO_NHANHANG)
        {
            try
            {

                Context.Entry(BC_DOANHSO_NHANHANG).State = EntityState.Modified;
                Context.SaveChanges();
            }
            catch (System.Exception ex)
            {

                throw ex;
            }

        }
    }
}
