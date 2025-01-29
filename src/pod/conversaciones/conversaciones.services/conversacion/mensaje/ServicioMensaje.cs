#pragma warning disable CS1998 // Async method lacks 'await' operators and will run synchronously
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using comunicaciones.modelo;
using comunicaciones.modelo.email;
using comunicaciones.modelo.whatsapp;
using conversaciones.model;
using conversaciones.services.dbcontext;
using conversaciones.services.extensiones;
using conversaciones.services.proxy.abstractions;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Collections.Specialized;
using System.Text.Json;
using static comunicaciones.modelo.Constantes;

namespace conversaciones.services.conversacion.mensaje;
[ServicioEntidadAPI(entidad:typeof(Mensaje))]
public class ServicioMensaje : ServicioEntidadGenericaBase<Mensaje, Mensaje, Mensaje, Mensaje, string>,
    IServicioEntidadAPI, IServicioMensaje
{
    private readonly ILogger<ServicioMensaje> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IConfiguration configuration;
    private readonly IProxyConversacionComunicaciones _proxyConversacionComunicaciones;
    private Conversacion? conversacion;
    private DbSet<Conversacion> _dbSetConversacion;
    public ServicioMensaje(ILogger<ServicioMensaje> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI reflector, IDistributedCache cache, IConfiguration configuration, IProxyConversacionComunicaciones proxyConversacionComunicaciones
        ) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        this.configuration = configuration;
        interpreteConsulta = new InterpreteConsultaExpresiones();
        _proxyConversacionComunicaciones = proxyConversacionComunicaciones;

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextConversaciones.NOMBRE_COLECCION_CONVERSACION);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_CONVERSACION}'";
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

            _db = MongoDbContextConversaciones.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetConversacion = ((MongoDbContextConversaciones)_db).Conversacion;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextConversaciones.NOMBRE_COLECCION_PLANTILLA}'");
            throw;
        }
    }

    private MongoDbContextConversaciones DB { get { return (MongoDbContextConversaciones)_db; } }

    public bool RequiereAutenticacion => true;

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMensaje-ActualizarAPI-{data}", data);
        var update = data.Deserialize<Mensaje>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update, parametros);
        _logger.LogDebug("ServicioMensaje-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        _logger.LogDebug("ServicioMensaje-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id, parametros, forzarEliminacion);
        _logger.LogDebug("ServicioMensaje-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioMensaje-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioMensaje-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioMensaje-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioMensaje-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioMensaje-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMensaje-InsertarAPI-{data}", data);
        var add = data.Deserialize<Mensaje>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMensaje-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioMensaje-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMensaje-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMensaje-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMensaje-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMensaje-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMensaje-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMensaje-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMensaje-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioMensaje-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalizacion de le ENTIDAD => Contenido
    public override async Task<ResultadoValidacion> ValidarInsertar(Mensaje data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Mensaje original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Mensaje actualizacion, Mensaje original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override Mensaje ADTOFull(Mensaje actualizacion, Mensaje actual)
    {
        actual.Id = actualizacion.Id;
        actual.Cuerpo = actualizacion.Cuerpo;
        actual.Encabezado = actualizacion.Encabezado;
        actual.PlantillaId = actualizacion.PlantillaId;
        actual.CargaUtil = actualizacion.CargaUtil;
        actual.FechaCreacion = actualizacion.FechaCreacion;
        actual.FechaEnvio = actualizacion.FechaEnvio;
        actual.ErrorEnvio = actualizacion.ErrorEnvio;
        actual.CodigoError = actualizacion.CodigoError;
        actual.EmisorId = actualizacion.EmisorId;
        actual.Bytes = actualizacion.Bytes;
        actual.PrepagoId = actualizacion.PrepagoId;
        return actual;
    }

    public override Mensaje ADTOFull(Mensaje data)
    {
        Mensaje mensaje = new Mensaje()
        {
            Id = data.Id,
            Cuerpo = data.Cuerpo,
            Encabezado = data.Encabezado,
            PlantillaId = data.PlantillaId,
            CargaUtil = data.CargaUtil,
            FechaCreacion = data.FechaCreacion,
            FechaEnvio = data.FechaEnvio,
            ErrorEnvio = data.ErrorEnvio,
            CodigoError = data.CodigoError,
            EmisorId = data.EmisorId,
            Bytes = data.Bytes,
            PrepagoId = data.PrepagoId
        };
        return mensaje;
    }

    public override Mensaje ADTODespliegue(Mensaje data)
    {
        Mensaje mensaje = new Mensaje()
        {
            Id = data.Id,
            Cuerpo = data.Cuerpo,
            Encabezado = data.Encabezado,
            PlantillaId = data.PlantillaId,
            CargaUtil = data.CargaUtil,
            FechaCreacion = data.FechaCreacion,
            FechaEnvio = data.FechaEnvio,
            ErrorEnvio = data.ErrorEnvio,
            CodigoError = data.CodigoError,
            EmisorId = data.EmisorId,
            Bytes = data.Bytes,
            PrepagoId = data.PrepagoId
        };
        return mensaje;
    }

    public override async Task<RespuestaPayload<Mensaje>> Insertar(Mensaje data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Mensaje>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);

                conversacion = _dbSetConversacion.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
                var r = SeleccionCanal(conversacion, entidad);

                if (r.Result.Ok == true)
                {
                    conversacion.Mensajes.Add(entidad);
                    _dbSetConversacion.Update(conversacion);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                    respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONVERSACIONES_MENSAJE_ERROR_ENVIO_A_CONVERSACION;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONVERSACIONES_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMensaje-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, Mensaje data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_MENSAJE_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            conversacion = _dbSetConversacion.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
            Mensaje actual = conversacion.Mensajes.FirstOrDefault(_ => _.Id == data.Id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_MENSAJE_NO_ENCONTRADA,
                    Mensaje = "No existe un Mensaje con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id, data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = conversacion.Mensajes.IndexOf(entidad);
                if (index == 0)
                {
                    conversacion.Mensajes[0] = entidad;
                    _dbSetConversacion.Update(conversacion);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.CONVERSACIONES_MENSAJE_ERROR_ACTUALIZAR,
                        Mensaje = "No ha sido posible actualizar el Mensaje",
                        HttpCode = HttpCode.BadRequest
                    };
                    respuesta.HttpCode = HttpCode.BadRequest;
                    return respuesta;
                }
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.CONVERSACIONES_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMensaje-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<Mensaje>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Mensaje>();
        try
        {
            conversacion = _dbSetConversacion.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
            Mensaje actual = conversacion.Mensajes.FirstOrDefault(_ => _.Id == id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_MENSAJE_NO_ENCONTRADA,
                    Mensaje = "No existe un Mensaje con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioMensaje-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_MENSAJE_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            conversacion = _dbSetConversacion.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
            Mensaje actual = conversacion.Mensajes.FirstOrDefault(_ => _.Id == id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_MENSAJE_NO_ENCONTRADA,
                    Mensaje = "No existe un Mensaje con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                conversacion.Mensajes.Remove(actual);
                _dbSetConversacion.Update(conversacion);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.CONVERSACIONES_MENSAJE_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR el Mensaje",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMensaje-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<PaginaGenerica<Mensaje>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioMensaje - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Mensaje));
        var Elementos = Enumerable.Empty<Mensaje>().AsQueryable();
        conversacion = _dbSetConversacion.FirstOrDefault(_ => _.Id == parametros["n0Id"]);
        if (conversacion != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<Mensaje>(consulta, entidad);

                if (predicateBody != null)
                {
                    var RConsulta = conversacion.Mensajes.AsQueryable().Provider.CreateQuery<Mensaje>(predicateBody.getWhereExpression(conversacion.Mensajes.AsQueryable().Expression));

                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = conversacion.Mensajes.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

            }
        }
        return Elementos.Paginado(consulta);
    }


    public async Task<Respuesta> SeleccionCanal(Conversacion conversacion, Mensaje mensaje)
    {
        var r = new Respuesta();
        switch (conversacion.Canal)
        {
            case TipoCanal.CorreoElectronico:
                r = await ProcesoEmail(conversacion, mensaje);
                break;
            case TipoCanal.WhatsApp:
                r = await ProcesoWhatsApp(conversacion, mensaje);
                break;
        }
        return r;
    }

    public async Task<Respuesta> ProcesoWhatsApp(Conversacion conversacion, Mensaje mensaje)
    {
        var r = new Respuesta();
        try
        {
            foreach (var item in conversacion.Participantes)
            {
                MensajeWhatsapp whatsapp = new()
                {
                    Mensaje = mensaje.CargaUtil,
                    Telefono = item.UsuarioId,
                    Tipo = TipoMensajeWhatsApp(mensaje.tipoMensaje),
                };
                var respuestaWhatsApp = await _proxyConversacionComunicaciones.EnvioWhatsApp(whatsapp);
                r = respuestaWhatsApp;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMensaje-ProcesoWhatsApp resultado {msg}", ex.Message);
            r.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            r.HttpCode = HttpCode.ServerError;
        }

        return r;
    }

    public async Task<Respuesta> ProcesoEmail(Conversacion conversacion, Mensaje mensaje)
    {
        var r = new Respuesta();
        try
        {
            foreach (var item in conversacion.Participantes)
            {
                DatosPlantilla data = new()
                {
                    Nombre = mensaje.EmisorId,
                    UrlBase = configuration.LeeUrlBase(),
                    Mensaje = mensaje.CargaUtil,
                };

                MensajeEmail m = new MensajeEmail()
                {
                    DireccionPara = item.Id,
                    NombrePara = item.Nombre,
                    JSONData = JsonSerializer.Serialize(data),
                    PlantillaCuerpo = configuration.LeePlantillaRegistro(),
                    PlantillaTema = configuration.LeeTemaRegistro()
                };

                var respuestaCorreo = await _proxyConversacionComunicaciones.EnvioCorreo(m);
                r = respuestaCorreo;
            }  
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioMensaje-ProcesoEmail resultado {msg}", ex.Message);
            r.Error = new ErrorProceso() { Codigo = CodigosError.CONVERSACIONES_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            r.HttpCode = HttpCode.ServerError;
        }

        return r;

    }

    private TipoMensaje TipoMensajeWhatsApp(TipoMsj tipo)
    {
        switch (tipo)
        {
            case TipoMsj.texto:
                return TipoMensaje.texto;
            case TipoMsj.img:
                return TipoMensaje.img;
            default:
                return TipoMensaje.texto;
        }
    }

    #endregion

}
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS1998 // Async method lacks 'await' operators and will run synchronously
