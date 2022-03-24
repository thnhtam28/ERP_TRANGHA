using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Sale.Repositories
{
    public class PlanRepository : GenericRepository<ErpSaleDbContext, vwPlanuseSkinCare>, IPlanRepository
    {
        public PlanRepository(ErpSaleDbContext context)
        : base(context)
        {

        }

        public IQueryable<vwPlanuseSkinCare> GetvwAllvwPlanuseSkinCare()
        {
            return Context.vwPlanuseSkinCares;
        }
    }
}
