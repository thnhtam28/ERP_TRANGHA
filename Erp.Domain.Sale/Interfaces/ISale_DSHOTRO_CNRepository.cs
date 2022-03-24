using Erp.Domain.Sale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Interfaces
{
    public interface ISale_DSHOTRO_CNRepository
    {
        IQueryable<Sale_DSHOTRO_CN> GetAllSale_DSHOTRO_CN();

        Sale_DSHOTRO_CN GetSale_DSHOTRO_CNByMonthOfYear(int month, int year);

        void InsertSale_DSHOTRO_CN(Sale_DSHOTRO_CN Sale_DSHOTRO_CN);

        void DeleteSale_DSHOTRO_CN(int month, int year);

        void UpdateSale_DSHOTRO_CN(Sale_DSHOTRO_CN Sale_DSHOTRO_CN);

        Sale_DSHOTRO_CN GetSale_DSHOTRO_CNByMonthYearBranch(int month, int year, int branchid);

        Sale_DSHOTRO_CN GetSale_DSHOTRO_CNById(int Id);
    }
}
