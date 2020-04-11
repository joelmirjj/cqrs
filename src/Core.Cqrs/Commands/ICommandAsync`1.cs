namespace Core.Cqrs.Commands
{
    public interface ICommandAsync<TResult> where TResult : struct
    {
    }
}