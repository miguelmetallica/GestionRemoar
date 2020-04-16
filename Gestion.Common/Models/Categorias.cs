using Newtonsoft.Json;
using System;

namespace Gestion.Common.Models
{
    public partial class Welcome
    {
        [JsonProperty("id")]
        public long Id { get; set; }

        [JsonProperty("nombre")]
        public string Nombre { get; set; }

        [JsonProperty("slug")]
        public string Slug { get; set; }

        [JsonProperty("parentId")]
        public long ParentId { get; set; }

        [JsonProperty("descripcion")]
        public string Descripcion { get; set; }

        [JsonProperty("display")]
        public object Display { get; set; }

        [JsonProperty("imagen")]
        public string Imagen { get; set; }

        [JsonProperty("menuOrden")]
        public long MenuOrden { get; set; }

        [JsonProperty("imagenFull")]
        public Uri ImagenFull { get; set; }
    }
}
