using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo.documentacion;
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
namespace controlescolar.servicios.entidadrolpersonaescuela;
[ServicioEntidadAPI(entidad: typeof(EntidadRolPersonaEscuela))]
public class ServicioEntidadRolPersonaEscuela : ServicioEntidadGenericaBase<EntidadRolPersonaEscuela, CreaRolPersonaEscuela, ActualizaRolPersonaEscuela, ConsultaRolPersonaEscuela, string>,
    IServicioEntidadAPI, IServicioEntidadRolPersonaEscuela
{
    private readonly ILogger<ServicioEntidadRolPersonaEscuela> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private EntidadEscuela? _entidadEscuela;
    private DbSet<EntidadEscuela> _dbSetEntidadEscuela;

    public ServicioEntidadRolPersonaEscuela(ILogger<ServicioEntidadRolPersonaEscuela> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
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
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaRolPersonaEscuela>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros, forzarEliminacion);
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaRolPersonaEscuela>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => EntidadRolPersonaEscuela
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaRolPersonaEscuela data)
    {
        return new ResultadoValidacion()
        {
            Valido = true
        };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadRolPersonaEscuela data, bool forzarEliminacion = false)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaRolPersonaEscuela actualizacion, EntidadRolPersonaEscuela original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override EntidadRolPersonaEscuela ADTOFull(ActualizaRolPersonaEscuela actualizacion, EntidadRolPersonaEscuela actual)
    {
        actual.Id = actualizacion.Id;
        actual.Nombre = actualizacion.Nombre;
        actual.Clave = actualizacion.Clave;
        actual.Descripcion = actualizacion.Descripcion;
        return actual;
    }

    public override EntidadRolPersonaEscuela ADTOFull(CreaRolPersonaEscuela data)
    {
        //Faltaría implementar el generador de Id, en función del Id max que exista en el repositorio.
        return new EntidadRolPersonaEscuela()
        { 
            Id = 1,
            Nombre = data.Nombre,
            Clave = data.Clave,
            Descripcion = data.Descripcion
        };
    }

    public override ConsultaRolPersonaEscuela ADTODespliegue(EntidadRolPersonaEscuela data)
    {
        return new ConsultaRolPersonaEscuela() 
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Clave = data.Clave,
            Descripcion = data.Descripcion,
            Movimientos = data.Movimientos
        };
    }

    public async Task<RespuestaPayload<EntidadRolPersonaEscuela>> CalculandoIdMax(EntidadRolPersonaEscuela entidadExpediente, StringDictionary parametros)
    {
        var respuesta = new RespuestaPayload<EntidadRolPersonaEscuela>();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));

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
            respuesta.Payload = entidadExpediente;
            return respuesta;
        }

        entidadExpediente.Id = _entidadEscuela.RolesPersona.Max(_ => _.Id) + 1;
        respuesta.Ok = true;
        respuesta.Payload = entidadExpediente;
        return respuesta;
    }
    public override async Task<RespuestaPayload<ConsultaRolPersonaEscuela>> Insertar(CreaRolPersonaEscuela data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ConsultaRolPersonaEscuela>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                _entidadEscuela.RolesPersona.Add(entidad);
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
            _logger.LogError(ex, "ServicioEntidadRolPersonaEscuela-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaRolPersonaEscuela data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADROLPERSONAESCUELA_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            EntidadRolPersonaEscuela actual = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADROLPERSONAESCUELA_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadRolPersonaEscuela con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = _entidadEscuela.RolesPersona.IndexOf(entidad);
                if (index == 0)
                {
                    _entidadEscuela.RolesPersona[0] = entidad;
                    _dbSetEntidadEscuela.Update(_entidadEscuela);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.CONTROLESCOLAR_ENTIDADROLPERSONAESCUELA_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar la EntidadRolPersonaEscuela",
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
            _logger.LogError(ex, "ServicioEntidadRolPersonaEscuela-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<EntidadRolPersonaEscuela>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadRolPersonaEscuela>();
        try
        {
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            EntidadRolPersonaEscuela actual = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADROLPERSONAESCUELA_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadRolPersonaEscuela con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadRolEscuelaPersona-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADROLPERSONAESCUELA_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            EntidadRolPersonaEscuela actual = _entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADROLPERSONAESCUELA_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadRolPersonaEscuela con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                _entidadEscuela.RolesPersona.Remove(actual);
                _dbSetEntidadEscuela.Update(_entidadEscuela);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADROLPERSONAESCUELA_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR la EntidadRolPersonaEscuela",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadRolPersonaEscuela-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<EntidadRolPersonaEscuela>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadRolPersonaEscuela - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(EntidadRolPersonaEscuela));
        var Elementos = Enumerable.Empty<EntidadRolPersonaEscuela>().AsQueryable();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        if (_entidadEscuela != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<EntidadRolPersonaEscuela>(consulta, entidad);
                if (predicateBody != null)
                {
                    var RConsulta = _entidadEscuela.Planteles.AsQueryable().Provider.CreateQuery<EntidadRolPersonaEscuela>(predicateBody.getWhereExpression(_entidadEscuela.Planteles.AsQueryable().Expression));
                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = _entidadEscuela.RolesPersona.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        return Elementos.Paginado(consulta);
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return
