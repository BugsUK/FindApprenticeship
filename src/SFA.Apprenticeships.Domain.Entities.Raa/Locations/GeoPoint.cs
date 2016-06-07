namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    public class GeoPoint
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }
        public int Easting { get; set; }
        public int Northing { get; set; }

        public static GeoPoint NotSet => new GeoPoint
        {
            Latitude = double.NaN,
            Longitude = double.NaN,
            Easting = int.MinValue,
            Northing = int.MinValue
        };

        public override string ToString()
        {
            return $"Latitude:{Latitude}, Longitude:{Longitude}, Easting: {Easting}, Northing:{Northing}";
        }

        public bool IsSet()
        {
            return !Equals(NotSet);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((GeoPoint) obj);
        }

        protected bool Equals(GeoPoint other)
        {
            return Longitude.Equals(other.Longitude) && Latitude.Equals(other.Latitude) && 
                Easting == other.Easting && Northing == other.Northing;
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Longitude.GetHashCode();
                hashCode = (hashCode*397) ^ Latitude.GetHashCode();
                hashCode = (hashCode*397) ^ Easting;
                hashCode = (hashCode*397) ^ Northing;
                return hashCode;
            }
        }
    }
}
