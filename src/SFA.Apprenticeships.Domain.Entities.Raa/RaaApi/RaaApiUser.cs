namespace SFA.Apprenticeships.Domain.Entities.Raa.RaaApi
{
    using System;

    public class RaaApiUser
    {
        public static readonly RaaApiUser UnknownApiUser = new RaaApiUser();

        public Guid PrimaryApiKey { get; set; }

        public Guid SecondaryApiKey { get; set; }

        /// <summary>
        /// The user's type
        /// </summary>
        public RaaApiUserType UserType { get; set; }

        /// <summary>
        /// The entities primary key in the database
        /// </summary>
        public int ReferencedEntityId { get; set; }

        /// <summary>
        /// The entities primary identifier in the database
        /// </summary>
        public Guid? ReferencedEntityGuid { get; set; }

        /// <summary>
        /// Can be a UKPRN, EDSURN or other identifier
        /// </summary>
        public int ReferencedEntitySurrogateId { get; set; }

        protected bool Equals(RaaApiUser other)
        {
            return PrimaryApiKey.Equals(other.PrimaryApiKey) && SecondaryApiKey.Equals(other.SecondaryApiKey) && UserType == other.UserType && ReferencedEntityId == other.ReferencedEntityId && ReferencedEntityGuid.Equals(other.ReferencedEntityGuid) && ReferencedEntitySurrogateId == other.ReferencedEntitySurrogateId;
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
                var hashCode = PrimaryApiKey.GetHashCode();
                hashCode = (hashCode*397) ^ SecondaryApiKey.GetHashCode();
                hashCode = (hashCode*397) ^ (int) UserType;
                hashCode = (hashCode*397) ^ ReferencedEntityId;
                hashCode = (hashCode*397) ^ ReferencedEntityGuid.GetHashCode();
                hashCode = (hashCode*397) ^ ReferencedEntitySurrogateId;
                return hashCode;
            }
        }
    }
}