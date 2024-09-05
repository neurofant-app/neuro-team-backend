using comunes.primitivas.atributos;
using MongoDB.Bson.Serialization.Attributes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace controlescolar.modelo.Correos
{
    [EntidadDB]
    public class EntidadCorreo
    {
        [BsonId]
        public virtual Guid Id { get; set; }
        
        [BsonElement("n")]
        public virtual string Nombre { get; set; }
    }
}
