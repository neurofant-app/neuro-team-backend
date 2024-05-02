using apigenerica.model.modelos;
using comunes.primitivas;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace apigenerica.model.servicios;

#pragma warning disable CS8602 // Dereference of a possibly null reference.
/// <summary>
/// Clase base para el sercivio de catálogos genéricos
/// </summary>
public class ServicioCatalogoGenericoBase : IServicioCatalogoGenerico
{
    protected DbSet<ElementoCatalogo> _dbSetFull;
    protected DbContext _db;
    protected ContextoUsuario? _contextoUsuario;
    protected ILogger _logger;


    /// <summary>
    /// COnstructor para el servicio de catálogo
    /// </summary>
    /// <param name="db"></param>
    /// <param name="dbSetFull"></param>
    public ServicioCatalogoGenericoBase(DbContext db, DbSet<ElementoCatalogo> dbSetFull, ILogger logger)
    {
        _dbSetFull = dbSetFull;
        _db = db;
        _logger = logger;
    }

    /// <summary>
    /// Establece el contexto del usurio en sesión
    /// </summary>
    /// <param name="contexto"></param>
    public void EstableceContextoUsuario(ContextoUsuario? contexto)
    {
        this._contextoUsuario = contexto;
    }

    public ContextoUsuario? ObtieneContextoUsuario()
    {
        return _contextoUsuario;
    }

    /// <summary>
    /// Devuelve la lista completa de elemntos del catálogo en base al idioma
    /// </summary>
    /// <param name="Idioma">Idioma del catálogo</param>
    /// <returns></returns>
    public virtual async Task<RespuestaPayload<List<ParClaveTexto>>> Todo(string catalogoId, string? idioma)
    {
        RespuestaPayload<List<ParClaveTexto>> respuesta = new();
        try
        {

            List<ParClaveTexto> payload = await _dbSetFull.Where
                (c => c.DominioId == _contextoUsuario.DominioId
                && c.UnidadOrganizacionalId == _contextoUsuario.UOrgId
                && c.CatalogoId == catalogoId
                && c.Idioma == idioma).OrderBy(c => c.Texto)
            .Select(x => new ParClaveTexto() { Clave = x.Id, Texto = x.Texto }).ToListAsync();

            respuesta = new()
            {
                Ok = true,
                HttpCode = HttpCode.Ok,
                Payload = payload
            };

        }
        catch (Exception ex)
        {
            _logger.LogError($"LLamada Todo {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    /// <summary>
    /// Devuelve una lista de entradas que contienen el texto buscad
    /// </summary>
    /// <param name="idioma">Idioma del catálogo</param>
    /// <param name="buscar">Texto a buscar</param>
    /// <param name="catalogoId">Texto a buscar</param>
    /// <returns></returns>
    public virtual async Task<RespuestaPayload<List<ParClaveTexto>>> PorTexto(string catalogoId, string? idioma, string? buscar)
    {
        RespuestaPayload<List<ParClaveTexto>> respuesta = new();
        try
        {
            List<ParClaveTexto> payload = new();
            if (!string.IsNullOrEmpty(buscar))
            {
                payload = await _dbSetFull.Where(c => c.DominioId == _contextoUsuario.DominioId
                && c.UnidadOrganizacionalId == _contextoUsuario.UOrgId
                && c.Idioma == idioma && c.CatalogoId == catalogoId
                && buscar.Contains(c.Texto, StringComparison.InvariantCultureIgnoreCase))
                .OrderBy(c => c.Texto)
                .Select(x => new ParClaveTexto() { Clave = x.Id, Texto = x.Texto }).ToListAsync();
            };

            respuesta = new()
            {
                Ok = true,
                HttpCode = HttpCode.Ok,
                Payload = payload
            };
        }
        catch (Exception ex)
        {
            _logger.LogError($"LLamada  PorTexto {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }


    /// <summary>
    /// Crea una nueva entrada nueva en el catálogo
    /// </summary>
    /// <param name="elemento"></param>
    /// <param name="catalogoId"></param>
    /// <returns></returns>
    public virtual async Task<RespuestaPayload<ElementoCatalogo>> CreaEntrada(string catalogoId, ElementoCatalogoInsertar elemento)
    {
        var respuesta = new RespuestaPayload<ElementoCatalogo>();
        try
        {
            var elementoNuevo = ADTOFull(catalogoId, elemento);
            var resultadoValidacion = await ValidarInsertar(elemento);
            if (resultadoValidacion.Valido)
            {

                _dbSetFull.Add(elementoNuevo);
                await _db.SaveChangesAsync();

                respuesta.Ok = true;
                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Payload = elementoNuevo;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"LLamada CreaEntrada {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }
        return respuesta;
    }

    /// <summary>
    /// Elimina una entrada del catálogo para todos los idiomas
    /// </summary>
    /// <param name="Id">Identificador único de la entrada</param>
    /// <param name="catalogoId">Identificador único de la entrada</param>
    /// <returns></returns>
    public virtual async Task<Respuesta> EliminaEntrada(string catalogoId, string id)
    {
        var respuesta = new Respuesta();
        try
        {
            var elemento = await _dbSetFull.FirstOrDefaultAsync(x => x.DominioId == _contextoUsuario.DominioId
                   && x.UnidadOrganizacionalId == _contextoUsuario.UOrgId
                   && x.CatalogoId == catalogoId
                   && x.Id == id);

            if (elemento == null)
            {
                respuesta.Error = "id".ErrorProcesoNoEncontrado();
                return respuesta;
            }

            var resultado = await ValidarEliminacion(id, elemento);
            if (resultado.Valido)
            {
                _dbSetFull.Remove(elemento);
                await _db.SaveChangesAsync();
                respuesta = new() { Ok = true, HttpCode = HttpCode.Ok };

            }
            else
            {
                respuesta = new()
                {
                    HttpCode = resultado.Error.HttpCode,
                    Ok = false,
                    Error = resultado.Error
                };
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"LLamada  EliminaEntrada {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }


    /// <summary>
    /// Actualiza una entrada en el catálogo por idioma
    /// </summary>
    /// <param name="Id">Identificador único de la entrada</param>
    /// <param name="idioma">Idioma de la entrada</param>
    /// <param name="texto">texto para la enrada</param>
    /// <returns></returns>
    public async Task<Respuesta> ActualizaEntrada(string catalogoId, string id, ElementoCatalogoActualizar elementoActualizar)
    {
        Respuesta respuesta = new();
        try
        {
            var elemento = await _dbSetFull.FirstOrDefaultAsync(x => x.DominioId == _contextoUsuario.DominioId
                       && x.UnidadOrganizacionalId == _contextoUsuario.UOrgId && x.Id == elementoActualizar.Id && x.CatalogoId == catalogoId);

            if (elemento == null)
            {
                respuesta.Error = "id".ErrorProcesoNoEncontrado();
                return respuesta;
            }

            var resultadoValidacion = await ValidarActualizar(id, elementoActualizar, elemento);
            if (resultadoValidacion.Valido)
            {
                elemento = ADTOFull(elementoActualizar, elemento);
                _dbSetFull.Update(elemento);
                await _db.SaveChangesAsync();

                respuesta.HttpCode = HttpCode.Ok;
                respuesta.Ok = true;
            }
            else
            {
                respuesta.Error = resultadoValidacion.Error;
                respuesta.HttpCode = resultadoValidacion.Error?.HttpCode ?? HttpCode.None;
            }
        }
        catch (Exception ex)
        {
            _logger.LogError($"LLamada ActualizaEntrada {ex.Message}");
            _logger.LogError($"{ex}");

            respuesta.Error = new ErrorProceso() { Codigo = "", HttpCode = HttpCode.ServerError, Mensaje = ex.Message };
            respuesta.HttpCode = HttpCode.ServerError;
        }

        return respuesta;
    }



    /// <summary>
    /// Devuelve la lista de idiomas disponibles para el catálogo
    /// </summary>
    /// <returns></returns>
    public async Task<List<string>> Idiomas()
    {
        return await _dbSetFull.Select(x => x.Idioma).Distinct().OrderBy(x => x).ToListAsync();
    }

    public virtual async Task<ResultadoValidacion> ValidarActualizar(string id, ElementoCatalogoActualizar actualizacion, ElementoCatalogo original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public virtual async Task<ResultadoValidacion> ValidarEliminacion(string id, ElementoCatalogo original)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public virtual async Task<ResultadoValidacion> ValidarInsertar(ElementoCatalogoInsertar data)
    {
        return new ResultadoValidacion() { Valido = true };
    }

    public virtual ElementoCatalogo ADTOFull(string catalogoId, ElementoCatalogoInsertar data)
    {
        ElementoCatalogo el = new()
        {
            Id = Guid.NewGuid().ToString(),
            Texto = data.Texto,
            Idioma = data.Idioma,
            DominioId = _contextoUsuario.DominioId,
            UnidadOrganizacionalId = _contextoUsuario.UOrgId,
            CatalogoId = catalogoId
        };
        return el;
    }


    public virtual ElementoCatalogo ADTOFull(ElementoCatalogoActualizar data, ElementoCatalogo elemento)
    {
        elemento.Texto = data.Texto;
        return elemento;
    }


    /// <summary>
    /// Establece el contexto de ejecicón del usaurio en sesión
    /// </summary>
    /// <param name="contexto"></param>
    public void EstableceContextoUsuarioAPI(ContextoUsuario contexto)
    {
        this._contextoUsuario = contexto;
    }

    /// <summary>
    /// Obtiene el contexto de ejecución del usuario en sesión
    /// </summary>
    /// <returns></returns>
    public ContextoUsuario? ObtieneContextoUsuarioAPI()
    {
        return this._contextoUsuario;
    }

}
#pragma warning restore CS8602 // Dereference of a possibly null reference.