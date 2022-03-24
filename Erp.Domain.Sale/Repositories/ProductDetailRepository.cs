using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class ProductDetailRepository : GenericRepository<ErpSaleDbContext, ProductDetail>, IProductDetailRepository
    {
        public ProductDetailRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all ProductDetail
        /// </summary>
        /// <returns>ProductDetail list</returns>
        public IQueryable<ProductDetail> GetAllProductDetail()
        {
            return Context.ProductDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public ProductDetail GetProductDetailByProductId(int ProductId)
        {
            return Context.ProductDetail.Single(item => item.ProductId == ProductId && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwProductDetail> GetvwAllProductDetail()
        {
            return Context.vwProductDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        public vwProductDetail GetvwProductDetailById(int Id)
        {
            return Context.vwProductDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get ProductDetail information by specific id
        /// </summary>
        /// <param name="ProductDetailId">Id of ProductDetail</param>
        /// <returns></returns>
        public ProductDetail GetProductDetailById(int Id)
        {
            return Context.ProductDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert ProductDetail into database
        /// </summary>
        /// <param name="ProductDetail">Object infomation</param>
        public void InsertProductDetail(ProductDetail ProductDetail)
        {
            Context.ProductDetail.Add(ProductDetail);
            Context.Entry(ProductDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete ProductDetail with specific id
        /// </summary>
        /// <param name="Id">ProductDetail Id</param>
        public void DeleteProductDetail(int Id)
        {
            ProductDetail deletedProductDetail = GetProductDetailById(Id);
            Context.ProductDetail.Remove(deletedProductDetail);
            Context.Entry(deletedProductDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a ProductDetail with its Id and Update IsDeleted IF that ProductDetail has relationship with others
        /// </summary>
        /// <param name="ProductDetailId">Id of ProductDetail</param>
        public void DeleteProductDetailRs(int Id)
        {
            ProductDetail deleteProductDetailRs = GetProductDetailById(Id);
            deleteProductDetailRs.IsDeleted = true;
            UpdateProductDetail(deleteProductDetailRs);
        }

        /// <summary>
        /// Update ProductDetail into database
        /// </summary>
        /// <param name="ProductDetail">ProductDetail object</param>
        public void UpdateProductDetail(ProductDetail ProductDetail)
        {
            Context.Entry(ProductDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
