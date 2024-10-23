#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using comunes.primitivas;
using extensibilidad.metadatos;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using seguridad.modelo;
using seguridad.modelo.instancias;
using seguridad.modelo.roles;
using seguridad.modelo.servicios;
using System.Collections.Specialized;
using System.Text.Json;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace seguridad.servicios.mysql;
[ServicioEntidadAPI(entidad: typeof(Rol), driver: Constantes.MYSQL)]
public class ServicioRolMysql : ServicioEntidadGenericaBase<Rol, CreaRol, ActualizaRol, ConsultaRol, string>,
    IServicioEntidadAPI, IServicioRol
{
    private readonly ILogger _logger;

    private readonly IReflectorEntidadesAPI reflector;
    private InstanciaAplicacion? aplicacion;
    private DbSet<InstanciaAplicacion>? _dbSetAplicacion;
    public ServicioRolMysql(DBContextMySql context, ILogger<ServicioRolMysql> logger,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache)
    {
        _dbSetAplicacion = context.InstanciaAplicacion;
        _logger = logger;
        reflector = Reflector;
        interpreteConsulta = new InterpreteConsultaExpresiones();

    }

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

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data, StringDictionary? parametros = null)
    {
        var add = data.Deserialize<CreaRol>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        var update = data.Deserialize<ActualizaRol>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update, parametros);
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null)
    {
        return await this.Eliminar((string)id, parametros);
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorId((string)id, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id, StringDictionary? parametros = null)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id, parametros);

        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.Pagina(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));

        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta, StringDictionary? parametros = null)
    {
        var temp = await this.PaginaDespliegue(consulta, parametros);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    #region Overrides para la personalización de la entidad LogoAplicacion
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaRol data)
    {
        ResultadoValidacion resultado = new();
        aplicacion = _dbSetAplicacion.FirstOrDefault(_ => _.Id == data.InstanciaAplicacionId);
        resultado.Valido = aplicacion != null && !aplicacion.RolesPersonalizados.Any(_ => _.Nombre == data.Nombre);
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Rol original)
    {
        ResultadoValidacion resultado = new();
        aplicacion = _dbSetAplicacion.FirstOrDefault(_ => _.Id == original.InstanciaAplicacionId);
        resultado.Valido = aplicacion != null ? true : false;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, ActualizaRol actualizacion, Rol original)
    {
        ResultadoValidacion resultado = new();
        aplicacion = _dbSetAplicacion.FirstOrDefault(_ => _.Id == actualizacion.InstanciaAplicacionId);

        resultado.Valido = aplicacion != null && !aplicacion.RolesPersonalizados.Any(_ => _.Nombre == actualizacion.Nombre && _.RolId!=actualizacion.RolId);

        return resultado;
    }

    public override Rol ADTOFull(ActualizaRol actualizacion, Rol actual)
    {
            actual.Nombre = actualizacion.Nombre;
            actual.Descripcion = actualizacion.Descripcion;
            actual.InstanciaAplicacionId = actualizacion.InstanciaAplicacionId;
            actual.ModuloId = actualizacion.ModuloId;
            actual.RolGrupo = actualizacion.RolGrupo;
            actual.RolUsuario = actualizacion.RolUsuario;
        
        return actual;
    }

    public override Rol ADTOFull(CreaRol data)
    {
            Rol rol = new Rol()
            {
                RolId = Guid.NewGuid().ToString(),
                Nombre = data.Nombre,
                Descripcion = data.Descripcion,
                Personalizado = false,
                InstanciaAplicacionId=data.InstanciaAplicacionId,
                ModuloId=data.ModuloId                   
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
             Personalizado=data.Personalizado,
             InstanciaAplicacionId = data.InstanciaAplicacionId,
             ModuloId = data.ModuloId
        };
    }
    public override async Task<RespuestaPayload<ConsultaRol>> Insertar(CreaRol data, StringDictionary? parametros = null)
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
    public override async Task<Respuesta> Actualizar(string id, ActualizaRol data, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Rol actual = aplicacion.RolesPersonalizados.FirstOrDefault(_=>_.RolId==data.RolId);

            if (actual == null)
            {
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
            _logger.LogError($"Insertar {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    public override async Task<RespuestaPayload<Rol>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<Rol>();
        try
        {
            Rol actual = aplicacion.RolesPersonalizados.FirstOrDefault(_ => _.RolId == id);
            if (actual == null)
            {
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

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

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null)
    {
        var respuesta = new Respuesta();
        try
        {

            if (string.IsNullOrEmpty(id))
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            Rol actual = aplicacion.RolesPersonalizados.FirstOrDefault(_=>_.RolId==id);
            if (actual == null)
            {
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
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
    public override async Task<PaginaGenerica<Rol>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
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
