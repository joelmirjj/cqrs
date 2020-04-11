using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;

namespace Core.Cqrs.Tests.Queries
{
    public class query_registration_tests
    {
        private ServiceProvider _provider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddCqrsHandlers(GetType().Assembly);
            services.AddTransient<MessageDispatcher>();
            _provider = services.BuildServiceProvider();
        }

        [Test]
        public async Task the_registration_of_query_works()
        {
            var messageDispatcher = _provider.GetRequiredService<MessageDispatcher>();
            var result = await messageDispatcher.DispatchAsync(new TestQuery("Query"));
            result.Should().Be("Query");
        }
    }
}