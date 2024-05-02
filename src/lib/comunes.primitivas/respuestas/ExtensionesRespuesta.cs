using System.Diagnostics.CodeAnalysis;

namespace comunes.primitivas;

[ExcludeFromCodeCoverage]
public static class ExtensionesRespuesta
{
    /// <summary>
    /// Genera un errror de poceso inexistente 404 para la propieda de la entidad
    /// </summary>
    /// <param name="propiedad"></param>
    /// <returns></returns>
    public static ErrorProceso ErrorProcesoNoEncontrado(this string propiedad)
    {
        return new ErrorProceso()
        {
            HttpCode = HttpCode.NotFound,
            Codigo = "INEXISTENTE",
            Propiedad = propiedad,
            Mensaje = "El elemento no fue localizado"
        };
    }

    /// <summary>
    /// Genera un errror de poceso duplicado 409 para la propieda de la entidad
    /// </summary>
    /// <param name="propiedad"></param>
    /// <returns></returns>
    public static ErrorProceso ErrorProcesoDuplicado(this string propiedad)
    {
        return new ErrorProceso()
        {
            HttpCode = HttpCode.Conflict,
            Codigo = "DUPLICADO",
            Propiedad = propiedad,
            Mensaje = $"{propiedad} se encuentra duplicado en el dominio"
        };
    }


    public static ErrorProceso ErrorEntidadPadreNoConfigurada(this string propiedad)
    {
        return new ErrorProceso()
        {
            Codigo = "",
            HttpCode = HttpCode.BadRequest,
            Mensaje = $"No existe una regla para el padre {propiedad}",
            Propiedad = propiedad
        };
    }

    public static ErrorProceso Error409(this string propiedad)
    {
        return new ErrorProceso()
        {
            Codigo = "",
            HttpCode = HttpCode.Conflict,
            Mensaje = $"Error en la ejecucion ",
            Propiedad = propiedad
        };
    }
}
