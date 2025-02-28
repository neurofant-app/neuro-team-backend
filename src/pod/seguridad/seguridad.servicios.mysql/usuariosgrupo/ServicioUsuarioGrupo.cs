﻿#pragma warning disable CS8603 // Possible null reference return.
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
using seguridad.modelo.roles;
using seguridad.modelo.servicios;
using System.Collections.Specialized;
using System.Text.Json;



namespace seguridad.servicios.mysql;
[ServicioEntidadAPI(entidad: typeof(UsuarioGrupo), driver: Constantes.MYSQL)]
public class ServicioUsuarioGrupo : ServicioEntidadGenericaBase<UsuarioGrupo, CreaUsuarioGrupo, UsuarioGrupo, ConsultaUsuarioGrupo, string>,
    IServicioEntidadAPI, IServicioUsuarioGrupo
{
    private readonly ILogger _logger;

    private readonly IReflectorEntidadesAPI reflector;
    private GrupoUsuarios? grupo;
    private DbSet<GrupoUsuarios>? _dbSetgrupoUsuarios;
    public ServicioUsuarioGrupo(DBContextMySql context, ILogger<ServicioRolMysql> logger,
        IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(null, null, logger, Reflector, cache)
    {
        _dbSetgrupoUsuarios = context.GrupoUsuarios;
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
        var add = data.Deserialize<CreaUsuarioGrupo>(JsonAPIDefaults());
        var temp = await this.Insertar(add, parametros);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data, StringDictionary? parametros = null)
    {
        throw new NotImplementedException();
    }

    public async Task<Respuesta> EliminarAPI(object id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        return await this.Eliminar((string)id, parametros, forzarEliminacion);
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
    public override async Task<ResultadoValidacion> ValidarInsertar(CreaUsuarioGrupo data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = grupo != null && !grupo.UsuarioId.Any(_=>_==data.UsuarioId);
        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, UsuarioGrupo original, bool forzarEliminacion = false)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = grupo != null ? true : false;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, UsuarioGrupo actualizacion, UsuarioGrupo original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override UsuarioGrupo ADTOFull(UsuarioGrupo actualizacion, UsuarioGrupo actual)
    {
        return actual;
    }

    public override UsuarioGrupo ADTOFull(CreaUsuarioGrupo data)
    {
        UsuarioGrupo rol = new UsuarioGrupo()
        {
            Id = Guid.NewGuid().ToString(),
            UsuarioId = data.UsuarioId
        };
        return rol;
    }
    public override ConsultaUsuarioGrupo ADTODespliegue(UsuarioGrupo data)
    {
        return new ConsultaUsuarioGrupo
        {
          UsuarioId= data.UsuarioId
        };
    }
    public override async Task<RespuestaPayload<ConsultaUsuarioGrupo>> Insertar(CreaUsuarioGrupo data, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<ConsultaUsuarioGrupo>();
        grupo = _dbSetgrupoUsuarios.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                grupo.UsuarioId.Add(entidad.UsuarioId);
                _dbSetgrupoUsuarios.Update(grupo);
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

    public override async Task<RespuestaPayload<UsuarioGrupo>> UnicaPorId(string id, StringDictionary? parametros = null)
    {
        var respuesta = new RespuestaPayload<UsuarioGrupo>();
        try
        {
            grupo = _dbSetgrupoUsuarios.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            string actual = grupo.UsuarioId.FirstOrDefault(_=>_==id);
            if (string.IsNullOrEmpty(actual))
            {                
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = respuesta.Payload = new UsuarioGrupo() {Id = Guid.NewGuid().ToString(), UsuarioId = actual };
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

    public override async Task<Respuesta> Eliminar(string id, StringDictionary? parametros = null, bool forzarEliminacion = false)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            grupo = _dbSetgrupoUsuarios.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
            var actual = grupo.UsuarioId.FirstOrDefault(_ => _ == id);
            if (string.IsNullOrEmpty(actual))
            {
                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                return respuesta;
            }
            UsuarioGrupo usuario= new UsuarioGrupo() {Id = Guid.NewGuid().ToString(), UsuarioId = actual };
            var resultadoValidacion = await ValidarEliminacion(id, usuario);
            if (resultadoValidacion.Valido)
            {
                grupo.UsuarioId.Remove(actual);
                _dbSetgrupoUsuarios.Update(grupo);
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
    public override async Task<PaginaGenerica<UsuarioGrupo>> ObtienePaginaElementos(Consulta consulta, StringDictionary? parametros = null)
    {
        grupo = _dbSetgrupoUsuarios.FirstOrDefault(_ => _.Id == Guid.Parse(parametros["n0Id"]));
        var Elementos =grupo!=null && grupo.UsuarioId.Any() ? grupo.UsuarioId.AsQueryable() : new List<string>().AsQueryable();
        var ElementosFinal = new List<UsuarioGrupo>();
        var pagina = Elementos.Paginado(consulta);
        if (pagina.Elementos.Any()) pagina.Elementos.ForEach(i => { ElementosFinal.Add(new UsuarioGrupo() {Id = Guid.NewGuid().ToString() , UsuarioId = i }); });
        return new PaginaGenerica<UsuarioGrupo>
        {
            ConsultaId = pagina.ConsultaId,
            Elementos = ElementosFinal,
            Milisegundos = 0,
            Paginado = pagina.Paginado,
            Total = pagina.Total,
        };
    }

    #endregion
}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
