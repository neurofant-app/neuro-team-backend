namespace disenocurricular.model;
/// <summary>
/// Estado de publicación del temario
/// </summary>
public enum EstadoTemario
{
    /// <summary>
    /// El temario si ecuentra en edición
    /// </summary>
    EnEdicion = 0,
    /// <summary>
    /// El temario ha sido publicado y no puede ser modificado
    /// </summary>
    Publicado = 1,
    /// <summary>
    /// El temario ha sido marcado como obsoleto y no debe utilizarse
    /// </summary>
    Obsoleto = 2
}
