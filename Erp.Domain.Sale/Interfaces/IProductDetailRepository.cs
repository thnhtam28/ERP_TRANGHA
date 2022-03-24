using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IProductDetailRepository
    {
        /// <summary>
        /// Get all ProductDetail
        /// </summary>
        /// <returns>ProductDetail list</returns>
        IQueryable<ProductDetail> GetAllProductDetail();
        IQueryable<vwProductDetail> GetvwAllProductDetail();
        /// <summary>
        /// Get ProductDetail information by specific id
        /// </summary>
        /// <param name="Id">Id of ProductDetail</param>
        /// <returns></returns>
        ProductDetail GetProductDetailById(int Id);
        vwProductDetail GetvwProductDetailById(int Id);
        /// <summary>
        /// Insert ProductDetail into database
        /// </summary>
        /// <param name="ProductDetail">Object infomation</param>
        void InsertProductDetail(ProductDetail ProductDetail);

        /// <summary>
        /// Delete ProductDetail with specific id
        /// </summary>
        /// <param name="Id">ProductDetail Id</param>
        void DeleteProductDetail(int Id);

        /// <summary>
        /// Delete a ProductDetail with its Id and Update IsDeleted IF that ProductDetail has relationship with others
        /// </summary>
        /// <param name="Id">Id of ProductDetail</param>
        void DeleteProductDetailRs(int Id);

        /// <summary>
        /// Update ProductDetail into database
        /// </summary>
        /// <param name="ProductDetail">ProductDetail object</param>
        void UpdateProductDetail(ProductDetail ProductDetail);
        ProductDetail GetProductDetailByProductId(int ProductId);
        
    }
}
