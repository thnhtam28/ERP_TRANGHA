using Erp.Domain.Sale.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Sale.Interfaces
{
    public interface IProductSampleRepository
    {
        /// <summary>
        /// Get all ProductSample
        /// </summary>
        /// <returns>ProductSample list</returns>
        IQueryable<ProductSample> GetAllProductSample();
        IQueryable<vwProductSample> GetvwAllProductSample();
        /// <summary>
        /// Get ProductSample information by specific id
        /// </summary>
        /// <param name="Id">Id of ProductSample</param>
        /// <returns></returns>
        ProductSample GetProductSampleById(int Id);
        vwProductSample GetvwProductSampleById(int Id);
        //vwSale_ProductSampleDetail GetvwProductSampleById(int Id);
        /// <summary>
        /// Get ProductSample by Branch
        /// </summary>
        /// <param name="Id">CreatUserId of Branch</param>
        /// <returns></returns>
        // IQueryable<Product> GetAllProductZeroPrice();//lấy danh sách sản phẩm tặng theo nhân viên chi nhánh
        /// <summary>
        /// Insert ProductSample into database
        /// </summary>
        /// <param name="ProductSample">Object infomation</param>
        void InsertProductSample(ProductSample ProductSample);
        
        /// <summary>
        /// Delete ProductSample with specific id
        /// </summary>
        /// <param name="Id">ProductSample Id</param>
        void DeleteProductSample(int Id);
      

        /// <summary>
        /// Delete a ProductSample with its Id and Update IsDeleted IF that ProductSample has relationship with others
        /// </summary>
        /// <param name="Id">Id of ProductSample</param>
        void DeleteProductSampleRs(int Id);

        /// <summary>
        /// Update ProductSample into database
        /// </summary>
        /// <param name="ProductSample">ProductSample object</param>
        void UpdateProductSample(ProductSample ProductSample);
    }
}
