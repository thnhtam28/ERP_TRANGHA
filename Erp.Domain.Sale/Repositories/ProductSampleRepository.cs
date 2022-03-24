using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;
using System;

namespace Erp.Domain.Sale.Repositories
{
    public class ProductSampleRepository : GenericRepository<ErpSaleDbContext, ProductSample>, IProductSampleRepository
    {
        public ProductSampleRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all ProductSample
        /// </summary>
        /// <returns>ProductSample list</returns>
        public IQueryable<ProductSample> GetAllProductSample()
        {
            return Context.ProductSample.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwProductSample> GetvwAllProductSample()
        {
            return Context.vwProductSample.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get ProductSample information by specific id
        /// </summary>
        /// <param name="ProductSampleId">Id of ProductSample</param>
        /// <returns></returns>
        public ProductSample GetProductSampleById(int Id)
        {
            return Context.ProductSample.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwProductSample GetvwProductSampleById(int Id)
        {
            return Context.vwProductSample.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert ProductSample into database
        /// </summary>
        /// <param name="ProductSample">Object infomation</param>
        public void InsertProductSample(ProductSample ProductSample)
        {
            Context.ProductSample.Add(ProductSample);
            Context.Entry(ProductSample).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete ProductSample with specific id
        /// </summary>
        /// <param name="Id">ProductSample Id</param>
        public void DeleteProductSample(int Id)
        {
            ProductSample deletedProductSample = GetProductSampleById(Id);
            Context.ProductSample.Remove(deletedProductSample);
            Context.Entry(deletedProductSample).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a ProductSample with its Id and Update IsDeleted IF that ProductSample has relationship with others
        /// </summary>
        /// <param name="ProductSampleId">Id of ProductSample</param>
        public void DeleteProductSampleRs(int Id)
        {
            ProductSample deleteProductSampleRs = GetProductSampleById(Id);
            deleteProductSampleRs.IsDeleted = true;
            UpdateProductSample(deleteProductSampleRs);
        }

        /// <summary>
        /// Update ProductSample into database
        /// </summary>
        /// <param name="ProductSample">ProductSample object</param>
        public void UpdateProductSample(ProductSample ProductSample)
        {
            Context.Entry(ProductSample).State = EntityState.Modified;
            Context.SaveChanges();
        }

    }
}
