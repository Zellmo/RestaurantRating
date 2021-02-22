using System;
using Microsoft.EntityFrameworkCore;
using RestaurantRating.Data;
using RestaurantRating.Interfaces;
using RestaurantRating.Models;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.JsonPatch.Adapters;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace RestaurantRating.Services
{
    public class CrudService : ICrudService
    {
        private readonly DataContext _ctx;

        public CrudService(DataContext ctx)
        {
            _ctx = ctx;
        }

        public async Task<List<RestaurantWithAverageRating>> ListRestaurantsService()
        {
            var ratingAverage = _ctx.rating.GroupBy(r => r.restaurant_id)
                .Select(g => new {restaurant_id = g.Key, Average = g.Average(r => r.rating)});
            var query = from restaurants in _ctx.restaurant
                    from ratingAverages in ratingAverage
                            .Where(rating => rating.restaurant_id == restaurants.restaurant_id).DefaultIfEmpty()
                select new RestaurantWithAverageRating
                {
                    restaurant_name = restaurants.restaurant_name,
                    restaurant_id = restaurants.restaurant_id,
                    restaurant_description = restaurants.restaurant_description,
                    restaurant_guid = restaurants.restaurant_guid,
                    full_address = restaurants.full_address,
                    opens = restaurants.opens,
                    closes = restaurants.closes,
                    averageRating =((decimal?)ratingAverages.Average)
                };
            return await query.ToListAsync();
        }

    public async Task<RestaurantWithAverageRating> GetRestaurantService(Guid restaurantId)
        {
            var ratingAverage = _ctx.rating.GroupBy(r => r.restaurant_id)
                .Select(g => new { restaurant_id = g.Key, Average = g.Average(r => r.rating) });
            var query = from restaurants in _ctx.restaurant
                    .Where(r => r.restaurant_guid == restaurantId)
                        from ratingAverages in ratingAverage
                    .Where(rating => rating.restaurant_id == restaurants.restaurant_id).DefaultIfEmpty()
                select new RestaurantWithAverageRating
                {
                    restaurant_name = restaurants.restaurant_name,
                    restaurant_id = restaurants.restaurant_id,
                    restaurant_description = restaurants.restaurant_description,
                    restaurant_guid = restaurants.restaurant_guid,
                    full_address = restaurants.full_address,
                    opens = restaurants.opens,
                    closes = restaurants.closes,
                    averageRating = ((decimal?)ratingAverages.Average)
                };
            return await query.FirstOrDefaultAsync();
        }

        public async Task<Restaurant> CreateRestaurantService(Restaurant restaurant)
        {
            if (await RestaurantExists(restaurant.restaurant_name))
            { 
                return null;
            }
            _ctx.restaurant.Add(restaurant);
            await _ctx.SaveChangesAsync();
            return restaurant;
        }

        public async Task<Restaurant> UpdateRestaurantService(Guid restaurantId, Restaurant restaurant)
        {
            var restaurantToBeUpdated = await GetRestaurantService(restaurantId);
            if (restaurantToBeUpdated == null || restaurant == null || await RestaurantExists(restaurant.restaurant_name, restaurantId))
            {
                return null;
            }
            restaurantToBeUpdated.restaurant_name = restaurant.restaurant_name ?? restaurantToBeUpdated.restaurant_name;
            restaurantToBeUpdated.restaurant_description = restaurant.restaurant_description ?? restaurantToBeUpdated.restaurant_description;
            restaurantToBeUpdated.full_address = restaurant.full_address ?? restaurantToBeUpdated.full_address;
            restaurantToBeUpdated.opens = restaurant.opens ?? restaurantToBeUpdated.opens;
            restaurantToBeUpdated.closes = restaurant.closes ?? restaurantToBeUpdated.closes;
            await _ctx.SaveChangesAsync();
            return restaurantToBeUpdated;
        }

        public async Task<bool> DeleteRestaurantService(Guid id)
        {
            var restaurant = await GetRestaurantService(id);
            if (restaurant == null)
            {
               return false;
            }
            _ctx.restaurant.Remove(restaurant);
            await _ctx.SaveChangesAsync();
            return true;
        }



        #region Private Methods
        private async Task<bool> RestaurantExists(string restaurantName)
        {
            return await _ctx.restaurant.FirstOrDefaultAsync(x => x.restaurant_name == restaurantName) != null;
        }
        private async Task<bool> RestaurantExists(string restaurantName, Guid restaurantId)
        {
            return await _ctx.restaurant.FirstOrDefaultAsync(x => x.restaurant_name == restaurantName && x.restaurant_guid != restaurantId) != null;
        }
        #endregion
    }
}