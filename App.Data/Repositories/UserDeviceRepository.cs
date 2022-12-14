using App.Models;
using App.Models.Entities;
using Dapper;
using System.Data;

namespace App.Data.Repositories
{
    public class UserDeviceRepository : IUserDeviceRepository
    {
        protected IDbTransaction Transaction { get; private set; }
        protected IDbConnection Connection { get { return Transaction.Connection; } }
        public UserDeviceRepository(IDbTransaction transaction) {
            Transaction = transaction;
        }

        public async Task<int> Create(CreateDeviceRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                MaxConsumption = request.MaxConsumption
            });

            var result = await Connection.QueryAsync<int>(
              "SP_Device_Create",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result.FirstOrDefault();
        }

        public async Task AssignDeviceToUser(AssignDeviceToUserRequest request)
        {
            var parameters = new DynamicParameters(new {
                UserId = request.UserId,
                DeviceId = request.DeviceId
            });

            await Connection.QueryAsync<int>(
              "SP_User_AssignDevice",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );
        }

        public async Task RemoveDeviceFromUser(RemoveDeviceFromUserRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                UserId = request.UserId,
                DeviceId = request.DeviceId
            });

            await Connection.QueryAsync<int>(
              "SP_User_RemoveDevice",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );
        }

        public async Task<GetUserDevicesResponse> GetUserDevices(GetUserDevicesRequest request)
        {
            var parameters = new DynamicParameters(new
            {
               UserId = request.UserId
            });

            var result = await Connection.QueryAsync<Device>(
              "SP_User_GetDevices",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return new GetUserDevicesResponse { Devices = result };
        }

        public async Task<IEnumerable<Device>> GetAllDevices()
        {
            var result = await Connection.QueryAsync<Device>(
              "SP_Device_GetAllDevices",
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result;
        }

        public async Task Delete(DeleteDeviceRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                Id = request.Id
            });

            await Connection.QueryAsync<int>(
              "SP_Device_Delete",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );
        }
        public async Task<int> Update(UpdateDeviceRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                Id = request.Id,
                Name = request.Name,
                Description = request.Description,
                Address = request.Address,
                MaxConsumption = request.MaxConsumption
            });

            var result = await Connection.QueryAsync<int>(
              "SP_Device_Update",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result.FirstOrDefault();
        }

        public async Task<IEnumerable<SensorModel>> GetDeviceMeasurements(GetDeviceMeasurementsRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                DeviceId = request.Id
            });

            var result = await Connection.QueryAsync<SensorModel>(
              "SP_Device_GetMeasurements",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result;
        }

        public async Task AddDeviceMeasurements(AddDeviceMeasurementsRequest request)
        {
            var parameters = new DynamicParameters(new
            {
                DeviceId = request.DeviceId,
                Timestamp = request.Timestamp,
                MeasurementValue = request.MeasurementValue
            });

            await Connection.QueryAsync<SensorModel>(
              "SP_Device_AddMeasurement",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );
        }

        public async Task<DeviceWithUser> GetDeviceById(int deviceId)
        {
            var parameters = new DynamicParameters(new
            {
                DeviceId = deviceId
            });

            var result = await Connection.QueryFirstAsync<DeviceWithUser>(
              "SP_Device_GetById",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result;
        }

        public async Task<IEnumerable<SensorModel>> GetHourlyMeasurements(int deviceId)
        {
            var parameters = new DynamicParameters(new
            {
                DeviceId = deviceId
            });

            var result = await Connection.QueryAsync<SensorModel>(
              "SP_Device_GetHourlyMeasurements",
              param: parameters,
              commandType: CommandType.StoredProcedure,
              commandTimeout: 60,
              transaction: Transaction
            );

            return result;
        }
    }
}
