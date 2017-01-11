namespace SFA.Apprenticeships.Web.Common.ViewModels.Locations
{
    using System;

    [Serializable]
    public class GeoPointViewModel
    {
        public double Longitude { get; set; }
        public double Latitude { get; set; }

        public static GeoPointViewModel NotSet => new GeoPointViewModel
        {
            Latitude = double.NaN,
            Longitude = double.NaN
        };

        public override string ToString()
        {
            return $"Latitude:{Latitude}, Longitude:{Longitude}";
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
            return Equals((GeoPointViewModel)obj);
        }

        protected bool Equals(GeoPointViewModel other)
        {
            return Longitude.Equals(other.Longitude) && Latitude.Equals(other.Latitude);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = Longitude.GetHashCode();
                hashCode = (hashCode * 397) ^ Latitude.GetHashCode();
                return hashCode;
            }
        }
    }
}
