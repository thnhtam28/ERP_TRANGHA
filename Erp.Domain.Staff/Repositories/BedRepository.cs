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
    public class BedRepository : GenericRepository<ErpStaffDbContext, Bed>, IBedRepository
    {
        public BedRepository(ErpStaffDbContext context)
            : base(context)
        {

        }
        public void DeleteBed(int Id)
        {
            Bed deletedBed = GetBedById(Id);
            Context.Bed.Remove(deletedBed);
            Context.Entry(deletedBed).State = EntityState.Deleted;
            Context.SaveChanges();
        }

        public void DeleteBedRs(int Id)
        {
            Bed deleteBedRs = GetBedById(Id);
            deleteBedRs.IsDeleted = true;
            UpdateBed(deleteBedRs);
        }

        public List<Bed> GetAllBed()
        {
            return Context.Bed.Where(item => item.IsDeleted == false).ToList();
        }
        public List<Bed> GetAllBedbyIdRoom(int Id)
        {
            return Context.Bed.Where(item => item.Room_Id == Id&&item.IsDeleted !=true).ToList();
        }
        public Bed GetBedById(int Id)
        {
            return Context.Bed.Where(item => item.Bed_ID == Id).FirstOrDefault();
        }

        public void InsertBed(Bed bed)
        {
            Context.Bed.Add(bed);
            Context.Entry(bed).State = EntityState.Added;
            Context.SaveChanges();
        }

        public void UpdateBed(Bed bed)
        {
            Context.Entry(bed).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
