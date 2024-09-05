using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Horario
{
    public class EntidadHorario
    {
        [BsonId]
        public virtual Guid Id { get; set; }
      
        [BsonElement("ai")]
        public virtual Guid AulaId { get; set; }
      
        [BsonElement("ci")]
        public virtual Guid CursoId { get; set; }

        [BsonElement("hi")]
        public DateTime? Hora_Inicio { get; set; }
        
        [BsonElement("hf")]
        public DateTime? Hora_Final { get; set; }
        
        [BsonElement("s")]
        public string Seccion { get; set; }
        
        [BsonElement("dni")]
        public string Dni_Profesor { get; set; }
    }
}
