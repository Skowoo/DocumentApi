using DocumentApi.Application.Translators.Commands.CreateTranslator;
using FluentValidation;
using Moq;
using Xunit;

namespace DocumentApi.Application.Common.Behaviours.Tests
{
    public class ValidationBehaviorTests
    {
        readonly CreateTranslatorCommandValidator CommandValidator = new();
        readonly Mock<MediatR.RequestHandlerDelegate<CreateTranslatorCommand>> nextDelegateMock = new();

        [Fact]
        public async Task ShouldThrowValidationError()
        {
            var testedBehaviour = new ValidationBehavior<CreateTranslatorCommand, CreateTranslatorCommand>([CommandValidator]);
            var command = new CreateTranslatorCommand() { Name = "" };
            await Xunit.Assert.ThrowsAsync<ValidationException>(async () => await testedBehaviour.Handle(command, nextDelegateMock.Object, CancellationToken.None));
        }

        [Fact]
        public async Task ShouldNotThrowValidationError()
        {
            var testedBehaviour = new ValidationBehavior<CreateTranslatorCommand, CreateTranslatorCommand>([CommandValidator]);
            var command = new CreateTranslatorCommand() { Name = "ValidName" };
            await testedBehaviour.Handle(command, nextDelegateMock.Object, CancellationToken.None);
        }
    }
}
