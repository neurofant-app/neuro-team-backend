using Google.Apis.Auth.OAuth2;
using Google.Cloud.Storage.V1;
using Microsoft.Extensions.Logging;

namespace sat.bot.comun.GoogleCloudPlatform;

/// <summary>
/// OBtiene acceso a los archivos relacionados con la suscripción y el RFC
/// </summary>
public class GestorArchivosGCP : IGestorArchivos
{
    private readonly ILogger logger;
    private readonly ConfiguracionGCP configuracion;
    private readonly StorageClient _storage;
    private readonly string _rutaCache;
    public GestorArchivosGCP(ILogger logger, ConfiguracionGCP configuracion)
    {
        this.logger = logger;
        this.configuracion = configuracion;
        _storage = StorageClient.Create(GoogleCredential.FromFile(Environment.GetEnvironmentVariable("PathGoogleCredentials")));
        _rutaCache = Environment.GetEnvironmentVariable("RutaCache");
    }

    public async Task<bool> AlmacenaDBSqlite(string rfc, string subscripcionId, string ruta, string version)
    {
        logger.LogDebug($"Almacenando DB para {rfc} de suscripción {subscripcionId}");
        var bucketName = configuracion.Bucket;
        var projectId = configuracion.ProyectoId;
        var rutadirectorio = @$"{subscripcionId}/{rfc}/";
        if (!await VerificarRuta(bucketName, rutadirectorio))
        {
            await _storage.UploadObjectAsync(bucketName, rutadirectorio, null, new MemoryStream());
        }
        try
        {
            var _db = _storage.GetObject(bucketName, $"{rutadirectorio}{rfc}-{version}.db");
            await _storage.DeleteObjectAsync(bucketName, $"{rutadirectorio}{rfc}-{version}.db");
            await _storage.UploadObjectAsync(bucketName, $"{rutadirectorio}{rfc}-{version}.db", "text/plain", new MemoryStream(File.ReadAllBytes(ruta)));
            return true;
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            await _storage.UploadObjectAsync(bucketName, $"{rutadirectorio}{rfc}-{version}.db", "text/plain", new MemoryStream(File.ReadAllBytes(ruta)));
            return true;
        }
        catch
        {
            return false;
        }


    }

    public async Task<string?> RutaRWDBSqlite(string rfc, string subscripcionId, string ruta)
    {
        logger.LogDebug($"Leyendo DB para {rfc} de suscripción {subscripcionId}");

        var bucketName = configuracion.Bucket;
        var projectId = configuracion.ProyectoId;
        var rutadirectorio = @$"{subscripcionId}/{rfc}/";
        if (!await VerificarRuta(bucketName, rutadirectorio))
        {
            await _storage.UploadObjectAsync(bucketName, rutadirectorio, null, new MemoryStream());
        }
        try
        {
            var _db = _storage.GetObject(bucketName, $"{rutadirectorio}{rfc}.db");
            using (var stream = File.Create($"{ruta}/{rfc}.db"))
            {
                await _storage.DownloadObjectAsync(bucketName, $"{rutadirectorio}{rfc}.db", stream);
            }
            return ruta;
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        catch
        {
            return null;
        }
    }

    public async Task<bool> AlmacenaXML(string rfc, string subscripcionId, Guid UUID, byte[] xml)
    {
        logger.LogDebug($"Almacenando XML {UUID} para el RFC {rfc}");
        var bucketName = configuracion.Bucket;
        var projectId = configuracion.ProyectoId;
        var rutadirectorio = @$"{subscripcionId}/{rfc}/";
        var pathXML = $"{rutadirectorio}{UUID}.xml";
        if (!await VerificarRuta(bucketName, rutadirectorio))
        {
            await _storage.UploadObjectAsync(bucketName, rutadirectorio, null, new MemoryStream());
        }
        try
        {
            var _db = _storage.GetObject(bucketName, pathXML);
            return true;
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var respuesta = await _storage.UploadObjectAsync(bucketName, pathXML, "text/plain", new MemoryStream(xml));
            if (respuesta != null && !string.IsNullOrEmpty(respuesta.Id))
            {
                return true;
            }
        }
        return false;

    }

    public async Task<byte[]?> LeeXML(string rfc, string subscripcionId, Guid UUID)
    {
        logger.LogDebug($"Leyendo XML {UUID} para el RFC {rfc}");
        var bucketName = configuracion.Bucket;
        var projectId = configuracion.ProyectoId;
        var rutadirectorio = @$"{subscripcionId}/{rfc}/";
        var pathXML = $"{rutadirectorio}{UUID}.xml";
        if (!await VerificarRuta(bucketName, rutadirectorio))
        {
            await _storage.UploadObjectAsync(bucketName, rutadirectorio, null, new MemoryStream());
        }
        try
        {
            var _db = _storage.GetObject(bucketName, pathXML);
            using (var stream = File.Create(Path.Combine(_rutaCache, $"{UUID}.xml")))
            {
                await _storage.DownloadObjectAsync(bucketName, pathXML, stream);
            }
            var xml = File.ReadAllBytes(Path.Combine(_rutaCache, $"{UUID}.xml"));
            if (xml != null)
            {
                return xml;
            }
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        return null;

    }


    public async Task<bool> AlmacenaPDF(string rfc, string subscripcionId, Guid UUID, byte[] pdf)
    {
        logger.LogDebug($"Almacenando PDF {UUID} para el RFC {rfc}");
        var bucketName = configuracion.Bucket;
        var projectId = configuracion.ProyectoId;
        var rutadirectorio = @$"{subscripcionId}/{rfc}/";
        var pathXML = $"{rutadirectorio}{UUID}.pdf";
        if (!await VerificarRuta(bucketName, rutadirectorio))
        {
            await _storage.UploadObjectAsync(bucketName, rutadirectorio, null, new MemoryStream());
        }
        try
        {
            var _db = _storage.GetObject(bucketName, pathXML);
            return true;
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            var respuesta = await _storage.UploadObjectAsync(bucketName, pathXML, "text/plain", new MemoryStream(pdf));
            if (respuesta != null && !string.IsNullOrEmpty(respuesta.Id))
            {
                return true;
            }
        }
        return false;


    }

    public async Task<byte[]?> LeePDF(string rfc, string subscripcionId, Guid UUID)
    {
        var bucketName = configuracion.Bucket;
        var projectId = configuracion.ProyectoId;
        var rutadirectorio = @$"{subscripcionId}/{rfc}/";
        var pathXML = $"{rutadirectorio}{UUID}.pdf";
        if (!await VerificarRuta(bucketName, rutadirectorio))
        {
            await _storage.UploadObjectAsync(bucketName, rutadirectorio, null, new MemoryStream());
        }
        try
        {
            var _db = _storage.GetObject(bucketName, pathXML);
            using (var stream = File.Create(Path.Combine(_rutaCache, $"{UUID}.pdf")))
            {
                await _storage.DownloadObjectAsync(bucketName, pathXML, stream);
            }
            var xml = File.ReadAllBytes(Path.Combine(_rutaCache, $"{UUID}.pdf"));
            if (xml != null)
            {
                return xml;
            }
        }
        catch (Google.GoogleApiException ex) when (ex.HttpStatusCode == System.Net.HttpStatusCode.NotFound)
        {
            return null;
        }
        return null;
    }

    private async Task<bool> VerificarRuta(string bucketName, string ruta)
    {
        var options = new ListObjectsOptions { Delimiter = "/" };
        var storageObjects = _storage.ListObjects(bucketName, $"{ruta}", options);
        if (storageObjects.Any())
        {
            return true;
        }
        return false;
    }
}
