using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo.escuela;
using controlescolar.modelo.rolesescolares;
using controlescolar.servicios.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
namespace controlescolar.servicios.entidadmovimientorolpersonaescuela;
[ServicioEntidadAPI(entidad: typeof(EntidadMovimientoRolPersonaEscuela))]
public class ServicioEntidadMovimientoRolPersonaEscuela : ServicioEntidadGenericaBase<EntidadMovimientoRolPersonaEscuela, CreaMovimientoRolPersonaEscuela, ActualizaMovimientoRolPersonaEscuela, ConsultaMovimientoRolPersonaEscuela, string>,
    IServicioEntidadAPI, IServicioMovimientoEntidadRolPersonaEscuela
{
        private readonly ILogger<ServicioEntidadMovimientoRolPersonaEscuela> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private EntidadEscuela? _entidadEscuela;
    private EntidadRolPersonaEscuela? _entidadRolPersonaEscuela;
    private DbSet<EntidadEscuela> _dbSetEntidadEscuela;

    public ServicioEntidadMovimientoRolPersonaEscuela(ILogger<ServicioEntidadMovimientoRolPersonaEscuela> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContext.NOMBRE_COLECCION_ESCUELAS);

        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContext.NOMBRE_COLECCION_ESCUELAS}'";
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

            _db = MongoDbContext.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetEntidadEscuela = ((MongoDbContext)_db).Escuelas;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContext.NOMBRE_COLECCION_ESCUELAS}'");
        }

    }

    private MongoDbContext DB { get { return (MongoDbContext)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaMovimientoRolPersonaEscuela>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros);
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaMovimientoRolPersonaEscuela>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => EntidadMovimientoRolPersonaEscuela
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaMovimientoRolPersonaEscuela data)
    {
        return new ResultadoValidacion()
        {
            Valido = true
        };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadMovimientoRolPersonaEscuela data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaMovimientoRolPersonaEscuela actualizacion, EntidadMovimientoRolPersonaEscuela original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override EntidadMovimientoRolPersonaEscuela ADTOFull(ActualizaMovimientoRolPersonaEscuela actualizacion, EntidadMovimientoRolPersonaEscuela actual)
    {
        actual.Id = actualizacion.Id;
        actual.RolPersonaEscuelaId = actualizacion.RolPersonaEscuelaId;
        actual.Nombre = actualizacion.Nombre;
        actual.Clave = actualizacion.Clave;
        actual.TipoMovimiento = actualizacion.TipoMovimiento;
        actual.TipoActualizacion = actualizacion.TipoActualizacion;
        return actual;
    }

    public override EntidadMovimientoRolPersonaEscuela ADTOFull(CreaMovimientoRolPersonaEscuela data)
    {
        return new EntidadMovimientoRolPersonaEscuela()
        {
            Id = 1,
            RolPersonaEscuelaId = data.RolPersonaEscuelaId,
            Nombre = data.Nombre,
            Clave = data.Clave,
            TipoMovimiento = data.TipoMovimiento,
            TipoActualizacion = data.TipoActualizacion
        };
    }

    public override ConsultaMovimientoRolPersonaEscuela ADTODespliegue(EntidadMovimientoRolPersonaEscuela data)
    {
        return new ConsultaMovimientoRolPersonaEscuela()
        {
            Id = data.Id,
            RolPersonaEscuelaId = data.RolPersonaEscuelaId,
            Nombre = data.Nombre,
            Clave = data.Clave,
            Eliminado = data.Eliminado,
            TipoMovimiento = data.TipoMovimiento,
            TipoActualizacion = data.TipoActualizacion
        };
    }

    public async Task<RespuestaPayload<EntidadMovimientoRolPersonaEscuela>> CalculandoIdMax(EntidadMovimientoRolPersonaEscuela entidadDocumentoBase, StringDictionary parametros)
    {
        var respuesta = new RespuestaPayload<EntidadMovimientoRolPersonaEscuela>();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        _entidadRolPersonaEscuela = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));

        if (_entidadEscuela == null)
        {
            respuesta.Error = new ErrorProceso()
            {
                Codigo = CodigosError.CONTROLESCOLAR_ENTIDADESCUELA_NO_ENCONTRADA,
                Mensaje = "No existe una EntidadEscuela con el Id proporcionado",
                HttpCode = HttpCode.NotFound
            };
            respuesta.HttpCode = HttpCode.NotFound;
            return respuesta;
        }

        if (!_entidadEscuela.Expedientes.Any())
        {
            respuesta.Ok = true;
            respuesta.Payload = entidadDocumentoBase;
            return respuesta;
        }

        entidadDocumentoBase.Id = _entidadRolPersonaEscuela.Movimientos.Max(_ => _.Id) + 1;
        respuesta.Ok = true;
        respuesta.Payload = entidadDocumentoBase;
        return respuesta;
    }

    public override async Task<RespuestaPayload<ConsultaMovimientoRolPersonaEscuela>> Insertar(CreaMovimientoRolPersonaEscuela data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ConsultaMovimientoRolPersonaEscuela>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                _entidadRolPersonaEscuela.Movimientos.Add(entidad);
                _dbSetEntidadEscuela.Update(_entidadEscuela);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONTROLESCOLAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadMovimientoRolPersonaEscuela-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaMovimientoRolPersonaEscuela data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADMOVIMIENTOROLPERSONAESCUELA_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            _entidadRolPersonaEscuela = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
            var actual = _entidadRolPersonaEscuela.Movimientos.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADMOVIMIENTOROLPERSONAESCUELA_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadMovientoRolPersonaEscuela con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = _entidadRolPersonaEscuela.Movimientos.IndexOf(entidad);
                if (index == 0)
                {
                    _entidadRolPersonaEscuela.Movimientos[0] = entidad;
                    _dbSetEntidadEscuela.Update(_entidadEscuela);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.CONTROLESCOLAR_ENTIDADMOVIMIENTOROLPERSONAESCUELA_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar la EntidadMovimientoRolPersonaEscuela",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
                }
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONTROLESCOLAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadMovimientoRolPersonaEscuela-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<EntidadMovimientoRolPersonaEscuela>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadMovimientoRolPersonaEscuela>();
        try
        {
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            _entidadRolPersonaEscuela = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
            var actual = _entidadRolPersonaEscuela.Movimientos.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADMOVIMIENTOROLPERSONAESCUELA_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadMovimientoRolPersonaEscuela con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadMovimientoRolPersonaEscuela-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADMOVIMIENTOROLPERSONAESCUELA_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            _entidadRolPersonaEscuela = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
            var actual = _entidadRolPersonaEscuela.Movimientos.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADMOVIMIENTOROLPERSONAESCUELA_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadMovimientoRolPersonaEscuela con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                _entidadRolPersonaEscuela.Movimientos.Remove(actual);
                _dbSetEntidadEscuela.Update(_entidadEscuela);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADMOVIMIENTOROLPERSONAESCUELA_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR la EntidadMovimientoRolPersonaEscuela",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadMovimientoRolPersonaEscuela-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<EntidadMovimientoRolPersonaEscuela>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadMovimientoRolPersonaEscuela - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(EntidadMovimientoRolPersonaEscuela));
        var Elementos = Enumerable.Empty<EntidadMovimientoRolPersonaEscuela>().AsQueryable();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        _entidadRolPersonaEscuela = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
        if (_entidadRolPersonaEscuela != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<EntidadMovimientoRolPersonaEscuela>(consulta, entidad);
                if (predicateBody != null)
                {
                    var RConsulta = _entidadRolPersonaEscuela.Movimientos.AsQueryable().Provider.CreateQuery<EntidadMovimientoRolPersonaEscuela>(predicateBody.getWhereExpression(_entidadRolPersonaEscuela.Movimientos.AsQueryable().Expression));
                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = _entidadRolPersonaEscuela.Movimientos.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        return Elementos.Paginado(consulta);
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.