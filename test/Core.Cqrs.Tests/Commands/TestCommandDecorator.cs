using System.Threading.Tasks;
using Core.Cqrs.Commands;
using FluentResults;

namespace Core.Cqrs.Tests
{
    public class TestCommandDecorator : ICommandHandlerAsync<TestCommand, int>
    {
        private readonly ICommandHandlerAsync<TestCommand, int> _handler;

        public TestCommandDecorator(ICommandHandlerAsync<TestCommand, int> handler)
        {
            _handler = handler;
        }

        public Task<Result<int>> HandleAsync(TestCommand command)
        {
            return _handler.HandleAsync(new TestCommand(command.Result + 10));
        }
    }
}