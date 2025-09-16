using System.Collections.Generic;
using System.Threading.Tasks;
using HomeApi.Data.Models;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Интерфейс определяет методы для доступа к объектам типа Room в базе 
    /// </summary>
    public interface IRoomRepository
    {
        Task<Room> GetRoomByName(string name);
        Task AddRoom(Room room);
        Task<IEnumerable<Room>> GetAllRooms();
        Task DeleteRoom(Room room);
    }
}