using System.Threading.Tasks;
using App.Models;
using App.Services;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserDeviceController : ControllerBase
    {
        private readonly IUserDeviceService _userDeviceService;
        public UserDeviceController(IUserDeviceService userDeviceService)
        {
            _userDeviceService = userDeviceService;
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
    }
}

