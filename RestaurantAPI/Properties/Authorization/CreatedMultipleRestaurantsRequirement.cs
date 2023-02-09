﻿using Microsoft.AspNetCore.Authorization;

namespace RestaurantAPI.Properties.Authorization
{
    public class CreatedMultipleRestaurantsRequirement : IAuthorizationRequirement
    {
        public CreatedMultipleRestaurantsRequirement(int minimumRestaurantsCreated)
        {
            MinimumRestaurantsCreated = minimumRestaurantsCreated;
        }

        public int MinimumRestaurantsCreated { get;}
    }
}
