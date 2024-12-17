using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using evaluacion.model.evaluacion;
using evaluacion.model.evaluacion.temas;
using evaluacion.model.evaluacion.variantes;
using evaluacion.services.dbcontext;
using extensibilidad.metadatos;
using extensibilidad.metadatos.atributos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;
using System.Text.RegularExpressions;
using ZstdSharp.Unsafe;

namespace evaluacion.services.evaluacion;
[ServicioEntidadAPI(typeof(VarianteEvaluacion))]
public class ServicioVarianteEvaluacion: ServicioEntidadGenericaBase<VarianteEvaluacion, VarianteEvaluacionInsertar, VarianteEvaluacionActualizar, VarianteEvaluacionDespliegue, Guid>,
    IServicioEntidadAPI, IServicioVarianteEvaluacion
{
    private readonly ILogger<ServicioVarianteEvaluacion> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IDistributedCache _cache;
    private DbSet<Evaluacion> _dbSetEvaluacion;
    private Evaluacion _evaluacion;
    public ServicioVarianteEvaluacion(ILogger<ServicioVarianteEvaluacion> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector, IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        _cache = cache;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION);

        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION}'";
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


            _db = MongoDbContextEvaluacion.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetEvaluacion = ((MongoDbContextEvaluacion)_db).Evaluaciones;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicialzar mongo para '{MongoDbContextEvaluacion.NOMBRE_COLECCION_EVALUACION}'");
            throw;
        }
    }



    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-ActualizarAPI-{data}", data);
        var update = data.Deserialize<VarianteEvaluacionActualizar>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioReactivo-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros);
        _logger.LogDebug("ServicioReactivo-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-InsertarAPI-{data}", data);
        var add = data.Deserialize<VarianteEvaluacionInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioReactivo-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-PaginaAPI");
        var respuesta = new RespuestaPayload<PaginaGenerica<object>>();
        {
            respuesta.Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            };
        };
        _logger.LogDebug("ServicioReactivo-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-PaginaDespliegueAPI");
        var respuesta = new RespuestaPayload<PaginaGenerica<object>>();
        {
            respuesta.Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            };
        };
        _logger.LogDebug("ServicioReactivo-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-UnicaPorIdAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioReactivo-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioReactivo-UnicaPorIdDespliegueAPI");
        var respuesta = new RespuestaPayload<object>()
        {
            Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ACCION_NO_IMPLEMENTADA,
                Mensaje = "Acción no implementada",
                HttpCode = HttpCode.NotImplemented
            },
        };
        _logger.LogDebug("ServicioReactivo-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioReactivo-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioReactivo-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioReactivo-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioReactivo-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioReactivo-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioReactivo-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    #region Overrides para la entidad VarianteEvaluacion

    public async Task<ResultadoValidacion> ValidarInsertar(VarianteEvaluacionInsertar data, StringDictionary? parametros = null)
    {
        var resultado = new ResultadoValidacion();

        _evaluacion = _dbSetEvaluacion.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"])
                                                         && _.DominioId == Guid.Parse(this._contextoUsuario.DominioId)
                                                         && _.OUId == Guid.Parse(this._contextoUsuario.UOrgId));

        if (_evaluacion == null && _evaluacion.Estado != model.EstadoEvaluacion.Diseno)
        {
            resultado.Valido = false;
            resultado.Error = new ErrorProceso()
            {
                Codigo = CodigosError.EVALUACION_REACTIVO_ERROR_INSERTAR,
                Mensaje = "Verifique que exista la evaluación, dominio y UnidadOrganizacional para realizar la acción",
                HttpCode = HttpCode.NotFound
            };
            return resultado;
        }

        resultado.Valido = true;
        return resultado;
    }

    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, VarianteEvaluacionActualizar actualizacion, VarianteEvaluacion original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, VarianteEvaluacion original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override VarianteEvaluacion ADTOFull(VarianteEvaluacionInsertar data)
    {
        return new VarianteEvaluacion()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            CreadorId = _evaluacion.CreadorId
        };
    }

    public override VarianteEvaluacion ADTOFull(VarianteEvaluacionActualizar actualizacion, VarianteEvaluacion actual)
    {
        actual.Nombre = actualizacion.Nombre;
        return actual;
    }

    public override VarianteEvaluacionDespliegue ADTODespliegue(VarianteEvaluacion data)
    {
        return new VarianteEvaluacionDespliegue()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            CreadorId = data.CreadorId,
            FechaCreacion = data.FechaCreacion,
            TotalReactivos = data.TotalReactivos,
            TotalPuntos = data.TotalPuntos,
            TotalEjecuciones = data.TotalEjecuciones
        };
    }


    public override async Task<RespuestaPayload<VarianteEvaluacionDespliegue>> Insertar(VarianteEvaluacionInsertar data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<VarianteEvaluacionDespliegue>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data,parametros);
            if (resultadoValidacion.Valido)
            {

                if (data.Nombre != null)
                {
                    var varianteEva = _evaluacion.Variantes.Any(_ => _.Nombre == data.Nombre);

                    if (varianteEva)
                    {
                        respuesta.Error = new ErrorProceso()
                        {
                            Codigo = CodigosError.EVALUACION_VARIANTEEVALUACION_EXISTENTE,
                            Mensaje = "Se ha encontrado una variante evaluacion con el mismo nombre",
                            HttpCode = HttpCode.Conflict
                        };
                        respuesta.HttpCode = HttpCode.Conflict;
                        return respuesta;
                    }
                }

                if (string.IsNullOrEmpty(data.Nombre))
                {
                    var nombres = _evaluacion.Variantes.Where(o => o.Nombre.StartsWith(_evaluacion.Nombre + "-")).Select(_ => _.Nombre).ToList();

                    if (nombres.Count == 0)
                    {
                        data.Nombre = _evaluacion.Nombre + "-" + 1;
                    }
                    else
                    {
                        List<int> numeros = new List<int>();

                        foreach (var nombre in nombres)
                        {
                            if (nombre.Length > 1 && int.TryParse(nombre.Split('-')[1], out int numero))
                            {
                                numeros.Add(numero);
                            }
                        }

                        var maximo = numeros.Max();

                        data.Nombre = _evaluacion + "-" + maximo;
                    }
                }

                var entidad = ADTOFull(data);
                _evaluacion.Variantes.Add(entidad);
                _evaluacion.TotalVariantes++;

                _dbSetEvaluacion.Update(_evaluacion);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);

            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioVarianteEvaluacion-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }



    public async Task<Respuesta> Actualizar(Guid id, VarianteEvaluacionActualizar data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (id == Guid.Empty || data == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_VARIANTEEVALUACION_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No se ha proporcionado Id o Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            _evaluacion = _dbSetEvaluacion.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"])
                                                             && _.DominioId == Guid.Parse(this._contextoUsuario.DominioId)
                                                             && _.OUId == Guid.Parse(this._contextoUsuario.UOrgId));

            if (_evaluacion == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_NO_ENCONTRADA,
                    Mensaje = "No se ha encontrado la Evaluacion",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var varianteEvaActual = _evaluacion.Variantes.FirstOrDefault(_ => _.Id == data.Id);

            if(varianteEvaActual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_VARIANTEEVALUACION_NO_ENCONTRADA,
                    Mensaje = "No se ha encontrado la VarianteEvaluacion",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var entidad = ADTOFull(data, varianteEvaActual);

            _dbSetEvaluacion.Update(_evaluacion);
            await _db.SaveChangesAsync();
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioVarianteEvaluacion-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public virtual async Task<Respuesta> Eliminar(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {

            if (id == Guid.Empty)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_VARIANTEEVALUACION_ID_NO_INGRESADO,
                    Mensaje = "No se ha proporcionado Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            _evaluacion = _dbSetEvaluacion.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"])
                                                 && _.DominioId == Guid.Parse(this._contextoUsuario.DominioId)
                                                 && _.OUId == Guid.Parse(this._contextoUsuario.UOrgId));


            var varianteEvaActual = _evaluacion.Variantes.FirstOrDefault(_ => _.Id == id);

            if (varianteEvaActual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.EVALUACION_VARIANTEEVALUACION_NO_ENCONTRADA,
                    Mensaje = "No se ha encontrado la VarianteEvaluacion",
                    HttpCode = HttpCode.NotFound,
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            _evaluacion.Variantes.Remove(varianteEvaActual);
            _evaluacion.TotalVariantes--;

            _dbSetEvaluacion.Update(_evaluacion);
            await _db.SaveChangesAsync();
            
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioVarlianteEvaluacion-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.EVALUACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public Task<RespuestaPayload<VarianteEvaluacion>> UnicaPorId(Guid id, StringDictionary? parametros)
    {
        throw new NotImplementedException();
    }

    public Task<RespuestaPayload<VarianteEvaluacionDespliegue>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros)
    {
        throw new NotImplementedException();
    }
    #endregion
}
