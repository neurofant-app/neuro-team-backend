using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using disenocurricular.model;
using disenocurricular.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.AspNetCore.Mvc.Diagnostics;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;


namespace disenocurricular.services.curso.plan.periodo;
[ServicioEntidadAPI(typeof(Periodo))]
public class ServicioPeriodo : ServicioEntidadHijoGenericaBase<Periodo, Periodo, Periodo, Periodo, string>,
    IServicioEntidadHijoAPI, IServicioPeriodo
{
    private readonly ILogger<Periodo> logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private Plan? plan;
    private DbSet<Plan> _dbSetPlanes;

    public ServicioPeriodo(ILogger<ServicioPeriodo> logger, 
        IServicionConfiguracionMongo configuracionMongo,  
        IReflectorEntidadesAPI reflector, IDistributedCache cache) :
        base (null,null,logger, reflector, cache)
    {
        this._logger = logger;
        this._reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();
        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextDisenoCurricular.NOMBRE_COLECCION_PLANES);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextDisenoCurricular.NOMBRE_COLECCION_PLANES}'";
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
            _dbSetPlanes = ((MongoDbContextDisenoCurricular)_db).Planes;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar mongo para {coleccion}'", MongoDbContextDisenoCurricular.NOMBRE_COLECCION_PLANES);
            throw;
        }
    }

    private MongoDbContextDisenoCurricular DB { get { return (MongoDbContextDisenoCurricular)_db; } }

    public bool RequiereAutenticacion => true;

    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }

    string IServicioEntidadHijoAPI.Padreid { get => this.plan.Id.ToString() ?? null; set => EstableceDbSet(value); }
    public void EstableceDbSet(string padreId)
    {
        _logger.LogDebug("ServicioPeriodo-EstableceDbSet - {padreId}", padreId);
        plan = _dbSetPlanes.FirstOrDefault(_ => _.Id == new Guid(padreId));
        this.Padreid = plan != null ? plan.Id.ToString() : null;
        _logger.LogDebug("ServicioPeriodo-EstableceDbSet - resultado {padreId}", this.Padreid);
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioPeriodo-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioPeriodo-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioPeriodo-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioPeriodo-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioPeriodo-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }


    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPeriodo-InsertarAPI-{data}", data);
        var add = data.Deserialize<Periodo>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPeriodo-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPeriodo-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Periodo>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioPeriodo-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPeriodo-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioPeriodo-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioPeriodo-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPeriodo-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPeriodo-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPeriodo-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPeriodo-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPeriodo-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPeriodo-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioPeriodo-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioPeriodo-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => Tema
    public override async Task<ResultadoValidacion> ValidarInsertar(Periodo data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Periodo original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Periodo actualizacion, Periodo original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override Periodo ADTOFull(Periodo actualizacion, Periodo actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.AntecesorId = actualizacion.AntecesorId;
        actual.SucesorId = actualizacion.SucesorId;
        actual.AceptaEspecialidad = actualizacion.AceptaEspecialidad;
        actual.TemariosLibres = actualizacion.TemariosLibres;
        actual.TemariosObligatorios = actualizacion.TemariosObligatorios;
        actual.TipoSeleccionObligatorios = actualizacion.TipoSeleccionObligatorios;
        actual.MinimoTemariosObligatorios = actualizacion.MinimoTemariosObligatorios;
        actual.TemariosOpcionales = actualizacion.TemariosOpcionales;
        actual.TipoSeleccionOpcionales = actualizacion.TipoSeleccionOpcionales;
        actual.MinimoTemariosOpcionales = actualizacion.MinimoTemariosOpcionales;
        actual.MinimoCreditos = actualizacion.MinimoCreditos;
        actual.MaximoCreditos = actualizacion.MaximoCreditos;
        actual.MinimoTemarios = actualizacion.MinimoTemarios;
        actual.MaximoTemarios = actualizacion.MaximoTemarios;
        return actual;
    }

    public override Periodo ADTOFull(Periodo data)
    {
        Periodo periodo = new Periodo()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            AntecesorId = data.AntecesorId,
            SucesorId = data.SucesorId,
            AceptaEspecialidad = data.AceptaEspecialidad,
            TemariosLibres = data.TemariosLibres,
            TemariosObligatorios = data.TemariosObligatorios,
            TipoSeleccionObligatorios = data.TipoSeleccionObligatorios,
            MinimoTemariosObligatorios = data.MinimoTemariosObligatorios,
            TemariosOpcionales = data.TemariosOpcionales,
            TipoSeleccionOpcionales = data.TipoSeleccionOpcionales,
            MinimoTemariosOpcionales = data.MinimoTemariosOpcionales,
            MinimoCreditos = data.MinimoCreditos,
            MaximoCreditos = data.MaximoCreditos,
            MinimoTemarios = data.MinimoTemarios,
            MaximoTemarios = data.MaximoTemarios
            
        };
        return periodo;
    }

    public override Periodo ADTODespliegue(Periodo data)
    {
        Periodo periodo = new Periodo()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            AntecesorId = data.AntecesorId,
            SucesorId = data.SucesorId,
            AceptaEspecialidad = data.AceptaEspecialidad,
            TemariosLibres = data.TemariosLibres,
            TemariosObligatorios = data.TemariosObligatorios,
            TipoSeleccionObligatorios = data.TipoSeleccionObligatorios,
            MinimoTemariosObligatorios = data.MinimoTemariosObligatorios,
            TemariosOpcionales = data.TemariosOpcionales,
            TipoSeleccionOpcionales = data.TipoSeleccionOpcionales,
            MinimoTemariosOpcionales = data.MinimoTemariosOpcionales,
            MinimoCreditos = data.MinimoCreditos,
            MaximoCreditos = data.MaximoCreditos,
            MinimoTemarios = data.MinimoTemarios,
            MaximoTemarios = data.MaximoTemarios
        };
        return periodo;
    }

    public override async Task<RespuestaPayload<Periodo>> Insertar(Periodo data)
    {
        var respuesta = new RespuestaPayload<Periodo>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                plan.Periodos.Add(entidad);
                _dbSetPlanes.Update(plan);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
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
            _logger.LogError(ex, "ServicioPeriodo-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, Periodo data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PERIODO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Periodo actual = plan.Periodos.FirstOrDefault(_ => _.Id == Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PERIODO_NO_ENCONTRADA,
                    Mensaje = "No existe un Periodo con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = plan.Periodos.IndexOf(entidad);
                if (index == 0)
                {
                    plan.Periodos[0] = entidad;
                    _dbSetPlanes.Update(plan);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.DISENOCURRICULAR_PERIODO_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar el Periodo",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
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
            _logger.LogError(ex, "ServicioPeriodo-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<Periodo>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Periodo>();
        try
        {
            Periodo actual = plan.Periodos.FirstOrDefault(_ => _.Id == Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_TEMA_NO_ENCONTRADA,
                    Mensaje = "No existe un Periodo con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioPeriodo-UnicaPorId {msg}", ex.Message);
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
                    Codigo = CodigosError.DISENOCURRICULAR_PERIODO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Periodo actual = _dbSetFull!.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PERIODO_NO_ENCONTRADA,
                    Mensaje = "No existe un Periodo con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                plan.Periodos.Remove(actual);
                _dbSetPlanes.Update(plan);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.DISENOCURRICULAR_PERIODO_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR el Periodo",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioPeriodo-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.DISENOCURRICULAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<Periodo>> ObtienePaginaElementos(Consulta consulta)
    {
        _logger.LogDebug("ServicioPeriodo - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Periodo));
        var Elementos = Enumerable.Empty<Periodo>().AsQueryable();
        if (plan != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<Periodo>(consulta, entidad);

                if (predicateBody != null)
                {
                    var RConsulta = plan.Periodos.AsQueryable().Provider.CreateQuery<Periodo>(predicateBody.getWhereExpression(plan.Periodos.AsQueryable().Expression));

                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = plan.Periodos.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

            }
        }
        return Elementos.Paginado(consulta);
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return
