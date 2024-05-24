using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace DocumentApi.Application.Clients.Commands.UpdateClient.Tests
{
    public class UpdateClientCommandValidatorTests
    {
        [Theory]
        [InlineData("Name", true)]
        [InlineData("", false, "empty")]
        [InlineData(null, false, "empty")]
        public void UpdateClientCommandValidator_NameValidationTests(string? name, bool expectedResult, string errorName = "")
        {
            var validator = new UpdateClientCommandValidator();
            var command = new UpdateClientCommand()
            {
                Name = name,
                Email = "test@test.pl",
                TelephoneNumber = "123456789"
            };

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("Name");
                result.Errors.Should().HaveCount(1);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData("test@test.pl", true)]
        [InlineData("test", false, "valid")]
        [InlineData("", false, "empty", 2)]
        [InlineData(null, false, "empty")]
        public void UpdateClientCommandValidator_EmailValidationTests(string? email, bool expectedResult, string errorName = "", int errorCount = 1)
        {
            var validator = new UpdateClientCommandValidator();
            var command = new UpdateClientCommand()
            {
                Name = "Name",
                Email = email,
                TelephoneNumber = "123456789"
            };

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("Email");
                result.Errors.Should().HaveCount(errorCount);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData("123456789", true)]
        [InlineData("12345678", false, "8")]
        [InlineData("1234567890123", false, "13")]
        [InlineData("", false, "empty", 2)]
        [InlineData(null, false, "empty")]
        public void UpdateClientCommandValidator_TelephoneValidationTests(string? telephoneNumber, bool expectedResult, string errorName = "", int errorCount = 1)
        {
            var validator = new UpdateClientCommandValidator();
            var command = new UpdateClientCommand()
            {
                Name = "Name",
                Email = "test@test.pl",
                TelephoneNumber = telephoneNumber
            };

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("TelephoneNumber");
                result.Errors.Should().HaveCount(errorCount);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }
    }
}