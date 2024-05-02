using modelo.sat.cfdi.v40;
using System.Text;
using System.Xml;


namespace modelo.sat.cfdi.Extensiones
{
    public static class ExtensionesCdfi40
    {
        /// <summary>
        /// Obtencion del compraboante CFDI "4.0".
        /// </summary>
        /// <param name="xmlXFDI"></param>
        /// <returns></returns>
        public static Comprobante? ObtieneCFDIV40(this string xmlXFDI)
        {
            XmlDocument root = new XmlDocument();
            root.LoadXml(xmlXFDI);
            Comprobante CFDI = new();
            if (root != null)
            {
                root.NodoUnicoPorTagName("Comprobante").EntidadDesdeNodo(CFDI);

                foreach (XmlNode v1 in root.DocumentElement.ChildNodes)
                {
                    switch (v1.LocalName)
                    {
                        case "InformacionGlobal":
                            CFDI.InformacionGlobal = v1.CrearInformacionGlobal40();
                            break;
                        case "Emisor":
                            CFDI.Emisor = v1.CrearEmisor40();
                            break;
                        case "Receptor":
                            CFDI.Receptor = v1.CrearReceptor40();
                            break;
                        case "Impuestos":
                            CFDI.Impuestos = v1.CrearImpuestos40();
                            break;
                        case "CfdiRelacionados":
                            CFDI.CfdiRelacionados.Add(v1.CrearCfdiRelacionados40());
                            break;
                        case "Conceptos":
                            CFDI.Conceptos = v1.CrearConceptos40();
                            break;
                        case "Complemento":
                            CFDI.Complemento = v1.CrearComplemento40();
                            break;
                        case "Addenda":
                            CFDI.Addenda = v1.CrearAddenda40();
                            break;

                    }
                }

                return CFDI;
            }
            return null;
        }
        /// <summary>
        /// Creacion de la Entidad InfomracionGlobal
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static InformacionGlobal CrearInformacionGlobal40(this XmlNode node)
        {
            if (node != null)
            {
                return (InformacionGlobal)node.EntidadDesdeNodo(new InformacionGlobal());
            }
            return null;

        }
        /// <summary>
        /// Creacion de la entidad Emisor
        /// para CFDI 4.0
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Emisor CrearEmisor40(this XmlNode node)
        {
            if (node != null)
            {
                return (Emisor)node.EntidadDesdeNodo(new Emisor());
            }
            return null;

        }
        /// <summary>
        /// Creacion de la entidad Receptor
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Receptor CrearReceptor40(this XmlNode node)
        {
            if (node != null)
            {
                return (Receptor)node.EntidadDesdeNodo(new Receptor());
            }
            return null;
        }
        /// <summary>
        /// Conversion de un nodo a un objeto para
        /// cualquier entidad que lo requiera.
        /// </summary>
        /// <param name="nodoRaiz"></param>
        /// <returns></returns>
        public static object ConvertirNodo(this XmlNode nodoRaiz)
        {
            if (nodoRaiz != null)
            {
                var propiedades = new Dictionary<string, object>(); //Aquí se guardan las propiedades.

                if (nodoRaiz.HasChildNodes)
                {
                    foreach (XmlNode nodoHijo in nodoRaiz.ChildNodes)
                    {
                        propiedades.Add(nodoRaiz.LocalName, ConvertirNodo(nodoRaiz));
                    }
                }
                else
                {
                    foreach (XmlAttribute nodoAtributos in nodoRaiz.Attributes)
                    {
                        propiedades.Add(nodoAtributos.LocalName, nodoAtributos.InnerText);
                    }
                }
                return new { propiedades }; //Rotorna un objeto, que tiene de propiedad un diccionario el cual se compone de un atributo valor


            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad Complemento
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Complemento CrearComplemento40(this XmlNode node)
        {
            if (node != null)
            {
                Complemento ob = new();
                if (node.HasChildNodes)
                {
                    foreach (XmlNode v1 in node.ChildNodes)
                    {
                        if (v1.LocalName == "TimbreFiscalDigital")
                        {

                            ob.TimbreFiscalDigital = (TimbreFiscalDigital?)v1.EntidadDesdeNodo(new TimbreFiscalDigital());
                        }

                    }
                    return ob;
                }

            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad Addenda
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Addenda CrearAddenda40(this XmlNode node)
        {
            if (node != null)
            {
                Addenda ob = new();
                ob.Objeto = node.ConvertirNodo();
                return ob;
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad Impuestos
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Impuestos CrearImpuestos40(this XmlNode node)
        {
            
            if (node != null)
            {
                Impuestos impuestos = new();
                node.EntidadDesdeNodo(impuestos);
                if (node.HasChildNodes)
                {

                    foreach (XmlNode nodoHijo in node.ChildNodes)
                    {
                        switch (nodoHijo.LocalName)
                        {
                            case "Retenciones":
                                impuestos.Retenciones = nodoHijo.CrearRetenciones40();
                                break;
                            case "Traslados":
                                impuestos.Traslados = nodoHijo.CrearTraslados40();
                                break;

                        }
                    }
                    
                }
                return impuestos;

            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad Retenciones
        /// para CFDI 4.0.
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Retenciones CrearRetenciones40(this XmlNode node)
        {
            Retenciones retenciones = new Retenciones();
            Retencion retencion = new Retencion();
            if (node.HasChildNodes)
            {
                foreach (XmlNode v1 in node.ChildNodes)
                {
                    if (v1.LocalName == "Retencion")
                    {
                        retencion = (Retencion)v1.EntidadDesdeNodo(new Retencion());
                        retenciones.Retencion.Add(retencion);
                    }

                }
                return retenciones;
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad Traslados
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Traslados CrearTraslados40(this XmlNode node)
        {
            Traslados traslados = new Traslados();
            Traslado traslado = new Traslado();
            if (node.HasChildNodes)
            {
                foreach (XmlNode v1 in node.ChildNodes)
                {
                    if (v1.LocalName == "Traslado")
                    {
                        traslado = (Traslado)v1.EntidadDesdeNodo(new Traslado());
                        traslados.Traslado.Add(traslado);
                    }

                }
                return traslados;
            }
            return null;
        }

        /// <summary>
        /// Creacion de la entidad CfdiRelacionados
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static CfdiRelacionados CrearCfdiRelacionados40(this XmlNode node)
        {
            if(node != null)
            {
                CfdiRelacionados cfdiRelacionados = new CfdiRelacionados();
                CfdiRelacionado cfdiRelacionado = new CfdiRelacionado();
                if (node.HasChildNodes)
                {
                    foreach (XmlNode v1 in node.ChildNodes)
                    {
                        if (v1.InnerText == "CfdiRelacionado")
                        {
                            cfdiRelacionado = (CfdiRelacionado)v1.EntidadDesdeNodo(new CfdiRelacionado());
                            cfdiRelacionados.CfdiRelacionado.Add(cfdiRelacionado);
                        }
                    }
                    return (CfdiRelacionados)node.EntidadDesdeNodo(new CfdiRelacionados());
                }
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad Conceptos
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Conceptos CrearConceptos40(this XmlNode node)
        {
            if (node != null)
            {
                Conceptos conceptos = new Conceptos();
                Concepto concepto = new Concepto();
                if (node.HasChildNodes)
                {
                    conceptos.Concepto = new List<Concepto>();
                    foreach (XmlNode v1 in node.ChildNodes)
                    {
                        if (v1.HasChildNodes)
                        {
                            foreach (XmlNode v2 in v1.ChildNodes)
                            {
                                switch (v2.LocalName)
                                {
                                    case "Impuestos":
                                        concepto.ConceptoImpuestos = v2.CrearConceptoImpuestos40();
                                        break;
                                    case "ACuentaTerceros":
                                        concepto.ACuentaTerceros = v2.CrearACuentaTercerosConcepto40();
                                        break;
                                    case "InformacionAduanera":
                                        concepto.InformacionAduanera.Add(v2.CrearInformacionAduaneraConcepto40());
                                        break;
                                    case "CuentaPredial":
                                        concepto.CuentaPredial.Add(v2.CrearCuentaPredialConcepto40());
                                        break;
                                    case "ComplementoConcepto":
                                        concepto.ComplementoConcepto = v2.CrearComplementoConcepto40();
                                        break;
                                    case "Parte":
                                        concepto.Parte.Add(v2.CrearParte40());
                                        break;
                                }
                            }
                        }
                            concepto = (Concepto)v1.EntidadDesdeNodo(new Concepto());
                            conceptos.Concepto.Add(concepto);
                    }
                    return conceptos;
                }
            }
            return null;
        }
        /// <summary>
        /// Creación de la entidad ConceptoImpuestos
        /// para CFDI "4.0"
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ConceptoImpuestos CrearConceptoImpuestos40(this XmlNode node)
        {
            if (node != null)
            {
                ConceptoImpuestos impuestos = new();
                if (node.HasChildNodes)
                {

                    foreach (XmlNode nodoHijo in node.ChildNodes)
                    {
                        switch (nodoHijo.LocalName)
                        {
                            case "Retenciones":
                                impuestos.Retenciones = nodoHijo.CrearConceptoRetenciones40();
                                break;
                            case "Traslados":
                                impuestos.Traslados = nodoHijo.CrearConceptoTraslados40();
                                break;

                        }
                    }

                }
                return impuestos;
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad ConceptoRetenciones
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ConceptoRetenciones CrearConceptoRetenciones40(this XmlNode node)
        {
            ConceptoRetenciones retenciones = new ConceptoRetenciones();
            ConceptoRetencion retencion = new ConceptoRetencion();
            if (node.HasChildNodes)
            {
                foreach (XmlNode v1 in node.ChildNodes)
                {
                    if (v1.LocalName == "Retencion")
                    {
                        retencion = (ConceptoRetencion)v1.EntidadDesdeNodo(new ConceptoRetencion());
                        retenciones.ConceptoRetencion.Add(retencion);
                    }

                }
                return retenciones;
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad ConceptoTraslados
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ConceptoTraslados CrearConceptoTraslados40(this XmlNode node)
        {
            ConceptoTraslados traslados = new ConceptoTraslados();
            ConceptoTraslado traslado = new ConceptoTraslado();
            if (node.HasChildNodes)
            {
                foreach (XmlNode v1 in node.ChildNodes)
                {
                    if (v1.LocalName == "Traslado")
                    {
                        traslado = (ConceptoTraslado)v1.EntidadDesdeNodo(new ConceptoTraslado());
                        traslados.ConceptoTraslado.Add(traslado);
                    }

                }
                return traslados;
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad ACuentaTercerosConcepto
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ACuentaTerceros? CrearACuentaTercerosConcepto40(this XmlNode node)
        {
            if (node != null)
            {
                return (ACuentaTerceros)node.EntidadDesdeNodo(new ACuentaTerceros());
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad InformacionConcepto
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static InformacionAduanera? CrearInformacionAduaneraConcepto40(this XmlNode node)
        {
            if (node != null)
            {
                return (InformacionAduanera)node.EntidadDesdeNodo(new InformacionAduanera());
            }
            return null;
        }
        /// <summary>
        /// Creacion de la entidad CuentaPredialConcepto
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static CuentaPredial CrearCuentaPredialConcepto40(this XmlNode node)
        {
            if (node != null)
            {
                return (CuentaPredial)node.EntidadDesdeNodo(new CuentaPredial());
            }
            return null;
        }
        /// <summary>
        /// Creación de la entidad Parte
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static Parte CrearParte40(this XmlNode node)
        {
            if (node != null)
            {
                Parte parte = new Parte();
                InformacionAduaneraParte informacionAduaneraParte = new InformacionAduaneraParte();
                if (node.HasChildNodes)
                {
                    foreach (XmlNode v1 in node.ChildNodes)
                    {
                        if (v1.LocalName == "InformacionAduanera")
                        {
                            informacionAduaneraParte = (InformacionAduaneraParte)v1.EntidadDesdeNodo(new InformacionAduaneraParte());
                            parte.InformacionAduaneraParte.Add(informacionAduaneraParte);
                        }
                    }
                    return (Parte)node.EntidadDesdeNodo(new Parte());

                }
            }
            return null;
        }
        /// <summary>
        /// Creación de la entidad ComplementoConcepto
        /// para CFDI "4.0".
        /// </summary>
        /// <param name="node"></param>
        /// <returns></returns>
        public static ComplementoConcepto CrearComplementoConcepto40(this XmlNode node)
        {
            if (node != null)
            {
                ComplementoConcepto ob = new();
                ob.Objeto = node.ConvertirNodo();
                return ob;
            }
            return null;
        }

        public static string ATextoIndexable(this Comprobante cfdi)
        {
            StringBuilder result = new();

            result.Append($"{cfdi.Version} {cfdi.Serie} {cfdi.Folio} {cfdi.Fecha} {cfdi.FormaPago} {cfdi.NoCertificado} {cfdi.CondicionesDePago} {cfdi.SubTotal} {cfdi.Descuento} {cfdi.Moneda} {cfdi.TipoCambio} {cfdi.Total} {cfdi.TipoDeComprobante} {cfdi.MetodoPago} {cfdi.LugarExpedicion} {cfdi.Confirmacion} ");
            result.Append($"{cfdi.Receptor.Rfc} {cfdi.Receptor.Nombre} {cfdi.Receptor.ResidenciaFiscal} {cfdi.Receptor.NumRegIdTrib} {cfdi.Receptor.UsoCFDI} ");
            result.Append($"{cfdi.Emisor.Rfc} {cfdi.Emisor.Nombre} {cfdi.Emisor.RegimenFiscal} ");
            cfdi.Conceptos.Concepto.ForEach(concepto => {
                result.Append($"{concepto.ClaveProdServ} {concepto.NoIdentificacion} {concepto.Cantidad} {concepto.ClaveUnidad} {concepto.Unidad} {concepto.Descripcion} {concepto.ValorUnitario} {concepto.Importe} {concepto.Descuento} ");
            });
            return result.ToString();
        }



    }
}
