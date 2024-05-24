﻿using Xunit;
using FluentValidation.TestHelper;
using FluentAssertions;
using DocumentApi.Application.Common.Interfaces;
using Microsoft.EntityFrameworkCore;
using DocumentApi.Domain.Entities;
using DocumentApi.Infrastructure.Data;

namespace DocumentApi.Application.Documents.Commands.CreateDocument.Tests
{
    public class CreateDocumentCommandValidatorTests
    {
        private readonly static IDocumentDbContext contextMock = MockDocumentDbContext;

        [Theory]
        [InlineData("Name", true)]
        [InlineData("", false, "empty")]
        [InlineData(null, false, "empty")]
        public void CreateDocumentCommandValidator_NameValidationTests(string? name, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock);
            var command = new CreateDocumentCommand()
            {
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
        public void CreateDocumentCommandValidator_SignsSizeValidationTests(int signSize, bool expectedResult, string errorName = "", int errorsCount = 1)
        {
            var validator = new CreateDocumentCommandValidator(contextMock);
            var command = new CreateDocumentCommand()
            {
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
        public void CreateDocumentCommandValidator_CreatedAtValidationTests(bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock);
            var command = new CreateDocumentCommand()
            {
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
        public void CreateDocumentCommandValidator_DeadlineValidationTests(bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock);
            var command = new CreateDocumentCommand()
            {
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
        public void CreateDocumentCommandValidator_DeadlineMustAppearAfterCreationValidationTests(int offset, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock);
            var command = new CreateDocumentCommand()
            {
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
        public void CreateDocumentCommandValidator_ClientValidationTests(int clientId, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock);
            var command = new CreateDocumentCommand()
            {
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
        public void CreateDocumentCommandValidator_TranslatorValidationTests(int? translatorId, bool expectedResult, string errorName = "")
        {
            var validator = new CreateDocumentCommandValidator(contextMock);
            var command = new CreateDocumentCommand()
            {
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