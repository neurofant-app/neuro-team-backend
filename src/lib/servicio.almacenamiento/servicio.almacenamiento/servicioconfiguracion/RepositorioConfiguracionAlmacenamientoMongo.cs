using Microsoft.Extensions.Options;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using servicio.almacenamiento.configuraciones;
using System.Diagnostics.CodeAnalysis;

namespace servicio.almacenamiento.servicioconfiguracion;

/// <summary>
/// Servicio de configuración de almacenamiento basado en mongo
/// </summary>
[ExcludeFromCodeCoverage]
public class RepositorioConfiguracionAlmacenamientoMongo : IRepositorioConfiguracionAlmacenamiento
{

    private readonly IMongoCollection<ConfiguracionProveedor> _configuraciones;
    
    /// <summary>
    /// 
    /// </summary>
    /// <param name="options"></param>
    public RepositorioConfiguracionAlmacenamientoMongo(IOptions<ConfiguracionRepositorioProveedorAlmacenamiento> options) {
        
        var pack = new ConventionPack
            {
                new IgnoreExtraElementsConvention(true)
            };
        ConventionRegistry.Register("Conventions", pack, t => true);


        var client = new MongoClient(options.Value.CadenaConexion);
        var database = client.GetDatabase(options.Value.NombreDb);
        _configuraciones = database.GetCollection<ConfiguracionProveedor>(options.Value.Coleccion);
    }

#pragma warning disable CS8600 // Converting null literal or possible null value to non-nullable type.
    public async Task<ConfiguracionProveedor?> ObtieneConfiguracion(string servicio, string? servicioId)
    {
        ConfiguracionProveedor config = null;
        var results = await _configuraciones.FindAsync(c=> c.Servicio == servicio && c.Activa == true);
        if(results.Any())
        {

            config = results.ToList().FirstOrDefault(c => c.Servicio == servicio && c.ServicioId == servicioId);
        }
        return config;
    }
#pragma warning restore CS8600 // Converting null literal or possible null value to non-nullable type.
}
