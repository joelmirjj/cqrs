using System.Threading.Tasks;
using Core.Cqrs.Queries;
using Newtonsoft.Json;
using Serilog;

namespace Core.Cqrs.Decorators
{
    public sealed class
        AuditQueryLoggingDecorator<TQuery, TResult> : IQueryHandlerAsync<TQuery, TResult>
        where TQuery : IQueryAsync<TResult>
    {
        private readonly IQueryHandlerAsync<TQuery, TResult> _handler;

        private readonly ILogger _logger;

        public AuditQueryLoggingDecorator(IQueryHandlerAsync<TQuery, TResult> handler
            , ILogger logger)
        {
            _handler = handler;
            _logger = logger;
        }

        public Task<TResult> HandleAsync(TQuery query)
        {
            var serializedQuery = JsonConvert.SerializeObject(query);
            _logger.Information("Query of type {FullName}: {serializedQuery}"
                , query.GetType().FullName, serializedQuery);
            return _handler.HandleAsync(query);
        }
    }
}