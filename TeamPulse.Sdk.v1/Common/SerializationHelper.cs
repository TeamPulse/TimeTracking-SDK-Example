using Newtonsoft.Json;

namespace Telerik.TeamPulse.Sdk.Common
{
    public class SerializationHelper
    {
        public static string SerializeToJson<T>(T obj)
        {
            return JsonConvert.SerializeObject(obj);
        }

        public static T DeserializeFromJson<T>(string json)
        {
            return (T)JsonConvert.DeserializeObject(json, typeof(T));
        }
    }
}
