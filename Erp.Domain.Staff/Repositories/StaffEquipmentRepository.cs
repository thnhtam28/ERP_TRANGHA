using Erp.Domain.Staff.Entities;
using Erp.Domain.Staff.Interfaces;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Repositories
{
    public class StaffEquipmentRepository: GenericRepository<ErpStaffDbContext, StaffEquipment>, IStaffEquipmentRepository
    {
        public StaffEquipmentRepository(ErpStaffDbContext context)
            : base(context)
        {

        }

        public void DeleteEquipment(int Id)
        {
            StaffEquipment deletedEquipment = GetStaffEquipmentById(Id);
            Context.StaffEquipment.Remove(deletedEquipment);
            Context.Entry(deletedEquipment).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeleteEquipmentRs(int Id)
        {
            StaffEquipment deleteEquipmentRs = GetStaffEquipmentById(Id);
            deleteEquipmentRs.IsDeleted = true;
            UpdateEquipment(deleteEquipmentRs);
        }

        public IQueryable<StaffEquipment> GetAllStaffEquipment()
        {
            return Context.StaffEquipment.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<StaffEquipment> GetListAllStaffEquipment()
        {
            return Context.StaffEquipment.Where(item => (item.IsDeleted == null|| item.IsDeleted == false)).ToList();
        }
        public List<StaffEquipment> GetlistAllStaffEquiment()
        {
            return Context.StaffEquipment.Where(item => (item.IsDeleted == null || item.IsDeleted == false)).ToList();
        }
        public StaffEquipment GetStaffEquipmentById(int Id)
        {
            return Context.StaffEquipment.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        public void InsertEquipment(StaffEquipment StaffEquipment)
        {
            Context.StaffEquipment.Add(StaffEquipment);
            Context.Entry(StaffEquipment).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void UpdateEquipment(StaffEquipment StaffEquipment)
        {
            Context.Entry(StaffEquipment).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
