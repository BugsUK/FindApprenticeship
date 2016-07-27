namespace SFA.Apprenticeships.Data.Migrate.Faa.Mappers
{
    using System.Collections.Generic;
    using Repository.Sql;

    public static class SchoolAttendedMappers
    {
        public static IDictionary<string, object> MapSchoolAttendedDictionary(this SchoolAttended schoolAttended)
        {
            return new Dictionary<string, object>
            {
                {"SchoolAttendedId", schoolAttended.SchoolAttendedId},
                {"CandidateId", schoolAttended.CandidateId},
                {"SchoolId", schoolAttended.SchoolId},
                {"OtherSchoolName", schoolAttended.OtherSchoolName},
                {"OtherSchoolTown", schoolAttended.OtherSchoolTown},
                {"StartDate", schoolAttended.StartDate},
                {"EndDate", schoolAttended.EndDate},
                {"ApplicationId", schoolAttended.ApplicationId}
            };
        }
    }
}