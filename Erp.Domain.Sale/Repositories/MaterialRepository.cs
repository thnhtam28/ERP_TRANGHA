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
    public class MaterialRepository : GenericRepository<ErpSaleDbContext, Material>, IMaterialRepository
    {
        public MaterialRepository(ErpSaleDbContext context): base(context)
        {

        }

        public IQueryable<Material> GettAllMaterial()
        {
            return Context.Material.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public Material GetMaterialById(int Id)
        {
            return Context.Material.Where(item => item.Id == Id).FirstOrDefault(); ;
        }

        public void InsertMaterial(Material Material)
        {
            Context.Material.Add(Material);
            Context.Entry(Material).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void DeleteMaterial(int Id)
        {
            Material deletedMaterial = GetMaterialById(Id);
            Context.Material.Remove(deletedMaterial);
            Context.Entry(deletedMaterial).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeleteMaterialRs(int Id)
        {
            Material deleteMaterialRs = GetMaterialById(Id);
            deleteMaterialRs.IsDeleted = true;
            UpdateMaterial(deleteMaterialRs);
        }

        public void UpdateMaterial(Material Material)
        {
            Context.Entry(Material).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
