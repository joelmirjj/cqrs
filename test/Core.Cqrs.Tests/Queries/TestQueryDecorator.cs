using System.Threading.Tasks;
using Core.Cqrs.Queries;

namespace Core.Cqrs.Tests.Queries
{
    public class TestQueryDecorator : IQueryHandlerAsync<TestQuery, string>
    {
        private readonly IQueryHandlerAsync<TestQuery, string> _handler;

        public TestQueryDecorator(IQueryHandlerAsync<TestQuery, string> handler)
        {
            _handler = handler;
        }

        public Task<string> HandleAsync(TestQuery query)
        {
            return _handler.HandleAsync(new TestQuery(query.Result + " - Decorated"));
        }
    }
}