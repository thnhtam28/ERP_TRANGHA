using Erp.Domain.Staff.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Interfaces
{
    public interface IStaffEquipmentRepository
    {
        IQueryable<StaffEquipment> GetAllStaffEquipment();
        List<StaffEquipment> GetListAllStaffEquipment();

        List<StaffEquipment> GetlistAllStaffEquiment();
        StaffEquipment GetStaffEquipmentById(int Id);


        void InsertEquipment(StaffEquipment StaffEquipment);


        void DeleteEquipment(int Id);


        void DeleteEquipmentRs(int Id);

        void UpdateEquipment(StaffEquipment StaffEquipment);
    }
}
