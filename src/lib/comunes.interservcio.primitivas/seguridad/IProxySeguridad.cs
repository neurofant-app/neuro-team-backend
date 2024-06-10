using comunes.primitivas.seguridad;

namespace comunes.interservicio.primitivas;


public interface IProxySeguridad
{
    public Task ActualizaSeguridad(List<Aplicacion> apps);
    
}
