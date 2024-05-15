#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using extensibilidad.metadatos;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using comunes.primitivas;
using apigenerica.model.servicios;
using aplicaciones.model;
using aplicaciones.services.aplicacion;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using comunes.primitivas.configuracion.mongo;
using aplicaciones.services.dbcontext;
using MongoDB.Driver;
using Polly.Caching;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        var add = data.Deserialize<CreaAplicacion>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<ActualizaAplicacion>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        return await this.Eliminar((string)id);
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

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaHijoAPI(Consulta consulta, string tipoPadre, string id)
    {
        var temp = await this.PaginaHijo(consulta, tipoPadre, id);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaHijosDespliegueAPI(Consulta consulta, string tipoPadre, string id)
    {
        var temp = await this.PaginaHijosDespliegue(consulta, tipoPadre, id);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
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

    public override async Task<(List<EntidadAplicacion> Elementos, int? Total)> ObtienePaginaElementos(Consulta consulta)
    {
        await Task.Delay(0);
        Entidad entidad = reflector.ObtieneEntidad(typeof(EntidadAplicacion));
        string query = interpreteConsulta.CrearConsulta(consulta, entidad, MongoDbContextAplicaciones.NOMBRE_COLECCION_APLICACION);

        int? total = null;
        List<EntidadAplicacion> elementos = DB.Aplicaciones.FromSqlRaw(query).ToList();

        if (consulta.Contar)
        {
            query = query.Split("ORDER")[0];
            query = $"{query.Replace("*", "count(*)")}";
            total = DB.Database.SqlQueryRaw<int>(query).ToArray().First();
        }


        if (elementos != null)
        {
            return new(elementos, total);
        }
        else
        {
            return new(new List<EntidadAplicacion>(), total); ;
        }
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaAplicacion data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            EntidadAplicacion actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
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


    public override async Task<RespuestaPayload<EntidadAplicacion>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<EntidadAplicacion>();
        try
        {
            EntidadAplicacion actual = await _dbSetFull.FindAsync(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            var plantillas = await DB.PlantillaInvitaciones
                .Where(x => x.AplicacionId == actual.Id)
                .ToListAsync();
            var logos = await DB.LogoAplicaciones
                .Where(x => x.AplicacionId == actual.Id)
                .ToListAsync();
            var consentimientos = await DB.Consentimientos
                .Where(x => x.AplicacionId == actual.Id)
                .ToListAsync();

            actual.Plantillas = plantillas;
            actual.Logotipos = logos;
            actual.Consentimientos = consentimientos;
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

            EntidadAplicacion actual = _dbSetFull.Find(Guid.Parse(id));
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
            _logger.LogError($"UnicaPorIdDespliegue {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public virtual async Task<RespuestaPayload<ConsultaAplicacion>> Insertar(CreaAplicacion data)
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

    #region Consulta Aplicacion
    public async Task<ConsultaAplicacionAnonima> ConsultaAplicacion(string host, string? clave)
    {
        ConsultaAplicacionAnonima consultaAplicacionAnonima = null;
        EntidadAplicacion aplicacion = null;

        aplicacion = !string.IsNullOrEmpty(clave)
        ? await DB.Aplicaciones.FirstOrDefaultAsync(x => x.Clave.ToLower() == clave.ToLower()) ?? DB.Aplicaciones.FirstOrDefault(x => x.Default)
        : await DB.Aplicaciones.FirstOrDefaultAsync(x => x.Hosts.Any(y => y.Equals(host))) ?? DB.Aplicaciones.FirstOrDefault(x => x.Default);


        consultaAplicacionAnonima = new()
        {
            Nombre = aplicacion.Nombre,
            Clave = aplicacion.Clave,
            Plantillas = aplicacion.Plantillas,
            Logotipos = aplicacion.Logotipos,
            Consentimientos = aplicacion.Consentimientos
        };
        return consultaAplicacionAnonima;

    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
