using comunes.primitivas.atributos;
using controlescolar.modelo.Planteles;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Escuelas
{
    [EntidadDB]
    public class EntidadEscuela
    {
        /// <summary>
        /// Identificador único del Escuela en el repositorio, se genera al crear un registro
        /// </summary>
        [BsonId]
        public virtual Guid Id { get; set; }
        /// <summary>
        /// Nombre de la escuela
        /// </summary>

        [BsonElement("n")]
        public required string Nombre { get; set; }
        /// <summary>
        /// IDentificadpr único de la dirección dentro de la escuela 
        /// </summary>
        [BsonElement("d")]
        public string? DireccionId { get; set; }
        /// <summary>
        /// clave de la escuela
        /// </summary>

        [BsonElement("ce")]
        public string? ClaveEscuela { get; set; }

        public List<EntidadPlanteles> Planteles { get; set; }
    }
}
