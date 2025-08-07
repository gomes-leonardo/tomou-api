using Microsoft.AspNetCore.Mvc;

namespace Tomou.Communication.Requests.Medications.Get;
public class MedicationsQuery
{
    [FromQuery(Name = "id")]
    public Guid? Id { get; set; }

    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    [FromQuery(Name = "order")]
    public string Order { get; set; } = "asc";
} 