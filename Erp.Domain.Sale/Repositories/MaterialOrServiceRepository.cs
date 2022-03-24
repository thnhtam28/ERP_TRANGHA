using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class MaterialOrServiceRepository : GenericRepository<ErpSaleDbContext, Material>, IMaterialOrServiceRepository
    {
        public MaterialOrServiceRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all Material
        /// </summary>
        /// <returns>Material list</returns>
        /// 
        public IQueryable<Material> GetAllMaterial()
        {
            return Context.Material.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwMaterialAndService> GetAllvwMaterialAndService()
        {
            return Context.vwMaterialAndService.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwMaterial> GetAllvwMaterial()
        {
            return Context.vwMaterial.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwService> GetAllvwService()
        {
            return Context.vwService.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        //public IQueryable<Material> GetAllMaterialByType(string type)
        //{
        //    return Context.Material.Where(item => item.Type.Contains(type) && (item.IsDeleted == null || item.IsDeleted == false));
        //}

        //public IQueryable<vwMaterial> GetAllvwMaterialByType(string type)
        //{
        //    return Context.vwMaterial.Where(item => item.Type.Contains(type) && (item.IsDeleted == null || item.IsDeleted == false));
        //}


        /// <summary>
        /// Get Material information by specific id
        /// </summary>
        /// <param name="MaterialId">Id of Material</param>
        /// <returns></returns>
        
        public Material GetMaterialById(int Id)
        {
            return Context.Material.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwMaterial GetvwMaterialById(int Id)
        {
            return Context.vwMaterial.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwService GetvwServiceById(int Id)
        {
            return Context.vwService.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert Material into database
        /// </summary>
        /// <param name="Material">Object infomation</param>
        public void InsertMaterial(Material Material)
        {
            Context.Material.Add(Material);
            Context.Entry(Material).State = EntityState.Added;
            Context.SaveChanges();
        }
        public void InsertService(Material Service)
        {
            Context.Material.Add(Service);
            Context.Entry(Service).State = EntityState.Added;
            Context.SaveChanges();
        }


        /// <summary>
        /// Delete Material with specific id
        /// </summary>
        /// <param name="Id">Material Id</param>
        public void DeleteMaterial(int Id)
        {
            Material deletedMaterial = GetMaterialById(Id);
            Context.Material.Remove(deletedMaterial);
            Context.Entry(deletedMaterial).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        public void DeleteService(int Id)
        {
            Material deletedService = GetMaterialById(Id);
            Context.Material.Remove(deletedService);
            Context.Entry(deletedService).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        /// <summary>
        /// Delete a Material with its Id and Update IsDeleted IF that Material has relationship with others
        /// </summary>
        /// <param name="MaterialId">Id of Material</param>
        public void DeleteMaterialRs(int Id)
        {
            Material deleteMaterialRs = GetMaterialById(Id);
            deleteMaterialRs.IsDeleted = true;
            UpdateMaterial(deleteMaterialRs);
        }
        public void DeleteServiceRs(int Id)
        {
            Material deleteServiceRs = GetMaterialById(Id);
            deleteServiceRs.IsDeleted = true;
            UpdateService(deleteServiceRs);
        }
        /// <summary>
        /// Update Material into database
        /// </summary>
        /// <param name="Material">Material object</param>
        public void UpdateMaterial(Material Material)
        {
            Context.Entry(Material).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void UpdateService(Material Service)
        {
            Context.Entry(Service).State = EntityState.Modified;
            Context.SaveChanges();
        }


        public void InsertServiceCombo(vwService Service, List<ServiceCombo> orderDetails)
        {
            Context.vwService.Add(Service);
            Context.Entry(Service).State = EntityState.Added;
            Context.SaveChanges();
            for (int i = 0; i < orderDetails.Count; i++)
            {
                orderDetails[i].ComboId = Service.Id;
                InsertServiceCombo(orderDetails[i]);
            }

            //return ServiceInvoice.Id;
        }
        public void InsertServiceCombo(ServiceCombo ServiceCombo)
        {
            Context.ServiceCombo.Add(ServiceCombo);
            Context.Entry(ServiceCombo).State = EntityState.Added;
            Context.SaveChanges();
        }
        public ServiceCombo GetServiceComboById(int Id)
        {
            return Context.ServiceCombo.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public void DeleteServiceCombo(IEnumerable<ServiceCombo> list)
        {
            for (int i = 0; i < list.Count(); i++)
            {
                ServiceCombo deletedServiceCombo = GetServiceComboById(list.ElementAt(i).Id);
                Context.ServiceCombo.Remove(deletedServiceCombo);
                Context.Entry(deletedServiceCombo).State = EntityState.Deleted;
            }
            Context.SaveChanges();
        }
    }
}
