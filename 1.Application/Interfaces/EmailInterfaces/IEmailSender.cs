namespace _1.Application.Interfaces.EmailInterfaces;

public interface IEmailSender
{
    Task SendAsync(string to, string subject, string body);
}