using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
    [Route("api/c/{controller}")]
    [ApiController]
    public class PlatformsController : ControllerBase
    {
        public PlatformsController()
        {
            
        }

        [HttpPost]
        public ActionResult TestConnection(){
            Console.WriteLine("--> Inbound POST # Command Service");
            return Ok("Inbound TestConnection");
        }
    }
}