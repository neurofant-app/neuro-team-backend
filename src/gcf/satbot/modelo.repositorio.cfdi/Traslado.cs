using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.repositorio.cfdi
{
    public class Traslado
    {

        public long Id { get; set; }

        /// <summary>
        /// Atributo de clave unica
        /// </summary>
        public long IdPadre { get; set; }
        /// <summary>
        /// Atributo requerido para señalar la base para el cálculo del impuesto, la 
        ///determinación de la base se realiza de acuerdo con las disposiciones
        ///fiscales vigentes.No se permiten valores negativos. (Tipo Base xs:decimal)
        /// </summary>
        public decimal Base { get; set; }  //Uso Requerido
        /// <summary>
        /// Atributo requerido para señalar la clave del tipo de impuesto trasladado 
        ///aplicable al concepto (Tipo Especial catCFDI:c_Impuesto)
        /// </summary>
        public string Impuesto { get; set; }  //UsoRequerido //string(10)
        /// <summary>
        /// Atributo requerido para señalar la clave del tipo de factor que se aplica a 
        ///la base del impuesto.  (Tipo Especial catCFDI:c_TipoFactor)
        /// </summary>
        public string TipoFactor { get; set; }  //UsoRequerido //string(10)
        /// <summary>
        /// Atributo condicional para señalar el valor de la tasa o cuota del impuesto 
        ///que se traslada para el presente concepto.Es requerido cuando el
        ///atributo TipoFactor tenga una clave que corresponda a Tasa o Cuota. (Tipo Base xs:decimal)
        /// </summary>
        public decimal TasaOCuota { get; set; } //UsoOpcional
        /// <summary>
        /// Atributo condicional para señalar el importe del impuesto trasladado que
        ///aplica al concepto.No se permiten valores negativos.Es requerido
        ///cuando TipoFactor sea Tasa o Cuota  (Tipo Especial tdCFDI:t_Importe)
        /// </summary>
        public decimal Importe { get; set; }  //UsoOpcional

        public CFDI Cfdi { get; set; }

        /*
         Traslado
          
         Almacena la información de los impuestos  trasladados para cada CFDI, debe 
         utilizarse como clave foránea el campo ROWID de la tabla cfdi con la finalidad 
         de economizar espacio. El valor de ROWID es de tipo numerico y el de UUID es texto 
         de longitud 48 que es muy grande para repetirse,
         */
    }
}
