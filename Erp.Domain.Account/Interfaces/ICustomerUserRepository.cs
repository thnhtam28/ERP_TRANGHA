using Erp.Domain.Account.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Account.Interfaces
{
    public interface ICustomerUserRepository
    {
        /// <summary>
        /// Get all CustomerUser
        /// </summary>
        /// <returns>CustomerUser list</returns>
        IQueryable<CustomerUser> GetAllCustomerUser();

        /// <summary>
        /// Get CustomerUser information by specific id
        /// </summary>
        /// <param name="Id">Id of CustomerUser</param>
        /// <returns></returns>
        CustomerUser GetCustomerUserById(int Id);

        /// <summary>
        /// Get All CustomerUser information by specific Customerid
        /// </summary>
        /// <param name="Id">CustomerId of CustomerUser</param>
        /// <returns></returns>
        IQueryable<CustomerUser> GetAllCustomerUserByCustomerId(int CustomerId);

        IQueryable<vwCustomerUser> GetAllvwCustomerUser();
        IQueryable<vwCustomerUser> GetAllvwCustomerUserByCustomerId(int CustomerId);
        vwCustomerUser GetvwCustomerUserById(int Id);
        /// <summary>
        /// Insert CustomerUser into database
        /// </summary>
        /// <param name="CustomerUser">Object infomation</param>
        void InsertCustomerUser(CustomerUser CustomerUser);

        /// <summary>
        /// Delete CustomerUser with specific id
        /// </summary>
        /// <param name="Id">CustomerUser Id</param>
        void DeleteCustomerUser(int Id);

        /// <summary>
        /// Delete a CustomerUser with its Id and Update IsDeleted IF that CustomerUser has relationship with others
        /// </summary>
        /// <param name="Id">Id of CustomerUser</param>
        void DeleteCustomerUserRs(int Id);

        /// <summary>
        /// Update CustomerUser into database
        /// </summary>
        /// <param name="CustomerUser">CustomerUser object</param>
        void UpdateCustomerUser(CustomerUser CustomerUser);
    }
}
