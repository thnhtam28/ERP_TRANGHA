using Erp.Domain.Sale.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IMembership_parentRepository
    {
        /// <summary>
        /// Get all Membership_parent
        /// </summary>
        /// <returns>Membership_parent list</returns>
        IQueryable<Membership_parent> GetAllMembership_parent();
        IQueryable<vwMembership_parent> GetAllvwMembership_parent();

        IQueryable<Membership_parent> GetAllMembership_parentByInvoiceId(int InvoiceId);
        IQueryable<vwMembership_parent> GetAllvwMembership_parentByInvoiceId(int InvoiceId);

        /// <summary>
        /// Get Membership_parent information by specific id
        /// </summary>
        /// <param name="Membership_parent">Id of Membership_parent</param>
        /// <returns></returns>
        Membership_parent GetMembership_parentById(Int64 Id);
        List<Membership_parent> GetListMembership_parentById(Int64 Id);
        List<Membership_parent> GetListMembership_parentByIdcustomer(Int64 Id);
        vwMembership_parent GetvwMembership_parentById(Int64 Id);

        /// <summary>
        /// Insert Membership_parent into database
        /// </summary>
        /// <param name="Membership_parent">Object infomation</param>
        void InsertMembership_parent(Membership_parent Membership_parent);

        /// <summary>
        /// Delete Membership_parent with specific id
        /// </summary>
        /// <param name="Id">Membership_parent Id</param>
        void DeleteMembership_parent(Int64 Id);

        /// <summary>
        /// Delete a Membership_parent with its Id and Update IsDeleted IF that Membership_parent has relationship with others
        /// </summary>
        /// <param name="Membership_parentId">Id of Membership_parent</param>
        void DeleteMembership_parentRs(Int64 Id);

        /// <summary>
        /// Update Membership_parent into database
        /// </summary>
        /// <param name="Membership_parent">Membership_parent object</param>
        void UpdateMembership_parent(Membership_parent Membership_parent);
       
    }
}
