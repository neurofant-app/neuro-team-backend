using apigenerica.model.modelos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace apigenerica.primitivas;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
/// <summary>
/// Controlador base para las entidades genéricas
/// </summary>
public abstract class ControladorBaseGenerico : ControllerBase
{
    /// <summary>
    /// Identificador par el encabezado de identficación de dominio
    /// </summary>
    protected const string DOMINIOHEADER = "x-d-id";

    /// <summary>
    /// Identificador par el encabezado de identficación de unidad organizacional
    /// </summary>
    protected const string UORGHEADER = "x-uo-id";

    /// <summary>
    /// Identificador par el encabezado de detección de idioma
    /// </summary>
    protected const string IDIOMAHEADER = "Accept-Language";


    protected readonly IHttpContextAccessor _httpContextAccessor;

    public ControladorBaseGenerico(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }


    /// <summary>
    /// Devuelve el valor del dominio actual para el request
    /// </summary>
    /// <returns></returns>
    protected virtual string? DominioId()
    {

        return _httpContextAccessor.HttpContext.Request.Headers?[DOMINIOHEADER];
    }


    /// <summary>
    /// Devuelve el valor de la unidad organizaciona para el request
    /// </summary>
    /// <returns></returns>
    protected virtual string? UnidadOrgId()
    {

        return _httpContextAccessor.HttpContext.Request.Headers?[UORGHEADER];
    }

    /// <summary>
    /// Devuelve el identificador del usuario en sesion si existe un JWT válido
    /// </summary>
    /// <returns></returns>
    protected virtual string? UsuarioId()
    {
        ContextoUsuario? ContextoUsuario = _httpContextAccessor.HttpContext.Features.Get<ContextoUsuario>();
        return ContextoUsuario?.UsuarioId;
    }

    /// <summary>
    /// Deveulve el valor del idioa solicitado por el request
    /// </summary>
    /// <returns></returns>
    protected virtual string? Idioma()
    {
        return _httpContextAccessor.HttpContext.Request.Headers?[IDIOMAHEADER];
    }

}
#pragma warning restore CS8602 // Dereference of a possibly null reference.