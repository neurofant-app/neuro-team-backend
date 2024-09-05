using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Municipios
{
    [EntidadDB]
    public class EntidadMunicipio
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        
        [BsonElement("e")]
        public virtual required Guid EstadoId { get; set; }

        [BsonElement("n")]
        public string? Nombre { get; set; }

        public required int CodigoPostal { get; set; }

    }
}
