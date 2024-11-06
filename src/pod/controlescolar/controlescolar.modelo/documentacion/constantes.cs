namespace controlescolar.modelo.documentacion;

/// <summary>
/// Define la caducidad de un documento
/// </summary>
public enum TipoCaducidadDocumento
{
    /// <summary>
    /// El documento no caduca
    /// </summary>
    Permanente = 0,
    /// <summary>
    /// El documento caduca en cada siclo escolar
    /// </summary>
    Ciclo = 1 ,
    /// <summary>
    /// El documento caduca de manera periodica
    /// </summary>
    Periodo = 2
}

/// <summary>
/// Define el ámbito de un documento
/// </summary>
public enum TipoAmbitoDocumento
{
    /// <summary>
    /// El documento es global para todos los planteles de una escuela
    /// </summary>
    Escuela = 0,
    /// <summary>
    /// El documento es local a un plantel
    /// </summary>
    Plantel = 1,

}
