using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IMembershipThugonRepository
    {
        /// <summary>
        /// Get all Membership
        /// </summary>
        /// <returns>Membership list</returns>
        IQueryable<MembershipThugon> GetAllMembershipThugon();
        IQueryable<vwMembershipThugon> GetvwAllMembershipThugon();
        
    }
}
