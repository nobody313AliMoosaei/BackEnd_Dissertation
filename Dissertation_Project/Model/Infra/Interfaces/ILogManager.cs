namespace Dissertation_Project.Model.Infra.Interfaces
{
    public interface ILogManager
    {
        Task<bool> InsertLogInDatabase(Model.DTO.INPUT.Temp.InsertLogDTO InsertLog);
    }
}
