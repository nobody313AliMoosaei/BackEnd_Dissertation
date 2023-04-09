namespace Dissertation_Project.Model.Infra.Interfaces
{
    public interface IEmailSender
    {
        Task<bool> SendEmailAsync(string To, string Title, string Body);
    }
}
