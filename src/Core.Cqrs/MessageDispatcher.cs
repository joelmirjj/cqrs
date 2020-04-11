using System;
using System.Threading.Tasks;
using Core.Cqrs.Commands;
using Core.Cqrs.Queries;
using FluentResults;
using Microsoft.Extensions.DependencyInjection;

namespace Core.Cqrs
{
    public class MessageDispatcher
    {
        private readonly IServiceProvider _provider;

        public MessageDispatcher(IServiceProvider provider)
        {
            _provider = provider;
        }

        public async Task<Result<TResult>> DispatchAsync<TResult>(ICommandAsync<TResult> command)
            where TResult : struct
        {
            using var scope = _provider.CreateScope();
            var type = typeof(ICommandHandlerAsync<,>);
            var handlerType = type.MakeGenericType(command.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            var handle = handlerType.GetMethod("HandleAsync");
            if (handle == null)
            {
                throw new InvalidOperationException(
                    $"Method 'HandleAsync' not found on type {handlerType.FullName}");
            }

            var result =
                await (Task<Result<TResult>>) handle.Invoke(handler, new object[] {command});
            return result;
        }

        public async Task<TResult> DispatchAsync<TResult>(IQueryAsync<TResult> query)
        {
            using var scope = _provider.CreateScope();
            var type = typeof(IQueryHandlerAsync<,>);
            var handlerType = type.MakeGenericType(query.GetType(), typeof(TResult));
            var handler = scope.ServiceProvider.GetRequiredService(handlerType);
            var handle = handlerType.GetMethod("HandleAsync");
            if (handle == null)
            {
                throw new InvalidOperationException(
                    $"Method 'Handle' not found on type {handlerType.FullName}");
            }

            var result = await (Task<TResult>) handle.Invoke(handler, new object[] {query});
            return result;
        }
    }
}