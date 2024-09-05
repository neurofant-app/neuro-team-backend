using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.DirecionCampus
{
    [EntidadDB]
    public class EntidadDireccionCampus
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("pid")]
        public virtual required Guid PlantelId { get; set; }
        [BsonElement("did")]
        public virtual required Guid DireccionId { get; set; }
    }
}
