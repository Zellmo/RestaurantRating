using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RestaurantRating.Models
{
    public class Restaurant
    {
        [Key]
        public Guid restaurant_guid { get; set; } = Guid.NewGuid();
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Int64 restaurant_id { get; set; }
        public string restaurant_name { get; set; }
        public string full_address { get; set; }
        public string restaurant_description { get; set; }
        public TimeSpan? opens { get; set; }
        public TimeSpan? closes { get; set; }
    }
}
