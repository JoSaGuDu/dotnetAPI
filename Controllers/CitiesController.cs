using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_asp_API.Controllers
{
    //defining a rout template common to the whole controller
    [Route("api/cities")]
    public class CitiesController
    {
        //Routing template
        [HttpGet()]
        //return a json list of cities
        public JsonResult GetCities()
        {
            return new JsonResult(CitiesDataStore.Current.Cities);
        }

        [HttpGet("{id}")]//if parameter here, add it to the acction function
        //return a json list of cities
        public JsonResult GetCity(int id)//same parameter from route
        {
            return new JsonResult(CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == id));
        }
    }
}