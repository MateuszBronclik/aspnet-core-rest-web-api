﻿namespace RestaurantAPI.Models
{
    public class RestaurntQuery
    {
        public string SearchPhrase { get; set; }
        public int PageNumber { get; set;}
        public int PageSize { get; set;}
    }
}
