using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace DocumentApi.Application.Translators.Commands.CreateTranslator.Tests
{
    public class CreateTranslatorCommandValidatorTests
    {
        [Theory]
        [InlineData("Name", true)]
        [InlineData("", false, "empty")]
        [InlineData(null, false, "empty")]
        public void CreateTranslatorCommandValidator_NameValidationTests(string? name, bool expectedResult, string errorName = "")
        {
            var validator = new CreateTranslatorCommandValidator();
            var command = new CreateTranslatorCommand()
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