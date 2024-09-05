using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.DireccionesAlumno
{
    [EntidadDB]
    public class EntidadDireccionAlumno
    {

        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("aid")]
        public virtual required Guid AlumnolId { get; set; }
        [BsonElement("did")]
        public virtual required Guid DireccionId { get; set; }
    }
}
