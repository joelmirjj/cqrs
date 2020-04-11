using System.Threading.Tasks;
using FluentResults;

namespace Core.Cqrs.Commands
{
    public interface ICommandHandlerAsync<in TCommand, TResult>
        where TCommand : ICommandAsync<TResult> where TResult : struct
    {
        Task<Result<TResult>> HandleAsync(TCommand command);
    }
}