namespace Tomou.Domain.Entities;

public class PasswordResetToken
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public long UserId { get; set; }
    public string Token { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime ExpiresAt { get; set; }
    public bool Used { get; set; } = false;

    public User? User { get; set; }  
}
