using Erp.Domain.Staff.Entities;
using System.Collections.Generic;
using System.Linq;

namespace Erp.Domain.Staff.Interfaces
{
    public interface IRoomRepository
    {
        /// <summary>
        /// Get all Room
        /// </summary>
        /// <returns>Room list</returns>
        IQueryable<Room> GetAllRoom();
        List<Room> GetListAllRoom();
        /// <summary>
        /// Get Room information by specific id
        /// </summary>
        /// <param name="Id">Id of Room</param>
        /// <returns></returns>
        Room GetRoomById(int Id);

        /// <summary>
        /// Insert Room into database
        /// </summary>
        /// <param name="Room">Object infomation</param>
        void InsertRoom(Room Room);

        /// <summary>
        /// Delete Room with specific id
        /// </summary>
        /// <param name="Id">Room Id</param>
        void DeleteRoom(int Id);

        /// <summary>
        /// Delete a Room with its Id and Update IsDeleted IF that Room has relationship with others
        /// </summary>
        /// <param name="Id">Id of Room</param>
        void DeleteRoomRs(int Id);

        /// <summary>
        /// Update Room into database
        /// </summary>
        /// <param name="Room">Room object</param>
        void UpdateRoom(Room Room);
    }
}
