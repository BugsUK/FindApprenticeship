namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq.Expressions;
    using AutoMapper;
    using AutoMapper.Mappers;
    using Domain.Entities.Locations;
    using Domain.Entities.Providers;
    using Domain.Entities.Vacancies;
    using Domain.Entities.Vacancies.ProviderVacancies;
    using Domain.Entities.Vacancies.ProviderVacancies.Apprenticeship;
    using SFA.Infrastructure.Interfaces;
    using VacancyLocationType = Sql.Schemas.Vacancy.Entities.VacancyLocationType;

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
            //var wageTypeMap = new CodeEnumMap<WageType>
            //{
            //    { "NAP", 0},
            //    { "AMW", WageType.ApprenticeshipMinimumWage },
            //    { "NMW", WageType.NationalMinimumWage },
            //    { "CUS", WageType.Custom }
            //};

            //var wageUnitMap = new CodeEnumMap<WageUnit>
            //{
            //    { "N", WageUnit.NotApplicable },
            //    { "W", WageUnit.Weekly },
            //    { "M", WageUnit.Monthly },
            //    { "A", WageUnit.Annually },
            //};

            //var durationTypeMap = new CodeEnumMap<DurationType>
            //{
            //    { "U", DurationType.Unknown },
            //    { "W", DurationType.Weeks },
            //    { "M", DurationType.Months },
            //    { "Y", DurationType.Years }
            //};

            //var trainingTypeMap = new CodeEnumMap<TrainingType>
            //{
            //    { "F", TrainingType.Frameworks },
            //    { "S", TrainingType.Standards },
            //    { "U", TrainingType.Unknown }, // TODO: null = blank??
            //};

            //var apprenticeshipLevelMap = new CodeEnumMap<ApprenticeshipLevel>
            //{
            //    { "2", ApprenticeshipLevel.Intermediate },
            //    { "3", ApprenticeshipLevel.Advanced },
            //    { "6", ApprenticeshipLevel.Degree },
            //    { "5", ApprenticeshipLevel.FoundationDegree },
            //    { "4", ApprenticeshipLevel.Higher },
            //    { "7", ApprenticeshipLevel.Masters },
            //    { "0", ApprenticeshipLevel.Unknown } // TODO: review
            //};

            //var vacancyStatusMap = new CodeEnumMap<ProviderVacancyStatuses>
            //{
            //    { "LIV", ProviderVacancyStatuses.Live },
            //    { "CLD", ProviderVacancyStatuses.Closed },
            //    { "DRA", ProviderVacancyStatuses.Draft },
            //    { "PQA", ProviderVacancyStatuses.PendingQA },
            //    { "REF", ProviderVacancyStatuses.RejectedByQA },
            //    { "RES", ProviderVacancyStatuses.ReservedForQA },
            //    { "PAR", ProviderVacancyStatuses.ParentVacancy },
            //    { "UNK", ProviderVacancyStatuses.Unknown}
            //};

            //Mapper.CreateMap<string, ProviderVacancyStatuses>().ConvertUsing(code => vacancyStatusMap.CodeToEnum[code]);
            //Mapper.CreateMap<ProviderVacancyStatuses, string>().ConvertUsing(status => vacancyStatusMap.EnumToCode[status]);
            
            Mapper.CreateMap<ApprenticeshipVacancy, Entities.Vacancy>()
                // Ignore (maybe temporaly) ids from other entities
                // .IgnoreMember(v => v.FrameworkId) // -> Map from FrameworkCodeName
                .IgnoreMember(v => v.ContractOwnerID)
                .IgnoreMember(v => v.CountyId)
                .IgnoreMember(v => v.DeliveryOrganisationID)
                .IgnoreMember(v => v.LocalAuthorityId)
                .IgnoreMember(v => v.OriginalContractOwnerId)
                .IgnoreMember(v => v.VacancyLocationTypeId)
                .IgnoreMember(v => v.VacancyManagerID)
                // .IgnoreMember(v => v.VacancyOwnerRelationshipId)
                .ForMember( v=> v.VacancyOwnerRelationshipId, opt => opt.UseValue(2)) // Hardcoded for testing puroposes
                .MapMemberFrom(v => v.VacancyStatusId, av => av.Status)
                .MapMemberFrom(v => v.VacancyGuid, av => av.EntityId)
                .IgnoreMember(v => v.VacancyId)
                
                // Map employer address
                .MapMemberFrom(v => v.AddressLine1, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine1)
                .MapMemberFrom(v => v.AddressLine2, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine2)
                .MapMemberFrom(v => v.AddressLine3, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine3)
                .MapMemberFrom(v => v.AddressLine4, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine4)
                .MapMemberFrom(v => v.AddressLine5, av => av.ProviderSiteEmployerLink.Employer.Address.AddressLine5)
                .MapMemberFrom(v => v.PostCode, av => av.ProviderSiteEmployerLink.Employer.Address.Postcode)
                .MapMemberFrom(v => v.Town, av => av.ProviderSiteEmployerLink.Employer.Address.Town)
                //.MapMemberFrom(v => v.Latitude, av => av.ProviderSiteEmployerLink.Employer.Address.GeoPoint.Latitude) // From double to deciaml?
                //.MapMemberFrom(v => v.Longitude, av => av.ProviderSiteEmployerLink.Employer.Address.GeoPoint.Longitude) // From double to deciaml?
                .IgnoreMember(v=>v.Latitude)
                .IgnoreMember(v=> v.Longitude)
                
                // .MapMemberFrom(v=> v.CountyId, av => av.ProviderSiteEmployerLink.Employer.Address.County) Get from DB?

                .MapMemberFrom(v => v.VacancyReferenceNumber, av => av.VacancyReferenceNumber)
                .MapMemberFrom(v => v.ContactName, av => av.ContactName)
                .MapMemberFrom(v => v.ContactEmail, av => av.ContactEmail)
                .MapMemberFrom(v => v.ContactNumber, av => av.ContactNumber)

                .IgnoreMember(v => v.GeocodeEasting)
                .IgnoreMember(v => v.GeocodeNorthing)
                .MapMemberFrom(v => v.Title, av => av.Title)
                .IgnoreMember(v => v.ApprenticeshipType) // DB Lookup from ApprenticeshipLevel?
                .MapMemberFrom(v => v.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(v=> v.Description, av=> av.LongDescription)
                .MapMemberFrom(v => v.WeeklyWage, av => av.Wage) // In migrated vacancies WageUnit will always be Week
                .MapMemberFrom(v => v.WageType, av => av.WageType) // I can't find any example of wagetypecode in AVMS database
                .IgnoreMember(v => v.WageText) // How are we going to map this?
                // .MapMemberFrom(v => v.NumberOfPositions, av => av.NumberOfPositions)
                .ForMember(v => v.NumberOfPositions, opt => opt.ResolveUsing<IntToShortConverter>().FromMember(av => av.NumberOfPositions))
                .MapMemberFrom(v => v.ApplicationClosingDate, av => av.ClosingDate)
                .MapMemberFrom(v => v.InterviewsFromDate, av => av.InterviewStartDate)
                .MapMemberFrom(v => v.ExpectedStartDate, av => av.PossibleStartDate)
                .IgnoreMember( v => v.ExpectedDuration) // How are we going to map this?
                .MapMemberFrom(v => v.WorkingWeek, av => av.WorkingWeek)
                .IgnoreMember(v => v.NumberOfViews)
                .IgnoreMember(v=>v.EmployerAnonymousName)
                .MapMemberFrom(v=> v.EmployerDescription, av => av.ProviderSiteEmployerLink.Description) // Correct?
                .MapMemberFrom(v => v.EmployersWebsite, av => av.ProviderSiteEmployerLink.WebsiteUrl) //Correct?
                .IgnoreMember(v => v.MaxNumberofApplications)
                .MapMemberFrom(v => v.ApplyOutsideNAVMS, av => av.OfflineVacancy)
                .MapMemberFrom(v => v.EmployersApplicationInstructions, av => av.OfflineApplicationInstructions)
                .MapMemberFrom(v => v.EmployersRecruitmentWebsite, av => av.OfflineApplicationUrl)
                .IgnoreMember(v => v.BeingSupportedBy)
                .IgnoreMember( v=> v.LockedForSupportUntil)
                .MapMemberFrom( v => v.NoOfOfflineApplicants, av => av.OfflineApplicationClickThroughCount)  // Which one is the right mapping
                .MapMemberFrom( v => v.NoOfOfflineSystemApplicants, av => av.OfflineApplicationClickThroughCount) // Which one is the right mapping
                // .MapMemberFrom( v=> v.MasterVacancyId, av => av.ParentVacancyId) // PAremtVacancyId needs to be an int
                .IgnoreMember(v => v.MasterVacancyId)
                .IgnoreMember(v => v.SmallEmployerWageIncentive)
                .IgnoreMember( v=> v.VacancyManagerAnonymous)
                .IgnoreMember(v => v.ApprenticeshipFrameworkId) // Db lookup?
                .End();

            Mapper.CreateMap<Entities.Vacancy, ApprenticeshipVacancy>()
                .MapMemberFrom(av => av.EntityId, v => v.VacancyGuid)
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
                // .MapMemberFrom(av => av.ParentVacancyId, v => v.MasterVacancyId) // Change to int
                .IgnoreMember(av => av.ParentVacancyId)
                .IgnoreMember(av => av.VacancyManagerId)
                .IgnoreMember(av => av.TrainingType)
                .IgnoreMember(av => av.ApprenticeshipLevel)
                .IgnoreMember(av => av.ApprenticeshipLevelComment)
                .IgnoreMember(av => av.FrameworkCodeName)
                .IgnoreMember(av => av.FrameworkCodeNameComment)
                .IgnoreMember(av => av.StandardId)
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
                .IgnoreMember(av => av.AdditionalLocationInformation)
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
                .IgnoreMember(av => av.HoursPerWeek)
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
                .IgnoreMember(av => av.DateSubmitted)
                .IgnoreMember(av => av.DateFirstSubmitted)
                .IgnoreMember(av => av.DateStartedToQA)
                .IgnoreMember(av => av.QAUserName)
                .IgnoreMember(av => av.DateQAApproved)
                .IgnoreMember(av => av.SubmissionCount)
                .IgnoreMember(av => av.LastEditedById)
                .IgnoreMember(av => av.DateCreated)
                .IgnoreMember(av => av.DateUpdated)
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
