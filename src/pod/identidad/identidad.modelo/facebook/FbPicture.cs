using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace identidad.modelo.facebook
{
    public partial class FbPicture
    {
        [JsonProperty("data")]
        public FbPictureData Data { get; set; }
    }

    public partial class FbPictureData
    {
        [JsonProperty("height")]
        public int Height { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("is_silhouette")]
        public bool IsSilhouette { get; set; }

        [JsonProperty("url")]
        public string Url { get; set; }
    }
}