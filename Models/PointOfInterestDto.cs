//Real model
namespace dotnet_asp_API
{
    public class PointOfInterestDto
    {
        public int Id { get; set; }//Fields that are not stablished by the user should not be part of the model for generate new elements of the class. Frist refactor: create another class to handle resource creation:see PointsOfInterestDto.ResourceGenretion.cs 
        public string Name { get; set; }
        public string Description { get; set; }
    }
}