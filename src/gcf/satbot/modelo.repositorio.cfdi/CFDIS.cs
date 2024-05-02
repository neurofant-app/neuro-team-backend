using System.Text.Json.Serialization;

namespace modelo.repositorio.cfdi;
public class CFDI
{
    /// <summary>
    /// Identificador único del registro en la tabla CFDI
    /// </summary>
    public long rowid { get; set; }

    /// <summary>
    /// Identificador único del CFDI
    /// </summary>
    public string UUID { get; set; }  
    /// <summary>
    /// Version CFDI
    /// </summary>
    public string Version { get; set; }  
    /// <summary>
    /// Indica si el comprobante ha sido emitido cuando el valro es true o      
    /// recibido cuando es false
    /// </summary>
    public bool Emitido { get; set; }   
    /// <summary>
    /// Indica si el comprobante ha sido cancelado
    /// </summary>
    public bool Cancelado { get; set; } 
    /// <summary> 
    /// Fecha de emisión del comprobante convertida a Ticks
    /// </summary>
    public long FechaCFDI { get; set; }  
    /// <summary>
    /// almacenará sólo el año de la fecha del CFDI
    /// </summary>
    public int iano { get; set; }  
    /// <summary>
    /// almacenará sólo el mes de la fecha del CFDI
    /// </summary>
    public int imes { get; set; }  
    /// <summary>
    /// almacenará sólo el dia de la fecha del CFDI
    /// </summary>
    public int idia { get; set; } 

    /// <summary>
    /// RFC del emisor en caso Emitido = False o del Receptor cuando Emitido = true
    /// </summary>
    public long RFCId { get; set; }  

    /// <summary>
    /// Subtotal del CFDI
    /// </summary>
    public decimal SubTotal { get; set; }  
    /// <summary>
    /// Total del CFDI
    /// </summary>
    public decimal Total { get; set; } 
    /// <summary>
    /// Total Impuestos Retenidos
    /// </summary>
    public decimal TotalIRetenidos { get; set; } 
    /// <summary>
    /// Total Impuestos Trasladados
    /// </summary>
    public decimal TotalITrasladados { get; set; }  
    /// <summary>
    /// Uso del CFDI si existe
    /// </summary>
    public string? Uso { get; set; }
    /// <summary>
    /// Indica si el comprobante tiene impuestos retenidos
    /// </summary>
    public bool TieneIRetenidos { get; set; }  
    /// <summary>
    /// Indica si el comprobante tiene impuestos trasladados
    /// </summary>
    public bool TieneITrasladados { get; set; }  
    /// <summary>
    /// Indica si el comprobante tiene CFDI relacionados
    /// </summary>
    public bool TieneRelacionados { get; set; }   
    /// <summary>
    /// Indica si el comprobante tiene Impouestos a cuenat de 3os
    /// </summary>
    public bool TieneI3os { get; set; }    
    /// <summary>
    /// Indica si el comprobante tiene información aduanera
    /// </summary>
    public bool TieneInfoAduanera { get; set; }    
    /// <summary>
    /// Indica si el comprobante tiene Cuenta predial
    /// </summary>
    public bool TieneCPredial { get; set; }    
    /// <summary>
    /// Indica si el comprobante tiene Complementos
    /// </summary>
    public bool TieneComplementos { get; set; }    
    /// <summary>
    /// Indica si el comprobante Tiene Adenda
    /// </summary>
    public bool TieneAddenda { get; set; }    
    /// <summary>
    /// Atributo opcional para precisar la serie para control interno del contribuyente.
    /// Este atributo acepta una cadena de caracteres.
    /// </summary>
    public string? Serie { get; set; }
    /// <summary>
    /// Atributo opcional para control interno del contribuyente que expresa el folio 
    /// del comprobante, acepta una cadena de caracteres.
    /// </summary>
    public string? Folio { get; set; }  
    /// <summary>
    /// Atributo condicional para expresar la clave de la forma de pago de los bienes o 
    /// servicios amparados por el comprobante.
    /// </summary>
    public string? FormaPago { get; set; }   

    /// <summary>
    /// Atributo requerido para identificar la clave de la moneda utilizada para expresar 
    /// los montos, cuando se usa moneda nacional se registra MXN.Conforme con la especificación 
    /// ISO 4217.
    /// </summary>
    public string? Moneda { get; set; }

    /// <summary>
    /// Atributo requerido para expresar la clave del efecto del comprobante fiscal para el 
    /// contribuyente emisor.
    /// </summary>
    public string TipoDeComprobante { get; set; }  

    /// <summary>
    /// Atributo condicional para precisar la clave del método de pago que aplica para 
    /// este comprobante fiscal digital por Internet, conforme al Artículo 29-A fracción 
    /// VII incisos a y b del CFF.
    /// </summary>
    public string? MetodoPago { get; set; } 

    /// <summary>
    /// Atributo requerido para incorporar el código postal del lugar de expedición del 
    /// comprobante (domicilio de la matriz o de la sucursal).
    /// </summary>
    public string LugarExpedicion { get; set; } 


    [JsonIgnore]
    public RFC RFC { get; set; }

    //public List<Retencion> Retenciones { get; set; }
    //public List<Traslado> Traslados { get; set; }

    
}
