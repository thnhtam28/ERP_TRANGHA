using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IInquiryCardRepository
    {
        /// <summary>
        /// Get all InquiryCard
        /// </summary>
        /// <returns>InquiryCard list</returns>
        IQueryable<InquiryCard> GetAllInquiryCard();
        IQueryable<vwInquiryCard> GetvwAllInquiryCard();
        /// <summary>
        /// Get InquiryCard information by specific id
        /// </summary>
        /// <param name="Id">Id of InquiryCard</param>
        /// <returns></returns>
        InquiryCard GetInquiryCardById(int Id);
        vwInquiryCard GetvwInquiryCardById(int Id);
        vwInquiryCard GetvwInquiryCardByCode(string code);
        /// <summary>
        /// Insert InquiryCard into database
        /// </summary>
        /// <param name="InquiryCard">Object infomation</param>
        void InsertInquiryCard(InquiryCard InquiryCard);

        /// <summary>
        /// Delete InquiryCard with specific id
        /// </summary>
        /// <param name="Id">InquiryCard Id</param>
        void DeleteInquiryCard(int Id);

        /// <summary>
        /// Delete a InquiryCard with its Id and Update IsDeleted IF that InquiryCard has relationship with others
        /// </summary>
        /// <param name="Id">Id of InquiryCard</param>
        void DeleteInquiryCardRs(int Id);

        /// <summary>
        /// Update InquiryCard into database
        /// </summary>
        /// <param name="InquiryCard">InquiryCard object</param>
        void UpdateInquiryCard(InquiryCard InquiryCard);
    }
}
