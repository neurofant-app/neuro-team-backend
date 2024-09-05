using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Estados
{
    [EntidadDB]
    public class EntidadEstado
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        
        [BsonElement("p")]
        public virtual required Guid PaisId { get; set; }
        [BsonElement("n")]
        public required string? Nombre { get; set; }
        public required int CodigoEstado { get; set; }

       

    }
}
