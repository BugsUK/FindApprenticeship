namespace SFA.Apprenticeships.Web.Raa.Common.UnitTests.Converters
{
    using System.Reflection;

    using Common.Converters;
    using Domain.Entities.Raa.Vacancies;
    using FluentAssertions;
    using NUnit.Framework;
    using Ploeh.AutoFixture;

    using SFA.Apprenticeships.Web.Raa.Common.ViewModels.Vacancy;

    [TestFixture]
    public class ApprenticeshipVacancyConverterTests
    {
        [Test]
        public void ApprenticeshipDurationTypes()
        {
            //Arrange
            var vacancy = new Fixture().Build<Vacancy>().With(v => v.VacancyType, VacancyType.Apprenticeship).Create();

            //Act
            var viewModel = vacancy.ConvertToVacancySummaryViewModel();

            //Assert
            viewModel.DurationTypes.Count.Should().Be(3);
            viewModel.DurationTypes[0].Value.Should().Be(DurationType.Weeks.ToString());
            viewModel.DurationTypes[0].Text.Should().Be(DurationType.Weeks.ToString().ToLower());
            viewModel.DurationTypes[1].Value.Should().Be(DurationType.Months.ToString());
            viewModel.DurationTypes[1].Text.Should().Be(DurationType.Months.ToString().ToLower());
            viewModel.DurationTypes[2].Value.Should().Be(DurationType.Years.ToString());
            viewModel.DurationTypes[2].Text.Should().Be(DurationType.Years.ToString().ToLower());
        }

        [Test]
        public void TraineeshipDurationTypes()
        {
            //Arrange
            var vacancy = new Fixture().Build<Vacancy>().With(v => v.VacancyType, VacancyType.Traineeship).Create();

            //Act
            var viewModel = vacancy.ConvertToVacancySummaryViewModel();

            //Assert
            viewModel.DurationTypes.Count.Should().Be(2);
            viewModel.DurationTypes[0].Value.Should().Be(DurationType.Weeks.ToString());
            viewModel.DurationTypes[0].Text.Should().Be(DurationType.Weeks.ToString().ToLower());
            viewModel.DurationTypes[1].Value.Should().Be(DurationType.Months.ToString());
            viewModel.DurationTypes[1].Text.Should().Be(DurationType.Months.ToString().ToLower());
        }

        [Test]
        public void ConvertToVacancySummaryViewModel_RemoveHtml()
        {
            //Arrange
            var vacancyLongDescription = "Hoshizaki Europe is growing quickly and we need to implement a structure that supports the ever growing needs of the business. We plan to develop the apprentice and place them in one of the key positions that arise from the growth. <br />"
              + "< br />"
              + "The apprentice will move from department to department, offering administration support and carrying out general office duties.The knowledge of the company and the departments you will work in will allow us to decide where your skills are best. < br />"
              + "< br />"
              + "< strong > JOB PURPOSE:</ strong > < br />"
              + "  To learn administration disciplines throughout the business to a good level of competence. To work through the pre prescribed modules issued by the company in line with business needs. The modular work could involve the completion of projects to enhance the apprentice & rsquo; s knowledge and bring added value to the department involved. < br />"
              + "  < br />"
              + "< strong > TASKS DUTIES AND RESPONSIBILITIES:</ strong > < br />"
              + " Working within the department of modular assignment, duties will include: < br />"
              + "< br />"
              + "& bull; Carrying out special project work < br />"
              + "& bull; Routine tasks specific to hosting department < br />"
              + "& bull; Answering telephone < br />"
              + "& bull; Answering door, greeting visitors < br />"
              + "& bull; Offering support to any department requiring additional resource < br />"
              + "& bull; To join the relevant B.I.T.team and carry out improvement exercises < br />"
              + "& bull; Administration tasks supporting all areas of the business < br />"
              + "& bull; To identify improvements in allocated work. < br />"
              + "< br />"
              + "< strong > COMMUNICATIONS:</ strong > < br />"
              + "& bull; Daily, with all levels of staff. < br />"
              + "& bull; To take part in team briefings < br />"
              + "& bull; Speak with TCAT regarding M.A.and training < br />"
              + "& bull; Answer phone professionally and route calls to correct departments < br />"
              + "& bull; Externally with customers / suppliers and external bodies by fax, e mail or telephone. < br />"
              + "< br />"
              + " < strong > TRAINING AND DEVLOPMENT</ strong >: < br />"
              + "   Will be offered on the job training which follow departmental disciplines and create marketable skills for the apprentice. < br />"
              + "The job holder has the opportunity to develop his / her self within the organisation and every support will be given to this end. < br /> ";

            var vacancy = new Fixture().Build<Vacancy>().With
                (v => v.VacancyType, VacancyType.Traineeship)
                .With(v=>v.LongDescription, vacancyLongDescription).Create();            

            //Act
            var viewModel = vacancy.ConvertToVacancySummaryViewModel();            

            //Assert
            viewModel.LongDescription.Should().NotContain("<");
            viewModel.LongDescription.Should().NotContain(">");
            viewModel.LongDescription.Should().NotContain("& bull;");
            viewModel.LongDescription.Should().NotContain("< br />");
        }        
    }
}