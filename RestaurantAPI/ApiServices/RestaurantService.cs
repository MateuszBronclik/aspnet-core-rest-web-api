﻿using AutoMapper;
using Microsoft.EntityFrameworkCore;
using RestaurantAPI.Entities;
using RestaurantAPI.Models;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Logging;
using RestaurantAPI.Exceptions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using RestaurantAPI.Properties.Authorization;
using System;
using System.Linq.Expressions;

namespace RestaurantAPI.ApiServices
{
    public interface IRestaurantService
    {
        RestaurantDto GetById(int id);
        PageResult<RestaurantDto> GetAll(RestaurntQuery query);
        int Create(CreateRestaurantDto dto);
        void Delete(int id);
        void Update(int id, UpdateResturantDto dto);
    }


    public class RestaurantService : IRestaurantService
    {
        private readonly RestaurantDbContext _dbContext;
        private readonly IMapper _mapper;
        private readonly ILogger<RestaurantService> _logger;
        private readonly IAuthorizationService _authorizationService;
        private readonly IUserContextService _userContextService;

        public RestaurantService(RestaurantDbContext dbContext, IMapper mapper, ILogger<RestaurantService> logger, IAuthorizationService authorizationService,
            IUserContextService userContextService)
        {
            _logger = logger;
            _dbContext = dbContext;
            _mapper = mapper;
            _authorizationService = authorizationService;
            _userContextService = userContextService;
        }

        public void Update(int id, UpdateResturantDto dto)
        {


            var restaurant = _dbContext
                .Restaurants
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync
                (_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Update)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }


            restaurant.Name = dto.Name;
            restaurant.Description = dto.Description;
            restaurant.HasDelivery = dto.HasDelivery;

            _dbContext.SaveChanges();
        }

        public void Delete(int id)
        {
            _logger.LogError($"Restautant with id: {id} DELETE action invoked");
            var restaurant = _dbContext.Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);


            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");

            var authorizationResult = _authorizationService.AuthorizeAsync
            (_userContextService.User, restaurant, new ResourceOperationRequirement(ResourceOperation.Delete)).Result;

            if (!authorizationResult.Succeeded)
            {
                throw new ForbidException();
            }

            _dbContext.Restaurants.Remove(restaurant);
            _dbContext.SaveChanges();

        }
        public RestaurantDto GetById(int id)
        {
            var restaurant = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .FirstOrDefault(r => r.Id == id);

            if (restaurant is null)
                throw new NotFoundException("Restaurant not found");
            var result = _mapper.Map<RestaurantDto>(restaurant);
            return result;

        }
        public PageResult<RestaurantDto> GetAll(RestaurntQuery query)
        {
            var baseQuery = _dbContext
                .Restaurants
                .Include(r => r.Address)
                .Include(r => r.Dishes)
                .Where(r => query.SearchPhrase == null || (r.Name.ToLower().Contains(query.SearchPhrase.ToLower()) ||
                r.Description.ToLower().Contains(query.SearchPhrase.ToLower())));

            if (!string.IsNullOrEmpty(query.SortBy))
            {
                var columnsSelector = new Dictionary<string, Expression<Func<Restaurant, object>>>
                {
                     {nameof(Restaurant.Name), r => r.Name},
                     {nameof(Restaurant.Description), r => r.Description},
                     {nameof(Restaurant.Category), r => r.Category}
                };

                var selectedColumn = columnsSelector[query.SortBy];

                baseQuery = query.SortDirection == SortDirection.ASC
                    ? baseQuery.OrderBy(selectedColumn)
                    : baseQuery.OrderByDescending(selectedColumn);
            }

            var restaurants = baseQuery
                .Skip(query.PageSize * (query.PageNumber - 1))
                .Take(query.PageSize)
                .ToList();

            var totalItemsCount = baseQuery.Count();

            var restaurantsDtos = _mapper.Map<List<RestaurantDto>>(restaurants);
            var result = new PageResult<RestaurantDto>(restaurantsDtos, totalItemsCount, query.PageSize, query.PageNumber);
            return result;
        }

        public int Create(CreateRestaurantDto dto)
        {
            var restaurant = _mapper.Map<Restaurant>(dto);
            restaurant.CreatedById = _userContextService.GetUserId;
            _dbContext.Restaurants.Add(restaurant);
            _dbContext.SaveChanges();

            return restaurant.Id;
        }

    }
}
