﻿#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
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
        _logger.LogDebug("ServicioTemario-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Temario>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioTemario-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioTemario-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioTemario-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioTemario-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioTemario-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioTemario-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioTemario-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioTemario-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioTemario-InsertarAPI-{data}", data);
        var add = data.Deserialize<Temario>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioTemario-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Task<Entidad>? Metadatos(string Tipo)
    {
        throw new NotImplementedException();
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioTemario-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioTemario-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioTemario-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioTemario-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioTemario-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioTemario-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioTemario-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioTemario-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioTemario-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
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
            Temas = data.Temas,
            EstadoPublicacion = data.EstadoPublicacion,
            Version = data.Version,
            TemarioDerivadoId = data.TemarioDerivadoId
        };
        return inv;
    }

    public override Temario ADTODespliegue(Temario data)
    {
        Temario temario = new Temario()
        {
            Id = data.Id,
            EspacioTrabajoId = data.EspacioTrabajoId,
            Idiomas = data.Idiomas,
            Nombre = data.Nombre,
            Publico = data.Publico,
            Temas = data.Temas,
            EstadoPublicacion = data.EstadoPublicacion,
            Version = data.Version,
            TemarioDerivadoId = data.TemarioDerivadoId

        };
        return temario;
    }

    public override async Task<Respuesta> Actualizar(string id, Temario data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_TEMARIO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Temario actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_TEMARIO_NO_ENCONTRADA,
                    Mensaje = "No existe un Temario con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioTemario-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_TEMARIO_NO_ENCONTRADA,
                    Mensaje = "No existe un Temario con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioTemario-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.APRENDIZAJE_TEMARIO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Temario actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_TEMARIO_NO_ENCONTRADA,
                    Mensaje = "No existe una Temario con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioTemario-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return
