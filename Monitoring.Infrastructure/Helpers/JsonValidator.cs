using Monitoring.Infrastructure.Models;
using Newtonsoft.Json;
using System;

namespace Monitoring.Infrastructure.Helpers
{
    public static class JsonValidator
    {
        public static bool IsValidJson(string strInput, out TaskConfiguration config)
        {
            if (string.IsNullOrWhiteSpace(strInput))
            {
                config = null;
                return false;
            }

            strInput = strInput.Trim();
            if ((strInput.StartsWith("{") && strInput.EndsWith("}")) || //For object
                (strInput.StartsWith("[") && strInput.EndsWith("]"))) //For array
            {
                try
                {
                    //var obj = JToken.Parse(strInput);
                    config = JsonConvert.DeserializeObject<TaskConfiguration>(strInput);
                    return true;
                }
                catch (JsonReaderException jex)
                {
                    config = null;
                    //Exception in parsing json
                    return false;
                }
                catch (Exception ex)
                {
                    config = null;
                    return false;
                }
            }
            else
            {
                config = null;
                return false;
            }
        }
    }
}