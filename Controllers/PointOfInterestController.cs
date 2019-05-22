using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_asp_API.Controllers
{
    //defining a rout template common to the whole controller
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        //Routing template
        [HttpGet("{cityId}/pointsOfInterest")]
        //return a json list of cities
        public IActionResult GetPointsOfIterest(int cityId)//IActionResult provides response codes implementation
        {
            //Find the city
            var cityOfPoints = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (cityOfPoints == null)
            {
                return new NotFoundResult();
            }
            //Alternativelly return new OkResult to send a empty response body with 200 header
            return new OkObjectResult(cityOfPoints.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointOfInterest/{pointId}")]//if parameter here, add it to the acction function
        //return a json list of cities
        public IActionResult GetAPointOFInterest(int cityId, int pointId)//same parameter from route
        {
            //Find the city
            var cityOfPoints = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (cityOfPoints == null)
            {
                return new NotFoundResult();
            }

            //find the point of interest 
            var pointOfInterest = cityOfPoints.PointsOfInterest.FirstOrDefault(point => point.Id == pointId);
            if (pointOfInterest == null)
            {
                return new NotFoundResult();
            }
            //Alternativelly return new OkResult to send a empty response body with 200 header
            return new OkObjectResult(pointOfInterest);
        }
    }
}