using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class Membership_parentRepository : GenericRepository<ErpSaleDbContext, Membership_parent>, IMembership_parentRepository
    {
        public Membership_parentRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all Membership_parent
        /// </summary>
        /// <returns>Membership_parent list</returns>
        public IQueryable<Membership_parent> GetAllMembership_parent()
        {
            return Context.Membership_parent.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMembership_parent> GetAllvwMembership_parent()
        {
            return Context.vwMembership_parent.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<Membership_parent> GetAllMembership_parentByInvoiceId(int InvoiceId)
        {
            return Context.Membership_parent.Where(item => item.ProductInvoiceId == InvoiceId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwMembership_parent> GetAllvwMembership_parentByInvoiceId(int InvoiceId)
        {
            return Context.vwMembership_parent.Where(item => item.ProductInvoiceId == InvoiceId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get Membership_parent information by specific id
        /// </summary>
        /// <param name="Membership_parent">Id of Membership_parent</param>
        /// <returns></returns>
        public Membership_parent GetMembership_parentById(Int64 Id)
        {
            return Context.Membership_parent.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<Membership_parent> GetListMembership_parentById(Int64 Id)
        {
            return Context.Membership_parent.Where(item => item.Id == Id).ToList();
        }
        public List<Membership_parent> GetListMembership_parentByIdcustomer(Int64 Id)
        {
            return Context.Membership_parent.Where(item => item.CustomerId == Id).ToList();
        }
        public vwMembership_parent GetvwMembership_parentById(Int64 Id)
        {
            return Context.vwMembership_parent.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert Membership_parent into database
        /// </summary>
        /// <param name="Membership_parent">Object infomation</param>
        public void InsertMembership_parent(Membership_parent Membership_parent)
        {
            Context.Membership_parent.Add(Membership_parent);
            Context.Entry(Membership_parent).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete Membership_parent with specific id
        /// </summary>
        /// <param name="Id">Membership_parent Id</param>
        public void DeleteMembership_parent(Int64 Id)
        {
            Membership_parent deletedMembership_parent = GetMembership_parentById(Id);
            Context.Membership_parent.Remove(deletedMembership_parent);
            Context.Entry(deletedMembership_parent).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a Membership_parent with its Id and Update IsDeleted IF that Membership_parent has relationship with others
        /// </summary>
        /// <param name="Membership_parentId">Id of Membership_parent</param>
        public void DeleteMembership_parentRs(Int64 Id)
        {
            Membership_parent deleteMembership_parentRs = GetMembership_parentById(Id);
            deleteMembership_parentRs.IsDeleted = true;
            UpdateMembership_parent(deleteMembership_parentRs);
        }

        /// <summary>
        /// Update Membership_parent into database
        /// </summary>
        /// <param name="Membership_parent">Membership_parent object</param>
        public void UpdateMembership_parent(Membership_parent Membership_parent)
        {
            Context.Entry(Membership_parent).State = EntityState.Modified;
            Context.SaveChanges();
        }
      
    }
}
