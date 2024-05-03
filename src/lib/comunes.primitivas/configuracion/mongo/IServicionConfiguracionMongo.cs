namespace comunes.primitivas.configuracion.mongo;

public interface IServicionConfiguracionMongo
{
    string? ConexionDefault();
    ConexionEntidad? ConexionEntidad(string entidad);
}
