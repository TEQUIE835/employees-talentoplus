using System.Net.Mail;
using _1.Application.Interfaces.EmailInterfaces;
using Microsoft.Extensions.Configuration;

namespace _1.Application.Services;

public class SmtpEmailSender : IEmailSender
{
    private readonly IConfiguration _configuration;
    public SmtpEmailSender(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    public async Task SendAsync(string to, string subject, string body)
    {
        var smtpHost = _configuration["Smtp:Host"];
        var smtpPort = int.Parse(_configuration["Smtp:Port"] ?? "25");
        var smtpUser = _configuration["Smtp:User"];
        var smtpPass = _configuration["Smtp:Password"];
        var fromEmail = _configuration["Smtp:From"];
        using var client = new SmtpClient(smtpHost, smtpPort)
        {
            Credentials = new System.Net.NetworkCredential(smtpUser, smtpPass),
            EnableSsl = true
        };
        var mailMessage = new MailMessage(fromEmail, to, subject, body)
        {
            From = new MailAddress(fromEmail),
            Subject = subject,
            Body = body,
            IsBodyHtml = true
        };
        try
        {
            mailMessage.To.Add(to);
            await client.SendMailAsync(mailMessage);
        }
        catch 
        {
            return;
        }
        
    }
}