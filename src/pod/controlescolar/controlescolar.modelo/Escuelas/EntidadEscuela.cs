using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Escuelas
{
    [EntidadDB]
    public class EntidadEscuela
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("cid")]
        public Guid CursoId { get; set; }
        [BsonElement("n")]
        public string NombreEscuela { get; set; }
        [BsonElement("d")]
        public string Direccion { get; set; }
        [BsonElement("p")]
        public string PaginaWeb { get; set; }
        [BsonElement("r")]
        public string RedesSociales { get; set; }
    }
}
