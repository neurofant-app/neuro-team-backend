using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo.documentacion;
using controlescolar.modelo.escuela;
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
namespace controlescolar.servicios.entidadexpediente;
[ServicioEntidadAPI(entidad: typeof(EntidadExpediente))]
public class ServicioEntidadExpediente : ServicioEntidadGenericaBase<EntidadExpediente, CreaExpediente, ActualizaExpediente, ConsultaExpediente, string>,
    IServicioEntidadAPI, IServicioEntidadExpediente
{
    private readonly ILogger<ServicioEntidadExpediente> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private EntidadEscuela? _entidadEscuela;
    private DbSet<EntidadEscuela> _dbSetEntidadEscuela;

    public ServicioEntidadExpediente(ILogger<ServicioEntidadExpediente> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
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
        _logger.LogDebug("ServicioEntidadExpediente-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaExpediente>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioEntidadExpediente-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadExpediente-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros);
        _logger.LogDebug("ServicioEntidadExpediente-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadExpediente-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadExpediente-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadExpediente-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadExpediente-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadExpediente-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadExpediente-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaExpediente>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadExpediente-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadExpediente-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadExpediente-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadExpediente-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadExpediente-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadExpediente-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadExpediente-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadExpediente-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadExpediente-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadExpediente-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => EntidadExpediente
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaExpediente data)
    {
        return new ResultadoValidacion()
        {
            Valido = true
        };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadExpediente data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaExpediente actualizacion, EntidadExpediente original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override EntidadExpediente ADTOFull(ActualizaExpediente actualizacion, EntidadExpediente actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Descripcion = actualizacion.Descripcion;
        actual.RolEscolarId = actualizacion.RolEscolarId;
        return actual;
    }

    public override EntidadExpediente ADTOFull(CreaExpediente data)
    {
        return new EntidadExpediente()
        { 
            Id = 0,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            RolEscolarId = data.RolEscolarId
        };
    }

    public override ConsultaExpediente ADTODespliegue(EntidadExpediente data)
    {
        return new ConsultaExpediente()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            FechaCreacion = data.FechaCreacion,
            Activo = data.Activo,
            RolEscolarId = data.RolEscolarId
        };
    }

    public async Task<RespuestaPayload<EntidadExpediente>> CalculandoIdMax(EntidadExpediente entidadExpediente, StringDictionary parametros)
    {
        var respuesta = new RespuestaPayload<EntidadExpediente>();
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

        entidadExpediente.Id = _entidadEscuela.Expedientes.Max(_ =>  _.Id) + 1;
        respuesta.Ok = true;
        respuesta.Payload = entidadExpediente;
        return respuesta;
    }

    public override async Task<RespuestaPayload<ConsultaExpediente>> Insertar(CreaExpediente data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ConsultaExpediente>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                var entidadExpediente = await CalculandoIdMax(entidad, parametros);
                if(entidadExpediente.Ok == true)
                {
                    _entidadEscuela.Expedientes.Add((EntidadExpediente)entidadExpediente.Payload);
                    _dbSetEntidadEscuela.Update(_entidadEscuela);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                    respuesta.Payload = ADTODespliegue(entidad);
                }
                else
                {
                    respuesta.Error = entidadExpediente.Error;
                    respuesta.Error!.Codigo = entidadExpediente.Error.Codigo;
                    respuesta.HttpCode = entidadExpediente.Error?.HttpCode ?? HttpCode.None;
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
            _logger.LogError(ex, "ServicioEntidadExpediente-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaExpediente data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADEXPEDIENTE_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            EntidadExpediente actual = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADEXPEDIENTE_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadExpediente con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = _entidadEscuela.Expedientes.IndexOf(entidad);
                if (index == 0)
                {
                    _entidadEscuela.Expedientes[0] = entidad;
                    _dbSetEntidadEscuela.Update(_entidadEscuela);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.CONTROLESCOLAR_ENTIDADEXPEDIENTE_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar la EntidadExpediente",
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
            _logger.LogError(ex, "ServicioEntidadExpediente-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<EntidadExpediente>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadExpediente>();
        try
        {
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            EntidadExpediente actual = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADEXPEDIENTE_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadExpediente con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadExpediente-UnicaPorId {msg}", ex.Message);
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
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADEXPEDIENTE_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            EntidadExpediente actual = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADEXPEDIENTE_NO_ENCONTRADA,
                    Mensaje = "No existe un ENTIDADEXPEDIENTE con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                _entidadEscuela.Expedientes.Remove(actual);
                _dbSetEntidadEscuela.Update(_entidadEscuela);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADEXPEDIENTE_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR la EntidadExpediente",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadExpediente-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<EntidadExpediente>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadExpediente - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(EntidadExpediente));
        var Elementos = Enumerable.Empty<EntidadExpediente>().AsQueryable();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        if (_entidadEscuela != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<EntidadExpediente>(consulta, entidad);
                if (predicateBody != null)
                {
                    var RConsulta = _entidadEscuela.Planteles.AsQueryable().Provider.CreateQuery<EntidadExpediente>(predicateBody.getWhereExpression(_entidadEscuela.Planteles.AsQueryable().Expression));
                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = _entidadEscuela.Expedientes.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        return Elementos.Paginado(consulta);
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return