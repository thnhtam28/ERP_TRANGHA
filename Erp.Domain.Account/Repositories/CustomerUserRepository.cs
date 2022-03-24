using Erp.Domain.Account.Entities;
using Erp.Domain.Account.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Account.Repositories
{
    public class CustomerUserRepository : GenericRepository<ErpAccountDbContext, CustomerUser>, ICustomerUserRepository
    {
        public CustomerUserRepository(ErpAccountDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all CustomerUser
        /// </summary>
        /// <returns>CustomerUser list</returns>
        public IQueryable<CustomerUser> GetAllCustomerUser()
        {
            return Context.CustomerUser.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<CustomerUser> GetAllCustomerUserByCustomerId(int CustomerId)
        {
            return Context.CustomerUser.Where(item => item.CustomerId == CustomerId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwCustomerUser> GetAllvwCustomerUser()
        {
            return Context.vwCustomerUser.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public IQueryable<vwCustomerUser> GetAllvwCustomerUserByCustomerId(int CustomerId)
        {
            return Context.vwCustomerUser.Where(item => item.CustomerId == CustomerId && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get CustomerUser information by specific id
        /// </summary>
        /// <param name="CustomerUserId">Id of CustomerUser</param>
        /// <returns></returns>
        public CustomerUser GetCustomerUserById(int Id)
        {
            return Context.CustomerUser.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwCustomerUser GetvwCustomerUserById(int Id)
        {
            return Context.vwCustomerUser.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert CustomerUser into database
        /// </summary>
        /// <param name="CustomerUser">Object infomation</param>
        public void InsertCustomerUser(CustomerUser CustomerUser)
        {
            Context.CustomerUser.Add(CustomerUser);
            Context.Entry(CustomerUser).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete CustomerUser with specific id
        /// </summary>
        /// <param name="Id">CustomerUser Id</param>
        public void DeleteCustomerUser(int Id)
        {
            CustomerUser deletedCustomerUser = GetCustomerUserById(Id);
            Context.CustomerUser.Remove(deletedCustomerUser);
            Context.Entry(deletedCustomerUser).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a CustomerUser with its Id and Update IsDeleted IF that CustomerUser has relationship with others
        /// </summary>
        /// <param name="CustomerUserId">Id of CustomerUser</param>
        public void DeleteCustomerUserRs(int Id)
        {
            CustomerUser deleteCustomerUserRs = GetCustomerUserById(Id);
            deleteCustomerUserRs.IsDeleted = true;
            UpdateCustomerUser(deleteCustomerUserRs);
        }

        /// <summary>
        /// Update CustomerUser into database
        /// </summary>
        /// <param name="CustomerUser">CustomerUser object</param>
        public void UpdateCustomerUser(CustomerUser CustomerUser)
        {
            Context.Entry(CustomerUser).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
