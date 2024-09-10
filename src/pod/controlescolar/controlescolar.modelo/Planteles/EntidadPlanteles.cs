using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Planteles
{
    [EntidadDB]
    public class EntidadPlanteles
    {
        [BsonId]
        public virtual Guid Id { get; set; }

        [BsonElement("eid")]
        public Guid EscuelaId { get; set; }

        [BsonElement("d")]
        public string Direccion { get; set; }
        [BsonElement("dat")]
        public string DivisionAreasTrabajo { get; set; }

        public int NumeroHabitates { get; set; }
        public string PlanEstudios { get; set; }
        public string Programas { get; set; }
        public string MetodosEnsenanza { get; set; }
        
    }
}
