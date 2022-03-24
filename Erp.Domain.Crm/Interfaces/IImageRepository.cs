using Erp.Domain.Crm.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Crm.Interfaces
{
    public interface IImageRepository
    {
        /// <summary>
        /// Get all REProductImage
        /// </summary>
        /// <returns>REProductImage list</returns>
        IQueryable<Image> GetAllImage();

        /// <summary>
        /// Get REProductImage information by specific id
        /// </summary>
        /// <param name="Id">Id of REProductImage</param>
        /// <returns></returns>
        Image GetImageById(int Id);

        /// <summary>
        /// Insert REProductImage into database
        /// </summary>
        /// <param name="REProductImage">Object infomation</param>
        void InsertImage(Image Image);

        /// <summary>
        /// Delete REProductImage with specific id
        /// </summary>
        /// <param name="Id">REProductImage Id</param>
        void DeleteImage(int Id);

        /// <summary>
        /// Delete a REProductImage with its Id and Update IsDeleted IF that REProductImage has relationship with others
        /// </summary>
        /// <param name="Id">Id of REProductImage</param>
        void DeleteImageRs(int Id);

        /// <summary>
        /// Update REProductImage into database
        /// </summary>
        /// <param name="REProductImage">REProductImage object</param>
        void UpdateImage(Image Image);
    }
}
