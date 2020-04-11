using System.Threading.Tasks;
using Core.Cqrs.Decorators;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;

namespace Core.Cqrs.Tests.Queries
{
    public class query_decorator_registration_tests
    {
        private ServiceProvider _provider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddCqrsHandlers(GetType().Assembly);
            services.AddCqrsGlobalQueryDecorator(typeof(AuditQueryLoggingDecorator<,>), GetType().Assembly);
            services.AddCqrsQueryDecorator(typeof(TestQueryHandler), typeof(TestQueryDecorator));
            services.AddTransient<MessageDispatcher>();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            services.AddSingleton(Log.Logger);
            _provider = services.BuildServiceProvider();
        }

        [Test]
        public async Task the_registration_of_query_works()
        {
            var messageDispatcher = _provider.GetRequiredService<MessageDispatcher>();
            var result = await messageDispatcher.DispatchAsync(new TestQuery("Query"));
            result.Should().Be("Query - Decorated");
        }
    }
}