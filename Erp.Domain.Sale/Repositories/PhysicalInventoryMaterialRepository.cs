using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System;

namespace Erp.Domain.Sale.Repositories
{
    public class PhysicalInventoryMaterialRepository : GenericRepository<ErpSaleDbContext, PhysicalInventoryMaterial>, IPhysicalInventoryMaterialRepository
    {
        public PhysicalInventoryMaterialRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        public void DeletePhysicalInventoryMaterial(int Id)
        {
            PhysicalInventoryMaterial deletedPhysicalInventoryMaterial = GetPhysicalInventoryMaterialById(Id);
            Context.PhysicalInventoryMaterial.Remove(deletedPhysicalInventoryMaterial);
            Context.Entry(deletedPhysicalInventoryMaterial).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeletePhysicalInventoryMaterialDetail(int Id)
        {
            PhysicalInventoryMaterialDetail deletedPhysicalInventoryMaterialDetail = GetPhysicalInventoryMaterialDetailById(Id);
            Context.PhysicalInventoryMaterialDetail.Remove(deletedPhysicalInventoryMaterialDetail);
            Context.Entry(deletedPhysicalInventoryMaterialDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeletePhysicalInventoryMaterialDetailRs(int Id)
        {
            PhysicalInventoryMaterialDetail deletePhysicalInventoryMaterialDetailRs = GetPhysicalInventoryMaterialDetailById(Id);
            deletePhysicalInventoryMaterialDetailRs.IsDeleted = true;
            UpdatePhysicalInventoryMaterialDetail(deletePhysicalInventoryMaterialDetailRs);
        }

        public void DeletePhysicalInventoryMaterialRs(int Id)
        {
            PhysicalInventoryMaterial deletePhysicalInventoryMaterialRs = GetPhysicalInventoryMaterialById(Id);
            deletePhysicalInventoryMaterialRs.IsDeleted = true;
            UpdatePhysicalInventoryMaterial(deletePhysicalInventoryMaterialRs);
        }

        public IQueryable<PhysicalInventoryMaterial> GetAllPhysicalInventoryMaterial()
        {
            return Context.PhysicalInventoryMaterial.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<PhysicalInventoryMaterialDetail> GetAllPhysicalInventoryMaterialDetail(int PhysicalInventoryMaterialId)
        {
            return Context.PhysicalInventoryMaterialDetail.Where(item => item.PhysicalInventoryMaterialId == PhysicalInventoryMaterialId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwPhysicalInventoryMaterial> GetAllvwPhysicalInventoryMaterial()
        {
            return Context.vwPhysicalInventoryMaterial;
        }

        public IQueryable<vwPhysicalInventoryMaterialDetail> GetAllvwPhysicalInventoryMaterialDetail(int PhysicalInventoryMaterialId)
        {
            return Context.vwPhysicalInventoryMaterialDetail.Where(item => item.PhysicalInventoryMaterialId == PhysicalInventoryMaterialId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public PhysicalInventoryMaterial GetPhysicalInventoryMaterialById(int Id)
        {
            return Context.PhysicalInventoryMaterial.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public PhysicalInventoryMaterialDetail GetPhysicalInventoryMaterialDetailById(int Id)
        {
            return Context.PhysicalInventoryMaterialDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwPhysicalInventoryMaterial GetvwPhysicalInventoryMaterialById(int Id)
        {
            return Context.vwPhysicalInventoryMaterial.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertPhysicalInventoryMaterial(PhysicalInventoryMaterial PhysicalInventoryMaterial, List<PhysicalInventoryMaterialDetail> DetailList)
        {
            Context.PhysicalInventoryMaterial.Add(PhysicalInventoryMaterial);
            Context.Entry(PhysicalInventoryMaterial).State = EntityState.Added;
            Context.SaveChanges();

            for (int i = 0; i < DetailList.Count; i++)
            {
                DetailList[i].PhysicalInventoryMaterialId = PhysicalInventoryMaterial.Id;
                InsertPhysicalInventoryMaterialDetail(DetailList[i]);
            }
        }

        public void InsertPhysicalInventoryMaterialDetail(PhysicalInventoryMaterialDetail PhysicalInventoryMaterialDetail)
        {
            Context.PhysicalInventoryMaterialDetail.Add(PhysicalInventoryMaterialDetail);
            Context.Entry(PhysicalInventoryMaterialDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void UpdatePhysicalInventoryMaterial(PhysicalInventoryMaterial PhysicalInventoryMaterial)
        {
            Context.Entry(PhysicalInventoryMaterial).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void UpdatePhysicalInventoryMaterialDetail(PhysicalInventoryMaterialDetail PhysicalInventoryMaterialDetail)
        {
            Context.Entry(PhysicalInventoryMaterialDetail).State = EntityState.Modified;
            Context.SaveChanges();

        }
    }
}
