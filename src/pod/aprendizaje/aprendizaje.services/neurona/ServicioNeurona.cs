#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using aprendizaje.model.neurona;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;

namespace aprendizaje.services;
[ServicioEntidadAPI(entidad: typeof(Neurona))]
public class ServicioNeurona : ServicioEntidadGenericaBase<Neurona, Neurona, Neurona, Neurona, string>,
    IServicioEntidadAPI, IServicioNeurona
{
    private readonly ILogger<ServicioNeurona> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    public ServicioNeurona(ILogger<ServicioNeurona> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextAprendizaje.NOMBRE_COLECCION_NEURONA);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextAprendizaje.NOMBRE_COLECCION_NEURONA}'";
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

            _db = MongoDbContextAprendizaje.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextAprendizaje)_db).Neurona;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextAprendizaje.NOMBRE_COLECCION_NEURONA}'");
            throw;
        }
    }
    private MongoDbContextAprendizaje DB { get { return (MongoDbContextAprendizaje)_db; } }
    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioNeurona-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Neurona>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioNeurona-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioNeurona-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioNeurona-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;

    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioNeurona-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioNeurona-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioNeurona-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioNeurona-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioNeurona-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioNeurona-InsertarAPI-{data}", data);
        var add = data.Deserialize<Neurona>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioNeurona-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Task<Entidad>? Metadatos(string Tipo, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioNeurona-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioNeurona-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioNeurona-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioNeurona-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioNeurona-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioNeurona-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioNeurona-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioNeurona-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioNeurona-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => NEURONA
    public override async Task<ResultadoValidacion> ValidarInsertar(Neurona data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Neurona original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Neurona actualizacion, Neurona original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override Neurona ADTOFull(Neurona actualizacion, Neurona actual)
    {
        actual.EspacioTrabajoId = actualizacion.EspacioTrabajoId;
        actual.Idiomas = actualizacion.Idiomas;
        actual.Nombre = actualizacion.Nombre;
        actual.Descripcion = actualizacion.Descripcion;
        actual.TemarioId = actualizacion.TemarioId;
        actual.EstadoPublicacion = actualizacion.EstadoPublicacion;
        actual.Version = actualizacion.Version;
        actual.NeuronaDerivadaId = actualizacion.NeuronaDerivadaId;
        actual.FechaCreacion = actualizacion.FechaCreacion;
        actual.FechaActualizacion = actualizacion.FechaActualizacion;
        actual.FechaConsulta = actualizacion.FechaConsulta;
        actual.AlmacenamientoId = actualizacion.AlmacenamientoId;
        actual.TipoLicencia = actualizacion.TipoLicencia;
        actual.ConteoFlashcards = actualizacion.ConteoFlashcards;
        actual.ConteoActividdades = actualizacion.ConteoActividdades;
        actual.ConteoDescargas = actualizacion.ConteoDescargas;
        actual.SecuenciaObjetos = actualizacion.SecuenciaObjetos;
        actual.Flashcards = actualizacion.Flashcards;
        actual.ActividadesIds = actualizacion.ActividadesIds;
        return actual;
    }

    public override Neurona ADTOFull(Neurona data)
    {
        Neurona neurona = new Neurona()
        {
            Id = Guid.NewGuid(),
            EspacioTrabajoId = data.EspacioTrabajoId,
            Idiomas = data.Idiomas,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            TemarioId = data.TemarioId,
            EstadoPublicacion = data.EstadoPublicacion,
            Version = data.Version,
            NeuronaDerivadaId = data.NeuronaDerivadaId,
            FechaCreacion = DateTime.UtcNow,
            FechaActualizacion = data.FechaActualizacion,
            FechaConsulta = data.FechaConsulta,
            AlmacenamientoId = data.AlmacenamientoId,
            TipoLicencia = data.TipoLicencia,
            ConteoFlashcards = data.ConteoFlashcards,
            ConteoActividdades = data.ConteoActividdades,
            ConteoDescargas = data.ConteoDescargas,
            SecuenciaObjetos = data.SecuenciaObjetos,
            Flashcards = data.Flashcards,
            ActividadesIds = data.ActividadesIds,
        };
        return neurona;
    }

    public override Neurona ADTODespliegue(Neurona data)
    {
        Neurona neurona = new Neurona()
        {
            Id = data.Id,
            EspacioTrabajoId = data.EspacioTrabajoId,
            Idiomas = data.Idiomas,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            TemarioId = data.TemarioId,
            EstadoPublicacion = data.EstadoPublicacion,
            Version = data.Version,
            NeuronaDerivadaId = data.NeuronaDerivadaId,
            FechaCreacion = DateTime.UtcNow,
            FechaActualizacion = data.FechaActualizacion,
            FechaConsulta = data.FechaConsulta,
            AlmacenamientoId = data.AlmacenamientoId,
            TipoLicencia = data.TipoLicencia,
            ConteoFlashcards = data.ConteoFlashcards,
            ConteoActividdades = data.ConteoActividdades,
            ConteoDescargas = data.ConteoDescargas,
            SecuenciaObjetos = data.SecuenciaObjetos,
            Flashcards = data.Flashcards,
            ActividadesIds = data.ActividadesIds,

        };
        return neurona;
    }


    public override async Task<Respuesta> Actualizar(string id, Neurona data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_NEURONA_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Neurona actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_NEURONA_NO_ENCONTRADA,
                    Mensaje = "No existe una NEURONA con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.APRENDIZAJE_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioNeurona-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<Neurona>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Neurona>();
        try
        {
            Neurona actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_NEURONA_NO_ENCONTRADA,
                    Mensaje = "No existe una Neurona con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioNeurona-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.APRENDIZAJE_NEURONA_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Neurona actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_NEURONA_NO_ENCONTRADA,
                    Mensaje = "No existe una Neurona con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.APRENDIZAJE_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {

            _logger.LogError(ex, "ServicioNeurona-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    #endregion
}

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return