using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.KardexGenerico
{
    [EntidadDB]
    public class EntidadKardexGenerico
    {
        /// <summary>
        /// Identificador único del KardexGenerico en el repositorio, se genera al crear un registro
        /// </summary>
        [BsonId]
        public virtual Guid Id { get; set; }
        /// <summary>
        /// Identificador del plan de estudios dentro del kardex generico
        /// </summary>
        [BsonElement("pe")]
        public Guid PlanEstudiosId { get; set; }
        /// <summary>
        ///  Turno 
        /// </summary>
        [BsonElement("t")]
        public Guid TurnoId { get; set; }
        /// <summary>
        /// Grupo asigando
        /// </summary>

        [BsonElement("g")]
        public Guid GrupoId { get; set; }
        /// <summary>
        /// certificaciones 
        /// </summary>
        [BsonElement("cer")]
        public string Certificaciones { get; set; }
     
        /// <summary>
        /// fotografia del empleado o alumno
        /// </summary>
        [BsonElement("fg")]
        public string fotoGrafia { get; set; }
        /// <summary>
        /// Identificador del  ciclo escolar  del alumno o empleado
        /// </summary>
        [BsonElement("ce")]
        public Guid CicloEscolarId { get; set; }

    }
}
