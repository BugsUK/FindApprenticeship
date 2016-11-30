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
        public Guid ReferencedEntityGuid { get; set; }

        /// <summary>
        /// Can be a UKPRN, EDSURN or other identifier
        /// </summary>
        public int ReferencedEntitySurrogateId { get; set; }
    }
}