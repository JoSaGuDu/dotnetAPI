using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using System.Web.Http;
using System.Net.Http;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.JsonPatch;

namespace dotnet_asp_API.Controllers
{
    //defining a rout template common to the whole controller
    [Route("api/cities")]
    public class PointsOfInterestController : Controller
    {
        //I need to log in the console
        //So I demand an instance of the required service factory
        private ILogger<PointsOfInterestController> _logger;//ILogger(I for Interface) instance provide by the container usin the T  technique: use the type of the category name: nameSpace<categoryName> _nameOfInstance

        //Routing template. Routes must reflect parent child relationships of data.
        [HttpGet("{cityId}/pointsOfInterest")]//if parameter here, add it to the acction function
        //Action: return a json list of cities
        public IActionResult GetPointsOfIterest(int cityId)//IActionResult provides response codes implementation
        {
            //Find the city
            var cityOfPoints = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (cityOfPoints == null)
            {
                return new BadRequestObjectResult(ModelState);
            }
            //Alternativelly return new OkResult to send a empty response body with 200 header
            return new OkObjectResult(cityOfPoints.PointsOfInterest);
        }

        [HttpGet("{cityId}/pointOfInterest/{pointId}", Name = "GetPointOfInterest")]//Add a name to reference in other parts of the class
        //Action:return a json list of cities
        public IActionResult GetAPointOFInterest(int cityId, int pointId)//same parameters from route
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

        //Poitn of interest
        [HttpPost("{cityId}/addPointOfInterest")]
        //Action:Generation of resources
        public IActionResult CreatePointOfInterest(int cityId, [FromBody] PointsOfInterestDtoResourceGeneration pointOfInterestPosted)
        {
            //input data validation. Input validation can be done via Data anotations in the entity used for generate resources; PointsOfInterestDtoResourceGeneration. This is the basic but not the best because doesn't respect separation of concerns by defining validation in controller and model. Check FluentVAlidation on Github to see a robust input validation.
            if (pointOfInterestPosted == null)
            {
                return new BadRequestResult();
            }

            //Using ModelsState is a dictionary that checks DataAnnotations on DTOs adherence and contains error mesages if failed.
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);//returns the ModelState that will display errors when user inpunt doesn't comply data anottations
            }

            //Custom input validation using ModelState
            if (pointOfInterestPosted.Description == pointOfInterestPosted.Name)
            {
                ModelState.AddModelError("CustomErrorKey", "CustomError message: The provided description should be different from the name");
                return new BadRequestObjectResult(ModelState);//returns the ModelState thar will display errors when user inpunt doesn't comply data anottations
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

        //Full resource update (PUT)
        [HttpPut("{cityId}/pointsOfInterest/{id}")]
        //Action:Full update of a point of interest of a city
        public IActionResult FullUpdatePointOfInterest(int cityId, int pointOfinterestToUpdateId, [FromBody]  PointsOfInterestDtoResourceFullUpdate pointOfInterestUpdate)
        {
            //Using ModelsState is a dictionary that checks DataAnnotations on DTOs adherence and contains error mesages if failed.
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);//returns the ModelState that will display errors when user inpunt doesn't comply data anottations
            }

            //Find the city of the point to be updated
            var cityOfPoints = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (cityOfPoints == null)
            {
                return new NotFoundResult();
            }

            //Find the the point to be updated
            //find the point of interest 
            var existentPointOfInterest = cityOfPoints.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfinterestToUpdateId);
            if (existentPointOfInterest == null)
            {
                return new NotFoundResult();
            }

            //Fully update the point
            existentPointOfInterest.Name = pointOfInterestUpdate.Name;
            existentPointOfInterest.Description = pointOfInterestUpdate.Description;

            return NoContent();
        }

        //Partial resource update (PATCH). PATCH uses A class that allow us to deserialize a list of operations to be performed on a Json document (Json Patch RFC 6905). The lists is in Json format.The document will be sended by the consumer via body in the PATCH action and will contain the list of task to perform (replace, copy...)  and the id of the item to be affected and the key: value pairs to be affected on the item. HttpPatchAttribute request body example: [{"op": "replace", "path": "propertyName", "value": "propertyValue"}, {"op": "operation2", "path": "property2Name", "value": "property2Value"}...].
        [HttpPatch("{cityId}/pointsOfInterest/{id}")]
        //Action:Partial update of a point of interest of a city
        public IActionResult PartialUpdatePointOfInterest(int cityId, int pointOfinterestToUpdateId, [FromBody]  JsonPatchDocument<PointsOfInterestDtoResourcePartialUpdate> patchDoc)
        {
            //Body is not empty
            if (patchDoc == null)
            {
                return new BadRequestObjectResult("Invalid informations");
            }

            //Find the city of the point to be updated
            var cityOfPoints = CitiesDataStore.Current.Cities.FirstOrDefault(city => city.Id == cityId);
            if (cityOfPoints == null)
            {
                return new NotFoundResult();
            }

            //Find the the point to be updated
            //find the point of interest 
            var existentPointOfInterest = cityOfPoints.PointsOfInterest.FirstOrDefault(point => point.Id == pointOfinterestToUpdateId);
            if (existentPointOfInterest == null)
            {
                return new NotFoundResult();
            }

            //Map <PointOfInterestDto>existentPointOfInterest to <PointOfInterestDtoResourcePartialUpdate>pointOfInterestToPartiallyUpdate
            var pointOfinterestToPartiallyUpdate =
                new PointsOfInterestDtoResourcePartialUpdate()
                {
                    Name = existentPointOfInterest.Name,
                    Description = existentPointOfInterest.Description
                };

            //Apply operations listed on patchDoc to item to be partially updated and the MOdelState to validate the input
            patchDoc.ApplyTo(pointOfinterestToPartiallyUpdate, ModelState);

            //validate input
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);//returns the ModelState that will display errors when user inpunt doesn't comply data anottations
            }

            if (pointOfinterestToPartiallyUpdate.Description == pointOfinterestToPartiallyUpdate.Name)
            {
                ModelState.AddModelError("CustomErrorKey", "CustomError message: The provided description should be different from the name");
                return new BadRequestObjectResult(ModelState);//returns the ModelState thar will display errors when user inpunt doesn't comply data anottations
            }

            TryValidateModel(pointOfinterestToPartiallyUpdate);
            if (!ModelState.IsValid)
            {
                return new BadRequestObjectResult(ModelState);//returns the ModelState that will display errors when user inpunt doesn't comply data anottations
            }


            //Partially update the item //Fully update the point
            existentPointOfInterest.Name = pointOfinterestToPartiallyUpdate.Name;
            existentPointOfInterest.Description = pointOfinterestToPartiallyUpdate.Description;
            return NoContent();
        }
    }
}