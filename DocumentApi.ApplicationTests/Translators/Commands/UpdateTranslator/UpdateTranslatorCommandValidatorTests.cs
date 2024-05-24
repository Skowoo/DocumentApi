using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;

namespace DocumentApi.Application.Translators.Commands.UpdateTranslator.Tests
{
    public class UpdateTranslatorCommandValidatorTests
    {
        [Theory]
        [InlineData("Name", true)]
        [InlineData("", false, "empty")]
        [InlineData(null, false, "empty")]
        public void UpdateTranslatorCommandValidator_NameValidationTests(string? name, bool expectedResult, string errorName = "")
        {
            var validator = new UpdateTranslatorCommandValidator();
            var command = new UpdateTranslatorCommand()
            {
                Name = name
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
    }
}