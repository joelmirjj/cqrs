using System.Threading.Tasks;
using Core.Cqrs.Commands;
using FluentResults;

namespace Core.Cqrs.Tests
{
    public class TestCommandHandler: ICommandHandlerAsync<TestCommand, int>
    {
        public Task<Result<int>> HandleAsync(TestCommand command)
        {
            return Task.FromResult(Results.Ok(command.Result));
        }
    }
}