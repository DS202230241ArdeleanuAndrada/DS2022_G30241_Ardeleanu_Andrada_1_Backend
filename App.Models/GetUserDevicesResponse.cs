using App.Models.Entities;

namespace App.Models
{
    public class GetUserDevicesResponse
    {
        public IEnumerable<Device> Devices { get; set; }
    }
}

