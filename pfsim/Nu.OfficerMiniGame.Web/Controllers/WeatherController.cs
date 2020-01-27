using Microsoft.AspNetCore.Mvc;
using Nu.OfficerMiniGame.Weather;

namespace Nu.OfficerMiniGame.Web.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherController : ControllerBase
    {

        [HttpPost]
        [Route("[action]")]
        public IActionResult Generate([FromBody] WeatherInput input) 
        {
            var generator = new PathfinderWeatherGenerator();
            var output = generator.GetWeatherConditions(input);
            return new JsonResult(output);
        }
    }
}
