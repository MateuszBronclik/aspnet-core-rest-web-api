using AutoMapper;
using RestaurantAPI.Entities;
using RestaurantAPI.Exceptions;
using RestaurantAPI.Models;
using System.Linq;
using System.Runtime.InteropServices;

namespace RestaurantAPI.ApiServices
{
    public interface IDishService
    {
        int Create(int restaurantID, CreateDishDto dto);
    }


    public class DishService : IDishService
    {
        
        private readonly RestaurantDbContext _context;
        private readonly IMapper _mapper;

        public DishService(RestaurantDbContext context, IMapper mapper)
        {
            
            _context = context;
            _mapper = mapper;
        }

        public int Create(int restaurantID, CreateDishDto dto)
        {
            var restaurant = _context.Restaurants.FirstOrDefault(r => r.Id == restaurantID);
            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var dishEntity = _mapper.Map<Dish>(dto);
            _context.Dishes.Add(dishEntity);
            _context.SaveChanges();

            return dishEntity.Id;
        }
    }
}
