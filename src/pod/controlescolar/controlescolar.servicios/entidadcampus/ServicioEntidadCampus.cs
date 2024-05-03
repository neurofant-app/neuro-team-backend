using comunes.primitivas.configuracion.mongo;
using controlescolar.modelo.campi;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using static OpenIddict.Abstractions.OpenIddictConstants;


namespace controlescolar.servicios.entidadcampus;

public class ServicioEntidadCampus
{
    private readonly IMongoCollection<EntidadCampus> _campus;
    private readonly ILogger _logger;
    public ServicioEntidadCampus(ILogger<ServicioEntidadCampus> logger, IServicionConfiguracionMongo configuracionMongo) {

        _logger = logger;
        var conexion = configuracionMongo.ConexionEntidad("campus");
        if (conexion == null )
        {
            string err = "No existe configuracion de mongo para 'campus'";
            _logger.LogError(err);
            throw new Exception(err);
        }

        // Este fragemnto debe ir simrpe al inicio para evitar conflicts de mongo con cambios en el modelo
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);


        try
        {
            _logger.LogDebug($"Mongo DB {conexion.Esquema} coleccioón {conexion.Esquema} utilziando conexion default {string.IsNullOrEmpty(conexion.Conexion)}");
            var client = new MongoClient(conexion.Conexion ?? configuracionMongo.ConexionDefault());
            var database = client.GetDatabase(conexion.Esquema);
            _campus = database.GetCollection<EntidadCampus>(conexion.Coleccion);
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error al inicializar mongo para 'campus'");
            throw;
        }        


    }
}
