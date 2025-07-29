using Bogus;
using Tomou.Communication.Requests.Medications.Register;

namespace Tomou.TestUtils.Medication.Register.Request
{
    public static class RequestRegisterMedicationJsonBuilder
    {
        private static readonly string[] items = new[]
        {
            "Dipirona",
            "Paracetamol",
            "Ibuprofeno",
            "Amoxicilina"
        };
        public static RequestRegisterMedicationsJson Build()
        {
            return new Faker<RequestRegisterMedicationsJson>()
                .RuleFor(x => x.Name, f => f.PickRandom(items))
                .RuleFor(x => x.Dosage, f =>
                {
                    var unit = f.PickRandom("mg", "comprimido");
                    return unit switch
                    {
                        "mg" => $"{f.Random.Number(100, 1000)}mg",
                        _ => $"{f.Random.Number(1, 5)} comprimido{(f.Random.Number(1, 5) > 1 ? "s" : "")}"
                    };
                })
                .RuleFor(x => x.StartDate, f =>
                    DateOnly.FromDateTime(DateTime.Today.AddDays(f.Random.Int(-1, 0))))
                .RuleFor(x => x.EndDate, (f, x) =>
                    x.StartDate.AddDays(f.Random.Int(1, 14)))
                .RuleFor(x => x.TimesToTake, f =>
                    f.Random.ListItems(
                        new[] { "06:00", "08:00", "12:00", "18:00", "20:00" },
                        f.Random.Int(1, 3)))
                .RuleFor(x => x.DaysOfWeek, f =>
                    f.PickRandom(
                        Enum.GetNames(typeof(DayOfWeek))
                            .Select(d => d.ToLowerInvariant()),
                        f.Random.Int(1, 3))
                    .ToList())
                .RuleFor(x => x.DependentId, f =>
                    f.Random.Bool(0.5f) ? f.Random.Guid() : (Guid?)null);
        }
    }
}
