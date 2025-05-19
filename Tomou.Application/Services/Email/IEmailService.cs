namespace Tomou.Application.Services.Email;
public interface IEmailService
{
    Task Send(string to, string subject, string body);
}
