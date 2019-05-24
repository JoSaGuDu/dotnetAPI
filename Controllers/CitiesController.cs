using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_asp_API.Controllers
{
    //defining a rout template common to the whole controller
    [Route("api/cities")]
    public class CitiesController : Controller
    {
        //Routing template
        [HttpGet()]
        //return a json list of cities
        public IActionResult GetCities()//IActionResult provides response codes implementation
        {
            return new OkResult();
        }

        [HttpGet("{id}")]//if parameter here, add it to the acction function
        //return a json list of cities
        public IActionResult GetCity(int id)//same parameter from route
        {
            //Find a city
            var cityToreturn = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id);
            if (cityToreturn == null)
            {
                return new NotFoundResult();
            }
            //Alternativelly return new OkResult to send a empty response body with 200 header
            return new OkObjectResult(cityToreturn);
        }

    }
}