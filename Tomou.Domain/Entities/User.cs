namespace Tomou.Domain.Entities;
public class User
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string Name { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
    public bool IsCaregiver { get; set; }
    public ICollection<Dependent> Dependents { get; set; } = new List<Dependent>();
    public ICollection<Medication> Medications { get; set; } = new List<Medication>();
    public ICollection<PasswordResetToken> PasswordResetTokens { get; set; } = new List<PasswordResetToken>();

}
