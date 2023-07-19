using MimeKit;
using MailKit.Net.Smtp;
using MailKit.Security;
using System.Threading.Tasks;
using RealEstateWebAPI.BLL.DTO;
using Microsoft.Extensions.Options;
using RealEstateWebAPI.Common.ErrorHandeling;

public interface IEmailService
{
    Task SendAsync(MimeMessage message);
}

public class EmailService : IEmailService
{
    
    private readonly EmailServiceSettings _emailSettings;

    public EmailService(IOptions<EmailServiceSettings> emailSettings)
    {
        _emailSettings = emailSettings.Value;
    }


    public async Task SendAsync(MimeMessage message)
    {
        using var client = new SmtpClient();

        try
        {
            // Merr Password e email nga te ruajtur ne cmd  
            string emailPassword = Environment.GetEnvironmentVariable("EMAIL_PASSWORD");

            // Kontrollon nese string eshte null
            if (string.IsNullOrEmpty(emailPassword))
            {
                throw new Exception("Email password not found. Make sure to set the EMAIL_PASSWORD environment variable.");
            }
            //lidhet me smtp 
            await client.ConnectAsync(_emailSettings.SmtpServer, _emailSettings.SmtpPort, SecureSocketOptions.Auto);
            //authentikon me ane te username dhe password
            await client.AuthenticateAsync(_emailSettings.Username, emailPassword);
            // dergo email
            await client.SendAsync(message);
        }
        catch (Exception ex)
        {
            // Handle exceptions or logging accordingly
            throw new Exception("An error occurred while sending the email.", ex);
        }
        finally
        {
            //shkeputu nga lidhja smtp
            await client.DisconnectAsync(true);
        }
    }
}
