using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace HomeApi.Data.Models
{
    [Table("Rooms")]
    public class Room
    {
        public Guid  Id { get; set; } = Guid.NewGuid();
        public DateTime AddDate { get; set; } = DateTime.UtcNow; // то есть не локальное время, а по UTC
        public string Name { get; set; }
        public int Area { get; set; }
        public bool GasConnected { get; set; }
        public int Voltage { get; set; }
    }
}