namespace DiagnosisRepositoryApi.Handlers;

public interface ICommandHandler<TCommand, TCommandResult> where TCommand : class
{
    public Task<TCommandResult> Handle(TCommand command);
}

public interface IQueryHandler<TQuery, TQueryResult> where TQuery : class
{
    public Task<TQueryResult> Handle(TQuery command);
}
