using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RestaurantAPI.Services;
using RestaurantAPI.Interface;

namespace RestaurantAPI.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly IWeatherForcastService _service;
        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, IWeatherForcastService service)
        {
            _logger = logger;
            _service = service;
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var result = _service.Get();
            return result;
        }

        [HttpGet("Test/{max}")]
        public IEnumerable<WeatherForecast> Get2([FromQuery] int take, [FromRoute] int max)
        {
            var result = _service.Get();
            return result;
        }

        [HttpPost]
        public ActionResult<string> Hello([FromBody]string name)
        {
            //HttpContext.Response.StatusCode = 401;
            //return StatusCode(401, $"Hello: {name}");
            //return $"Hello {name}";
            return NotFound($"Hello : {name}");
        }
    }
}
