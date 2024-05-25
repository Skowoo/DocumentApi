using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;
using DocumentApi.Application.Common.Interfaces;
using DocumentApi.ApplicationTests;

namespace DocumentApi.Application.Translators.Commands.UpdateTranslator.Tests
{
    public class UpdateTranslatorCommandValidatorTests
    {
        private readonly IDocumentDbContext contextMock = DocumentDbContextDataFixture.GetContext();

        [Theory]
        [InlineData("Name", true)]
        [InlineData("", false, "empty")]
        [InlineData(null, false, "empty")]
        public void UpdateTranslatorCommandValidator_NameValidationTests(string? name, bool expectedResult, string errorName = "")
        {
            var validator = new UpdateTranslatorCommandValidator(contextMock);
            var command = new UpdateTranslatorCommand()
            {
                Id = 1,
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

        [Theory]
        [InlineData(null, false, "empty", 2)]
        [InlineData(0, false, "empty", 2)]
        [InlineData(2, false, "Translator with given Id does not exists")]
        [InlineData(1, true)]
        public void UpdateTranslatorCommandValidator_IdValidationTests(int? id, bool expectedResult, string errorName = "", int errorsCount = 1)
        {
            var validator = new UpdateTranslatorCommandValidator(contextMock);
            var command = new UpdateTranslatorCommand()
            {
                Name = "Name"
            };
            if (id is not null)
                command.Id = (int)id;


            var result = validator.TestValidate(command);


            result.IsValid.Should().Be(expectedResult);
            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("Id");
                result.Errors.Should().HaveCount(errorsCount);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }
    }
}