using modelo.repositorio.cfdi;

namespace procesador.sat.cfdi;

public static class Extensiones
{

    public static CFDI? ACFDIRepositorio(this modelo.sat.cfdi.v33.Comprobante ccfdi)
    {
        CFDI cfdiRepositorio = new()
        {
            UUID = ccfdi.Complemento.TimbreFiscalDigital.UUID,
            Version = ccfdi.Version,
            Emitido = false,
            Cancelado = false,
            FechaCFDI = ccfdi.Fecha.Ticks,
            iano = ccfdi.Fecha.Year,
            imes = ccfdi.Fecha.Month,
            idia = ccfdi.Fecha.Day,
            SubTotal = ccfdi.SubTotal,
            Total = ccfdi.Total,
            TotalIRetenidos = ccfdi.Impuestos?.TotalImpuestosRetenidos != null ? (decimal)ccfdi.Impuestos.TotalImpuestosRetenidos : 0,
            TotalITrasladados = ccfdi.Impuestos?.TotalImpuestosTrasladados != null ? (decimal)ccfdi.Impuestos.TotalImpuestosTrasladados : 0,
            Uso = ccfdi.Receptor.UsoCFDI,
            TieneIRetenidos = ccfdi.Impuestos?.Retenciones != null ? true : false,
            TieneITrasladados = ccfdi.Impuestos?.Traslados != null ? true : false,
            TieneRelacionados = ccfdi.CfdiRelacionados != null ? true : false,
            TieneI3os = false,
            TieneAddenda = ccfdi.Addenda != null ? true : false,
            TieneInfoAduanera = ccfdi.TieneIAduanera33(),
            TieneCPredial = ccfdi.TieneCPredial33(),
            TieneComplementos = ccfdi.TieneComplementosConcepto33(),
            Serie = ccfdi.Serie,
            Folio = ccfdi.Folio,
            FormaPago = ccfdi.FormaPago,
            Moneda = ccfdi.Moneda,
            TipoDeComprobante = ccfdi.TipoDeComprobante,
            MetodoPago = ccfdi.MetodoPago,
            LugarExpedicion = ccfdi.LugarExpedicion,
            RFCId = 0,
            rowid =0
            
        };

        return cfdiRepositorio;
    }

    public static CFDI? ACFDIRepositorio(this modelo.sat.cfdi.v40.Comprobante ccfdi)
    {
        CFDI CFDI = new CFDI();
        CFDI.UUID = ccfdi.Complemento.TimbreFiscalDigital.UUID;
        CFDI.Version = ccfdi.Version;
        CFDI.FechaCFDI = ccfdi.Fecha.Ticks;
        CFDI.iano = ccfdi.Fecha.Year;
        CFDI.imes = ccfdi.Fecha.Month;
        CFDI.idia = ccfdi.Fecha.Day;
        CFDI.SubTotal= ccfdi.SubTotal;
        CFDI.Total = ccfdi.Total;
        CFDI.TotalIRetenidos = (decimal)(ccfdi.Impuestos != null && ccfdi.Impuestos.TotalImpuestosRetenidos != null?  ccfdi.Impuestos.TotalImpuestosRetenidos : 0);
        CFDI.TotalITrasladados = (decimal)(ccfdi.Impuestos != null && ccfdi.Impuestos.TotalImpuestosTrasladados != null ? ccfdi.Impuestos.TotalImpuestosTrasladados : 0);
        CFDI.Uso = ccfdi.Receptor.UsoCFDI;
        CFDI.TieneIRetenidos = ccfdi.Impuestos != null ? true : false;
        CFDI.TieneITrasladados = ccfdi.Impuestos != null? true : false;
        CFDI.TieneRelacionados = ccfdi.CfdiRelacionados != null ? true : false;
        CFDI.TieneI3os = ccfdi.TieneI3ros40();
        CFDI.TieneInfoAduanera = ccfdi.TieneIAduanera40();
        CFDI.TieneCPredial = ccfdi.TieneCPredial40();
        CFDI.TieneComplementos = ccfdi.TieneComplementosConcepto40();
        CFDI.TieneAddenda = ccfdi.Addenda != null ? true : false;
        CFDI.Serie = ccfdi.Serie;
        CFDI.Folio = ccfdi.Folio;
        CFDI.FormaPago = ccfdi.FormaPago;
        CFDI.Moneda = ccfdi.Moneda;
        CFDI.TipoDeComprobante = ccfdi.TipoDeComprobante;
        CFDI.MetodoPago = ccfdi.MetodoPago;
        CFDI.LugarExpedicion = ccfdi.LugarExpedicion;
        CFDI.RFCId = 0;
        CFDI.rowid = 0;
        return CFDI;
    }

    public static bool TieneIAduanera40(this modelo.sat.cfdi.v40.Comprobante cfdi40)
    {
        if (cfdi40 != null)
        {
            if (cfdi40.Conceptos.Concepto.Any(x => x.InformacionAduanera != null && x.InformacionAduanera.Count> 0)!=false)
            {
                return true;
            }
        }
        return false;
    }


    public static bool TieneI3ros40(this modelo.sat.cfdi.v40.Comprobante cfdi40)
    {
        if (cfdi40 != null)
        {
           
            if (cfdi40.Conceptos.Concepto.Any(x => x.ACuentaTerceros != null && x.ACuentaTerceros.RfcAcuentaTerceros != null &&
                                              x.ACuentaTerceros.NombreACuentaTerceros != null && 
                                              x.ACuentaTerceros.RegimenFiscalACuentaTerceros != null && 
                                              x.ACuentaTerceros.DomicilioFiscalACuentaTerceros != null))
            {
                return true;
            }
        }
        return false;
    }

    public static bool TieneCPredial40(this modelo.sat.cfdi.v40.Comprobante cfdi40)
    {
        if (cfdi40 != null)
        {
            if (cfdi40.Conceptos.Concepto.Any(x => x.CuentaPredial.Count != 0) != false)
            {
                return true;
            }
        }
        return false;
    }

    public static bool TieneComplementosConcepto40(this modelo.sat.cfdi.v40.Comprobante cfdi40)
    {
        if (cfdi40 != null)
        {
            if (cfdi40.Conceptos.Concepto.Any(x => x.ComplementoConcepto != null && x.ComplementoConcepto.Objeto != null))
            {
                return true;
            }
        }
        return false;
    }

    public static bool TieneIAduanera33(this modelo.sat.cfdi.v33.Comprobante cfdi33)
    {
        if (cfdi33 != null)
        {
            if (cfdi33.Conceptos.Concepto.Any(x => x.InformacionAduanera!= null && x.InformacionAduanera.Count > 0) != false)
            {
                return true;
            }
        }
        return false;
    }


    public static bool TieneCPredial33(this modelo.sat.cfdi.v33.Comprobante cfdi33)
    {
        if (cfdi33 != null)
        {
            if (cfdi33.Conceptos.Concepto.Any(x => x.CuentaPredial != null))
            {
                return true;
            }
        }
        return false;
    }

    public static bool TieneComplementosConcepto33(this modelo.sat.cfdi.v33.Comprobante cfdi33)
    {
        if (cfdi33 != null)
        {
            if (cfdi33.Conceptos.Concepto.Any(x => x.ComplementoConcepto != null))
            {
                return true;
            }
        }
        return false;
    }


}
