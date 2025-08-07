using Microsoft.AspNetCore.Mvc;

namespace Tomou.Communication.Requests.Dependent.Get;
public class DependentQuery
{
    [FromQuery(Name = "name")]
    public string? Name { get; set; }

    [FromQuery(Name = "order")]
    public string Order { get; set; } = "asc";
} 