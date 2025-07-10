using Tomou.Communication.Requests.Medications.Register;
using Tomou.Communication.Responses.Medications;

namespace Tomou.Application.UseCases.Medications.Register;
public interface IRegisterMedicationUseCase
{
    Task<ResponseRegisterMedicationJson> Execute(RequestRegisterMedicationsJson request);
}
