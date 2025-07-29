using Tomou.Application.UseCases.Medications.Register;
using Shouldly;
using Tomou.TestUtils.Medication.Register.Request;

namespace Tomou.UnitTests.Validators.Medication.Register;
public class RegisterMedicationValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new RegisterMedicationValidator();
        var request = RequestRegisterMedicationJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }
}
