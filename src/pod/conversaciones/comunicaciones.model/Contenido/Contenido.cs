
namespace comunicaciones.model;

public class Contenido
{
    public TipoCanal Canal { get; set; }
    public string Cuerpo { get; set; }
    public string? Encabezado { get; set; }
    public string? Idioma { get; set; }
}
