namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    public class GeoPoint
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public override string ToString()
        {
            return $"Latitude:{Latitude}, Longitude:{Longitude}";
        }
    }
}
