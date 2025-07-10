namespace Tomou.Domain.Repositories.Medications;
public interface IMedicationsWriteOnlyRepository
{
    Task Add(Tomou.Domain.Entities.Medication medication);
   
}
