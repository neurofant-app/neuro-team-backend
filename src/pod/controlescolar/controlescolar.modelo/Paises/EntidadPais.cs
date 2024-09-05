using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Paises
{
    [EntidadDB]
    public class EntidadPais
    {
        [BsonId]
        public virtual Guid Id { get; set; }

        [BsonElement("n")]
        public required string? Nombre { get; set; }

        public required int CodigoPais { get; set; }

    }
}
