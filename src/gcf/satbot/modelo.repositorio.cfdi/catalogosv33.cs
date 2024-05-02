using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace modelo.repositorio.cfdi
{
    public class Catalogosv33
    {

        public Catalogosv33() { }


        /*
         La clave primaria debrá conformarse por los campos catalogo y clave y los valores deben tomarse 
         EN OTRA TAREA del archivo de Excel de catálogos para la versión 3.3
         */
        public Catalogosv33(string cat, string cla) 
        {
            catalogo = cat;

            clave = cla;

           // Id = cat + cla 
            
        }
        /// <summary>
        /// Clave para catalogo 33 tipo long (no se pudo convinar la clave primaria de catalogo con clave)
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
         * catalogosv33

           Almacena el valor de los elementos de catálogo para 
           comprobantes de la versión 3.3 los campos de esta tabla son 

           catalogo: string
           clave: string
           valor: string
           indice: int

           La clave primaria debrá conformarse por los campos catalogo 
           y clave y los valores deben tomarse EN OTRA TAREA del archivo 
           de Excel de catálogos para la versión 3.3
         */
    }
}
