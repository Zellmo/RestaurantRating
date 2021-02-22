using System;
using System.ComponentModel.DataAnnotations;

namespace RestaurantRating.Models
{
    public class Rating
    {
        [Key]
        public Guid rating_guid { get; set; } = Guid.NewGuid();
        public Int64 restaurant_id { get; set; }
        public Int64 user_account_id { get; set; }
        public int rating { get; set; }
    }
}
