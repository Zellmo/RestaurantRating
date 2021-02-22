using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RestaurantRating.Interfaces;

namespace RestaurantRating.Functions
{
    public class ListRestaurants
    {
        private ICrudService _service;

        public ListRestaurants(ICrudService service)
        {
            _service = service;
        }

        [FunctionName("ListRestaurants")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "restaurant")] HttpRequest request,
            ILogger log)
        {
            log.LogInformation("Getting restaurant list");
            try
            {
                var restaurant = await _service.ListRestaurantsService();
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
