using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using modelo.sat.cfdi.v33;

namespace modelo.sat.cfdi.Extensiones
{
    public static class ExtensionesCfdi33
    {/// <summary>
    /// Crear un Objeto Xml apartir de un string que contiene el xml
    /// </summary>
    /// <param name="xmlXFDI">Xml</param>
    /// <returns>Comprobante</returns>
        public static Comprobante? ObtieneCFDIV33(this string xmlXFDI)
        { if (xmlXFDI != null)
            {
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(xmlXFDI);
                return doc.GenerarEntidadCdfi33();
            }
        return null;
        }
        /// <summary>
        /// Genera un CFDI 3.3 dado un Documento XML 
        /// </summary>
        /// <param name="Factura">Xml que se convertira a objeto</param>
        /// <returns>Comprobante version 3.3</returns>
        public static Comprobante GenerarEntidadCdfi33(this XmlDocument Factura)
        {
            Comprobante CFDI = Factura.DocumentElement.CrearComprobante33();
            foreach (XmlNode NodoHijo in Factura.DocumentElement.ChildNodes)
            {
                switch (NodoHijo.LocalName)
                {
                    case "CfdiRelacionados":
                        CFDI.CfdiRelacionados = NodoHijo.CrearCfdiRelacionados33();
                        break;
                    case "Emisor":
                        CFDI.Emisor = NodoHijo.CrearEmisor33();
                        break;
                    case "Receptor":
                        CFDI.Receptor = NodoHijo.CrearReceptor33();
                        break;
                    case "Conceptos":
                        CFDI.Conceptos = NodoHijo.CrearConceptos33();
                        break;
                    case "Impuestos":
                        CFDI.Impuestos = NodoHijo.CrearImpuestos33();
                        break;
                    case "Complemento":
                        CFDI.Complemento = NodoHijo.CrearComplemento33();
                        break;
                    case "Addenda":
                        CFDI.Addenda = NodoHijo.CrearAddenda33();
                        break;
                    default:
                        throw new Exception();
                }

            }
            return CFDI;          
        }
        /// <summary>
        /// Genera una entidad Comprobante dado un nodo
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento Comprobante</param>
        /// <returns>Objeto Comprobante</returns>
        public static Comprobante CrearComprobante33(this XmlNode Nodo)
        {
            return (Comprobante)Nodo.EntidadDesdeNodo(new Comprobante());
        }
        /// <summary>
        /// Genera una entidad CfdiRelacionados dado un nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento CfdiRelacionados </param>
        /// <returns>Objeto CfdiRelacionados</returns>
        public static CfdiRelacionados? CrearCfdiRelacionados33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                CfdiRelacionados cfdiRelacionados = new();
                Nodo.EntidadDesdeNodo(cfdiRelacionados);
                if (Nodo.HasChildNodes)
                {
                    foreach (XmlNode NodoHijo in Nodo)
                    {
                        cfdiRelacionados.CfdiRelacionado?.Add((CfdiRelacionado)NodoHijo.EntidadDesdeNodo(new CfdiRelacionado()));
                    }
                }
                return cfdiRelacionados;
            }
            return null;
        }
        /// <summary>
        /// Genera una entidad Emisor Dado un Nodo.
        /// </summary>
        /// <param name="Nodo">Contiene el nodo Emisor del XML</param>
        /// <returns>Objeto Emisor</returns>
        public static Emisor CrearEmisor33(this XmlNode Nodo)
        {
            return (Emisor)Nodo.EntidadDesdeNodo(new Emisor());
        }
        /// <summary>
        /// Genera una entidad Receptor dado un Nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento Receptor</param>
        /// <returns>Objeto Receptor</returns>
        public static Receptor CrearReceptor33(this XmlNode Nodo)
        {
            return (Receptor)Nodo.EntidadDesdeNodo(new Receptor());
        }
        /// <summary>
        /// Genera una entidad Addenda dado un nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento Addenda</param>
        /// <returns>Objeto Addenda</returns>
        public static Addenda? CrearAddenda33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                Addenda addenda = new Addenda();
                addenda.Any = Nodo.ObjetoDesdeNdo();
                return addenda;
            }
            return null;
        }
        /// <summary>
        /// Genera Una Lista de entidades tipo Complemento dado un Nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el Elemento Complementos</param>
        /// <returns>Lista List<Complemento> </returns>
        public static Complemento? CrearComplemento33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                Complemento complemento = new ();
                if (Nodo.HasChildNodes)
                {
                    foreach (XmlNode NodoHijo in Nodo.ChildNodes)
                        
                    {
                        if (NodoHijo.LocalName=="TimbreFiscalDigital")
                        {
                            complemento.TimbreFiscalDigital = NodoHijo.GenerarTimbreFiscal();
                        }
                    }
                }
                return complemento;
            }
            return null;
        }
        /// <summary>
        /// Genera una entidad Impuestos  dado un Nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento Impuestos</param>
        /// <returns>Objeto Impuestos</returns>
        public static Impuestos? CrearImpuestos33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                Impuestos impuestos = new();
                Nodo.EntidadDesdeNodo(impuestos);
                if (Nodo.HasChildNodes)
                {
                    foreach (XmlNode NodoHijo in Nodo.ChildNodes)
                    {
                        var ObjetoNdo = impuestos.PropiedadObjeto(NodoHijo.LocalName).GetValue(impuestos);

                        if (NodoHijo.HasChildNodes)
                        {
                            var ListaObjetoNdo = ObjetoNdo?.PropiedadObjeto(NodoHijo.FirstChild.LocalName).GetValue(ObjetoNdo);
                            foreach (XmlNode HijoNodoHijo in NodoHijo)
                            {
                                if (ListaObjetoNdo is IList lista)
                                {
                                    lista.Add(HijoNodoHijo.EntidadDesdeNodo(Activator.CreateInstance(ListaObjetoNdo.GetType().GetGenericArguments()[0])));
                                }
                            }
                        }
                    }
                }
                return impuestos;
            }
            return null;
        }

        /// <summary>
        /// Genera un entidad de tipo conceptos dado un nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento Conceptos</param>
        /// <returns>Objeto Conceptos</returns>
        public static Conceptos CrearConceptos33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                v33.Conceptos conceptos = new();
                foreach (XmlNode NodoHijo in Nodo)
                {
                    conceptos.Concepto.Add(NodoHijo.CrearConcepto33());
                }
                return conceptos;
            }
            return null;
        }

        /// <summary>
        /// Genera una entidad de tipo Concepto dado un Nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento Concepto</param>
        /// <returns>Objeto Concepto</returns>
        /// <exception  cref="Exception"></exception>
        public static Concepto CrearConcepto33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                Concepto concepto = new();
                Nodo.EntidadDesdeNodo(concepto);
                if (Nodo.HasChildNodes)
                {
                    foreach (XmlNode NodoHijo in Nodo.ChildNodes)
                    {
                        switch (NodoHijo.LocalName)
                        {
                            case "Impuestos":
                                concepto.Impuestos = NodoHijo.CrearImpuestosConcepto33();
                                break;
                            case "InformacionAduanera":
                                concepto.InformacionAduanera.Add(NodoHijo.CrearInformacionAduaneraConcepto());
                                break;
                            case "CuentaPredial":
                                concepto.CuentaPredial = NodoHijo.CrearCuentaPredialConcepto();
                                break;
                            case "ComplementoConcepto":
                                concepto.ComplementoConcepto = NodoHijo.CrearComplementoConcepto33();
                                break;
                            case "Parte":
                                concepto.Parte.Add(NodoHijo.CrearParte33());
                                break;
                            default:
                                throw new Exception();
                        }

                    }

                }
                return concepto;
            }
            return null;
        }

        /// <summary>
        /// Genera una lista de entidades de tipo Parte dado un Nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento Parte</param>
        /// <returns>List List<Parte></returns>
        public static Parte? CrearParte33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {          
                Parte parte = new Parte();
                Nodo.EntidadDesdeNodo(parte);
                if (Nodo.HasChildNodes)
                {
                    foreach (XmlNode HijoNodoHijo in Nodo)
                    {
                        parte.InformacionAduanera?.Add((InformacionAduaneraParte)HijoNodoHijo.EntidadDesdeNodo(new InformacionAduaneraParte()));
                    }
                }
                return parte;              
            }
            return null;
        }
        /// <summary>
        /// Genera una entidad de tipo ComplementoConcepto dado un nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento ComplementoConcepto</param>
        /// <returns>Objeto ComplementoConcepto</returns>
        public static ComplementoConcepto? CrearComplementoConcepto33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                ComplementoConcepto complementoConcepto = new();
                complementoConcepto.Any = Nodo.ObjetoDesdeNdo();
                return complementoConcepto;
            }
            return null;
            
        }
        /// <summary>
        /// Genera una entidad de tipo CuentaPredialConcepto dado un nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento CuentaPredialConcepto</param>
        /// <returns>objeto CuentaPredialConcepto</returns>
        public static CuentaPredial? CrearCuentaPredialConcepto(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                return (CuentaPredial)Nodo.EntidadDesdeNodo(new CuentaPredial());
            }
            return null;
        }
        /// <summary>
        /// Genera una lista de entidades de tipo InformacionAduaneraConcepto dado un nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento InformacionAduaneraConcepto</param>
        /// <returns>Lista List<InformacionAduaneraConcepto></returns>
        public static InformacionAduaneraConcepto? CrearInformacionAduaneraConcepto(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                return (InformacionAduaneraConcepto)Nodo.EntidadDesdeNodo(new InformacionAduaneraConcepto());
            }
            return null;
        }
        /// <summary>
        /// Genera una entidad de tipo ImpuestosConcepto dado un nodo.
        /// </summary>
        /// <param name="Nodo">Nodo que contiene el elemento ImpuestosConcepto</param>
        /// <returns>Objeto ImpuestosConcepto</returns>
        public static ImpuestosConcepto? CrearImpuestosConcepto33(this XmlNode Nodo)
        {
            if (Nodo != null)
            {
                ImpuestosConcepto impuestos = new();
                if (Nodo.HasChildNodes)
                {
                    foreach (XmlNode NodoHijo in Nodo.ChildNodes)
                    {
                        var ObjetoNdo = impuestos.PropiedadObjeto(NodoHijo.LocalName).GetValue(impuestos);

                        if (NodoHijo.HasChildNodes)
                        {
                            var ListaObjetoNdo = ObjetoNdo.PropiedadObjeto(NodoHijo.FirstChild.LocalName).GetValue(ObjetoNdo);
                            foreach (XmlNode HijoNodoHijo in NodoHijo)
                            {
                                if (ListaObjetoNdo is IList lista)

                                {
                                    lista.Add(HijoNodoHijo.EntidadDesdeNodo(Activator.CreateInstance(ListaObjetoNdo.GetType().GetGenericArguments()[0])));
                                }
                            }
                        }
                    }
                }
                return impuestos;
            }
            return null;
        }

        public static TimbreFiscalDigital? GenerarTimbreFiscal (this XmlNode Nodo)
        { if (Nodo != null)
            {
                return (TimbreFiscalDigital)Nodo.EntidadDesdeNodo(new TimbreFiscalDigital());
            
            }
            return null;
        }
        public static string ATextoIndexable(this Comprobante cfdi)
        {
            StringBuilder result=new StringBuilder();

            result.Append($"{cfdi.Complemento.TimbreFiscalDigital.UUID} {cfdi.Version} {cfdi.Serie} {cfdi.Folio} {cfdi.Fecha} {cfdi.FormaPago} {cfdi.NoCertificado} {cfdi.CondicionesDePago} {cfdi.SubTotal} {cfdi.Descuento} {cfdi.Moneda} {cfdi.TipoCambio} {cfdi.Total} {cfdi.TipoDeComprobante} {cfdi.MetodoPago} {cfdi.LugarExpedicion} {cfdi.Confirmacion} ");
            result.Append($"{cfdi.Receptor.Rfc} {cfdi.Receptor.Nombre} {cfdi.Receptor.ResidenciaFiscal} {cfdi.Receptor.NumRegIdTrib} {cfdi.Receptor.UsoCFDI} ");
            result.Append($"{cfdi.Emisor.Rfc} {cfdi.Emisor.Nombre} {cfdi.Emisor.RegimenFiscal} ");
            cfdi.Conceptos.Concepto.ForEach(concepto => {
                result.Append($"{concepto.ClaveProdServ} {concepto.NoIdentificacion} {concepto.Cantidad} {concepto.ClaveUnidad} {concepto.Unidad} {concepto.Descripcion} {concepto.ValorUnitario} {concepto.Importe} {concepto.Descuento} ");
            });
            return result.ToString();
        }
    }
}



    