using Erp.Domain.Sale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IMaterialRepository
    {
        IQueryable<Material> GettAllMaterial();
        Material GetMaterialById(int Id);             
        void InsertMaterial(Material Material);

        void DeleteMaterial(int Id);

        void DeleteMaterialRs(int Id);

        void UpdateMaterial(Material Material);
    }
}
