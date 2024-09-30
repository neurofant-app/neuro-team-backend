#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.atributos;
using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo.campi;
using controlescolar.servicios.dbcontext;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text.Json;


namespace controlescolar.servicios.entidadcampus;
[ServicioEntidadAPI(entidad:typeof(EntidadCampus))]
public class ServicioEntidadCampus : ServicioEntidadGenericaBase<EntidadCampus, CreaCampus, ActualizaCampus, ConsultaCampusCuenta, string>,
    IServicioEntidadAPI, IServicioEntidadCampus
{
    private readonly ILogger _logger;

    private readonly IReflectorEntidadesAPI reflector;
    public ServicioEntidadCampus(ILogger<ServicioEntidadCampus> logger, 
        IServicionConfiguracionMongo configuracionMongo, 
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache) {
        _logger = logger;
        reflector = Reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContext.NOMBRE_COLECCION_CAMPUS);
        if (configuracionEntidad == null )
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContext.NOMBRE_COLECCION_CAMPUS}'";
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
            
            _db = MongoDbContext.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContext)_db).EntidadCampi;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContext.NOMBRE_COLECCION_CAMPUS}'");
            throw;
        }        
    }
    private MongoDbContext DB { get { return (MongoDbContext)_db; } }
    public bool RequiereAutenticacion => true;
    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioEntidadCampus-EntidadRepoAPI");
        return this.EntidadRepo();
    }
    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioEntidadCampus-EntidadInsertAPI");
        return this.EntidadInsert();
    }
    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioEntidadCampus-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }
    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioEntidadCampus-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioEntidadCampus-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioEntidadCampus-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    private bool permisosValidos(string appId, [CallerMemberName] string metodoId = null)
    {
        var metodoActual = _contextoUsuario.AtributosMetodos.FirstOrDefault(_ => _.MetodoId == metodoId);

        if (metodoActual == null) { return false;}
        foreach (var id in metodoActual.atributosId)
        {
            if (!_contextoUsuario.RolesAplicacion.Contains(id) && !_contextoUsuario.PermisosAplicacion.Contains(id)) return false;
        }
        return true;
    }

    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_ADMIN)]
    [Permiso(Constantes.AplicacionId, Constantes.CE_CAMPUS_PERM_ADMIN)]
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadCampus-InsertarAPI-{data}", data);
        if (!permisosValidos(Constantes.AplicacionId))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadCampus-InsertarAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        }
        var add = data.Deserialize<CreaCampus>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadCampus-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_ADMIN)]
    [Permiso(Constantes.AplicacionId, Constantes.CE_CAMPUS_PERM_ADMIN)]
    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadCampus-ActualizarAPI-{data}", data);
        if (!permisosValidos(Constantes.AplicacionId))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadCampus-ActualizarAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        }
        var update = data.Deserialize<ActualizaCampus>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioEntidadCampus-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_ADMIN)]
    [Permiso(Constantes.AplicacionId, Constantes.CE_CAMPUS_PERM_ADMIN)]
    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadCampus-EliminarAPI");
        if (!permisosValidos(Constantes.AplicacionId))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadCampus-EliminarAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        }
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioEntidadAlumno-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_ADMIN)]
    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_VISOR)]
    [Permiso(Constantes.AplicacionId, Constantes.CE_CAMPUS_PERM_VIEW)]
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadCampus-UnicaPorIdAPI");
        if (!permisosValidos(Constantes.AplicacionId))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadCampus-UnicaPorIdAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        }
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadCampus-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_ADMIN)]
    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_VISOR)]
    [Permiso(Constantes.AplicacionId, Constantes.CE_CAMPUS_PERM_VIEW)]
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadCampus-UnicaPorIdDespliegueAPI");
        if (!permisosValidos(Constantes.AplicacionId))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadCampus-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        }
        var temp = await this.UnicaPorIdDespliegue((string)id);

        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadCampus-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_ADMIN)]
    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_VISOR)]
    [Permiso(Constantes.AplicacionId, Constantes.CE_CAMPUS_PERM_VIEW)]
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadCampus-PaginaAPI-{consulta}", consulta);
        if (!permisosValidos(Constantes.AplicacionId))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadCampus-PaginaAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        }
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadCampus-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }


    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_ADMIN)]
    [Rol(Constantes.AplicacionId, Constantes.CE_CAMPUS_ROL_VISOR)]
    [Permiso(Constantes.AplicacionId, Constantes.CE_CAMPUS_PERM_VIEW)]
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioEntidadCampus-PaginaDespliegueAPI-{consulta}", consulta);
        if (!permisosValidos(Constantes.AplicacionId))
        {
            RespuestaPayload<object> respuestaPayload = new RespuestaPayload<object>()
            { HttpCode = HttpCode.FORBIDDEN, Error = new() { Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ACCION_PROHIBIDA_NO_PERMISOS, Mensaje = "No se tiene el permiso acccion prohibida", HttpCode = HttpCode.FORBIDDEN } };
            _logger.LogDebug("ServicioEntidadCampus-PaginaDespliegueAPI resultado {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        }
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioEntidadAlumno-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la entidad EntidadCampus
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaCampus data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, EntidadCampus original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaCampus actualizacion, EntidadCampus original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override EntidadCampus ADTOFull(ActualizaCampus actualizacion, EntidadCampus actual)
    {
        actual.Nombre = actualizacion.Nombre;
        actual.Virtual = actualizacion.Virtual;
        return actual;
    }

    public override EntidadCampus ADTOFull(CreaCampus data)
    {
        EntidadCampus entidadCampus = new EntidadCampus()
        {
            Id = Guid.NewGuid(),
            Nombre = data.Nombre,
            Virtual = data.Virtual,
            CampusPadreId = data.CampusPadreId,
            Activo = data.Activo
        };
        return entidadCampus;
    }

    public override async Task<Respuesta> Actualizar(string id, ActualizaCampus data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            EntidadCampus actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_NO_ENCONTRADA,
                    Mensaje = "No existe una EntidadCampus con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.CONTROLESCOLAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadCampus-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<EntidadCampus>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<EntidadCampus>();
        try
        {
            EntidadCampus actual = await _dbSetFull.FindAsync(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_NO_ENCONTRADA,
                    Mensaje = "No existe una EntidadCampus con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioEntidadCampus-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            EntidadCampus actual = _dbSetFull.Find(Guid.Parse(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONTROLESCOLAR_ENTIDADCAMPUS_NO_ENCONTRADA,
                    Mensaje = "No existe una EntidadCampus con el Id proporcionado",
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
                respuesta.Error!.Codigo = CodigosError.CONTROLESCOLAR_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioEntidadCampus-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONTROLESCOLAR_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    #endregion


}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
