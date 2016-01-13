namespace SFA.Apprenticeships.NewDB.Domain.Entities
{
    using System;
    using System.Data.Entity;
    using System.ComponentModel.DataAnnotations.Schema;
    using System.Linq;

    public partial class Model1 : DbContext
    {
        public Model1()
            : base("name=Model1")
        {
        }

        public virtual DbSet<PostalAddress> PostalAddresses { get; set; }
        public virtual DbSet<ValidationSource> ValidationSources { get; set; }
        public virtual DbSet<C__RefactorLog> C__RefactorLog { get; set; }
        public virtual DbSet<County> Counties { get; set; }
        public virtual DbSet<Framework> Frameworks { get; set; }
        public virtual DbSet<FrameworkStatu> FrameworkStatus { get; set; }
        public virtual DbSet<Level> Levels { get; set; }
        public virtual DbSet<Occupation> Occupations { get; set; }
        public virtual DbSet<OccupationStatu> OccupationStatus { get; set; }
        public virtual DbSet<Sector> Sectors { get; set; }
        public virtual DbSet<Standard> Standards { get; set; }
        public virtual DbSet<Vacancy.DurationType> DurationTypes { get; set; }
        public virtual DbSet<Vacancy.TrainingType> TrainingTypes { get; set; }
        public virtual DbSet<Vacancy.Vacancy> Vacancies { get; set; }
        public virtual DbSet<Vacancy.VacancyLocation> VacancyLocations { get; set; }
        public virtual DbSet<Vacancy.VacancyLocationType> VacancyLocationTypes { get; set; }
        public virtual DbSet<Vacancy.VacancyParty> VacancyParties { get; set; }
        public virtual DbSet<Vacancy.VacancyPartyRelationship> VacancyPartyRelationships { get; set; }
        public virtual DbSet<Vacancy.VacancyPartyRelationshipType> VacancyPartyRelationshipTypes { get; set; }
        public virtual DbSet<Vacancy.VacancyPartyType> VacancyPartyTypes { get; set; }
        public virtual DbSet<Vacancy.VacancyStatu> VacancyStatus { get; set; }
        public virtual DbSet<Vacancy.VacancyType> VacancyTypes { get; set; }
        public virtual DbSet<Vacancy.WageType> WageTypes { get; set; }
        public virtual DbSet<WebProxyConsumer> WebProxyConsumers { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {

        }
    }
}
