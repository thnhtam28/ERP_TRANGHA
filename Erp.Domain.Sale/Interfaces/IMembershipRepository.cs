using Erp.Domain.Sale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IMembershipRepository
    {
        /// <summary>
        /// Get all Membership
        /// </summary>
        /// <returns>Membership list</returns>
        IQueryable<Membership> GetAllMembership();
        IQueryable<Membership> GetAllMembershipByIdParent(Int64 IdParent);
        List<Membership> GetListAllMembershipByIdParent(Int64 IdParent);
        IQueryable<vwMembership> GetAllvwMembershipByIdParent(Int64 IdParent);
        IQueryable<vwMembership> GetvwAllMembership();
        /// <summary>
        /// Get Membership information by specific id
        /// </summary>
        /// <param name="Id">Id of Membership</param>
        /// <returns></returns>
        Membership GetMembershipById(Int64 Id);
        vwMembership GetvwMembershipById(Int64 Id);
        /// <summary>
        /// Insert Membership into database
        /// </summary>
        /// <param name="Membership">Object infomation</param>
        void InsertMembership(Membership Membership);

        /// <summary>
        /// Delete Membership with specific id
        /// </summary>
        /// <param name="Id">Membership Id</param>
        void DeleteMembership(Int64 Id);

        /// <summary>
        /// Delete a Membership with its Id and Update IsDeleted IF that Membership has relationship with others
        /// </summary>
        /// <param name="Id">Id of Membership</param>
        void DeleteMembershipRs(Int64 Id);

        /// <summary>
        /// Update Membership into database
        /// </summary>
        /// <param name="Membership">Membership object</param>
        void UpdateMembership(Membership Membership);
    }
}
