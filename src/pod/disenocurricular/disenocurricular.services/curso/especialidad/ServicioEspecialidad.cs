#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
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
using System.Collections.Specialized;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace disenocurricular.services.curso.especialidad;
[ServicioEntidadAPI(entidad:typeof(Especialidad))]
public class ServicioEspecialidad : ServicioEntidadGenericaBase<Especialidad,Especialidad,Especialidad,Especialidad,string>,
    IServicioEntidadAPI, IServicioEspecialidad
{
    private readonly ILogger<ServicioEspecialidad> _logger;
    private readonly IServicioCurso servicioCurso;
    private readonly IReflectorEntidadesAPI _reflector;
    private Curso? curso;

    public ServicioEspecialidad(ILogger<ServicioEspecialidad> logger,
        IServicionConfiguracionMongo configuracionMongo, IServicioCurso servicioCurso,
        IReflectorEntidadesAPI reflector,
        IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        this._logger = logger;
        this.servicioCurso = servicioCurso;
        this._reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextDisenoCurricular.NOMBRE_COLECCION_ESPECIALIDADES);
        if(configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextDisenoCurricular.NOMBRE_COLECCION_ESPECIALIDADES}'";
            _logger.LogError(err);
            throw new Exception(err);
        }
        try
        {
            _logger.LogDebug($"Mongo DB {configuracionEntidad.Esquema} coleccion {configuracionEntidad.Esquema} utilizando conexion default {string.IsNullOrEmpty(configuracionEntidad.Conexion)}");
            var cadenaConexion = string.IsNullOrEmpty(configuracionEntidad.Conexion) && string.IsNullOrEmpty(configuracionMongo.ConexionDefault())
                ? configuracionMongo.ConexionDefault()
                : string.IsNullOrEmpty(configuracionEntidad.Conexion)
                    ? configuracionMongo.ConexionDefault()
                    : configuracionEntidad.Conexion;
            var client = new MongoClient(cadenaConexion);

            _db = MongoDbContextDisenoCurricular.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextDisenoCurricular)_db).Especialidades;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar mongo para {coleccion}'", MongoDbContextDisenoCurricular.NOMBRE_COLECCION_ESPECIALIDADES);
            throw;
        }
    }

    private MongoDbContextDisenoCurricular DB
    {
        get
        {
            return (MongoDbContextDisenoCurricular)_db;
        }
    }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEspecialidad - ActualizarAPI - {data}", data);
        var update = data.Deserialize<Especialidad>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioEspecialidad - ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioEspecialidad - EliminarAPI - {id}", id);
        Respuesta respuesta = await this.Eliminar((string) id, parametros, forzarEliminacion);
        _logger.LogDebug("ServicioEspecialidad - EliminarAPI - resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEspecialidad - EntidadDespliegueAPI");
        return this.EntidadDespliegueAPI();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEspecialidad - EntidadInsertAPI");
        return this.EntidadInsertAPI();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEspecialidad - EntidadRepoAPI");
        return this.EntidadRepoAPI();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEspecialidad - EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEspecialidad - EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEspecialidad - ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEspecialidad - InsertarAPI {data}", data);
        var add = data.Deserialize<Especialidad>(JsonAPIDefaults());
        var insert = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(insert));
        _logger.LogDebug("ServicioEspecialidad - InsertarAPI {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEspecialidad - UnicaPodId - {id}", id);
        var unica = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta= JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(unica));
        _logger.LogDebug("ServicioEspecialidad - UnicaPorId - resultado {ok} {code} {error}");
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEspecialidad - UnicaPorIdDespliegueAPI - {id}", id);
        var unicaDespliegue = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(unicaDespliegue));
        _logger.LogDebug("ServicioEspecialidad - UnicaPorIdDespliegueAPI - resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEspecialidad - PaginaAPI - {consulta}", consulta);
        var consult = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(consult));
        _logger.LogDebug("ServicioEspecialidad - PaginaAPI - resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEspecialidad - PaginaDespliegueAPI {consulta}", consulta);
        var consult = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(consult));
        _logger.LogDebug("ServicioEspecialidad - PaginaDespliegueAPI - resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la EntidadGenericaHijo - Especialidad

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Especialidad actualizacion, Especialidad original)
    {
        var resultado = new ResultadoValidacion();
        var existeCurso = this.servicioCurso.UnicaPorId(actualizacion.CursoId.ToString());

        if (existeCurso.Result.Ok == false)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_ERROR_VALIDACION_CURSO_NOENCONTRADO,
                Mensaje = "No existe un Curso al que pertenezca la Especialidad",
                HttpCode = HttpCode.BadRequest
            };
            resultado.Valido = false;
            return resultado;
        }
        curso = (Curso)existeCurso.Result.Payload;
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Especialidad original, bool forzarEliminacion = false)
    {
        var resultado = new ResultadoValidacion();
        var existeCurso = this.servicioCurso.UnicaPorId(original.CursoId.ToString());

        if (existeCurso.Result.Ok == false)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_ERROR_VALIDACION_CURSO_NOENCONTRADO,
                Mensaje = "No existe un Curso al que pertenezca la Especialidad",
                HttpCode = HttpCode.BadRequest
            };
            resultado.Valido = false;
            return resultado;
        }
        curso = (Curso)existeCurso.Result.Payload;
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarInsertar(Especialidad data)
    {

        var resultado = new ResultadoValidacion();
        var existeCurso = this.servicioCurso.UnicaPorId(data.CursoId.ToString());

        if (existeCurso.Result.Ok == false)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_ERROR_VALIDACION_CURSO_NOENCONTRADO,
                Mensaje = "No existe un Curso al que pertenezca la Especialidad",
                HttpCode = HttpCode.BadRequest
            };
            resultado.Valido = false;
            return resultado;
        }
        curso = (Curso)existeCurso.Result.Payload;
        resultado.Valido = true;
        return resultado;
    }

    public override Especialidad ADTOFull(Especialidad actualizacion, Especialidad actual)
    {
        actual.CursoId = actualizacion.CursoId;
        actual.Nombre = actualizacion.Nombre;
        actual.Descripcion = actualizacion.Descripcion;
        actual.TemariosOpcionales = actualizacion.TemariosOpcionales;
        actual.TemariosObligatorios = actualizacion.TemariosObligatorios;
        actual.MinimoCreditos = actualizacion.MinimoCreditos;
        actual.MaximoCreditos = actualizacion.MaximoCreditos;
        return actual;
    }

    public override Especialidad ADTOFull(Especialidad data)
    {
        Especialidad especialidad = new Especialidad()
        {
            Id = Guid.NewGuid(),
            CursoId = data.CursoId,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            TemariosObligatorios = data.TemariosObligatorios,
            TemariosOpcionales = data.TemariosOpcionales,
            MinimoCreditos = data.MinimoCreditos,
            MaximoCreditos = data.MaximoCreditos
        };

        return especialidad;
    }

    public override Especialidad ADTODespliegue(Especialidad data)
    {
        Especialidad especialidad = new Especialidad()
        {
            Id = data.Id,
            CursoId = data.CursoId,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            TemariosObligatorios = data.TemariosObligatorios,
            TemariosOpcionales = data.TemariosOpcionales,
            MinimoCreditos = data.MinimoCreditos,
            MaximoCreditos = data.MaximoCreditos
        };
        return especialidad;
    }

    public override async Task<RespuestaPayload<Especialidad>> Insertar(Especialidad data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Especialidad>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                curso?.Especialidades.Add(entidad.Id);

                var updateCurso = await this.servicioCurso.ActualizaDbSetCurso(curso);

                if (updateCurso.Ok == true)
                {
                    _dbSetFull.Add(entidad);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                    respuesta.Payload = ADTODespliegue(entidad);
                }
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
            _logger.LogError(ex, "ServicioEspecialidad-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, Especialidad data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id) || data == null)
            {
                respuesta.Error = new()
                {
                    Mensaje = "No se ha proporcionado Id ó Paylaod",
                    Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_ID_PAYLOAD_NO_INGRESADO,
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Especialidad actual = _dbSetFull!.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new()
                {
                    Mensaje = "No se encontró una Especialidad con el Id proporcionado",
                    Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_ID_NO_INGRESADO,
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarActualizar(id, data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                _dbSetFull!.Update(entidad);
                
                await _db!.SaveChangesAsync();

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
            _logger.LogError(ex,"ServicioEspecialidad - Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                respuesta.Error = new()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_ID_NO_INGRESADO,
                    HttpCode = HttpCode.BadRequest,
                    Mensaje = "No se ha proporcionado un Id"
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Especialidad actual = _dbSetFull!.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_NO_ENCONTRADA,
                    Mensaje = "No se encontró una Especialidad con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                curso!.Especialidades.Remove(actual.Id);

                var updateCurso = await this.servicioCurso.ActualizaDbSetCurso(curso);

                if(updateCurso.Ok == true)
                {
                    _dbSetFull!.Remove(actual);
                    await _db!.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
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
            _logger.LogError("ServicioEspecialidad - Eliminar  {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    public override async Task<RespuestaPayload<Especialidad>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Especialidad>();
        try
        {
            Especialidad actual = await _dbSetFull!.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_ESPECIALIDAD_NO_ENCONTRADA,
                    HttpCode = HttpCode.NotFound,
                    Mensaje = "No se encontró una Especialidad con el Id proporcionado"
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
            _logger.LogError(ex, "ServicioEspecialidad - UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<PaginaGenerica<Especialidad>>> Pagina(Consulta consulta, StringDictionary? parametros = null)
    {
        RespuestaPayload<PaginaGenerica<Especialidad>> respuesta = new();
        try
        {
            if (interpreteConsulta == null)
            {
                respuesta.HttpCode = HttpCode.UnprocessableEntity;
                respuesta.Error = new ErrorProceso() { HttpCode = HttpCode.UnprocessableEntity, Codigo = CodigosError.DISENOCURRICULAR_SIN_INTERPRETE_CONSULTA, Mensaje = CodigosError.DISENOCURRICULAR_NO_HAY_INTERPRETE_CONSULTA };
                return respuesta;
            }

            var pagina = await ObtienePaginaElementos(consulta);

            respuesta.Payload = pagina;
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"ServicioEspecialidad - Pagina {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<PaginaGenerica<Especialidad>>> PaginaDespliegue(Consulta consulta, StringDictionary? parametros = null)
    {
        RespuestaPayload<PaginaGenerica<Especialidad>> respuesta = new RespuestaPayload<PaginaGenerica<Especialidad>>();

        try
        {
            var resultado = await Pagina(consulta, parametros);

            respuesta.Ok = resultado.Ok;

            if (resultado.Ok)
            {
                PaginaGenerica<Especialidad> pagina = new()
                {
                    ConsultaId = Guid.NewGuid().ToString(),
                    Elementos = new List<Especialidad>(),
                    Milisegundos = 0,
                    Paginado = new Paginado() { Indice = 0, Tamano = ((PaginaGenerica<Especialidad>)resultado.Payload).Paginado.Tamano, Ordenamiento = consulta.Paginado.Ordenamiento, ColumnaOrdenamiento = consulta.Paginado.ColumnaOrdenamiento },
                    Total = ((PaginaGenerica<Especialidad>)resultado.Payload).Total
                };

                foreach (Especialidad item in ((PaginaGenerica<Especialidad>)resultado.Payload).Elementos)
                {
                    pagina.Elementos.Add(ADTODespliegue(item));
                }
                respuesta.Ok = true;
                respuesta.Payload = pagina;
                respuesta.HttpCode = HttpCode.Ok;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex,"ServicioEspecialidad - PaginaDespliegue {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<PaginaGenerica<Especialidad>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Especialidad));
        var Elementos = Enumerable.Empty<Especialidad>().AsQueryable();

        if (consulta.Filtros?.Count > 0)
        {
            var predicateBody = interpreteConsulta?.CrearConsultaExpresion<Especialidad>(consulta, entidad);

            if (predicateBody != null)
            {
                var RConsulta = _dbSetFull?.AsQueryable().Provider.CreateQuery<Especialidad>(predicateBody.getWhereExpression(_dbSetFull.AsQueryable().Expression));

                Elementos = RConsulta?.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        else
        {
            var RConsulta = _dbSetFull?.AsQueryable();
            Elementos = RConsulta?.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

        }
        return await Elementos.PaginadoAsync(consulta);
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return