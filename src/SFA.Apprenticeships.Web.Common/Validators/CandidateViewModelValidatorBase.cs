namespace SFA.Apprenticeships.Web.Common.Validators
{
    using FluentValidation;
    using ViewModels.Candidate;

    public abstract class CandidateViewModelClientValidatorBase<T> : AbstractValidator<T> where T : CandidateViewModelBase
    {
        protected CandidateViewModelClientValidatorBase()
        {
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelClientValidator());
        }
    }

    public abstract class CandidateViewModelServerValidatorBase<T> : AbstractValidator<T> where T : CandidateViewModelBase
    {
        protected CandidateViewModelServerValidatorBase()
        {
            RuleFor(x => x.EmployerQuestionAnswers).SetValidator(new EmployerQuestionAnswersViewModelServerValidator());
            RuleFor(x => x.Qualifications)
               .SetCollectionValidator(new QualificationViewModelValidator())
               .When(x => x.HasQualifications);
            RuleFor(x => x.WorkExperience)
                .SetCollectionValidator(new WorkExperienceViewModelValidator())
                .When(x => x.HasWorkExperience);
            RuleFor(x => x.TrainingCourses)
                .SetCollectionValidator(new TrainingCourseViewModelValidator())
                .When(x => x.HasTrainingCourses);
        }
    }
}