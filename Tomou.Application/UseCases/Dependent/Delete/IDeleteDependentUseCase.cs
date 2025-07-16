namespace Tomou.Application.UseCases.Dependent.Delete;
public interface IDeleteDependentUseCase
{
    Task Execute(Guid id);
}
