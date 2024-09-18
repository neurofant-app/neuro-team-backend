using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.KardexEmpledos
{
    [EntidadDB]
    public class EntidadKardexEmpleado
    {
        /// <summary>
        /// Identificador único del Kardex Empleado en el repositorio, se genera al crear un registro
        /// </summary>
        [BsonId]
        public virtual Guid Id { get; set; }
        /// <summary>
        /// Identificador de empleado dentro de Kardex Empleados
        /// </summary>
        [BsonElement("pe")]
        public virtual Guid EmpleoId { get; set; }
        /// <summary>
        ///   Identificador del turno del empleado
        /// </summary>
        [BsonElement("t")]
        public virtual Guid TurnoId { get; set; }
        /// <summary>
        ///Periodo escolar del empleado
        /// </summary>
        [BsonElement("pe")]
        public virtual string PeriodoEscolar  { get; set; }
        /// <summary>
        /// especialidad del empleado si este es profesor
        /// </summary>

        [BsonElement("ep")]
        public virtual string Especialidad { get; set; }
        /// <summary>
        ///  Documento que acredita la identidad y nacionalidad de una persona
        /// </summary>

        [BsonElement("dn")]
        public virtual string DNI { get; set; }
        /// <summary>
        ///Horario de trabajo del empleado
        /// </summary>
        [BsonElement("hl")]
        public virtual string HorarioLaboral { get; set; }
        /// <summary>
        ///   Nivel de estudios del empleado
        /// </summary>
        [BsonElement("ne")]
        public virtual Guid NivelEstudiosId { get; set; }
       


    }
}
