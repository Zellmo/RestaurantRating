using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantRating.Models
{
    public class RestaurantWithAverageRating: Restaurant
    {
        public decimal? averageRating { get; set; }
    }
}
