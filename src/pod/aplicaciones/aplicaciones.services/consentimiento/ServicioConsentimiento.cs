#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using extensibilidad.metadatos;
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using comunes.primitivas;
using apigenerica.model.servicios;
using aplicaciones.model;
using aplicaciones.services.dbContext;
using aplicaciones.services.plantilla;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Logging;
using System.Text.Json;

namespace aplicaciones.services.consentimiento;
[ServicioEntidadAPI(entidad: typeof(Consentimiento))]

public class ServicioConsentimiento :ServicioEntidadGenericaBase<Consentimiento, Consentimiento, Consentimiento, Consentimiento, string>,
    IServicioEntidadAPI, IServicioConsentimiento
{
    private DbContextAplicaciones localContext;
    private readonly IReflectorEntidadesAPI reflector;

    public ServicioConsentimiento(DbContextAplicaciones context, ILogger<IServicioConsentimiento> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache) : base(context, context.Consentimientos, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
        reflector = Reflector;
    }
    private DbContextAplicaciones DB { get { return (DbContextAplicaciones)_db; } }
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
        var add = data.Deserialize<Consentimiento>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<Consentimiento>(JsonAPIDefaults());
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
    #region Overrides para la personalización de la entidad PlantillaAplicacion
    public override async Task<ResultadoValidacion> ValidarInsertar(Consentimiento data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Consentimiento original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, Consentimiento actualizacion, Consentimiento original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override Consentimiento ADTOFull(Consentimiento actualizacion, Consentimiento actual)
    {
        actual.Id = actualizacion.Id;
        actual.AplicacionId = actualizacion.AplicacionId;
        actual.Tipo = actualizacion.Tipo;
        actual.Idioma = actualizacion.Idioma;
        actual.IdiomaDefault = actualizacion.IdiomaDefault;
        actual.Texto = actualizacion.Texto;
        return actual;
    }

    public override Consentimiento ADTOFull(Consentimiento data)
    {
        Consentimiento consentimiento = new Consentimiento()
        {
            Id = Guid.NewGuid(),
            AplicacionId = data.AplicacionId,
            Tipo = data.Tipo,
            Idioma = data.Idioma,
            IdiomaDefault = data.IdiomaDefault,
            Texto = data.Texto,
        };
        return consentimiento;
    }

    public override Consentimiento ADTODespliegue(Consentimiento data)
    {
        Consentimiento consentimiento = new Consentimiento()
        {
            Id = data.Id,
            AplicacionId = data.AplicacionId,
            Tipo = data.Tipo,
            Idioma = data.Idioma,
            IdiomaDefault = data.IdiomaDefault,
            Texto = data.Texto,

        };
        return consentimiento;
    }

    public override async Task<(List<Consentimiento> Elementos, int? Total)> ObtienePaginaElementos(Consulta consulta)
    {
        await Task.Delay(0);
        Entidad entidad = reflector.ObtieneEntidad(typeof(Consentimiento));
        string query = interpreteConsulta.CrearConsulta(consulta, entidad, DbContextAplicaciones.TablaConsentimientos);

        int? total = null;
        List<Consentimiento> elementos = localContext.Consentimientos.FromSqlRaw(query).ToList();

        if (consulta.Contar)
        {
            query = query.Split("ORDER")[0];
            query = $"{query.Replace("*", "count(*)")}";
            total = localContext.Database.SqlQueryRaw<int>(query).ToArray().First();
        }


        if (elementos != null)
        {
            return new(elementos, total);
        }
        else
        {
            return new(new List<Consentimiento>(), total); ;
        }
    }

    public override async Task<Respuesta> Actualizar(string id, Consentimiento data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Consentimiento actual = _dbSetFull.Find(Guid.Parse(id));

            if (actual == null)
            {
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


    public override async Task<RespuestaPayload<Consentimiento>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Consentimiento>();
        try
        {
            Consentimiento actual = await _dbSetFull.FindAsync(Guid.Parse(id));
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

            Consentimiento actual = _dbSetFull.Find(Guid.Parse(id));
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

    #endregion
}
