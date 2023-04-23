namespace Dissertation_Project.Model.Infra.Interfaces
{
    public interface IGoogle_Recaptcha
    {
        Task<bool> Verify(string Token);
    }
}
