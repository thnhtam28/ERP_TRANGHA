using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Crm.Repositories
{
    public class CALLogRepository : GenericRepository<ErpCrmDbContext, CALLog>, ICALLogRepository
    {
        public CALLogRepository(ErpCrmDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all CALLog
        /// </summary>
        /// <returns>CALLog list</returns>
        public IQueryable<CALLog> GetAllCALLog()
        {
            return Context.CALLog.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get CALLog information by specific id
        /// </summary>
        /// <param name="CALLogId">Id of CALLog</param>
        /// <returns></returns>
        public CALLog GetCALLogById(int Id)
        {
            return Context.CALLog.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public CALLog GetCALLogByKeyLog(string keylog)
        {
            return Context.CALLog.SingleOrDefault(item => item.keylog == keylog && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert CALLog into database
        /// </summary>
        /// <param name="CALLog">Object infomation</param>
        public void InsertCALLog(CALLog CALLog)
        {
            Context.CALLog.Add(CALLog);
            Context.Entry(CALLog).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete CALLog with specific id
        /// </summary>
        /// <param name="Id">CALLog Id</param>
        public void DeleteCALLog(int Id)
        {
            CALLog deletedCALLog = GetCALLogById(Id);
            Context.CALLog.Remove(deletedCALLog);
            Context.Entry(deletedCALLog).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a CALLog with its Id and Update IsDeleted IF that CALLog has relationship with others
        /// </summary>
        /// <param name="AnswerId">Id of CALLog</param>
        public void DeleteCALLogRs(int Id)
        {
            CALLog deleteAnswerRs = GetCALLogById(Id);
            deleteAnswerRs.IsDeleted = true;
            UpdateCALLog(deleteAnswerRs);
        }

        /// <summary>
        /// Update CALLog into database
        /// </summary>
        /// <param name="CALLog">CALLog object</param>
        public void UpdateCALLog(CALLog Answer)
        {
            Context.Entry(Answer).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
