using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using RestaurantAPI.Services;
using RestaurantAPI.Interface;
using Microsoft.AspNetCore.Server.Kestrel.Core;
using Microsoft.AspNetCore.Http;

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

        [HttpPost("generate")]
        public ActionResult<IEnumerable<WeatherForecast>> Generate([FromQuery]int count, [FromBody]TemperatureRequest request)
        {
            if(count < 0 || request.Max < request.Min)
            {
                return BadRequest();
            }
            var result = _service.Get(count, request.Min, request.Max);
            return Ok(result);
        }
    }
}
