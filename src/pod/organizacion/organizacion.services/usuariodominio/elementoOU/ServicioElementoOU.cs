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
using organizacion.model.usuariodominio;
using organizacion.services.dbcontext;
using organizacion.services.dominio;
using organizacion.services.usuariodominio.elementoDominio;
using System.Collections.Specialized;
using System.Text.Json;

namespace organizacion.services.usuariodominio.elementoOU;
[ServicioEntidadAPI(typeof(ElementoOU))]
public class ServicioElementoOU : ServicioEntidadGenericaBase<ElementoOU, ElementoOU, ElementoOU, ElementoOU, Guid>,
    IServicioEntidadAPI, IServicioElementoUO
{
    private readonly ILogger<ServicioElementoOU> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IServicioDominio servicioDominio;
    private readonly IServicioUsuarioDominio servicioUsuarioDominio;
    private readonly IDistributedCache _cache;
    private DbSet<UsuarioDominio> _dbSetUsuarioDominio;
    public ServicioElementoOU(ILogger<ServicioElementoOU> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IServicioDominio servicioDominio, IServicioUsuarioDominio servicioUsuarioDominio,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        this.servicioDominio = servicioDominio;
        this.servicioUsuarioDominio = servicioUsuarioDominio;
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
            _dbSetUsuarioDominio = ((MongoDbContextOrganizacion)_db).UsuarioDominios;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicialzar mongo para '{MongoDbContextOrganizacion.NOMBRE_COLECCION_USUARIODOMINIOS}'");
            throw;
        }

    }

    public bool RequiereAutenticacion => true;
    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioElementoOU-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioElementoOU-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioElementoOU-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioElementoOU-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioElementoOU-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioElementoOU-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioElementoOU-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ElementoOU>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioElementoOU-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioElementoOU-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros, forzarEliminacion);
        _logger.LogDebug("ServicioElementoOU-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioElementoOU-InsertarAPI-{data}", data);
        var add = data.Deserialize<ElementoOU>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioElementoOU-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioElementoOU-PaginaAPI");
        var respuesta = new RespuestaPayload<PaginaGenerica<object>>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioElementoOU-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioElementoOU-PaginaDespliegueAPI");
        var respuesta = new RespuestaPayload<PaginaGenerica<object>>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioElementoOU-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioElementoOU-UnicaPorIdAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioElementoOU-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioElementoOU-UnicaPorIdDespliegueAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioElementoOU-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }



    #region Overrides para la entidad ElementoUO
    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, ElementoOU actualizacion, ElementoOU original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, ElementoOU original, bool forzarEliminacion = false)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarInsertar(ElementoOU data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<RespuestaPayload<ElementoOU>> Insertar(ElementoOU data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ElementoOU>();
        try
        {
            var busquedaDominio = await this.servicioDominio.UnicaPorId(Guid.Parse(parametros["n0Id"]));


            if(busquedaDominio.Ok == false)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_DOMINIO_NO_ENCONTRADA,
                    Mensaje = "Dominio no encontrado",
                    HttpCode = HttpCode.NotFound
                };
                return respuesta;
            }

            var dominio = (Dominio)busquedaDominio.Payload;
            var existeOU = dominio.UnidadesOrganizacionales.FirstOrDefault(_ => _.Id == data.OUId);

            if(existeOU == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_NO_EXISTE_OU_EN_DOMINIO,
                    Mensaje = "No existe la OU en el dominio",
                    HttpCode = HttpCode.NotFound
                };
                return respuesta;
            }

            var usuarioDom = _dbSetUsuarioDominio.Find(Guid.Parse(parametros["n1Id"]));
            if(usuarioDom == null)
            {
                var usuario = await this.servicioUsuarioDominio.Insertar(new ElementoDominioInsertar() { UsurioId = Guid.Parse(parametros["n1Id"]) ,Activo = data.Activa}, parametros);
                if(usuario.Payload == null)
                {
                    var usrDom = this._dbSetUsuarioDominio.Find(Guid.Parse(parametros["n1Id"]));
                    if(usrDom == null)
                    {
                        ElementoDominio dom = usrDom.Dominios.FirstOrDefault(_ => _.DominioId == Guid.Parse(parametros["n0Id"]));
                        dom.OUIds.Add(data);
                        _dbSetUsuarioDominio.Update(usrDom);
                        await _db.SaveChangesAsync();

                        respuesta.Ok = true;
                        respuesta.HttpCode = HttpCode.Ok;
                        respuesta.Payload = ADTODespliegue(data);
                        return respuesta;
                    }
                }
            }
            var elementoDom = usuarioDom.Dominios.FirstOrDefault(_ => _.DominioId == Guid.Parse(parametros["n0Id"]));
            if (elementoDom == null)
            {
                ElementoDominio elemento = new ElementoDominio()
                {
                    DominioId = Guid.Parse(parametros["n0Id"]),
                    Activo = data.Activa,
                };
                elemento.OUIds.Add(data);
                usuarioDom.DominiosId.Add(Guid.Parse(parametros["n0Id"]));
                usuarioDom.Dominios.Add(elemento);
            }
            else
            {
                elementoDom.OUIds.Add(data);
            }

            _dbSetUsuarioDominio.Update(usuarioDom);
            await _db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = ADTODespliegue(data);
            return respuesta;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioElementoOU-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }
    public async Task<Respuesta> Actualizar(Guid id, ElementoOU data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (id == (Guid.Empty) || data == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No se ha proporcionado Id o Payload",
                    HttpCode = HttpCode.BadRequest
                };
            }
            
            UsuarioDominio actual = _dbSetUsuarioDominio.Find(Guid.Parse(parametros["n1Id"]));
            if (actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_NO_ENCONTRADO,
                    Mensaje = "No se ha encontrado un UsuarioDominio",
                    HttpCode = HttpCode.NotFound
                };
            }

            var elementoDominio = actual.Dominios.FirstOrDefault(_ => _.DominioId == Guid.Parse(parametros["n0Id"]));
            var elementoOud = elementoDominio.OUIds.FirstOrDefault(_ => _.OUId == data.OUId);
            if(elementoDominio.Activo && data.Activa) elementoOud.Activa = data.Activa;
            _dbSetUsuarioDominio.Update(actual);
            await _db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioElemetoOU-Actualizar {msg}", ex.Message);
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
            if (id == (Guid.Empty))
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_ID_NO_INGRESADO,
                    Mensaje = "No se ha proporcionado Id",
                    HttpCode = HttpCode.BadRequest
                };
                return respuesta;
            }

            UsuarioDominio actual = _dbSetUsuarioDominio.Find(Guid.Parse(parametros["n1Id"]));
            if (actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.ORGANIZACION_ELEMENTOOU_NO_ENCONTRADO,
                    Mensaje = "No existe un ElementoOU con el Id proporcionado",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var elementoDominio = actual.Dominios.FirstOrDefault(_ => _.DominioId == Guid.Parse(parametros["n0Id"]));
            elementoDominio.OUIds.Remove(elementoDominio.OUIds.FirstOrDefault(_ => _.OUId == id));
            await _db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioUsuarioDominio-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ORGANIZACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public Task<RespuestaPayload<ElementoOU>> UnicaPorId(Guid id, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }
    public Task<RespuestaPayload<ElementoOU>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }
    #endregion
}
