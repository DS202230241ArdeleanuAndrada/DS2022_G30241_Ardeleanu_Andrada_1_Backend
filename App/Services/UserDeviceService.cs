using App.Data;
using App.Models;
using App.Models.Entities;
using Microsoft.AspNetCore.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace App.Services
{
    public class UserDeviceService : IUserDeviceService
    {
        private readonly IUnitOfWork _uow;
        private readonly IHubContext<NotificationHubBasic> _hub;

        public UserDeviceService(IUnitOfWork uow, IHubContext<NotificationHubBasic> hub)
        {
            _uow = uow;
            _hub = hub;
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

        public async Task Delete(DeleteDeviceRequest request)
        {
            await _uow.UserDeviceRepository.Delete(request);
            _uow.Commit();
        }

        public async Task<int> Update(UpdateDeviceRequest request)
        {
            var result = await _uow.UserDeviceRepository.Update(request);
            _uow.Commit();
            return result;
        }

        public async Task<IEnumerable<SensorModel>> GetDeviceMeasurements(GetDeviceMeasurementsRequest request)
        {
            var result = await _uow.UserDeviceRepository.GetDeviceMeasurements(request);
            _uow.Commit();
            return result;
        }

        public async Task AddDeviceMeasurements(AddDeviceMeasurementsRequest request)
        {
            await _uow.UserDeviceRepository.AddDeviceMeasurements(request);

            _uow.Commit();
        }

        public async Task CheckDeviceHourlyConsumption(int deviceId)
        {
            var device = await _uow.UserDeviceRepository.GetDeviceById(deviceId);

            var hourlyMeasurements = await _uow.UserDeviceRepository.GetHourlyMeasurements(deviceId);

            var hourlyConsumption = hourlyMeasurements.Sum(m => m.MeasurementValue);

            if (hourlyConsumption > Decimal.Parse(device.MaxConsumption))
            {
                await _hub.Clients.Groups(device.Username).SendAsync("ReceiveMessage", $"Device {device.Name} has passed the maximum hourly consumption!");
            }

            _uow.Commit();
        }
    }
}
