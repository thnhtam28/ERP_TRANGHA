using Erp.Domain.Crm.Entities;
using Erp.Domain.Crm.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Crm.Repositories
{
    public class KH_TUONGTACRepository : GenericRepository<ErpCrmDbContext, KH_TUONGTAC>, IKH_TUONGTACRepository
    {
        public KH_TUONGTACRepository(ErpCrmDbContext context)
           : base(context)
        {

        }

        /// <summary>
        /// Get all KH_TUONGTAC
        /// </summary>
        /// <returns>KH_TUONGTAC list</returns>
        public IQueryable<KH_TUONGTAC> GetAllKH_TUONGTAC()
        {
            return Context.KH_TUONGTAC.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public IQueryable<vwCRM_TK_TUONGTAC> GetAllvwCRM_TK_TUONGTAC()
        {
            return Context.vwCRM_TK_TUONGTAC;
        }
        public vwCRM_KH_TUONGTAC GetvwKH_TUONGTACById(int Id)
        {
            return Context.vwKH_TUONGTAC.SingleOrDefault(item => item.KH_TUONGTAC_ID == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }


        public IQueryable<vwCRM_KH_TUONGTAC> GetvwKH_TUONGTACByALL()
        {
            return Context.vwKH_TUONGTAC.Where(item =>(item.IsDeleted == null || item.IsDeleted == false));
        }


        public IQueryable<KH_TUONGTAC> GetAllKH_TUONGTACById(int KH_TUONGTAC_ID)
        {
            return Context.KH_TUONGTAC.Where(item => item.KH_TUONGTAC_ID == KH_TUONGTAC_ID && (item.IsDeleted == null || item.IsDeleted == false));
        }
        /// <summary>
        /// Get KH_TUONGTAC information by specific id
        /// </summary>
        /// <param name="KH_TUONGTAC_ID">Id of KH_TUONGTAC</param>
        /// <returns></returns>
        public KH_TUONGTAC GetKH_TUONGTACById(int? KH_TUONGTAC_ID)
        {
            return Context.KH_TUONGTAC.SingleOrDefault(item => item.KH_TUONGTAC_ID == KH_TUONGTAC_ID && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert KH_TUONGTAC into database
        /// </summary>
        /// <param name="KH_TUONGTAC">Object infomation</param>
        public void InsertKH_TUONGTAC(KH_TUONGTAC KH_TUONGTAC)
        {
            Context.KH_TUONGTAC.Add(KH_TUONGTAC);
            Context.Entry(KH_TUONGTAC).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete KH_TUONGTAC with specific id
        /// </summary>
        /// <param name="IdKH_TUONGTAC_ID>EmailLog Id</param>
        public void DeleteKH_TUONGTAC(int KH_TUONGTAC_ID)
        {
            KH_TUONGTAC deletedKH_TUONGTAC = GetKH_TUONGTACById(KH_TUONGTAC_ID);
            Context.KH_TUONGTAC.Remove(deletedKH_TUONGTAC);
            Context.Entry(deletedKH_TUONGTAC).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete a EmailLog with its Id and Update IsDeleted IF that Campaign has relationship with others
        /// </summary>
        /// <param name="KH_TUONGTAC_ID">Id of EmailLog</param>
        public void DeleteKH_TUONGTACRs(int KH_TUONGTAC_ID)
        {
            KH_TUONGTAC deleteKH_TUONGTACRs = GetKH_TUONGTACById(KH_TUONGTAC_ID);
            deleteKH_TUONGTACRs.IsDeleted = true;
            UpdateKH_TUONGTAC(deleteKH_TUONGTACRs);
        }

        /// <summary>
        /// Update KH_TUONGTAC into database
        /// </summary>
        /// <param name="KH_TUONGTAC">KH_TUONGTAC object</param>
        public void UpdateKH_TUONGTAC(KH_TUONGTAC KH_TUONGTAC)
        {
            Context.Entry(KH_TUONGTAC).State = EntityState.Modified;
            Context.SaveChanges();
        }

        /// <summary>
        /// Get all vwEmailLog
        /// </summary>
        /// <returns>vwEmailLog list</returns>
        public IQueryable<vwCRM_KH_TUONGTAC> GetAllvwKH_TUONGTAC()
        {
            return Context.vwKH_TUONGTAC.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Get vwEmailLog information by specific id
        /// </summary>
        /// <param name="vwEmailLogId">Id of vwEmailLog</param>
        /// <returns></returns>
        //public vwEmailLog GetvwEmailLogById(int Id)
        //{
        //    return Context.vwEmailLog.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        //}
    }
}
