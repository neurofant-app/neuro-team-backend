using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.repositorio.cfdi
{
    public class Catalogosv40
    {
        public Catalogosv40() { }
        /*
         La clave primaria debrá conformarse por los campos catalogo y clave y los valores deben tomarse 
         EN OTRA TAREA del archivo de Excel de catálogos para la versión 4.0
         */
        public Catalogosv40(string cat, string cla)
        {
            catalogo = cat;

            clave = cla;

            // Id = cat + cla 

        }
        /// <summary>
        /// Clave para catalogo 40  (no se pudo convinar la clave primaria de catalogo con clave)
        /// </summary>
        public long Id { get; set; }
        /// <summary>
        /// catalogo: string
        /// </summary>
        public string catalogo { get; set; }
        /// <summary>
        /// clave: string
        /// </summary>
        public string clave { get; set; }
        /// <summary>
        /// valor: string
        /// </summary>
        public string valor { get; set; }
        /// <summary>
        /// indice: int
        /// </summary>
        public int indice { get; set; }
        /*
         * catalogosv40
            
           Almacena el valor de los elementos de catálogo para 
           comprobantes de la versión 4.0 los campos de esta tabla son 

           catalogo: string
           clave: string
           valor: string
           indice: int

           La clave primaria debrá conformarse por los campos catalogo 
           y clave y los valores deben tomarse EN OTRA TAREA del archivo 
           de Excel de catálogos para la versión 4.0
         */
    }
}
