using System.Collections.Generic;
using RestaurantAPI.Controllers;
using RestaurantAPI.Services;

namespace RestaurantAPI.Interface
{
    public interface IWeatherForcastService
    {
       IEnumerable<WeatherForecast> Get(int count, int minTemperature, int maxTemperature);
    }
}
