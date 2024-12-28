#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using organizacion.model.usuariodominio;
using organizacion.services.dbcontext;
using System.Collections.Specialized;
using System.Text.Json;

namespace organizacion.services.usuariodominio.elementoDominio;
[ServicioEntidadAPI(typeof(UsuarioDominio))]
public class ServicioUsuarioDominio : ServicioEntidadGenericaBase<UsuarioDominio, ElementoDominioInsertar, ElementoDominioActualizar, UsuarioDominio, Guid>, IServicioEntidadAPI, IServicioUsuarioDominio
{
    private readonly ILogger<ServicioUsuarioDominio>  _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IDistributedCache _cache;

    public ServicioUsuarioDominio(ILogger<ServicioUsuarioDominio> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        _cache = cache;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextOrganizacion.NOMBRE_COLECCION_USUARIODOMINIOS);

        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextOrganizacion.NOMBRE_COLECCION_USUARIODOMINIOS}'";
            _logger.LogError(err);
            throw new Exception(err);
        }

        try
        {
            _logger.LogDebug($"Mongo DB {configuracionEntidad.Esquema} coleccioón {configuracionEntidad.Esquema} utilizando conexión default {string.IsNullOrEmpty(configuracionEntidad.Conexion)}");
            var cadenaConexion = string.IsNullOrEmpty(configuracionEntidad.Conexion) && string.IsNullOrEmpty(configuracionMongo.ConexionDefault())
                ? configuracionMongo.ConexionDefault()
                : string.IsNullOrEmpty(configuracionEntidad.Conexion)
                    ? configuracionMongo.ConexionDefault()
                    : configuracionEntidad.Conexion;
            var client = new MongoClient(cadenaConexion);


            _db = MongoDbContextOrganizacion.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextOrganizacion)_db).UsuarioDominios;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicialzar mongo para '{MongoDbContextOrganizacion.NOMBRE_COLECCION_USUARIODOMINIOS}'");
            throw;
        }

    }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioDominio-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ElementoDominioActualizar>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioUsuarioDominio-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioUsuarioDominio-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros, forzarEliminacion);
        _logger.LogDebug("ServicioUsuarioDominio-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioDominio-InsertarAPI-{data}", data);
        var add = data.Deserialize<ElementoDominioInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUsuarioDominio-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioDominio-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUsuarioDominio-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioDominio-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUsuarioDominio-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioDominio-UnicaPorIdAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.ORGANIZACION_USUARIODOMINIO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioUsuarioDominio-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUsuarioDominio-UnicaPorIdDespliegueAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.ORGANIZACION_USUARIODOMINIO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioUsuarioDominio-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioUsuarioDominio-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioUsuarioDominio-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioUsuarioDominio-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioUsuarioDominio-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioUsuarioDominio-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioUsuarioDominio-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }



    #region Overrides para la entidad UsuarioDominio

    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, ElementoDominioActualizar actualizacion, UsuarioDominio original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, UsuarioDominio original, bool forzarEliminacion = false)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarInsertar(ElementoDominioInsertar data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override UsuarioDominio ADTOFull(ElementoDominioInsertar data)
    {
        var usuarioDominio = new UsuarioDominio(){Id = data.UsurioId};
        return usuarioDominio;
    }


    public UsuarioDominio ADTOFull(ElementoDominioActualizar actualizacion, UsuarioDominio actual)
    {
        actual.Id = actualizacion.UsuarioId;
        return actual;
    }


    public override UsuarioDominio ADTODespliegue(UsuarioDominio data)
    {
        return new UsuarioDominio()
        {
            Id = data.Id,
            Dominios = data.Dominios,
            DominiosId = data.DominiosId
        };
    }


    public override async Task<RespuestaPayload<UsuarioDominio>> Insertar(ElementoDominioInsertar data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<UsuarioDominio>();
        try
        {
            var usuarioDominio = _dbSetFull.Find(data.UsurioId);
            if (usuarioDominio == null)
            {
                var entidad = ADTOFull(data);
                entidad.DominiosId.Add(Guid.Parse(parametros["n0Id"]));
                entidad.Dominios.Add(new ElementoDominio() { DominioId = Guid.Parse(parametros["n0Id"]), Activo = (bool)data.Activo});
                _dbSetFull.Add(entidad);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);

            }
            else
            {
                var existeDominio = usuarioDominio.DominiosId.FirstOrDefault(_ => _ == Guid.Parse(parametros["n0Id"]));

                if(existeDominio != Guid.Empty)
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.ORGANIZACION_USUARIODOMINIO_EXISTENTE_EN_LISTA,
                        Mensaje = "Se ha encontrado un dominio en la lista, ingrese otro dominio para realiacionar al usuario",
                        HttpCode = HttpCode.Conflict
                    };
                    return respuesta;
                }

                usuarioDominio.DominiosId.Add(Guid.Parse(parametros["n0Id"]));
                usuarioDominio.Dominios.Add(new ElementoDominio() { DominioId = Guid.Parse(parametros["n0Id"]), Activo = (bool)data.Activo });
                _dbSetFull.Update(usuarioDominio);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(usuarioDominio);

            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUsuarioDominio-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public async Task<Respuesta> Actualizar(Guid id, ElementoDominioActualizar data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if(id == (Guid.Empty) || data == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_USUARIODOMINIO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No se ha proporcionado Id o Payload",
                    HttpCode = HttpCode.BadRequest
                };
            }
            UsuarioDominio actual = _dbSetFull.Find(id);
            if(actual == null)
            {
                return await Insertar(new ElementoDominioInsertar() { UsurioId = data.UsuarioId, Activo = data.Activo}, parametros);
            }

            var resultadoValidacion = await ValidarActualizar(id, data, actual);

            if(resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var elementoDominio = entidad.Dominios.FirstOrDefault(x => x.DominioId == Guid.Parse(parametros["n0Id"]));
                elementoDominio.Activo = (bool)data.Activo;
                _dbSetFull.Update(entidad);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.ORGANIZACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUsuarioDominio-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<Respuesta> Eliminar(Guid id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        var respuesta = new Respuesta();
        try
        {
            if (id == Guid.Empty)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_USUARIODOMINIO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporciando el Id",
                    HttpCode = HttpCode.BadRequest,
                };
                return respuesta;
            }

            UsuarioDominio actual = _dbSetFull.Find(id);

            if(actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_USUARIODOMINIO_NO_ENCONTRADO,
                    Mensaje = "No existe un UsuarioDominoi con el Id proporcionado",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if(resultadoValidacion.Valido)
            {
                actual.Dominios.Remove(actual.Dominios.FirstOrDefault(x => x.DominioId == Guid.Parse(parametros["n0Id"])));
                actual.DominiosId.Remove(Guid.Parse(parametros["n0Id"]));
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.ORGANIZACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUsuarioDominio-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<UsuarioDominio>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(UsuarioDominio));
        var Elementos = Enumerable.Empty<UsuarioDominio>().AsQueryable();

        if (consulta.Filtros.Count > 0)
        {
            var predicateBody = interpreteConsulta.CrearConsultaExpresion<UsuarioDominio>(consulta, entidad);

            if (predicateBody != null)
            {
                var RConsulta = _dbSetFull.AsQueryable().Provider.CreateQuery<UsuarioDominio>(predicateBody.getWhereExpression(_dbSetFull.AsQueryable().Expression));

                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "UsuarioId", consulta.Paginado.Ordenamiento ??Ordenamiento.asc);
            }
        }
        else
        {
            var RConsulta = _dbSetFull.AsQueryable();
            Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "UsuarioId", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

        }
        return await Elementos.PaginadoAsync(consulta);
    }

    public Task<RespuestaPayload<UsuarioDominio>> UnicaPorId(Guid id, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public Task<RespuestaPayload<UsuarioDominio>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return