namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Linq.Expressions;
    using AutoMapper;
    using DomainVacancyLocation = Domain.Entities.Raa.Locations.VacancyLocation;
    using DomainPostalAddress = Domain.Entities.Raa.Locations.PostalAddress;
    using DbPostalAddress = Address.Entities.PostalAddress;
    using Domain.Entities.Raa.Vacancies;
    using Infrastructure.Common.Mappers;
    using Presentation;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using DbVacancy = Entities.Vacancy;
    using DbVacancyLocation = Entities.VacancyLocation;

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

    public class IsEmployerLocationMainApprenticeshipLocationResolver : ValueResolver<int?, bool>
    {
        protected override bool ResolveCore(int? source)
        {
            return source != (int)VacancyLocationType.MultipleLocations;
        }
    }

    public class VacancyMappers : MapperEngine
    {
        public override void Initialise()
        {
            //TODO: Review the validity of using automapper in this situation and check if every field needs explicitly mapping. It shouldn't be required
            Mapper.CreateMap<DomainVacancy, DbVacancy>()
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
                .MapMemberFrom(v => v.VacancyTypeId, av => av.VacancyType)

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
                .IgnoreMember(v => v.ApprenticeshipType) 
                .MapMemberFrom(v => v.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(v => v.Description, av => av.LongDescription)
                .MapMemberFrom(v => v.WeeklyWage, av => av.Wage)
                .MapMemberFrom(v => v.WageType, av => av.WageType)
                .MapMemberFrom(v => v.WageUnitId, av => av.WageUnit)
                .ForMember(v => v.WageText, opt => opt.MapFrom(av => new Wage(av.WageType, av.Wage, av.WageText, av.WageUnit).GetDisplayText(av.HoursPerWeek)))
                .ForMember(v => v.NumberOfPositions, opt => opt.ResolveUsing<IntToShortConverter>().FromMember(av => av.NumberOfPositions))
                .MapMemberFrom(v => v.ApplicationClosingDate, av => av.ClosingDate)
                .MapMemberFrom(v => v.ExpectedStartDate, av => av.PossibleStartDate)
                .ForMember(v => v.ExpectedDuration, opt => opt.MapFrom(av => new Duration(av.DurationType, av.Duration).GetDisplayText()))
                .MapMemberFrom(v => v.WorkingWeek, av => av.WorkingWeek)
                .ForMember(v => v.NumberOfViews, opt => opt.UseValue(0))
                .MapMemberFrom(v => v.EmployerAnonymousName, av => av.EmployerAnonymousName)
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
                .MapMemberFrom(v => v.MasterVacancyId, av => av.ParentVacancyId)
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
                .ForMember(v => v.WageUnitId, opt => opt.MapFrom(av => av.WageUnit == WageUnit.NotApplicable ? default(int) : av.WageUnit))
                .MapMemberFrom(v => v.UpdatedDateTime, av => av.UpdatedDateTime)
                .IgnoreMember(v => v.SectorId)
                .IgnoreMember(v => v.InterviewsFromDate)
                .MapMemberFrom(v => v.ContractOwnerID, av => av.ProviderId)
                .End();

            Mapper.CreateMap<DbVacancy, DomainVacancy>()
                .MapMemberFrom(av => av.VacancyId, v => v.VacancyId)
                .MapMemberFrom(av => av.VacancyGuid, v => v.VacancyGuid)
                .MapMemberFrom(av => av.VacancyReferenceNumber, v => v.VacancyReferenceNumber)
                .MapMemberFrom(av => av.VacancyType, v => v.VacancyTypeId)
                .MapMemberFrom(av => av.OwnerPartyId, v => v.VacancyOwnerRelationshipId)
                .MapMemberFrom(av => av.Title, v => v.Title)
                .MapMemberFrom(av => av.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(av => av.LongDescription, v => v.Description)
                .MapMemberFrom(av => av.Wage, v => RoundMoney(v.WeeklyWage))
                .MapMemberFrom(av => av.WageType, v => v.WageType)  //db lookup
                .ForMember(av => av.NumberOfPositions, opt => opt.ResolveUsing<ShortToIntConverter>().FromMember(v => v.NumberOfPositions))
                .MapMemberFrom(av => av.ClosingDate, v => v.ApplicationClosingDate)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.ExpectedStartDate)
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeek)
                .MapMemberFrom(av => av.OfflineVacancy, v => v.ApplyOutsideNAVMS)
                .MapMemberFrom(av => av.OfflineApplicationInstructions, v => v.EmployersApplicationInstructions)
                .MapMemberFrom(av => av.OfflineApplicationUrl, v => v.EmployersRecruitmentWebsite)
                .MapMemberFrom(av => av.OfflineApplicationClickThroughCount, v => v.NoOfOfflineApplicants)
                .MapMemberFrom(av => av.ParentVacancyId, v => v.MasterVacancyId)
                .MapMemberFrom(av => av.EmployerWebsiteUrl, v => v.EmployersWebsite)
                .MapMemberFrom(av => av.VacancyManagerId, v => v.VacancyManagerID)
                .IgnoreMember(av => av.TrainingType)
                .IgnoreMember(av => av.ApprenticeshipLevel)
                .IgnoreMember(av => av.ApprenticeshipLevelComment)
                .IgnoreMember(av => av.FrameworkCodeName)
                .IgnoreMember(av => av.FrameworkCodeNameComment)
                .MapMemberFrom(av => av.StandardId, v => v.StandardId)
                .IgnoreMember(av => av.StandardIdComment)
                .MapMemberFrom(av => av.Status, v => v.VacancyStatusId)
                .ForMember(av => av.IsEmployerLocationMainApprenticeshipLocation, opt => opt.ResolveUsing<IsEmployerLocationMainApprenticeshipLocationResolver>().FromMember(v => v.VacancyLocationTypeId))
                .IgnoreMember(av => av.WageComment)
                .IgnoreMember(av => av.ClosingDateComment)
                .IgnoreMember(av => av.DurationComment)
                .IgnoreMember(av => av.LongDescriptionComment)
                .IgnoreMember(av => av.PossibleStartDateComment)
                .IgnoreMember(av => av.WorkingWeekComment)
                .IgnoreMember(av => av.FirstQuestionComment)
                .IgnoreMember(av => av.SecondQuestionComment)
                .MapMemberFrom(av => av.AdditionalLocationInformation, v => v.AdditionalLocationInformation)
                .MapMemberFrom(av => av.EmployerAnonymousName, v => v.EmployerAnonymousName)
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
                .ForMember(av => av.WageUnit, opt => opt.MapFrom(v =>
                    v.WageUnitId.HasValue ? (WageUnit)v.WageUnitId.Value : v.WageType == (int)WageType.LegacyWeekly ? WageUnit.Weekly : WageUnit.NotApplicable))
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
                .IgnoreMember(av => av.CreatedDateTime)
                .MapMemberFrom(av => av.UpdatedDateTime, v => v.UpdatedDateTime)
                .IgnoreMember(av => av.SectorCodeName)
                .IgnoreMember(av => av.SectorCodeNameComment)
                .IgnoreMember(dvl => dvl.Address)
                .IgnoreMember(av => av.RegionalTeam)
                .IgnoreMember(av => av.CreatedByProviderUsername)
                .MapMemberFrom(av => av.VacancyLocationType, v => v.VacancyLocationTypeId.HasValue ? (VacancyLocationType)v.VacancyLocationTypeId.Value : VacancyLocationType.Unknown)
                .MapMemberFrom(av => av.ProviderId, v => v.ContractOwnerID ?? 0)
                .AfterMap((v, av) =>
                {
                    av.Address = new DomainPostalAddress
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



            Mapper.CreateMap<DbVacancy, VacancySummary>()
                .MapMemberFrom(av => av.VacancyId, v => v.VacancyId)
                .MapMemberFrom(av => av.VacancyGuid, v => v.VacancyGuid)
                .MapMemberFrom(av => av.VacancyReferenceNumber, v => v.VacancyReferenceNumber)
                .MapMemberFrom(av => av.VacancyType, v => v.VacancyTypeId)
                .MapMemberFrom(av => av.OwnerPartyId, v => v.VacancyOwnerRelationshipId)
                .MapMemberFrom(av => av.Title, v => v.Title)
                .MapMemberFrom(av => av.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(av => av.Wage, v => v.WeeklyWage)
                .MapMemberFrom(av => av.WageType, v => v.WageType)  //db lookup
                .ForMember(av => av.NumberOfPositions, opt => opt.ResolveUsing<ShortToIntConverter>().FromMember(v => v.NumberOfPositions))
                .MapMemberFrom(av => av.ClosingDate, v => v.ApplicationClosingDate)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.ExpectedStartDate)
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeek)
                .MapMemberFrom(av => av.OfflineVacancy, v => v.ApplyOutsideNAVMS)
                .MapMemberFrom(av => av.OfflineApplicationClickThroughCount, v => v.NoOfOfflineApplicants)
                .MapMemberFrom(av => av.VacancyManagerId, v => v.VacancyManagerID)
                .IgnoreMember(av => av.TrainingType)
                .MapMemberFrom(av => av.ApprenticeshipLevel, v => v.ApprenticeshipType ?? 0)
                .MapMemberFrom(av => av.FrameworkCodeName, v => v.ApprenticeshipFrameworkId.HasValue ? v.ApprenticeshipFrameworkId.ToString() : null)
                .MapMemberFrom(av => av.StandardId, v => v.StandardId)
                .MapMemberFrom(av => av.Status, v => v.VacancyStatusId)
                .ForMember(av => av.IsEmployerLocationMainApprenticeshipLocation, opt => opt.ResolveUsing<IsEmployerLocationMainApprenticeshipLocationResolver>().FromMember(v => v.VacancyLocationTypeId))
                .MapMemberFrom(av => av.EmployerAnonymousName, v => v.EmployerAnonymousName)
                .MapMemberFrom(av => av.HoursPerWeek, v => v.HoursPerWeek)
                .ForMember(av => av.WageUnit, opt => opt.MapFrom(v =>
                    v.WageUnitId.HasValue ? (WageUnit)v.WageUnitId.Value : v.WageType == (int)WageType.LegacyWeekly ? WageUnit.Weekly : WageUnit.NotApplicable))
                .MapMemberFrom(av => av.DurationType, v => v.DurationTypeId)
                .MapMemberFrom(av => av.Duration, v => v.DurationValue)
                .IgnoreMember(av => av.QAUserName)
                .IgnoreMember(av => av.DateQAApproved)
                .MapMemberFrom(av => av.SubmissionCount, v => v.SubmissionCount)
                .MapMemberFrom(av => av.DateStartedToQA, v => v.StartedToQADateTime)
                .IgnoreMember(av => av.DateSubmitted)
                .MapMemberFrom(av => av.QAUserName, v => v.QAUserName)
                .MapMemberFrom(av => av.TrainingType, v => v.TrainingTypeId)
                .MapMemberFrom(av => av.UpdatedDateTime, v => v.UpdatedDateTime)
                .MapMemberFrom(av => av.SectorCodeName, v => v.SectorId.HasValue ? v.SectorId.ToString() : null)
                .IgnoreMember(dvl => dvl.Address)
                .IgnoreMember(av => av.DateFirstSubmitted)
                .MapMemberFrom(av => av.ParentVacancyId, v => v.MasterVacancyId)
                .IgnoreMember(av => av.RegionalTeam)
                .MapMemberFrom(av => av.VacancyLocationType, v => v.VacancyLocationTypeId.HasValue ? (VacancyLocationType)v.VacancyLocationTypeId.Value : VacancyLocationType.Unknown)
                .AfterMap((v, av) =>
                {
                    av.Address = new DomainPostalAddress
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
                            Latitude = (double)v.Latitude.Value,
                            Longitude = (double)v.Longitude.Value
                        };
                    }
                })
                .End();

            Mapper.CreateMap<DomainPostalAddress, DbPostalAddress>()
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

            Mapper.CreateMap<DbPostalAddress, DomainPostalAddress>()
                //.ForMember(a => a.Uprn, opt => opt.Ignore()) // TODO: What is this??
                //.MapMemberFrom(a => a.GeoPoint, a => (a.Latitude == null || a.Longitude == null) ? null : new GeoPoint() { Latitude = (double)a.Latitude, Longitude = (double)a.Longitude })
                .MapMemberFrom(a => a.Town, a => a.PostTown)
                .IgnoreMember(a => a.County) // Done by database lookup -> TODO
                                             // TODO: Hacks
                                             //.MapMemberFrom(a => a.AddressLine4, a => (a.AddressLine4 + " " + a.AddressLine5).TrimEnd())
                .IgnoreMember(dpa => dpa.GeoPoint)
                .AfterMap((dbpa, dpa) =>
                {
                    if (dbpa.Latitude.HasValue && dbpa.Longitude.HasValue)
                    {
                        dpa.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint
                        {
                            Latitude = (double)dbpa.Latitude.Value,
                            Longitude = (double)dbpa.Longitude.Value
                        };
                    }
                })
                ;

            Mapper.CreateMap<DomainVacancyLocation, DbVacancyLocation>()
                .IgnoreMember(dbvl => dbvl.EmployersWebsite)
                .MapMemberFrom(dbvl => dbvl.AddressLine1, dvl => dvl.Address.AddressLine1)
                .MapMemberFrom(dbvl => dbvl.AddressLine2, dvl => dvl.Address.AddressLine2)
                .MapMemberFrom(dbvl => dbvl.AddressLine3, dvl => dvl.Address.AddressLine3)
                .MapMemberFrom(dbvl => dbvl.AddressLine4, dvl => dvl.Address.AddressLine4)
                .MapMemberFrom(dbvl => dbvl.AddressLine5, dvl => dvl.Address.AddressLine5)
                .MapMemberFrom(dbvl => dbvl.PostCode, dvl => dvl.Address.Postcode)
                .MapMemberFrom(dbvl => dbvl.Town, dvl => dvl.Address.Town)
                .MapMemberFrom(dbvl => dbvl.Latitude, dvl => (decimal) dvl.Address.GeoPoint.Latitude)
                // use a converter?
                .MapMemberFrom(dbvl => dbvl.Longitude, dvl => (decimal) dvl.Address.GeoPoint.Longitude)
                // use a converter?
                .IgnoreMember(dbvl => dbvl.CountyId)
                .IgnoreMember(dbvl => dbvl.LocalAuthorityId)
                .IgnoreMember(dbvl => dbvl.GeocodeNorthing)
                .IgnoreMember(dbvl => dbvl.GeocodeEasting);

            Mapper.CreateMap<DbVacancyLocation, DomainVacancyLocation>()
                .IgnoreMember(dvl => dvl.Address)
                .AfterMap((dbvl, dvl) =>
                {
                    dvl.Address = new DomainPostalAddress
                    {
                        AddressLine1 = dbvl.AddressLine1,
                        AddressLine2 = dbvl.AddressLine2,
                        AddressLine3 = dbvl.AddressLine3,
                        AddressLine4 = dbvl.AddressLine4,
                        AddressLine5 = dbvl.AddressLine5,
                        Postcode = dbvl.PostCode,
                        Town = dbvl.Town
                    };

                    if (dbvl.Latitude.HasValue && dbvl.Longitude.HasValue)
                    {
                        dvl.Address.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint
                        {
                            Latitude = (double)dbvl.Latitude.Value,
                            Longitude = (double)dbvl.Longitude.Value
                        };
                    }
                });
        }

        private static decimal? RoundMoney(decimal? money)
        {
            return money.HasValue
                ? Math.Round(money.Value, 2)
                : default(decimal?);
        }
    }

    public static class MappingExpressionExtensions
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
        {
        }
    }
}
