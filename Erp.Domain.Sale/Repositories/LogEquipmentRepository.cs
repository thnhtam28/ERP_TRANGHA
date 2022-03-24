using Erp.Domain.Sale.Entities;
using Erp.Domain.Sale.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Sale.Repositories
{
    public class LogEquipmentRepository : GenericRepository<ErpSaleDbContext, LogEquipment>, ILogEquipmentRepository
    {
        public LogEquipmentRepository(ErpSaleDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all LogEquipment
        /// </summary>
        /// <returns>LogEquipment list</returns>
        public IQueryable<LogEquipment> GetAllLogEquipment()
        {
            return Context.LogEquipment.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<LogEquipment> GetListAllLogEquipment()
        {
            return Context.LogEquipment.Where(item => (item.IsDeleted == null || item.IsDeleted == false)).ToList();
        }
        public IQueryable<vwLogEquipment> GetvwAllLogEquipment()
        {
            return Context.vwLogEquipment.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get LogEquipment information by specific id
        /// </summary>
        /// <param name="LogEquipmentId">Id of LogEquipment</param>
        /// <returns></returns>
        public LogEquipment GetLogEquipmentById(int Id)
        {
            return Context.LogEquipment.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        public vwLogEquipment GetvwLogEquipmentById(int Id)
        {
            return Context.vwLogEquipment.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Insert LogEquipment into database
        /// </summary>
        /// <param name="LogEquipment">Object infomation</param>
        public void InsertLogEquipment(LogEquipment LogEquipment)
        {
            Context.LogEquipment.Add(LogEquipment);
            Context.Entry(LogEquipment).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete LogEquipment with specific id
        /// </summary>
        /// <param name="Id">LogEquipment Id</param>
        public void DeleteLogEquipment(int Id)
        {
            LogEquipment deletedLogEquipment = GetLogEquipmentById(Id);
            Context.LogEquipment.Remove(deletedLogEquipment);
            Context.Entry(deletedLogEquipment).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a LogEquipment with its Id and Update IsDeleted IF that LogEquipment has relationship with others
        /// </summary>
        /// <param name="LogEquipmentId">Id of LogEquipment</param>
        public void DeleteLogEquipmentRs(int Id)
        {
            LogEquipment deleteLogEquipmentRs = GetLogEquipmentById(Id);
            deleteLogEquipmentRs.IsDeleted = true;
            UpdateLogEquipment(deleteLogEquipmentRs);
        }

        /// <summary>
        /// Update LogEquipment into database
        /// </summary>
        /// <param name="LogEquipment">LogEquipment object</param>
        public void UpdateLogEquipment(LogEquipment LogEquipment)
        {
            Context.Entry(LogEquipment).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
