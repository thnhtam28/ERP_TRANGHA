using System;
using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IPhysicalInventoryMaterialRepository
    {
        IQueryable<PhysicalInventoryMaterial> GetAllPhysicalInventoryMaterial();
        IQueryable<vwPhysicalInventoryMaterial> GetAllvwPhysicalInventoryMaterial();

        PhysicalInventoryMaterial GetPhysicalInventoryMaterialById(int Id);
        vwPhysicalInventoryMaterial GetvwPhysicalInventoryMaterialById(int Id);

        void InsertPhysicalInventoryMaterial(PhysicalInventoryMaterial PhysicalInventoryMaterial, List<PhysicalInventoryMaterialDetail> DetailList);

        void DeletePhysicalInventoryMaterial(int Id);

        void DeletePhysicalInventoryMaterialRs(int Id);

        void UpdatePhysicalInventoryMaterial(PhysicalInventoryMaterial PhysicalInventoryMaterial);


        // ------------------------- 

        IQueryable<PhysicalInventoryMaterialDetail> GetAllPhysicalInventoryMaterialDetail(int PhysicalInventoryMaterialId);

        IQueryable<vwPhysicalInventoryMaterialDetail> GetAllvwPhysicalInventoryMaterialDetail(int PhysicalInventoryMaterialId);

        PhysicalInventoryMaterialDetail GetPhysicalInventoryMaterialDetailById(int Id);

        void InsertPhysicalInventoryMaterialDetail(PhysicalInventoryMaterialDetail PhysicalInventoryMaterialDetail);

        void DeletePhysicalInventoryMaterialDetail(int Id);

        void DeletePhysicalInventoryMaterialDetailRs(int Id);

        void UpdatePhysicalInventoryMaterialDetail(PhysicalInventoryMaterialDetail PhysicalInventoryMaterialDetail);
    }
}
