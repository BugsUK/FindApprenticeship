namespace SFA.Apprenticeships.Web.Recruit.UnitTests.Validators.VacancyPosting
{
    using System;
    using System.Linq;
    using System.Linq.Expressions;
    using FluentAssertions;
    using FluentValidation;
    using FluentValidation.Internal;

    public static class ValidatorExtensions
    {
        public static void ShouldNotHaveValidationErrorFor<T, TParent, TChild>(this AbstractValidator<T> validator, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression, T vacancyViewModel, string ruleSet = null)
        {
            var propertyName = GetPropertyName(parentPropertyExpression, childPropertyExpression);
            var aggregateResult = string.IsNullOrEmpty(ruleSet) ? validator.Validate(vacancyViewModel) : validator.Validate(vacancyViewModel, ruleSet: ruleSet);
            aggregateResult.Errors.Count(e => e.PropertyName == propertyName).Should().Be(0);
        }

        public static void ShouldHaveValidationErrorFor<T, TParent, TChild>(this AbstractValidator<T> validator, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression, T vacancyViewModel, string ruleSet = null)
        {
            var propertyName = GetPropertyName(parentPropertyExpression, childPropertyExpression);
            var aggregateResult = string.IsNullOrEmpty(ruleSet) ? validator.Validate(vacancyViewModel) : validator.Validate(vacancyViewModel, ruleSet: ruleSet);
            var errors = aggregateResult.Errors.Where(e => e.PropertyName == propertyName).Select(e => e.ErrorMessage).ToList();
            errors.Count.Should().BeGreaterThan(0);
            errors.Count.Should().Be(errors.Distinct().Count(), "All errors should be distinct. Found " + string.Join(", ", errors));
        }

        public static void ShouldNotHaveValidationErrorFor<T, TGrandparent, TParent, TChild>(this AbstractValidator<T> validator, Expression<Func<T, TGrandparent>> grandparentPropertyExpression, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression, T vacancyViewModel, string ruleSet = null)
        {
            var propertyName = GetPropertyName(grandparentPropertyExpression, parentPropertyExpression, childPropertyExpression);
            var aggregateResult = string.IsNullOrEmpty(ruleSet) ? validator.Validate(vacancyViewModel) : validator.Validate(vacancyViewModel, ruleSet: ruleSet);
            aggregateResult.Errors.Count(e => e.PropertyName == propertyName).Should().Be(0);
        }

        public static void ShouldNotHaveValidationErrorFor<T, TGreatGrandparent, TGrandparent, TParent, TChild>(this AbstractValidator<T> validator, Expression<Func<T, TGreatGrandparent>> greatGrandparentPropertyExpression, Expression<Func<T, TGrandparent>> grandparentPropertyExpression, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression, T vacancyViewModel, string ruleSet = null)
        {
            var propertyName = GetPropertyName(greatGrandparentPropertyExpression, grandparentPropertyExpression, parentPropertyExpression, childPropertyExpression);
            var aggregateResult = string.IsNullOrEmpty(ruleSet) ? validator.Validate(vacancyViewModel) : validator.Validate(vacancyViewModel, ruleSet: ruleSet);
            aggregateResult.Errors.Count(e => e.PropertyName == propertyName).Should().Be(0);
        }

        public static void ShouldHaveValidationErrorFor<T, TGrandparent, TParent, TChild>(this AbstractValidator<T> validator, Expression<Func<T, TGrandparent>> grandparentPropertyExpression, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression, T vacancyViewModel, string ruleSet = null)
        {
            var propertyName = GetPropertyName(grandparentPropertyExpression, parentPropertyExpression, childPropertyExpression);
            var aggregateResult = string.IsNullOrEmpty(ruleSet) ? validator.Validate(vacancyViewModel) : validator.Validate(vacancyViewModel, ruleSet: ruleSet);
            var errors = aggregateResult.Errors.Where(e => e.PropertyName == propertyName).Select(e => e.ErrorMessage).ToList();
            errors.Count.Should().BeGreaterThan(0);
            errors.Count.Should().Be(errors.Distinct().Count(), "All errors should be distinct. Found " + string.Join(", ", errors));
        }

        public static void ShouldHaveValidationErrorFor<T, TGreatGrandparent, TGrandparent, TParent, TChild>(this AbstractValidator<T> validator, Expression<Func<T, TGreatGrandparent>> greatGrandparentPropertyExpression, Expression<Func<T, TGrandparent>> grandparentPropertyExpression, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression, T vacancyViewModel, string ruleSet = null)
        {
            var propertyName = GetPropertyName(greatGrandparentPropertyExpression, grandparentPropertyExpression, parentPropertyExpression, childPropertyExpression);
            var aggregateResult = string.IsNullOrEmpty(ruleSet) ? validator.Validate(vacancyViewModel) : validator.Validate(vacancyViewModel, ruleSet: ruleSet);
            var errors = aggregateResult.Errors.Where(e => e.PropertyName == propertyName).Select(e => e.ErrorMessage).ToList();
            errors.Count.Should().BeGreaterThan(0);
            errors.Count.Should().Be(errors.Distinct().Count(), "All errors should be distinct. Found " + string.Join(", ", errors));
        }

        private static string GetPropertyName<T, TParent, TChild>(Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression)
        {
            //Doing it this way due to a probable bug in ShouldHaveValidationErrorFor
            //http://stackoverflow.com/questions/27945523/using-fluentvalidator-to-validate-children-of-properties
            var propertyName = parentPropertyExpression.GetMember().Name + "." + childPropertyExpression.GetMember().Name;
            return propertyName;
        }

        private static string GetPropertyName<T, TGrandparent, TParent, TChild>(Expression<Func<T, TGrandparent>> grandparentPropertyExpression, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression)
        {
            //Doing it this way due to a probable bug in ShouldHaveValidationErrorFor
            //http://stackoverflow.com/questions/27945523/using-fluentvalidator-to-validate-children-of-properties
            var propertyName = grandparentPropertyExpression.GetMember().Name + "." + parentPropertyExpression.GetMember().Name + "." + childPropertyExpression.GetMember().Name;
            return propertyName;
        }

        private static string GetPropertyName<T, TGreatGrandparent, TGrandparent, TParent, TChild>(Expression<Func<T, TGreatGrandparent>> greatGrandparentPropertyExpression, Expression<Func<T, TGrandparent>> grandparentPropertyExpression, Expression<Func<T, TParent>> parentPropertyExpression, Expression<Func<T, TChild>> childPropertyExpression)
        {
            //Doing it this way due to a probable bug in ShouldHaveValidationErrorFor
            //http://stackoverflow.com/questions/27945523/using-fluentvalidator-to-validate-children-of-properties
            var propertyName = greatGrandparentPropertyExpression.GetMember().Name + "." + grandparentPropertyExpression.GetMember().Name + "." + parentPropertyExpression.GetMember().Name + "." + childPropertyExpression.GetMember().Name;
            return propertyName;
        }
    }
}