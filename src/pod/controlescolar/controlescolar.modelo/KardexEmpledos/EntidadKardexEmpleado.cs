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
        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("pe")]
        public virtual Guid EmpleoId { get; set; }
        [BsonElement("c")]
        public virtual string Categoria { get; set; }
        [BsonElement("pt")]
        public virtual string PlanTrabajo  { get; set; }

        [BsonElement("ep")]
        public virtual string Especialidad { get; set; }
        
        [BsonElement("dn")]
        public virtual string DNI { get; set; }
        [BsonElement("s")]
        public virtual string Salario { get; set; }
        [BsonElement("tp")]
        public virtual string TituloProfesional { get; set; }
       


    }
}
