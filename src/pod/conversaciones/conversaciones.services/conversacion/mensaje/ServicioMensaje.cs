#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using conversaciones.model;
using conversaciones.services.dbcontext;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace conversaciones.services.conversacion.mensaje;
[ServicioEntidadAPI(entidad:typeof(Mensaje))]
public class ServicioMensaje : ServicioEntidadHijoGenericaBase<Mensaje, Mensaje, Mensaje, Mensaje, string>,
    IServicioEntidadHijoAPI, IServicioMensaje
{
    private readonly ILogger<ServicioMensaje> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private Conversacion? conversacion;
    private DbSet<Conversacion> _dbSetConversacion;
    public ServicioMensaje(ILogger<ServicioMensaje> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI reflector, IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

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

    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }

    string IServicioEntidadHijoAPI.Padreid { get => this.conversacion.Id ?? null; set => EstableceDbSet(value); }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<Mensaje>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        return await this.Eliminar((string)id);
    }

    public Entidad EntidadDespliegueAPI()
    {
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        this.EstableceContextoUsuario(contexto);
    }

    public void EstableceDbSet(string padreId)
    {
        conversacion = _dbSetConversacion.FirstOrDefault(_ => _.Id == padreId);
        this.Padreid = conversacion != null ? conversacion.Id : null;
    }
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        var add = data.Deserialize<Mensaje>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<Object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
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

    #region Overrides para la personalizacion de le ENTIDAD => Contenido
    public override async Task<ResultadoValidacion> ValidarInsertar(Mensaje data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Mensaje original)
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
            FechaCreacion = DateTime.UtcNow,
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

    public virtual async Task<RespuestaPayload<Mensaje>> Insertar(Mensaje data)
    {
        var respuesta = new RespuestaPayload<Mensaje>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                conversacion.Mensajes.Add(entidad);
                _dbSetConversacion.Update(conversacion);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.BadRequest;
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

    public override async Task<Respuesta> Actualizar(string id, Mensaje data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Mensaje actual = conversacion.Mensajes.FirstOrDefault(_ => _.Id == data.Id);
            if (actual == null)
            {
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
                    respuesta.Error = resultadoValidacion.Error;
                    respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
                }

            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError($"Actualizar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<Mensaje>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Mensaje>();
        try
        {
            Mensaje actual = conversacion.Mensajes.FirstOrDefault(_ => _.Id == id);
            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.Ok;
                return respuesta;
            }
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch (Exception ex)
        {
            _logger.LogError($"UnicaPorId {ex.Message}");
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

            Mensaje actual = conversacion.Mensajes.FirstOrDefault(_ => _.Id == id);
            if (actual == null)
            {
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

    public override async Task<PaginaGenerica<Mensaje>> ObtienePaginaElementos(Consulta consulta)
    {
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Mensaje));
        var Elementos = Enumerable.Empty<Mensaje>().AsQueryable();
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

    #endregion

}
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.