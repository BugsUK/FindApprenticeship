namespace SFA.Apprenticeships.Data.Migrate
{
    using System;
    using System.Collections.Generic;

    public interface IAvmsSyncRespository
    {
        bool IsVacancyOwnedByTargetDatabase(int vacancyId);

        bool IsVacancyOwnerRelationshipOwnedByTargetDatabase(int vacancyOwnerRelationshipId);


        /// <summary>
        /// Get some anonymous details that are always the same for a particular id
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        AnonDetail GetAnonymousDetails(int id);

        IDictionary<string, int> GetPersonTitleTypeIdsByTitleFullName();
    }

    // Based on http://www.fakenamegenerator.com/
    public class AnonDetail
    {
        /// <summary>E.g. male or female</summary>
        public string Gender { get; set; }

        /// <summary>E.g. Mr.</summary>
        /// <seealso cref="TitleWithoutDot"/>
        public string Title { get; set; }

        public string GivenName { get; set; }
        public string MiddleInitial { get; set; }
        public string Surname { get; set; }
        public string StreetAddress { get; set; }

        /// <summary>E.g. COVENTRY</summary>
        public string City { get; set; }

        public string ZipCode { get; set; }
        public string CountryFull { get; set; }
        public string EmailAddress { get; set; }
        public string TelephoneNumber { get; set; }
        public string MothersMaiden { get; set; }

        /// <summary>E.g. 2/22/1990</summary>
        public string Birthday { get; set; }

        public string TitleWithoutDot { get { return Title.Replace(".", ""); } }
    }
}
