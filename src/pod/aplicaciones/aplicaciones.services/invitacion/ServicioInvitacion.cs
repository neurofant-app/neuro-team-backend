#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using api.comunicaciones;
using extensibilidad.metadatos;
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using aplicaciones.model;
using aplicaciones.services.dbContext;
using aplicaciones.services.extensiones;
using comunicaciones.modelo;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Text;
using System.Text.Json;
using comunes.primitivas;
using aplicaciones.services.proxy.abstractions;
using aplicaciones.services.proxy;


namespace aplicaciones.services.invitacion;
[ServicioEntidadAPI(entidad: typeof(Invitacion))]
public class ServicioInvitacion : ServicioEntidadGenericaBase<Invitacion, InvitacionInsertar, InvitacionActualizar, InvitacionDesplegar, string>,
    IServicioEntidadAPI, IServicioInvitacion
{
    private DbContextAplicaciones localContext;
    private readonly IReflectorEntidadesAPI reflector;
    private readonly IConfiguration configuration;
    private readonly IProxyComunicacionesServices _proxyComunicacionesServices;
    public ServicioInvitacion(DbContextAplicaciones context, ILogger<ServicioInvitacion> logger, IReflectorEntidadesAPI Reflector, IDistributedCache cache, IConfiguration configuration, IProxyComunicacionesServices proxyComunicacionesServices) : base(context, context.Invitaciones, logger, Reflector, cache)
    {
        interpreteConsulta = new InterpreteConsultaMySQL();
        localContext = context;
        reflector = Reflector;
        this.configuration = configuration;
        _proxyComunicacionesServices = proxyComunicacionesServices;
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
        var add = data.Deserialize<InvitacionInsertar>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaPayload<object>>(System.Text.Json.JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        var update = data.Deserialize<InvitacionActualizar>(JsonAPIDefaults());
        return await this.Actualizar((string)id, update);
    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        return await this.Eliminar((string)id);
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaPayload<object>>(System.Text.Json.JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        var temp = await this.UnicaPorIdDespliegue((string)id);

        RespuestaPayload<object> respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaPayload<object>>(System.Text.Json.JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(System.Text.Json.JsonSerializer.Serialize(temp));

        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(System.Text.Json.JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaHijoAPI(Consulta consulta, string tipoPadre, string id)
    {
        var temp = await this.PaginaHijo(consulta, tipoPadre, id);
        RespuestaPayload<PaginaGenerica<object>> respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(System.Text.Json.JsonSerializer.Serialize(temp));
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaHijosDespliegueAPI(Consulta consulta, string tipoPadre, string id)
    {
        var temp = await this.PaginaHijosDespliegue(consulta, tipoPadre, id);
        RespuestaPayload<PaginaGenerica<object>> respuesta = System.Text.Json.JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(System.Text.Json.JsonSerializer.Serialize(temp));
        return respuesta;
    }
    #region Overrides para la personalización de la entidad Invitacion
    public override async Task<ResultadoValidacion> ValidarInsertar(InvitacionInsertar data)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;

        return resultado;
    }
    public override async Task<ResultadoValidacion> ValidarEliminacion(string id, Invitacion original)
    {
        ResultadoValidacion resultado = new();
        resultado.Valido = true;
        return resultado;
    }

    public override async Task<ResultadoValidacion> ValidarActualizar(string id, InvitacionActualizar actualizacion, Invitacion original)
    {
        ResultadoValidacion resultado = new();

        resultado.Valido = true;

        return resultado;
    }

    public override Invitacion ADTOFull(InvitacionActualizar actualizacion, Invitacion actual)
    {
        actual.Id = actualizacion.Id;
        actual.AplicacionId = actualizacion.AplicacionId;
        return actual;
    }

    public override Invitacion ADTOFull(InvitacionInsertar data)
    {
        Invitacion inv = new Invitacion()
        {
            Id = Guid.NewGuid(),
            AplicacionId = data.AplicacionId,
            Email = data.Email,
            RolId = data.RolId,
            Nombre = data.Nombre,
            Tipo = data.Tipo,
            Token = data.Token
        };
        return inv;
    }

    public override InvitacionDesplegar ADTODespliegue(Invitacion data)
    {
        InvitacionDesplegar invitacionDesplegar = new InvitacionDesplegar()
        {
            Id = data.Id,
            AplicacionId = data.AplicacionId,
            Fecha = data.Fecha,
            Estado = data.Estado,
            Email = data.Email,
            RolId= data.RolId,

        };
        return invitacionDesplegar;
    }

    public override async Task<(List<Invitacion> Elementos, int? Total)> ObtienePaginaElementos(Consulta consulta)
    {
        await Task.Delay(0);
        Entidad entidad = reflector.ObtieneEntidad(typeof(Invitacion));
        string query = interpreteConsulta.CrearConsulta(consulta, entidad, DbContextAplicaciones.TablaInvitaciones);

        int? total = null;  
        List<Invitacion> elementos = localContext.Invitaciones.FromSqlRaw(query).ToList();

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
            return new(new List<Invitacion>(), total); ;
        }
    }

    public override async Task<Respuesta> Actualizar(string id, InvitacionActualizar data)
    {
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }


            Invitacion actual = _dbSetFull.Find(Guid.Parse(id));

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


    public override async Task<RespuestaPayload<Invitacion>> UnicaPorId(string id)
    {
        var respuesta = new RespuestaPayload<Invitacion>();
        try
        {
            Invitacion actual = await _dbSetFull.FindAsync(Guid.Parse(id));
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

            Invitacion actual = _dbSetFull.Find(Guid.Parse(id));
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

    public override async Task<RespuestaPayload<InvitacionDesplegar>> Insertar(InvitacionInsertar data)
    {
        var respuesta = new RespuestaPayload<InvitacionDesplegar>();

        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                //para determinar el tipo de plantilla que se enviará, invitación, recuperación contraseña, etc.
                var tipoPlantillaContenido = TipoCOntenidoPlantilla(data.Tipo);

                PlantillaInvitacion plantillaInvitacion= await localContext.PlantillasAplicaciones.Where(x => x.AplicacionId == data.AplicacionId && x.TipoContenido == tipoPlantillaContenido).FirstOrDefaultAsync();
                LogoAplicacion logoAplicacion= await localContext.LogosAplicaciones.Where(x => x.AplicacionId == data.AplicacionId).FirstOrDefaultAsync();
                byte[] bytes = Convert.FromBase64String(plantillaInvitacion.Plantilla);
                string html = Encoding.UTF8.GetString(bytes);
                DatosPlantillaRegistro datosPlantilla = new DatosPlantillaRegistro()
                {
                    Activacion = entidad.Id.ToString(),
                    FechaLimite = entidad.Fecha.ToString(),
                    Nombre = entidad.Nombre,
                    UrlBase = configuration.LeeUrlBase(),
                    Logo64 = "data:image/jpeg;base64," + logoAplicacion.LogoURLBase64,
                    Remitente = entidad.Nombre,
                };


                api.comunicaciones.MensajeEmail m = new ()
                {
                    NombreDe = null,
                    DireccionDe = null,
                    DireccionPara = entidad.Email,
                    NombrePara = entidad.Nombre,
                    PlantillaTema = configuration.LeeTemaRegistro(),
                    PlantillaCuerpo = html,
                    JsonData = JsonConvert.SerializeObject(datosPlantilla),
                };

                var respuestaCorreo = await _proxyComunicacionesServices.EnviarCorreo(m);
                if (respuestaCorreo.Ok)
                {
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

    private TipoContenido TipoCOntenidoPlantilla(TipoComunicacion tipo)
    {
        switch (tipo)
        {
            case TipoComunicacion.Registro:
                return TipoContenido.Invitacion;
            case TipoComunicacion.RecuperacionContrasena:
                return TipoContenido.RecuperacionPassword;
            default:
                return TipoContenido.Invitacion;
        }
    }



    #endregion





}
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
#pragma warning restore CS8603 // Possible null reference return.
