using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Direcciones
{
    [EntidadDB]
    public class Direccion
    {
        [BsonId]
        public virtual Guid Id { get; set; }
       
        [BsonElement("es")]
        public virtual required Guid MunicipioId { get; set; }

        [BsonElement("ca")]
        public required string Calle { get; set; }
          
        [BsonElement("col")]
        public required string Colonia { get; set; }
       
        [BsonElement("ci")]

        public int? numeroEx { get; set; }
        public int? numeroInt { get; set; }
        
        [BsonElement("com")]
        public string? Comunidad { get; set; }



    }
}
