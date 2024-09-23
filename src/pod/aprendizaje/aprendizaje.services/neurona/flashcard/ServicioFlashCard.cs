﻿#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
using apigenerica.model.interpretes;
using apigenerica.model.modelos;
using apigenerica.model.reflectores;
using apigenerica.model.servicios;
using aprendizaje.model.flashcard;
using aprendizaje.model.neurona;
using comunes.primitivas;
using comunes.primitivas.configuracion.mongo;
using extensibilidad.metadatos;
using FluentStorage;
using FluentStorage.Blobs;
using FluentStorage.ConnectionString;
using Microsoft.AspNetCore.Identity.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;
using System.Text.Json;

namespace aprendizaje.services.neurona.flashcard;

[ServicioEntidadAPI(typeof(FlashCard))]
public class ServicioFlashCard : ServicioEntidadHijoGenericaBase<FlashCard, FlashCard, FlashCard, FlashCard, string>,
    IServicioEntidadHijoAPI, IServicioFlashCard
{
    private readonly ILogger<ServicioFlashCard> _logger;
    private readonly IReflectorEntidadesAPI _reflector;
    private IBlobStorage blobStorage;
    private readonly IConfiguration configuration;
    public Neurona? neurona;
    public DbSet<Neurona> _dbSetNeurona;


    public ServicioFlashCard(ILogger<ServicioFlashCard> logger,
                             IReflectorEntidadesAPI reflector,
                             IServicionConfiguracionMongo configuracionMongo,
                             IDistributedCache cache,
                             IBlobStorage blobStorage,
                             IConfiguration configuration)
                             : base(null, null, logger, reflector, cache)
    {
        this._logger = logger;
        this._reflector = reflector;
        this.blobStorage = blobStorage;
        this.configuration = configuration;
        interpreteConsulta = new InterpreteConsultaExpresiones();

        var configuracionEntidad = configuracionMongo.ConexionEntidad(MongoDbContextAprendizaje.NOMBRE_COLECCION_NEURONA);
        if (configuracionEntidad == null)
        {
            string err = $"No se pudo establecer conexión con {MongoDbContextAprendizaje.NOMBRE_COLECCION_NEURONA}";
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
            _dbSetNeurona = ((MongoDbContextAprendizaje)_db).Neurona;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error al inicializar mongo para '{MongoDbContextAprendizaje.NOMBRE_COLECCION_NEURONA}'");
            throw;
        }
    }
    private MongoDbContextAprendizaje DB { get { return (MongoDbContextAprendizaje)_db; } }

    public bool RequiereAutenticacion => true;

    string IServicioEntidadHijoAPI.TipoPadreId { get => this.TipoPadreId; set => this.TipoPadreId = value; }

    string IServicioEntidadHijoAPI.Padreid { get => this.neurona.Id.ToString() ?? null; set => EstableceDbSet(value); }
    public void EstableceDbSet(string padreId)
    {
        _logger.LogDebug("ServicioFlashCard-EstableceDbSet - {padreId}", padreId);
        neurona = _dbSetNeurona.FirstOrDefault(_ => _.Id.Equals(new Guid(padreId)));
        this.Padreid = neurona != null ? neurona.Id.ToString() : null;
        _logger.LogDebug("ServicioFlashCard-EstableceDbSet - resultado {padreId}", this.Padreid);
    }

    public Entidad EntidadRepoAPI()
    {
        _logger.LogDebug("ServicioFlashCard - EntidadRepoAPI");
        return this.EntidadRepo();
    }

    public Entidad EntidadInsertAPI()
    {
        _logger.LogDebug("ServicioFlashCard - EntidadInsertAPI");
        return this.EntidadInsert();
    }

    public Entidad EntidadUpdateAPI()
    {
        _logger.LogDebug("ServicioFlashCard - EntidadUpdateAPI");
        return this.EntidadUpdate();
    }

    public Entidad EntidadDespliegueAPI()
    {
        _logger.LogDebug("ServicioFlashCard - EntidadDespliegueAPI");
        return this.EntidadDespliegue();
    }

    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        _logger.LogDebug("ServicioFlashCard - EstableceContextoUsuarioAPI");
        this.EstableceContextoUsuario(contexto);
    }

    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        _logger.LogDebug("ServicioFlasCard - ObtieneContextoUsuarioAPI");
        return this.ObtieneContextoUsuario();
    }

    public async Task<RespuestaPayload<object>> InsertarAPI(JsonElement data)
    {
        _logger.LogDebug("ServicioFlashCard - InsertarAPI {data}", data);
        var add = data.Deserialize<FlashCard>(JsonAPIDefaults());
        var temp = await this.Insertar(add);
        RespuestaPayload<object> respuestaPayload = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioFlashCard - InsertarAPI {ok} {code} {error}", respuestaPayload!.Ok, respuestaPayload!.HttpCode, respuestaPayload.Error);
        return respuestaPayload;
    }

    public async Task<Respuesta> ActualizarAPI(object id, JsonElement data)
    {
        _logger.LogDebug("ServicioFlashCard - ActualizarAPI {data}", data);
        var update = data.Deserialize<FlashCard>(JsonAPIDefaults());
        Respuesta respuesta = await this.Actualizar((string)id, update);
        _logger.LogDebug("ServicioFlashCard - ActualizarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;

    }

    public async Task<Respuesta> EliminarAPI(object id)
    {
        _logger.LogDebug("ServicioFlashCard - EliminarAPI {id}", id);
        Respuesta respuesta = await this.Eliminar((string)id);
        _logger.LogDebug("ServicioFlashCard - EliminarAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdAPI(object id)
    {
        _logger.LogDebug("ServicioFlashCard - UnicaPorIdAPI {id}", id);
        var temp = await this.UnicaPorId((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioFlashCard - UnicaPorIdAPI {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<object>> UnicaPorIdDespliegueAPI(object id)
    {
        _logger.LogDebug("ServicioFlashCard - UnicaPorIdDespliegueAPI");
        var temp = await this.UnicaPorIdDespliegue((string)id);
        RespuestaPayload<object> respuesta = JsonSerializer.Deserialize<RespuestaPayload<object>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioFlashCard - UnicaPorIdDespliegueAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioFlashCard-PaginaAPI-{consulta}", consulta);
        var temp = await this.Pagina(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioFlashCard - PaginaAPI resultado {ok} {code} {error}", respuesta!.Ok, respuesta!.HttpCode, respuesta.Error);
        return respuesta;
    }

    public async Task<RespuestaPayload<PaginaGenerica<object>>> PaginaDespliegueAPI(Consulta consulta)
    {
        _logger.LogDebug("ServicioFlashCard - PaginaDespliegueAPI {consulta}", consulta);
        var temp = await this.PaginaDespliegue(consulta);
        RespuestaPayload<PaginaGenerica<object>> respuesta = JsonSerializer.Deserialize<RespuestaPayload<PaginaGenerica<object>>>(JsonSerializer.Serialize(temp));
        _logger.LogDebug("ServicioFlashCard - PaginaDespliegueAPI {consulta}", consulta);
        return respuesta;
    }

    #region Overrides para la personalización de la ENTIDAD => FlashCard hijo de Neurona
    public override async Task<ResultadoValidacion> ValidarInsertar(FlashCard data)
    {
        var settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value + neurona?.Id;
        this.blobStorage = StorageFactory.Blobs.DirectoryFiles(Path.Combine(settingsPath, "flashcard"));
        var pathtemp = Path.Combine(Path.Combine(settingsPath, "flashcard"), data.Id.ToString() + ".json");
        var existe = this.blobStorage.ExistsAsync(pathtemp).Result;
        if (existe == true)
        {

            return new ResultadoValidacion()
            {
                Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_FLASHCARD_ERROR_INSERTAR,
                    Mensaje = "Ya existe el FlashCard con el ID proporcionado",
                    HttpCode = HttpCode.NotFound
                },
                Valido = false
            };
        }

        return new ResultadoValidacion()
        {
            Valido = true,
        };
    }

    public async Task<ResultadoValidacion> ValidacionEliminarFlashCard(string id)
    {
        var settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value + neurona?.Id;
        this.blobStorage = StorageFactory.Blobs.DirectoryFiles(Path.Combine(settingsPath, "flashcard"));
        var pathtemp = Path.Combine(Path.Combine(settingsPath, "flashcard"), id + ".json");
        var existe = this.blobStorage.ExistsAsync(pathtemp).Result;
        if (existe == false)
        {

            return new ResultadoValidacion()
            {
                Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_FLASHCARD_ERROR_ELIMINAR,
                    Mensaje = "No existe el FlashCard con el ID proporcionado",
                    HttpCode = HttpCode.NotFound
                },
                Valido = false
            };
        }

        return new ResultadoValidacion()
        {
            Valido = true,
        };
    }

    public async Task<ResultadoValidacion> ValidacionActualizarFlashCard(string id, FlashCard actualizacion)
    {
        var settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value + neurona?.Id;
        this.blobStorage = StorageFactory.Blobs.DirectoryFiles(Path.Combine(settingsPath, "flashcard"));
        var pathtemp = Path.Combine(Path.Combine(settingsPath, "flashcard"), id + ".json");
        var existe = this.blobStorage.ExistsAsync(pathtemp).Result;
        if (existe == false)
        {

            return new ResultadoValidacion()
            {
                Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_FLASHCARD_ERROR_ACTUALIZAR,
                    Mensaje = "No existe el FlashCard con el ID proporcionado",
                    HttpCode = HttpCode.NotFound
                },
                Valido = false
            };
        }

        return new ResultadoValidacion()
        {
            Valido = true,
        };

    }

    public override FlashCard ADTOFull(FlashCard data)
    {
        return new FlashCard()
        {
            Id = data.Id,
            NeuronaId = data.NeuronaId,
            TemaId = data.TemaId,
            TimeStampt = data.TimeStampt,
            TipoContenido = data.TipoContenido,
            TipoConcepto = data.TipoConcepto,
            ContenidoPersonalizadoId = data.ContenidoPersonalizadoId,
            ContenidoGaleria = data.ContenidoGaleria,
            Concepto = data.Concepto,
            Contenido = data.Contenido,
            TextoTTS = data.TextoTTS
        };
    }

    public override FlashCard ADTODespliegue(FlashCard data)
    {
        return new FlashCard()
        {
            Id = data.Id,
            NeuronaId = data.NeuronaId,
            TemaId = data.TemaId,
            TimeStampt = data.TimeStampt,
            TipoContenido = data.TipoContenido,
            TipoConcepto = data.TipoConcepto,
            ContenidoPersonalizadoId = data.ContenidoPersonalizadoId,
            ContenidoGaleria = data.ContenidoGaleria,
            Concepto = data.Concepto,
            Contenido = data.Contenido,
            TextoTTS = data.TextoTTS
        };
    }

    public override async Task<RespuestaPayload<FlashCard>> Insertar(FlashCard data)
    {
        var settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value + neurona?.Id;

        var respuesta = new RespuestaPayload<FlashCard>();
        try
        {
            var resultadoValidacion = await ValidarInsertar(data);
            if (resultadoValidacion.Valido)
            {
                var entidad = ADTOFull(data);
                FlashcardNeurona flashcardNeurona = new FlashcardNeurona()
                {
                    FlashcardId = data.Id,
                    Estado = neurona.EstadoPublicacion,
                    UsuarioId = this._contextoUsuario.UsuarioId,
                    TemaId = data.TemaId,
                    TipoConcepto = data.TipoConcepto,
                    TipoContenido = data.TipoContenido,
                    TimeStampt = data.TimeStampt

                };
                neurona.Flashcards.Add(flashcardNeurona);
                _dbSetNeurona.Update(neurona);
                this.blobStorage = StorageFactory.Blobs.DirectoryFiles(Path.Combine(settingsPath, "flashcard"));
                await this.blobStorage.WriteJsonAsync(Path.Combine(Path.Combine(settingsPath, "flashcard"), entidad.Id.ToString() + ".json"  ), entidad);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = ADTODespliegue(entidad);
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = CodigosError.APRENDIZAJE_DATOS_NO_VALIDOS;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioFlashCard-Insertar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Actualizar(string id, FlashCard data)
    {
        var settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value + neurona?.Id;
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id.ToString()) || data == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_FLASHCARD_ID_PAYLOAD_NO_INGRESADO,
                    Mensaje = "No ha sido procionado el Id ó Payloasd",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }
            this.blobStorage = StorageFactory.Blobs.DirectoryFiles(Path.Combine(settingsPath, "flashcard"));
            var pathtemp = Path.Combine(Path.Combine(settingsPath, "flashcard"), id + ".json");

            var resultadoValidacion = await ValidacionActualizarFlashCard(id.ToString(), data);

            var existe = this.blobStorage.ExistsAsync(pathtemp).Result;
            if(resultadoValidacion.Valido == false)
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.Error!.Codigo = resultadoValidacion.Error!.Codigo;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }

            await this.blobStorage.DeleteAsync(pathtemp);
            await this.blobStorage.WriteJsonAsync(pathtemp, data);
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "ServicioFlashCard-Actualizar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<Respuesta> Eliminar(string id)
    {
        var settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value + neurona?.Id;
        var respuesta = new Respuesta();
        try
        {
            if (string.IsNullOrEmpty(id))
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_FLASHCARD_ID_NO_INGRESADO,
                    Mensaje = "No ha sido proporcionado el Id",
                    HttpCode = HttpCode.BadRequest
                };
                respuesta.HttpCode = HttpCode.BadRequest;
                return respuesta;
            }

            FlashcardNeurona actual = neurona.Flashcards.FirstOrDefault(_ => _.FlashcardId == long.Parse(id));

            this.blobStorage = StorageFactory.Blobs.DirectoryFiles(Path.Combine(settingsPath, "flashcard"));
            var pathtemp = Path.Combine(Path.Combine(settingsPath, "flashcard"), id + ".json");
            var resultadoValidacion = await this.ValidacionEliminarFlashCard(id);
            if (resultadoValidacion.Valido == false && actual == null)
            {
                respuesta.Error = new ErrorProceso()
                {
                    Codigo = CodigosError.APRENDIZAJE_FLASHCARD_NO_ENCONTRADA,
                    Mensaje = "No existe el FlashCard con el ID proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }

            neurona.Flashcards.Remove(actual);
            _dbSetNeurona.Update(neurona);
            await _db.SaveChangesAsync();
            await this.blobStorage.DeleteAsync(pathtemp);
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;

        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioFlashCard-Eliminar {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    public override async Task<RespuestaPayload<FlashCard>> UnicaPorId(string id)
    {
        var settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value + neurona?.Id;
        var respuesta = new RespuestaPayload<FlashCard>();
        try
        {
            this.blobStorage = StorageFactory.Blobs.DirectoryFiles(Path.Combine(settingsPath,"flashcard"));
            var pathtemp = Path.Combine(Path.Combine(settingsPath, "flashcard"), id + ".json");
            var actual = await this.blobStorage.ReadJsonAsync<FlashCard>(pathtemp);

            if (actual == null)
            {
                respuesta.Error = new()
                {
                    Codigo = CodigosError.APRENDIZAJE_FLASHCARD_NO_ENCONTRADA,
                    Mensaje = "No se encontró FlashCard con el Id proporcionado",
                    HttpCode = HttpCode.NotFound
                };
                respuesta.HttpCode = HttpCode.NotFound;
                return respuesta;
            }
            respuesta.Ok = true;
            respuesta.HttpCode = HttpCode.Ok;
            respuesta.Payload = actual;
        }
        catch(Exception ex)
        {
            _logger.LogError(ex, "ServicioFlashCard-UnicaPorId {msg}", ex.Message);
            respuesta.Error = new ErrorProceso() { Codigo = CodigosError.APRENDIZAJE_ERROR_DESCONOCIDO, HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }



    #endregion
}
#pragma warning disable CS8603 // Possible null reference return.
#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.