using aprendizaje.model.flashcard;
using comunes.primitivas;
using FluentStorage.Blobs;
using FluentStorage;
using Microsoft.Extensions.Configuration;

namespace aprendizaje.services.almacenamientoNeurona;

public class ServicioAlmacenamientoNeuronaGCPBucket : IServicioAlmacenamientoNeurona
{
    private IBlobStorage blobStorage;
    private readonly IConfiguration configuration;
    private string _rutaFlashCard;

    public ServicioAlmacenamientoNeuronaGCPBucket(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<Respuesta> CreaActualizaFlashcard(string NeuronaId, string FlashcardId, FlashCard JsonFlashcard)
    {
        Respuesta respuesta = new Respuesta();
        var existe = await ExisteFlashCard(NeuronaId, FlashcardId);
        if (existe.Ok == true)
        {
            await this.blobStorage.DeleteAsync(this._rutaFlashCard);
            await this.blobStorage.WriteJsonAsync(this._rutaFlashCard, JsonFlashcard);
            respuesta.Ok = true;
            return respuesta;
        }
        await this.blobStorage.WriteJsonAsync(this._rutaFlashCard, JsonFlashcard);
        respuesta.Ok = true;
        return respuesta;
    }

    public async Task<Respuesta> CreaFolderBaseNeurona(string NeuronaId)
    {
        Respuesta respuesta = new();
        ConexionGCPBucket();
        var paths = new string[] { "flashcard", "evaluacion", "contenido" };
        await this.blobStorage.CreateFolderAsync(NeuronaId);
        foreach (var path in paths)
        {
            await this.blobStorage.CreateFolderAsync(NeuronaId+"/"+path);
        }
        respuesta.Ok = true;
        return respuesta;
    }

    public async Task<Respuesta> EliminaFlashcard(string NeuronaId, string FlashcardId)
    {
        Respuesta respuesta = new();
        var existe = await ExisteFlashCard(NeuronaId, FlashcardId);
        if (existe.Ok == false)
        {
            respuesta.Error = new()
            {
                Codigo = CodigosError.APRENDIZAJE_SERVICIO_ALMACENAMIENTO_NEURONA_FLASHCARD_NOENCONTRADO,
                Mensaje = "FlashCard no encontrado, no existe en el Storage",
                HttpCode = HttpCode.NotFound
            };
            respuesta.HttpCode = HttpCode.NotFound;
            return respuesta;
        }
        await this.blobStorage.DeleteAsync(this._rutaFlashCard);
        respuesta.Ok = true;
        return respuesta;
    }

    public async Task<RespuestaPayload<FlashCard>> ObtieneFlashcard(string NeuronaId, string FlashcardId)
    {
        RespuestaPayload<FlashCard> respuesta = new RespuestaPayload<FlashCard>();
        var existe = await ExisteFlashCard(NeuronaId, FlashcardId);
        if (existe.Ok == false)
        {
            respuesta.Error = new()
            {
                Codigo = CodigosError.APRENDIZAJE_SERVICIO_ALMACENAMIENTO_NEURONA_FLASHCARD_NOENCONTRADO,
                Mensaje = "FlashCard no encontrado, no existe en el Storage",
                HttpCode = HttpCode.NotFound
            };
            respuesta.HttpCode = HttpCode.NotFound;
            return respuesta;
        }
        var flashCard = await this.blobStorage.ReadJsonAsync<FlashCard>(this._rutaFlashCard);
        respuesta.Ok = true;
        respuesta.Payload = flashCard;
        return respuesta;

    }

    public async Task<Respuesta> ExisteFlashCard(string NeuronaId, string FlashcardId)
    {
        Respuesta respuesta = new();
        ConexionGCPBucket();
        var _rutaFlashCardJson = NeuronaId + "/" + "flashcard" + "/" + FlashcardId + ".json";
        this._rutaFlashCard = _rutaFlashCardJson;
        var existe = this.blobStorage.ExistsAsync(this._rutaFlashCard).Result;
        respuesta.Ok = existe;
        return respuesta;
    }

    public void ConexionGCPBucket()
    {
        string _bucket = configuration.GetSection("ConfiguracionBucket").GetSection("Bucket").Value;
        var credenciales = configuration.GetValue<string>("ConfiguracionBucket:GoogleCredentials");
        this.blobStorage = StorageFactory.Blobs.GoogleCloudStorageFromJsonFile(_bucket, credenciales);
    }
}
