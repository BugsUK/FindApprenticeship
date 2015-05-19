﻿namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Mediators.ApprenticeshipApplication
{
    using System.Collections.Generic;
    using Candidate.Mediators.Application;
    using Candidate.ViewModels.Applications;
    using Candidate.ViewModels.Candidate;
    using Candidate.ViewModels.VacancySearch;
    using FluentAssertions;
    using NUnit.Framework;

    [TestFixture]
    public class AddEmptyWorkExperienceRowsTests : TestsBase
    {
        private const string BlankSpace = "  ";
        private const string SomeDescription = "Some description";
        private const string SomeEmployer = "Some employer";
        private const int SomeMonth = 1;
        private const string SomeYear = "2012";

        private static string _someJobTitle;

        [Test]
        public void Ok()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = new ApprenticeshipCandidateViewModel(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyWorkExperienceRows(viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.AddEmptyWorkExperienceRows.Ok, true);
        }

        [Test]
        public void WillRemoveEmptyWorkExperienceRows()
        {
            var viewModel = new ApprenticeshipApplicationViewModel
            {
                Candidate = CreateCandidateWithOneCompletedAndTwoEmptyWorkExperienceRows(),
                VacancyDetail = new ApprenticeshipVacancyDetailViewModel()
            };

            var response = Mediator.AddEmptyWorkExperienceRows(viewModel);

            response.AssertCode(ApprenticeshipApplicationMediatorCodes.AddEmptyWorkExperienceRows.Ok, true);
            response.ViewModel.Candidate.WorkExperience.Should().HaveCount(1);
        }

        private static ApprenticeshipCandidateViewModel CreateCandidateWithOneCompletedAndTwoEmptyWorkExperienceRows()
        {
            _someJobTitle = "Job title";
            return new ApprenticeshipCandidateViewModel
            {
                WorkExperience = new List<WorkExperienceViewModel>
                {
                    new WorkExperienceViewModel(),
                    new WorkExperienceViewModel
                    {
                        Description = SomeDescription,
                        Employer = SomeEmployer,
                        FromMonth = SomeMonth,
                        FromYear = SomeYear,
                        JobTitle = _someJobTitle,
                        ToMonth = SomeMonth,
                        ToYear = SomeYear
                    },
                    new WorkExperienceViewModel
                    {
                        Description = BlankSpace,
                        Employer = BlankSpace,
                        FromMonth = SomeMonth,
                        FromYear = BlankSpace,
                        JobTitle = BlankSpace,
                        ToMonth = SomeMonth,
                        ToYear = BlankSpace
                    }
                }
            };
        }
    }
}