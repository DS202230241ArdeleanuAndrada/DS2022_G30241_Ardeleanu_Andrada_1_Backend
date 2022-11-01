using App.Models;
using App.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public interface IUserDeviceService
    {
        Task<int> Create(CreateDeviceRequest request);
        Task AssignDeviceToUser(AssignDeviceToUserRequest request);
        Task RemoveDeviceFromUser(RemoveDeviceFromUserRequest request);
        Task<GetUserDevicesResponse> GetUserDevices(GetUserDevicesRequest request);
        Task<IEnumerable<Device>> GetAllDevices();
        Task Delete(DeleteDeviceRequest request);
        Task<int> Update(UpdateDeviceRequest request);
    }
}
