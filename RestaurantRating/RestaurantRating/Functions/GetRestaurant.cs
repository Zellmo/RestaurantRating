using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Extensions.Http;
using Microsoft.Extensions.Logging;
using RestaurantRating.Interfaces;

namespace RestaurantRating.Functions
{
    public class GetRestaurant
    {
        private ICrudService _service;

        public GetRestaurant(ICrudService service)
        {
            _service = service;
        }

        [FunctionName("GetRestaurant")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "get", Route = "restaurant/{id}")] HttpRequest request,
            string id,
            ILogger log)
        {
            log.LogInformation("Getting restaurant");
            try
            {
                var restaurant = await _service.GetRestaurantService(new Guid(id));
                if (restaurant == null)
                {
                    return new BadRequestObjectResult($"restaurant id {id} doesn't exists");
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
