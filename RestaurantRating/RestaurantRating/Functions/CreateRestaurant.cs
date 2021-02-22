using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using RestaurantRating.Interfaces;
using RestaurantRating.Models;

namespace RestaurantRating.Functions
{
    public class CreateRestaurant
    {
        private ICrudService _service;

        public CreateRestaurant(ICrudService service)
        {
            _service = service;
        }

        [FunctionName("CreateRestaurant")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "post", Route = "restaurant")] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("Creating new restaurant");
            try
            {
                var body = await new StreamReader(request.Body).ReadToEndAsync();
                var restaurantToCreate = JsonConvert.DeserializeObject<Restaurant>(body);
                var restaurant = await _service.CreateRestaurantService(restaurantToCreate);
                if (restaurant == null)
                {
                    return new BadRequestObjectResult($"restaurant_name {restaurantToCreate.restaurant_name} already exists");
                }
                var responseMessage = $"Restaurant is created, the id is {restaurant.restaurant_guid}";
                return new OkObjectResult(responseMessage);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
