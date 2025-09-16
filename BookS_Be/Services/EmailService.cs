using BookS_Be.Models;
using BookS_Be.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;

namespace BookS_Be.Services;

public class EmailService(IOptions<EmailSettings> options) : IEmailService
{
    private readonly EmailSettings _emailSettings = options.Value;
    
    private static SecureSocketOptions GetSocketOptions(EmailSettings cfg)
    {
        // Gmail: 587 ⇒ STARTTLS, 465 ⇒ SSL on connect
        if (cfg.Port == 465) return SecureSocketOptions.SslOnConnect;
        if (cfg.Port == 587) return SecureSocketOptions.StartTls;
        return cfg.UseStartTls ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
    }
    
    private async Task SendEmailAsync(string to, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_emailSettings.FromName, _emailSettings.FromAddress));
        message.To.Add(MailboxAddress.Parse(to));
        message.Subject = subject;
        message.Body = new BodyBuilder { HtmlBody = body }.ToMessageBody();

        using var client = new SmtpClient();
        var socketOptions = GetSocketOptions(_emailSettings);
        await client.ConnectAsync(_emailSettings.Host, _emailSettings.Port, socketOptions);
        client.AuthenticationMechanisms.Remove("XOAUTH2");

        if (!string.IsNullOrWhiteSpace(_emailSettings.Username))
        {
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);
        }
        
        await client.SendAsync(message);
        await client.DisconnectAsync(true);
    }

   public Task SendEmailConfirmationAsync(string to, string token)
   {
       const string subject = "Confirm your email";
       var verifyUrl = $"https://localhost:44335/api/auth/verify-email?token={Uri.EscapeDataString(token)}";
       var body = "<p>Please confirm your email by clicking the link below:</p>" +
                  $"<p><a href='{verifyUrl}'>Confirm Email</a></p>";
       return SendEmailAsync(to, subject, body);
   }
}