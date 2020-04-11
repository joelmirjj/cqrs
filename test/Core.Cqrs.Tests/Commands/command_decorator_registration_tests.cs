using System.Threading.Tasks;
using Core.Cqrs.Decorators;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using Serilog;
using Serilog.Core;

namespace Core.Cqrs.Tests.Commands
{
    public class command_decorator_registration_tests
    {
        private ServiceProvider _provider;

        [SetUp]
        public void Setup()
        {
            var services = new ServiceCollection();
            services.AddCqrsHandlers(GetType().Assembly);
            services.AddCqrsGlobalCommandDecorator(typeof(AuditCommandLoggingDecorator<,>), GetType().Assembly);
            services.AddCqrsCommandDecorator(typeof(TestCommandHandler), typeof(TestCommandDecorator));
            services.AddTransient<MessageDispatcher>();
            Log.Logger = new LoggerConfiguration()
                .WriteTo.Console()
                .CreateLogger();
            services.AddSingleton(Log.Logger);
            _provider = services.BuildServiceProvider();
        }

        [Test]
        public async Task the_registration_of_command_works()
        {
            var messageDispatcher = _provider.GetRequiredService<MessageDispatcher>();
            var result = await messageDispatcher.DispatchAsync(new TestCommand(10));
            result.IsSuccess.Should().BeTrue();
            result.Value.Should().Be(20);
        }
    }
}