﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Monitoring.Data.Interfaces;
using Monitoring.Infrastructure.Helpers;
using Monitoring.Infrastructure.Models;
using Monitoring.Job.Jobs;
using Monitoring.Jobs.Interfaces;
using System;
using System.Threading.Tasks;

namespace Monitoring.Services
{
    public class MonitoringService : ScopedProcessor
    {
        private readonly IDataController _dataCtrl;
        private readonly MonitorSettings _settings;
        private readonly ILogger _logger;
        private TaskConfiguration _TaskConfig;
        //https://crontab.guru/every-5-minutes
        //MINUTES HOURS DAYS MONTHS DAYS-OF-WEEK
        //protected override string Schedule => "* * * * 1-5";
        public MonitoringService(IDataController dataCtrl, IServiceScopeFactory serviceScopeFactory, IOptions<MonitorSettings> settings,
            ILogger<ScopedProcessor> logger, IHostApplicationLifetime appLifetime) : base(serviceScopeFactory, logger, appLifetime)
        {
            _dataCtrl = dataCtrl;
            _settings = settings.Value;
            _logger = logger;
        }

        //scoped call
        public override async Task ProcessInScopeAsync(IServiceProvider serviceProvider)
        {
            try
            {
                var configId = _settings.ConfigId;
                var clientId = _settings.ClientId;

                _logger.LogInformation("Pulling configuration settings from database.");
                var configData = await _dataCtrl.ReadConfiguration(configId.StringToGuid());
                string jsonConfig = String.Empty;
                if (configData != null)
                    jsonConfig = configData.Configuration;

                if (JsonValidator.IsValidJson(jsonConfig, out _TaskConfig))
                {
                    var serviceTimeInms = (_TaskConfig.Service.PollingFrequency.Minutes * 60) * 1000;

                    ITaskFactory taskObjFactory = serviceProvider.GetService<ITaskFactory>();



                    foreach (var task in _TaskConfig.Tasks)
                    {
                        var toDo = taskObjFactory.GetTask(TaskType.DicType[task.Type.ToUpper()], serviceProvider);
                        _logger.LogInformation($"{toDo.GetType().ToString().ToUpper()} task process starting.");
                        await toDo.StartTask(task, configId, clientId, null);
                    }
                    //await Task.Delay(serviceTimeInms, stoppingToken);
                    await Task.Delay(serviceTimeInms);
                }

            }
            catch (Exception e)
            {
                _logger.LogError(e, e.Message);
                throw;
            }
        }
    }
}