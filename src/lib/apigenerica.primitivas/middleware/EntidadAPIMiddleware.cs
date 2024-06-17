﻿using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.primitivas.aplicacion;
using apigenerica.primitivas.modelos;
using apigenerica.primitivas.seguridad;
using comunes.primitivas.atributos;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;
using System.Reflection;
using System.Text;
using System.Text.Json;

namespace apigenerica.primitivas;

#pragma warning disable CS8602 // Dereference of a possibly null reference.

/// <summary>
/// Middleware para la inyección de servicio de entidad y catálogo genéricos
/// </summary>
public class EntidadAPIMiddleware
{

    public const string GenericAPIServiceKey = "GENERICAPISERVICE";
    public const string GenericCatalogAPIServiceKey = "GENERICCATALOGAPISERVICE";

    private readonly RequestDelegate _next;
    private readonly IConfiguracionAPIEntidades _configuracionAPI;
    private readonly ILogger<EntidadAPIMiddleware> _logger;
    private readonly IProveedorAplicaciones _proveedorAplicaciones;
    private readonly ICacheSeguridad _cacheSeguridad;
    private readonly ICacheAtributos _cacheAtributos;

    public EntidadAPIMiddleware(RequestDelegate next, IConfiguracionAPIEntidades configuracionAPI, ILogger<EntidadAPIMiddleware> logger,
        IProveedorAplicaciones proveedorAplicaciones, ICacheSeguridad cacheSeguridad, ICacheAtributos cacheAtributos )
    {
        _next = next;
        _configuracionAPI = configuracionAPI;
        _logger = logger;
        _proveedorAplicaciones = proveedorAplicaciones;
        _cacheSeguridad = cacheSeguridad;
        this._cacheAtributos = cacheAtributos;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        var controllerName = context.GetRouteData().Values["controller"];
        if (controllerName != null)
        {
            switch (controllerName)
            {
                case "CatalogoGenerico":
                    await ProcesaCatalogoGenerico(context);
                    break;

                case "EntidadGenerica":
                    await ProcesaEntidadGenerica(context);
                    break;
                case "EntidadGenericaHijo":
                    await ProcesaEntidadHijoGenerica(context);
                    break;

            }
        }


        // Call the next delegate/middleware in the pipeline.
        await _next(context);
    }


    /// <summary>
    /// Realiza el procesamiento de un endpoint atendido por un servicio de catálogo genérico
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task ProcesaCatalogoGenerico(HttpContext context)
    {
        if(context.GetRouteData().Values["entidad"] == null )
        {
            return;
        }

        string entidad = context.GetRouteData().Values["entidad"].ToString() ?? "";
        var servicios = _configuracionAPI.ObtienesServiciosICatalogoEntidadAPI();
        var servicio = servicios.FirstOrDefault(x => x.NombreRuteo.Equals(entidad, StringComparison.InvariantCultureIgnoreCase));

        if (servicio == null)
        {
            await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
            {
                Entidad = entidad,
                Error = ErrorMiddlewareGenerico.ERROR_SERVICIO_NO_LOCALIZADO,
                HttpCode = 400
            });
        }

        var assembly = Assembly.LoadFrom(servicio.Ruta);
        var tt = assembly.GetType(servicio.NombreEnsamblado);

        if (tt == null)
        {
            await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
            {
                Entidad = entidad,
                Error = ErrorMiddlewareGenerico.ERROR_ENSAMBLADO_NO_LOCALIZADO,
                HttpCode = 400
            });
        }

        var ctors = tt.GetConstructors();
        var ps = ctors[0].GetParameters();
        object[] paramArray = new object[ps.Length];
        int i = 0;
        foreach (var p in ps)
        {
            var s = context.RequestServices.GetService(p.ParameterType);
            if (s != null)
            {
                paramArray[i] = s;
            }
            i++;
        }

        try
        {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            var service = (IServicioCatalogoAPI)Activator.CreateInstance(tt, paramArray);
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            if (service != null)
            {
                var contexto = context.ObtieneContextoUsuario();
                contexto = await AdicionaSeguridad(contexto);
                contexto = await AdicionaAtributosMetodo(contexto, tt);
#if !DEBUG
                if (service.RequiereAutenticacion)
                {
                    if (string.IsNullOrEmpty(contexto.UsuarioId))
                    {
                        await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
                        {
                            Entidad = entidad,
                            Error = ErrorMiddlewareGenerico.ERROR_SIN_AUTENTICACION_BEARER,
                            HttpCode = 401
                        });
                    }
                }
#endif

                service.EstableceContextoUsuarioAPI(contexto);
                context.Request.HttpContext.Items.Add(GenericCatalogAPIServiceKey, service);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Realiza el procesamiento de un endpoint atendido por un servicio de entidad genérica
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task ProcesaEntidadGenerica(HttpContext context)
    {
        if (context.GetRouteData().Values["entidad"] == null)
        {
            return;
        }

        string entidad = context.GetRouteData().Values["entidad"].ToString() ?? "";
        var servicios = _configuracionAPI.ObtienesServiciosIEntidadAPI();
        var servicio = servicios.FirstOrDefault(x => x.NombreRuteo.Equals(entidad, StringComparison.InvariantCultureIgnoreCase));

        if (servicio == null)
        {
            await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
            {
                Entidad = entidad,
                Error = ErrorMiddlewareGenerico.ERROR_SERVICIO_NO_LOCALIZADO,
                HttpCode = 400
            });
        }

        var assembly = Assembly.LoadFrom(servicio.Ruta);
        var tt = assembly.GetType(servicio.NombreEnsamblado);

        if (tt == null)
        {
            await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
            {
                Entidad = entidad,
                Error = ErrorMiddlewareGenerico.ERROR_ENSAMBLADO_NO_LOCALIZADO,
                HttpCode = 400
            });
        }


        var ctors = tt.GetConstructors();
        var ps = ctors[0].GetParameters();
        object[] paramArray = new object[ps.Length];
        int i = 0;
        foreach (var p in ps)
        {
            var s = context.RequestServices.GetService(p.ParameterType);
            if (s != null)
            {
                paramArray[i] = s;
            }
            i++;
        }
        
        try
        {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            var service = (IServicioEntidadAPI)Activator.CreateInstance(tt, paramArray);
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            if (service != null)
            {
                var contexto = context.ObtieneContextoUsuario();
                contexto = await AdicionaSeguridad(contexto);
                contexto = await AdicionaAtributosMetodo(contexto,tt);
#if !DEBUG
                if (service.RequiereAutenticacion)
                {
                    if (string.IsNullOrEmpty(contexto.UsuarioId))
                    {
                        await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
                        {
                            Entidad = entidad,
                            Error = ErrorMiddlewareGenerico.ERROR_SIN_AUTENTICACION_BEARER,
                            HttpCode = 401
                        });
                    }
                }
#endif
                service.EstableceContextoUsuarioAPI(contexto);
                context.Request.HttpContext.Items.Add(GenericAPIServiceKey, service);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.ToString());
            throw;
        }
    }

    /// <summary>
    /// Realiza el procesamiento de un endpoint atendido por un servicio de entidad Hijo genérica
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    private async Task ProcesaEntidadHijoGenerica(HttpContext context)
    {
        if (context.GetRouteData().Values["entidad"] == null)
        {
            return;
        }

        string entidad = context.GetRouteData().Values["entidad"].ToString() ?? "";
        var servicios = _configuracionAPI.ObtienesServiciosIEntidadHijoAPI();
        var servicio = servicios.FirstOrDefault(x => x.NombreRuteo.Equals(entidad, StringComparison.InvariantCultureIgnoreCase));

        if (servicio == null)
        {
            await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
            {
                Entidad = entidad,
                Error = ErrorMiddlewareGenerico.ERROR_SERVICIO_NO_LOCALIZADO,
                HttpCode = 400
            });
        }

        var assembly = Assembly.LoadFrom(servicio.Ruta);
        var tt = assembly.GetType(servicio.NombreEnsamblado);

        if (tt == null)
        {
            await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
            {
                Entidad = entidad,
                Error = ErrorMiddlewareGenerico.ERROR_ENSAMBLADO_NO_LOCALIZADO,
                HttpCode = 400
            });
        }


        var ctors = tt.GetConstructors();
        var ps = ctors[0].GetParameters();
        object[] paramArray = new object[ps.Length];
        int i = 0;
        foreach (var p in ps)
        {
            var s = context.RequestServices.GetService(p.ParameterType);
            if (s != null)
            {
                paramArray[i] = s;
            }
            i++;
        }

        try
        {
#pragma warning disable CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            var service = (IServicioEntidadHijoAPI)Activator.CreateInstance(tt, paramArray);
#pragma warning restore CS8600 // Se va a convertir un literal nulo o un posible valor nulo en un tipo que no acepta valores NULL
            if (service != null)
            {
                var contexto = context.ObtieneContextoUsuario();
                contexto = await AdicionaSeguridad(contexto);
                contexto = await AdicionaAtributosMetodo(contexto, tt);

#if !DEBUG
                if (service.RequiereAutenticacion)
                {
                    if (string.IsNullOrEmpty(contexto.UsuarioId))
                    {
                        await ReturnMiddlewareError(context, new ErrorMiddlewareGenerico()
                        {
                            Entidad = entidad,
                            Error = ErrorMiddlewareGenerico.ERROR_SIN_AUTENTICACION_BEARER,
                            HttpCode = 401
                        });
                    }
                }
#endif
                service.EstableceContextoUsuarioAPI(contexto);
                context.Request.HttpContext.Items.Add(GenericAPIServiceKey, service);
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(message: ex.ToString());
            throw;
        }
    }



    /// <summary>
    /// Devulve un error en el pipeline para exepciones de entidades genéricas
    /// </summary>
    /// <param name="context"></param>
    /// <param name="error"></param>
    /// <returns></returns>
    private static async Task ReturnMiddlewareError(HttpContext context, ErrorMiddlewareGenerico error)
    {
        var bytes = Encoding.UTF8.GetBytes(JsonSerializer.Serialize(error));
        await context.Response.Body.WriteAsync(bytes, 0, bytes.Length);
        context.Response.ContentType = "application/json";
        context.Response.StatusCode = error.HttpCode;
        await context.Response.StartAsync();
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contexto"></param>
    /// <returns></returns>
    private async Task<ContextoUsuario> AdicionaSeguridad(ContextoUsuario contexto) {

        
        var aplicacion = await _proveedorAplicaciones.ObtieneApliaciones();
            var roles = await _cacheSeguridad.RolesUsuario(aplicacion.First().ApplicacionId.ToString(), contexto.UsuarioId, contexto.DominioId, contexto.UOrgId);
            var permisos = await _cacheSeguridad.PermisosUsuario(aplicacion.First().ApplicacionId.ToString(), contexto.UsuarioId, contexto.DominioId, contexto.UOrgId);
            contexto.RolesAplicacion = roles.Select(_ => _.RolId).ToList();
            contexto.PermisosAplicacion = permisos.Select(_ => _.PermisoId).ToList();
        return contexto;
    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="contexto"></param>
    /// <returns></returns>
    private async Task<ContextoUsuario> AdicionaAtributosMetodo(ContextoUsuario contexto, Type tipoServicio)
    {
      contexto.AtributosMetodos= await _cacheAtributos.AtributosServicio(tipoServicio);
       
        return contexto;
    }

}
#pragma warning restore CS8602 // Dereference of a possibly null reference.