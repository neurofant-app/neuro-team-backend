using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Aspirantes
{
    [EntidadDB]
    public class EntidadAspirante : PersonaBase
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("pi")]
        public virtual Guid PlantelId { get; set; }
        [BsonElement("pei")]
        public virtual Guid PlanEstudiosId { get; set; }
        [BsonElement("cai")]
        public virtual Guid CarreraId { get; set; }
        [BsonElement("di")]
        public virtual Guid DireccionId { get; set; }
    }
}
