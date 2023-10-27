using Azure.Core;
using BusinessLayer.Models;
using Hangfire;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessLayer.Services.Log
{
    public class HistoryManager : IHistoryManager
    {
        public async Task InsertHistory(DateTime DateTime, string IP, string Url, string Level, string Client, string Message)
        {
            try
            {
                BackgroundJob.Enqueue<BusinessLayer.Services.Log.ILogManager>
                        (t => t.InsertLogInDatabase(new Models.INPUT.Log.InsertLogDTO
                        {
                            Client = Client,
                            Date = DateTime,
                            Ip = IP,
                            Level = IP,
                            Url = Url,
                            Message = Message
                        }));
            }
            catch
            {

            }
        }
    }
}
