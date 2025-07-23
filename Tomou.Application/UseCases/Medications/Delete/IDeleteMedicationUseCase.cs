namespace Tomou.Application.UseCases.Medications.Delete;
public interface IDeleteMedicationUseCase
{
    public Task Execute(Guid? id, Guid medicamentId);
}
