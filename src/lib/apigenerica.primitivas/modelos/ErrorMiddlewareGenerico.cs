namespace apigenerica.primitivas.modelos;
public class ErrorMiddlewareGenerico
{
    /// <summary>
    /// El servicio para la entidad genérica no fue localizado en la lista de interfaces IServicioEntidadAPI
    /// </summary>
    public const string ERROR_SERVICIO_NO_LOCALIZADO = "ERROR_SERVICIO_NO_LOCALIZADO";

    /// <summary>
    /// El ensamblado que contiene el servicio genérico no fue localizado
    /// </summary>
    public const string ERROR_ENSAMBLADO_NO_LOCALIZADO = "ERROR_ENSAMBLADO_NO_LOCALIZADO";


    /// <summary>
    /// El servicio require autenticación pero no hay un token de barrera presente
    /// </summary>
    public const string ERROR_SIN_AUTENTICACION_BEARER = "ERROR_SIN_AUTENTICACION_BEARER";

    public string? Entidad { get; set; }
    public int HttpCode { get; set; }
    public string? Error { get; set; }
}
