using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;
using DocumentApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using DocumentApi.Domain.Entities;
using DocumentApi.Infrastructure.Data;

namespace DocumentApi.Application.Documents.Commands.UpdateDocument.Tests
{
    public class UpdateDocumentCommandValidatorTests
    {
        private readonly static IDocumentDbContext contextMock = MockDocumentDbContext;

        [Theory]
        [InlineData("01cbe5e5-ae75-4972-b741-14bf69f33f48", true)]
        [InlineData("02cbe5e5-ae75-4972-b741-14bf69f33f48", false, "not")]
        [InlineData(null, false, "not", 2)]
        public void UpdateDocumentCommandValidator_IdValidationTests(string? guidBody, bool expectedResult, string errorName = "", int errorCount = 1)
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {                
                Title = "Name",
                SignsSize = 100,
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow.AddDays(1),
                ClientId = 1,
                TranslatorId = null
            };
            if (guidBody is not null)
                command.Id = Guid.Parse(guidBody);


            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("Id");
                result.Errors.Should().HaveCount(errorCount);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData("Name", true)]
        [InlineData("", false, "empty")]
        [InlineData(null, false, "empty")]
        public void UpdateDocumentCommandValidator_NameValidationTests(string? name, bool expectedResult, string errorName = "")
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {
                Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48"),
                Title = name,
                SignsSize = 100,
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow.AddDays(1),
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
        public void UpdateDocumentCommandValidator_SignsSizeValidationTests(int signSize, bool expectedResult, string errorName = "", int errorsCount = 1)
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {
                Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48"),
                Title = "Name",
                SignsSize = signSize,
                CreatedAt = DateTime.UtcNow,
                Deadline = DateTime.UtcNow.AddDays(1),
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
        public void UpdateDocumentCommandValidator_CreatedAtValidationTests(bool expectedResult, string errorName = "")
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {
                Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48"),
                Title = "Name",
                SignsSize = 100,
                Deadline = DateTime.UtcNow.AddDays(1),
                ClientId = 1,
                TranslatorId = null
            };
            if (expectedResult)
                command.CreatedAt = DateTime.Now;

            var result = validator.TestValidate(command);

            result.IsValid.Should().Be(expectedResult);

            if (!expectedResult)
            {
                result.ShouldHaveValidationErrorFor("CreatedAt");
                result.Errors.Should().HaveCount(1);
                result.Errors[0].ErrorMessage.Should().Contain(errorName);
            }
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false, "empty")]
        public void UpdateDocumentCommandValidator_DeadlineValidationTests(bool expectedResult, string errorName = "")
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {
                Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48"),
                Title = "Name",
                SignsSize = 100,
                CreatedAt = DateTime.Now,
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
        public void UpdateDocumentCommandValidator_DeadlineMustAppearAfterCreationValidationTests(int offset, bool expectedResult, string errorName = "")
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {
                Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48"),
                Title = "Name",
                SignsSize = 100,
                CreatedAt = DateTime.Now,
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
        public void UpdateDocumentCommandValidator_ClientValidationTests(int clientId, bool expectedResult, string errorName = "")
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {
                Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48"),
                Title = "Name",
                SignsSize = 100,
                CreatedAt = DateTime.Now,
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
        public void UpdateDocumentCommandValidator_TranslatorValidationTests(int? translatorId, bool expectedResult, string errorName = "")
        {
            var validator = new UpdateDocumentCommandValidator(contextMock);
            var command = new UpdateDocumentCommand()
            {
                Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48"),
                Title = "Name",
                SignsSize = 100,
                CreatedAt = DateTime.Now,
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

        private static IDocumentDbContext MockDocumentDbContext
        {
            get
            {
                var options = new DbContextOptionsBuilder<DocumentDbContext>()
                .UseInMemoryDatabase(databaseName: "TestDb").Options;

                var context = new DocumentDbContext(options);

                context.Clients.Add(new Client { Id = 1, Name = "Klient1" });
                context.Translators.Add(new Translator { Id = 1, Name = "Tłumacz1" });
                context.Documents.Add(new Document { Id = Guid.Parse("01cbe5e5-ae75-4972-b741-14bf69f33f48") });
                context.SaveChanges();

                return context;
            }
        }
    }
}