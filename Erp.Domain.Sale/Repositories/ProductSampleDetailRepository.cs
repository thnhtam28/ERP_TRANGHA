using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;
using System;

namespace Erp.Domain.Sale.Repositories
{
    public class ProductSampleDetailRepository : GenericRepository<ErpSaleDbContext, ProductSampleDetail>, IProductSampleDetailRepository
    {
        public ProductSampleDetailRepository(ErpSaleDbContext context)
            : base(context)
        {

        }
       

        /// <summary>
        /// Get all ProductSampleDetail
        /// </summary>
        /// <returns>ProductSampleDetail list</returns>
        public IQueryable<ProductSampleDetail> GetAllProductSampleDetail()
        {
            return Context.ProductSampleDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwProductSampleDetail> GetvwAllProductSampleDetail()
        {
            return Context.vwProductSampleDetail.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get ProductSampleDetail information by specific id
        /// </summary>
        /// <param name="ProductSampleDetailId">Id of ProductSampleDetail</param>
        /// <returns></returns>
        public ProductSampleDetail GetProductSampleDetailById(int Id)
        {
            return Context.ProductSampleDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwProductSampleDetail GetvwProductSampleDetailById(int Id)
        {
            return Context.vwProductSampleDetail.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert ProductSampleDetail into database
        /// </summary>
        /// <param name="ProductSampleDetail">Object infomation</param>
        public void InsertProductSampleDetail(ProductSampleDetail ProductSampleDetail)
        {
            Context.ProductSampleDetail.Add(ProductSampleDetail);
            Context.Entry(ProductSampleDetail).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete ProductSampleDetail with specific id
        /// </summary>
        /// <param name="Id">ProductSampleDetail Id</param>
        public void DeleteProductSampleDetail(int Id)
        {
            ProductSampleDetail deletedProductSampleDetail = GetProductSampleDetailById(Id);
            Context.ProductSampleDetail.Remove(deletedProductSampleDetail);
            Context.Entry(deletedProductSampleDetail).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a ProductSampleDetail with its Id and Update IsDeleted IF that ProductSampleDetail has relationship with others
        /// </summary>
        /// <param name="ProductSampleDetailId">Id of ProductSampleDetail</param>
        public void DeleteProductSampleDetailRs(int Id)
        {
            ProductSampleDetail deleteProductSampleDetailRs = GetProductSampleDetailById(Id);
            deleteProductSampleDetailRs.IsDeleted = true;
            UpdateProductSampleDetail(deleteProductSampleDetailRs);
        }

        /// <summary>
        /// Update ProductSampleDetail into database
        /// </summary>
        /// <param name="ProductSampleDetail">ProductSampleDetail object</param>
        public void UpdateProductSampleDetail(ProductSampleDetail ProductSampleDetail)
        {
            Context.Entry(ProductSampleDetail).State = EntityState.Modified;
            Context.SaveChanges();
        }

        public IQueryable<ProductSampleDetail> GetAllProductSampleDetailByProductSampleId(int Id)
        {
            return Context.ProductSampleDetail.Where(x => x.ProductSampleId == Id);
        }
    }
}
