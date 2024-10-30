using controlescolar.modelo.comunes;
using System.Diagnostics.CodeAnalysis;

namespace controlescolar.modelo.documentacion;

/// <summary>
/// Define un documento perteneciente a un expediente
/// </summary>
[ExcludeFromCodeCoverage]
public class ActualizaDocumentoBase
{
    /// <summary>
    /// Nombre asigando al documento
    /// </summary>
    public required List<ValorI18N<string>> Nombre { get; set; } = [];


    /// <summary>
    /// Determina si el documento es permanente o tiene una caducidad
    /// </summary>
    public TipoCaducidadDocumento Caducidad { get; set; }

    /// <summary>
    /// Ámbito de aplicación del documento
    /// </summary>
    public TipoAmbitoDocumento Ambito { get; set; }

    /// <summary>
    /// En el caso de que el ámbito sea de plantel especifica el plantel al que aplica el documento
    /// </summary>
    public long? PlantelId { get; set; }

    /// <summary>
    /// Determina si un documento es opcional
    /// </summary>
    public bool Opcional { get; set; }

}
