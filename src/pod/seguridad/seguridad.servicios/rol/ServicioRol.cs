#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using seguridad.modelo;
using seguridad.modelo.instancias;
using seguridad.modelo.roles;
using seguridad.modelo.servicios;
using seguridad.servicios.dbcontext;
using System.Text.Json;


namespace seguridad.servicios;
[ServicioEntidadAPI(entidad: typeof(Rol), driver: Constantes.MONGODB)]
public class ServicioRol : ServicioEntidadHijoGenericaBase<Rol, CreaRol, ActualizaRol, ConsultaRol, string>,
    IServicioEntidadHijoAPI, IServicioRol
{
    private readonly ILogger _logger;
    private readonly IReflectorEntidadesAPI reflector;
    private InstanciaAplicacion? aplicacion;
    private DbSet<InstanciaAplicacion>? _dbSetAplicacion;
    public ServicioRol(ILogger<ServicioRol> logger,
        IServicionConfiguracionMongo configuracionMongo,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache)
    {
        _logger = logger;
        reflector = Reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContext.NOMBRE_COLECCION_INSTANCIAAPLICAION);
        if (configuracionEntidad == null)
        {
            string err = $"No existe configuracion de mongo para '{MongoDbContext.NOMBRE_COLECCION_INSTANCIAAPLICAION}'";
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
            _dbSetAplicacion = ((MongoDbContext)_db).instanciaAplicacion;
            
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContext.NOMBRE_COLECCION_INSTANCIAAPLICAION}'");
            throw;
        }
    }
    private MongoDbContext DB { get { return (MongoDbContext)_db; } }
    public bool RequiereAutenticacion => true;

    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }
    string IServicioEntidadHijoAPI.Padreid { get => this.aplicacion.Id ?? null; set => EstableceDbSet(value); }
    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioRol-EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioRol-EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioRol-EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioRol-EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioRol-EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public void EstableceDbSet(string padreId)
    {
        _logger.LogDebug("ServicioRol-EstableceDbSet - {padreId}", padreId);
        aplicacion = _dbSetAplicacion.FirstOrDefault(_ => _.Id == padreId);
        this.Padreid= aplicacion != null?aplicacion.Id:null;
        _logger.LogDebug("ServicioRol-EstableceDbSet - resultado {padreId}", this.Padreid);
    }
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioRol-ObtieneContextoUsuarioAPI");
        return this._contextoUsuario;
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioRol-InsertarAPI-{data}", data); var add = data.Deserialize<CreaRol>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioRol-InsertarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioRol-ActualizarAPI-{data}", data);
        var update = data.Deserialize<ActualizaRol>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioRol-ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioRol-EliminarAPI");
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioRol-EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioRol-UnicaPorIdAPI");
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioRol-UnicaPorIdAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioRol-UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioRol-UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioRol-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioRol-PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioRol-PaginaDespliegueAPI-{consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioRol-PaginaDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    #region Overrides para la personalización de la entidad LogoAplicacion
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaRol data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = aplicacion != null && !aplicacion.RolesPersonalizados.Any(_ => _.Nombre == data.Nombre);
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Rol original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = aplicacion != null ? true : false;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaRol actualizacion, Rol original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = aplicacion != null && !aplicacion.RolesPersonalizados.Any(_ => _.Nombre == actualizacion.Nombre && _.RolId!=actualizacion.RolId);

        return resultado;
    }

    public override Rol ADTOFull(ActualizaRol actualizacion, Rol actual)
    {
            actual.Nombre = actualizacion.Nombre;
            actual.Descripcion = actualizacion.Descripcion;
        
        return actual;
    }

    public override Rol ADTOFull(CreaRol data)
    {
            Rol rol = new Rol()
            {
                RolId = Guid.NewGuid().ToString(),
                Nombre = data.Nombre,
                Descripcion = data.Descripcion,
                Permisos = new List<string>(),
                Personalizado = false
            };          
        return rol;
    }
    public override ConsultaRol ADTODespliegue(Rol data)
    {
        return new ConsultaRol
        {
             RolId=data.RolId,
             Nombre = data.Nombre,
             Descripcion = data.Descripcion,
             Permisos=data.Permisos,
             Personalizado=data.Personalizado
        };
    }
    public override async Task<RespuestaPayload<ConsultaRol>> Insertar(CreaRol data)
    {
        var respuesta = new RespuestaPayload<ConsultaRol>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                aplicacion.RolesPersonalizados.Add(entidad);
                _dbSetAplicacion.Update(aplicacion);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.SEGURIDAD_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioRol-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.SEGURIDAD_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }
    public override async Task<Respuesta> Actualizar(string id, ActualizaRol data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.SEGURIDAD_ROL_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Rol actual = aplicacion.RolesPersonalizados.FirstOrDefault(_=>_.RolId==data.RolId);

            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.SEGURIDAD_ROL_NO_ENCONTRADA,
                    Mensaje = "No existe un Rol con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarActualizar(id.ToString(), data, actual);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data, actual);
                var index = aplicacion.RolesPersonalizados.IndexOf(entidad);
                
                if(index>=0)
                {
                    aplicacion.RolesPersonalizados[index] = entidad;
                    _dbSetAplicacion.Update(aplicacion);
                    await _db.SaveChangesAsync();
                    respuesta.Ok = true;
                    respuesta.HttpCode = HttpCode.Ok;
                }
                else
                    {
                    respuesta.Error = new ErrorProceso()
                    {
                        Codigo = CodigosError.SEGURIDAD_ROL_NO_ENCONTRADA,
                        Mensaje = "No existe un Rol con el Id proporcionado",
                        HttpCode = HttpCode.NotFound
                    };
                    respuesta.HttpCode = HttpCode.NotFound;
                    return respuesta;
                }               
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.SEGURIDAD_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioRol-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.SEGURIDAD_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<Rol>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Rol>();
        try
        {
            Rol actual = aplicacion.RolesPersonalizados.FirstOrDefault(_ => _.RolId == id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.SEGURIDAD_ROL_NO_ENCONTRADA,
                    Mensaje = "No existe un Rol con el Id proporcionado",
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
            _logger.LogError(ex, "ServicioRol-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.SEGURIDAD_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
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
                    Codigo = CodigosError.SEGURIDAD_ROL_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id ó Payload",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Rol actual = aplicacion.RolesPersonalizados.FirstOrDefault(_=>_.RolId==id);
            if (actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.SEGURIDAD_ROL_NO_ENCONTRADA,
                    Mensaje = "No existe un Rol con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            var resultadoValidacion = await ValidarEliminacion(id, actual);
            if (resultadoValidacion.Valido)
            {
                aplicacion.RolesPersonalizados.Remove(actual);
                _dbSetAplicacion.Update(aplicacion);
                await _db.SaveChangesAsync();
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
            }
            else
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.SEGURIDAD_ROL_ERROR_ELIMINAR,
                    Mensaje = "No ha sido posible ELIMINAR el Rol",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioRol-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.SEGURIDAD_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }
    public override async Task<PaginaGenerica<Rol>> ObtienePaginaElementos(Consulta consulta)
    {
        _logger.LogDebug("ServicioRol - ObtienePaginaElementos - {consulta}", consulta);
        Entidad entidad = reflectorEntidades.ObtieneEntidad(typeof(Rol));
        var Elementos = Enumerable.Empty<Rol>().AsQueryable();
        if (aplicacion != null)
        {
            if (consulta.Filtros.Count > 0)
            {
                var predicateBody = interpreteConsulta.CrearConsultaExpresion<Rol>(consulta, entidad);

                if (predicateBody != null)
                {
                    var RConsulta = aplicacion.RolesPersonalizados.AsQueryable().Provider.CreateQuery<Rol>(predicateBody.getWhereExpression(aplicacion.RolesPersonalizados.AsQueryable().Expression));

                    Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);
                }
            }
            else
            {
                var RConsulta = aplicacion.RolesPersonalizados.AsQueryable();
                Elementos = RConsulta.OrdenarPor(consulta.Paginado.ColumnaOrdenamiento ?? "Id", consulta.Paginado.Ordenamiento ?? Ordenamiento.asc);

            }
        }
        return Elementos.Paginado(consulta);
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
