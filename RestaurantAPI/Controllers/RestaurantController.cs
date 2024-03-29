﻿
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestaurantAPI.ApiServices;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Security.Claims;

namespace RestaurantAPI.Controllers
{
    [Route("api/restaurant")]
    [ApiController]
    [Authorize]
    public class RestaurantController : ControllerBase
    {
        private readonly IRestaurantService _restaurantService;

        public RestaurantController(IRestaurantService restaurantService)
        {
            _restaurantService = restaurantService;
        }

        [HttpPost]
        [Authorize(Roles = "Admin,Manager")]
        public ActionResult CreateRestaurant([FromBody] CreateRestaurantDto dto)
        {
            var userId = int.Parse(User.FindFirst(c => c.Type == ClaimTypes.NameIdentifier).Value);
            var id = _restaurantService.Create(dto);
            return Created($"/api/restaurant/{id}", null);
        }

        [HttpGet]
        [AllowAnonymous]
        public ActionResult<IEnumerable<RestaurantDto>> GetAll([FromQuery]RestaurntQuery query)
        {
            var restaurantsDtos = _restaurantService.GetAll(query);
            return Ok(restaurantsDtos);
        }

        [HttpGet("{id}")]
        public ActionResult<RestaurantDto> Get([FromRoute] int id)
        {
            var restaurant = _restaurantService.GetById(id);
            return Ok(restaurant);
        }
        [HttpPut("{id}")]
        public ActionResult Update([FromBody] UpdateResturantDto dto, [FromRoute] int id)
        {
            _restaurantService.Update(id, dto);
            return Ok();
        }
        [HttpDelete("{id}")]
        public ActionResult Delete([FromRoute] int id)
        {
            _restaurantService.Delete(id);
            return NotFound();
        }
    }
}

