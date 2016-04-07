namespace SFA.Apprenticeships.Domain.Entities.Raa.Locations
{
    public class GeoPoint
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public static GeoPoint Invalid => new GeoPoint
        {
            Latitude = double.NaN,
            Longitude = double.NaN
        };

        public override string ToString()
        {
            return $"Latitude:{Latitude}, Longitude:{Longitude}";
        }

        public bool IsValid()
        {
            return !Equals(Invalid);
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
            return Longitude.Equals(other.Longitude) && Latitude.Equals(other.Latitude);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                return (Longitude.GetHashCode()*397) ^ Latitude.GetHashCode();
            }
        }
    }
}
