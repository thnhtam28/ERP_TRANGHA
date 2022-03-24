using Erp.Domain.Staff.Entities;
using Erp.Domain.Staff.Interfaces;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity;
using System.Linq;

namespace Erp.Domain.Staff.Repositories
{
    public class RoomRepository : GenericRepository<ErpStaffDbContext, Room>, IRoomRepository
    {
        public RoomRepository(ErpStaffDbContext context)
            : base(context)
        {

        }

        /// <summary>
        /// Get all Room
        /// </summary>
        /// <returns>Room list</returns>
        public IQueryable<Room> GetAllRoom()
        {
            return Context.Room.Where(item => (item.IsDeleted == null || item.IsDeleted == false));
        }
        public List<Room> GetListAllRoom()
        {
            return Context.Room.Where(item => (item.IsDeleted == null || item.IsDeleted == false)).ToList();
        }
        /// <summary>
        /// Get Room information by specific id
        /// </summary>
        /// <param name="RoomId">Id of Room</param>
        /// <returns></returns>
        public Room GetRoomById(int Id)
        {
            return Context.Room.SingleOrDefault(item => item.Id == Id && (item.IsDeleted == null || item.IsDeleted == false));
        }

        /// <summary>
        /// Insert Room into database
        /// </summary>
        /// <param name="Room">Object infomation</param>
        public void InsertRoom(Room Room)
        {
            Context.Room.Add(Room);
            Context.Entry(Room).State = EntityState.Added;
            Context.SaveChanges();
        }

        /// <summary>
        /// Delete Room with specific id
        /// </summary>
        /// <param name="Id">Room Id</param>
        public void DeleteRoom(int Id)
        {
            Room deletedRoom = GetRoomById(Id);
            Context.Room.Remove(deletedRoom);
            Context.Entry(deletedRoom).State = EntityState.Deleted;
            Context.SaveChanges();
        }
        
        /// <summary>
        /// Delete a Room with its Id and Update IsDeleted IF that Room has relationship with others
        /// </summary>
        /// <param name="RoomId">Id of Room</param>
        public void DeleteRoomRs(int Id)
        {
            Room deleteRoomRs = GetRoomById(Id);
            deleteRoomRs.IsDeleted = true;
            UpdateRoom(deleteRoomRs);
        }

        /// <summary>
        /// Update Room into database
        /// </summary>
        /// <param name="Room">Room object</param>
        public void UpdateRoom(Room Room)
        {
            Context.Entry(Room).State = EntityState.Modified;
            Context.SaveChanges();
        }
    }
}
