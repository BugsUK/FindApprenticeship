namespace SFA.DAS.RAA.Api.Entities
{
    using System;

    public class RaaApiUser
    {
        public static readonly RaaApiUser UnknownApiUser = new RaaApiUser {Name = "UnknownApiUser"};

        /// <summary>
        /// The api key that identified this user
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// The user's type
        /// </summary>
        public RaaApiUserType UserType { get; set; }

        /// <summary>
        /// The entities primary key in the database
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// The entities primary identifier in the database
        /// </summary>
        public Guid Guid { get; set; }

        /// <summary>
        /// Can be a UKPRN, EDSURN or other identifier
        /// </summary>
        public string SurrogateId { get; set; }

        protected bool Equals(RaaApiUser other)
        {
            return string.Equals(Name, other.Name) && UserType == other.UserType && Id == other.Id && Guid.Equals(other.Guid) && string.Equals(SurrogateId, other.SurrogateId);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != this.GetType()) return false;
            return Equals((RaaApiUser) obj);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                var hashCode = (Name != null ? Name.GetHashCode() : 0);
                hashCode = (hashCode*397) ^ (int) UserType;
                hashCode = (hashCode*397) ^ Id;
                hashCode = (hashCode*397) ^ Guid.GetHashCode();
                hashCode = (hashCode*397) ^ (SurrogateId != null ? SurrogateId.GetHashCode() : 0);
                return hashCode;
            }
        }
    }
}