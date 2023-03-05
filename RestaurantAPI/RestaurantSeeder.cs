using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace RestaurantAPI
{
    public class RestaurantSeeder
    {
        private readonly RestaurantDbContext _dbContext;
        public RestaurantSeeder(RestaurantDbContext dbContext)
        {
            _dbContext = dbContext;
        }
        public void Seed()
        {
            if (_dbContext.Database.CanConnect())
            {
                var pendingMigrations = _dbContext.Database.GetPendingMigrations();
                if (pendingMigrations != null && pendingMigrations.Any())
                {
                    _dbContext.Database.Migrate();
                }

                if (_dbContext.Database.CanConnect())
                {
                    if (!_dbContext.Roles.Any())
                    {
                        var roles = GetRoles();
                        _dbContext.Roles.AddRange(roles);
                        _dbContext.SaveChanges();
                    }
                    if (!_dbContext.Restaurants.Any())
                    {
                        var restaurants = GetRestaurants();
                        _dbContext.Restaurants.AddRange(restaurants);
                        _dbContext.SaveChanges();
                    }
                }
            }


        }
        private IEnumerable<Role> GetRoles()
        {
            var roles = new List<Role>()
            {
                new Role()
                {
                    Name= "User",
                },
                new Role()
                {
                    Name= "Manager"
                },
                new Role()
                {
                    Name = "Admin"
                }
            };
            return roles;

        }
        private IEnumerable<Restaurant> GetRestaurants()
        {
            var restaurants = new List<Restaurant>()
            {
                new Restaurant()
                {
                    Name = "KFC",
                    Category = "Fast Food",
                    Description = "KFC is a global chicken restaurant brand with a rich," +
                    " decades-long history of success and innovation. It all started with one cook, " +
                    "Colonel Harland Sanders, who created a finger lickin' good recipe more than 75 years ago—a " +
                    "list of 11 secret herbs and spices scratched out on the back of his kitchen door.",
                    ContactEmail = "contact@kfc.com",
                    HasDelivery= true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Hot Chilli Poops",
                            Price= 15.45m,
                        },
                        new Dish()
                        {
                            Name ="Spicy Boobi Poops",
                            Price= 12.99m,

                        },

                    },
                    Address = new Address()
                    {
                        City = "Szczecin",
                        Street = "Rayskiego 4",
                        PostalCode = "00-001"
                    }
                },
                new Restaurant()
                {
                    Name = "Gruby Benek",
                    Category = "Pizza",
                    Description = "Gruby Benek is a franchise network of pizzerias with several dozen premises throughout Poland." +
                    "Visit us or order with delivery and try the best American-style BenPizza, delicious BenBurgers and fried chicken BenKura!",
                    ContactEmail = "contact@grubybenek.com",
                    HasDelivery= true,
                    Dishes = new List<Dish>()
                    {
                        new Dish()
                        {
                            Name = "Peperoni Du Sia Slice",
                            Price= 20.99m,
                        },
                        new Dish()
                        {
                            Name ="Boobi Slice",
                            Price= 13.00m,

                        },

                    },
                    Address = new Address()
                    {
                        City = "Gdańsk",
                        Street = "Piekna 4",
                        PostalCode = "00-002"
                    }
                },

            };
            return restaurants;

        }
    }
}
