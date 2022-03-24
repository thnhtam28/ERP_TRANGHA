using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IBC_DOANHSO_NHANHANGRepository
    {
        IQueryable<BC_DOANHSO_NHANHANG> GetAllBC_DOANHSO_NHANHANG();



        BC_DOANHSO_NHANHANG GetBC_DOANHSO_NHANHANGById(int Id);
        BC_DOANHSO_NHANHANG GetBC_DOANHSO_NHANHANGByMaDH(string MaDH, int? BranchId);

        void InsertBC_DOANHSO_NHANHANG(BC_DOANHSO_NHANHANG BC_DOANHSO_NHANHANG);

       
        void DeleteBC_DOANHSO_NHANHANG(int Id);

       
        void DeleteBC_DOANHSO_NHANHANGRs(int Id);

        
        void UpdateBC_DOANHSO_NHANHANG(BC_DOANHSO_NHANHANG BC_DOANHSO_NHANHANG);

        

       

    }
}
