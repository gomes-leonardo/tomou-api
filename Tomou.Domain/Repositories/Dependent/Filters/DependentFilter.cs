namespace Tomou.Domain.Repositories.Dependent.Filters;
public class DependentFilter
{
    public Guid CaregiverId { get; }
    public string? NameContains { get; }
    public bool Ascending { get; }


    public DependentFilter(Guid caregiverId, bool ascending, string nameContains)
    {
        CaregiverId = caregiverId;
        NameContains = nameContains;
        Ascending = ascending;
    }
}
