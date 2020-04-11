using System.Threading.Tasks;
using Core.Cqrs.Queries;

namespace Core.Cqrs.Tests.Queries
{
    public class TestQueryHandler : IQueryHandlerAsync<TestQuery, string>
    {
        public Task<string> HandleAsync(TestQuery query)
        {
            return Task.FromResult(query.Result);
        }
    }
}