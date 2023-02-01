using System;
using System.Runtime.CompilerServices;

namespace RestaurantAPI.Exceptions
{
    public class BadRequestException : Exception
    {
        public BadRequestException(string message) : base(message) 
        {
            
        }
        

        
    }
}
