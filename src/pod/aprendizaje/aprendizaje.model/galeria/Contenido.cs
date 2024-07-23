using aprendizaje.model.comunes;
using System.Diagnostics.CodeAnalysis;

namespace aprendizaje.model.galeria;

/// <summary>
/// Describe un elemtno de contenido de la galeria
/// </summary>
[ExcludeFromCodeCoverage]
public class Contenido
{
    /// <summary>
    /// Identificador unico de contenido
    /// </summary>
    public Guid Id { get; set; }

    /// <summary>
    /// Nombre para el despliegue del elemento
    /// </summary>
    public required string Nombre { get; set; }

    /// <summary>
    /// Tipo del contenido de aprendizaje
    /// </summary>
    public TipoContenido Tipo { get; set; }

    /// <summary>
    /// Imágen en minuarura para mostrar en la galería, so botiene automáticamente 
    /// con al actualizar crear o actualizar el contenido de imágenes y videos,
    /// para los archivos de audio se utiliza un icono estándar y la propiedad es nula
    /// </summary>
    public string? Miniatura { get; set; }

    /// <summary>
    /// Fecha de creación o actualizaicón del contenido
    /// </summary>
    public DateTime Fecha { get; set; } = DateTime.UtcNow;

    /// <summary>
    /// Anexos del contenido
    /// </summary>
    public List<ValorI18N<Anexo>> Anexos { get; set; } = [];

    /// <summary>
    /// Temas en los que se incluye el contenido
    /// </summary>
    public List<int> Temas { get; set; } = [];

    /// <summary>
    /// Determina si el contenido ha sido marcado como eliminado, el contenido 
    /// no se borra del bucket para evitar que se pierda la referencia en neuronas exisntentes
    /// </summary>
    public bool Eliminado { get; set; }

    /// <summary>
    /// Total de bytes utilizados para el almacenamiento del contenido y todo sus anexos
    /// </summary>
    public long BytesTotales { get; set; } = 0;
}
