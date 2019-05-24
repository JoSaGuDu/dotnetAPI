using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_asp_API.Entities
{
    public class City //calculated fields no allowed here!
    {
        [Key]//Key data annotation
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Determine hoh and when the key will be generated: None, Identity(generated on Add), Computed(Generated on Add and Update)
        public int Id { get; set; }//Giving a class a filed with the Id identifier or FieldNameId, automatically will be take as the Primary Key by the entity framework  for the table at the moment of code database creation. Optionally use the Key data anotation from System.ComponentModel.DataAnnotations
        public string Name { get; set; }
        public string Description { get; set; }
        //Extend CityDto to show list of points od interest
        public ICollection<PointOfInterestDto> PointsOfInterest { get; set; } = new List<PointOfInterestDto>();
    }
}