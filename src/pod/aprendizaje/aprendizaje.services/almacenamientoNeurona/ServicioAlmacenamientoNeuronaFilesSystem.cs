using aprendizaje.model.flashcard;
using comunes.primitivas;
using FluentStorage;
using FluentStorage.Blobs;
using Microsoft.Extensions.Configuration;

namespace aprendizaje.services.almacenamientoNeurona;

public class ServicioAlmacenamientoNeuronaFilesSystem : IServicioAlmacenamientoNeurona
{
    private IBlobStorage blobStorage;
    private readonly IConfiguration configuration;
    private string _rutaFlashCard;

    public ServicioAlmacenamientoNeuronaFilesSystem(IConfiguration configuration)
    {
        this.configuration = configuration;
    }

    public async Task<Respuesta> CreaActualizaFlashcard(string NeuronaId, string FlashcardId, FlashCard JsonFlashcard)
    {
        Respuesta respuesta = new Respuesta();
        var existe = await ExisteFlashCard(NeuronaId, FlashcardId);
        if(existe.Ok == true)
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
        string _settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value;
        var _rutaNeurona = Path.Combine(_settingsPath, NeuronaId);
        var paths = new string[] { "flashcard", "evaluacion", "contenido"};
        this.blobStorage = StorageFactory.Blobs.DirectoryFiles(_rutaNeurona);
        foreach( var path in paths )
        {
            await this.blobStorage.CreateFolderAsync(Path.Combine(_rutaNeurona, path));
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
        RespuestaPayload<FlashCard> respuesta= new RespuestaPayload<FlashCard>();
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
        var _rutaNeurona = RutaFlashCard(NeuronaId);
        ConexionDirectorioFlashCard(_rutaNeurona);
        var _Flash = Path.Combine(_rutaNeurona, FlashcardId + ".json");
        this._rutaFlashCard = _Flash;
        var existe = this.blobStorage.ExistsAsync(this._rutaFlashCard).Result;
        respuesta.Ok = existe;
        return respuesta;
    }

    public string RutaFlashCard(string NeuronaId)
    {
        var _settingsPath = configuration.GetSection("FluentStorageDesarrollo").GetSection("rutaBase").Value;
        var _rutaNeurona = Path.Combine(_settingsPath, NeuronaId);
        var _pathFlashCard = Path.Combine(_rutaNeurona, "flashcard");
        return _pathFlashCard;
    }

    public void ConexionDirectorioFlashCard(string rutaFlashCard)
    {
        this.blobStorage = StorageFactory.Blobs.DirectoryFiles(rutaFlashCard);
    }
}
