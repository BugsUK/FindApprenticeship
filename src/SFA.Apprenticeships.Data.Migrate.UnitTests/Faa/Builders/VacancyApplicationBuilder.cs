namespace SFA.Apprenticeships.Data.Migrate.UnitTests.Faa.Builders
{
    using System;
    using Migrate.Faa.Entities.Mongo;

    public class VacancyApplicationBuilder
    {
        private ApplicationStatuses _status = ApplicationStatuses.Submitted;
        private int _legacyApplicationId = 123456;
        private string _unsuccessfulReason;
        private string _withdrawnOrDeclinedReason;
        private ApplicationTemplate _applicationTemplate;
        private string _notes;

        public VacancyApplication Build()
        {
            return new VacancyApplication
            {
                Id = Guid.NewGuid(),
                DateCreated = DateTime.Now.AddDays(-7),
                DateUpdated = DateTime.Now,
                Status = _status,
                DateApplied = DateTime.Now.AddDays(-6),
                CandidateId = Guid.NewGuid(),
                LegacyApplicationId = _legacyApplicationId,
                CandidateInformation = _applicationTemplate,
                Notes = _notes,
                SuccessfulDateTime = DateTime.Now.AddDays(-1),
                UnsuccessfulDateTime = DateTime.Now.AddDays(-2),
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

        public VacancyApplicationBuilder WithStatus(ApplicationStatuses status)
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

        public VacancyApplicationBuilder WithApplicationTemplate(ApplicationTemplate applicationTemplate)
        {
            _applicationTemplate = applicationTemplate;
            return this;
        }

        public VacancyApplicationBuilder WithNotes(string notes)
        {
            _notes = notes;
            return this;
        }
    }
}