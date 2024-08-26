#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo.campi;
using controlescolar.modelo.instructores;
using controlescolar.servicios.dbcontext;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace controlescolar.servicios;
[ServicioEntidadAPI(entidad:typeof(EntidadInstructor))]
public class ServicioEntidadInstructor : ServicioEntidadGenericaBase<EntidadInstructor, CreaInstructor, ActualizaInstructor, ConsultaInstructor, string>,
    IServicioEntidadAPI, IServicioEntidadInstructor
{
    private readonly ILogger _logger;

    private readonly IReflectorEntidadesAPI reflector;
    public ServicioEntidadInstructor(ILogger<ServicioEntidadInstructor> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContext.NOMBRE_COLECCION_INSTRUCTORES);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContext.NOMBRE_COLECCION_INSTRUCTORES}'";
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
            _dbSetFull = ((MongoDbContext)_db).EntidadInstructor;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContext.NOMBRE_COLECCION_INSTRUCTORES}'");
            throw;
        }
    }
    private MongoDbContext DB { get { return (MongoDbContext)_db; } }
    public bool RequiereAutenticacion => true;
    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadInstructor-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadInstructor-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadInstructor-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadInstructor-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadInstructor-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadInstructor-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioEntidadInstructor-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaInstructor>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInstructor-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioEntidadInstructor-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaInstructor>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioEntidadInstructor-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioEntidadInstructor-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioEntidadInstructor-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioEntidadInstructor-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInstructor-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioEntidadInstructor-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInstructor-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioEntidadInstructor-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInstructor-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioEntidadInstructor-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadInstructor-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la entidad EntidadInstructor
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaInstructor data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadInstructor original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaInstructor actualizacion, EntidadInstructor original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override EntidadInstructor ADTOFull(ActualizaInstructor actualizacion, EntidadInstructor actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Apellido1 = actualizacion.Apellido1;
        actual.Apellido2 = actualizacion.Apellido2;
        actual.FechaNacimiento = actualizacion.FechaNacimiento;
        actual.IdNacional = actualizacion.IdNacional;
        actual.Genero = actualizacion.Genero;
        actual.IdInterno = actualizacion.IdInterno;
        return actual;
    }

    public override EntidadInstructor ADTOFull(CreaInstructor data)
    {
        EntidadInstructor entidadInstructor = new EntidadInstructor()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            Apellido1 = data.Apellido1,
            Apellido2 = data.Apellido2,
            FechaNacimiento = data.FechaNacimiento,
            IdNacional = data.IdNacional,
            Genero = data.Genero,
            CampusId = data.CampusId,
            IdInterno = data.IdInterno
            
           
        };
        return entidadInstructor;
    }

    public override ConsultaInstructor ADTODespliegue(EntidadInstructor data)
    {
        ConsultaInstructor entidadInstructor = new ConsultaInstructor()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Apellido1 = data.Apellido1,
            Apellido2 = data.Apellido2,
            FechaNacimiento = data.FechaNacimiento,
            Genero = data.Genero,
            IdInterno = data.IdInterno,
            IdNacional = data.IdNacional
        };
        return entidadInstructor;
    }


    public override async Task<Respuesta> Actualizar(string id, ActualizaInstructor data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            EntidadInstructor actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADINSTRUCTOR_NO_ENCONTRADA,
                    Mensaje = "No existe una EntidadInstructor con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadInstructor-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<EntidadInstructor>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<EntidadInstructor>();
        try
        {
            EntidadInstructor actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADINSTRUCTOR_NO_ENCONTRADA,
                    Mensaje = "No existe una EntidadInstructor con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadInstructor-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Eliminar(string id)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADINSTRUCTOR_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadInstructor actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADINSTRUCTOR_NO_ENCONTRADA,
                    Mensaje = "No existe una EntidadInstructor con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
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
            _logger.LogError(ex, "ServicioEntidadInstructor-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    #endregion

}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
