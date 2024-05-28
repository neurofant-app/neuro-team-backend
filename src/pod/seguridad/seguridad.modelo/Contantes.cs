namespace seguridad.modelo;


/// <summary>
/// Define el ámbito de asignación del permiso
/// </summary>
[Flags]
public enum AmbitoPermiso {

    /// <summary>
    /// El permiso sólo aplica globalmente
    /// </summary>
    Global = 1,
    /// <summary>
    /// El permiso puede localizarse a un contexto, por ejemplo el Id de un objeto
    /// </summary>
    Contextual = 2
    
}
