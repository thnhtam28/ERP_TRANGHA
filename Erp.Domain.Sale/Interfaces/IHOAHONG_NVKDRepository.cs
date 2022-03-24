using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;


namespace Erp.Domain.Sale.Interfaces
{
     public interface IHOAHONG_NVKDRepository
    {
        /// <summary>
        /// Get all LoyaltyPoint
        /// </summary>
        /// <returns>LoyaltyPoint list</returns>
        IQueryable<HOAHONG_NVKD> GetAllHOAHONG_NVKD();

        /// <summary>
        /// Get LoyaltyPoint information by specific id
        /// </summary>
        /// <param name="Id">Id of LoyaltyPoint</param>
        /// <returns></returns>
        HOAHONG_NVKD GetHOAHONG_NVKDById(int Id);

        /// <summary>
        /// Insert LoyaltyPoint into database
        /// </summary>
        /// <param name="LoyaltyPoint">Object infomation</param>
        void InsertHOAHONG_NVKD(HOAHONG_NVKD HOAHONG_NVKD);

        /// <summary>
        /// Delete LoyaltyPoint with specific id
        /// </summary>
        /// <param name="Id">LoyaltyPoint Id</param>
        void DeleteHOAHONG_NVKD(int Id);

        /// <summary>
        /// Delete a LoyaltyPoint with its Id and Update IsDeleted IF that LoyaltyPoint has relationship with others
        /// </summary>
        /// <param name="Id">Id of LoyaltyPoint</param>
        void DeleteHOAHONG_NVKDRs(int Id);

        /// <summary>
        /// Update LoyaltyPoint into database
        /// </summary>
        /// <param name="HOAHONG_NVKD">LoyaltyPoint object</param>
        void UpdateHOAHONG_NVKD(HOAHONG_NVKD HOAHONG_NVKD);
    }
}
