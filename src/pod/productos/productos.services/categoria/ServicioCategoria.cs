
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using organizacion.services.dbcontext;
using productos.model.categoria;
using System.Collections.Specialized;
using System.Text.Json;

namespace productos.services.categoria;
[ServicioEntidadAPI(typeof(Categoria))]
public class ServicioCategoria : ServicioEntidadGenericaBase<Categoria, CategoriaInsertar, CategoriaActualizar, CategoriaDespliegue, Guid>, IServicioEntidadAPI, IServicioCategoria
{
    private readonly ILogger<ServicioCategoria> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private readonly IDistributedCache _cache;

    public ServicioCategoria(ILogger<ServicioCategoria> logger, IServicionConfiguracionMongo configuracionMongo, IReflectorEntidadesAPI reflector, IDistributedCache cache) : base(null, null, logger, reflector, cache)
    {
        _logger = logger;
        _reflector = reflector;
        _cache = cache;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextProductos.NOMBRE_COLECCION_CATEGORIAS);

        if(configuracionEntidad == null)
        {
            string err = $"No existe configuración de mongo para '{MongoDbContextProductos.NOMBRE_COLECCION_CATEGORIAS}'";
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

            _db = MongoDbContextProductos.Create(client.GetDatabase(configuracionEntidad.Esquema));
            _dbSetFull = ((MongoDbContextProductos)_db).Categorias;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextProductos.NOMBRE_COLECCION_CATEGORIAS}'");
            throw;
        }
    }

    public bool RequiereAutenticacion => true;
    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioCategoria-ActualizarAPI-{data}", data);
        var update = data.Deserialize<CategoriaActualizar>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar(Guid.Parse((string)id), update, parametros);
        _logger.LogDebug("ServicioCategoria-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioCategoria-EliminarAPI");
        Respuesta respuesta = await this.Eliminar(Guid.Parse((string)id), parametros);
        _logger.LogDebug("ServicioCategoria-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioCategoria-InsertarAPI-{data}", data);
        var add = data.Deserialize<CategoriaInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCategoria-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioCategoria-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCategoria-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioCategoria-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCategoria-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioCategoria-UnicaPorIdAPI");
        var temp = await this.UnicaPorId(Guid.Parse(id.ToString()), parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCategoria-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        _logger.LogDebug("ServicioCategoria-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue(Guid.Parse(id.ToString()), parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioCategoria-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }
    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioCategoria-EntidadRepoAPI");
        return this.EntidadRepo();
    }
    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioCategoria-EntidadInsertAPI");
        return this.EntidadInsert();
    }
    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioCategoria-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }
    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioCategoria-DespliegueAPI");
        return this.EntidadDespliegue();
    }
    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioCategoria-ContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioCategoria-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }
    #region Overrides para la entidad Categoria
    public async Task<ResultadoValidacion> ValidarActualizar(Guid id, CategoriaActualizar actualizacion, Categoria original)
    {
        return new ResultadoValidacion() { Valido = true };
    }
    public async Task<ResultadoValidacion> ValidarEliminacion(Guid id, Categoria original)
    {
        var resultado = new ResultadoValidacion();

        var existeSubCategorias = _dbSetFull.Any(_ => _.CategoríaPadreId == id);
        if (existeSubCategorias)
        {
            resultado.Valido = false;
            resultado.Error = new ErrorProceso() 
            {
                Codigo = CodigosError.PRODUCTOS_CATEGORIA_ERROR_ELIMINAR_CATEGORIA_TIENE_HIJOS,
                Mensaje = "No se puede eliminar la categoría ya que tiene subcategorías",
                HttpCode = HttpCode.Conflict
            };
            return resultado;
        }

        resultado.Valido = true;
        return resultado;
    }
    public async Task<ResultadoValidacion> ValidarInsertar(CategoriaActualizar data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public override Categoria ADTOFull(CategoriaInsertar data)
    {
        return new Categoria()
        {
            Id = Guid.NewGuid(),
            IdiomaDefault = data.IdiomaDefault,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            CategoríaPadreId = data.CategoríaPadreId,
            URLImagen = data.URLImagem
        };
    }

    public override Categoria ADTOFull(CategoriaActualizar actualizacion, Categoria actual)
    {
        actual.IdiomaDefault = actualizacion.IdiomaDefault;
        actual.Nombre = actualizacion.Nombre;
        actual.Descripcion = actualizacion.Descripcion;
        actual.CategoríaPadreId = actualizacion.CategoríaPadreId;
        actual.URLImagen = actualizacion.URLImagem;
        return actual;
    }


    public override CategoriaDespliegue ADTODespliegue(Categoria data)
    {
        return new CategoriaDespliegue()
        {
            Id = data.Id,
            Nombre = data.Nombre,
            Descripcion = data.Descripcion,
            CategoríaPadreId = data.CategoríaPadreId,
            URLImagem = data.URLImagen
        };
    }

    public override async Task<RespuestaPayload<CategoriaDespliegue>> Insertar(CategoriaInsertar data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<CategoriaDespliegue>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                if (string.IsNullOrEmpty(entidad.IdiomaDefault)) { entidad.IdiomaDefault = entidad.Nombre[0].Idioma; }
                _dbSetFull.Add(entidad);
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
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }

    public async Task<Respuesta> Actualizar(Guid id, CategoriaActualizar data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (id == Guid.Empty || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.PRODUCTOS_CATEGORIA_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest,
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            Categoria actual = _dbSetFull.Find(id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.PRODUCTOS_CATEGORIA_NO_ENCONTRADA,
                    Mensaje = "No existe un Categoría con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            var resultadoValidacion = await ValidarActualizar(id, data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                if (string.IsNullOrEmpty(entidad.IdiomaDefault)) { entidad.IdiomaDefault = entidad.Nombre[0].Idioma; }
                _dbSetFull.Update(entidad);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.PRODUCTOS_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioCategoria-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.PRODUCTOS_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    public async Task<Respuesta> Eliminar(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (id == Guid.Empty)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.PRODUCTOS_CATEGORIA_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporciando el Id",
                    HttpCode = HttpCode.BadRequest,
                };
                return respuesta;
            }
            Categoria actual = _dbSetFull.Find(id);
            if (actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.PRODUCTOS_CATEGORIA_NO_ENCONTRADA,
                    Mensaje = "No existe un Categoria con el Id proporcionado",
                    HttpCode = HttpCode.NotFound,
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
                respuesta.Error!.Codigo = resultadoValidacion.Error?.Codigo?? CodigosError.PRODUCTOS_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioCategoria-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.PRODUCTOS_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    public async Task<RespuestaPayload<Categoria>> UnicaPorId(Guid id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Categoria>();
        try
        {
            Categoria actual = await _dbSetFull.FindAsync(id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.PRODUCTOS_CATEGORIA_NO_ENCONTRADA,
                    Mensaje = "No existe una Categoría con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioCategoria-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.PRODUCTOS_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    public async Task<RespuestaPayload<CategoriaDespliegue>> UnicaPorIdDespliegue(Guid id, StringDictionary? parametros = null)
    {
        RespuestaPayload<CategoriaDespliegue> respuesta = new RespuestaPayload<CategoriaDespliegue>();
        try
        {
            var resultado = await UnicaPorId(id, parametros);
            respuesta.Ok = resultado.Ok;
            if (resultado.Ok)
            {
                respuesta.Payload = ADTODespliegue((Categoria)resultado.Payload);
            }
            else
            {
                respuesta.Error = resultado.Error;
                respuesta.Error!.Codigo = CodigosError.PRODUCTOS_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultado.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioCategoria-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.PRODUCTOS_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    #endregion
}
