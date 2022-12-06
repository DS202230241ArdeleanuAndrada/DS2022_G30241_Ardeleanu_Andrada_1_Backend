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
        Task Delete(DeleteDeviceRequest request);
        Task<int> Update(UpdateDeviceRequest request);
        Task<IEnumerable<SensorModel>> GetDeviceMeasurements(GetDeviceMeasurementsRequest request);
        Task AddDeviceMeasurements(AddDeviceMeasurementsRequest request);
        Task<IEnumerable<SensorModel>> GetHourlyMeasurements(int deviceId);
        Task<DeviceWithUser> GetDeviceById(int deviceId);
    }
}
