using comunes.primitivas.seguridad;

namespace aplicaciones.api.seguridad;

public interface IProxySeguridad
{
    public Task ActualizaSeguridad(List<Aplicacion> apps);
    
}
