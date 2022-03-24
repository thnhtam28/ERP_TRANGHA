using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IMaterialOrServiceRepository
    {
        /// <summary>
        /// Get all Material
        /// </summary>
        /// <returns>Material list</returns>
        /// Lấy hàng hóa và dịch vụ chung luôn là Material
        IQueryable<Material> GetAllMaterial();
        /// <summary>
        /// lấy view của hàng hóa và dịch vụ
        /// </summary>
        /// <returns></returns>
        IQueryable<vwMaterialAndService> GetAllvwMaterialAndService();
        /// <summary>
        /// Lấy hàng hóa không là Material
        /// </summary>
        /// <returns></returns>
        IQueryable<vwMaterial> GetAllvwMaterial();
        /// <summary>
        /// lấy dịch vụ không là service
        /// </summary>
        /// <returns></returns>
        IQueryable<vwService> GetAllvwService();

        //IQueryable<Material> GetAllMaterialByType(string type);

        //IQueryable<vwMaterial> GetAllvwMaterialByType(string type);
        /// <summary>
        /// Get Material information by specific id
        /// </summary>
        /// <param name="Id">Id of Material</param>
        /// <returns></returns>
        Material GetMaterialById(int Id);
        vwMaterial GetvwMaterialById(int Id);
        vwService GetvwServiceById(int Id);
        /// <summary>
        /// Insert Material into database
        /// </summary>
        /// <param name="Material">Object infomation</param>
        void InsertMaterial(Material Material);
        void InsertService(Material Service);
        /// <summary>
        /// Delete Material with specific id
        /// </summary>
        /// <param name="Id">Material Id</param>
        void DeleteMaterial(int Id);
        void DeleteService(int Id);
        /// <summary>
        /// Delete a Material with its Id and Update IsDeleted IF that Material has relationship with others
        /// </summary>
        /// <param name="Id">Id of Material</param>
        void DeleteMaterialRs(int Id);
        void DeleteServiceRs(int Id);
        /// <summary>
        /// Update Material into database
        /// </summary>
        /// <param name="Material">Material object</param>
        void UpdateMaterial(Material Material);
        void UpdateService(Material Service);
        ServiceCombo GetServiceComboById(int Id);
        void InsertServiceCombo(vwService Service, List<ServiceCombo> orderDetails);
        void DeleteServiceCombo(IEnumerable<ServiceCombo> list);
        void InsertServiceCombo(ServiceCombo ServiceCombo);
    }
}
