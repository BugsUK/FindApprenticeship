namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Linq.Expressions;
    using AutoMapper;
    using AutoMapper.Mappers;
    using Domain.Entities.Raa.Locations;
    using Domain.Entities.Raa.Vacancies;
    using Presentation;
    using SFA.Infrastructure.Interfaces;
    using GeoPoint = Domain.Entities.Locations.GeoPoint;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using Vacancy = Entities.Vacancy;

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
            Mapper.CreateMap<DomainVacancy, Vacancy>()
                .IgnoreMember(v => v.ContractOwnerID) // -> null for new entries
                .IgnoreMember(v => v.CountyId) // -> DB Lookup
                .IgnoreMember(v => v.DeliveryOrganisationID) // -> null for new entries
                .ForMember(v => v.LocalAuthorityId, opt => opt.UseValue(8))  // -> GeoMapping story will fill this one
                .IgnoreMember(v => v.OriginalContractOwnerId) // -> null for new entries
                .IgnoreMember(v => v.VacancyLocationTypeId) // DB Lookup
                .MapMemberFrom(v => v.VacancyManagerID, av => av.VacancyManagerId)
                .MapMemberFrom(v => v.VacancyOwnerRelationshipId, av => av.OwnerPartyId)
                .MapMemberFrom(v => v.VacancyStatusId, av => av.Status)
                .MapMemberFrom(v => v.VacancyGuid, av => av.VacancyGuid)
                .MapMemberFrom(v => v.VacancyId, av => av.VacancyId)

                .MapMemberFrom(v => v.AddressLine1, av => av.Address.AddressLine1)
                .MapMemberFrom(v => v.AddressLine2, av => av.Address.AddressLine2)
                .MapMemberFrom(v => v.AddressLine3, av => av.Address.AddressLine3)
                .MapMemberFrom(v => v.AddressLine4, av => av.Address.AddressLine4)
                .MapMemberFrom(v => v.AddressLine5, av => av.Address.AddressLine5)
                .MapMemberFrom(v => v.PostCode, av => av.Address.Postcode)
                .MapMemberFrom(v => v.Town, av => av.Address.Town)
                .MapMemberFrom(v => v.Latitude, av => (decimal)av.Address.GeoPoint.Latitude)  // use a converter?
                .MapMemberFrom(v => v.Longitude, av => (decimal)av.Address.GeoPoint.Longitude) // use a converter?

                .MapMemberFrom(v => v.VacancyReferenceNumber, av => av.VacancyReferenceNumber)
                .MapMemberFrom(v => v.ContactName, av => av.ContactName)
                .MapMemberFrom(v => v.ContactEmail, av => av.ContactEmail)
                .MapMemberFrom(v => v.ContactNumber, av => av.ContactNumber)

                .IgnoreMember(v => v.GeocodeEasting) // Encoding user story
                .IgnoreMember(v => v.GeocodeNorthing) // Encoding user story
                .MapMemberFrom(v => v.Title, av => av.Title)
                .IgnoreMember(v => v.ApprenticeshipType) // DB Lookup from ApprenticeshipLevel? yes
                .MapMemberFrom(v => v.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(v => v.Description, av => av.LongDescription)
                .MapMemberFrom(v => v.WeeklyWage, av => av.Wage) // In migrated vacancies WageUnit will always be Week
                .MapMemberFrom(v => v.WageType, av => av.WageType)
                .ForMember(v => v.WageText, opt => opt.MapFrom(av => new Wage(av.WageType, av.Wage, av.WageUnit).GetDisplayText(av.HoursPerWeek)))
                .ForMember(v => v.NumberOfPositions, opt => opt.ResolveUsing<IntToShortConverter>().FromMember(av => av.NumberOfPositions))
                .MapMemberFrom(v => v.ApplicationClosingDate, av => av.ClosingDate)
                // .MapMemberFrom(v => v.InterviewsFromDate, av => av.InterviewStartDate)
                .MapMemberFrom(v => v.ExpectedStartDate, av => av.PossibleStartDate)
                .ForMember(v => v.ExpectedDuration, opt => opt.MapFrom(av => new Duration(av.DurationType, av.Duration).GetDisplayText()))
                .MapMemberFrom(v => v.WorkingWeek, av => av.WorkingWeek)
                .ForMember(v => v.NumberOfViews, opt => opt.UseValue(0))
                .IgnoreMember(v => v.EmployerAnonymousName)
                .MapMemberFrom(v => v.EmployerDescription, av => av.EmployerDescription)
                .MapMemberFrom(v => v.EmployersWebsite, av => av.EmployerWebsiteUrl)
                .IgnoreMember(v => v.MaxNumberofApplications)
                .MapMemberFrom(v => v.ApplyOutsideNAVMS, av => av.OfflineVacancy)
                .MapMemberFrom(v => v.EmployersApplicationInstructions, av => av.OfflineApplicationInstructions)
                .MapMemberFrom(v => v.EmployersRecruitmentWebsite, av => av.OfflineApplicationUrl)
                .IgnoreMember(v => v.BeingSupportedBy)
                .IgnoreMember(v => v.LockedForSupportUntil)
                .MapMemberFrom(v => v.NoOfOfflineApplicants, av => av.OfflineApplicationClickThroughCount)
                .ForMember(v => v.NoOfOfflineSystemApplicants, opt => opt.UseValue(0))
                .IgnoreMember(v => v.MasterVacancyId) //db lookup
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
                .MapMemberFrom(v => v.TrainingTypeId, av => av.TrainingType)
                .MapMemberFrom(v => v.WageUnitId, av => av.WageUnit)
                .End();

            Mapper.CreateMap<Vacancy, DomainVacancy>()
                .MapMemberFrom(av => av.VacancyId, v => v.VacancyId)
                .MapMemberFrom(av => av.VacancyGuid, v => v.VacancyGuid)
                .MapMemberFrom(av => av.VacancyReferenceNumber, v => v.VacancyReferenceNumber)
                .MapMemberFrom(av => av.OwnerPartyId, v => v.VacancyOwnerRelationshipId)
                .MapMemberFrom(av => av.VacancyManagerId, v => v.VacancyManagerID)
                .MapMemberFrom(av => av.Title, v => v.Title)
                .MapMemberFrom(av => av.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(av => av.LongDescription, v => v.Description)
                .MapMemberFrom(av => av.Wage, v => v.WeeklyWage)
                .MapMemberFrom(av => av.WageType, v => v.WageType)  //db lookup
                .ForMember(av => av.NumberOfPositions, opt => opt.ResolveUsing<ShortToIntConverter>().FromMember(v => v.NumberOfPositions))
                .MapMemberFrom(av => av.ClosingDate, v => v.ApplicationClosingDate)
                //.MapMemberFrom(av => av.InterviewStartDate, v => v.InterviewsFromDate)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.ExpectedStartDate)
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeek)
                .MapMemberFrom(av => av.OfflineVacancy, v => v.ApplyOutsideNAVMS)
                .MapMemberFrom(av => av.OfflineApplicationInstructions, v => v.EmployersApplicationInstructions)
                .MapMemberFrom(av => av.OfflineApplicationUrl, v => v.EmployersRecruitmentWebsite)
                .MapMemberFrom(av => av.OfflineApplicationClickThroughCount, v => v.NoOfOfflineApplicants)
                .IgnoreMember(av => av.ParentVacancyReferenceNumber) // db lookup
                .MapMemberFrom(av => av.EmployerWebsiteUrl, v => v.EmployersWebsite)
                .MapMemberFrom(av => av.VacancyManagerId, v => v.VacancyManagerID.Value)
                .IgnoreMember(av => av.TrainingType)
                .IgnoreMember(av => av.ApprenticeshipLevel)
                .IgnoreMember(av => av.ApprenticeshipLevelComment)
                .IgnoreMember(av => av.FrameworkCodeName)
                .IgnoreMember(av => av.FrameworkCodeNameComment)
                .MapMemberFrom(av => av.StandardId, v => v.StandardId)
                .IgnoreMember(av => av.StandardIdComment)
                .MapMemberFrom(av => av.Status, v => v.VacancyStatusId)
                .IgnoreMember(av => av.WageComment)
                .IgnoreMember(av => av.ClosingDateComment)
                .IgnoreMember(av => av.DurationComment)
                .IgnoreMember(av => av.LongDescriptionComment)
                .IgnoreMember(av => av.PossibleStartDateComment)
                .IgnoreMember(av => av.WorkingWeekComment)
                .IgnoreMember(av => av.FirstQuestionComment)
                .IgnoreMember(av => av.SecondQuestionComment)
                .MapMemberFrom(av => av.AdditionalLocationInformation, v => v.AdditionalLocationInformation)
                .IgnoreMember(av => av.IsEmployerLocationMainApprenticeshipLocation)
                .IgnoreMember(av => av.EmployerDescriptionComment)
                .IgnoreMember(av => av.EmployerWebsiteUrlComment)
                .IgnoreMember(av => av.LocationAddressesComment)
                .IgnoreMember(av => av.NumberOfPositionsComment)
                .IgnoreMember(av => av.AdditionalLocationInformationComment)
                .IgnoreMember(av => av.TrainingProvided)
                .IgnoreMember(av => av.TrainingProvidedComment)
                .MapMemberFrom(av => av.ContactNumber, v => v.ContactNumber)
                .MapMemberFrom(av => av.ContactEmail, v => v.ContactEmail)
                .IgnoreMember(av => av.ContactDetailsComment)
                .IgnoreMember(av => av.TitleComment)
                .IgnoreMember(av => av.ShortDescriptionComment)
                .MapMemberFrom(av => av.HoursPerWeek, v => v.HoursPerWeek)
                .MapMemberFrom(av => av.WageUnit, v => v.WageUnitId)
                .MapMemberFrom(av => av.DurationType, v => v.DurationTypeId)
                .MapMemberFrom(av => av.Duration, v => v.DurationValue)
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
                .MapMemberFrom(av => av.TrainingType, v => v.TrainingTypeId)
                
                .AfterMap((v, av) =>
                {
                    av.Address = new PostalAddress
                    {
                        AddressLine1 = v.AddressLine1,
                        AddressLine2 = v.AddressLine2,
                        AddressLine3 = v.AddressLine3,
                        AddressLine4 = v.AddressLine4,
                        AddressLine5 = v.AddressLine5,
                        Postcode = v.PostCode,
                        Town = v.Town
                    };

                    if (v.Latitude.HasValue && v.Longitude.HasValue)
                    {
                        av.Address.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint
                        {
                            Latitude = (double) v.Latitude.Value,
                            Longitude = (double) v.Longitude.Value
                        };
                    }
                })

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
}