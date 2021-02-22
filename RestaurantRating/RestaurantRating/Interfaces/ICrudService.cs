using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using RestaurantRating.Models;

namespace RestaurantRating.Interfaces
{
    public interface ICrudService
    {
        Task<RestaurantWithAverageRating> GetRestaurantService(Guid restaurantId);
        Task<Restaurant> CreateRestaurantService(Restaurant restaurant);
        Task<Restaurant> UpdateRestaurantService(Guid restaurantId, Restaurant restaurant);
        Task<bool> DeleteRestaurantService(Guid id);
        Task<List<RestaurantWithAverageRating>> ListRestaurantsService();

    }
}