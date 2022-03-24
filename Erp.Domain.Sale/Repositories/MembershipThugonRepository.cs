using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class MembershipThugonRepository : GenericRepository<ErpSaleDbContext, MembershipThugon>, IMembershipThugonRepository 
    {
        public MembershipThugonRepository(ErpSaleDbContext context)
            : base(context)
        {

        }
        /// <summary>
        /// Get all Membership
        /// </summary>
        /// <returns>Membership list</returns>
        public IQueryable<MembershipThugon> GetAllMembershipThugon()
        {
            return Context.MembershipThugon.Where(item => item.CustomerId != null );
        }
        public IQueryable<vwMembershipThugon> GetvwAllMembershipThugon()
        {
            return Context.vwMembershipThugon.Where(item => (item.CustomerId != null ));
        }

        
    }
}
