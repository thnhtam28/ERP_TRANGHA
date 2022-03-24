using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class InventoryMaterialRepository : GenericRepository<ErpSaleDbContext, InventoryMaterial>, IInventoryMaterialRepository
    {
        public InventoryMaterialRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        public IQueryable<InventoryMaterial> GetAllInventoryMaterial()
        {
            return Context.InventoryMaterial.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterial()
        {
            return Context.vwInventoryMaterial;
        }
        public IQueryable<InventoryMaterial> GetAllInventoryMaterialByWarehouseId(int Id)
        {
            return Context.InventoryMaterial.Where(item => item.WarehouseId == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<InventoryMaterial> GetAllInventoryMaterialByMaterialId(int Id)
        {
            return Context.InventoryMaterial.Where(item => item.MaterialId == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterialByWarehouseId(int Id)
        {
            return Context.vwInventoryMaterial.Where(item => item.WarehouseId == Id);
        }

        public IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterialByBranchId(int BranchId)
        {
            return Context.vwInventoryMaterial.Where(item => item.BranchId == BranchId);
        }

        public IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterialByMaterialId(int Id)
        {
            return Context.vwInventoryMaterial.Where(item => item.MaterialId == Id);
        }

        public InventoryMaterial GetInventoryMaterialById(int Id)
        {
            return Context.InventoryMaterial.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public InventoryMaterial GetInventoryMaterialByWarehouseIdAndMaterialId(int warehouseId, int MaterialId)
        {
            return Context.InventoryMaterial.SingleOrDefault(item => item.WarehouseId == warehouseId && item.MaterialId == MaterialId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertInventoryMaterial(InventoryMaterial InventoryMaterial)
        {
            Context.InventoryMaterial.Add(InventoryMaterial);
            Context.Entry(InventoryMaterial).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteInventoryMaterial(int Id)
        {
            InventoryMaterial deletedInventoryMaterial = GetInventoryMaterialById(Id);
            Context.InventoryMaterial.Remove(deletedInventoryMaterial);
            Context.Entry(deletedInventoryMaterial).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        public void DeleteInventoryMaterialRs(int Id)
        {
            InventoryMaterial deleteInventoryMaterialRs = GetInventoryMaterialById(Id);
            deleteInventoryMaterialRs.IsDeleted = true;
            UpdateInventoryMaterial(deleteInventoryMaterialRs);
        }

        public void UpdateInventoryMaterial(InventoryMaterial InventoryMaterial)
        {
            Context.Entry(InventoryMaterial).State = EntityState.Modified;
            Context.SaveChanges();
        }

        // InventoryMaterial by month -------------------------------------------------------------------------------------------------------------------

        public InventoryMaterialByMonth GetInventoryMaterialByMonthById(int id)
        {
            return Context.InventoryMaterialByMonth.Where(x => x.Id == id && (x.IsDeleted == null || x.IsDeleted == false)).FirstOrDefault();
        }

        public IQueryable<InventoryMaterialByMonth> GetInventoryMaterialByMonthByWarehouseIdAndMaterialId(int warehouseId, int MaterialId)
        {
            return Context.InventoryMaterialByMonth.Where(x => x.WarehouseId == warehouseId && x.MaterialId == MaterialId && (x.IsDeleted == null || x.IsDeleted == false));
        }

        public IQueryable<InventoryMaterialByMonth> GetInventoryMaterialByMonth(int month, int year)
        {
            return Context.InventoryMaterialByMonth.Where(x => x.Month == month && x.Year == year && (x.IsDeleted == null || x.IsDeleted == false));
        }

        public void InsertInventoryMaterialByMonth(InventoryMaterialByMonth InventoryMaterial)
        {
            Context.InventoryMaterialByMonth.Add(InventoryMaterial);
            Context.Entry(InventoryMaterial).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void UpdateInventoryMaterialByMonth(InventoryMaterialByMonth InventoryMaterial)
        {
            Context.Entry(InventoryMaterial).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public void DeleteInventoryMaterialByMonth(int Id)
        {
            InventoryMaterialByMonth deletedInventoryMaterial = GetInventoryMaterialByMonthById(Id);
            Context.InventoryMaterialByMonth.Remove(deletedInventoryMaterial);
            Context.Entry(deletedInventoryMaterial).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeleteInventoryMaterialByMonthRs(int Id)
        {
            InventoryMaterialByMonth deleteInventoryMaterialRs = GetInventoryMaterialByMonthById(Id);
            deleteInventoryMaterialRs.IsDeleted = true;
            UpdateInventoryMaterialByMonth(deleteInventoryMaterialRs);
        }
    }
}
