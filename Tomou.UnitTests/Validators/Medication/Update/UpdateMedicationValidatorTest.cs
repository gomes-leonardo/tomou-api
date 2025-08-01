using Tomou.Application.UseCases.Medications.Update;
using Shouldly;
using Tomou.TestUtils.Medication.Update.Request;

namespace Tomou.UnitTests.Validators.Medication.Update;
public class UpdateMedicationValidatorTest
{
    [Fact]
    public void Success()
    {
        var validator = new UpdateMedicationValidator();
        var request = RequestUpdateMedicationJsonBuilder.Build();

        var result = validator.Validate(request);

        result.IsValid.ShouldBeTrue();
    }
} 