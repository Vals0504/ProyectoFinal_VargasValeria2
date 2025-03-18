using Microsoft.AspNetCore.Identity.UI.Services;

namespace ProyectoFinal_VargasValeria.Models
{
   public class FakeEmailSender : IEmailSender
{
    public Task SendEmailAsync(string email, string subject, string htmlMessage) {
        // Simula el envío de un correo electrónico
        System.Diagnostics.Debug.WriteLine($"Email to: {email}, Subject: {subject}, Message: {htmlMessage}");
        return Task.CompletedTask;
    }
}

}
