using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Cursos
{
    [EntidadDB]
    public class EntidadCurso
    {
        [BsonId]

        public virtual Guid Id { get; set; }

        [BsonElement("pid")]
        public virtual required Guid PlantelId { get; set; }
        [BsonElement("aid")]
        public virtual required Guid AreaId { get; set; }

        [BsonElement("caid")]
        public virtual required Guid CarreraId { get; set; }

        [BsonElement("c")]
        public virtual string Clave { get; set; }

        [BsonElement("n")]
        public virtual string Nombre { get; set; }

    }
}
