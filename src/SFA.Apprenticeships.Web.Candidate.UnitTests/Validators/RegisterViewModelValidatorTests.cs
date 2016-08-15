namespace SFA.Apprenticeships.Web.Candidate.UnitTests.Validators
{
    using Candidate.ViewModels.Register;
    using FluentValidation.TestHelper;
    using NUnit.Framework;
    using Candidate.Validators;

    [TestFixture]
    [Parallelizable]
    public class RegisterViewModelValidatorTests
    {
        [TestCase("Password1")]
        [TestCase("Password1$%")]
        public void ShouldNotHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = password
            };
            var viewModelClientValidator = new RegisterViewModelClientValidator();

            viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("abc")]
        [TestCase("123")]
        [TestCase("%^$£&^$%123aadff01sdaf*&^")]
        [TestCase("password1")]
        public void ShouldHaveErrorsWhenPasswordComplexitySatisfied(string password)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = password
            };
            var viewModelClientValidator = new RegisterViewModelClientValidator();

            viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("krister.bone_@gmail.com", "?Password01!")]
        [TestCase("krister_bone@gmail.com", "?Password01!")]
        [TestCase("krister+bone@gmail.com", "?Password01!")]
        [TestCase("krister*bone@gmail.number", "?Password01!")]
        [TestCase("kristerbone@gmail.subdomain.number", "?Password01!")]
        [TestCase("kristerbone@gmail.subsubdomain.subdomain.number", "?Password01!")]
        public void ShouldNotHaveErrorWhenEmailAddressIsValid(string emailAddress, string password)
        {
            var viewModel = new RegisterViewModel
            {
                EmailAddress = emailAddress,
                Password = password,
                ConfirmPassword = password
            };
            var viewModelClientValidator = new RegisterViewModelClientValidator();

            viewModelClientValidator.ShouldNotHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }

        [TestCase(".krister.bonegmail.com", "?Password01!")]
        [TestCase("krister.bone@gmailcom", "?Password01!")]
        public void ShouldHaveErrorWhenEmailAddressIsInvalid(string emailAddress, string password)
        {
            var viewModel = new RegisterViewModel
            {
                EmailAddress = emailAddress ,
                Password = password,
                ConfirmPassword = password
            };
            var viewModelClientValidator = new RegisterViewModelClientValidator();

            viewModelClientValidator.ShouldHaveValidationErrorFor(x => x.EmailAddress, viewModel);
        }

        [TestCase("?Password01!", "?Password02!")]
        public void ShouldHaveErrorWhenPasswordsDoNotMatch(string password, string confirmPassword)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = confirmPassword
            };
            var viewModelServerValidator = new RegisterViewModelServerValidator();

            viewModelServerValidator.ShouldHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase("?Password01!")]
        public void ShouldNotHaveErrorWhenPasswordsMatch(string password)
        {
            var viewModel = new RegisterViewModel
            {
                Password = password,
                ConfirmPassword = password
            };
            var viewModelServerValidator = new RegisterViewModelServerValidator();

            viewModelServerValidator.ShouldNotHaveValidationErrorFor(x => x.Password, viewModel);
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(35, true)]
        [TestCase(36, false)]
        public void ShouldRequireFirstName(int? length, bool expectValid)
        {
            // Arrange.
            var viewModel = new RegisterViewModel
            {
                Firstname= length.HasValue ? new string('X', length.Value) : null
            };

            // Act.
            var validator = new RegisterViewModelServerValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.Firstname, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.Firstname, viewModel);
            }
        }

        [TestCase("John", true)]
        [TestCase("Jo<hn", false)]
        public void ShouldWhitelistFirstName(string firstName, bool expectValid)
        {
            // Arrange.
            var viewModel = new RegisterViewModel
            {
                Firstname = firstName
            };

            // Act.
            var validator = new RegisterViewModelServerValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.Firstname, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.Firstname, viewModel);
            }
        }

        [TestCase(null, false)]
        [TestCase(0, false)]
        [TestCase(1, true)]
        [TestCase(33, true)]
        [TestCase(34, false)]
        public void ShouldRequireLastName(int? length, bool expectValid)
        {
            // Arrange.
            var viewModel = new RegisterViewModel
            {
                Lastname = length.HasValue ? new string('X', length.Value) : null
            };

            // Act.
            var validator = new RegisterViewModelServerValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.Lastname, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.Lastname, viewModel);
            }
        }

        [TestCase("Smith", true)]
        [TestCase("Smi<th", false)]
        public void ShouldWhitelistLastName(string lastName, bool expectValid)
        {
            // Arrange.
            var viewModel = new RegisterViewModel
            {
                Lastname = lastName
            };

            // Act.
            var validator = new RegisterViewModelServerValidator();

            // Assert.
            if (expectValid)
            {
                validator.ShouldNotHaveValidationErrorFor(vm => vm.Lastname, viewModel);
            }
            else
            {
                validator.ShouldHaveValidationErrorFor(vm => vm.Lastname, viewModel);
            }
        }
    }
}
