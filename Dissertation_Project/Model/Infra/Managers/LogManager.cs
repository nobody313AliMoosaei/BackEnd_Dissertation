using DataLayer.DataBase;
using Dissertation_Project.Model.DTO.INPUT.Temp;
using Dissertation_Project.Model.Infra.Interfaces;
using System.Globalization;

namespace Dissertation_Project.Model.Infra.Managers
{
    public class LogManager : ILogManager
    {
        private DataLayer.DataBase.Context_Project _Context;

        public LogManager(Context_Project context)
        {
            _Context = context;
        }

        public async Task<bool> InsertLogInDatabase(InsertLogDTO InsertLog)
        {
            try
            {
                DataLayer.Entities.Logs newlog = new DataLayer.Entities.Logs()
                {
                    Date = InsertLog.Date,
                    Client = InsertLog.Client,
                    Ip = InsertLog.Ip,
                    Level = InsertLog.Level,
                    Url = InsertLog.Url,
                    Time = InsertLog.Time,
                    System = InsertLog.System,
                    Method = InsertLog.Method,
                    Message = InsertLog.Message
                };
                await _Context.AddAsync(newlog);
                await _Context.SaveChangesAsync();
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
