﻿using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monitoring.Data.Interfaces;
using Monitoring.Infrastructure.Models;
using Monitoring.Service.Interfaces;
using Monitoring.Service.Services;
using System;
using System.Net;
using System.Net.NetworkInformation;
using System.Threading.Tasks;

namespace Monitoring.Service.Jobs
{
    public class PingFactory : ScheduledProcessor, IPingFactory
    {
        private static Ping _ping;
        private IPAddress _ip;
        private readonly MonitorSettings _settings;

        static PingFactory()
        {
            _ping = new Ping();
        }

        public PingFactory(IOptions<MonitorSettings> settings, IDataController dataCtr, ILogger<PingFactory> logger) : base(dataCtr, logger)
        {
            _settings = settings.Value;
        }

        private IPHostEntry GetIPAddress(string server)
        {
            try
            {
                return Dns.GetHostEntry(server);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Resolving [{server}] is failed to an ip host entery instance is failed.");
            }

            return null;
        }

        public override async Task Process(TasksToDo task, string configID, string customerId, string guid)
        {
            PingReply pingreply;
            IPHostEntry iPHostEntry;

            try
            {
                if (!await IsDoTaskOk(task, configID, customerId))
                    return;

                //int timeOut = 120;
                var server = task.HostName;

                iPHostEntry = GetIPAddress(task.HostName);

                foreach (var item in iPHostEntry.AddressList)
                {
                    if (item.IsIPv6SiteLocal == false)
                        _ip = item;
                }
                _logger.LogInformation("IP : " + _ip);

                lock (_ping)
                {
                    pingreply = _ping.Send(_ip/*, timeOut*/);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong while trying to Ping.");
                return;
            }

            //var res = new ResultSet() { Duration = pingreply.RoundtripTime };
            //if (pingreply.Status == IPStatus.Success)
            //    res.Result = true;
            //else
            //    res.Result = false;

            ////TODO 
            //guid = Guid.NewGuid().ToString();
            //AbMonitorResult abMonitorResult = new AbMonitorResult
            //{
            //    ResultId = new Guid(guid),
            //    ConfigId = new Guid(configID),
            //    TaskId = new Guid(task.Id),
            //    TimeStamp = DateTime.Now,
            //    TaskType = task.Type,
            //    Result = res.ToString(),
            //    Level = 1,
            //    CustomerId = new Guid(customerId)
            //};

            //if (!_dataCtr.CreateResult(abMonitorResult))
            //    _logger.LogError("Something went wrong while trying to save the data.");
            await Task.CompletedTask;
        }

        public override async Task<bool> IsInitDataOk(TasksToDo task, string configID, string customerID)
        {
            async Task<bool> IsValidAsync(string config)
            {
                return await Task.FromResult(Guid.TryParse(config, out Guid guid));
            }

            if (string.IsNullOrWhiteSpace(task.HostName) || string.IsNullOrWhiteSpace(configID) || string.IsNullOrWhiteSpace(customerID))
            {
                return await Task.FromResult(false);
            }
            if (await IsValidAsync(configID) && await IsValidAsync(customerID))
                return await Task.FromResult(true);

            return await Task.FromResult(false);
        }


    }
}
