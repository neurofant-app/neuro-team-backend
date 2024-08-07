using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.comunes;

/// <summary>
/// Describe un contenido de aprendizaje vinculado a un archivo o arreglo de bytes
/// </summary>
[ExcludeFromCodeCoverage]
public class ContenidoAnexo: ValorI18N<string>
{

    /// <summary>
    /// Tipo del contenido de aprendizaje
    /// </summary>
    public TipoContenido Tipo { get; set; }

}
