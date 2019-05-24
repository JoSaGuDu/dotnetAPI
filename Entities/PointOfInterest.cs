using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace dotnet_asp_API.Entities
{
    public class PointOfInterest //calculated fields no allowed here!
    {
        [Key]//Key data annotation
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]//Determine hoh and when the key will be generated: None, Identity(generated on Add), Computed(Generated on Add and Update)
        public int Id { get; set; }//Fields that are not stablished by the user should not be part of the model for generate new elements of the class. Frist refactor: create another class to handle resource creation.
        public string Name { get; set; }
    }
}