namespace SFA.Apprenticeships.Web.Manage.UnitTests.Validators.VacancyPosting
{
    using System;
    using Common.UnitTests.Validators;
    using Common.Validators;
    using Common.ViewModels;
    using Domain.Entities.Raa.Vacancies;
    using FluentValidation;
    using NUnit.Framework;
    using Ploeh.AutoFixture;
    using Raa.Common.Validators.Vacancy;
    using Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    internal class DifferentVacancySourceValidationTests

    {
        [SetUp]
        public void SetUp()
        {
            _aggregateValidator = new VacancyViewModelValidator();
        }

        private VacancyViewModelValidator _aggregateValidator;

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void SkillsRequiredValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.DesiredSkills = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredSkills, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void DesiredQualificationsValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.DesiredQualifications = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.DesiredQualifications, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void PersonalQualitiesValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.PersonalQualities = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.PersonalQualities, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        public void FutureProspectsValidation(VacancySource vacancySource, string text, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.VacancyRequirementsProspectsViewModel.FutureProspects = text;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.VacancyRequirementsProspectsViewModel,
                    vm => vm.VacancyRequirementsProspectsViewModel.FutureProspects, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, " ", false)]
        [TestCase(VacancySource.Raa, "12 - 14 Months", false)]
        [TestCase(VacancySource.Raa, "11", true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, " ", true)]
        [TestCase(VacancySource.Av, "12 - 14 Months", true)]
        [TestCase(VacancySource.Av, "11", true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, " ", true)]
        [TestCase(VacancySource.Api, "12 - 14 Months", true)]
        [TestCase(VacancySource.Api, "11", true)]
        public void DurationValidation(VacancySource vacancySource, string durationString, bool expectValid)
        {
            int? duration = null;
            int parsedDuration;
            if (int.TryParse(durationString, out parsedDuration))
            {
                duration = parsedDuration;
            }
            
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
            {
                Duration = duration,
                VacancySource = vacancySource,
                HoursPerWeek = 40
            };

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, " ", false)]
        [TestCase(VacancySource.Raa, "30", true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, " ", true)]
        [TestCase(VacancySource.Av, "30", true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, " ", true)]
        [TestCase(VacancySource.Api, "30", true)]
        public void HoursPerWeekValidation(VacancySource vacancySource, string hoursPerWeekString, bool expectValid)
        {
            decimal? hoursPerWeek = null;
            decimal parsedHoursPerWeek;
            if (decimal.TryParse(hoursPerWeekString, out parsedHoursPerWeek))
            {
                hoursPerWeek = parsedHoursPerWeek;
            }
            
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
            {
                HoursPerWeek = hoursPerWeek,
                VacancySource = vacancySource
            };

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, null, false)]
        [TestCase(VacancySource.Raa, "", false)]
        [TestCase(VacancySource.Raa, " ", false)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeHtmlText, true)]
        [TestCase(VacancySource.Av, null, true)]
        [TestCase(VacancySource.Av, "", true)]
        [TestCase(VacancySource.Av, " ", true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeHtmlText, true)]
        [TestCase(VacancySource.Api, null, true)]
        [TestCase(VacancySource.Api, "", true)]
        [TestCase(VacancySource.Api, " ", true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeHtmlText, true)]
        public void TrainingProvidedRequired(VacancySource vacancySource, string trainingProvided, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.TrainingDetailsViewModel = new TrainingDetailsViewModel
            {
                TrainingProvided = trainingProvided,
                VacancySource = vacancySource
            };

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.TrainingDetailsViewModel, vm => vm.TrainingDetailsViewModel.TrainingProvided, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, true, "http://www.google.com/", true)]
        [TestCase(VacancySource.Raa, true, "", false)]
        [TestCase(VacancySource.Raa, false, "http://www.google.com/", false)]
        [TestCase(VacancySource.Raa, false, "", true)]
        [TestCase(VacancySource.Av, true, "http://www.google.com/", true)]
        [TestCase(VacancySource.Av, true, "", false)]
        [TestCase(VacancySource.Av, false, "http://www.google.com/", true)]
        [TestCase(VacancySource.Av, false, "", true)]
        [TestCase(VacancySource.Api, true, "http://www.google.com/", true)]
        [TestCase(VacancySource.Api, true, "", false)]
        [TestCase(VacancySource.Api, false, "http://www.google.com/", true)]
        [TestCase(VacancySource.Api, false, "", true)]
        public void OfflineApplicationUrlRequired(VacancySource vacancySource, bool offlineVacancy, string url, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.NewVacancyViewModel = new NewVacancyViewModel
            {
                OfflineVacancy = offlineVacancy,
                OfflineApplicationUrl = url,
                VacancySource = vacancySource
            };

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationUrl, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, true, "some instructions", true)]
        [TestCase(VacancySource.Raa, true, "", true)]
        [TestCase(VacancySource.Raa, false, "some instructions", false)]
        [TestCase(VacancySource.Raa, false, "", true)]
        [TestCase(VacancySource.Av, true, "some instructions", true)]
        [TestCase(VacancySource.Av, true, "", true)]
        [TestCase(VacancySource.Av, false, "some instructions", true)]
        [TestCase(VacancySource.Av, false, "", true)]
        [TestCase(VacancySource.Api, true, "some instructions", true)]
        [TestCase(VacancySource.Api, true, "", true)]
        [TestCase(VacancySource.Api, false, "some instructions", true)]
        [TestCase(VacancySource.Api, false, "", true)]
        public void OfflineApplicationInstructionsRequired(VacancySource vacancySource, bool offlineVacancy, string instructions, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.NewVacancyViewModel = new NewVacancyViewModel
            {
                OfflineVacancy = offlineVacancy,
                OfflineApplicationInstructions = instructions,
                VacancySource = vacancySource
            };

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.NewVacancyViewModel, vm => vm.NewVacancyViewModel.OfflineApplicationInstructions, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, "40", 120.0, true, true)]
        [TestCase(VacancySource.Raa, "40", null, true, false)]
        [TestCase(VacancySource.Raa, null, null, false, false)]
        [TestCase(VacancySource.Raa, null, 120.0, false, true)]
        [TestCase(VacancySource.Av, "40", 120.0, true, true)]
        [TestCase(VacancySource.Av, "40", null, true, true)]
        [TestCase(VacancySource.Av, null, null, true, true)]
        [TestCase(VacancySource.Av, null, 120.0, false, true)]
        [TestCase(VacancySource.Api, "40", 120.0, true, true)]
        [TestCase(VacancySource.Api, "40", null, true, true)]
        [TestCase(VacancySource.Api, null, null, true, true)]
        [TestCase(VacancySource.Api, null, 120.0, false, true)]
        public void DurationAndHoursPerWeekValidation(VacancySource vacancySource, string hoursPerWeekString, double? duration, bool expectHoursPerWeekValid, bool expectDurationValid)
        {
            decimal? hoursPerWeek = null;
            decimal parsedHoursPerWeek;
            if (decimal.TryParse(hoursPerWeekString, out parsedHoursPerWeek))
            {
                hoursPerWeek = parsedHoursPerWeek;
            }

            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
            {
                HoursPerWeek = hoursPerWeek,
                VacancySource = vacancySource,
                Duration = (decimal?) duration
            };

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectHoursPerWeekValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }

            if (expectDurationValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, "40", 12.0, true, false)]
        [TestCase(VacancySource.Av, "40", 12.0, true, false)]
        [TestCase(VacancySource.Api, "40", 12.0, true, false)]
        public void DurationWarningsValidation(VacancySource vacancySource, string hoursPerWeekString, double? duration, bool expectHoursPerWeekValid, bool expectDurationValid)
        {
            decimal? hoursPerWeek = null;
            decimal parsedHoursPerWeek;
            if (decimal.TryParse(hoursPerWeekString, out parsedHoursPerWeek))
            {
                hoursPerWeek = parsedHoursPerWeek;
            }

            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.FurtherVacancyDetailsViewModel = new FurtherVacancyDetailsViewModel
            {
                HoursPerWeek = hoursPerWeek,
                VacancySource = vacancySource,
                Duration = (decimal?)duration
            };

            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectHoursPerWeekValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.HoursPerWeek, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }

            if (expectDurationValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.Duration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        [TestCase(VacancySource.Raa, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Raa, Samples.ValidFreeHtmlText, false)]
        [TestCase(VacancySource.Raa, Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(VacancySource.Raa, Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(VacancySource.Raa, Samples.InvalidHtmlTextWithScript, false)]
        [TestCase(VacancySource.Av, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Av, Samples.ValidFreeHtmlText, false)]
        [TestCase(VacancySource.Av, Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(VacancySource.Av, Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(VacancySource.Av, Samples.InvalidHtmlTextWithScript, false)]
        [TestCase(VacancySource.Api, Samples.ValidFreeText, true)]
        [TestCase(VacancySource.Api, Samples.ValidFreeHtmlText, false)]
        [TestCase(VacancySource.Api, Samples.InvalidHtmlTextWithInput, false)]
        [TestCase(VacancySource.Api, Samples.InvalidHtmlTextWithObject, false)]
        [TestCase(VacancySource.Api, Samples.InvalidHtmlTextWithScript, false)]
        public void ExpectedDurationValidation(VacancySource vacancySource, string expectedDuration, bool expectValid)
        {
            var vacancyViewModel = BuildValidVacancy(vacancySource);
            vacancyViewModel.FurtherVacancyDetailsViewModel.ExpectedDuration = expectedDuration;

            _aggregateValidator.Validate(vacancyViewModel);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Errors);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.Warnings);
            _aggregateValidator.Validate(vacancyViewModel, ruleSet: RuleSets.ErrorsAndWarnings);

            if (expectValid)
            {
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
            else
            {
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel, RuleSets.Errors);
                _aggregateValidator.ShouldNotHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel, RuleSets.Warnings);
                _aggregateValidator.ShouldHaveValidationErrorFor(vm => vm.FurtherVacancyDetailsViewModel, vm => vm.FurtherVacancyDetailsViewModel.ExpectedDuration, vacancyViewModel, RuleSets.ErrorsAndWarnings);
            }
        }

        private VacancyViewModel BuildValidVacancy(VacancySource vacancySource)
        {
            var viewModel = new Fixture().Build<VacancyViewModel>().Create();
            viewModel.NewVacancyViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.NewVacancyViewModel.OfflineVacancy = false;
            viewModel.NewVacancyViewModel.OfflineApplicationUrl = null;
            viewModel.NewVacancyViewModel.OfflineApplicationInstructions = null;
            viewModel.TrainingDetailsViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.TrainingDetailsViewModel.TrainingType = TrainingType.Frameworks;
            viewModel.TrainingDetailsViewModel.ApprenticeshipLevel = ApprenticeshipLevel.Higher;
            viewModel.TrainingDetailsViewModel.ContactName = null;
            viewModel.TrainingDetailsViewModel.ContactNumber = null;
            viewModel.TrainingDetailsViewModel.ContactEmail = null;
            viewModel.FurtherVacancyDetailsViewModel.Status = VacancyStatus.Live;
            viewModel.FurtherVacancyDetailsViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.FurtherVacancyDetailsViewModel.HoursPerWeek = 30;
            viewModel.FurtherVacancyDetailsViewModel.Duration = 12;
            viewModel.FurtherVacancyDetailsViewModel.DurationType = DurationType.Months;
            viewModel.FurtherVacancyDetailsViewModel.WageType = WageType.NationalMinimum;
            viewModel.FurtherVacancyDetailsViewModel.VacancyDatesViewModel = new VacancyDatesViewModel
            {
                PossibleStartDate = new DateViewModel(DateTime.UtcNow.AddDays(28)),
                ClosingDate = new DateViewModel(DateTime.UtcNow.AddDays(14))
            };
            viewModel.VacancyRequirementsProspectsViewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.Status = VacancyStatus.Live;
            viewModel.VacancyType = VacancyType.Apprenticeship;
            viewModel.VacancySource = vacancySource;
            viewModel.VacancyRequirementsProspectsViewModel.VacancySource = vacancySource;
            viewModel.FurtherVacancyDetailsViewModel.VacancySource = vacancySource;

            return viewModel;
        }
    }
}