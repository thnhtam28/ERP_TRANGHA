using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IProductSampleDetailRepository
    {
        /// <summary>
        /// Get all ProductSampleDetail
        /// </summary>
        /// <returns>ProductSampleDetail list</returns>
        IQueryable<ProductSampleDetail> GetAllProductSampleDetail();
        IQueryable<ProductSampleDetail> GetAllProductSampleDetailByProductSampleId(int Id);
        IQueryable<vwProductSampleDetail> GetvwAllProductSampleDetail();
        /// <summary>
        /// Get ProductSampleDetail information by specific id
        /// </summary>
        /// <param name="Id">Id of ProductSampleDetail</param>
        /// <returns></returns>
        ProductSampleDetail GetProductSampleDetailById(int Id);
        vwProductSampleDetail GetvwProductSampleDetailById(int Id);
        /// <summary>
        /// Insert ProductSampleDetail into database
        /// </summary>
        /// <param name="ProductSampleDetail">Object infomation</param>
        void InsertProductSampleDetail(ProductSampleDetail ProductSampleDetail);

        /// <summary>
        /// Delete ProductSampleDetail with specific id
        /// </summary>
        /// <param name="Id">ProductSampleDetail Id</param>
        void DeleteProductSampleDetail(int Id);

        /// <summary>
        /// Delete a ProductSampleDetail with its Id and Update IsDeleted IF that ProductSampleDetail has relationship with others
        /// </summary>
        /// <param name="Id">Id of ProductSampleDetail</param>
        void DeleteProductSampleDetailRs(int Id);

        /// <summary>
        /// Update ProductSampleDetail into database
        /// </summary>
        /// <param name="ProductSampleDetail">ProductSampleDetail object</param>
        void UpdateProductSampleDetail(ProductSampleDetail ProductSampleDetail);
        
    }
}
