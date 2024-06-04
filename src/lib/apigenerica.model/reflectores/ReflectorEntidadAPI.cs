using extensibilidad.metadatos;
using extensibilidad.metadatos.atributos;
using extensibilidad.metadatos.configuraciones;
using System.Reflection;


namespace apigenerica.model.reflectores;

/// <summary>
/// REfelcto para entidades de API genérica
/// </summary>
public class ReflectorEntidadAPI : IReflectorEntidadesAPI
{

    public Entidad ObtieneEntidad(Type Tipo)
    {
        Entidad entidad = new()
        {
            Nombre = Tipo.Name.ToString(),
            Id = Guid.NewGuid().ToString()
        };

        foreach (var propertyInfo in Tipo.GetProperties())
        {
            Propiedad? propiedad = propertyInfo.ObtieneMetadatos();
            if (propiedad != null)
            {
                entidad.Propiedades.Add(propiedad);
            }
        }
        return entidad;
    }

    public Entidad ObtieneEntidadUI(Type dtoFull, Type dtoInsertar, Type dtoActualizar, Type dtoDespliegue)
    {
        Entidad entidad = ObtieneEntidad(dtoFull);

        List<string> propiedadesInsertar = dtoInsertar.GetProperties().ToList().Select(p => p.Name).ToList();
        List<string> propiedadesActualizar = dtoActualizar.GetProperties().ToList().Select(p => p.Name).ToList();
        List<string> propiedadesDesplegar = dtoDespliegue.GetProperties().ToList().Select(p => p.Name).ToList();

        foreach (var p in entidad.Propiedades)
        {
            p.HabilitadoCrear = propiedadesInsertar.Contains(p.Nombre, StringComparer.InvariantCultureIgnoreCase);
            p.HabilitadoEditar = propiedadesActualizar.Contains(p.Nombre, StringComparer.InvariantCultureIgnoreCase);
            p.HabilitadoDespliegue = propiedadesDesplegar.Contains(p.Nombre, StringComparer.InvariantCultureIgnoreCase);
        }

        return entidad;
    }


}