
using System.Diagnostics;

namespace Tomou.Application.Services.Email;
public class FakeEmailService : IEmailService
{
    public Task Send(string to, string subject, string body)
    {
        Debug.WriteLine($"[FAKE EMAIL] To: {to}\nSubject: {subject}\nBody: {body}");
        return Task.CompletedTask;
    }
}
