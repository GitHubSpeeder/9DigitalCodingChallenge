using Newtonsoft.Json;

namespace JsonWebService.Models
{
    public class ErrorModel
    {
        public ErrorModel(string err)
        {
            Error = err;
        }

        [JsonProperty("error", Required = Required.Always)]
        public string Error { get; private set; }
    }
}