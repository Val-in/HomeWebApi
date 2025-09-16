using System;
using System.Linq;
using System.Threading.Tasks;
using HomeApi.Data.Models;
using HomeApi.Data.Queries;
using Microsoft.EntityFrameworkCore;

namespace HomeApi.Data.Repos
{
    /// <summary>
    /// Репозиторий для операций с объектами типа "Device" в базе
    /// </summary>
    public class DeviceRepository : IDeviceRepository
    {
        private readonly HomeApiContext _context;
        
        public DeviceRepository (HomeApiContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Выгрузить все устройства
        /// </summary>
        public async Task<Device[]> GetDevices()
        {
            return await _context.Devices
                .Include( d => d.Room)
                .ToArrayAsync();
        }

        /// <summary>
        /// Найти устройство по имени
        /// </summary>
        public async Task<Device> GetDeviceByName(string name)
        {
            return await _context.Devices
                .Include( d => d.Room)
                .Where(d => d.Name == name).FirstOrDefaultAsync();
        }

        /// <summary>
        /// Найти устройство по идентификатору
        /// </summary>
        public async Task<Device> GetDeviceById(Guid id)
        {
            var device = await _context.Devices
                .Include(d => d.Room)
                .Where(d => d.Id == id)
                .FirstOrDefaultAsync();

            Console.WriteLine(device == null ? "Не найдено" : $"Найдено: {device.Name}");
            return device;
        }
        
        /// <summary>
        /// Добавить новое устройство
        /// </summary>
        public async Task SaveDevice(Device device, Room room)
        {
            _context.Rooms.Attach(room);  // EF Core понимает, что это существующая сущность
            // Привязываем новое устройство к соответствующей комнате перед сохранением
            device.RoomId = room.Id;
            device.Room = room;
            
            await _context.Devices.AddAsync(device);
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Обновить существующее устройство
        /// </summary>
        public async Task UpdateDevice(Device device, Room room, UpdateDeviceQuery query)
        {
            device.RoomId = room.Id; // ссылка по ключу
            
            if (!string.IsNullOrEmpty(query.NewName))
                device.Name = query.NewName;
            if (!string.IsNullOrEmpty(query.NewSerial))
                device.SerialNumber = query.NewSerial;
            if (!string.IsNullOrEmpty(query.NewManufacturer))
                device.Manufacturer = query.NewManufacturer;
            
            // Неподходящий вариант
            // var entry = _context.Entry(device);
            // if (entry.State == EntityState.Detached)
            //     _context.Devices.Update(device);
            
            _context.Devices.Update(device); // обновляем Device
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Удалить устройство
        /// </summary>
        public async Task DeleteDevice(Device device)
        {
            _context.Devices.Remove(device);
            await _context.SaveChangesAsync();
        }

        public async Task PutDevice(Device device, Room room, UpdateDeviceQuery query)
        {
            device.RoomId = room.Id; // ссылка по ключу
            
            if (!string.IsNullOrEmpty(query.NewName))
                device.Name = query.NewName;
            if (!string.IsNullOrEmpty(query.NewSerial))
                device.SerialNumber = query.NewSerial;
            if (!string.IsNullOrEmpty(query.NewRoomLocation))
                device.Location = query.NewRoomLocation;
            if (!string.IsNullOrEmpty(query.NewModel))
                device.Model = query.NewModel;
            if (!string.IsNullOrEmpty(query.NewManufacturer))
                device.Manufacturer = query.NewManufacturer;
            if (!int.IsNegative(query.NewCurrentVolts))
                device.CurrentVolts = query.NewCurrentVolts;
            device.GasUsage = query.NewGasUsage;
            
            _context.Devices.Update(device); // обновляем Device
            await _context.SaveChangesAsync();
        }
    }
}