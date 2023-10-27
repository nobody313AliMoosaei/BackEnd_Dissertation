using BusinessLayer.Models.INPUT.Log;
using DataLayer.DataBase;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Log
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
