using System;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using HomeApi.Contracts.Models.Rooms;
using HomeApi.Data.Models;
using HomeApi.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace HomeApi.Controllers
{
    /// <summary>
    /// Контроллер комнат
    /// </summary>
    [ApiController]
    [Route("[controller]")]
    public class RoomsController : ControllerBase
    {
        private IRoomRepository _repository;
        private IMapper _mapper;
        
        public RoomsController(IRoomRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Получение всех комнат
        /// </summary>
        [HttpGet("getRooms")]
        public async Task<IActionResult> GetRooms()
        {
            var rooms = await _repository.GetAllRooms();
            if (rooms == null || !rooms.Any())
                return NotFound("Нет комнат.");
            
            var roomViews = _mapper.Map<RoomView[]>(rooms);

            return Ok(roomViews);
        }


        /// <summary>
        /// Добавление комнаты
        /// </summary>
        [HttpPost("addRoom")]
        public async Task<IActionResult> Add([FromBody] AddRoomRequest request)
        {
            var existingRoom = await _repository.GetRoomByName(request.Name);
            if (existingRoom == null)
            {
                var newRoom = _mapper.Map<AddRoomRequest, Room>(request);
                await _repository.AddRoom(newRoom);
                return StatusCode(201, $"Комната {request.Name} добавлена!");
            }
            
            return StatusCode(409, $"Ошибка: Комната {request.Name} уже существует.");
        }
    }
}