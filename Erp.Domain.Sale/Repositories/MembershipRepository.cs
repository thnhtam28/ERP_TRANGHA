using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class MembershipRepository : GenericRepository<ErpSaleDbContext, Membership>, IMembershipRepository
    {
        public MembershipRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all Membership
        /// </summary>
        /// <returns>Membership list</returns>
        public IQueryable<Membership> GetAllMembership()
        {
            return Context.Membership.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<Membership> GetAllMembershipByIdParent(Int64 IdParent)
        {
            return Context.Membership.Where(item => item.IdParent == IdParent && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<Membership> GetListAllMembershipByIdParent(Int64 IdParent)
        {
            return Context.Membership.Where(item => item.IdParent == IdParent && (item.IsDeleted == null || item.IsDeleted == false)).ToList();
        }
        public IQueryable<vwMembership> GetAllvwMembershipByIdParent(Int64 IdParent)
        {
            return Context.vwMembership.Where(item => item.IdParent == IdParent && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMembership> GetvwAllMembership()
        {
            return Context.vwMembership.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get Membership information by specific id
        /// </summary>
        /// <param name="MembershipId">Id of Membership</param>
        /// <returns></returns>
        public Membership GetMembershipById(Int64 Id)
        {
            return Context.Membership.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }


        public vwMembership GetvwMembershipById(Int64 Id)
        {
            return Context.vwMembership.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert Membership into database
        /// </summary>
        /// <param name="Membership">Object infomation</param>
        public void InsertMembership(Membership Membership)
        {
            Context.Membership.Add(Membership);
            Context.Entry(Membership).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete Membership with specific id
        /// </summary>
        /// <param name="Id">Membership Id</param>
        public void DeleteMembership(Int64 Id)
        {
            Membership deletedMembership = GetMembershipById(Id);
            Context.Membership.Remove(deletedMembership);
            Context.Entry(deletedMembership).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a Membership with its Id and Update IsDeleted IF that Membership has relationship with others
        /// </summary>
        /// <param name="MembershipId">Id of Membership</param>
        public void DeleteMembershipRs(Int64 Id)
        {
            Membership deleteMembershipRs = GetMembershipById(Id);
            if (deleteMembershipRs != null)
            {
                deleteMembershipRs.IsDeleted = true;
                UpdateMembership(deleteMembershipRs);
            }
        }

        /// <summary>
        /// Update Membership into database
        /// </summary>
        /// <param name="Membership">Membership object</param>
        public void UpdateMembership(Membership Membership)
        {
            Context.Entry(Membership).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
