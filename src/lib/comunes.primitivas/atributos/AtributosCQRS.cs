namespace comunes.primitivas.atributos;


/// <summary>
/// Atributo que indica que la clase es utilziada como entidad para persisntencia en la base de datos
/// </summary>
[AttributeUsage( AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class EntidadDBAttribute: Attribute
{
}


/// <summary>
/// Atributo que indica que la clase es utilizada para crear instancias de un tipo de datos 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CQRSCrearAttribute : Attribute 
{
}


/// <summary>
/// Atributo que indica que la clase es utilizada para actualizar instancias de un tipo de datos 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CQRSActualizarAttribute : Attribute
{
}


/// <summary>
/// Atributo que indica que la clase es utilizada para obtener instancias de un tipo de datos 
/// </summary>
[AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
public class CQRSConsultaAttribute : Attribute
{
}