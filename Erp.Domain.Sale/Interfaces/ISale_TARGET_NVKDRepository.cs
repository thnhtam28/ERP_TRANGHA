using Erp.Domain.Sale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Interfaces
{
    public interface ISale_TARGET_NVKDRepository
    {
        IQueryable<Sale_TARGET_NVKD> GetAllSale_TARGET_NVKD();

        Sale_TARGET_NVKD GetSale_TARGET_NVKDByMonthOfYear(int month, int year);

        void InsertSale_TARGET_NVKD(Sale_TARGET_NVKD Sale_TARGET_NVKD);

        void DeleteSale_TARGET_NVKD(int month, int year);

        void UpdateSale_TARGET_NVKD(Sale_TARGET_NVKD Sale_TARGET_NVKD);

        Sale_TARGET_NVKD GetSale_TARGET_NVKDByMonthYearBranch(int month, int year, int branchid);

        Sale_TARGET_NVKD GetSale_TARGET_NVKDById(int Id);
    }
}
