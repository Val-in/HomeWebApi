namespace HomeApi.Data.Queries
{
    /// <summary>
    /// Тут мы передаем в query параметры, которые стоит обновить для устройства в базе. Эти параметры, как правило, необязательные (мы можем, к примеру, захотеть обновить только имя, но не серийный номер), поэтому перед их использованием в методе UpdateDevice делается проверка на null, и обновляется только то, что передано.
    /// </summary>
    public class UpdateDeviceQuery
    {
        public string NewName { get; }
        public string NewSerial{ get; }
        public string NewManufacturer { get; set; }
        public string NewModel { get; set; }

        public int NewCurrentVolts { get; set; }
        public bool NewGasUsage { get; set; }
        public string NewRoomLocation { get; set; }

        public UpdateDeviceQuery(string newName = null, string newSerial = null,  string newManufacturer = null, string newModel = null,
            int newCurrentVolts = 0, bool newGasUsage = false, string newRoomLocation = null)
        {
            NewName = newName;
            NewSerial = newSerial;
            NewManufacturer = newManufacturer;
            NewModel = newModel;
            NewCurrentVolts = newCurrentVolts;
            NewGasUsage = newGasUsage;
            NewRoomLocation = newRoomLocation;
        }
    }
}