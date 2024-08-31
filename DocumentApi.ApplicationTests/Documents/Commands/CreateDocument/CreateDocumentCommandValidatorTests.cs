using DocumentApi.Application.Common.Interfaces;
using DocumentApi.ApplicationTests;
using FluentAssertions;
using FluentValidation.TestHelper;
using Xunit;

namespace DocumentApi.Application.Documents.Commands.CreateDocument.Tests
{
    public class CreateDocumentCommandValidatorTests
    {
        private readonly IDocumentDbContext contextMock = DocumentDbContextDataFixture.GetContext();
        private readonly ITimeProvider timeProviderMock = DocumentDbContextDataFixture.GetTimeProvider();

        [Theory]
        [InlineData("Name", true)]
        [InlineData("", false, "empty")]
        [InlineData(null, false, "empty")]
        public void CreateDocumentCommandValidator_NameValidationTests(string? name, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock, timeProviderMock);
            var command = new CreateDocumentCommand()
            {
                Title = name,
                SignsSize = 100,
                Deadline = DateTime.Now.AddDays(1),
                ClientId = 1,
                TranslatorId = null
            };

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("Title");
                result.Errors.Should().HaveCount(1);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData(100, true)]
        [InlineData(0, false, "empty", 2)]
        [InlineData(100001, false, "100001")]
        public void CreateDocumentCommandValidator_SignsSizeValidationTests(int signSize, bool expectedResult, string errorName = "", int errorsCount = 1)
        {
            var validator = new CreateDocumentCommandValidator(contextMock, timeProviderMock);
            var command = new CreateDocumentCommand()
            {
                Title = "Name",
                SignsSize = signSize,
                Deadline = DateTime.Now.AddDays(1),
                ClientId = 1,
                TranslatorId = null
            };

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("SignsSize");
                result.Errors.Should().HaveCount(errorsCount);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false, "empty")]
        public void CreateDocumentCommandValidator_DeadlineValidationTests(bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock, timeProviderMock);
            var command = new CreateDocumentCommand()
            {
                Title = "Name",
                SignsSize = 100,
                ClientId = 1,
                TranslatorId = null
            };
            if (expectedResult)
                command.Deadline = DateTime.Now.AddDays(1);

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("Deadline");
                result.Errors.Should().HaveCount(2);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(0, true)]
        [InlineData(-1, false, "after")]
        public void CreateDocumentCommandValidator_DeadlineMustAppearAfterCreationValidationTests(int offset, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock, timeProviderMock);
            var command = new CreateDocumentCommand()
            {
                Title = "Name",
                SignsSize = 100,
                Deadline = DateTime.Now.AddDays(offset),
                ClientId = 1,
                TranslatorId = null
            };
            if (expectedResult)
                command.Deadline = DateTime.Now.AddDays(1);

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.Errors.Should().HaveCount(1);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData(1, true)]
        [InlineData(2, false, "Client")]
        public void CreateDocumentCommandValidator_ClientValidationTests(int clientId, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock, timeProviderMock);
            var command = new CreateDocumentCommand()
            {
                Title = "Name",
                SignsSize = 100,
                Deadline = DateTime.Now.AddDays(1),
                ClientId = clientId,
                TranslatorId = null
            };

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("ClientId");
                result.Errors.Should().HaveCount(1);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }


        [Theory]
        [InlineData(null, true)]
        [InlineData(1, true)]
        [InlineData(2, false, "Translator")]
        public void CreateDocumentCommandValidator_TranslatorValidationTests(int? translatorId, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock, timeProviderMock);
            var command = new CreateDocumentCommand()
            {
                Title = "Name",
                SignsSize = 100,
                Deadline = DateTime.Now.AddDays(1),
                ClientId = 1,
                TranslatorId = translatorId
            };

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("TranslatorId");
                result.Errors.Should().HaveCount(1);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }
    }
}