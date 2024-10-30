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
namespace controlescolar.servicios.entidaddocumentobase;
[ServicioEntidadAPI(entidad: typeof(EntidadDocumentoBase))]
public class ServicioEntidadDocumentoBase : ServicioEntidadGenericaBase<EntidadDocumentoBase, CreaDocumentoBase, ActualizaDocumentoBase, ConsultaDocumentoBase, string>,
    IServicioEntidadAPI, IServicioEntidadDocumentoBase
{
    private readonly ILogger<ServicioEntidadDocumentoBase> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private EntidadEscuela? _entidadEscuela;
    private EntidadExpediente? _entidadExpediente;
    private DbSet<EntidadEscuela> _dbSetEntidadEscuela;

    public ServicioEntidadDocumentoBase(ILogger<ServicioEntidadDocumentoBase> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
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
        _logger.LogDebug("ServicioEntidadDocumentoBase-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaDocumentoBase>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioEntidadDocumentoBase-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros);
        _logger.LogDebug("ServicioEntidadDocumentoBase-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaDocumentoBase>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadDocumentoBase-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadDocumentoBase-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadDocumentoBase-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadDocumentoBase-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadDocumentoBase-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => EntidadDocumentoBase
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaDocumentoBase data)
    {
        return new ResultadoValidacion()
        {
            Valido = true
        };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadDocumentoBase data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaDocumentoBase actualizacion, EntidadDocumentoBase original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override EntidadDocumentoBase ADTOFull(ActualizaDocumentoBase actualizacion, EntidadDocumentoBase actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Caducidad = actualizacion.Caducidad;
        actual.Ambito = actualizacion.Ambito;
        actual.PlantelId = actualizacion.PlantelId;
        actual.Opcional = actualizacion.Opcional;
        return actual;
    }

    public override EntidadDocumentoBase ADTOFull(CreaDocumentoBase data)
    {
        return new EntidadDocumentoBase()
        {
            Id = 1,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            Caducidad = data.Caducidad,
            Ambito = data.Ambito,
            PlantelId = data.PlantelId,
            Opcional = data.Opcional,
        };
    }

    public override ConsultaDocumentoBase ADTODespliegue(EntidadDocumentoBase data)
    {
        return new ConsultaDocumentoBase()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            Activo = data.Activo,
            Caducidad = data.Caducidad,
            Ambito = data.Ambito,
            PlantelId = data.PlantelId,
            Opcional = data.Opcional
        };
    }

    public async Task<RespuestaPayload<EntidadDocumentoBase>> CalculandoIdMax(EntidadDocumentoBase entidadDocumentoBase, StringDictionary parametros)
    {
        var respuesta = new RespuestaPayload<EntidadDocumentoBase>();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        _entidadExpediente = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));

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

        entidadDocumentoBase.Id = _entidadExpediente.Documentos.Max(_ => _.Id) + 1;
        respuesta.Ok = true;
        respuesta.Payload = entidadDocumentoBase;
        return respuesta;
    }

    public override async Task<RespuestaPayload<ConsultaDocumentoBase>> Insertar(CreaDocumentoBase data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ConsultaDocumentoBase>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                _entidadExpediente.Documentos.Add(entidad);
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
            _logger.LogError(ex, "ServicioEntidadDocumentoBase-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaDocumentoBase data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADDOCUMENTOBASE_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == new Guid(parametros["n0Id"]));
            _entidadExpediente = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
            var actual = _entidadExpediente.Documentos.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADDOCUMENTOBASE_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadDocumentoBase con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = _entidadExpediente.Documentos.IndexOf(entidad);
                if (index == 0)
                {
                    _entidadExpediente.Documentos[0] = entidad;
                    _dbSetEntidadEscuela.Update(_entidadEscuela);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.CONTROLESCOLAR_ENTIDADDOCUMENTOBASE_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar la EntidadDocumentoBase",
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
            _logger.LogError(ex, "ServicioEntidadDocumentoBase-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<EntidadDocumentoBase>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadDocumentoBase>();
        try
        {
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            _entidadExpediente = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
            var actual = _entidadExpediente.Documentos.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADDOCUMENTOBASE_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadDocumentoBase con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadDocumentoBase-UnicaPorId {msg}", ex.Message);
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
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADDOCUMENTOBASE_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            _entidadExpediente = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
            var actual = _entidadExpediente.Documentos.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADDOCUMENTOBASE_NO_ENCONTRADA,
                    Mensaje = "No existe un EntidadDocumentoBase con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                _entidadExpediente.Documentos.Remove(actual);
                _dbSetEntidadEscuela.Update(_entidadEscuela);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADDOCUMENTOBASE_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR la EntidadDocumentoBase",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadDocumentoBase-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<EntidadDocumentoBase>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadDocumentoBase - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(EntidadDocumentoBase));
        var Elementos = Enumerable.Empty<EntidadDocumentoBase>().AsQueryable();
        _entidadEscuela = _dbSetEntidadEscuela.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        _entidadExpediente = _entidadEscuela.Expedientes.FirstOrDefault(_ => _.Id == (long)Convert.ToDouble(parametros["n1Id"]));
        if (_entidadExpediente != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<EntidadDocumentoBase>(consulta, entidad);
                if (predicateBody != null)
                {
                    var RConsulta = _entidadExpediente.Documentos.AsQueryable().Provider.CreateQuery<EntidadDocumentoBase>(predicateBody.getWhereExpression(_entidadExpediente.Documentos.AsQueryable().Expression));
                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = _entidadExpediente.Documentos.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        return Elementos.Paginado(consulta);
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
