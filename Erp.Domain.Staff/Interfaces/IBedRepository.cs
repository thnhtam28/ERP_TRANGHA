using Erp.Domain.Staff.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Erp.Domain.Staff.Interfaces
{
    public interface IBedRepository
    {
        List<Bed> GetAllBed();
        List<Bed> GetAllBedbyIdRoom(int Id);


        Bed GetBedById(int Id);

       
        void InsertBed(Bed bed);

     
        void DeleteBed(int Id);

     
        void DeleteBedRs(int Id);

      
        void UpdateBed(Bed bed);
    }
}
