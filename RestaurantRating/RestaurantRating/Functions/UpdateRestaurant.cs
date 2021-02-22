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
    public class UpdateRestaurant
    {
        private ICrudService _service;

        public UpdateRestaurant(ICrudService service)
        {
            _service = service;
        }

        [FunctionName("UpdateRestaurant")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "restaurant/{id}")] HttpRequest request,
            string id,
            ILogger log)
        {
            log.LogInformation("Updating restaurant");
            try
            {
                var body = await new StreamReader(request.Body).ReadToEndAsync();
                var updatedRestaurant = JsonConvert.DeserializeObject<Restaurant>(body);
                var restaurant = await _service.UpdateRestaurantService(new Guid(id),updatedRestaurant);
                if (restaurant == null)
                {
                    return new BadRequestObjectResult($"No restaurant to update or another {updatedRestaurant.restaurant_name} already exists");
                }
                return new OkObjectResult(restaurant);
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
