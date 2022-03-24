using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Crm.Repositories
{
    public class ImageRepository : GenericRepository<ErpCrmDbContext, Image>, IImageRepository
    {
        public ImageRepository(ErpCrmDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all REProductImage
        /// </summary>
        /// <returns>REProductImage list</returns>
        public IQueryable<Image> GetAllImage()
        {
            return Context.Image.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get REProductImage information by specific id
        /// </summary>
        /// <param name="REProductImageId">Id of REProductImage</param>
        /// <returns></returns>
        public Image GetImageById(int Id)
        {
            return Context.Image.SingleOrDefault(item =>item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert REProductImage into database
        /// </summary>
        /// <param name="REProductImage">Object infomation</param>
        public void InsertImage(Image Image)
        {
            Context.Image.Add(Image);
            Context.Entry(Image).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete REProductImage with specific id
        /// </summary>
        /// <param name="Id">REProductImage Id</param>
        public void DeleteImage(int Id)
        {
            Image deletedImage = GetImageById(Id);
            Context.Image.Remove(deletedImage);
            Context.Entry(deletedImage).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a REProductImage with its Id and Update IsDeleted IF that REProductImage has relationship with others
        /// </summary>
        /// <param name="REProductImageId">Id of REProductImage</param>
        public void DeleteImageRs(int Id)
        {
            Image deleteImageRs = GetImageById(Id);
            deleteImageRs.IsDeleted = true;
            UpdateImage(deleteImageRs);
        }

        /// <summary>
        /// Update REProductImage into database
        /// </summary>
        /// <param name="REProductImage">REProductImage object</param>
        public void UpdateImage(Image Image)
        {
            Context.Entry(Image).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
