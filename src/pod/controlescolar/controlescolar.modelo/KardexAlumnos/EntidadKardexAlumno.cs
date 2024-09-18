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
        /// <summary>
        /// Identificador único del Kardex Alumno en el repositorio, se genera al crear un registro
        /// </summary>
        [BsonId]
        public virtual Guid Id { get; set; }
        /// <summary>
        ///Identificador  de alumno dentro del kardex de alumnos
        /// </summary>
        [BsonElement("ai")]
        public Guid AlumnoId { get; set; }
  
        /// <summary>
        /// Evaluacion de rendimiento
        /// </summary>
        [BsonElement("er")]
        public string EvaluacionesRendimiento { get; set; }
       
        /// <summary>
        /// especialidad del alumno 
        /// </summary>
        [BsonElement("es")]
        public Guid EspecialidadId { get; set; }

    }
}
