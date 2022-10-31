using App.Models;
using App.Models.Entities;

namespace App.Data.Repositories
{
    public interface IUserDeviceRepository
    {
        Task<int> Create(CreateDeviceRequest request);
        Task<IEnumerable<Device>> GetAllDevices();
        Task AssignDeviceToUser(AssignDeviceToUserRequest request);
        Task RemoveDeviceFromUser(RemoveDeviceFromUserRequest request);
        Task<GetUserDevicesResponse> GetUserDevices(GetUserDevicesRequest request);

    }
}
