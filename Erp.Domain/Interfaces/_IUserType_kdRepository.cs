using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Erp.Domain.Entities;

namespace Erp.Domain.Interfaces
{
    public interface IUserType_kdRepository
    {
        IQueryable<UserType_kd> GetUserTypes();
        UserType_kd GetUserTypeById(int userTypeId);
        void InsertUserType(UserType_kd userType);
        void DeleteUserType(int userTypeId);
        void UpdateUserType(UserType_kd userType);
        IQueryable<vwUserType_kd> GetvwUserTypes();
        vwUserType_kd GetvwUserTypeById(int userTypeId);
        void InsertvwUserType(vwUserType_kd userType);
        void DeletevwUserType(int userTypeId);
        void UpdatevwUserType(vwUserType_kd userType);
        void Save();

        UserType_kd GetUserTypeByCode(string code);
        vwUserType_kd GetvwUserTypeByCode(string code);
    }
}
