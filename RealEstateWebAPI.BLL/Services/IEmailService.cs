using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using RealEstateWebAPI.BLL.DTO;

public interface IEmailService
{
    Task SendAsync(MimeMessage message);
}

public class EmailService : IEmailService
{
    private readonly EmailServiceSettings _emailSettings;

    public EmailService(EmailServiceSettings emailSettings)
    {
        _emailSettings = emailSettings;
    }

    public async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        try
        {
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.Auto);
            await client.AuthenticateAsync(_emailSettings.Username, _emailSettings.Password);

            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            // Handle exceptions or logging accordingly
            throw new Exception("An error occurred while sending the email.", ex);
        }
        finally
        {
            await client.DisconnectAsync(true);
        }
    }
}
