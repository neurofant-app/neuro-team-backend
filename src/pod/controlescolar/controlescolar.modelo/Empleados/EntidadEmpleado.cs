using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Empleados
{
    [EntidadDB]
    public class EntidadEmpleado : PersonaBase
    {
        /// <summary>
        /// Identificador único del Empleado en el repositorio, se genera al crear un registro
        /// </summary>
        [BsonId]
        public virtual Guid Id { get; set; }
        /// <summary>
        /// Identificador único del Plantel en el repositorio, se genera al crear un registro
        /// </summary>

        [BsonElement("eid")]
        public Guid EscuelaId { get; set; }
        /// <summary>
        /// IDentificadpr único de la dirección dentro del empleado 
        /// </summary>
        [BsonElement("d")]
        public Guid DireccionId { get; set; }
         /// <summary>
         /// Folio o número de empleado destinado para cada empleado del plantel
         /// </summary>
  
        [BsonElement("fe")]
        public string FolioEmpleado { get; set; }
        /// <summary>
        /// Validar si es profesor o administrativo
        /// </summary>
        public bool isProfesor { get; set; }
        /// <summary>
        /// Cedula de identidad del empleado
        /// </summary>

        [BsonElement("ci")]
        public string Cedula_Identidad { get; set; }


    }
}
