#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using organizacion.model.dominio;
using organizacion.model.unidadorganizacional;
using organizacion.services.dbcontext;
using System.Collections.Specialized;
using System.Text.Json;

namespace organizacion.services.unidadorganizacional;
[ServicioEntidadAPI(typeof(UnidadOrganizacional))]
public class ServicioUnidadOrganizacional : ServicioEntidadGenericaBase<UnidadOrganizacional, UnidadOrganizacionalInsertar, UnidadOrganizacionalActualizar, UnidadOrganizacionalDespliegue, Guid>,
    IServicioEntidadAPI, IServicioUnidadOrganizacional
{
    private readonly ILogger<ServicioUnidadOrganizacional> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IDistributedCache _cache;
    private DbSet<Dominio> _dbSetDominio;
    public ServicioUnidadOrganizacional(ILogger<ServicioUnidadOrganizacional> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        _cache = cache;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextOrganizacion.NOMBRE_COLECCION_DOMINIOS);

        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextOrganizacion.NOMBRE_COLECCION_DOMINIOS}'";
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
            _dbSetDominio = ((MongoDbContextOrganizacion)_db).Dominios;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicialzar mongo para '{MongoDbContextOrganizacion.NOMBRE_COLECCION_DOMINIOS}'");
            throw;
        }

    }

    public bool RequiereAutenticacion => true;

    private MongoDbContextOrganizacion DB { get { return (MongoDbContextOrganizacion)_db; } }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-ActualizarAPI-{data}", data);
        var update = data.Deserialize<UnidadOrganizacionalActualizar>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioUnidadOrganizacional-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros);
        _logger.LogDebug("ServicioUnidadOrganizacional-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-InsertarAPI-{data}", data);
        var add = data.Deserialize<UnidadOrganizacionalInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUnidadOrganizacional-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUnidadOrganizacional-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUnidadOrganizacional-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUnidadOrganizacional-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioUnidadOrganizacional-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioUnidadOrganizacional-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    #region Overrides para la entidad UnidadOrganizacional
    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, UnidadOrganizacionalActualizar actualizacion, UnidadOrganizacional original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, UnidadOrganizacional original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarInsertar(UnidadOrganizacionalInsertar data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override UnidadOrganizacional ADTOFull(UnidadOrganizacionalInsertar data)
    {
        return new UnidadOrganizacional()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            Activa = true
        };
    }

    public override UnidadOrganizacional ADTOFull(UnidadOrganizacionalActualizar actualizacion, UnidadOrganizacional actual)
    {
        actual.Id = actualizacion.Id;
        actual.Nombre = actualizacion.Nombre;
        actual.Activa = actualizacion.Activa;
        return actual;
    }

    public override UnidadOrganizacionalDespliegue ADTODespliegue(UnidadOrganizacional data)
    {
        return new UnidadOrganizacionalDespliegue()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Activa = data.Activa
        };
    }

    public override async Task<RespuestaPayload<UnidadOrganizacionalDespliegue>> Insertar(UnidadOrganizacionalInsertar data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<UnidadOrganizacionalDespliegue>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if(resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                var dominio = _dbSetDominio.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(parametros["n0Id"])));
                dominio.UnidadesOrganizacionales.Add(entidad);
                _dbSetDominio.Update(dominio);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
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
            _logger.LogError(ex, "ServicioUnidadOrganizacional-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public async Task<Respuesta> Actualizar(Guid id, UnidadOrganizacionalActualizar data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (id == Guid.Empty || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_UNIDADORGANIZACIONAL_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest,
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            var dominio = _dbSetDominio.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(parametros["n0Id"])));
            UnidadOrganizacional actual = dominio.UnidadesOrganizacionales.FirstOrDefault(_ => _.Id.Equals(id));
            if(actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_UNIDADORGANIZACIONAL_NO_ENCONTRADO,
                    Mensaje = "No existe una UnidadOrganizacional con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
            }

            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = dominio.UnidadesOrganizacionales.IndexOf(entidad);
                if(index == 0)
                {
                    dominio.UnidadesOrganizacionales[0] = entidad;
                    _dbSetDominio.Update(dominio);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.ORGANIZACION_UNIDADORGANIZACIONAL_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar la UnidadOrganizacional",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
                }
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
            _logger.LogError(ex, "ServicioUnidadOrganizacional-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<RespuestaPayload<UnidadOrganizacional>> UnicaPorId(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<UnidadOrganizacional>();
        try
        {
            var dominio = _dbSetDominio.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(parametros["n0Id"])));
            UnidadOrganizacional actual = dominio.UnidadesOrganizacionales.FirstOrDefault(_ => _.Id.Equals(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_UNIDADORGANIZACIONAL_NO_ENCONTRADO,
                    Mensaje = "No existe la UnidadOrganizacional con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUnidadOrganizacional-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<Respuesta> Eliminar(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (id == Guid.Empty)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_UNIDADORGANIZACIONAL_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest,
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            var dominio = _dbSetDominio.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(parametros["n0Id"])));
            UnidadOrganizacional actual = dominio.UnidadesOrganizacionales.FirstOrDefault(_ => _.Id.Equals(id));
            if(actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_UNIDADORGANIZACIONAL_NO_ENCONTRADO,
                    Mensaje = "No existe la UnidadOrganizacional con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                dominio.UnidadesOrganizacionales.Remove(actual);
                _dbSetDominio.Update(dominio);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_UNIDADORGANIZACIONAL_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR la UnidadOrganizacional",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUnidadOrganizacional-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<UnidadOrganizacional>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioUnidadOrganizacional - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(UnidadOrganizacional));
        var Elementos = Enumerable.Empty<UnidadOrganizacional>().AsQueryable();
        var dominio = _dbSetDominio.FirstOrDefault(_ => _.Id.Equals(Guid.Parse(parametros["n0Id"])));
        if(dominio != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<UnidadOrganizacional>(consulta, entidad);
                if (predicateBody != null)
                {
                    var RConsulta = dominio.UnidadesOrganizacionales.AsQueryable().Provider.CreateQuery<UnidadOrganizacional>(predicateBody.getWhereExpression(dominio.UnidadesOrganizacionales.AsQueryable().Expression));
                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = dominio.UnidadesOrganizacionales.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        return Elementos.Paginado(consulta);
    }

    public async Task<RespuestaPayload<UnidadOrganizacionalDespliegue>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros = null)
    {
        RespuestaPayload<UnidadOrganizacionalDespliegue> respuesta = new RespuestaPayload<UnidadOrganizacionalDespliegue>();
        try
        {
            var resultado = await UnicaPorId(id, parametros);
            respuesta.Ok = resultado.Ok;
            if (resultado.Ok)
            {
                respuesta.Payload = ADTODespliegue((UnidadOrganizacional)resultado.Payload);
            }
            else
            {
                respuesta.Error = resultado.Error;
                respuesta.Error!.Codigo = CodigosError.ORGANIZACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultado.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUnidadPorIdDespliegue-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return