using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Semestre
{
    [EntidadDB]
    public class EntidadSemestre
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("pid")]
        public virtual required Guid PlantelId { get; set; }
        [BsonElement("cid")]
        public virtual required Guid CarreraId { get; set; }

        [BsonElement("Aid")]
        public virtual required Guid AlumnoId { get; set; }

        public int NumeroSemestre { get; set; }
    }
}
