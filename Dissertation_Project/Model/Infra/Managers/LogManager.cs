using DataLayer.DataBase;
using Dissertation_Project.Model.DTO.INPUT.Temp;
using Dissertation_Project.Model.Infra.Interfaces;
using System.Globalization;

namespace Dissertation_Project.Model.Infra.Managers
{
    public class LogManager : ILogManager
    {
        private DataLayer.DataBase.Context_Project _Context;
        private IHttpContextAccessor _httpcontext;

        public LogManager(Context_Project context, IHttpContextAccessor httpcontext)
        {
            _Context = context;
            _httpcontext = httpcontext;
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

        public async void InsertLogAsync(string Level, string ActionMethod, string Url,string Message)
        {
            try
            {
                DataLayer.Entities.Logs log = new DataLayer.Entities.Logs()
                {
                    Client= _httpcontext.HttpContext?.Request.Headers["sec-ch-ua"].ToString(),
                    Date= Core.Utlities.Persian_Calender.Shamsi_Calender.GetDate_Shamsi(),
                    Level = Level,
                    Ip = _httpcontext.HttpContext?.Connection.RemoteIpAddress?.ToString(),
                    Method = ActionMethod,
                    Time = DateTime.Now.ToLongTimeString(),
                    Url = Url,
                    System = _httpcontext.HttpContext?.Request.Headers["sec-ch-ua-platform"].ToString(),
                    Message = Message
                };
                await _Context.AddAsync(log);
                await _Context.SaveChangesAsync();
            }catch
            {
                
            }
        }
    }
}
