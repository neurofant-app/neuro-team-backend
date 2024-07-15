#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using Amazon.Runtime.Internal.Util;
using apigenerica.model.abstracciones;
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using aprendizaje.model.neurona;
using aprendizaje.model.temario;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Data.Common;
using System.Text.Json;

namespace aprendizaje.services;
[ServicioEntidadAPI(entidad: typeof(Temario))]
public class ServicioTemario : ServicioEntidadGenericaBase<Temario, Temario, Temario, Temario, string>,
    IServicioEntidadAPI, IServicioTemario
{
    private readonly ILogger<ServicioTemario> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    public ServicioTemario(ILogger<ServicioTemario> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextAprendizaje.NOMBRE_COLECCION_TEMARIO);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextAprendizaje.NOMBRE_COLECCION_TEMARIO}'";
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
            _dbSetFull = ((MongoDbContextAprendizaje)_db).Temario;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextAprendizaje.NOMBRE_COLECCION_TEMARIO}'");
            throw;
        }
    }
    private MongoDbContextAprendizaje DB { get { return (MongoDbContextAprendizaje)_db; } }
    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<Temario>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        return await this.Eliminar((string)id);
    }

    public Entidad EntidadDespliegueAPI()
    {
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        var add = data.Deserialize<Temario>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<Object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public Task<Entidad>? Metadatos(string Tipo)
    {
        throw new NotImplementedException();
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => NEURONA
    public override async Task<ResultadoValidacion> ValidarInsertar(Temario data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Temario original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Temario actualizacion, Temario original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override Temario ADTOFull(Temario actualizacion, Temario actual)
    {
        actual.Id = actualizacion.Id;
        actual.EspacioTrabajoId = actualizacion.EspacioTrabajoId;
        actual.Idiomas = actualizacion.Idiomas;
        actual.Nombre = actualizacion.Nombre;
        actual.Publico = actualizacion.Publico;
        actual.Temas = actualizacion.Temas;
        actual.EstadoPublicacion = actualizacion.EstadoPublicacion;
        actual.Version = actualizacion.Version;
        actual.TemarioDerivadoId = actualizacion.TemarioDerivadoId;
        return actual;
    }

    public override Temario ADTOFull(Temario data)
    {
        Temario inv = new Temario()
        {
            Id = Guid.NewGuid(),
            EspacioTrabajoId = data.EspacioTrabajoId,
            Idiomas = data.Idiomas,
            Nombre = data.Nombre,
            Publico = data.Publico,
            EstadoPublicacion = data.EstadoPublicacion,
            Version = data.Version,
            TemarioDerivadoId = data.TemarioDerivadoId
        };
        return inv;
    }

    public override async Task<Respuesta> Actualizar(string id, Temario data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Temario actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
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
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<Temario>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Temario>();
        try
        {
            Temario actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Temario actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
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
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    #endregion
}

#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // P
