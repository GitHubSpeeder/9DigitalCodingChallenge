using Newtonsoft.Json;
using System.Collections.Generic;

namespace JsonWebService.Models
{
    public class ResponseShowModel
    {
        public ResponseShowModel(string image, string slug, string title)
        {
            Image = image;
            Slug = slug;
            Title = title;
        }

        [JsonProperty("image", Required = Required.Always)]
        public string Image { get; private set; }

        [JsonProperty("slug", Required = Required.Always)]
        public string Slug { get; private set; }

        [JsonProperty("title", Required = Required.Always)]
        public string Title { get; private set; }
    }

    public class ResponseShowModelList
    {
        public ResponseShowModelList(List<ResponseShowModel> list)
        {
            ShowList = list;
        }

        [JsonProperty("response", Required = Required.Always)]
        public List<ResponseShowModel> ShowList { get; private set; }
    }
}