namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Builders
{
    using System;
    using Migrate.Faa.Entities.Mongo;

    public class VacancyApplicationBuilder
    {
        private int _status = 30;
        private int _legacyApplicationId = 123456;
        private string _unsuccessfulReason;
        private string _withdrawnOrDeclinedReason;

        public VacancyApplication Build()
        {
            return new VacancyApplication
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now.AddDays(-7),
                DateUpdated = DateTime.Now,
                Status = _status,
                DateApplied = DateTime.Now.AddDays(-4),
                CandidateId = Guid.NewGuid(),
                LegacyApplicationId = _legacyApplicationId,
                WithdrawnOrDeclinedReason = _withdrawnOrDeclinedReason,
                UnsuccessfulReason = _unsuccessfulReason,
                Vacancy = new Vacancy
                {
                    Id = 654321,
                    Title = "VacancyTitle",
                    VacancyReference = "VAC000987654"
                }
            };
        }

        public VacancyApplicationBuilder WithStatus(int status)
        {
            _status = status;
            return this;
        }

        public VacancyApplicationBuilder WithLegacyApplicationId(int legacyApplicationId)
        {
            _legacyApplicationId = legacyApplicationId;
            return this;
        }

        public VacancyApplicationBuilder WithUnsuccessfulReason(string unsuccessfulReason)
        {
            _unsuccessfulReason = unsuccessfulReason;
            return this;
        }

        public VacancyApplicationBuilder WithWithdrawnOrDeclinedReason(string withdrawnOrDeclinedReason)
        {
            _withdrawnOrDeclinedReason = withdrawnOrDeclinedReason;
            return this;
        }
    }
}