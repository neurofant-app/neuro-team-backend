using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.DivisionAreas
{
    [EntidadDB]
    public class EntidadDivisionArea
    {
        [BsonId]
        public virtual Guid Id { get; set; }

        [BsonElement("n")]
        public string? Nombre { get; set; }

        [BsonElement("pi")]
        public virtual Guid PlantelId { get; set; }
    }
}
