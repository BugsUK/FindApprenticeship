namespace SFA.Apprenticeships.Infrastructure.Repositories.Sql.Schemas.Vacancy
{
    using System;
    using System.Linq.Expressions;
    using AutoMapper;
    using DomainVacancyLocation = Domain.Entities.Raa.Locations.VacancyLocation;
    using DomainPostalAddress = Domain.Entities.Raa.Locations.PostalAddress;
    using DbPostalAddress = Address.Entities.PostalAddress;
    using Domain.Entities.Raa.Vacancies;
    using Domain.Entities.Vacancies;
    using Entities;
    using Infrastructure.Common.Mappers;
    using Presentation;
    using DomainVacancy = Domain.Entities.Raa.Vacancies.Vacancy;
    using DbVacancy = Entities.Vacancy;
    using DbVacancySummary = Entities.VacancySummary;
    using DbVacancyLocation = Entities.VacancyLocation;
    using VacancySummary = Domain.Entities.Raa.Vacancies.VacancySummary;

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

    public class IsEmployerLocationMainApprenticeshipLocationResolver : ValueResolver<int?, bool?>
    {
        protected override bool? ResolveCore(int? source)
        {
            if (source.HasValue)
            {
                return source != (int)VacancyLocationType.MultipleLocations;
            }
            return null;
        }
    }

    public class VacancyMappers : MapperEngine
    {
        private string MapExpectedDuration(DomainVacancy vacancy)
        {
            return !vacancy.Duration.HasValue
                ? vacancy.ExpectedDuration
                : new Duration(vacancy.DurationType, vacancy.Duration).GetDisplayText();
        }

        private Wage MapWage(IVacancyWage vacancy)
        {
            //not sure why we are doing this here??
            //TODO: ensure that moving this from the db => domain entity mapper will not cause issues, then move this logic into Wage object ctor
            var wageType = vacancy.WageType == (int) WageType.LegacyWeekly ? WageType.Custom : (WageType)vacancy.WageType;
            var wageUnit = vacancy.WageUnitId.HasValue ? (WageUnit)vacancy.WageUnitId.Value : vacancy.WageType == (int)WageType.LegacyWeekly ? WageUnit.Weekly : WageUnit.NotApplicable;
            var wageAmount = RoundMoney(vacancy.WeeklyWage);
            return new Wage(wageType, wageAmount, vacancy.WageText, wageUnit, vacancy.HoursPerWeek);
        }

        public override void Initialise()
        {
            //TODO: Review the validity of using automapper in this situation and check if every field needs explicitly mapping. It shouldn't be required
            Mapper.CreateMap<DomainVacancy, DbVacancy>()
                .ForMember(v => v.EditedInRaa, opt => opt.UseValue(true)) // Always true when saving
                .ForMember(v => v.ExpectedDuration, opt => opt.MapFrom(av => MapExpectedDuration(av)))
                .ForMember(v => v.LocalAuthorityId, opt => opt.UseValue(8))  // -> GeoMapping story will fill this one
                .ForMember(v => v.NoOfOfflineSystemApplicants, opt => opt.UseValue(0))
                .ForMember(v => v.NumberOfPositions, opt => opt.ResolveUsing<IntToShortConverter>().FromMember(av => av.NumberOfPositions))
                .ForMember(v => v.NumberOfViews, opt => opt.UseValue(0))
                .ForMember(v => v.SmallEmployerWageIncentive, opt => opt.UseValue(false))
                .ForMember(v => v.VacancyManagerAnonymous, opt => opt.UseValue(false))
                .ForMember(v => v.WageText, opt => opt.MapFrom(av => av.Wage == null ? null : WagePresenter.GetDisplayAmount(av.Wage.Type, av.Wage.Amount, av.Wage.Text, av.Wage.HoursPerWeek, av.PossibleStartDate)))
                .ForMember(v => v.WageType, opt => opt.MapFrom(av => av.Wage == null ? 0 : av.Wage.Type))
                .ForMember(v => v.WageUnitId, opt => opt.MapFrom(av => av.Wage == null ? default(int) : av.Wage.Unit == WageUnit.NotApplicable ? default(int) : av.Wage.Unit))
                .ForMember(v => v.WeeklyWage, opt => opt.MapFrom(av => av.Wage == null ? null : av.Wage.Amount))

                .IgnoreMember(v => v.ApprenticeshipFrameworkId) // Change domain entity to use an id
                .IgnoreMember(v => v.ApprenticeshipType)
                .IgnoreMember(v => v.BeingSupportedBy)
                .IgnoreMember(v => v.CountyId) // -> DB Lookup
                .IgnoreMember(v => v.InterviewsFromDate)
                .IgnoreMember(v => v.LocalAuthorityId)
                .IgnoreMember(v => v.LockedForSupportUntil)
                .IgnoreMember(v => v.MaxNumberofApplications)
                .IgnoreMember(v => v.SectorId)
                .IgnoreMember(v => v.VacancyLocationTypeId) // DB Lookup

                .MapMemberFrom(v => v.AdditionalLocationInformation, av => av.AdditionalLocationInformation)
                .MapMemberFrom(v => v.AdditionalLocationInformationComment, av => av.AdditionalLocationInformationComment)
                .MapMemberFrom(v => v.AddressLine1, av => av.Address.AddressLine1)
                .MapMemberFrom(v => v.AddressLine2, av => av.Address.AddressLine2)
                .MapMemberFrom(v => v.AddressLine3, av => av.Address.AddressLine3)
                .MapMemberFrom(v => v.AddressLine4, av => av.Address.AddressLine4)
                .MapMemberFrom(v => v.AddressLine5, av => av.Address.AddressLine5)
                .MapMemberFrom(v => v.ApplicationClosingDate, av => av.ClosingDate)
                .MapMemberFrom(v => v.ApplyOutsideNAVMS, av => av.OfflineVacancy)
                .MapMemberFrom(v => v.ApprenticeshipLevelComment, av => av.ApprenticeshipLevelComment)
                .MapMemberFrom(v => v.ClosingDateComment, av => av.ClosingDateComment)
                .MapMemberFrom(v => v.ContactDetailsComment, av => av.ContactDetailsComment)
                .MapMemberFrom(v => v.ContactEmail, av => av.ContactEmail)
                .MapMemberFrom(v => v.ContactName, av => av.ContactName)
                .MapMemberFrom(v => v.ContactNumber, av => av.ContactNumber)
                .MapMemberFrom(v => v.ContractOwnerID, av => av.ContractOwnerId)
                .MapMemberFrom(v => v.DateQAApproved, av => av.DateQAApproved)
                .MapMemberFrom(v => v.DateFirstSubmitted, av => av.DateFirstSubmitted)
                .MapMemberFrom(v => v.DateSubmitted, av => av.DateSubmitted)
                .MapMemberFrom(v => v.DeliveryOrganisationID, av => av.DeliveryOrganisationId)
                .MapMemberFrom(v => v.Description, av => av.LongDescription)
                .MapMemberFrom(v => v.DesiredQualifications, av => av.DesiredQualifications)
                .MapMemberFrom(v => v.DesiredQualificationsComment, av => av.DesiredQualificationsComment)
                .MapMemberFrom(v => v.DesiredSkills, av => av.DesiredSkills)
                .MapMemberFrom(v => v.DesiredSkillsComment, av => av.DesiredSkillsComment)
                .MapMemberFrom(v => v.DurationComment, av => av.DurationComment)
                .MapMemberFrom(v => v.DurationTypeId, av => av.DurationType)
                .MapMemberFrom(v => v.DurationValue, av => av.Duration)
                .MapMemberFrom(v => v.EmployerAnonymousName, av => av.EmployerAnonymousName)
                .MapMemberFrom(v => v.EmployerDescription, av => av.EmployerDescription)
                .MapMemberFrom(v => v.EmployerDescriptionComment, av => av.EmployerDescriptionComment)
                .MapMemberFrom(v => v.EmployersApplicationInstructions, av => av.OfflineApplicationInstructions)
                .MapMemberFrom(v => v.EmployersRecruitmentWebsite, av => av.OfflineApplicationUrl)
                .MapMemberFrom(v => v.EmployersWebsite, av => av.EmployerWebsiteUrl)
                .MapMemberFrom(v => v.EmployerWebsiteUrlComment, av => av.EmployerWebsiteUrlComment)
                .MapMemberFrom(v => v.ExpectedStartDate, av => av.PossibleStartDate)
                .MapMemberFrom(v => v.FirstQuestionComment, av => av.FirstQuestionComment)
                .MapMemberFrom(v => v.FrameworkCodeNameComment, av => av.FrameworkCodeNameComment)
                .MapMemberFrom(v => v.FutureProspects, av => av.FutureProspects)
                .MapMemberFrom(v => v.FutureProspectsComment, av => av.FutureProspectsComment)
                .MapMemberFrom(v => v.GeocodeEasting, av => av.Address.GeoPoint.Easting)
                .MapMemberFrom(v => v.GeocodeNorthing, av => av.Address.GeoPoint.Northing)
                .MapMemberFrom(v => v.HoursPerWeek, av => av.Wage.HoursPerWeek)
                .MapMemberFrom(v => v.Latitude, av => (decimal)av.Address.GeoPoint.Latitude)
                .MapMemberFrom(v => v.LocationAddressesComment, av => av.LocationAddressesComment)
                .MapMemberFrom(v => v.LongDescriptionComment, av => av.LongDescriptionComment)
                .MapMemberFrom(v => v.Longitude, av => (decimal)av.Address.GeoPoint.Longitude)
                .MapMemberFrom(v => v.MasterVacancyId, av => av.ParentVacancyId)
                .MapMemberFrom(v => v.NoOfOfflineApplicants, av => av.NoOfOfflineApplicants)
                .MapMemberFrom(v => v.NumberOfPositionsComment, av => av.NumberOfPositionsComment)
                .MapMemberFrom(v => v.OfflineApplicationInstructionsComment, av => av.OfflineApplicationInstructionsComment)
                .MapMemberFrom(v => v.OfflineApplicationUrlComment, av => av.OfflineApplicationUrlComment)
                .MapMemberFrom(v => v.OfflineVacancyTypeId, av => av.OfflineVacancyType)
                .MapMemberFrom(v => v.OriginalContractOwnerId, av => av.OriginalContractOwnerId)
                .MapMemberFrom(v => v.OtherInformation, av => av.OtherInformation)
                .MapMemberFrom(v => v.PersonalQualities, av => av.PersonalQualities)
                .MapMemberFrom(v => v.PersonalQualitiesComment, av => av.PersonalQualitiesComment)
                .MapMemberFrom(v => v.PossibleStartDateComment, av => av.PossibleStartDateComment)
                .MapMemberFrom(v => v.PostCode, av => av.Address.Postcode)
                .MapMemberFrom(v => v.QAUserName, av => av.QAUserName)
                .MapMemberFrom(v => v.SecondQuestionComment, av => av.SecondQuestionComment)
                .MapMemberFrom(v => v.SectorCodeNameComment, av => av.SectorCodeNameComment)
                .MapMemberFrom(v => v.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(v => v.ShortDescriptionComment, av => av.ShortDescriptionComment)
                .MapMemberFrom(v => v.StandardId, av => av.StandardId)
                .MapMemberFrom(v => v.StandardIdComment, av => av.StandardIdComment)
                .MapMemberFrom(v => v.StartedToQADateTime, av => av.DateStartedToQA)//changed to locked field
                .MapMemberFrom(v => v.SubmissionCount, av => av.SubmissionCount)
                .MapMemberFrom(v => v.ThingsToConsider, av => av.ThingsToConsider)
                .MapMemberFrom(v => v.ThingsToConsiderComment, av => av.ThingsToConsiderComment)
                .MapMemberFrom(v => v.Title, av => av.Title)
                .MapMemberFrom(v => v.TitleComment, av => av.TitleComment)
                .MapMemberFrom(v => v.Town, av => av.Address.Town)
                .MapMemberFrom(v => v.TrainingProvided, av => av.TrainingProvided)
                .MapMemberFrom(v => v.TrainingProvidedComment, av => av.TrainingProvidedComment)
                .MapMemberFrom(v => v.TrainingTypeId, av => av.TrainingType)
                .MapMemberFrom(v => v.UpdatedDateTime, av => av.UpdatedDateTime)
                .MapMemberFrom(v => v.VacancyGuid, av => av.VacancyGuid)
                .MapMemberFrom(v => v.VacancyId, av => av.VacancyId)
                .MapMemberFrom(v => v.VacancyManagerID, av => av.VacancyManagerId)
                .MapMemberFrom(v => v.VacancyOwnerRelationshipId, av => av.VacancyOwnerRelationshipId)
                .MapMemberFrom(v => v.VacancyReferenceNumber, av => av.VacancyReferenceNumber)
                .MapMemberFrom(v => v.VacancySourceId, av => av.VacancySource)
                .MapMemberFrom(v => v.VacancyStatusId, av => av.Status)
                .MapMemberFrom(v => v.VacancyTypeId, av => av.VacancyType)
                .MapMemberFrom(v => v.WageComment, av => av.WageComment)
                .MapMemberFrom(v => v.WorkingWeek, av => av.WorkingWeek)
                .MapMemberFrom(v => v.WorkingWeekComment, av => av.WorkingWeekComment)

                .End();

            Mapper.CreateMap<DbVacancy, DomainVacancy>()
                .ForMember(av => av.IsEmployerLocationMainApprenticeshipLocation, opt => opt.ResolveUsing<IsEmployerLocationMainApprenticeshipLocationResolver>().FromMember(v => v.VacancyLocationTypeId))
                .ForMember(av => av.NumberOfPositions, opt => opt.ResolveUsing<ShortToIntConverter>().FromMember(v => v.NumberOfPositions))
                .ForMember(av => av.Wage, opt => opt.MapFrom(v => MapWage(v)))

                .IgnoreMember(av => av.ApplicantCount)
                .IgnoreMember(av => av.CreatedByProviderUsername)
                .IgnoreMember(av => av.CreatedDate)
                .IgnoreMember(av => av.CreatedDateTime)
                .IgnoreMember(av => av.DateFirstSubmitted)
                .IgnoreMember(av => av.DateQAApproved)
                .IgnoreMember(av => av.DateSubmitted)
                .IgnoreMember(av => av.EmployerId)
                .IgnoreMember(av => av.LastEditedById)
                .IgnoreMember(av => av.LocalAuthorityCode)
                .IgnoreMember(av => av.NewApplicationCount)
                .IgnoreMember(av => av.ProviderTradingName)
                .IgnoreMember(av => av.QAUserName)
                .IgnoreMember(av => av.SectorCodeName)
                .IgnoreMember(av => av.SectorCodeNameComment)
                .IgnoreMember(av => av.StandardIdComment)
                .IgnoreMember(av => av.TrainingType)
                .IgnoreMember(dvl => dvl.Address)

                .MapMemberFrom(av => av.ApprenticeshipLevel, v => v.ApprenticeshipLevel)
                .MapMemberFrom(av => av.ApprenticeshipLevelComment, v => v.ApprenticeshipLevelComment)
                .MapMemberFrom(av => av.AdditionalLocationInformation, v => v.AdditionalLocationInformation)
                .MapMemberFrom(av => av.ClosingDate, v => v.ApplicationClosingDate)
                .MapMemberFrom(av => av.ContactEmail, v => v.ContactEmail)
                .MapMemberFrom(av => av.ContactNumber, v => v.ContactNumber)
                .MapMemberFrom(av => av.ContractOwnerId, v => v.ContractOwnerID ?? 0)
                .MapMemberFrom(av => av.DateStartedToQA, v => v.StartedToQADateTime)
                .MapMemberFrom(av => av.DateQAApproved, v => v.DateQAApproved)
                .MapMemberFrom(av => av.DateFirstSubmitted, v => v.DateFirstSubmitted)
                .MapMemberFrom(av => av.DateSubmitted, v => v.DateSubmitted)
                .MapMemberFrom(av => av.DeliveryOrganisationId, v => v.DeliveryOrganisationID)
                .MapMemberFrom(av => av.Duration, v => v.DurationValue)
                .MapMemberFrom(av => av.DurationType, v => v.DurationTypeId)
                .MapMemberFrom(av => av.EditedInRaa, v => v.EditedInRaa)
                .MapMemberFrom(av => av.EmployerAnonymousName, v => v.EmployerAnonymousName)
                .MapMemberFrom(av => av.EmployerWebsiteUrl, v => v.EmployersWebsite)
                .MapMemberFrom(av => av.LongDescription, v => v.Description)
                .MapMemberFrom(av => av.NoOfOfflineApplicants, v => v.NoOfOfflineApplicants)
                .MapMemberFrom(av => av.OfflineApplicationInstructions, v => v.EmployersApplicationInstructions)
                .MapMemberFrom(av => av.OfflineApplicationUrl, v => v.EmployersRecruitmentWebsite)
                .MapMemberFrom(av => av.OfflineVacancy, v => v.ApplyOutsideNAVMS)
                .MapMemberFrom(av => av.OfflineVacancyType, v => (OfflineVacancyType?)v.OfflineVacancyTypeId)
                .MapMemberFrom(av => av.OriginalContractOwnerId, v => v.OriginalContractOwnerId ?? 0)
                .MapMemberFrom(av => av.ParentVacancyId, v => v.MasterVacancyId)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.ExpectedStartDate)
                .MapMemberFrom(av => av.QAUserName, v => v.QAUserName)
                .MapMemberFrom(av => av.ShortDescription, av => av.ShortDescription)
                .MapMemberFrom(av => av.StandardId, v => v.StandardId)
                .MapMemberFrom(av => av.Status, v => v.VacancyStatusId)
                .MapMemberFrom(av => av.SubmissionCount, v => v.SubmissionCount)
                .MapMemberFrom(av => av.Title, v => v.Title)
                .MapMemberFrom(av => av.TrainingType, v => v.TrainingTypeId)
                .MapMemberFrom(av => av.UpdatedDateTime, v => v.UpdatedDateTime)
                .MapMemberFrom(av => av.VacancyGuid, v => v.VacancyGuid)
                .MapMemberFrom(av => av.VacancyId, v => v.VacancyId)
                .MapMemberFrom(av => av.VacancyLocationType, v => v.VacancyLocationTypeId.HasValue ? (VacancyLocationType)v.VacancyLocationTypeId.Value : VacancyLocationType.Unknown)
                .MapMemberFrom(av => av.VacancyManagerId, v => v.VacancyManagerID)
                .MapMemberFrom(av => av.VacancyOwnerRelationshipId, v => v.VacancyOwnerRelationshipId)
                .MapMemberFrom(av => av.VacancyReferenceNumber, v => v.VacancyReferenceNumber)
                .MapMemberFrom(av => av.VacancySource, v => v.VacancySourceId)
                .MapMemberFrom(av => av.VacancyType, v => v.VacancyTypeId)
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeek)
                .MapMemberFrom(v => v.FrameworkCodeName, av => av.FrameworkCodeName)
                .MapMemberFrom(v => v.RegionalTeam, av => av.RegionalTeam)
                .MapMemberFrom(v => v.EmployerName, av => av.EmployerName)
                .MapMemberFrom(v => v.EmployerLocation, av => av.EmployerLocation)


                .AfterMap((v, av) =>
                {
                    if (!string.IsNullOrWhiteSpace(v.AddressLine1) || !string.IsNullOrWhiteSpace(v.AddressLine2)
                        || !string.IsNullOrWhiteSpace(v.AddressLine3) || !string.IsNullOrWhiteSpace(v.AddressLine4)
                        || !string.IsNullOrWhiteSpace(v.AddressLine5) || !string.IsNullOrWhiteSpace(v.PostCode)
                        || !string.IsNullOrWhiteSpace(v.Town))
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
                    }

                    if (av.Address != null)
                    {
                        if ((v.Latitude.HasValue && v.Longitude.HasValue) ||
                        (v.GeocodeEasting.HasValue && v.GeocodeNorthing.HasValue))
                        {
                            av.Address.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint();
                        }

                        if (v.Latitude.HasValue && v.Longitude.HasValue)
                        {
                            av.Address.GeoPoint.Latitude = (double)v.Latitude.Value;
                            av.Address.GeoPoint.Longitude = (double)v.Longitude.Value;
                        }

                        if (v.GeocodeEasting.HasValue && v.GeocodeNorthing.HasValue)
                        {
                            av.Address.GeoPoint.Easting = v.GeocodeEasting.Value;
                            av.Address.GeoPoint.Northing = v.GeocodeNorthing.Value;
                        }
                    }
                })

                .MapMemberFrom(v => v.TrainingProvided, av => av.TrainingProvided)
                .MapMemberFrom(v => v.DesiredQualifications, av => av.DesiredQualifications)
                .MapMemberFrom(v => v.DesiredSkills, av => av.DesiredSkills)
                .MapMemberFrom(v => v.PersonalQualities, av => av.PersonalQualities)
                .MapMemberFrom(v => v.ThingsToConsider, av => av.ThingsToConsider)
                .MapMemberFrom(v => v.FutureProspects, av => av.FutureProspects)
                .MapMemberFrom(v => v.OtherInformation, av => av.OtherInformation)
                .MapMemberFrom(v => v.TitleComment, av => av.TitleComment)
                .MapMemberFrom(v => v.ApprenticeshipLevelComment, av => av.ApprenticeshipLevelComment)
                .MapMemberFrom(v => v.ClosingDateComment, av => av.ClosingDateComment)
                .MapMemberFrom(v => v.ContactDetailsComment, av => av.ContactDetailsComment)
                .MapMemberFrom(v => v.DateQAApproved, av => av.DateQAApproved)
                .MapMemberFrom(v => v.DesiredQualificationsComment, av => av.DesiredQualificationsComment)
                .MapMemberFrom(v => v.DesiredSkillsComment, av => av.DesiredSkillsComment)
                .MapMemberFrom(v => v.DurationComment, av => av.DurationComment)
                .MapMemberFrom(v => v.EmployerDescriptionComment, av => av.EmployerDescriptionComment)
                .MapMemberFrom(v => v.EmployerWebsiteUrlComment, av => av.EmployerWebsiteUrlComment)
                .MapMemberFrom(v => v.FirstQuestionComment, av => av.FirstQuestionComment)
                .MapMemberFrom(v => v.SecondQuestionComment, av => av.SecondQuestionComment)
                .MapMemberFrom(v => v.FrameworkCodeNameComment, av => av.FrameworkCodeNameComment)
                .MapMemberFrom(v => v.FutureProspectsComment, av => av.FutureProspectsComment)
                .MapMemberFrom(v => v.LongDescriptionComment, av => av.LongDescriptionComment)
                .MapMemberFrom(v => v.NumberOfPositionsComment, av => av.NumberOfPositionsComment)
                .MapMemberFrom(v => v.OfflineApplicationInstructionsComment, av => av.OfflineApplicationInstructionsComment)
                .MapMemberFrom(v => v.OfflineApplicationUrlComment, av => av.OfflineApplicationUrlComment)
                .MapMemberFrom(v => v.PersonalQualitiesComment, av => av.PersonalQualitiesComment)
                .MapMemberFrom(v => v.PossibleStartDateComment, av => av.PossibleStartDateComment)
                .MapMemberFrom(v => v.SectorCodeNameComment, av => av.SectorCodeNameComment)
                .MapMemberFrom(v => v.ShortDescriptionComment, av => av.ShortDescriptionComment)
                .MapMemberFrom(v => v.StandardIdComment, av => av.StandardIdComment)
                .MapMemberFrom(v => v.ThingsToConsiderComment, av => av.ThingsToConsiderComment)
                .MapMemberFrom(v => v.TrainingProvidedComment, av => av.TrainingProvidedComment)
                .MapMemberFrom(v => v.WageComment, av => av.WageComment)
                .MapMemberFrom(v => v.WorkingWeekComment, av => av.WorkingWeekComment)
                .MapMemberFrom(v => v.LocationAddressesComment, av => av.LocationAddressesComment)
                .MapMemberFrom(v => v.AdditionalLocationInformationComment, av => av.AdditionalLocationInformationComment)
                .MapMemberFrom(v => v.FrameworkCodeName, av => av.FrameworkCodeName)
                .MapMemberFrom(v => v.RegionalTeam, av => av.RegionalTeam)
                .MapMemberFrom(v => v.EmployerName, av => av.EmployerName)
                .MapMemberFrom(v => v.EmployerLocation, av => av.EmployerLocation)

                .End();

            /*  
                FrameworkCodeName
                RegionalTeam
                EmployerName
                EmployerLocation
             */

            Mapper.CreateMap<DbVacancy, VacancySummary>()
                .ForMember(av => av.IsEmployerLocationMainApprenticeshipLocation, opt => opt.ResolveUsing<IsEmployerLocationMainApprenticeshipLocationResolver>().FromMember(v => v.VacancyLocationTypeId))
                .ForMember(av => av.NumberOfPositions, opt => opt.ResolveUsing<ShortToIntConverter>().FromMember(v => v.NumberOfPositions))
                .ForMember(av => av.Wage, opt => opt.MapFrom(v => MapWage(v)))

                .IgnoreMember(av => av.ApplicantCount)
                .IgnoreMember(av => av.CreatedDate)
                .IgnoreMember(av => av.DateFirstSubmitted)
                .IgnoreMember(av => av.DateQAApproved)
                .IgnoreMember(av => av.DateSubmitted)
                .IgnoreMember(av => av.EmployerId)
                .IgnoreMember(av => av.EmployerLocation)
                .IgnoreMember(av => av.EmployerName)
                .IgnoreMember(av => av.NewApplicationCount)
                .IgnoreMember(av => av.ProviderTradingName)
                .IgnoreMember(av => av.QAUserName)
                .IgnoreMember(av => av.RegionalTeam)
                .IgnoreMember(av => av.TrainingType)
                .IgnoreMember(dvl => dvl.Address)

                .MapMemberFrom(av => av.ApprenticeshipLevel, v => v.ApprenticeshipType ?? 0)
                .MapMemberFrom(av => av.ClosingDate, v => v.ApplicationClosingDate)
                .MapMemberFrom(av => av.ContractOwnerId, v => v.ContractOwnerID ?? 0)
                .MapMemberFrom(av => av.DateStartedToQA, v => v.StartedToQADateTime)
                .MapMemberFrom(av => av.DeliveryOrganisationId, v => v.DeliveryOrganisationID)
                .MapMemberFrom(av => av.Duration, v => v.DurationValue)
                .MapMemberFrom(av => av.DurationType, v => v.DurationTypeId)
                .MapMemberFrom(av => av.EmployerAnonymousName, v => v.EmployerAnonymousName)
                .MapMemberFrom(av => av.FrameworkCodeName, v => v.ApprenticeshipFrameworkId.HasValue ? v.ApprenticeshipFrameworkId.ToString() : null)
                .MapMemberFrom(av => av.NoOfOfflineApplicants, v => v.NoOfOfflineApplicants)
                .MapMemberFrom(av => av.OfflineVacancy, v => v.ApplyOutsideNAVMS)
                .MapMemberFrom(av => av.OriginalContractOwnerId, v => v.OriginalContractOwnerId ?? 0)
                .MapMemberFrom(av => av.ParentVacancyId, v => v.MasterVacancyId)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.ExpectedStartDate)
                .MapMemberFrom(av => av.QAUserName, v => v.QAUserName)
                .MapMemberFrom(av => av.SectorCodeName, v => v.SectorId.HasValue ? v.SectorId.ToString() : null)
                .MapMemberFrom(av => av.ShortDescription, v => v.ShortDescription)
                .MapMemberFrom(av => av.StandardId, v => v.StandardId)
                .MapMemberFrom(av => av.Status, v => v.VacancyStatusId)
                .MapMemberFrom(av => av.SubmissionCount, v => v.SubmissionCount)
                .MapMemberFrom(av => av.Title, v => v.Title)
                .MapMemberFrom(av => av.TrainingType, v => v.TrainingTypeId)
                .MapMemberFrom(av => av.UpdatedDateTime, v => v.UpdatedDateTime)
                .MapMemberFrom(av => av.VacancyGuid, v => v.VacancyGuid)
                .MapMemberFrom(av => av.VacancyId, v => v.VacancyId)
                .MapMemberFrom(av => av.VacancyLocationType, v => v.VacancyLocationTypeId.HasValue ? (VacancyLocationType)v.VacancyLocationTypeId.Value : VacancyLocationType.Unknown)
                .MapMemberFrom(av => av.VacancyManagerId, v => v.VacancyManagerID)
                .MapMemberFrom(av => av.VacancyOwnerRelationshipId, v => v.VacancyOwnerRelationshipId)
                .MapMemberFrom(av => av.VacancyReferenceNumber, v => v.VacancyReferenceNumber)
                .MapMemberFrom(av => av.VacancyType, v => v.VacancyTypeId)
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeek)

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

                    if ((v.Latitude.HasValue && v.Longitude.HasValue) ||
                        (v.GeocodeEasting.HasValue && v.GeocodeNorthing.HasValue))
                    {
                        av.Address.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint();
                    }

                    if (v.Latitude.HasValue && v.Longitude.HasValue)
                    {
                        av.Address.GeoPoint.Latitude = (double)v.Latitude.Value;
                        av.Address.GeoPoint.Longitude = (double)v.Longitude.Value;
                    }

                    if (v.GeocodeEasting.HasValue && v.GeocodeNorthing.HasValue)
                    {
                        av.Address.GeoPoint.Easting = v.GeocodeEasting.Value;
                        av.Address.GeoPoint.Northing = v.GeocodeNorthing.Value;
                    }
                })
                .End();


            Mapper.CreateMap<DbVacancySummary, VacancySummary>()
                .ForMember(av => av.Wage, opt => opt.MapFrom(v => MapWage(v)))
                .ForMember(av => av.IsEmployerLocationMainApprenticeshipLocation, opt => opt.ResolveUsing<IsEmployerLocationMainApprenticeshipLocationResolver>().FromMember(v => v.VacancyLocationTypeId))

                .MapMemberFrom(av => av.ContractOwnerId, v => v.ContractOwnerId)
                .MapMemberFrom(av => av.DateFirstSubmitted, v => v.DateFirstSubmitted)
                .MapMemberFrom(av => av.DateQAApproved, v => v.DateQAApproved)
                .MapMemberFrom(av => av.DateSubmitted, v => v.DateSubmitted)
                .MapMemberFrom(av => av.DeliveryOrganisationId, v => v.DeliveryOrganisationId)
                .MapMemberFrom(av => av.Duration, v => v.Duration)
                .MapMemberFrom(av => av.DurationType, v => v.DurationType)
                .MapMemberFrom(av => av.EmployerAnonymousName, v => v.EmployerAnonymousName)
                .MapMemberFrom(av => av.ExpectedDuration, v => v.ExpectedDuration)
                .IgnoreMember(av => av.IsEmployerLocationMainApprenticeshipLocation)
                .MapMemberFrom(av => av.NumberOfPositions, v => v.NumberOfPositions)
                .MapMemberFrom(av => av.OriginalContractOwnerId, v => v.OriginalContractOwnerId)
                .MapMemberFrom(av => av.ParentVacancyId, v => v.ParentVacancyId)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.PossibleStartDate)
                .MapMemberFrom(av => av.QAUserName, v => v.QAUserName)
                .MapMemberFrom(av => av.RegionalTeam, v => v.RegionalTeam)
                .MapMemberFrom(av => av.TrainingType, v => v.TrainingTypeId)
                .IgnoreMember(av => av.UpdatedDateTime)
                .MapMemberFrom(av => av.VacancyLocationType, v => v.VacancyLocationTypeId)
                .MapMemberFrom(av => av.VacancyManagerId, v => v.VacancyManagerId)
                .MapMemberFrom(av => av.WorkingWeek, v => v.WorkingWeek)
                .IgnoreMember(dvl => dvl.Address)

                .MapMemberFrom(av => av.ApplicantCount, v => v.ApplicantCount)
                .MapMemberFrom(av => av.ApprenticeshipLevel, v => v.ApprenticeshipLevel)
                .MapMemberFrom(av => av.ClosingDate, v => v.ApplicationClosingDate)
                .MapMemberFrom(av => av.ContractOwnerId, v => v.ContractOwnerId)
                .MapMemberFrom(av => av.CreatedDate, v => v.CreatedDate)
                .MapMemberFrom(av => av.DateFirstSubmitted, v => v.DateFirstSubmitted)
                .MapMemberFrom(av => av.DateQAApproved, v => v.DateQAApproved)
                .MapMemberFrom(av => av.DateStartedToQA, v => v.StartedToQADateTime)
                .MapMemberFrom(av => av.DateSubmitted, v => v.DateSubmitted)
                .MapMemberFrom(av => av.EmployerId, v => v.EmployerId)
                .MapMemberFrom(av => av.EmployerLocation, v => v.EmployerLocation)
                .MapMemberFrom(av => av.EmployerName, v => v.EmployerName)
                .MapMemberFrom(av => av.FrameworkCodeName, v => v.FrameworkCodeName)
                .MapMemberFrom(av => av.NewApplicationCount, v => v.NewApplicantCount)
                .MapMemberFrom(av => av.NoOfOfflineApplicants, v => v.NoOfOfflineApplicants)
                .MapMemberFrom(av => av.NumberOfPositions, v => v.NumberOfPositions)
                .MapMemberFrom(av => av.OfflineVacancy, v => v.ApplyOutsideNAVMS)
                .MapMemberFrom(av => av.PossibleStartDate, v => v.PossibleStartDate)
                .MapMemberFrom(av => av.ProviderTradingName, v => v.ProviderTradingName)
                .MapMemberFrom(av => av.QAUserName, v => v.QAUserName)
                .MapMemberFrom(av => av.SectorCodeName, v => v.SectorCodeName)
                .MapMemberFrom(av => av.ShortDescription, v => v.ShortDescription)
                .MapMemberFrom(av => av.StandardId, v => v.StandardId)
                .MapMemberFrom(av => av.Status, v => v.VacancyStatusId)
                .MapMemberFrom(av => av.SubmissionCount, v => v.SubmissionCount)
                .MapMemberFrom(av => av.Title, v => v.Title)
                .MapMemberFrom(av => av.VacancyGuid, v => v.VacancyGuid)
                .MapMemberFrom(av => av.VacancyId, v => v.VacancyId)
                .MapMemberFrom(av => av.VacancyOwnerRelationshipId, v => v.VacancyOwnerRelationshipId)
                .MapMemberFrom(av => av.VacancyReferenceNumber, v => v.VacancyReferenceNumber)
                .MapMemberFrom(av => av.VacancyType, v => v.VacancyTypeId)

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

                    if ((v.Latitude.HasValue && v.Longitude.HasValue) ||
                        (v.GeocodeEasting.HasValue && v.GeocodeNorthing.HasValue))
                    {
                        av.Address.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint();
                    }

                    if (v.Latitude.HasValue && v.Longitude.HasValue)
                    {
                        av.Address.GeoPoint.Latitude = (double)v.Latitude.Value;
                        av.Address.GeoPoint.Longitude = (double)v.Longitude.Value;
                    }

                    if (v.GeocodeEasting.HasValue && v.GeocodeNorthing.HasValue)
                    {
                        av.Address.GeoPoint.Easting = v.GeocodeEasting.Value;
                        av.Address.GeoPoint.Northing = v.GeocodeNorthing.Value;
                    }
                })
                .End();

            Mapper.CreateMap<DomainPostalAddress, DbPostalAddress>()
                .MapMemberFrom(a => a.Latitude, a => a.GeoPoint == null ? null : (decimal?)a.GeoPoint.Latitude)
                .MapMemberFrom(a => a.Longitude, a => a.GeoPoint == null ? null : (decimal?)a.GeoPoint.Longitude)
                .MapMemberFrom(a => a.Easting, a => a.GeoPoint == null ? null : (int?)a.GeoPoint.Easting)
                .MapMemberFrom(a => a.Northing, a => a.GeoPoint == null ? null : (int?)a.GeoPoint.Northing)

                .MapMemberFrom(a => a.PostTown, a => a.Town)

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
                    if ((dbpa.Latitude.HasValue && dbpa.Longitude.HasValue) || (dbpa.Easting.HasValue && dbpa.Northing.HasValue))
                    {
                        dpa.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint();
                    }

                    if (dbpa.Latitude.HasValue && dbpa.Longitude.HasValue)
                    {
                        dpa.GeoPoint.Latitude = (double) dbpa.Latitude.Value;
                        dpa.GeoPoint.Longitude = (double) dbpa.Longitude.Value;
                    }

                    if (dbpa.Easting.HasValue && dbpa.Northing.HasValue)
                    {
                        dpa.GeoPoint.Easting = dbpa.Easting.Value;
                        dpa.GeoPoint.Northing = dbpa.Northing.Value;
                    }
                })
                ;

            Mapper.CreateMap<DomainVacancyLocation, DbVacancyLocation>()
                .MapMemberFrom(dbvl => dbvl.EmployersWebsite, dvl => dvl.EmployersWebsite)
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
                .MapMemberFrom(dbvl => dbvl.GeocodeEasting, dvl => dvl.Address.GeoPoint.Easting)
                .MapMemberFrom(dbvl => dbvl.GeocodeNorthing, dvl => dvl.Address.GeoPoint.Northing)
                // use a converter?
                .IgnoreMember(dbvl => dbvl.CountyId)
                .IgnoreMember(dbvl => dbvl.LocalAuthorityId);

            Mapper.CreateMap<DbVacancyLocation, DomainVacancyLocation>()
                .IgnoreMember(dvl => dvl.Address)
                .IgnoreMember(dvl => dvl.LocalAuthorityCode)
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

                    if ((dbvl.Latitude.HasValue && dbvl.Longitude.HasValue) || 
                        (dbvl.GeocodeEasting.HasValue && dbvl.GeocodeNorthing.HasValue))
                    {
                        dvl.Address.GeoPoint = new Domain.Entities.Raa.Locations.GeoPoint();
                    }

                    if (dbvl.Latitude.HasValue && dbvl.Longitude.HasValue)
                    {
                        dvl.Address.GeoPoint.Latitude = (double)dbvl.Latitude.Value;
                        dvl.Address.GeoPoint.Longitude = (double)dbvl.Longitude.Value;
                    }

                    if (dbvl.GeocodeEasting.HasValue && dbvl.GeocodeNorthing.HasValue)
                    {
                        dvl.Address.GeoPoint.Easting = dbvl.GeocodeEasting.Value;
                        dvl.Address.GeoPoint.Northing = dbvl.GeocodeNorthing.Value;
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
