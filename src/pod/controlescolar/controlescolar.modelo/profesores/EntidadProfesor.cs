using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.profesores
{
    [EntidadDB]
    public class EntidadProfesor : PersonaBase
    {

        /// <summary>
        /// Identificador único del profesor en el repositorio, se genera al crear un registro
        /// </summary>
        [BsonId]
        public virtual Guid Id { get; set; }


        /// <summary>
        /// Identificador único del campus al que pertenece el profesor
        /// </summary>
        [BsonElement("cid")]
        public virtual required Guid CampusId { get; set; }

        /// <summary>
        /// IDentificadpr único del profesor dentro del campus por ejemplo número de profesor 
        /// </summary>
        [BsonElement("idi")]
        public string? IdInterno { get; set; }
    }
}
