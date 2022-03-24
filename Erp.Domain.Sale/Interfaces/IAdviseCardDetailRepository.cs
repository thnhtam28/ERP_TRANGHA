using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IAdviseCardDetailRepository
    {
        /// <summary>
        /// Get all AdviseCardDetail
        /// </summary>
        /// <returns>AdviseCardDetail list</returns>
        IQueryable<AdviseCardDetail> GetAllAdviseCardDetail();
        IQueryable<AdviseCardDetail> GetAllAdviseCardDetailById(int AdviseCardId);
        /// <summary>
        /// Get AdviseCardDetail information by specific id
        /// </summary>
        /// <param name="Id">Id of AdviseCardDetail</param>
        /// <returns></returns>
        AdviseCardDetail GetAdviseCardDetailById(int Id);
        AdviseCardDetail GetAdviseCardDetailById(int AdviseCardId, int QuestionId, int AnswerId);
        /// <summary>
        /// Insert AdviseCardDetail into database
        /// </summary>
        /// <param name="AdviseCardDetail">Object infomation</param>
        void InsertAdviseCardDetail(AdviseCardDetail AdviseCardDetail);

        /// <summary>
        /// Delete AdviseCardDetail with specific id
        /// </summary>
        /// <param name="Id">AdviseCardDetail Id</param>
        void DeleteAdviseCardDetail(int Id);

        /// <summary>
        /// Delete a AdviseCardDetail with its Id and Update IsDeleted IF that AdviseCardDetail has relationship with others
        /// </summary>
        /// <param name="Id">Id of AdviseCardDetail</param>
        void DeleteAdviseCardDetailRs(int Id);

        /// <summary>
        /// Update AdviseCardDetail into database
        /// </summary>
        /// <param name="AdviseCardDetail">AdviseCardDetail object</param>
        void UpdateAdviseCardDetail(AdviseCardDetail AdviseCardDetail);
    }
}
