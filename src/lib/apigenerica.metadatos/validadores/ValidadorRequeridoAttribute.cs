namespace extensibilidad.metadatos.validadores;

public enum RequeridaOperacion
{
    Ninguna = 0,
    Insertar = 1,
    Actualizar = 4,
    Elimininar = 8
}

/// <summary>
/// Especifica si una propiedad es requerida 
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = true, Inherited = false)]
public class ValidarRequeridaAttribute(RequeridaOperacion requerida = RequeridaOperacion.Ninguna) : Attribute
{
    private readonly RequeridaOperacion _requerida = requerida;

    /// <summary>
    /// DEterina apra que métodos una operación es requerida
    /// </summary>
    public virtual RequeridaOperacion Requerida
    {
        get { return _requerida; }
    }
}
