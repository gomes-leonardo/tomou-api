using System.Diagnostics;
using Tomou.Application.Services.Email;

namespace Tomou.Infrastructure.Services.Email;

public class FakeEmailService : IEmailService
{
    public Task Send(string to, string subject, string body)
    {
        Debug.WriteLine($"""
        [FAKE EMAIL SENT]
        To: {to}
        Subject: {subject}
        Body: {body}
        """);

        return Task.CompletedTask;
    }
}
