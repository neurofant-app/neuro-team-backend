namespace extensibilidad.metadatos.atributos;


/// <summary>
/// Determina si una propiedad debe ocultarse al sitema de obtención metadatos 
/// seutiliza principalmete cuando su valor no se obtiene de la intedacción con el usuario en la UI
/// </summary>
[AttributeUsage(AttributeTargets.Property, AllowMultiple = false, Inherited = false)]
public class ProtegidoAttribute : Attribute
{
    public ProtegidoAttribute()
    {
    }
}

