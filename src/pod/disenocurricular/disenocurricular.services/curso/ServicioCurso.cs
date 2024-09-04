﻿#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using disenocurricular.model;
using disenocurricular.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Polly;
using System.Text.Json;

namespace disenocurricular.services.curso;

[ServicioEntidadAPI(typeof(Curso))]
public class ServicioCurso : ServicioEntidadGenericaBase<Curso, Curso, Curso, Curso, string>,
    IServicioEntidadAPI, IServicioCurso
{
    private readonly ILogger<ServicioCurso> _logger;
    private readonly IReflectorEntidadesAPI reflector;

    public ServicioCurso(ILogger<ServicioCurso> logger, IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI reflector, IDistributedCache distributedCache) : base(null, null, logger, reflector, distributedCache)
    {
        this._logger = logger;
        this.reflector = reflector;

        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextDisenoCurricular.NOMBRE_COLECCION_CURSOS);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextDisenoCurricular.NOMBRE_COLECCION_CURSOS}'";
            _logger.LogError(err);
            throw new Exception(err);
        }

        try
        {
            _logger.LogDebug($"Mongo DB{configuracionEntidad.Esquema} colección {configuracionEntidad.Esquema} utilizando conexión default {string.IsNullOrEmpty(configuracionEntidad.Conexion)}");
            var cadenaConexion = string.IsNullOrEmpty(configuracionEntidad.Conexion) && string.IsNullOrEmpty(configuracionMongo.ConexionDefault())
                ? configuracionMongo.ConexionDefault()
                : string.IsNullOrEmpty(configuracionEntidad.Conexion)
                    ? configuracionMongo.ConexionDefault()
                    : configuracionEntidad.Conexion;
            var client = new MongoClient(cadenaConexion);
            
            _db = MongoDbContextDisenoCurricular.Create(client.GetDatabase(configuracionEntidad.Esquema ));
            _dbSetFull = ((MongoDbContextDisenoCurricular)_db).Cursos;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextDisenoCurricular.NOMBRE_COLECCION_CURSOS}'");
            throw;
        }

    }

    private MongoDbContextDisenoCurricular DB { get { return (MongoDbContextDisenoCurricular)_db; } }
    public bool RequiereAutenticacion => true;

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioCurso-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioCurso-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioCurso-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioCurso-EntidadUpdateAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioCurso-EstableceContextoUsuarioAPI");
        this._contextoUsuario = contexto;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioCurso-ObtieneContextoUsuarioAPI");
        return this.ObtieneContextoUsuario();
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioCurso-InsertarAPI-{data}", data);
        var add = data.Deserialize<Curso>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCurso-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioCurso-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Curso>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioCurso-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioCurso-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioCurso-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioCurso-UnicaPorIdAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCurso-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioCurso-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCurso-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioCurso-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCurso-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioCurso-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegueAPI(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCurso-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => Curso

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Curso actualizacion, Curso original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Curso original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override async Task<ResultadoValidacion> ValidarInsertar(Curso data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override Curso ADTOFull(Curso data)
    {
        Curso curso = new()
        {
            Id = Guid.NewGuid(),
            EspacioTrabajoId = data.EspacioTrabajoId,
            Idiomas = data.Idiomas,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            Version = data.Version,

        };
        return curso;
    }

    public override Curso ADTOFull(Curso actualizacion, Curso actual)
    {
        actual.EspacioTrabajoId = actualizacion.EspacioTrabajoId;
        actual.Idiomas = actualizacion.Idiomas;
        actual.Nombre = actualizacion.Nombre;
        actual.Descripcion = actualizacion.Descripcion;
        actual.Version = actualizacion.Version;
        return actual;
    }


    public override Curso ADTODespliegue(Curso data)
    {
        Curso curso = new()
        {
            Id = data.Id,
            EspacioTrabajoId = data.EspacioTrabajoId,
            Idiomas = data.Idiomas,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            Version = data.Version,
        };

        return curso;
    }

    public override async Task<Respuesta> Actualizar(string id, Curso data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_CURSO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Curso actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_CURSO_NO_ENCONTRADA,
                    Mensaje = "No existe una CURSO con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarActualizar(id, data, actual);
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
                respuesta.Error!.Codigo = CodigosError.DISENOCURRICULAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioCurso-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<RespuestaPayload<Curso>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Curso>();
        try
        {
            Curso actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_CURSO_NO_ENCONTRADA,
                    Mensaje = "No existe una CURSO con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioCurso-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.DISENOCURRICULAR_CURSO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Curso actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_CURSO_NO_ENCONTRADA,
                    Mensaje = "No existe una CURSO con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.DISENOCURRICULAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioCurso-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public async Task<Respuesta> ActualizaDbSetCurso(Curso curso)
    {
        _logger.LogDebug("ServicioCurso - ActualizaContext {curso} ", curso);
        var respuesta = new Respuesta();

        Curso actual = _dbSetFull.Find(curso.Id);


        if (actual == null)
        {
            respuesta.Error = new ErrorProceso()
            {
                Mensaje = "Ocurrió un problema al actualizar la entidad Curso",
                Codigo = CodigosError.DISENOCURRICULAR_CURSO_ERROR_ACTUALIZAR,
                HttpCode = HttpCode.ServerError
            };
            respuesta.HttpCode = HttpCode.ServerError;
            return respuesta;
        }

        var entidad = ADTOFull(curso, actual);
        
        this._dbSetFull.Update(entidad);
        await _db.SaveChangesAsync();

        respuesta.Ok = true;
        _logger.LogDebug("ServicioCurso - ActualizaContext resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return