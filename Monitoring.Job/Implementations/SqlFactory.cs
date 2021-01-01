using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Monitoring.Job.Implementations
{
    public class SqlFactory : ScheduledProcessor, ISqlFactory
    {
        private readonly MonitorSettings _settings;

        public SqlFactory(IOptions<MonitorSettings> settings, IDataController dataCtr, ILogger<SqlFactory> logger) : base(dataCtr, logger)
        {
            _settings = settings.Value;
        }

        public override bool IsInitDataOk(ToDoTask task, string configID, string customerID)
        {
            bool IsValid(string config)
            {
                return Guid.TryParse(config, out Guid guid);
            }

            if (string.IsNullOrWhiteSpace(task.ConnectionString) || string.IsNullOrWhiteSpace(configID) || string.IsNullOrWhiteSpace(customerID))
            {
                return false;
            }
            if (IsValid(configID) && IsValid(customerID))
                return true;

            return false;
        }

        public override async Task Process(ToDoTask task, string configID, string customerId, string guid)
        {
            if (!IsDoTaskOk(task, configID, customerId))
                return;
            SqlConnection connection = new SqlConnection(task.ConnectionString);

            try
            {
                Stopwatch stopwatch = new Stopwatch();
                stopwatch.Start();

                connection.Open();
                var command = connection.CreateCommand();
                command.CommandText = task.Query;

                using (SqlDataReader dataReader = command.ExecuteReader()) { }
                command.CommandText = "select @@ROWCOUNT";

                var totalRow = command.ExecuteScalar();

                stopwatch.Stop();

                _logger.LogInformation("CommandText: " + task.Query + "Found :" + (int)totalRow + " Row(s).");

                ResultSet result = new ResultSet() { Duration = stopwatch.ElapsedMilliseconds };

                if ((int)totalRow > 0)
                    result.Result = true;
                else
                    result.Result = false;

                connection.Close();

                //AbMonitorResult abMonitorResult = new AbMonitorResult
                //{
                //    ResultId = new Guid(guid),
                //    ConfigId = new Guid(configID),
                //    TaskId = new Guid(task.Id),
                //    TimeStamp = DateTime.Now,
                //    TaskType = task.Type,
                //    Result = result.ToString(),
                //    Level = 1,
                //    CustomerId = new Guid(customerId)
                //};

                //if (!_dataCtr.CreateResult(abMonitorResult))
                //    _logger.LogError("Something went wrong while trying to save the data.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Something went wrong");
                throw;
            }
            await Task.CompletedTask;
        }
    }
}
