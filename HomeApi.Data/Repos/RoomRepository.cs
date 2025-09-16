using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Room" в базе
    /// </summary>
    public class RoomRepository : IRoomRepository
    {
        private readonly HomeApiContext _context;
        
        public RoomRepository (HomeApiContext context)
        {
            _context = context;
        }
        
        /// <summary>
        ///  Найти комнату по имени
        /// </summary>
        public async Task<Room> GetRoomByName(string name)
        {
            // Принудительно игнорировать кэш контекста
            return await _context.Rooms
                .Where(r => r.Name == name)
                .FirstOrDefaultAsync();
        }
        
        /// <summary>
        ///  Добавить новую комнату
        /// </summary>
        public async Task AddRoom(Room room)
        {
            await _context.Rooms.AddAsync(room);
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Room>> GetAllRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        public async Task DeleteRoom(Room room)
        {
            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();
        }
    }
}