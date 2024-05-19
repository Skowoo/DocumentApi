using MediatR;

namespace DocumentApi.Application.Common.Interfaces
{
    /// <summary>
    /// Base marker interface to separate MediatR Commands
    /// </summary>
    public interface ICommandBase;

    /// <summary>
    /// Marker interface to separate MediatR Commands with anything to return
    /// </summary>
    public interface ICommand<TResponse> : IRequest<TResponse>, ICommandBase;

    /// <summary>
    /// Marker interface to separate MediatR Commands
    /// </summary>
    public interface ICommand : IRequest, ICommandBase;
}
