#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas.configuracion.mongo;
using comunes.primitivas;
using disenocurricular.model;
using disenocurricular.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Collections.Specialized;

namespace disenocurricular.services.curso.plan;
[ServicioEntidadAPI(typeof(Plan))]
public class ServicioPlan : ServicioEntidadHijoGenericaBase<Plan, Plan, Plan, Plan, string>,
    IServicioEntidadHijoAPI, IServicioPlan
{
    private readonly ILogger<ServicioPlan> _logger;
    private readonly IServicioCurso servicioCurso;
    private readonly IReflectorEntidadesAPI _reflector;
    private Curso? curso;


    public ServicioPlan(ILogger<ServicioPlan> logger, IServicionConfiguracionMongo configuracionMongo, IServicioCurso servicioCurso,
        IReflectorEntidadesAPI reflector, IDistributedCache distributedCache) : base(null, null, logger, reflector, distributedCache)
    {
        this._logger = logger;
        this._reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();
        this.servicioCurso = servicioCurso;

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextDisenoCurricular.NOMBRE_COLECCION_PLANES);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextDisenoCurricular.NOMBRE_COLECCION_PLANES}'";
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

            _db = MongoDbContextDisenoCurricular.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextDisenoCurricular)_db).Planes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextDisenoCurricular.NOMBRE_COLECCION_PLANES}'");
            throw;
        }

    }

    private MongoDbContextDisenoCurricular DB { get { return (MongoDbContextDisenoCurricular)_db; } }
    public bool RequiereAutenticacion => true;

    string IServicioEntidadHijoAPI.TipoPadreId
    {
        get => this.TipoPadreId;
        set => this.TipoPadreId = value;
    }

    string IServicioEntidadHijoAPI.Padreid
    {
        get => this.curso.Id.ToString() ?? null;
        set => EstableceDbSet(value);
    }


    public async void EstableceDbSet(string padreId)
    {
        _logger.LogDebug("ServicioEspecialidad - EstableceDbSet {padreId}", padreId);
        var entidadCurso = await this.servicioCurso.UnicaPorId(padreId);
        curso = (Curso)entidadCurso.Payload;
        this.Padreid = curso != null ? curso.Id.ToString() : null;
        _logger.LogDebug("ServicioEspecialidad - EstableceDbSet - resultado {padreId}", this.Padreid);
    }


    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioPlan-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioPlan-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioPlan-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioPlan-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioPlan-ContextoUsuarioAPI");
        this._contextoUsuario = contexto;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioPlan-ObtieneContextoUsuarioAPI");
        return this.ObtieneContextoUsuario();
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlan-InsertarAPI-{data}", data);
        var add = data.Deserialize<Plan>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPlan-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlan-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Plan>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioPlan-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlan-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioPlan-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlan-UnicaPorIdAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPlan-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlan-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPlan-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlan-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPlan-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPlan-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPlan-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => Curso

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Plan actualizacion, Plan original)
    {
        var resultado = new ResultadoValidacion();
        var existeCurso = this.UnicaPorId(actualizacion.CursoId.ToString());

        if (existeCurso == null)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.DISENOCURRICULAR_PLAN_ERROR_VALIDACION_CURSO_NOENCONTRADO,
                Mensaje = "No existe un Curso al que pertenezca el Plan para poder Actualizarlo",
                HttpCode = HttpCode.BadRequest
            };
            resultado.Valido = false;
            return resultado;
        }
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Plan original)
    {
        var resultado = new ResultadoValidacion();
        var existeCurso = this.UnicaPorId(original.CursoId.ToString());

        if (existeCurso == null)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.DISENOCURRICULAR_PLAN_ERROR_VALIDACION_CURSO_NOENCONTRADO,
                Mensaje = "No existe un Curso al que pertenezca el Plan para Eliminar",
                HttpCode = HttpCode.BadRequest
            };
            resultado.Valido = false;
            return resultado;
        }
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarInsertar(Plan data)
    {
        var resultado = new ResultadoValidacion();
        var existeCurso = this.UnicaPorId(data.CursoId.ToString());

        if (existeCurso == null)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.DISENOCURRICULAR_PLAN_ERROR_VALIDACION_CURSO_NOENCONTRADO,
                Mensaje = "No existe un Curso al que pertenezca el Plan",
                HttpCode = HttpCode.BadRequest
            };
            resultado.Valido = false;
            return resultado;
        }
        resultado.Valido = true;
        return resultado;
    }

    public override Plan ADTOFull(Plan data)
    {
        Plan plan = new()
        {
            Id = Guid.NewGuid(),
            EspacioTrabajoId = data.EspacioTrabajoId,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            Version = data.Version,
            Periodicidad = data.Periodicidad
        };
        return plan;
    }

    public override Plan ADTOFull(Plan actualizacion, Plan actual)
    {
        actual.EspacioTrabajoId = actualizacion.EspacioTrabajoId;
        actual.Nombre = actualizacion.Nombre;
        actual.Descripcion = actualizacion.Descripcion;
        actual.Version = actualizacion.Version;
        actual.Periodicidad = actualizacion.Periodicidad;
        return actual;
    }


    public override Plan ADTODespliegue(Plan data)
    {
        Plan plan = new()
        {
            Id = data.Id,
            EspacioTrabajoId = data.EspacioTrabajoId,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            Version = data.Version,
            Periodicidad = data.Periodicidad,
            Periodos = data.Periodos
        };

        return plan;
    }

    public override async Task<RespuestaPayload<Plan>> Insertar(Plan data)
    {
        var respuesta = new RespuestaPayload<Plan>();
        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);

                this.curso?.PlanesEstudio.Add(entidad.Id);
                var updateCurso = await this.servicioCurso.ActualizaDbSetCurso(curso);

                if(updateCurso.Ok == true)
                {
                    _dbSetFull?.Add(entidad);
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
            _logger.LogError(ex, "ServicioPlan-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    public override async Task<Respuesta> Actualizar(string id, Plan data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PLAN_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Plan actual = _dbSetFull!.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PLAN_NO_ENCONTRADA,
                    Mensaje = "No existe un Plan con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioPlan-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<RespuestaPayload<Plan>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Plan>();
        try
        {
            Plan actual = await _dbSetFull!.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PLAN_NO_ENCONTRADA,
                    Mensaje = "No existe un Plan con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioPlan-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.DISENOCURRICULAR_PLAN_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Plan actual = _dbSetFull!.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PLAN_NO_ENCONTRADA,
                    Mensaje = "No existe un Plan con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                curso!.PlanesEstudio.Remove(actual.Id);

                var updateCurso = await this.servicioCurso.ActualizaDbSetCurso(curso);

                if (updateCurso.Ok == true)
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
            _logger.LogError(ex, "ServicioPlan-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<PaginaGenerica<Plan>>> Pagina(Consulta consulta)
    {
        RespuestaPayload<PaginaGenerica<Plan>> respuesta = new();
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
            _logger.LogError(ex, "ServicioPlan - Pagina {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<PaginaGenerica<Plan>>> PaginaDespliegue(Consulta consulta)
    {
        RespuestaPayload<PaginaGenerica<Plan>> respuesta = new RespuestaPayload<PaginaGenerica<Plan>>();

        try
        {
            var resultado = await Pagina(consulta);

            respuesta.Ok = resultado.Ok;

            if (resultado.Ok)
            {
                PaginaGenerica<Plan> pagina = new()
                {
                    ConsultaId = Guid.NewGuid().ToString(),
                    Elementos = new List<Plan>(),
                    Milisegundos = 0,
                    Paginado = new Paginado() { Indice = 0, Tamano = ((PaginaGenerica<Plan>)resultado.Payload).Paginado.Tamano, Ordenamiento = consulta.Paginado.Ordenamiento, ColumnaOrdenamiento = consulta.Paginado.ColumnaOrdenamiento },
                    Total = ((PaginaGenerica<Plan>)resultado.Payload).Total
                };

                foreach (Plan item in ((PaginaGenerica<Plan>)resultado.Payload).Elementos)
                {
                    pagina.Elementos.Add(ADTODespliegue(item));
                }
                respuesta.Payload = pagina;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioPlan - PaginaDespliegue {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<PaginaGenerica<Plan>> ObtienePaginaElementos(Consulta consulta)
    {
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Plan));
        var Elementos = Enumerable.Empty<Plan>().AsQueryable();

        if (consulta.Filtros?.Count > 0)
        {
            var predicateBody = interpreteConsulta?.CrearConsultaExpresion<Plan>(consulta, entidad);

            if (predicateBody != null)
            {
                var RConsulta = _dbSetFull?.AsQueryable().Provider.CreateQuery<Plan>(predicateBody.getWhereExpression(_dbSetFull.AsQueryable().Expression));

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
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.


