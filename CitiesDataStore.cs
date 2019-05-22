using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using dotnet_asp_API.Models;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_asp_API
{
    public class CitiesDataStore
    {
        //I need a temporal datapersistance solution
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        //I need a list for store cities
        public List<CityDto> Cities { get; set; }

        public CitiesDataStore()
        {
            //init dummy data
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The one with that big park.",
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Antwerp",
                    Description = "The one with the cathedral that was never really finished.",
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Paris",
                    Description = "The one with that big tower.",
                },
            };
        }
    }
}