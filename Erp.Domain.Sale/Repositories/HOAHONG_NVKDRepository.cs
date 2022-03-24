using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;

using System.Linq;
using System;


namespace Erp.Domain.Sale.Repositories
{
   
    public class HOAHONG_NVKDRepository : GenericRepository<ErpSaleDbContext, HOAHONG_NVKD>, IHOAHONG_NVKDRepository
    {
        public HOAHONG_NVKDRepository(ErpSaleDbContext context) 
            : base(context)
        { }

        public IQueryable<HOAHONG_NVKD> GetAllHOAHONG_NVKD()
        {
            return Context.HOAHONG_NVKD.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public HOAHONG_NVKD GetHOAHONG_NVKDById(int Id)
        {
            return Context.HOAHONG_NVKD.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertHOAHONG_NVKD(HOAHONG_NVKD HOAHONG_NVKD)
        {
            Context.HOAHONG_NVKD.Add(HOAHONG_NVKD);
            Context.Entry(HOAHONG_NVKD).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteHOAHONG_NVKD(int Id)
        {
            HOAHONG_NVKD deletedLoyaltyPoint = GetHOAHONG_NVKDById(Id);
            Context.HOAHONG_NVKD.Remove(deletedLoyaltyPoint);
            Context.Entry(deletedLoyaltyPoint).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeleteHOAHONG_NVKDRs(int Id)
        {
            HOAHONG_NVKD deleteLoyaltyPointRs = GetHOAHONG_NVKDById(Id);
            deleteLoyaltyPointRs.IsDeleted = true;
            UpdateHOAHONG_NVKD(deleteLoyaltyPointRs);
        }

        public void UpdateHOAHONG_NVKD(HOAHONG_NVKD LoyaltyPoint)
        {
            Context.Entry(LoyaltyPoint).State = EntityState.Modified;
            Context.SaveChanges();
        }
    } 
}

