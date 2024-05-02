
using Microsoft.EntityFrameworkCore;
using modelo.repositorio.cfdi;
using modelo.sat.cfdi.Extensiones;
using sat.bot.modelo;
using sat.bot.services;
using System.Text.Json;
using System.Xml;

namespace procesador.sat.cfdi;

public class ProcesadorCFDI : IProcesadorCFDI
{
    private readonly DbContextSqLite _db;

    public ProcesadorCFDI(DbContextSqLite db)
    {
        this._db = db;
    }

    /// <summary>
    /// Creacion de la Entidad Repositorio para
    /// CFDI "3.3" ó "4.0".
    /// </summary>
    /// <param name="xmlCFID"></param>
    /// <returns></returns>
    public CFDI? EntidadRepositorio(string xmlCFID)
    {
        // Verifica si el texto es un CFDI
        var version = VersionCFDI(xmlCFID);
        if(version != cfdi.VersionCFDI.NoEsCFDI)
        {
            // Obtiene el objeto CFDI de acuerdo al tipo
            var objetoCFDI = ProcesaXML(xmlCFID, version);
            if(objetoCFDI != null)
            {
                switch(version)
                {
                    case cfdi.VersionCFDI.v33:
                        return ((modelo.sat.cfdi.v33.Comprobante)objetoCFDI).ACFDIRepositorio();

                    case cfdi.VersionCFDI.v40:
                        return ((modelo.sat.cfdi.v40.Comprobante)objetoCFDI).ACFDIRepositorio();
                }
            }
        }
        return null;
    }
    /// <summary>
    /// Se procesa el XML para la obtencion
    /// de un CFDI ya sea v33 ó v40.
    /// </summary>
    /// <param name="xmlCFID"></param>
    /// <param name="version"></param>
    /// <returns></returns>
    public object? ProcesaXML(string xmlCFID, VersionCFDI? version = null)
    {
        if(version == null)
        {
            version = VersionCFDI(xmlCFID);
        }
        if (version != cfdi.VersionCFDI.NoEsCFDI)
        {
            switch (version)
            {
                case cfdi.VersionCFDI.v33:
                    return xmlCFID.ObtieneCFDIV33();

                case cfdi.VersionCFDI.v40:
                    return xmlCFID.ObtieneCFDIV40();
            }
        }

        return null;
    }

    /// <summary>
    /// Se retorna la version de CFDI.
    /// </summary>
    /// <param name="xmlCFID"></param>
    /// <returns></returns>
    public  VersionCFDI VersionCFDI(string xmlCFID)
    {
        XmlDocument doc = new XmlDocument();
        doc.LoadXml(xmlCFID);
        string Version = doc.DocumentElement!.GetAttribute("Version");
        switch (Version)
        {
            case "3.3":
                return cfdi.VersionCFDI.v33;

            case "4.0":
                return cfdi.VersionCFDI.v40;

            default:
                return cfdi.VersionCFDI.NoEsCFDI;
        }
    }

    /// <summary>
    /// Retorna RFC de Receptor.
    /// </summary>
    /// <param name="xmlCFID"></param>
    /// <returns></returns>
    public string DevuelveRFCReceptor(string xmlCFID)
    {
        XmlDocument root = new XmlDocument();
        root.Load(xmlCFID);
        return root.NodoPorTagName("Receptor")!.ValorAtributoNodoPorNombre("RFC")!;
        
    }

    /// <summary>
    /// Retorna RFC de Emisor.
    /// </summary>
    /// <param name="xmlCFID"></param>
    /// <returns></returns>
    public string DevuelveRFCEmisor(string xmlCFID)
    {
        XmlDocument root = new XmlDocument();
        root.Load(xmlCFID);
        return root.NodoPorTagName("Emisor")!.ValorAtributoNodoPorNombre("RFC")!;
    }

    public async Task<CFDI> ProcesarEmitido(CfdiEmitido emitido)
    {

            await verificarRFC(emitido.RFCEmisor, emitido.NombreEmisor);
            await verificarRFC(emitido.RFCReceptor, emitido.NombreEmisor);
            var CFDI = await _db.CFDIs.FirstOrDefaultAsync(_ => _.UUID == emitido.FolioFiscal);
            if (CFDI != null && emitido.EstadoComprobante == "Cancelado")
            {
                CFDI.Cancelado = true;
            }
            else
            {
                if (emitido.EstadoComprobante == "Cancelado")
                {
                    CFDI = new CFDI()
                    {
                        rowid = 0,
                        UUID = emitido.FolioFiscal,
                        Version = "",
                        Emitido = false,
                        Cancelado = true,
                        FechaCFDI = emitido.FechaEmisión.Ticks,
                        iano = emitido.FechaEmisión.Year,
                        imes = emitido.FechaEmisión.Month,
                        idia = emitido.FechaEmisión.Day,
                        RFCId = 0,
                        SubTotal = 0,
                        Total = emitido.Total,
                        TotalIRetenidos = 0,
                        TotalITrasladados = 0,
                        Uso = null,
                        TieneIRetenidos = false,
                        TieneITrasladados = false,
                        TieneRelacionados = false,
                        TieneI3os = false,
                        TieneInfoAduanera = false,
                        TieneCPredial = false,
                        TieneComplementos = false,
                        TieneAddenda = false,
                        Serie = null,
                        Folio = null,
                        FormaPago = null,
                        Moneda = null,
                        TipoDeComprobante = "",
                        MetodoPago = null,
                        LugarExpedicion = ""
                    };

                    var rfc = await GetRFC(emitido.RFCReceptor);
                    if (rfc != null)
                    {
                        CFDI.RFCId = rfc.rowid;
                    }
                }
                else
                {
                var rutaXml = Environment.GetEnvironmentVariable("PathDescargaXML");
                var ruta = Path.Combine(rutaXml,$@"{emitido.FolioFiscal}.xml");
                    try
                    {
                        var xml = File.ReadAllText(ruta);
                        CFDI = EntidadRepositorio(xml);
                        CFDI.Cancelado = false;
                        CFDI.Emitido = true;
                        var rfc = await GetRFC(emitido.RFCEmisor);
                        if (rfc != null)
                        {
                            CFDI.RFCId = rfc.rowid;
                        }
                    }
                    catch { return CFDI; }
                }
            }
            await updateCfdi(CFDI);
        return CFDI;

    }

    public async Task<CFDI> ProcesarRecibido(CfdiRecibido recibido)
    {

            await verificarRFC(recibido.RFCEmisor, recibido.NombreEmisor);
            await verificarRFC(recibido.RFCReceptor, recibido.NombreEmisor);
            var CFDI = await _db.CFDIs.FirstOrDefaultAsync(_ => _.UUID == recibido.FolioFiscal);
            if (CFDI != null && recibido.EstadoComprobante == "Cancelado")
            {
                CFDI.Cancelado = true;
            }
            else
            {
                if (recibido.EstadoComprobante == "Cancelado")
                {
                    CFDI = new CFDI()
                    {
                        rowid = 0,
                        UUID = recibido.FolioFiscal,
                        Version = "",
                        Emitido = false,
                        Cancelado = true,
                        FechaCFDI = recibido.FechaEmisión.Ticks,
                        iano = recibido.FechaEmisión.Year,
                        imes = recibido.FechaEmisión.Month,
                        idia = recibido.FechaEmisión.Day,
                        RFCId = 0,
                        SubTotal = 0,
                        Total = recibido.Total,
                        TotalIRetenidos = 0,
                        TotalITrasladados = 0,
                        Uso = null,
                        TieneIRetenidos = false,
                        TieneITrasladados = false,
                        TieneRelacionados = false,
                        TieneI3os = false,
                        TieneInfoAduanera = false,
                        TieneCPredial = false,
                        TieneComplementos = false,
                        TieneAddenda = false,
                        Serie = null,
                        Folio = null,
                        FormaPago = null,
                        Moneda = null,
                        TipoDeComprobante = "",
                        MetodoPago = null,
                        LugarExpedicion = ""
                    };

                    var rfc = await GetRFC(recibido.RFCReceptor);
                    if (rfc != null)
                    {
                        CFDI.RFCId = rfc.rowid;
                    }
                }
                else
                {

                var rutaXml = Environment.GetEnvironmentVariable("PathDescargaXML");
                var ruta = Path.Combine(rutaXml, $@"{recibido.FolioFiscal}.xml");
                try
                        {
                            var xml = File.ReadAllText(ruta);
                            CFDI = EntidadRepositorio(xml);
                            CFDI.Emitido = false;
                            var rfc = await GetRFC(recibido.RFCReceptor);
                            if (rfc != null)
                            {
                                CFDI.RFCId = rfc.rowid;
                            }
                        }
                       catch { return CFDI; }
                    

                }
            }
            await updateCfdi(CFDI); 
            return CFDI;
        
    }



    public async Task<CFDI> ProcesarCancelado(string canceladoJson)
    {
        var cancelado = JsonSerializer.Deserialize<CfdiCancelado>(canceladoJson);
        var CFDI = await _db.CFDIs.FirstOrDefaultAsync(_ => _.UUID == cancelado.FolioFiscal);
        return CFDI;
    }
    public async Task updateCfdi(CFDI cfdi)
    {
            var existeCFDI = await _db.CFDIs.AnyAsync(_ => _.UUID == cfdi.UUID);
            if (!existeCFDI)
            {
                 _db.CFDIs.Update(cfdi);
            }
    }

    public async Task<RFC?> GetRFC(string rfc)
    {
        return await _db.Rfc.FirstOrDefaultAsync(_ => _.Rfc == rfc);
    }

    public async Task verificarRFC(string rfc,string nombre)
    {
        var rfcExiste = await _db.Rfc.AnyAsync(_ => _.Rfc==rfc);
        if (!rfcExiste)
        {
            await _db.Rfc.AddAsync(new RFC() {rowid=0,Rfc=rfc,Nombre=nombre });
        }
      
    }
    public async Task guardarCfdiUiEmitido(CfdiEmitido emitido)
    {
        var ExisteCfdi = await _db.cfdisUi.AnyAsync(_ => _.UUID == emitido.FolioFiscal);
        if (!ExisteCfdi)
        {
            var cfdi = new CfdiUi()
            {
                UUID = emitido.FolioFiscal,
                Tipo = emitido.EstadoComprobante == "Cancelado" ? TipoComprobante.EmitidoCancelado : TipoComprobante.Emitido,
                RFCEmisor = emitido.RFCEmisor,
                NombreEmisor = emitido.NombreEmisor,
                RFCReceptor = emitido.RFCReceptor,
                NombreReceptor = emitido.NombreReceptor,
                FechaEmisión = emitido.FechaEmisión,
                FechaCertificacion = emitido.FechaCertificacion,
                PacCertifico = emitido.PacCertifico,
                Total = emitido.Total,
                EfectoComprobante = emitido.EfectoComprobante,
                EstatusCancelacion = emitido.EstatusCancelacion,
                EstadoComprobante = emitido.EstadoComprobante,
                StatusProcesoCancelacion = emitido.StatusProcesoCancelacion,
                FechaProcesoCancelacion = emitido.FechaProcesoCancelacion,
                Motivo = emitido.Motivo,
                FolioSustitución = emitido.FolioSustitución,
                UrlDescarga=emitido.UrlDescarga
            };
            await _db.cfdisUi.AddAsync(cfdi);
            
        }
      
    }

   public async Task guardarCfdiUiRecibido(CfdiRecibido recibido)
    {
        var ExisteCfdi = await _db.cfdisUi.AnyAsync(_ => _.UUID == recibido.FolioFiscal);
        if (!ExisteCfdi)
        {
            var cfdi = new CfdiUi()
            {
                UUID = recibido.FolioFiscal,
                Tipo = recibido.EstadoComprobante == "Cancelado" ? TipoComprobante.EmitidoCancelado : TipoComprobante.Emitido,
                RFCEmisor = recibido.RFCEmisor,
                NombreEmisor = recibido.NombreEmisor,
                RFCReceptor = recibido.RFCReceptor,
                NombreReceptor = recibido.NombreReceptor,
                FechaEmisión = recibido.FechaEmisión,
                FechaCertificacion = recibido.FechaCertificacion,
                PacCertifico = recibido.PacCertifico,
                Total = recibido.Total,
                EfectoComprobante = recibido.EfectoComprobante,
                EstatusCancelacion = recibido.EstatusCancelacion,
                EstadoComprobante = recibido.EstadoComprobante,
                StatusProcesoCancelacion = recibido.StatusProcesoCancelacion,
                FechaProcesoCancelacion = recibido.FechaProcesoCancelacion,
                UrlDescarga=recibido.UrlDescarga
            };
            await _db.cfdisUi.AddAsync(cfdi);
           
        }
    }
    public async Task SaveChanges()
    {
        await _db.SaveChangesAsync();
    }

}
