using Newtonsoft.Json;

namespace Monitoring.Infrastructure.Models
{
    public class ResultSet
    {
        public bool Result { get; set; }

        /// <summary>
        /// In ms
        /// </summary>
        public long Duration { get; set; }

        //convert this object to json
        public override string ToString()
        {
            return JsonConvert.SerializeObject(this);
        }
    }
}
