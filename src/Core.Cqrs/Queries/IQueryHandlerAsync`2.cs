using System.Threading.Tasks;

namespace Core.Cqrs.Queries
{
    public interface IQueryHandlerAsync<in TQuery, TResult>
        where TQuery : IQueryAsync<TResult>
    {
        Task<TResult> HandleAsync(TQuery query);
    }
}