#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using aprendizaje.model.galeria;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using Polly.Timeout;
using System.Numerics;
using System.Text.Json;

namespace aprendizaje.services.galeria.temagaleria;
[ServicioEntidadAPI(entidad: typeof(TemaGaleria))]
public class ServicioTemaGaleria : ServicioEntidadHijoGenericaBase<TemaGaleria, TemaGaleria, TemaGaleria, TemaGaleria, string>,
    IServicioEntidadHijoAPI,IServicioTemaGaleria
{
    private readonly ILogger<ServicioTemaGaleria> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private Galeria? galeria;
    private DbSet<Galeria> _dbSetGaleria;
    public ServicioTemaGaleria(ILogger<ServicioTemaGaleria> logger, 
        IServicionConfiguracionMongo configuracionMongo, 
        IReflectorEntidadesAPI reflector, IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextAprendizaje.NOMBRE_COLECCION_GALERIA);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContextAprendizaje.NOMBRE_COLECCION_GALERIA}'";
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

            _db = MongoDbContextAprendizaje.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetGaleria = ((MongoDbContextAprendizaje)_db).Galeria;

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextAprendizaje.NOMBRE_COLECCION_GALERIA}'");
            throw;
        }
    }

    private MongoDbContextAprendizaje DB { get { return (MongoDbContextAprendizaje)_db; } }
    
    public bool RequiereAutenticacion => true;
    
    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }
    
    string IServicioEntidadHijoAPI.Padreid { get => this.galeria.Id.ToString() ?? null; set => EstableceDbSet(value); }
    
    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<TemaGaleria>(JsonAPIDefaults());
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
        galeria = _dbSetGaleria.FirstOrDefault(_ => _.Id == new Guid(padreId));
        this.Padreid = galeria != null ? galeria.Id.ToString() : null;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        var add = data.Deserialize<TemaGaleria>(JsonAPIDefaults());
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

    #region Overrides para la personalización de la ENTIDAD => TEMAGALERIA
    public override async Task<ResultadoValidacion> ValidarInsertar(TemaGaleria data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, TemaGaleria original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarActualizar(string id, TemaGaleria actualizacion, TemaGaleria original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override TemaGaleria ADTOFull(TemaGaleria actualizacion, TemaGaleria actual)
    {
        actual.Nombre = actualizacion.Nombre;
        return actual;
    }

    public override TemaGaleria ADTOFull(TemaGaleria data)
    {
        TemaGaleria temaGaleria = new TemaGaleria()
        {
            Id = data.Id,
            Nombre = data.Nombre
        };
        return temaGaleria;
    }

    public override TemaGaleria ADTODespliegue(TemaGaleria data)
    {
        TemaGaleria temaGaleria = new TemaGaleria()
        {
            Id = data.Id,
            Nombre = data.Nombre
        };
        return temaGaleria;
    }

    public virtual async Task<RespuestaPayload<TemaGaleria>> Insertar(TemaGaleria data)
    {
        var respuesta = new RespuestaPayload<TemaGaleria>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                galeria.ListaTemasGaleria.Add(entidad);
                _dbSetGaleria.Update(galeria);
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

    public override async Task<Respuesta> Actualizar(string id, TemaGaleria data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            TemaGaleria actual = galeria.ListaTemasGaleria.FirstOrDefault(_ => _.Id == data.Id);
            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }


            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = galeria.ListaTemasGaleria.IndexOf(entidad);
                if(index == 0)
                {
                    galeria.ListaTemasGaleria[0] = entidad;
                    _dbSetGaleria.Update(galeria);
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

    public override async Task<RespuestaPayload<TemaGaleria>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<TemaGaleria>();
        try
        {
            TemaGaleria actual = galeria.ListaTemasGaleria.FirstOrDefault(_ => _.Id == int.Parse(id));
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

            TemaGaleria actual = galeria.ListaTemasGaleria.FirstOrDefault(_=>_.Id == int.Parse(id));
            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                galeria.ListaTemasGaleria.Remove(actual);
                _dbSetGaleria.Update(galeria);
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

    public virtual async Task<PaginaGenerica<TemaGaleria>> ObtienePaginaElementos(Consulta consulta)
    {
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(TemaGaleria));
        var Elementos = Enumerable.Empty<TemaGaleria>().AsQueryable();

        if (consulta.Filtros.Count > 0)
        {
            var predicateBody = interpreteConsulta.CrearConsultaExpresion<TemaGaleria>(consulta, entidad);

            if (predicateBody != null)
            {
                var RConsulta = galeria.ListaTemasGaleria.AsQueryable().Provider.CreateQuery<TemaGaleria>(predicateBody.getWhereExpression(galeria.ListaTemasGaleria.AsQueryable().Expression));

                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
            }
        }
        else
        {
            var RConsulta = galeria.ListaTemasGaleria.AsQueryable();
            Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

        }
        return await Elementos.PaginadoAsync(consulta);
    }
    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return
