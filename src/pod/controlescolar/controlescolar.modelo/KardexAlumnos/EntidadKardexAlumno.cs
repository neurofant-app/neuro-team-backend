using comunes.primitivas.atributos;
using controlescolar.modelo.alumnos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.KardexAlumnos
{
    [EntidadDB]
    public class EntidadKardexAlumno
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("ai")]
        public Guid AlumnoId { get; set; }
        
        [BsonElement("ne")]
        public string NotasEntrevista   { get; set; }
        [BsonElement("er")]
        public string EvaluacionesRendimiento { get; set; }
        

        [BsonElement("es")]
        public string Especialidad { get; set; }

    }
}
