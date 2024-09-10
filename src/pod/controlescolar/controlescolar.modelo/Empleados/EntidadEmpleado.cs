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
        [BsonId]
        public virtual Guid Id { get; set; }

        [BsonElement("eid")]
        public Guid EscuelaId { get; set; }
        [BsonElement("d")]
        public string Direccion { get; set; }
        [BsonElement("r")]
        public string RedesSociales { get; set; }
        [BsonElement("fe")]
        public string FolioEmpleado { get; set; }
        public bool isProfesor { get; set; }

        [BsonElement("ci")]
        public string Cedula_Identidad { get; set; }
        [BsonElement("aI")]
        public string Area_Trabajo { get; set; }

        [BsonElement("de")]
        public string Dedicacion { get; set; }

        [BsonElement("s")]
        public string subdivision { get; set; }

        [BsonElement("pt")]
        public string PuestoTrabajo { get; set; }

    }
}
