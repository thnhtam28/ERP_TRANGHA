using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IInventoryMaterialRepository
    {
        IQueryable<InventoryMaterial> GetAllInventoryMaterial();
        IQueryable<InventoryMaterial> GetAllInventoryMaterialByWarehouseId(int Id);
        IQueryable<InventoryMaterial> GetAllInventoryMaterialByMaterialId(int Id);

        IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterial();
        IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterialByWarehouseId(int Id);

        IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterialByBranchId(int BranchId);
        IQueryable<vwInventoryMaterial> GetAllvwInventoryMaterialByMaterialId(int Id);

        InventoryMaterial GetInventoryMaterialById(int Id);
        InventoryMaterial GetInventoryMaterialByWarehouseIdAndMaterialId(int warehouseId, int MaterialId);

        void InsertInventoryMaterial(InventoryMaterial InventoryMaterial);

        void DeleteInventoryMaterial(int Id);

        void DeleteInventoryMaterialRs(int Id);

        void UpdateInventoryMaterial(InventoryMaterial InventoryMaterial);


        // InventoryMaterial by month  ---------------------------------------------------------------------------------------------------

        InventoryMaterialByMonth GetInventoryMaterialByMonthById(int id);
        IQueryable<InventoryMaterialByMonth> GetInventoryMaterialByMonth(int month, int year);
        IQueryable<InventoryMaterialByMonth> GetInventoryMaterialByMonthByWarehouseIdAndMaterialId(int warehouseId, int MaterialId);

        void InsertInventoryMaterialByMonth(InventoryMaterialByMonth InventoryMaterial);

        void UpdateInventoryMaterialByMonth(InventoryMaterialByMonth InventoryMaterial);

        void DeleteInventoryMaterialByMonth(int Id);

        void DeleteInventoryMaterialByMonthRs(int Id);
    }
}
