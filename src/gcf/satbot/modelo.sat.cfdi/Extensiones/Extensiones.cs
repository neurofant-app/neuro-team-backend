using modelo.sat.cfdi.v33;
using modelo.sat.cfdi.v40;
using System;
using System.Reflection;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace modelo.sat.cfdi.Extensiones
{
    public static class Extensiones
    {
        /// <summary>
        /// LLena automaticamente las propiedades de un objeto a partir de los atributos de un nodo de XML
        /// </summary>
        /// <param name="Node">El nodo con los datos</param>
        /// <param name="Objeto">El objeto a llenar</param>
        /// <param name="Namespace">Espacio de nombres del nodo en el XML</param>
        /// <returns>El objeto con las propiedades que coinciden exactamente con los atributos del XML</returns>
        public static object EntidadDesdeNodo(this XmlNode Node, object Objeto)
        {
            if (Node.Attributes != null)
            {
                var Diccionario = new Dictionary<string, string>(StringComparer.InvariantCultureIgnoreCase);
                foreach (XmlAttribute item in Node.Attributes)
                {
                    Diccionario.Add(item.LocalName, item.Value);
                }
                foreach (var propertyInfo in Objeto.GetType().GetProperties())
                {
                    if (Diccionario.ContainsKey(propertyInfo.Name))
                    {
                        switch (propertyInfo.PropertyType)
                        {
                            case Type type when type == typeof(string):

                                propertyInfo.SetValue(Objeto, Diccionario[propertyInfo.Name]);
                                break;

                            case Type type when type == typeof(decimal) || type == typeof(decimal?):

                                propertyInfo.SetValue(Objeto, decimal.Parse(Diccionario[propertyInfo.Name]));
                                break;

                            case Type type when type == typeof(DateTime):

                                propertyInfo.SetValue(Objeto, DateTime.Parse(Diccionario[propertyInfo.Name]));
                                break;

                            default:
                                throw new NotImplementedException();
                        }

                  }

                  

                }
            }
            return Objeto;
        }
        /// <summary>
        /// Devuelve un elemento único a partir del nombre de un documento de XML Utilizando el metodo GetElementsByTagName
        /// </summary>
        /// <param name="documento">XmlDocumento donde se buscara el nodo</param>
        /// <param name="Elemento">Nombre del Nodo</param>
        /// <returns>XmlNode que coincida con Elemento</returns>
        public static XmlNode? NodoUnicoPorTagName(this XmlDocument Documento, string Elemento, string Namespace = "cfdi:")
        {
            var lista = Documento.GetElementsByTagName($"{Namespace}{Elemento}");
            if (lista.Count == 1)
            {
                return lista[0];
            }
            else
            {
                return null;
            }
        }
        /// <summary>
        /// Devuelve una lista de nodos a partir del nombre de un documento de XML
        /// </summary>
        /// <param name="Documento">XmlDocumento donde buscaran los nodos</param>
        /// <param name="Elemento">Nombre del nodo a buscar</param>
        /// <param name="Namespace">NameSpace del XmlDocument</param>
        /// <returns>XmlNodeList que contiene todos los nodos que conincidan con Elemento</returns>
        public static XmlNodeList NodosPorTagName(this XmlDocument Documento, string Elemento, string Namespace = "cfdi:")
        {
            return Documento.GetElementsByTagName($"{Namespace}{Elemento}");
        }
        /// <summary>
        /// Devuelve el valor de un atributo en especifico dado un nodo
        /// </summary>
        /// <param name="Nodo">Nodo donde se buscara el atributo</param>
        /// <param name="Atributo">Nombre el atributo a buscar</param>
        /// <returns>XmlAtributte que con incida con el valor de Atributo</returns>
        public static string? ValorAtributoNodoPorNombre(this XmlNode Nodo, string Atributo)
        {
            return Nodo.Attributes[Atributo]?.Value;
        }
        /// <summary>
        /// Convierte un XmlNode a un Objeto,donde los atributos del nodo se convierte a un diccionario de datos que funge como propiedad del objeto creado
        /// </summary>
        /// <param name="Nodo">Nodo que se convertira a objeto</param>
        /// <returns>El nodo y sus hijos convertido a Object</returns>
        public static object ObjetoDesdeNdo(this XmlNode Nodo)
        {
            var Atributos = new Dictionary<string, object>();
            foreach (XmlNode NodoHijo in Nodo.ChildNodes)
            {
                if (NodoHijo.HasChildNodes)
                {
                    Atributos.Add(NodoHijo.Name, NodoHijo.ObjetoDesdeNdo());
                }
                else
                {
                    foreach (XmlAttribute atributo in NodoHijo.Attributes)
                    {
                        Atributos.Add(atributo.Name, atributo.Value);
                    }
                }
            }
            return new { Atributos };
        }

        /// <summary>
        /// Extrae mediante reflexion la propiedad de un objeto de acuerdo a Name
        /// </summary>
        /// <param name="Objeto">Objeto donde se buscara la propiedad</param>
        /// <param name="Name">Nombre de la propiedad a buscar</param>
        /// <returns>Propiedad tipo Type que coincida con el valor de Name</returns>
        public static PropertyInfo? PropiedadObjeto(this Object Objeto, string Name)
        {
            return Objeto.GetType().GetProperty(Name);
        }
        /// <summary>
        /// Devuelve un elemento único a partir del nombre de un documento de XML Utilizando el metodo GetElementsByTagName
        /// utilizando el metodo SelectSingleNode
        /// </summary>
        /// <param name="Doc">XML donde se buscara el nodo</param>
        /// <param name="Name">Nombre del Nodo a buscar</param>
        /// <returns>XmlNode que coincida con el valor de Name</returns>
        public static XmlNode? NodoPorTagName(this XmlDocument Doc, string Name)
        {
            XmlNamespaceManager nsManager = new XmlNamespaceManager(Doc.NameTable);
            nsManager.AddNamespace("cfdi", Doc.DocumentElement.NamespaceURI);            
            return Doc.DocumentElement.SelectSingleNode($"cfdi:{Name}", nsManager);
        }

        public static string? GetUUID(this XmlDocument Doc)
        {
            string UUID=string.Empty;
            foreach (XmlNode NodoHijo in Doc.NodoPorTagName("Comprobante"))

            {
                if (NodoHijo.LocalName == "TimbreFiscalDigital")
                {
                    UUID = NodoHijo.ValorAtributoNodoPorNombre("UUID");
                }
            }
                return UUID;
        }
        }
}
