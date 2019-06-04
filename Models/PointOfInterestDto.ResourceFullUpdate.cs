//This classes are also DTOs but are specialized to transport data in the way the database will undestand. 
using System.ComponentModel.DataAnnotations;

namespace dotnet_asp_API
{
    public class PointsOfInterestDtoResourceFullUpdate
    {
        [Required(ErrorMessage = "You should provide a name value. ")]//Using data annotations that helps to validate user input. Those rules have to be chequed with help of the ModelState in the controller.
        [MaxLength(50)]
        public string Name { get; set; }

        [MaxLength(200)]
        public string Description { get; set; }
    }
}