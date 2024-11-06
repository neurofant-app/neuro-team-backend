using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo;
using controlescolar.modelo.escuela;
using controlescolar.modelo.rolesescolares;
using controlescolar.servicios.dbcontext;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
namespace controlescolar.servicios.entidadescuela;
[ServicioEntidadAPI(entidad: typeof(EntidadEscuela))]
public class ServicioEntidadEscuela : ServicioEntidadGenericaBase<EntidadEscuela, CreaEscuela, ActualizaEscuela, ConsultaEscuela, string>,
    IServicioEntidadAPI, IServicioEntidadEscuela
{
    private readonly ILogger<ServicioEntidadEscuela> _logger;
    private readonly IReflectorEntidadesAPI _reflector;

    public ServicioEntidadEscuela(ILogger<ServicioEntidadEscuela> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
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
            _dbSetFull = ((MongoDbContext)_db).Escuelas;
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
        _logger.LogDebug("ServicioEntidadEscuela-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaEscuela>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioEntidadEscuela-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadEscuela-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros);
        _logger.LogDebug("ServicioEntidadEscuela-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadEscuela-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadEscuela-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadEscuela-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadEscuela-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadEscuela-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadEscuela-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaEscuela>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadEscuela-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Task<Entidad>? Metadatos(string Tipo, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadEscuela-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadEscuela-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadEscuela-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadEscuela-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadEscuela-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadEscuela-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadEscuela-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadEscuela-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadEscuela-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la Entidad => EntidadEscuela

    public override async Task<ResultadoValidacion> ValidarInsertar(CreaEscuela data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadEscuela data)
    {
        return new ResultadoValidacion() { Valido = true };
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaEscuela actualizacion, EntidadEscuela original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override EntidadEscuela ADTOFull(ActualizaEscuela actualizacion, EntidadEscuela actual)
    {
        actual.Id = actualizacion.Id;
        actual.Nombre = actualizacion.Nombre;
        actual.Clave = actualizacion.Clave;
        return actual;
    }

    public override EntidadEscuela ADTOFull(CreaEscuela data)
    {
        EntidadEscuela entidadEscuela = new EntidadEscuela()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            Activa = true,
            Creacion = DateTime.UtcNow,
            CuentaId = _contextoUsuario.UsuarioId,
            Clave = data.Clave,
            RolesPersona = Defaults.RolesPersonaEscuelaBase(""),
        };

        entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Nombre == "Alumno").Movimientos = Defaults.MovimientosRolAlumnoPersonaEscuelaBase("");
        entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Nombre == "Docente").Movimientos = Defaults.MovimientosRolDocentePersonaEscuelaBase("");
        entidadEscuela.RolesPersona.FirstOrDefault(_ => _.Nombre == "Administrativo").Movimientos = Defaults.MovimientosRolAdministrativoPersonaEscuelaBase("");
        return entidadEscuela;
    }

    public override ConsultaEscuela ADTODespliegue(EntidadEscuela data)
    {
        return new ConsultaEscuela()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Activa = data.Activa,
            Creacion = data.Creacion,
            Clave = data.Clave,
            RolesPersona = data.RolesPersona,
            Planteles = data.Planteles,
            Expedientes = data.Expedientes,
            Configuracion = data.Configuracion
        };
    }


    ///faltan los metodos para actualizar eliminar, unico paghina y asi...
    public override async Task<Respuesta> Actualizar(string id, ActualizaEscuela data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADESCUELA_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadEscuela actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
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

            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                _dbSetFull.Update(entidad);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
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
            _logger.LogError(ex, "ServicioEntidadEscuela-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    } 

    public override async Task<RespuestaPayload<EntidadEscuela>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadEscuela>();

        try
        {
            EntidadEscuela actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if(actual == null)
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
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadEscuela-UnicaPorId {msg}", ex.Message);
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
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADESCUELA_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcioando el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
            }

            EntidadEscuela actual = _dbSetFull.Find(Guid.Parse(id));
            if(actual != null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADESCUELA_NO_ENCONTRADA,
                    Mensaje = "No existe una EntidadEscuela con el Id proporcioando",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if(resultadoValidacion.Valido)
            {
                _dbSetFull.Remove(actual);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
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
            _logger.LogError(ex, "ServicioEntidadEscuela - Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.