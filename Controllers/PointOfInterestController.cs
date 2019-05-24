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
        [HttpGet("{cityId}/pointsOfInterest")]//if parameter here, add it to the acction function
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

        [HttpGet("{cityId}/pointOfInterest/{pointId}", Name = "GetPointOfInterest")]//Add a name to reference in other parts of the class
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

        //Generation of resources

        //Poitn of interest
        [HttpPost("{cityId}/addPointOfInterest")]
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointsOfInterestDtoResourceGeneration pointOfInterestPosted)
        {
            //input data validation
            if (pointOfInterestPosted == null)
            {
                return new BadRequestResult();
            }


            var cityOfPoints = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (cityOfPoints == null)
            {
                return new NotFoundResult();
            }

            //create the point of interest

            //Generating id for new point --temporary solution using maping. to be refactored
            //I need to iterate trhough all the point od interest of all cities to find the highest id and increment it
            var maxPointOfInterestId = CitiesDataStore.Current.Cities.SelectMany(city => city.PointsOfInterest).Max(pointOfInterest => pointOfInterest.Id);

            //generating the new poit of interest
            var newPointOfInterest = new PointOfInterestDto()
            {
                Id = ++maxPointOfInterestId,
                Name = pointOfInterestPosted.Name,
                Description = pointOfInterestPosted.Description
            };

            //Adding the newlly created point of interest to the city
            cityOfPoints.PointsOfInterest.Add(newPointOfInterest);

            //Generating response for the request 

            //I need to sendback the ubication for the newlly created point. I use the name given to the get route to get a Interest point and pass the city and new point ids as a parameter in a annonimus type(new {}) and finally I pass the newlly created point to be sended in the body of the response.
            return CreatedAtRoute("GetPointOfInterest", new
            { cityId = cityId, pointId = newPointOfInterest.Id }, newPointOfInterest);


        }
    }
}