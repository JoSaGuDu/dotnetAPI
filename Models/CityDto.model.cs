//DTO (Data Transfer objects) is a data container for moving data between layers. They are also termed as transfer objects. DTO is only used to pass data and does not contain any business logic. They only have simple setters and getters.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_asp_API.Models
{
    public class CityDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }

        public int NumberOfPointsOfInterest//This shouldn't be part of the entity class as it is calculated on the fly and no dafe to interact with the database.TO BE REFACTORED.
        {
            get
            {
                return PointsOfInterest.Count;
            }
        }

        //Extend CityDto to show list of points od interest
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();//C# autoinitializer sintax. It is not recomendable to leave fileds uninitialized. This will generate null reference errors. Initialize to an empty element.
    }
}