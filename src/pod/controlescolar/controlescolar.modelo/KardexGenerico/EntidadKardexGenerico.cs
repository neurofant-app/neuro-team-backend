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
        [BsonId]
        public virtual Guid Id { get; set; }
        [BsonElement("pe")]
        public string PlanEstudios { get; set; }
        [BsonElement("t")]
        public string Turno { get; set; }

        [BsonElement("g")]
        public string Grupo { get; set; }
        [BsonElement("cer")]
        public string Certificaciones { get; set; }
        [BsonElement("d")]
        public string Dedicacion { get; set; }
        [BsonElement("fg")]
        public string fotoGrafia { get; set; }
        [BsonElement("ce")]
        public string CicloEscolar { get; set; }
        [BsonElement("m")]
        public string Modalidad { get; set; }
        


    }
}
