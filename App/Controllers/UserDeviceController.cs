using System.Threading.Tasks;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDeviceController : ControllerBase
    {
        private readonly IUserDeviceService _userDeviceService;
        private readonly IHubContext<NotificationHubBasic> _hub;
        public UserDeviceController(IUserDeviceService userDeviceService, IHubContext<NotificationHubBasic> hub)
        {
            _userDeviceService = userDeviceService;
            _hub = hub;
        }

        [HttpPost]
        [Route("create")]
        public async Task<ActionResult<int>> Create([FromBody] CreateDeviceRequest request)
        {
            return Ok(await _userDeviceService.Create(request));
        }

        [HttpPost]
        [Route("assignDevice")]
        public async Task<ActionResult> AssignDeviceToUser([FromBody] AssignDeviceToUserRequest request)
        {
            await _userDeviceService.AssignDeviceToUser(request);
            return Ok();
        }

        [HttpPost]
        [Route("unassignDevice")]
        public async Task<ActionResult> RemoveDeviceFromUser([FromBody] RemoveDeviceFromUserRequest request)
        {
            await _userDeviceService.RemoveDeviceFromUser(request);
            return Ok();
        }

        [HttpGet]
        [Route("getDevices")]
        public async Task<ActionResult<GetUserDevicesResponse>> GetUserDevices([FromQuery] int userId)
        {
            var result = await _userDeviceService.GetUserDevices(new GetUserDevicesRequest { UserId = userId });
            return Ok(result);
        }

        [HttpGet]
        [Route("getAllDevices")]
        public async Task<ActionResult> GetAllDevices()
        {
            var result = await _userDeviceService.GetAllDevices();
            return Ok(result);
        }


        [HttpPost]
        [Route("deleteDevice")]
        public async Task<ActionResult<int>> Delete([FromBody] DeleteDeviceRequest request)
        {
            await _userDeviceService.Delete(request);
            return Ok();
        }

        [HttpPost]
        [Route("updateDevice")]
        public async Task<ActionResult<int>> Update([FromBody] UpdateDeviceRequest request)
        {
            return Ok(await _userDeviceService.Update(request));
        }

        [HttpGet]
        [Route("getMeasurements")]
        public async Task<ActionResult> GetDeviceMeasurements([FromQuery] int deviceId)
        {
            var result = await _userDeviceService.GetDeviceMeasurements(new GetDeviceMeasurementsRequest { Id = deviceId });
            return Ok(result);
        }

        [HttpPost]
        [Route("addMeasurements")]
        public async Task<ActionResult> AddDeviceMeasurements([FromBody] AddDeviceMeasurementsRequest request)
        {
            await _userDeviceService.AddDeviceMeasurements(request);
            await _userDeviceService.CheckDeviceHourlyConsumption(request.DeviceId);

            return Ok();
        }

        [HttpPost]
        [Route("sendNotification")]
        public async Task SendNotification()
        {
            await _hub.Clients.All.SendAsync("ReceiveMessage", "test message");
            //var not = new NotificationHub();
            //await not.SendMessage(new Models.Entities.NotifyMessage { Message = "test" });
        }
    }
}

