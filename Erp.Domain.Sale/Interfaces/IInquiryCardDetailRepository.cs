using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IInquiryCardDetailRepository
    {
        /// <summary>
        /// Get all InquiryCardDetail
        /// </summary>
        /// <returns>InquiryCardDetail list</returns>
        IQueryable<InquiryCardDetail> GetAllInquiryCardDetail();

        /// <summary>
        /// Get InquiryCardDetail information by specific id
        /// </summary>
        /// <param name="Id">Id of InquiryCardDetail</param>
        /// <returns></returns>
        InquiryCardDetail GetInquiryCardDetailById(int Id);

        /// <summary>
        /// Insert InquiryCardDetail into database
        /// </summary>
        /// <param name="InquiryCardDetail">Object infomation</param>
        void InsertInquiryCardDetail(InquiryCardDetail InquiryCardDetail);

        /// <summary>
        /// Delete InquiryCardDetail with specific id
        /// </summary>
        /// <param name="Id">InquiryCardDetail Id</param>
        void DeleteInquiryCardDetail(int Id);

        /// <summary>
        /// Delete a InquiryCardDetail with its Id and Update IsDeleted IF that InquiryCardDetail has relationship with others
        /// </summary>
        /// <param name="Id">Id of InquiryCardDetail</param>
        void DeleteInquiryCardDetailRs(int Id);

        /// <summary>
        /// Update InquiryCardDetail into database
        /// </summary>
        /// <param name="InquiryCardDetail">InquiryCardDetail object</param>
        void UpdateInquiryCardDetail(InquiryCardDetail InquiryCardDetail);
    }
}
