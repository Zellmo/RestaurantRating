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
    public class DeleteRestaurant
    {
        private ICrudService _service;

        public DeleteRestaurant(ICrudService service)
        {
            _service = service;
        }

        [FunctionName("DeleteRestaurant")]
        public async Task<IActionResult> Run(
            [HttpTrigger(AuthorizationLevel.Anonymous, "delete", Route = "restaurant/{id}")] HttpRequest request,
            string id,
            ILogger log)
        {
            log.LogInformation("Deleting restaurant");
            try
            {
                var deleted = await _service.DeleteRestaurantService(new Guid(id));
                if (deleted)
                {
                    return new OkObjectResult($"Restaurant {id} is deleted.");
                }
                return new BadRequestObjectResult($"Restaurant {id} does not exist.");
            }
            catch (Exception e)
            {
                log.LogError(e.Message);
                return new BadRequestObjectResult(e.Message);
            }
        }
    }
}
