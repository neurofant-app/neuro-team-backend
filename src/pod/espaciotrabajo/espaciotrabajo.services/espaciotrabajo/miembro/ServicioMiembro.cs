#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using espaciotrabajo.model.espaciotrabajo;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace espaciotrabajo.services.espaciotrabajo.miembro;

[ServicioEntidadAPI(typeof(Miembro))]
public class ServicioMiembro : ServicioEntidadHijoGenericaBase<Miembro, Miembro, Miembro, Miembro,string>,
    IServicioEntidadHijoAPI, IServicioMiembro
{
    private readonly ILogger<Miembro> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private DbSet<EspacioTrabajo> _dbSetEspacioTrabajo;
    private EspacioTrabajo? espacioTrabajo;


    public ServicioMiembro(ILogger<Miembro> logger,IReflectorEntidadesAPI reflector, IServicionConfiguracionMongo configuracionMongo, IDistributedCache cache) 
        : base (null, null, logger, reflector, cache)
    {
        this._logger = logger;
        this._reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();
        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextEspacioTrabajo.NOMBRE_COLECCION_ESPACIOTRABAJO);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextEspacioTrabajo.NOMBRE_COLECCION_ESPACIOTRABAJO}'";
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

            _db = MongoDbContextEspacioTrabajo.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetEspacioTrabajo = ((MongoDbContextEspacioTrabajo)_db).EspaciosTrabajo;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar mongo para {coleccion}'", MongoDbContextEspacioTrabajo.NOMBRE_COLECCION_ESPACIOTRABAJO);
            throw;
        }
    }

    private MongoDbContextEspacioTrabajo DB { get { return (MongoDbContextEspacioTrabajo)_db; } }

    public bool RequiereAutenticacion => true;

    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }

    string IServicioEntidadHijoAPI.Padreid { get => this.espacioTrabajo.Id.ToString() ?? null; set => EstableceDbSet(value); }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioMiembro-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Miembro>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioMiembro-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioMiembro-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioMiembro-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioMiembro-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioMiembro-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public void EstableceDbSet(string padreId)
    {
        _logger.LogDebug("ServicioMiembro-EstableceDbSet - {padreId}", padreId);
        espacioTrabajo = _dbSetEspacioTrabajo.FirstOrDefault(_ => _.Id == new Guid(padreId));
        this.Padreid = espacioTrabajo != null ? espacioTrabajo.Id.ToString() : null;
        _logger.LogDebug("ServicioMiembro-EstableceDbSet - resultado {padreId}", this.Padreid);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioMiembro-InsertarAPI-{data}", data);
        var add = data.Deserialize<Miembro>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioMiembro-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioMiembro-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioMiembro-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioMiembro-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioMiembro-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMiembro-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => Tema
    public override async Task<ResultadoValidacion> ValidarInsertar(Miembro data)
    {
        ResultadoValidacion resultado = new();


        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Miembro original)
    {
        ResultadoValidacion resultado = new();


        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Miembro actualizacion, Miembro original)
    {
        ResultadoValidacion resultado = new();


        resultado.Valido = true;
        return resultado;
    }

    public override Miembro ADTOFull(Miembro actualizacion, Miembro actual)
    {
        actual.UsuarioId = actualizacion.UsuarioId;
        return actual;
    }

    public override Miembro ADTOFull(Miembro data)
    {
        Miembro miembro = new Miembro()
        {
            UsuarioId = data.UsuarioId,
        };
        return miembro;
    }

    public override Miembro ADTODespliegue(Miembro data)
    {
        Miembro miembro = new Miembro()
        {
            UsuarioId = data.UsuarioId
        };
        return miembro;
    }

    public override async Task<RespuestaPayload<Miembro>> Insertar(Miembro data)
    {
        var respuesta = new RespuestaPayload<Miembro>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                espacioTrabajo.Miembros.Add(entidad);
                _dbSetEspacioTrabajo.Update(espacioTrabajo);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.ESPACIOTRABAJO_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMiembro-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ESPACIOTRABAJO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, Miembro data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Miembro actual = espacioTrabajo.Miembros.FirstOrDefault(_ => _.UsuarioId.Equals(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_NO_ENCONTRADO,
                    Mensaje = "No existe un MIEMBRO con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = espacioTrabajo.Miembros.IndexOf(entidad);
                if (index == 0)
                {
                    espacioTrabajo.Miembros[0] = entidad;
                    _dbSetEspacioTrabajo.Update(espacioTrabajo);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar el Miembro",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
                }
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.ESPACIOTRABAJO_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMiembro-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ESPACIOTRABAJO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<Miembro>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Miembro>();
        try
        {
            Miembro actual = espacioTrabajo.Miembros.FirstOrDefault(_ => _.UsuarioId.Equals(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_NO_ENCONTRADO,
                    Mensaje = "No existe un MIEMBRO con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioMiembro-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ESPACIOTRABAJO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Miembro actual = espacioTrabajo.Miembros.FirstOrDefault(_ => _.UsuarioId.Equals(id));
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_NO_ENCONTRADO,
                    Mensaje = "No existe un Miembro con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                espacioTrabajo.Miembros.Remove(actual);
                _dbSetEspacioTrabajo.Update(espacioTrabajo);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.ESPACIOTRABAJO_MIEMBRO_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR el Tema",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMiembro-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.ESPACIOTRABAJO_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<Miembro>> ObtienePaginaElementos(Consulta consulta)
    {
        _logger.LogDebug("ServicioMiembro - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Miembro));
        var Elementos = Enumerable.Empty<Miembro>().AsQueryable();
        if (espacioTrabajo != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<Miembro>(consulta, entidad);
                if (predicateBody != null)
                {
                    var RConsulta = espacioTrabajo.Miembros.AsQueryable().Provider.CreateQuery<Miembro>(predicateBody.getWhereExpression(espacioTrabajo.Miembros.AsQueryable().Expression));
                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "UsuarioId", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = espacioTrabajo.Miembros.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "UsuarioId", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        return Elementos.Paginado(consulta);
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return
