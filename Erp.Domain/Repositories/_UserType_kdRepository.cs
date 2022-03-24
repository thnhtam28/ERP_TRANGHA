using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erp.Domain.Entities;
using Erp.Domain.Interfaces;
using System.Data.Entity;

namespace Erp.Domain.Repositories
{
    public class UserType_kdRepository : GenericRepository<ErpDbContext, UserType_kd>, IUserType_kdRepository
    {
        public UserType_kdRepository(ErpDbContext context)
            : base(context)
        {
        }

        #region IUserType_kdRepository Members

        public IQueryable<Entities.UserType_kd> GetUserTypes()
        {
            return Context.UserType_kd.Where(u => u.Id != 0).OrderBy(u => u.OrderNo);
        }
        public IQueryable<Entities.vwUserType_kd> GetvwUserTypes()
        {
            return Context.vwUserType_kd.Where(u => u.Id != 0).OrderBy(u => u.OrderNo);
        }
        public Entities.UserType_kd GetUserTypeById(int userTypeId)
        {
            if (userTypeId == 0)
            {
                return null;
            }
            return Context.UserType_kd.FirstOrDefault(u => u.Id == userTypeId);
        }
        public Entities.vwUserType_kd GetvwUserTypeById(int userTypeId)
        {
            if (userTypeId == 0)
            {
                return null;
            }
            return Context.vwUserType_kd.FirstOrDefault(u => u.Id == userTypeId);
        }
        public Entities.UserType_kd GetUserTypeByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            return Context.UserType_kd.FirstOrDefault(u => u.Code == code);
        }
        public Entities.vwUserType_kd GetvwUserTypeByCode(string code)
        {
            if (string.IsNullOrEmpty(code))
            {
                return null;
            }
            return Context.vwUserType_kd.FirstOrDefault(u => u.Code == code);
        }
        public void InsertUserType(Entities.UserType_kd userType_kd)
        {
            Context.UserType_kd.Add(userType_kd);

            //=========== Database ======//
            Context.Entry(userType_kd).State = EntityState.Added;
            Context.SaveChanges();
        }
        public void InsertvwUserType(Entities.vwUserType_kd userType)
        {
            Context.vwUserType_kd.Add(userType);

            //=========== Database ======//
            Context.Entry(userType).State = EntityState.Added;
            Context.SaveChanges();
        }
        public void DeleteUserType(int userTypeId)
        {
            UserType_kd userType_kd = Context.UserType_kd.FirstOrDefault(u => u.Id == userTypeId);
            Context.UserType_kd.Remove(userType_kd);

            //===== Database ========//

            Context.Entry(userType_kd).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        public void DeletevwUserType(int userTypeId)
        {
            vwUserType_kd userType = Context.vwUserType_kd.FirstOrDefault(u => u.Id == userTypeId);
            Context.vwUserType_kd.Remove(userType);

            //===== Database ========//

            Context.Entry(userType).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        public void UpdateUserType(Entities.UserType_kd userType_kd)
        {
            Context.Entry(userType_kd).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void UpdatevwUserType(Entities.vwUserType_kd userType)
        {
            Context.Entry(userType).State = EntityState.Modified;
            Context.SaveChanges();
        }
        public void Save()
        {
            Context.SaveChanges();
        }

        #endregion
    }
}
