using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    public class CitiesController
    {
        //return a json list of cities
        public JsonResult GetCities()
        {
            return new JsonResult(new List<object>()
            {
                new {id=1, Name="Montreal"},
                new {id=2, Name="Ottawa"}

            });
        }    
    }
}