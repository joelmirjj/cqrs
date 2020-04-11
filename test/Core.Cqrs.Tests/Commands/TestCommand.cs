using Core.Cqrs.Commands;

namespace Core.Cqrs.Tests
{
    public class TestCommand : ICommandAsync<int>
    {
        public TestCommand(int result)
        {
            Result = result;
        }

        public int Result { get; }
    }
}