namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using AutoMapper;
    using AutoMapper.Mappers;
    using Domain.Entities.Locations;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using Presentation;
    using SFA.Infrastructure.Interfaces;

    /// <summary>
    /// TODO: Copied because I don't want to depend on SFA.Apprenticeships.Infrastructure.Common.Mappers because this depends on lots of other things
    /// But don't want to move it to SFA.Infrastructure project because that would then depend on AutoMapper
    /// </summary>
    public abstract class MapperEngine : IMapper
    {
        private readonly IMappingEngine _mappingEngine;

        protected MapperEngine()
        {
            Mapper = new ConfigurationStore(new TypeMapFactory(), MapperRegistry.Mappers);
            _mappingEngine = new MappingEngine(Mapper);
            Initialise();
        }

        public ConfigurationStore Mapper { get; private set; }

        public TDestination Map<TSource, TDestination>(TSource sourceObject)
        {
            return _mappingEngine.Map<TDestination>(sourceObject);
        }

        public abstract void Initialise();
    }

    public class ShortToIntConverter : ValueResolver<short?, int?>
    {
        protected override int? ResolveCore(short? source)
        {
            int? result = null;
            if (source.HasValue)
            {
                result = Convert.ToInt32(source.Value);
            }

            return result;
        }
    }

    public class IntToShortConverter : ValueResolver<int?, short?>
    {
        protected override short? ResolveCore(int? source)
        {
            short? result = null;
            if (source.HasValue)
            {
                result = Convert.ToInt16(source.Value);
            }

            return result;
        }
    }
    
    public class ApprenticeshipVacancyMappers : MapperEngine
    {
        public override void Initialise()
        {
            Mapper.CreateMap<ApprenticeshipVacancy, Entities.Vacancy>()
                .IgnoreMember(v => v.ContractOwnerID) // -> null for new entries
                .IgnoreMember(v => v.CountyId) // -> DB Lookup
                .IgnoreMember(v => v.DeliveryOrganisationID) // -> null for new entries
                .ForMember(v => v.LocalAuthorityId, opt => opt.UseValue(8))  // -> GeoMapping story will fill this one
                .IgnoreMember(v => v.OriginalContractOwnerId) // -> null for new entries
                .IgnoreMember(v => v.VacancyLocationTypeId) // TODO
                .IgnoreMember(v => v.VacancyManagerID) // DB Lookup using the vacancyOwnerRelationshipId?
                .IgnoreMember(v => v.VacancyOwnerRelationshipId) // DB Lookup
                .MapMemberFrom(v => v.VacancyStatusId, av => av.Status)
                .MapMemberFrom(v => v.VacancyGuid, av => av.VacancyGuid)
                .MapMemberFrom(v => v.VacancyId, av => av.VacancyId)

                // Map employer address
                .MapMemberFrom(v => v.AddressLine1, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine1)
                .MapMemberFrom(v => v.AddressLine2, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine2)
                .MapMemberFrom(v => v.AddressLine3, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine3)
                .MapMemberFrom(v => v.AddressLine4, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine4)
                .MapMemberFrom(v => v.AddressLine5, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine5)
                .MapMemberFrom(v => v.PostCode, av => av.ProviderSiteEmployerLink.Employer.Address.Postcode)
                .MapMemberFrom(v => v.Town, av => av.ProviderSiteEmployerLink.Employer.Address.Town)
                .IgnoreMember(v=>v.Latitude) // Encoding user story
                .IgnoreMember(v=> v.Longitude) // Encoding user story

                .MapMemberFrom(v => v.VacancyReferenceNumber, av => av.VacancyReferenceNumber)
                .MapMemberFrom(v => v.ContactName, av => av.ContactName)
                .MapMemberFrom(v => v.ContactEmail, av => av.ContactEmail)
                .MapMemberFrom(v => v.ContactNumber, av => av.ContactNumber)

                .IgnoreMember(v => v.GeocodeEasting) // Encoding user story
                .IgnoreMember(v => v.GeocodeNorthing) // Encoding user story
                .MapMemberFrom(v => v.Title, av => av.Title)
                .IgnoreMember(v => v.ApprenticeshipType) // DB Lookup from ApprenticeshipLevel? yes
                .MapMemberFrom(v => v.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(v=> v.Description, av=> av.LongDescription)
                .MapMemberFrom(v => v.WeeklyWage, av => av.Wage) // In migrated vacancies WageUnit will always be Week
                .IgnoreMember(v => v.WageType) // DbLookup
                .ForMember(v => v.WageText, opt => opt.MapFrom(av => new Wage(av.WageType, av.Wage, av.WageUnit).GetDisplayText(av.HoursPerWeek)))
                .ForMember(v => v.NumberOfPositions, opt => opt.ResolveUsing<IntToShortConverter>().FromMember(av => av.NumberOfPositions))
                .MapMemberFrom(v => v.ApplicationClosingDate, av => av.ClosingDate)
                .MapMemberFrom(v => v.InterviewsFromDate, av => av.InterviewStartDate)
                .MapMemberFrom(v => v.ExpectedStartDate, av => av.PossibleStartDate)
                .ForMember( v => v.ExpectedDuration, opt => opt.MapFrom(av => new Duration(av.DurationType, av.Duration).GetDisplayText()))
                .MapMemberFrom(v => v.WorkingWeek, av => av.WorkingWeek)
                .ForMember(v => v.NumberOfViews, opt => opt.UseValue(0))
                .IgnoreMember(v => v.EmployerAnonymousName)
                .MapMemberFrom(v => v.EmployerDescription, av => av.ProviderSiteEmployerLink.Description)
                .MapMemberFrom(v => v.EmployersWebsite, av => av.ProviderSiteEmployerLink.WebsiteUrl)
                .IgnoreMember(v => v.MaxNumberofApplications)
                .MapMemberFrom(v => v.ApplyOutsideNAVMS, av => av.OfflineVacancy)
                .MapMemberFrom(v => v.EmployersApplicationInstructions, av => av.OfflineApplicationInstructions)
                .MapMemberFrom(v => v.EmployersRecruitmentWebsite, av => av.OfflineApplicationUrl)
                .IgnoreMember(v => v.BeingSupportedBy)
                .IgnoreMember(v => v.LockedForSupportUntil)
                .MapMemberFrom(v => v.NoOfOfflineApplicants, av => av.OfflineApplicationClickThroughCount)  // Which one is the right mapping
                .ForMember(v => v.NoOfOfflineSystemApplicants, opt => opt.UseValue(0)) // Which one is the right mapping
                .MapMemberFrom( v=> v.MasterVacancyId, av => av.ParentVacancyId)
                .ForMember(v => v.SmallEmployerWageIncentive, opt => opt.UseValue(false))
                .ForMember(v => v.VacancyManagerAnonymous, opt => opt.UseValue(false))
                .IgnoreMember(v => v.ApprenticeshipFrameworkId) // Change domain entity to use an id
                .MapMemberFrom(v => v.SubmissionCount, av => av.SubmissionCount)
                .MapMemberFrom(v => v.StartedToQADateTime, av => av.DateStartedToQA)//changed to locked field
                .MapMemberFrom(v => v.StandardId, av => av.StandardId)
                .MapMemberFrom(v => v.HoursPerWeek, av => av.HoursPerWeek)
                .MapMemberFrom(v => v.AdditionalLocationInformation, av => av.AdditionalLocationInformation)
                .ForMember(v => v.EditedInRaa, opt => opt.UseValue(true)) // Always true when saving
                .MapMemberFrom(v => v.DurationTypeId, av => av.DurationType)
                .MapMemberFrom(v => v.DurationValue, av => av.Duration)
                .MapMemberFrom(v => v.QAUserName, av => av.QAUserName)
                .End();

            Mapper.CreateMap<Entities.Vacancy, ApprenticeshipVacancy>()
                .MapMemberFrom(av => av.VacancyId, v => v.VacancyId)
                .MapMemberFrom(av => av.VacancyGuid, v => v.VacancyGuid)
                .MapMemberFrom(av => av.VacancyReferenceNumber, v => v.VacancyReferenceNumber)
                .MapMemberFrom(av => av.Title, v => v.Title)
                .MapMemberFrom(av => av.ShortDescription,av => av.ShortDescription)
                .MapMemberFrom(av => av.LongDescription, v => v.Description)
                .MapMemberFrom(av => av.Wage, v => v.WeeklyWage) // In migrated vacancies WageUnit will always be Week
                .MapMemberFrom(av => av.WageType, v => v.WageType) // I can't find any example of wagetypecode in AVMS database
                .ForMember(av => av.NumberOfPositions, opt => opt.ResolveUsing<ShortToIntConverter>().FromMember(v => v.NumberOfPositions))
                .MapMemberFrom(av => av.ClosingDate, v => v.ApplicationClosingDate)
                .MapMemberFrom(av => av.InterviewStartDate, v => v.InterviewsFromDate)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.ExpectedStartDate)
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeek)
                
                .MapMemberFrom(av => av.OfflineVacancy, v => v.ApplyOutsideNAVMS)
                .MapMemberFrom(av => av.OfflineApplicationInstructions, v => v.EmployersApplicationInstructions)
                .MapMemberFrom(av => av.OfflineApplicationUrl, v => v.EmployersRecruitmentWebsite)
                .MapMemberFrom(av => av.OfflineApplicationClickThroughCount, v => v.NoOfOfflineApplicants)  // Which one is the right mapping
                .MapMemberFrom(av => av.OfflineApplicationClickThroughCount, v => v.NoOfOfflineSystemApplicants) // Which one is the right mapping
                .MapMemberFrom(av => av.ParentVacancyId, v => v.MasterVacancyId) // Change to int
                .IgnoreMember(av => av.VacancyManagerId)
                .IgnoreMember(av => av.TrainingType)
                .IgnoreMember(av => av.ApprenticeshipLevel)
                .IgnoreMember(av => av.ApprenticeshipLevelComment)
                .IgnoreMember(av => av.FrameworkCodeName)
                .IgnoreMember(av => av.FrameworkCodeNameComment)
                .MapMemberFrom(av => av.StandardId, v => v.StandardId)
                .IgnoreMember(av => av.StandardIdComment)
                .IgnoreMember(av => av.Status)
                .IgnoreMember(av => av.WageComment)
                .IgnoreMember(av => av.ClosingDateComment)
                .IgnoreMember(av => av.DurationComment)
                .IgnoreMember(av => av.LongDescriptionComment)
                .IgnoreMember(av => av.PossibleStartDateComment)
                .IgnoreMember(av => av.WorkingWeekComment)
                .IgnoreMember(av => av.FirstQuestionComment)
                .IgnoreMember(av => av.SecondQuestionComment)
                .MapMemberFrom(av => av.AdditionalLocationInformation, v => v.AdditionalLocationInformation)
                .IgnoreMember(av => av.LocationAddresses)
                .IgnoreMember(av => av.IsEmployerLocationMainApprenticeshipLocation)
                .IgnoreMember(av => av.EmployerDescriptionComment)
                .IgnoreMember(av => av.EmployerWebsiteUrlComment)
                .IgnoreMember(av => av.LocationAddressesComment)
                .IgnoreMember(av => av.NumberOfPositionsComment)
                .IgnoreMember(av => av.AdditionalLocationInformationComment)
                .IgnoreMember(av => av.TrainingProvided)
                .IgnoreMember(av => av.TrainingProvidedComment)
                .IgnoreMember(av => av.ContactNumber)
                .IgnoreMember(av => av.ContactEmail)
                .IgnoreMember(av => av.ContactDetailsComment)
                .IgnoreMember(av => av.Ukprn)
                .IgnoreMember(av => av.TitleComment)
                .IgnoreMember(av => av.ShortDescriptionComment)
                .MapMemberFrom(av => av.HoursPerWeek, v => v.HoursPerWeek)
                .IgnoreMember(av => av.WageUnit)
                .IgnoreMember(av => av.DurationType)
                .IgnoreMember(av => av.Duration)
                .IgnoreMember(av => av.DesiredSkills)
                .IgnoreMember(av => av.DesiredSkillsComment)
                .IgnoreMember(av => av.FutureProspects)
                .IgnoreMember(av => av.FutureProspectsComment)
                .IgnoreMember(av => av.PersonalQualities)
                .IgnoreMember(av => av.PersonalQualitiesComment)
                .IgnoreMember(av => av.ThingsToConsider)
                .IgnoreMember(av => av.ThingsToConsiderComment)
                .IgnoreMember(av => av.DesiredQualifications)
                .IgnoreMember(av => av.DesiredQualificationsComment)
                .IgnoreMember(av => av.FirstQuestion)
                .IgnoreMember(av => av.SecondQuestion)
                .IgnoreMember(av => av.ProviderSiteEmployerLink)
                .IgnoreMember(av => av.OfflineApplicationUrlComment)
                .IgnoreMember(av => av.OfflineApplicationInstructionsComment)
                .IgnoreMember(av => av.QAUserName)
                .IgnoreMember(av => av.LastEditedById)                
                .IgnoreMember(av => av.DateQAApproved)
                .IgnoreMember(av => av.DateFirstSubmitted)
                .MapMemberFrom(av => av.SubmissionCount, v => v.SubmissionCount)
                .MapMemberFrom(av => av.DateStartedToQA, v => v.StartedToQADateTime)
                .IgnoreMember(av => av.DateSubmitted)
                .MapMemberFrom(av => av.QAUserName, v => v.QAUserName)
                /*.AfterMap((v, av) => 
                {
                    
                    // Map employer address
                //.MapMemberFrom(v => v.AddressLine1, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine1)
                //.MapMemberFrom(v => v.AddressLine2, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine2)
                //.MapMemberFrom(v => v.AddressLine3, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine3)
                //.MapMemberFrom(v => v.AddressLine4, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine4)
                //.MapMemberFrom(v => v.AddressLine5, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine5)
                //.MapMemberFrom(v => v.PostCode, av => av.ProviderSiteEmployerLink.Employer.Address.Postcode)
                //.MapMemberFrom(v => v.Town, av => av.ProviderSiteEmployerLink.Employer.Address.Town)
                //.MapMemberFrom(v => v.Latitude, av => av.ProviderSiteEmployerLink.Employer.Address.GeoPoint.Latitude)
                //.MapMemberFrom(v => v.Longitude, av => av.ProviderSiteEmployerLink.Employer.Address.GeoPoint.Longitude)
                // .MapMemberFrom(v=> v.CountyId, av => av.ProviderSiteEmployerLink.Employer.Address.County) Get from DB?
                // .MapMemberFrom(v => v.EmployerDescription, av => av.ProviderSiteEmployerLink.Description) // Correct?
                // .MapMemberFrom(v => v.EmployerWebsite, av => av.ProviderSiteEmployerLink.WebsiteUrl) //Correct?
                    
                    av.ProviderSiteEmployerLink = new ProviderSiteEmployerLink()
                    {
                        WebsiteUrl = v.EmployerWebsite,
                        Description = v.EmployerDescription,
                    };
                })*/

                .End();

            Mapper.CreateMap<PostalAddress, Schemas.Address.Entities.PostalAddress>()
                .MapMemberFrom(a => a.Latitude, a => a.GeoPoint == null ? null : (decimal?)a.GeoPoint.Latitude)
                .MapMemberFrom(a => a.Longitude, a => a.GeoPoint == null ? null : (decimal?)a.GeoPoint.Longitude)

                .MapMemberFrom(a => a.PostTown, a => a.Town)

                // TODO: Remove from Vacancy.Vacancy?
                .IgnoreMember(a => a.Easting)
                .IgnoreMember(a => a.Northing)

                // TODO: Not in model and may not need to be
                .IgnoreMember(a => a.PostalAddressId) // TODO: Need to add to round-trip...?
                .MapMemberFrom(a => a.DateValidated, a => (DateTime?)null) // Why?
                .MapMemberFrom(a => a.CountyId, a => (int?)null) // done via database lookup -> TODO

                //        .ForMember(a => a.Uprn, opt => opt.Ignore()) // TODO
                ;

            Mapper.CreateMap<Schemas.Address.Entities.PostalAddress, PostalAddress>()
                //.ForMember(a => a.Uprn, opt => opt.Ignore()) // TODO: What is this??
                .MapMemberFrom(a => a.GeoPoint, a => (a.Latitude == null || a.Longitude == null) ? null : new GeoPoint() { Latitude = (double)a.Latitude, Longitude = (double)a.Longitude })
                .MapMemberFrom(a => a.Town, a => a.PostTown)
                .IgnoreMember(a => a.County) // Done by database lookup -> TODO
                // TODO: Hacks
                //.MapMemberFrom(a => a.AddressLine4, a => (a.AddressLine4 + " " + a.AddressLine5).TrimEnd())
                ;
        }
    }

    public static class IMappingExpressionExtensions
    {
        public static IMappingExpression<TSource, TDestination> MapMemberFrom<TSource, TDestination, TMember>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, object>> destinationMember,
            Expression<Func<TSource, TMember>> mapFunction)
        {
            return mappingExpression.ForMember(destinationMember, opt => opt.MapFrom<TMember>(mapFunction));
        }

        public static IMappingExpression<TSource, TDestination> IgnoreMember<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression,
            Expression<Func<TDestination, object>> destinationMember)
        {
            return mappingExpression.ForMember(destinationMember, opt => opt.Ignore());
        }

        /// <summary>
        /// Handy when mappings are being edited so that the terminating ; doesn't keeping moving
        /// </summary>
        /// <typeparam name="TSource"></typeparam>
        /// <typeparam name="TDestination"></typeparam>
        /// <param name="mappingExpression"></param>
        public static void End<TSource, TDestination>(
            this IMappingExpression<TSource, TDestination> mappingExpression)
        { }
    }

    public class CodeEnumMap<T> : IEnumerable<KeyValuePair<string, T>>
    {
        private Map<string, T> _map;

        public CodeEnumMap()
        {
            _map = new Map<string, T>();
        }

        public void Add(string codeValue, T enumValue)
        {
            _map.Add(codeValue ?? "**NULL**", enumValue); // TODO: Outside the otherwise
        }

        public IEnumerator<KeyValuePair<string, T>> GetEnumerator()
        {
            return _map.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return _map.GetEnumerator(); // TODO: Call the right one
        }

        public Map<string, T>.Indexer<string, T> CodeToEnum { get { return _map.Forward; } }
        public Map<string, T>.Indexer<T, string> EnumToCode { get { return _map.Reverse; } }
    }

    public class Map<T1, T2> : IEnumerable<KeyValuePair<T1, T2>>
    {
        private Dictionary<T1, T2> _forward = new Dictionary<T1, T2>();
        private Dictionary<T2, T1> _reverse = new Dictionary<T2, T1>();

        public Map()
        {
            this.Forward = new Indexer<T1, T2>(_forward);
            this.Reverse = new Indexer<T2, T1>(_reverse);
        }

        public class Indexer<T3, T4>
        {
            private Dictionary<T3, T4> _dictionary;
            public Indexer(Dictionary<T3, T4> dictionary)
            {
                _dictionary = dictionary;
            }
            public T4 this[T3 index]
            {
                get { return _dictionary[index]; }
                set { _dictionary[index] = value; }
            }
        }

        public void Add(T1 t1, T2 t2)
        {
            _forward.Add(t1, t2);
            _reverse.Add(t2, t1);
        }

        public IEnumerator<KeyValuePair<T1, T2>> GetEnumerator()
        {
            throw new NotImplementedException();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            throw new NotImplementedException();
        }

        public Indexer<T1, T2> Forward { get; private set; }
        public Indexer<T2, T1> Reverse { get; private set; }

        public T2 this[T1 index]
        {
            get { return Forward[index]; }
        }
    }
}
