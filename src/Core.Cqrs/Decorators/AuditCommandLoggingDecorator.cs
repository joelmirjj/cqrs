using System.Threading.Tasks;
using Core.Cqrs.Commands;
using FluentResults;
using Newtonsoft.Json;
using ILogger = Serilog.ILogger;

namespace Core.Cqrs.Decorators
{
    public sealed class
        AuditCommandLoggingDecorator<TCommand, TResult> : ICommandHandlerAsync<TCommand,
            TResult> where TCommand : ICommandAsync<TResult> where TResult : struct
    {
        private readonly ICommandHandlerAsync<TCommand, TResult> _handler;

        private readonly ILogger _logger;

        public AuditCommandLoggingDecorator(
            ICommandHandlerAsync<TCommand, TResult> handler, ILogger logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public Task<Result<TResult>> HandleAsync(TCommand command)
        {
            var serializedCommand = JsonConvert.SerializeObject(command);
            _logger.Information("Command of type {FullName}: {serializedCommand}"
                , command.GetType().FullName, serializedCommand);
            return _handler.HandleAsync(command);
        }
    }
}