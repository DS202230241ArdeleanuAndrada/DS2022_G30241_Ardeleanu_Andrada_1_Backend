using App.Data;
using App.Models;
using App.Models.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace App.Services
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUnitOfWork _uow;

        public UserDeviceService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<int> Create(CreateDeviceRequest request)
        {
            var result = await _uow.UserDeviceRepository.Create(request);
            _uow.Commit();
            return result;
        }

        public async Task AssignDeviceToUser(AssignDeviceToUserRequest request)
        {
            await _uow.UserDeviceRepository.AssignDeviceToUser(request);
            _uow.Commit();
        }

        public async Task RemoveDeviceFromUser(RemoveDeviceFromUserRequest request)
        {
            await _uow.UserDeviceRepository.RemoveDeviceFromUser(request);
            _uow.Commit();
        }

        public async Task<GetUserDevicesResponse> GetUserDevices(GetUserDevicesRequest request)
        {
            var result = await _uow.UserDeviceRepository.GetUserDevices(request);
            return result;
        }
        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            var result = await _uow.UserDeviceRepository.GetAllDevices();
            return result;
        }
    }
}
