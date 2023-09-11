using CommunicationService.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CommunicationService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CommunicationAPI : ControllerBase
    {
        public CommunicationAPI() { }

        [HttpPost]
        public ActionResult TestInbound(Category category)
        {
            Console.WriteLine("TestInbound");
            return Ok("TestInbound is ok.");
        }

        [HttpGet]
        public ActionResult TestOutboundGet()
        {
            Console.WriteLine("Get is called");
            return Ok("Get is called");
        }
    }
}
