#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using extensibilidad.metadatos;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using comunes.primitivas;
using apigenerica.model.servicios;
using aplicaciones.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using comunes.primitivas.configuracion.mongo;
using aplicaciones.services.dbcontext;
using MongoDB.Driver;

namespace aplicaciones.services.aplicacion;
[ServicioEntidadAPI(entidad: typeof(EntidadAplicacion))]
public class ServicioAplicacion : ServicioEntidadGenericaBase<EntidadAplicacion, CreaAplicacion, ActualizaAplicacion, ConsultaAplicacion, string>,
   IServicioEntidadAPI, IServicioAplicacion
{
    private readonly ILogger _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private readonly IDistributedCache cache;

    public ServicioAplicacion(ILogger<ServicioAplicacion> logger, 
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;
        this.cache = cache;

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextAplicaciones.NOMBRE_COLECCION_APLICACION);
        if(configuracionEntidad == null)
        {
            string err = $"No existe configuración de mongo para '{MongoDbContextAplicaciones.NOMBRE_COLECCION_APLICACION}'";
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

            _db = MongoDbContextAplicaciones.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextAplicaciones)_db).Aplicaciones;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextAplicaciones.NOMBRE_COLECCION_APLICACION}'");
            throw;
        }
    }

    private MongoDbContextAplicaciones DB { get { return (MongoDbContextAplicaciones)_db; } }
    public bool RequiereAutenticacion => true;

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioAplicacion-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioAplicacion-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioAplicacion-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioAplicacion-DespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioAplicacion-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioAplicacion-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioAplicacion-InsertarAPI-{data}", data);
        var add = data.Deserialize<CreaAplicacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioAplicacion-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error );
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioAplicacion-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaAplicacion>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioAplicacion-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioAplicacion-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("SevicioAplicacion-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioAplicacion-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("SevicioAplicacion-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioAplicacion-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("SevicioAplicacion-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioAplicacion-PaginaAPI-{consulta}",consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("SevicioAplicacion-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioAplicacion-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("SevicioAplicacion-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    #region Overrides para la personalización de la entidad Aplicacion
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaAplicacion data)
    {
        ResultadoValidacion resultado = new ResultadoValidacion() { Error = new ErrorProceso() };

        var apps = await DB.Aplicaciones
            .Where(x => x.Clave.ToLower() == data.Clave.ToLower())
            .ToListAsync();

        resultado.Valido = apps.Count == 0;
        if (!resultado.Valido)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = "409",
                Mensaje = "Ya existe una aplicación con la misma clave",
                HttpCode = HttpCode.Conflict
            };
        }

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadAplicacion original)
    {
        ResultadoValidacion resultado = new() { Error = new ErrorProceso() };
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaAplicacion actualizacion, EntidadAplicacion original)
    {
        ResultadoValidacion resultado = new ResultadoValidacion() { Error = new ErrorProceso() };

        var apps = await DB.Aplicaciones
            .Where(x => x.Clave.ToLower() == actualizacion.Clave.ToLower() && x.Id == original.Id)
            .ToListAsync();

        resultado.Valido = apps.Count <= 1;
        if (!resultado.Valido)
        {
            resultado.Error = new ErrorProceso()
            {
                Codigo = "409",
                Mensaje = "Ya existe una aplicación con la misma clave",
                HttpCode = HttpCode.Conflict
            };
        }


        return resultado;
    }

    public override EntidadAplicacion ADTOFull(ActualizaAplicacion actualizacion, EntidadAplicacion actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Activa = actualizacion.Activa;
        actual.Clave = actualizacion.Clave;
        actual.Hosts = actualizacion.Hosts;
        actual.Default = actualizacion.Default;
        return actual;
    }

    public override EntidadAplicacion ADTOFull(CreaAplicacion data)
    {
        EntidadAplicacion aplicacion = new EntidadAplicacion()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            Activa = data.Activa,
            Hosts = data.Hosts,
            Clave = data.Clave,
            Default = data.Default,
        };
        return aplicacion;
    }

    public override ConsultaAplicacion ADTODespliegue(EntidadAplicacion data)
    {

        ConsultaAplicacion aplicacion = new ConsultaAplicacion()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Activa = data.Activa,
            Plantillas = data.Plantillas,
            Logotipos = data.Logotipos,
            Consentimientos = data.Consentimientos,
            Clave =   data.Clave,
            Default = data.Default
        };
        return aplicacion;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaAplicacion data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APPLICACION_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadAplicacion actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APPLICACION_NO_ENCONTRADA,
                    Mensaje = "No existe una aplicación con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                
                if (entidad.Default == true)
                {
                    var aplicaciones = await DB.Aplicaciones.Where(x => x.Default == true).ToListAsync();

                    foreach (var aplicacion in aplicaciones)
                    {
                        aplicacion.Default = false;
                        _dbSetFull.Update(aplicacion);
                        await _db.SaveChangesAsync();
                    }
                }

                _dbSetFull.Update(entidad);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.APPLICACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioAplicacion-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    public override async Task<RespuestaPayload<EntidadAplicacion>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<EntidadAplicacion>();
        try
        {
            EntidadAplicacion actual = await _dbSetFull.FindAsync(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APPLICACION_NO_ENCONTRADA,
                    Mensaje = "No existe una aplicación con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            actual.Plantillas = await DB.PlantillaInvitaciones
                .Where(x => x.AplicacionId == actual.Id)
                .ToListAsync();
            actual.Logotipos = await DB.LogoAplicaciones
                .Where(x => x.AplicacionId == actual.Id)
                .ToListAsync();
            actual.Consentimientos = await DB.Consentimientos
                .Where(x => x.AplicacionId == actual.Id)
                .ToListAsync();

            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioAplicacion-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.APPLICACION_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadAplicacion actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APPLICACION_NO_ENCONTRADA,
                    Mensaje = "No existe una aplicación con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.APPLICACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioAplicacion-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    public override async Task<RespuestaPayload<ConsultaAplicacion>> UnicaPorIdDespliegue(string id)
    {
        RespuestaPayload<ConsultaAplicacion> respuesta = new RespuestaPayload<ConsultaAplicacion>();

        try
        {
            // Intentar obtener la información de la caché
            var cacheKey = $"AplicacionDesplegar_{id}";
            var cachedData = await _cache.GetStringAsync(cacheKey);
            ConsultaAplicacion dtoDespliegue;
            if (cachedData != null)
            {
                dtoDespliegue = JsonSerializer.Deserialize<ConsultaAplicacion>(cachedData);
            }
            else
            {
                var resultado = await UnicaPorId(id);

                if (!resultado.Ok)
                {
                    respuesta.Error = resultado.Error;
                    respuesta.HttpCode = resultado.HttpCode;
                    return respuesta;
                }

                dtoDespliegue = ADTODespliegue((EntidadAplicacion)resultado.Payload);

                await _cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(dtoDespliegue, new JsonSerializerOptions { IgnoreNullValues = true }));

            }

            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = dtoDespliegue;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioAplicacion-UnicaPorIdDespliegue {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<RespuestaPayload<ConsultaAplicacion>> Insertar(CreaAplicacion data)
    {
        var respuesta = new RespuestaPayload<ConsultaAplicacion>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);

                if(entidad.Default == true)
                {
                    var aplicaciones = await DB.Aplicaciones.Where(x => x.Default == true).ToListAsync();

                    foreach(var aplicacion in aplicaciones)
                    {
                        aplicacion.Default = false;
                        _dbSetFull.Update(aplicacion);
                        await _db.SaveChangesAsync();
                    }
                }

                _dbSetFull!.Add(entidad);
                await _db!.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.APPLICACION_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioAplicacion-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    #endregion

    #region Consulta Aplicacion
    public async Task<RespuestaPayload<ConsultaAplicacionAnonima>> ConsultaAplicacion(string host, string? clave)
    {
        RespuestaPayload<ConsultaAplicacionAnonima> respuesta = new();
        try
        {
            _logger.LogDebug("ServicioAplicacion-ConsultaAplicacion-{clave} ", clave);

            EntidadAplicacion aplicacion = null;

            aplicacion = !string.IsNullOrEmpty(clave)
            ? await DB.Aplicaciones.FirstOrDefaultAsync(x => x.Clave.ToLower() == clave.ToLower()) ?? DB.Aplicaciones.FirstOrDefault(x => x.Default)
            : await DB.Aplicaciones.FirstOrDefaultAsync(x => x.Hosts.Any(y => y.Equals(host))) ?? DB.Aplicaciones.FirstOrDefault(x => x.Default);


            if (aplicacion != null)
            {
                aplicacion.Plantillas = await DB.PlantillaInvitaciones
                .Where(x => x.AplicacionId == aplicacion.Id)
                .ToListAsync();
                aplicacion.Logotipos = await DB.LogoAplicaciones
                .Where(x => x.AplicacionId == aplicacion.Id)
                .ToListAsync();
                aplicacion.Consentimientos = await DB.Consentimientos
                .Where(x => x.AplicacionId == aplicacion.Id)
                .ToListAsync();

                ConsultaAplicacionAnonima consultaAplicacionAnonima = new()
                {
                    Nombre = aplicacion.Nombre,
                    Clave = aplicacion.Clave,
                    Plantillas = aplicacion.Plantillas,
                    Logotipos = aplicacion.Logotipos,
                    Consentimientos = aplicacion.Consentimientos
                };

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = consultaAplicacionAnonima;
                _logger.LogDebug("SevicioAplicacion-ConsultaAplicacion resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APPLICACION_NO_ENCONTRADA,
                    Mensaje = "No existe una aplicación con la misma clave",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
            }
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioAplicacion-ConsultaAplicacion {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APPLICACION_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        
        return respuesta;
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
