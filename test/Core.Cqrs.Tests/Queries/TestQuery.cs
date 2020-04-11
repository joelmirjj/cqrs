using Core.Cqrs.Queries;

namespace Core.Cqrs.Tests.Queries
{
    public class TestQuery : IQueryAsync<string>
    {
        public TestQuery(string result)
        {
            Result = result;
        }

        public string Result { get;}
    }
}